using Base.Assets.Scripts.UI.UIBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base.UI
{
    public class ContextDropdownController : MonoBehaviour
    {
        GameObject ContextMenuObjectPrefab;
        GameObject ObjectContainer;
        Button BackButton;
        public GameObject RootStorageParent;
        private Stack<GameObject> navigationStack = new Stack<GameObject>();
        void Start()
        {
            BackButton = GetComponentInChildren<Button>();
            ContextMenuObjectPrefab = Resources.Load<GameObject>("UIPrefabs/Builder/ContextMenuObjectPrefab");
            targetRectTransform = GetComponent<RectTransform>();
            ObjectContainer = GameObject.Find("ObjectContainerPanel");
            RootStorageParent = new GameObject("RootStorageParent");
            RootStorageParent.transform.SetParent(ObjectContainer.transform.parent);
            BackButton.onClick.AddListener(OnBackButtonClicked);
            PopulateData(new List<string> { "Player/Inventory/Item", "Player/Inventory/Weapon", "Player/Stats/Health", "Player/Stats/Attack", "Player/Stats/Defense", "Target/Inventory/Item", "Target/Stats/Attack", "Target/Stats/HP" });
        }
        public RectTransform targetRectTransform;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (targetRectTransform == null)
            {
                Debug.LogError("Target RectTransform is not assigned.");
                return;
            }

            Vector2 localMousePosition = targetRectTransform.InverseTransformPoint(eventData.position);
            if (targetRectTransform.rect.Contains(localMousePosition))
            {
                Debug.Log("Clicked inside the target bounds.");
            }
            else
            {
                Debug.Log("Clicked outside the target bounds.");
            }
        }
        public void PopulateData(List<string> data)
        {
            Dictionary<string, GameObject> createdObjects = new Dictionary<string, GameObject>();

            foreach (string path in data)
            {
                string[] parts = path.Split('/');
                string currentPath = "";
                Transform parentTransform = RootStorageParent.transform;

                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    currentPath = string.IsNullOrEmpty(currentPath) ? part : currentPath + "/" + part;

                    if (!createdObjects.ContainsKey(currentPath))
                    {
                        GameObject newObj = Instantiate(ContextMenuObjectPrefab, parentTransform);
                        ContextMenuObjectController controller = newObj.GetComponent<ContextMenuObjectController>();
                        controller.SetText(part);
                        controller.Parent = RootStorageParent.transform;
                        createdObjects[currentPath] = newObj;
                        newObj.name = currentPath;

                        if (i > 0)
                        {
                            string parentPath = currentPath.Substring(0, currentPath.LastIndexOf('/'));
                            ContextMenuObjectController parentController = createdObjects[parentPath].GetComponent<ContextMenuObjectController>();
                            parentController.AddChild(newObj);
                            newObj.GetComponent<ContextMenuObjectController>().Parent = createdObjects[parentPath].transform;
                            HideObject(newObj);
                        }
                        newObj.GetComponent<Button>().onClick.AddListener(() => OnContextMenuObjectClicked(newObj));
                    }

                    parentTransform = createdObjects[currentPath].transform;
                }

            }

            // Move root objects to ObjectContainer
            foreach (var obj in createdObjects.Values)
            {
                if (obj.transform.parent == RootStorageParent.transform)
                {
                    obj.transform.SetParent(ObjectContainer.transform);
                }
            }
        }
        private void OnBackButtonClicked()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in ObjectContainer.transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                child.SetParent(child.gameObject.GetComponent<ContextMenuObjectController>().Parent);
                HideObject(child.gameObject);
            }


            if (navigationStack.Count <= 1)
            {
                List<Transform> childrenT = new List<Transform>();
                foreach (Transform baseChild in RootStorageParent.transform)
                {
                    childrenT.Add(baseChild);
                }
                foreach (Transform child in childrenT)
                {
                    child.SetParent(ObjectContainer.transform);
                    ShowObject(child.gameObject);
                }
                navigationStack.Pop();
            }
            else if (navigationStack.Count > 1)
            {
                GameObject lastContextMenuObject = navigationStack.Pop();

                foreach (GameObject child in lastContextMenuObject.transform.parent.GetComponent<ContextMenuObjectController>().children)
                {
                    child.transform.SetParent(ObjectContainer.transform);
                    ShowObject(child);
                }
            }
        }
        private void OnContextMenuObjectClicked(GameObject contextMenuObject)
        {
            ContextMenuObjectController controller = contextMenuObject.GetComponent<ContextMenuObjectController>();
            if (controller.HasChildren)
            {
                List<Transform> children = new List<Transform>();
                foreach (Transform child in ObjectContainer.transform)
                {
                    children.Add(child);
                }

                foreach (Transform child in children)
                {
                    child.SetParent(child.GetComponent<ContextMenuObjectController>().Parent);
                    HideObject(child.gameObject);
                }
                foreach (GameObject child in controller.children)
                {
                    child.transform.SetParent(ObjectContainer.transform);
                    ShowObject(child);
                }
                navigationStack.Push(contextMenuObject);
                HideObject(contextMenuObject);
            }
        }
        private void HideObject(GameObject gameObject)
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
        private void ShowObject(GameObject gameObject)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}