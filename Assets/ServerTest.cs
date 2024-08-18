using FishNet;
using System.Collections;
using UnityEngine;

namespace Base
{
    public class ServerTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        IEnumerator Test()
        {
            while (true)
            {
                Debug.LogError(InstanceFinder.IsServerStarted);
                yield return new WaitForSeconds(1);
            }
        }
        public class RandomEventSimulator : MonoBehaviour
        {
            // Simulate an event with specified odds of success (0.0 to 1.0)
            public bool SimulateEvent(float oddsOfSuccess)
            {
                if (oddsOfSuccess < 0.0f || oddsOfSuccess > 1.0f)
                {
                    Debug.LogError("Odds of success must be between 0.0 and 1.0.");
                    return false;
                }

                float randomValue = Random.value; // Generate a random value (0.0 to 1.0)
                return randomValue <= oddsOfSuccess;
            }
        }
    }
}
