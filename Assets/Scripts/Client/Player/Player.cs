using Base.Inventory;
using Base.Skills;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Invector.vCharacterController;
using UnityEngine;

namespace Base.Client.Player
{
    public class Player : NetworkBehaviour
    {

        public readonly SyncVar<string> PlayerName = new();
        public readonly SyncVar<string> UserID = new();
        public Base.Inventory.InventoryController Inventory;
        public readonly SyncList<SkillBase> SkillList = new();
        public EquipmentHandler Equipment;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!base.IsOwner)
            {
                GetComponent<vThirdPersonInput>().enabled = false;
            }
            else
            {
                Camera.main.GetComponent<vThirdPersonCamera>().target = gameObject.transform;
            }
        }
        private void Awake()
        {
            UserID.Value = "Banana";
            Core.Instance.LocalPlayer = this;
            Inventory = gameObject.AddComponent<Base.Inventory.InventoryController>();
            Equipment = gameObject.AddComponent<EquipmentHandler>();

        }
        private void OnDestroy()
        {
            SkillList.Clear();
        }
    }
}
