using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Base.Assets.Scripts.UI.UIBuilder
{
    public class ContextMenuObjectController : MonoBehaviour
    {
        TMPro.TextMeshProUGUI text;
        public List<GameObject> children;
        [SerializeField]
        public Transform Parent
        {
            get; set;
        }
        public bool HasChildren
        {
            get
            {
                return children.Count > 0;
            }
        }
        public void SetText(string item)
        {
            if (text == null) text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            text.text = item;
        }
        public string GetText()
        {
            return GetComponent<TMPro.TextMeshProUGUI>().text;
        }
        public void AddChild(GameObject child)
        {
            if (children == null) children = new List<GameObject>();
            children.Add(child);
        }
        // Use this for initialization
        void Start()
        {
        }
    }
}