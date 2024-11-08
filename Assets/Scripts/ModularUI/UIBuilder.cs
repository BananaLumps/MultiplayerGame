using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

        GameObject ghostObject;
        float alphaValue = .25f;
        GameObject objectToMove;
        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) IsPlacing = !IsPlacing;
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
    }
}

