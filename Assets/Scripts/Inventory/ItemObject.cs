using Base.ModularUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.Inventory
{
    [CreateAssetMenu(menuName = "Inventory Items/Item")]
    public class ItemObject : ScriptableObject
    {
        [UIDataBind("ID", "ItemBase")]
        public string ID;
        [UIDataBind("Name", "ItemBase")]
        public string Name;
        public string Description;
        public int ItemType;
        public float MaxDurability;
        public int MaxStack;
    }
}
