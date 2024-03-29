﻿using Nop.Core.Domain.Discounts;
using Nop.Core.Events;
using Nop.Services.Configuration;
using Nop.Services.Events;
using System.Threading.Tasks;

namespace Nop.Plugin.DiscountRules.ShippingMethods.Infrastructure.Cache
{
    /// <summary>
    /// Discount requirement rule event consumer (used for removing unused settings)
    /// </summary>
    public partial class DiscountRequirementEventConsumer : IConsumer<EntityDeletedEvent<DiscountRequirement>>
    {
        #region Fields
        
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public DiscountRequirementEventConsumer(ISettingService settingService)
        {
            _settingService = settingService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle discount requirement deleted event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public async Task HandleEventAsync(EntityDeletedEvent<DiscountRequirement> eventMessage)
        {
            var discountRequirement = eventMessage?.Entity;
            if (discountRequirement == null)
                return;

            var setting = await _settingService.GetSettingAsync(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id));
            if (setting != null)
                await _settingService.DeleteSettingAsync(setting);
        }

        #endregion
    }
}