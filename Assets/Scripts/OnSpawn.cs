using FishNet.Object;
using Invector.vCharacterController;
using UnityEngine;

namespace Base
{

    public class OnSpawn : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
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
