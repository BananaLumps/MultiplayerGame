using Base.Inventory;
using UnityEngine;

namespace Base
{
    public class UIInventorySlot : MonoBehaviour
    {
        InventoryItem item;
        [SerializeField]
        int slot;
        public InventoryItem Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }
        public int Slot
        {
            get
            {
                return slot;
            }
            set
            {
                slot = value;
            }
        }

        public void SwapItemSlot(UIInventorySlot target)
        {
            InventoryItem temp = new InventoryItem(Item);
            item = target.Item;
            target.Item = temp;
        }
    }
}
