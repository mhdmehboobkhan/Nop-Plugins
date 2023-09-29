using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Widgets.BulkEdit.Models
{
    public partial record BulkEditListModel: BaseNopModel
    {
        public BulkEditListModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.List.SearchKeyWord")]
        public string SearchKeyWord { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.List.SearchManufacturerId")]
        public int SearchManufacturerId { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.BulkEdit.List.SearchProductTypeId")]
        public int SearchProductTypeId { get; set; }
        
        public IList<SelectListItem> AvailableProductTypes { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
    }
}
