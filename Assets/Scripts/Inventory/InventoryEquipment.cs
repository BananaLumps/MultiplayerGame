using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Inventory
{
    public class InventoryEquipment : InventoryItem
    {
        public float CurrentDurability;

        public InventoryEquipment(string ID, int count, float durability) : base(ID, count)
        {
            CurrentDurability = durability;
        }
    }
}
