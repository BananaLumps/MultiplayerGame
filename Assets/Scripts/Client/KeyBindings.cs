using UnityEngine;

namespace Base.Client
{
    public static class KeyBindings
    {
        public static KeyCode[] AllBinds = { Inventory, Debug };
        public const KeyCode Inventory = KeyCode.I;
        public const KeyCode Debug = KeyCode.Keypad0;

    }
}
