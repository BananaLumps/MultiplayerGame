using Base.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Inventory
{
    public class EquipmentObject : ItemObject
    {
        public float Power;
        public float Speed;
        public float Defense;
        public float Accuracy;
        public int EquipmentSlot;
        public Dictionary<string, int> SkillRequirements;
    }
}
