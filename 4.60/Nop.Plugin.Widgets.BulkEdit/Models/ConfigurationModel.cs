using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.BulkEdit.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowSku")]
        public bool ShowSku { get; set; }
        public bool ShowSku_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowOldPrice")]
        public bool ShowOldPrice { get; set; }
        public bool ShowOldPrice_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowGtin")]
        public bool ShowGtin { get; set; }
        public bool ShowGtin_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowManufacturerPartNumber")]
        public bool ShowManufacturerPartNumber { get; set; }
        public bool ShowManufacturerPartNumber_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowManageInventoryMethod")]
        public bool ShowManageInventoryMethod { get; set; }
        public bool ShowManageInventoryMethod_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowOnHomepage")]
        public bool ShowOnHomepage { get; set; }
        public bool ShowOnHomepage_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowCallForPrice")]
        public bool ShowCallForPrice { get; set; }
        public bool ShowCallForPrice_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowDisplayOrder")]
        public bool ShowDisplayOrder { get; set; }
        public bool ShowDisplayOrder_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowDisableBuyButton")]
        public bool ShowDisableBuyButton { get; set; }
        public bool ShowDisableBuyButton_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Configure.ShowDisableWishlistButton")]
        public bool ShowDisableWishlistButton { get; set; }
        public bool ShowDisableWishlistButton_OverrideForStore { get; set; }
    }
}