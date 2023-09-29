using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.BulkEdit.Kendoui;
using Nop.Plugin.Widgets.BulkEdit.Models;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.BulkEdit.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsBulkEditController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IWorkContext _workContext;
        private readonly IProductService _productService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;

        public WidgetsBulkEditController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IWorkContext workContext,
            IProductService productService,
            IBackInStockSubscriptionService backInStockSubscriptionService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _storeContext = storeContext;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _workContext = workContext;
            _productService = productService;
            _backInStockSubscriptionService = backInStockSubscriptionService;
        }

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var bulkEditSettings = await _settingService.LoadSettingAsync<BulkEditSettings>(storeScope);
            var model = new ConfigurationModel
            {
                ShowSku = bulkEditSettings.ShowSku,
                ShowOldPrice = bulkEditSettings.ShowOldPrice,
                ShowGtin = bulkEditSettings.ShowGtin,
                ShowManufacturerPartNumber = bulkEditSettings.ShowManufacturerPartNumber,
                ShowManageInventoryMethod = bulkEditSettings.ShowManageInventoryMethod,
                ShowOnHomepage = bulkEditSettings.ShowOnHomepage,
                ShowCallForPrice = bulkEditSettings.ShowCallForPrice,
                ShowDisplayOrder = bulkEditSettings.ShowDisplayOrder,
                ShowDisableBuyButton = bulkEditSettings.ShowDisableBuyButton,
                ShowDisableWishlistButton = bulkEditSettings.ShowDisableWishlistButton,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.ShowSku_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowSku, storeScope);
                model.ShowOldPrice_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowOldPrice, storeScope);
                model.ShowGtin_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowGtin, storeScope);
                model.ShowManufacturerPartNumber_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowManufacturerPartNumber, storeScope);
                model.ShowManageInventoryMethod_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowManageInventoryMethod, storeScope);
                model.ShowOnHomepage_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowOnHomepage, storeScope);
                model.ShowCallForPrice_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowCallForPrice, storeScope);
                model.ShowDisplayOrder_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowDisplayOrder, storeScope);
                model.ShowDisableBuyButton_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowDisableBuyButton, storeScope);
                model.ShowDisableWishlistButton_OverrideForStore = await _settingService.SettingExistsAsync(bulkEditSettings, x => x.ShowDisableWishlistButton, storeScope);
            }

            return View("~/Plugins/Widgets.BulkEdit/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var bulkEditSettings = await _settingService.LoadSettingAsync<BulkEditSettings>(storeScope);

            bulkEditSettings.ShowSku = model.ShowSku;
            bulkEditSettings.ShowOldPrice = model.ShowOldPrice;
            bulkEditSettings.ShowGtin = model.ShowGtin;
            bulkEditSettings.ShowManufacturerPartNumber = model.ShowManufacturerPartNumber;
            bulkEditSettings.ShowManageInventoryMethod = model.ShowManageInventoryMethod;
            bulkEditSettings.ShowOnHomepage = model.ShowOnHomepage;
            bulkEditSettings.ShowCallForPrice = model.ShowCallForPrice;
            bulkEditSettings.ShowDisplayOrder = model.ShowDisplayOrder;
            bulkEditSettings.ShowDisableBuyButton = model.ShowDisableBuyButton;
            bulkEditSettings.ShowDisableWishlistButton = model.ShowDisableWishlistButton;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowSku, model.ShowSku_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowOldPrice, model.ShowOldPrice_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowGtin, model.ShowGtin_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowManufacturerPartNumber, model.ShowManufacturerPartNumber_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowManageInventoryMethod, model.ShowManageInventoryMethod_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowOnHomepage, model.ShowOnHomepage_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowCallForPrice, model.ShowCallForPrice_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowDisplayOrder, model.ShowDisplayOrder_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowDisableBuyButton, model.ShowDisableBuyButton_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(bulkEditSettings, x => x.ShowDisableWishlistButton, model.ShowDisableWishlistButton_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
            return await Configure();
        }

        #region Bulk editing

        public async Task<IActionResult> BulkEdit()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = new BulkEditListModel();
            //categories
            model.AvailableCategories.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem { Text = await _categoryService.GetFormattedBreadCrumbAsync(c), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var m in await _manufacturerService.GetAllManufacturersAsync(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            //product types
            model.AvailableProductTypes = (await ProductType.SimpleProduct.ToSelectListAsync(false)).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });

            return View("~/Plugins/Widgets.BulkEdit/Views/BulkEdit.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> BulkEditSelect(DataSourceRequest command, BulkEditListModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var vendor = await _workContext.GetCurrentVendorAsync();
            int vendorId = 0;
            //a vendor should have access only to his products
            if (vendor != null)
                vendorId = vendor.Id;

            var products = await _productService.SearchProductsAsync(categoryIds: new List<int> { model.SearchCategoryId },
                manufacturerIds: new List<int> { model.SearchManufacturerId },
                vendorId: vendorId,
                productType: model.SearchProductTypeId > 0 ? (ProductType?)model.SearchProductTypeId : null,
                keywords: model.SearchKeyWord,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true);
            var gridModel = new DataSourceResult();
            gridModel.Data = await (products.SelectAwait(async x =>
            {
                var productModel = new BulkEditProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sku = x.Sku,
                    Price = x.Price,
                    OldPrice = x.OldPrice,
                    Gtin = x.Gtin,
                    ManufacturerPartNumber = x.ManufacturerPartNumber,
                    ManageInventoryMethod = await _localizationService.GetLocalizedEnumAsync(x.ManageInventoryMethod),
                    StockQuantity = x.StockQuantity,
                    ShowOnHomepage = x.ShowOnHomepage,
                    DisableBuyButton = x.DisableBuyButton,
                    DisableWishlistButton = x.DisableWishlistButton,
                    CallForPrice = x.CallForPrice,
                    DisplayOrder = x.DisplayOrder,
                    Published = x.Published
                };

                if (x.ManageInventoryMethod == ManageInventoryMethod.ManageStock && x.UseMultipleWarehouses)
                {
                    //multi-warehouse supported
                    //TODO localize
                    productModel.ManageInventoryMethod += " (multi-warehouse)";
                }

                return productModel;
            }).ToListAsync());
            gridModel.Total = products.TotalCount;

            return Json(gridModel);
        }

        [HttpPost]
        public async Task<IActionResult> BulkEditUpdate(IEnumerable<BulkEditProductModel> products)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (products != null)
            {
                var bulkEditSettings = await _settingService.LoadSettingAsync<BulkEditSettings>(await _storeContext.GetActiveStoreScopeConfigurationAsync());
                var vendor = await _workContext.GetCurrentVendorAsync();

                foreach (var pModel in products)
                {
                    //update
                    var product = await _productService.GetProductByIdAsync(pModel.Id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (vendor != null && product.VendorId != vendor.Id)
                            continue;

                        var prevStockQuantity = await _productService.GetTotalStockQuantityAsync(product);
                        if(bulkEditSettings.ShowSku)
                            product.Sku = pModel.Sku;

                        product.Price = pModel.Price;
                        
                        if(bulkEditSettings.ShowOldPrice)
                            product.OldPrice = pModel.OldPrice;

                        if(bulkEditSettings.ShowGtin)
                            product.Gtin = pModel.Gtin;

                        if (bulkEditSettings.ShowManufacturerPartNumber)
                            product.ManufacturerPartNumber = pModel.ManufacturerPartNumber;

                        if (bulkEditSettings.ShowOnHomepage)
                            product.ShowOnHomepage = pModel.ShowOnHomepage;

                        if (bulkEditSettings.ShowCallForPrice)
                            product.CallForPrice = pModel.CallForPrice;

                        if (bulkEditSettings.ShowDisableBuyButton)
                            product.DisableBuyButton = pModel.DisableBuyButton;

                        if (bulkEditSettings.ShowDisableWishlistButton)
                            product.DisableWishlistButton = pModel.DisableWishlistButton;

                        if (bulkEditSettings.ShowDisplayOrder)
                            product.DisplayOrder = pModel.DisplayOrder;

                        product.StockQuantity = pModel.StockQuantity;
                        product.Published = pModel.Published;

                        await _productService.UpdateProductAsync(product);

                        //back in stock notifications
                        if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                            product.BackorderMode == BackorderMode.NoBackorders &&
                            product.AllowBackInStockSubscriptions &&
                            await _productService.GetTotalStockQuantityAsync(product) > 0 &&
                            prevStockQuantity <= 0 &&
                            product.Published &&
                            !product.Deleted)
                        {
                            await _backInStockSubscriptionService.SendNotificationsToSubscribersAsync(product);
                        }
                    }
                }
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public async Task<IActionResult> BulkEditDelete(IEnumerable<BulkEditProductModel> products)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (products != null)
            {
                var vendor = await _workContext.GetCurrentVendorAsync();
                foreach (var pModel in products)
                {
                    //delete
                    var product = await _productService.GetProductByIdAsync(pModel.Id);
                    if (product != null)
                    {
                        //a vendor should have access only to his products
                        if (vendor != null && product.VendorId != vendor.Id)
                            continue;

                        await _productService.DeleteProductAsync(product);
                    }
                }
            }
            return new NullJsonResult();
        }

        #endregion
    }
}