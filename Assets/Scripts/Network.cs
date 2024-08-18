using FishNet.Connection;
using FishNet.Object;

namespace Base
{
    public class Network : NetworkBehaviour
    {
        [ServerRpc]
        public void Login(NetworkConnection nc, string username)
        {
        }
    }
}
