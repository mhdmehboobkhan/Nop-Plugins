using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.BulkEdit.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.BulkEdit
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class BulkEditPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly INopFileProvider _fileProvider;

        public BulkEditPlugin(ILocalizationService localizationService,
            IPictureService pictureService,
            ISettingService settingService,
            IWebHelper webHelper,
            INopFileProvider fileProvider)
        {
            _localizationService = localizationService;
            _pictureService = pictureService;
            _settingService = settingService;
            _webHelper = webHelper;
            _fileProvider = fileProvider;
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var mainMenuItem = new SiteMapNode()
            {
                SystemName = "WidgetsBulkEdit",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.BulkEdit"),
                ControllerName = "",
                ActionName = "",
                IconClass = "fa fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };

            var listMenus = new List<SiteMapNode>();
            var menuItem = new SiteMapNode()
            {
                SystemName = "WidgetsBulkEditConfiguration",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.BulkEdit.Configuration"),
                ControllerName = "WidgetsBulkEdit",
                ActionName = "Configure",
                IconClass = "fa fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            listMenus.Add(menuItem);

            var menuItem2 = new SiteMapNode()
            {
                SystemName = "WidgetsBulkEditList",
                Title = await _localizationService.GetResourceAsync("Plugins.Widgets.BulkEdit.List"),
                ControllerName = "WidgetsBulkEdit",
                ActionName = "BulkEdit",
                IconClass = "fa fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            listMenus.Add(menuItem2);

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == mainMenuItem.SystemName);
            if (pluginNode != null)
            {
                foreach (var item in listMenus)
                    pluginNode.ChildNodes.Add(item);
            }
            else
            {
                foreach (var item in listMenus)
                    mainMenuItem.ChildNodes.Add(item);
                rootNode.ChildNodes.Add(mainMenuItem);
            }
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { AdminWidgetZones.ProductListButtons });
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsBulkEdit/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsBulkEditViewComponent);
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override async Task InstallAsync()
        {
            //settings
            var settings = new BulkEditSettings
            {
                ShowSku = true,
                ShowManageInventoryMethod = true,
                ShowDisplayOrder = true
            };
            await _settingService.SaveSettingAsync(settings);

            var resources = new Dictionary<string, string>
            {
                ["Plugins.Widgets.BulkEdit"] = "Bulk edit",
                ["Plugins.Widgets.BulkEdit.Configuration"] = "Configuration",
                ["Plugins.Widgets.BulkEdit.Configuration.Detail"] = "Please mark the properties to show or hide at editor page.",
                ["Plugins.Widgets.BulkEdit.List"] = "Bulk Products",
                ["Plugins.Widgets.BulkEdit.List.SearchCategory"] = "Categories",
                ["Plugins.Widgets.BulkEdit.List.SearchCategory.hint"] = "Search product by category",
                ["Plugins.Widgets.BulkEdit.List.SearchManufacturerId"] = "Manufacturers",
                ["Plugins.Widgets.BulkEdit.List.SearchManufacturerId.hint"] = "Search product by manufacturer",
                ["Plugins.Widgets.BulkEdit.List.SearchProductTypeId"] = "Product type",
                ["Plugins.Widgets.BulkEdit.List.SearchProductTypeId.hint"] = "Search product by product type",
                ["Plugins.Widgets.BulkEdit.List.SearchKeyWord"] = "Keyword",
                ["Plugins.Widgets.BulkEdit.List.SearchKeyWord.hint"] = "Search product by keyword like: name, sku or description",
                ["Plugins.Widgets.BulkEdit.Fields.Name"] = "Name",
                ["Plugins.Widgets.BulkEdit.Fields.SKU"] = "SKU",
                ["Plugins.Widgets.BulkEdit.Fields.Price"] = "Price",
                ["Plugins.Widgets.BulkEdit.Fields.OldPrice"] = "Old price",
                ["Plugins.Widgets.BulkEdit.Fields.Gtin"] = "GTIN",
                ["Plugins.Widgets.BulkEdit.Fields.ManufacturerPartNumber"] = "Manufacturer part number",
                ["Plugins.Widgets.BulkEdit.Fields.ManageInventoryMethod"] = "Inventory method",
                ["Plugins.Widgets.BulkEdit.Fields.StockQuantity"] = "Stock quantity",
                ["Plugins.Widgets.BulkEdit.Fields.ShowOnHomepage"] = "Show on home",
                ["Plugins.Widgets.BulkEdit.Fields.DisableBuyButton"] = "Disable buy",
                ["Plugins.Widgets.BulkEdit.Fields.DisableWishlistButton"] = "Disable wishlist",
                ["Plugins.Widgets.BulkEdit.Fields.CallForPrice"] = "CallFor price",
                ["Plugins.Widgets.BulkEdit.Fields.DisplayOrder"] = "Display order",
                ["Plugins.Widgets.BulkEdit.Fields.Published"] = "Published",

                ["Plugins.Widgets.BulkEdit.Configure.ShowSku"] = "Show sku",
                ["Plugins.Widgets.BulkEdit.Configure.ShowSku.hint"] = "Set the property to show sku field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowOldPrice"] = "Show old price",
                ["Plugins.Widgets.BulkEdit.Configure.ShowOldPrice.hint"] = "Set the property to show old price field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowGtin"] = "Show GTIN",
                ["Plugins.Widgets.BulkEdit.Configure.ShowGtin.hint"] = "Set the property to show GTIN field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowManufacturerPartNumber"] = "Show manufacturer part number",
                ["Plugins.Widgets.BulkEdit.Configure.ShowManufacturerPartNumber.hint"] = "Set the property to show manufacturer part number field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowManageInventoryMethod"] = "Show inventory method",
                ["Plugins.Widgets.BulkEdit.Configure.ShowManageInventoryMethod.hint"] = "Set the property to show inventory method field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowOnHomepage"] = "Show on homepage",
                ["Plugins.Widgets.BulkEdit.Configure.ShowOnHomepage.hint"] = "Set the property to show homepage field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowCallForPrice"] = "Show call for price",
                ["Plugins.Widgets.BulkEdit.Configure.ShowCallForPrice.hint"] = "Set the property to show call for price field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisplayOrder"] = "Show display order",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisplayOrder.hint"] = "Set the property to show display order field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisableBuyButton"] = "Show disable buy button",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisableBuyButton.hint"] = "Set the property to show disable buy button field at editor.",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisableWishlistButton"] = "Show disable wishlist button",
                ["Plugins.Widgets.BulkEdit.Configure.ShowDisableWishlistButton.hint"] = "Set the property to show disable wishlist button field at editor.",
            };

            foreach (var res in resources)
                await _localizationService.AddOrUpdateLocaleResourceAsync(res.Key, res.Value, null);

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<BulkEditSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BulkEdit");

            await base.UninstallAsync();
        }

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;
    }
}