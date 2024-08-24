using Base.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Base
{
    public class DebugTesting : MonoBehaviour
    {
        private void Start()
        {
            UserInput.DebugTestKeyPressed += UserInput_DebugTestKeyPressed;
        }

        private void UserInput_DebugTestKeyPressed()
        {
            Core.Instance.LocalPlayer.Inventory.AddItem("item:stone", 1);
        }
    }
}
