using UnityEngine;

namespace Base
{
    public class UIInventorySlot : MonoBehaviour
    {
        Item item;
        [SerializeField]
        int slot;
        public Item Item
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
            Item temp = new Item(Item);
            item = target.Item;
            target.Item = temp;
        }
    }
}
