using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Base.ModularUI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        private static UIBuilder _builder;

        public static UIBuilder Builder
        {
            get
            {
                if (_builder == null)
                {
                    _builder = FindObjectOfType<UIBuilder>();
                    if (_builder == null)
                    {
                        _builder = Instance.gameObject.AddComponent<UIBuilder>();
                    }
                }
                return _builder;
            }
        }
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UIManager>();
                    if (_instance == null)
                    {
                        GameObject uiManagerObj = new GameObject("UIManager");
                        _instance = uiManagerObj.AddComponent<UIManager>();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// The GameObject containing the canvas to display on.
        /// </summary>
        [SerializeField]
        GameObject canvas;
        [SerializeField]
        GameObject test;

        Vector2 currentPosition;
        UISnapPoint currentSnapPoint;
        [SerializeField]
        GameObject testPrefab;
        GameObject gridGO;
        public UIGrid Grid;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            gridGO = new GameObject("UI Grid");
            gridGO.transform.SetParent(canvas.transform);
            Grid = gridGO.AddComponent<UIGrid>();
        }

        public void SpawnAtPoint(GameObject prefabToSpawn, UISnapPoint point)
        {
            GameObject temp = Instantiate(prefabToSpawn);
            temp.transform.SetParent(gridGO.transform);
            temp.transform.position = point.transform.position;
        }
        private void Update()
        {
            try
            {
                currentSnapPoint = Grid.Grid[Grid.SnapToGrid(Input.mousePosition)];
                currentPosition = currentSnapPoint.transform.position;
                test.transform.position = currentPosition;
            }
            catch (Exception)
            {
            }
        }
    }
    public enum AnchorPoint
    {
        Center,
        TopLeft,
        BottomLeft,
        BottomRight,
        TopRight
    }
}
