using Base.Client;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    public class Inventory : NetworkBehaviour
    {
        [SerializeField]
        GameObject UIObject;
        string userID;
        int size = 16;
        readonly SyncDictionary<int, Item> _items = new();
        Dictionary<int, int> itemLocation = new();
        public SyncDictionary<int, Item> Items => _items;
        public Dictionary<int, int> ItemLocation => itemLocation;
        public static EventHandler<InventoryEventArgs> ItemAdded;

        public int GetStackSize(Item item)
        {
            //TODO
            //Check the stack size based on type and skill levels
            return 1;
        }
        [ServerRpc]
        public void AddItem(Item item, NetworkConnection nc = null)
        {
            int fullAmount = item.count;
            if (base.Owner != nc || item.count <= 0) return;
            int stackSize = GetStackSize(item);
            foreach (var i in _items)
            {
                if (item.count <= 0) break;
                if (i.Value.id == item.id && i.Value.count < stackSize)
                {
                    int amountToRemove = stackSize - i.Value.count;
                    if (amountToRemove > item.count) amountToRemove = item.count;
                    i.Value.count += amountToRemove;
                    item.count -= amountToRemove;
                }
            }
            if (item.count <= 0) return;
            foreach (var i in _items)
            {
                if (i.Value.id == "Empty")
                {
                    if (item.count <= stackSize)
                    {
                        _items[i.Key] = item; break;
                    }
                    else
                    {
                        _items[i.Key] = new Item(item.id, stackSize);
                        item.count -= stackSize;
                        continue;
                    }
                }

            }
            ItemAdded?.Invoke(this, new InventoryEventArgs(userID, item.id, item.count, InventoryEvents.Added));
        }
        [ServerRpc]
        public void RemoveItem(Item item, NetworkConnection nc = null)
        {
            if (base.Owner != nc || item.count <= 0) return;
            foreach (var i in _items)
            {
                if (i.Value.id != item.id) continue;
                if (item.count < i.Value.count)
                {
                    i.Value.count -= item.count;
                    break;
                }
                if (item.count == i.Value.count)
                {
                    _items[i.Key] = new Item("Empty");
                    break;
                }
                if (item.count > GetStackSize(item))
                {
                    _items[i.Key] = new Item("Empty");
                    item.count -= GetStackSize(item);
                }

            }
        }
        private void Start()
        {
            userID = Core.Instance.LocalPlayer.UserID.Value;
            UserInput.InventoryKeyPressed += UserInput_InventoryKeyPressed;
            UIObject = GameObject.Find("InventoryPanel");
        }
        public override void OnStartClient()
        {

        }
        private void UserInput_InventoryKeyPressed()
        {
            UIObject.SetActive(!UIObject.activeInHierarchy);
        }

        public int AvailableItemAmount(Item item, NetworkConnection nc = null)
        {
            if (base.Owner != nc) return -1;
            int amount = 0;
            foreach (var i in _items)
            {
                if (i.Value.id == item.id)
                {
                    amount += i.Value.count;
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
                if (i.Value.id == "Empty") amount++;
            }
            return amount;
        }

        public int AvailableItemSpace(Item item, NetworkConnection nc = null)
        {
            if (base.Owner != nc) return -1;
            int space = 0;
            foreach (var i in _items)
            {
                if (i.Value.id == item.id)
                {
                    space += i.Value.count;
                }
            }
            return space += EmptySlotAmount() * GetStackSize(item);
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
