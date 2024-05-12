using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Base.Player
{
    public class Player : NetworkBehaviour
    {
        public readonly SyncVar<string> UserID = new();
        // public readonly SyncList<string> UserNames = new();


        private void Awake()
        {
            UserID.val
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
