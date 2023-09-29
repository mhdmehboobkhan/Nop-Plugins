using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.DiscountRules.CustomerVerify.Models
{
    public class RequirementModel
    {
        public RequirementModel()
        {
        }

        [NopResourceDisplayName("Plugins.DiscountRules.CustomerVerify.Fields.Customer")]
        public string CustomerEmail { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }

    }
}