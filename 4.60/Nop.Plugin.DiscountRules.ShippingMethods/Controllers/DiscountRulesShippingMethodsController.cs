using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.DiscountRules.ShippingMethods.Models;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.DiscountRules.ShippingMethods.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class DiscountRulesShippingMethodsController : BasePluginController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IShippingService _shippingService;
        
        #endregion

        #region Ctor

        public DiscountRulesShippingMethodsController(ICustomerService customerService,
            IDiscountService discountService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IShippingService shippingService)
        {
            _customerService = customerService;
            _discountService = discountService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _shippingService = shippingService;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure(int discountId, int? discountRequirementId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            //load the discount
            var discount = await _discountService.GetDiscountByIdAsync(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            //check whether the discount requirement exists
            if (discountRequirementId.HasValue && _discountService.GetDiscountRequirementByIdAsync(discountRequirementId.Value) is null)
                return Content("Failed to load requirement.");

            var shippingMethod =await _settingService.GetSettingByKeyAsync<int>(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirementId ?? 0));

            var model = new RequirementModel
            {
                RequirementId = discountRequirementId ?? 0,
                DiscountId = discountId,
                ShippingMethodId = shippingMethod
            };

            model.AvailableShippingMethods.Add(new SelectListItem { Text = "Select shipping method", Value = "0" });
            foreach (var cr in await _shippingService.GetAllShippingMethodsAsync())
                model.AvailableShippingMethods.Add(new SelectListItem { Text = cr.Name, Value = cr.Id.ToString(), Selected = cr.Id == shippingMethod });

            //set the HTML field prefix
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format(DiscountRequirementDefaults.HtmlFieldPrefix, discountRequirementId ?? 0);

            return View("~/Plugins/DiscountRules.ShippingMethods/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(int discountId, int? discountRequirementId, int shippingMethodId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            //load the discount
            var discount = _discountService.GetDiscountByIdAsync(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            //get the discount requirement
               var discountRequirement = await _discountService.GetDiscountRequirementByIdAsync(discountRequirementId.Value);

            //the discount requirement does not exist, so create a new one
            if (discountRequirement == null)
            {
                discountRequirement = new DiscountRequirement
                {
                    DiscountId = discount.Id,
                    DiscountRequirementRuleSystemName = DiscountRequirementDefaults.SystemName
                };

                await _discountService.InsertDiscountRequirementAsync(discountRequirement);
            }

            await _settingService.SetSettingAsync(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id), shippingMethodId);

            return Json(new { Result = true, NewRequirementId = discountRequirement.Id });
        }

        #endregion
    }
}