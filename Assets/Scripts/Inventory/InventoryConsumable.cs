using Base.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Inventory
{
    public class InventoryConsumable : InventoryItem
    {
        public int DosesRemaining;

        public InventoryConsumable(string ID, int count, int doses) : base(ID, count)
        {
            DosesRemaining = doses;
        }
    }
}
