
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
        List<UIObject> uiObjects;
        public List<UIObject> UIObjects
        {
            get
            {
                return uiObjects;
            }
        }

        public void AddUIObject(UIObject obj)
        {
            uiObjects.Add(obj);
        }
        public void RemoveUIObject(UIObject obj)
        {
            uiObjects.Remove(obj);
        }
        public void SetDraggable(bool b)
        {
            draggable = b;
        }
    }
}
