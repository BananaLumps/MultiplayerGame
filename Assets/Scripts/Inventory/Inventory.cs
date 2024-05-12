using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;

namespace Base.Inventory
{
    public class Inventory : NetworkBehaviour
    {
        Dictionary<int, Item> _items = new Dictionary<int, Item>();
        public Dictionary<int, Item> Items => _items;

        public int GetStackSize(string itemID)
        {
            //TODO
            //Check the stack size based on type and skill levels
            return 1;
        }
        [ServerRpc]
        void AddItem(Item item, NetworkConnection nc)
        {
            int stackSize = GetStackSize(item.id);
            foreach (var i in _items)
            {
                if (i.Value.id == "Empty")
                {
                    if (item.count <= stackSize)
                    {
                        _items[i.Key] = item; break;
                    }
                    else
                    {
                        _items[i.Key] = new Item(item.id, stackSize);
                        item.count -= stackSize;
                        continue;
                    }
                }

            }
        }
    }
}
