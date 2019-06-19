using SDG.Unturned;

namespace RestoreMonarchy.Pickpocket.Models
{
    public class InventoryItem
    {
        public InventoryItem(ItemJar item, byte page, byte index)
        {
            this.Item = item;
            this.Page = page;
            this.Index = index;
        }

        public ItemJar Item { get; set; }
        public byte Page { get; set; }
        public byte Index { get; set; }
    }
}
