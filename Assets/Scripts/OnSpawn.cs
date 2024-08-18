using FishNet.Object;
using Invector.vCharacterController;
using UnityEngine;

namespace Base
{

    public class OnSpawn : NetworkBehaviour
    {
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
    }
}
