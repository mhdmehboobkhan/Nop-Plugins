using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.DiscountRules.CustomerVerify
{
    public partial class CustomerDiscountRequirementRule : BasePlugin, IDiscountRequirementRule
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public CustomerDiscountRequirementRule(IActionContextAccessor actionContextAccessor,
            IDiscountService discountService,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _actionContextAccessor = actionContextAccessor;
            _discountService = discountService;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _webHelper = webHelper;
            _orderService = orderService;
            _customerService = customerService;
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

            //try to get saved restricted customer role identifier
            var restrictedCustomerEmail = await _settingService.GetSettingByKeyAsync<string>(string.Format(DiscountRequirementDefaults.SettingsKey, request.DiscountRequirementId));
            if (string.IsNullOrEmpty(restrictedCustomerEmail))
                return result;

            //result is valid if the customer equals to the restricted one
            if (await _customerService.IsRegisteredAsync(request.Customer) && !string.IsNullOrEmpty(request.Customer.Email))
                result.IsValid = request.Customer.Email.Trim().ToLower() == restrictedCustomerEmail.Trim().ToLower();

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

            return urlHelper.Action("Configure", "DiscountRulesCustomerRoles",
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
                ["Plugins.DiscountRules.CustomerVerify.Fields.Customer"] = "Customer Email",
                ["Plugins.DiscountRules.CustomerVerify.Fields.Customer.Hint"] = "Discount will be applied if customer email is equal to the given email.",
                ["Plugins.DiscountRules.CustomerVerify.Fields.Customer.Select"] = "Select customer",
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
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.DiscountRules.CustomerVerify");


            await base.UninstallAsync();
        }

        #endregion
    }
}