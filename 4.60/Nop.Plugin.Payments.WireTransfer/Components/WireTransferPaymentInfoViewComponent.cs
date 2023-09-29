using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.WireTransfer.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.WireTransfer.Components
{
    public class WireTransferViewComponent : NopViewComponent
    {
        private readonly WireTransferPaymentSettings _checkMoneyOrderPaymentSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        public WireTransferViewComponent(WireTransferPaymentSettings checkMoneyOrderPaymentSettings,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _checkMoneyOrderPaymentSettings = checkMoneyOrderPaymentSettings;
            _localizationService = localizationService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var store = await _storeContext.GetCurrentStoreAsync();

            var model = new PaymentInfoModel
            {
                DescriptionText = await _localizationService.GetLocalizedSettingAsync(_checkMoneyOrderPaymentSettings,
                    x => x.DescriptionText, (await _workContext.GetWorkingLanguageAsync()).Id, store.Id)
            };

            return View("~/Plugins/Payments.WireTransfer/Views/PaymentInfo.cshtml", model);
        }
    }
}