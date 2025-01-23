using Base.ModularUI;
using Base.ModularUI.DataBinding;
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
        [DataBind("Player/Inventory/Item/ID", DataBindType.Text, "ItemID")]
        public string ID;
        [DataBind("Player/Inventory/Item/Name", DataBindType.Text, "ItemName")]
        public string Name;
        public string Description;
        public int ItemType;
        public float MaxDurability;
        public int MaxStack;
    }
}
