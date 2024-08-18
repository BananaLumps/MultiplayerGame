using System.Collections.Generic;
using UnityEngine;

namespace Base.Actions
{
    interface IAction
    {
        public ActionType Type();
        public float Time();
        public Dictionary<string, int> Consumables();
        public ILootTable LootTable();
        public Vector3 TargetLocation();
        public Vector3 TargetObject();
        public Vector3 StartLocation();
        public Dictionary<string, int> SkillRequirements();
        public Dictionary<string, int> ToolRequirements();
        public bool InterruptedByMovement();
        public bool InterruptedByCombat();
        public float SuccessChance();
        public int DamageOnFailure();
        public object FailureDamageTarget();
        public Animation Animation();
        public ParticleSystem ParticleSystem();
    }
}
