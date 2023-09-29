using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.BulkEdit.Models;
using Nop.Services.Caching;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BulkEdit.Components
{
    [ViewComponent(Name = "WidgetsBulkEdit")]
    public class WidgetsBulkEditViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        public WidgetsBulkEditViewComponent(IStoreContext storeContext, 
            IStaticCacheManager staticCacheManager, 
            ISettingService settingService, 
            IPictureService pictureService,
            IWebHelper webHelper)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var model = new PublicInfoModel();
            return View("~/Plugins/Widgets.BulkEdit/Views/PublicInfo.cshtml", model);
        }
    }
}
