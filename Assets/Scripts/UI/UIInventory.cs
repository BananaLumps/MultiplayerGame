using DevionGames;
using UnityEngine;

namespace Base.Assets.Scripts.UI
{
    public class UIInventory : MonoBehaviour
    {
        public GameObject slotPrefab;
        GameObject UIObject;
        GameObject UIGrid;
        bool dirty = true;
        private void Awake()
        {
            UIObject = GameObject.Find("InventoryPanel");
            UIGrid = UIObject.FindChild("Grid", true);
            slotPrefab = Resources.Load<GameObject>("InventorySlotPrefab");
        }
        private void Update()
        {
            if (dirty) RedrawInventory();
        }
        public void RedrawInventory()
        {
            foreach (Transform t in UIGrid.transform)
            {
                Destroy(t.gameObject);
            }
            foreach (var item in Core.Instance.LocalPlayer.Inventory.Items)
            {
                GameObject temp = Instantiate(slotPrefab, UIGrid.transform);

            }
            dirty = false;
        }
    }
}
