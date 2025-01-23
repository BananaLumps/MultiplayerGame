using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base.ModularUI
{
    public class UIGrid : MonoBehaviour
    {
        /// <summary>
        /// Pixels between snap points
        /// </summary>
        public int gridSize = 16;
        /// <summary>
        /// Grid offset from screen edge. Defaults to half of the grid size which is recommended.
        /// </summary>
        public int gridOffset = 8;

        public Dictionary<Vector2, UISnapPoint> Grid
        {
            get; private set;
        }
        private void Awake()
        {
            Grid = new Dictionary<Vector2, UISnapPoint>();
            gridOffset = gridSize / 2;
            CreateGrid();
        }
        private void CreateGrid()
        {
            for (int y = 0; y < Screen.height / gridSize; y++)
            {
                for (int x = 0; x < Screen.width / gridSize; x++)
                {
                    GameObject temp = new($"x:{x} y:{y}");
                    temp.transform.SetParent(transform);
                    temp.transform.position = new Vector3((x * gridSize) + gridOffset, (y * gridSize) + gridOffset, 0);
                    Grid.Add(new Vector2(x, y), temp.AddComponent<UISnapPoint>());
                }
            }
        }
        /// <summary>
        /// Returns the snap position closest to the provided position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 SnapToGrid(Vector2 position)
        {
            var snapPoint = new Vector2(Mathf.Floor(position.x / gridSize), Mathf.Floor(position.y / gridSize));
            snapPoint.Set(snapPoint.x + gridSize / 2 / 100, snapPoint.y + gridSize / 2 / 100);
            return snapPoint;
        }

    }
}
