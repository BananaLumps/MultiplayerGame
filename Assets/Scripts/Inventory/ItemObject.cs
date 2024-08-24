using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.Inventory
{
    public class ItemObject : ScriptableObject
    {
        public string ID;
        public string Name;
        public string Description;
        public int ItemType;
        public float MaxDurability;
    }
}
