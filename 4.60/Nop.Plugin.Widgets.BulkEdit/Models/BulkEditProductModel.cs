using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Widgets.BulkEdit.Models
{
    public partial record BulkEditProductModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.SKU")]
        public string Sku { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.Price")]
        public decimal Price { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.OldPrice")]
        public decimal OldPrice { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.Gtin")]
        public string Gtin { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.ManufacturerPartNumber")]
        public string ManufacturerPartNumber { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.ManageInventoryMethod")]
        public string ManageInventoryMethod { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.ShowOnHomepage")]
        public bool ShowOnHomepage { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.DisableBuyButton")]
        public bool DisableBuyButton { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.CallForPrice")]
        public bool CallForPrice { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.Fields.Published")]
        public bool Published { get; set; }
    }
}
