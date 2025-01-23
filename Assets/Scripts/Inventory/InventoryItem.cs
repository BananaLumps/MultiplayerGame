using Base.ModularUI.DataBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.Inventory
{
    [DataBind("Player/Inventory/Item", DataBindType.Source, "InventoryItem")]
    public class InventoryItem
    {
        public ItemObject ItemObject;
        public int Count;
        public bool IsEquipment = false;
        public bool IsConsumable = false;

        public InventoryItem(string ID, int count)
        {
            Count = count;
            if (ID == "Empty") ItemObject = new ItemObject() { ID = "Empty", MaxStack = 1 };
            else
                ItemObject = Core.Items[ID];
        }
        public InventoryItem(InventoryItem item)
        {
            ItemObject = item.ItemObject;
            Count = item.Count;
        }
        public InventoryItem()
        {
        }
        #region Operators
        public static bool operator ==(InventoryItem left, InventoryItem right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(InventoryItem left, InventoryItem right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is InventoryItem otherItem)
                return Equals(otherItem);
            return false;
        }

        public bool Equals(InventoryItem other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (ReferenceEquals(other, null))
                return false;

            if (IsEquipment && other.IsEquipment) return Count == other.Count && ItemObject.ID == other.ItemObject.ID && (this as InventoryEquipment).CurrentDurability == (other as InventoryEquipment).CurrentDurability;
            if (IsConsumable && other.IsConsumable) return Count == other.Count && ItemObject.ID == other.ItemObject.ID && (this as InventoryConsumable).DosesRemaining == (other as InventoryConsumable).DosesRemaining;
            return Count == other.Count && ItemObject.ID == other.ItemObject.ID;
        }

        public override int GetHashCode()
        {
            if (IsEquipment) return HashCode.Combine(Count, ItemObject.ID, (this as InventoryEquipment).CurrentDurability);
            if (IsConsumable) return HashCode.Combine(Count, ItemObject.ID, (this as InventoryConsumable).DosesRemaining);
            return HashCode.Combine(Count, ItemObject.ID);
        }
        #endregion
    }
}
