using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
