
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.ModularUI
{
    public class UIGroup : MonoBehaviour
    {
        Bounds bounds;
        bool draggable = false;
        List<UIObjectController> uiObjects;
        public List<UIObjectController> UIObjects
        {
            get
            {
                return uiObjects;
            }
        }

        public void AddUIObject(UIObjectController obj)
        {
            uiObjects.Add(obj);
        }
        public void RemoveUIObject(UIObjectController obj)
        {
            uiObjects.Remove(obj);
        }
        public void SetDraggable(bool b)
        {
            draggable = b;
        }
    }
}
