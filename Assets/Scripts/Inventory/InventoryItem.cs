using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.Inventory
{

    public class InventoryItem
    {
        public ItemObject ItemObject;
        public int Count;

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
    }
}
