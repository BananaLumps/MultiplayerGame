using Base;
using Base.Inventory;

public class Item
{
    public readonly string id;
    public readonly string name;
    public readonly string description;
    public int count;
    public readonly ItemStats stats;
    public readonly ItemType type;
    public Item(Item item)
    {
        this.id = item.id;
        this.name = item.name;
        this.description = item.description;
        this.count = item.count;
        this.stats = item.stats;
        this.type = item.type;
    }
    public Item(string id)
    {
        this.id = id;
        this.name = Core.Instance.Items[id].name;
        this.description = Core.Instance.Items[id].description;
        this.count = 1;
        this.stats = Core.Instance.Items[id].stats;
        this.type = Core.Instance.Items[id].type;
    }
    public Item(string id, int count)
    {
        this.id = id;
        this.name = Core.Instance.Items[id].name;
        this.description = Core.Instance.Items[id].description;
        this.count = count;
        this.stats = Core.Instance.Items[id].stats;
        this.type = Core.Instance.Items[id].type;
    }
    public Item()
    {
    }

}
