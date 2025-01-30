using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.ModularUI
{
    public class UIBuilder : MonoBehaviour
    {
        [SerializeField]
        public List<GameObject> prefabGameObjects;
        [SerializeField]
        UIManager manager;
        [SerializeField]
        GameObject currentlySelectedPrefab;
        bool isPlacing = false;
        bool isMoving = false;
        bool IsPlacing
        {
            get
            {
                return isPlacing;
            }
            set
            {
                Destroy(ghostObject);
                ghostObject = null;
                isPlacing = value;
            }
        }
        bool IsMoving
        {
            get
            {
                return isMoving;
            }
            set
            {
                Destroy(ghostObject);
                ghostObject = null;
                isMoving = value;
            }
        }
        public Button TestButton;
        GameObject ghostObject;
        float alphaValue = .25f;
        GameObject objectToMove;
        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) { CancelMovingObject(); }
            if (IsPlacing && currentlySelectedPrefab != null)
            {
                if (ghostObject == null) GetGhostInstance(currentlySelectedPrefab);
                ghostObject.transform.position = manager.Grid.Grid[manager.Grid.SnapToGrid(Input.mousePosition)].transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    manager.SpawnAtPoint(currentlySelectedPrefab, manager.Grid.Grid[manager.Grid.SnapToGrid(Input.mousePosition)]);
                    IsPlacing = false;
                }
            }
            if (IsMoving && objectToMove != null)
            {
                if (ghostObject == null) ghostObject = GetGhostInstance(objectToMove);
                ghostObject.transform.position = manager.Grid.Grid[manager.Grid.SnapToGrid(Input.mousePosition)].transform.position;
            }
        }
        private void Awake()
        {
            TestButton.onClick.AddListener(ButtonClick);
        }
        private void ButtonClick()
        {
            CreateContextMenuAtLocation(TestButton.transform.position);
        }
        public GameObject GetGhostInstance(GameObject go)
        {
            GameObject temp = Instantiate(go);
            temp.transform.SetParent(go.transform.parent);
            SetAlphaForGameObject(temp, alphaValue);
            return temp;
        }
        public void ChangeSelectedPrefab(GameObject prefab)
        {
            currentlySelectedPrefab = prefab;
        }
        public void BeginMovingObject(GameObject obj)
        {
            objectToMove = obj;
            IsMoving = true;
        }
        public void FinishMovingObject()
        {
            objectToMove.transform.position = ghostObject.transform.position;
            IsMoving = false;
            objectToMove = null;
        }
        public void CancelMovingObject()
        {
            objectToMove.GetComponent<UIObjectController>().CancelMove();
            IsMoving = false;
            objectToMove = null;
        }
        public void SetAlphaForGameObject(GameObject targetObject, float alphaValue)
        {
            Image[] images = targetObject.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = alphaValue;
                image.color = color;
            }
        }
        public void CreateContextMenuAtLocation(Vector3 location)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("UIPrefabs/Builder/ContextDropdownPanelPrefab"));
            temp.transform.parent = GameObject.Find("Canvas").transform;
            temp.transform.position = new Vector3(location.x, location.y + ((temp.GetComponent<RectTransform>().rect.height / 2) + (TestButton.GetComponent<RectTransform>().rect.height / 2)));
            temp.GetComponent<RectTransform>().sizeDelta = new Vector2(TestButton.GetComponent<RectTransform>().sizeDelta.x, temp.GetComponent<RectTransform>().sizeDelta.y);
        }
        public void CreatePanel()
        {
            // Load the panel prefab from Resources/Prefabs
            GameObject panelPrefab = Resources.Load<GameObject>("Prefabs/PanelPrefab");
            if (panelPrefab == null)
            {
                Debug.LogError("PanelPrefab not found in Resources/Prefabs");
                return;
            }

            // Instantiate the panel prefab
            GameObject panelInstance = Instantiate(panelPrefab);

            // Set the parent of the instantiated panel to the canvas
            panelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

            // Position the panel in the center of the canvas
            RectTransform rectTransform = panelInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
        }
        public enum BoundsPosition
        {
            Center,
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }
        public Vector3 GetBoundsPosition(Bounds bounds, BoundsPosition position)
        {
            switch (position)
            {
                case BoundsPosition.Center:
                    return bounds.center;
                case BoundsPosition.BottomLeft:
                    return new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
                case BoundsPosition.BottomRight:
                    return new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
                case BoundsPosition.TopLeft:
                    return new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
                case BoundsPosition.TopRight:
                    return new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
                default:
                    return Vector3.zero;
            }
        }
    }
}

