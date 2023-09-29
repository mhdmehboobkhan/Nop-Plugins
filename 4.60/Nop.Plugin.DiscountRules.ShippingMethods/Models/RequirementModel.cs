using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.DiscountRules.ShippingMethods.Models
{
    public class RequirementModel
    {
        public RequirementModel()
        {
            AvailableShippingMethods = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.DiscountRules.ShippingMethods.Fields.ShippingMethod")]
        public int ShippingMethodId { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }

        public IList<SelectListItem> AvailableShippingMethods { get; set; }
    }
}