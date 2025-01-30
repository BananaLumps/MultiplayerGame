using Base.Client;
using Base.ModularUI;
using Base.ModularUI.DataBinding;
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
    public class InventoryController : NetworkBehaviour
    {
        /// <summary>
        /// The ID of the owner player or set to "Server" if it is not a player inventory
        /// </summary>
        private string userID;
        public string UserID
        {
            get
            {
                return userID;
            }
            private set
            {
                if (value is null) userID = "Server";
                else userID = value;
            }
        }
        /// <summary>
        /// How many total slots the inventory has.
        /// </summary>
        public int size { get; private set; } = 16;
        /// <summary>
        /// Dictionary of all inventory items.
        /// <Key>Slot Number</Key>
        /// <Value>InventoryItem</Value>
        /// </summary>
        public Dictionary<int, InventoryItem> Items
        {
            get; private set;
        }
        /// <summary>
        /// Invoked when the inventory changes.
        /// </summary>
        public static EventHandler<InventoryEventArgs> InventoryEvent;
        /// <summary>
        /// Add an item to inventory by ID. Amount defaults to one.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="amount"></param>
        public void AddItem(string itemID, int amount = 1)
        {
            AddItem(new InventoryItem(itemID, amount));
        }
        /// <summary>
        /// Add an equipment item to inventory by ID. Amount defaults to 1, Durability defaults to 0.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="amount"></param>
        /// <param name="durability"></param>
        public void AddEquipment(string itemID, float durability = 0, int amount = 1)
        {
            AddItem(new InventoryEquipment(itemID, amount, durability));
        }
        /// <summary>
        /// Add a consumable item to inventory by ID. Amount and doses default to 1.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="doses"></param>
        /// <param name="amount"></param>
        /// <param name=""></param>
        public void AddConsumable(string itemID, int doses = 1, int amount = 1)
        {
            AddItem(new InventoryConsumable(itemID, amount, doses));
        }
        /// <summary>
        /// Add existing InventoryItem to inventory.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(InventoryItem item)
        {
            int fullAmount = item.Count;
            if (item.Count <= 0) return;
            int stackSize = item.ItemObject.MaxStack;
            foreach (var i in Items)
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
            foreach (var i in Items)
            {
                if (i.Value.ItemObject.ID == "Empty")
                {
                    if (item.Count <= stackSize)
                    {
                        Items[i.Key] = item; break;
                    }
                    else
                    {
                        Items[i.Key] = new InventoryItem(item.ItemObject.ID, stackSize);
                        item.Count -= stackSize;
                        continue;
                    }
                }

            }
            InventoryEvent?.Invoke(this, new InventoryEventArgs(UserID, item.ItemObject.ID, item.Count, InventoryEvents.Added));
        }
        /// <summary>
        /// Remove existing InventoryItem from the inventory.
        /// </summary>
        /// <param name="item"></param>
        public bool RemoveItem(InventoryItem item)
        {
            if (item.Count <= 0) return false;
            int maxStackSize = item.ItemObject.MaxStack;
            int itemCount = AvailableItemSpace(item.ItemObject.ID);
            int amountToRemove = item.Count;
            if (itemCount < amountToRemove) return false;
            foreach (var i in Items)
            {
                if (amountToRemove <= 0) break;
                if (i.Value.ItemObject.ID == item.ItemObject.ID && i.Value.Count < amountToRemove)
                {
                    i.Value.Count -= amountToRemove;
                    amountToRemove -= amountToRemove;
                }
            }
            if (amountToRemove > 0)
            {
                foreach (var i in Items)
                {
                    if (i.Value.ItemObject.ID == "Empty")
                    {
                        if (amountToRemove <= maxStackSize)
                        {
                            Items[i.Key] = item; break;
                        }
                        else
                        {
                            Items[i.Key] = new InventoryItem(item.ItemObject.ID, maxStackSize);
                            amountToRemove -= maxStackSize;
                            continue;
                        }
                    }

                }
            }
            InventoryEvent?.Invoke(this, new InventoryEventArgs(UserID, item.ItemObject.ID, item.Count, InventoryEvents.Added));
            return true;
        }
        /// <summary>
        /// Call this after creation to set the UserID of the inventory. Pass string.Empty if it is not a player inventory and it will be set to "Server".
        /// </summary>
        /// <param name="userID"></param>
        public void Init(string userID)
        {
            UserID = userID;
            Items = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < size; i++)
            {
                Items.Add(i, new InventoryItem("Empty", 1));
            }
        }
        /// <summary>
        /// Returns the number of the specified item by ID, currently inside the inventory.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int AvailableItemAmount(string itemID)
        {
            int amount = 0;
            foreach (var i in Items)
            {
                if (i.Value.ItemObject.ID == itemID)
                {
                    amount += i.Value.Count;
                }
            }
            return amount;
        }
        /// <summary>
        /// Returns the number of empty slots.
        /// </summary>
        /// <param name="nc"></param>
        /// <returns></returns>
        public int EmptySlotAmount(NetworkConnection nc = null)
        {
            if (base.Owner != nc) return -1;
            int amount = 0;
            foreach (var i in Items)
            {
                if (i.Value.ItemObject.ID == "Empty") amount++;
            }
            return amount;
        }
        /// <summary>
        /// Returns how much of the specified item can fit inside the inventory. Takes into account item stack and empty slots.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public int AvailableItemSpace(string itemID)
        {
            int space = 0;
            foreach (var i in Items)
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
            Items.Clear();
        }
    }
    /// <summary>
    /// Returns the player, the item and the amount processed. EventType specifies if it was added or removed.
    /// </summary>
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
    /// <summary>
    /// Tracks if item in the event was added or removed.
    /// </summary>
    public enum InventoryEvents : int
    {
        None = 0,
        Added = 1,
        Removed = 2,
    }
}
