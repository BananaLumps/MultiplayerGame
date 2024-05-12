using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Base.Player
{
    public class Player : NetworkBehaviour
    {
        public readonly SyncVar<string> UserID = new();

        private void Awake()
        {

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
