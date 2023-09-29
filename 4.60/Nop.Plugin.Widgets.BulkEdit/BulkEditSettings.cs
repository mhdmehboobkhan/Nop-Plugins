using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.BulkEdit
{
    public class BulkEditSettings : ISettings
    {
        public bool ShowSku { get; set; }
        public bool ShowOldPrice { get; set; }
        public bool ShowGtin { get; set; }
        public bool ShowManufacturerPartNumber { get; set; }
        public bool ShowManageInventoryMethod { get; set; }
        public bool ShowOnHomepage { get; set; }
        public bool ShowCallForPrice { get; set; }
        public bool ShowDisableBuyButton { get; set; }
        public bool ShowDisableWishlistButton { get; set; }
        public bool ShowDisplayOrder { get; set; }
    }
}