using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Shipping;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;

namespace Nop.Plugin.DiscountRules.ShippingMethods
{
    public partial class ShippingMethodDiscountRequirementRule : BasePlugin, IDiscountRequirementRule
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IShippingService _shippingService;

        #endregion

        #region Ctor

        public ShippingMethodDiscountRequirementRule(IActionContextAccessor actionContextAccessor,
            IDiscountService discountService,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper,
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IShippingService shippingService)
        {
            _actionContextAccessor = actionContextAccessor;
            _discountService = discountService;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _webHelper = webHelper;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _shippingService = shippingService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check discount requirement
        /// </summary>
        /// <param name="request">Object that contains all information required to check the requirement (Current customer, discount, etc)</param>
        /// <returns>Result</returns>
        public async Task<DiscountRequirementValidationResult> CheckRequirementAsync(DiscountRequirementValidationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            //invalid by default
            var result = new DiscountRequirementValidationResult();

            if (request.Customer == null)
                return result;

            //try to get saved restricted shipping methods identifier
            var shippingMethodId = await _settingService.GetSettingByKeyAsync<int>(string.Format(DiscountRequirementDefaults.SettingsKey, request.DiscountRequirementId));
            if (shippingMethodId == 0)
                return result;

            //find a selected (previously) shipping method
            var shippingOption = await _genericAttributeService.GetAttributeAsync<ShippingOption>(await _workContext.GetCurrentCustomerAsync(), NopCustomerDefaults.SelectedShippingOptionAttribute, _storeContext.GetCurrentStore().Id);
            if (shippingOption != null)
            {
                var shippingMethod = await _shippingService.GetShippingMethodByIdAsync(shippingMethodId);

                if (shippingMethod.Name.Equals(shippingOption.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    //valid
                    result.IsValid = true;
                    return result;
                }
                else
                    result.UserError = await _localizationService.GetResourceAsync("Plugins.DiscountRules.ShippingMethods.Fields.NotAllowed");
            }

            return result;
        }

        /// <summary>
        /// Get URL for rule configuration
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="discountRequirementId">Discount requirement identifier (if editing)</param>
        /// <returns>URL</returns>
        public string GetConfigurationUrl(int discountId, int? discountRequirementId)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            return urlHelper.Action("Configure", "DiscountRulesShippingMethods",
                new { discountId = discountId, discountRequirementId = discountRequirementId }, _webHelper.GetCurrentRequestProtocol());
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override async Task InstallAsync()
        {
            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.DiscountRules.ShippingMethods.Fields.ShippingMethod"] = "Required shipping method",
                ["Plugins.DiscountRules.ShippingMethods.Fields.ShippingMethod.Hint"] = "Discount will be applied if customer selected the shipping method.",
                ["Plugins.DiscountRules.ShippingMethods.Fields.NotAllowed"] = "Selected shipping method not allowed to use this discount"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override async Task UninstallAsync()
        {
            //discount requirements
            var discountRequirements = (await _discountService.GetAllDiscountRequirementsAsync())
                 .Where(discountRequirement => discountRequirement.DiscountRequirementRuleSystemName == DiscountRequirementDefaults.SystemName);
            foreach (var discountRequirement in discountRequirements)
            {
                await _discountService.DeleteDiscountRequirementAsync(discountRequirement, false);
            }

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.DiscountRules.ShippingMethods");

            await base.UninstallAsync();
        }

        #endregion
    }
}