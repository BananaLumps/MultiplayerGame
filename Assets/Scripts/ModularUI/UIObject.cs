using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base.ModularUI
{
    public class UIObject : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        public bool draggable = false;
        bool isMoving = false;
        public string displayName;
        public DataType dataType = DataType.None;
        private void Awake()
        {
            GetComponent<Image>().raycastTarget = draggable;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (draggable && !isMoving)
            {
                UIManager.Builder.BeginMovingObject(gameObject);
                isMoving = true;
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (isMoving)
            {
                UIManager.Builder.FinishMovingObject();
                isMoving = false;
            }
        }
        public UIObject[] GetChildUIObjects()
        {
            return GetComponentsInChildren<UIObject>();
        }
        public GameObject[] GetChildUIObjectGOs()
        {
            GameObject[] list = null;
            foreach (var obj in GetChildUIObjects())
            {
                list.Append(obj.gameObject);
            }
            return list;
        }
    }
}