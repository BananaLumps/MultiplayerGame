using UnityEngine;

namespace Base.Client
{
    public class UserInput : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }
        public static event System.Action InventoryKeyPressed;
        public static event System.Action DebugTestKeyPressed;

        // Update is called once per frame
        void Update()
        {
            foreach (KeyCode key in KeyBindings.AllBinds)
            {
                if (Input.GetKeyDown(key))
                {
                    switch (key)
                    {
                        case KeyBindings.Inventory:
                            InventoryKeyPressed?.Invoke();
                            break;
                        case KeyBindings.Debug:
                            DebugTestKeyPressed?.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
