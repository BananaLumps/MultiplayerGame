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
    /// <summary>
    /// Controls the behavior of a UI object in the scene.
    /// </summary>
    public class UIObjectController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {

        /// <summary>
        /// Determines if the object can be dragged.
        /// </summary>
        [SerializeField]
        public bool IsDraggable = false;
        /// <summary>
        /// True if the object is being dragged.
        /// </summary>
        bool isMoving = false;
        bool releaseDrag = false;
        public string displayText;
        public DataType dataType = DataType.None;
        private void Awake()
        {
            GetComponent<Image>().raycastTarget = IsDraggable;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (releaseDrag) eventData.pointerDrag = null;
            if (IsDraggable && !isMoving)
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
        public void CancelMove()
        {
            isMoving = false;
            releaseDrag = true;

        }
        public UIObjectController[] GetChildUIObjects()
        {
            return GetComponentsInChildren<UIObjectController>();
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