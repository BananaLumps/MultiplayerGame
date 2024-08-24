using Base.Client;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using GameKit.Dependencies.Utilities;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Base.Inventory
{
    public class Inventory : NetworkBehaviour
    {
        [SerializeField]
        GameObject UIObject;
        public string UserID;
        int size = 16;
        Dictionary<string, InventoryItem> _items = new();
        Dictionary<int, int> itemLocation = new();
        public Dictionary<string, InventoryItem> Items => _items;
        public Dictionary<int, int> ItemLocation => itemLocation;
        public static EventHandler<InventoryEventArgs> InventoryEvent;

        //TODO: Fix creation of inventory and empty slots.
        public void AddItem(string itemID, int amount)
        {
            AddItem(new InventoryItem(itemID, amount));
        }

        public void AddEquipment(string itemID, int amount, float durability)
        {
            AddItem(new InventoryEquipment(itemID, amount, durability));
        }

        public void AddConsumable(string itemID, int amount, int doses)
        {
            AddItem(new InventoryConsumable(itemID, amount, doses));
        }
        void AddItem(InventoryItem item)
        {
            int fullAmount = item.Count;
            if (item.Count <= 0) return;
            int stackSize = item.ItemObject.MaxStack;
            foreach (var i in _items)
            {
                if (item.Count <= 0) break;
                if (i.Value.ItemObject.ID == item.ItemObject.ID && i.Value.Count < stackSize)
                {
                    int amountToRemove = stackSize - i.Value.Count;
                    if (amountToRemove > item.Count) amountToRemove = item.Count;
                    i.Value.Count += amountToRemove;
                    item.Count -= amountToRemove;
                }
            }
            if (item.Count <= 0) return;
            foreach (var i in _items)
            {
                if (i.Value.ItemObject.ID == "Empty")
                {
                    if (item.Count <= stackSize)
                    {
                        _items[i.Key] = item; break;
                    }
                    else
                    {
                        _items[i.Key] = new InventoryItem(item.ItemObject.ID, stackSize);
                        item.Count -= stackSize;
                        continue;
                    }
                }

            }
            if (_items.Count == 0) _items.Add(item.ItemObject.ID, item);
            InventoryEvent?.Invoke(this, new InventoryEventArgs(UserID, item.ItemObject.ID, item.Count, InventoryEvents.Added));
        }
        void RemoveItem(InventoryItem item)
        {
            if (item.Count <= 0) return;
            foreach (var i in _items)
            {
                if (i.Value.ItemObject.ID != item.ItemObject.ID) continue;
                if (item.Count < i.Value.Count)
                {
                    i.Value.Count -= item.Count;
                    break;
                }
                if (item.Count == i.Value.Count)
                {
                    _items[i.Key] = new InventoryItem("Empty", 1);
                    break;
                }
                if (item.Count > item.ItemObject.MaxStack)
                {
                    _items[i.Key] = new InventoryItem("Empty", 1);
                    item.Count -= item.ItemObject.MaxStack;
                }

            }
        }
        private void Start()
        {
            UserID = Core.Instance.LocalPlayer.UserID.Value;
            UserInput.InventoryKeyPressed += UserInput_InventoryKeyPressed;
            UIObject = GameObject.Find("InventoryPanel");
            for (int i = 0; i < size; i++)
            {
                Items.Add("Empty" + i.ToString(), new InventoryItem("Empty", 1));
            }
        }
        public override void OnStartClient()
        {

        }
        private void UserInput_InventoryKeyPressed()
        {
            UIObject.SetActive(!UIObject.activeInHierarchy);
        }

        public int AvailableItemAmount(string itemID)
        {
            int amount = 0;
            foreach (var i in _items)
            {
                if (i.Value.ItemObject.ID == itemID)
                {
                    amount += i.Value.Count;
                }
            }
            return amount;
        }

        public int EmptySlotAmount(NetworkConnection nc = null)
        {
            if (base.Owner != nc) return -1;
            int amount = 0;
            foreach (var i in _items)
            {
                if (i.Value.ItemObject.ID == "Empty") amount++;
            }
            return amount;
        }

        public int AvailableItemSpace(string itemID)
        {
            int space = 0;
            foreach (var i in _items)
            {
                if (i.Value.ItemObject.ID == itemID)
                {
                    space += Core.Items[itemID].MaxStack - i.Value.Count;
                }
            }
            return space += EmptySlotAmount() * Core.Items[itemID].MaxStack;
        }
        private void OnDestroy()
        {
            // _items.Clear();
            itemLocation.Clear();
            UserInput.InventoryKeyPressed -= UserInput_InventoryKeyPressed;
        }
    }
    public class InventoryEventArgs : EventArgs
    {
        public string UserID;
        public string ItemID;
        public int Amount;
        public InventoryEvents EventType;

        public InventoryEventArgs(string userID, string itemID, int amount, InventoryEvents eventType)
        {
            UserID = userID;
            ItemID = itemID;
            Amount = amount;
            EventType = eventType;
        }
    }
    public enum InventoryEvents : int
    {
        None = 0,
        Added = 1,
        Removed = 2,
    }
}
