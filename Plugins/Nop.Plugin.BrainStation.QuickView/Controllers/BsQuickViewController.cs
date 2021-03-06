﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.BrainStation.QuickView.Models;
using Nop.Services.Configuration;
using Nop.Web.Extensions;
using Nop.Plugin.BrainStation.QuickView.Infrastructure.Cache;


using Nop.Services.Catalog;

using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Controllers;
using Nop.Web.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.UI.Captcha;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;

namespace Nop.Plugin.BrainStation.QuickView.Controllers
{
    public class BsQuickViewController : BasePublicController
    {
        
        
        


        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IWebHelper _webHelper;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
        private readonly ICompareProductsService _compareProductsService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IProductTagService _productTagService;
        private readonly IOrderReportService _orderReportService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IShippingService _shippingService;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly SeoSettings _seoSettings;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingService _settingService;


        public BsQuickViewController(
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService,
            IVendorService vendorService,
            IProductTemplateService productTemplateService,
            IProductAttributeService productAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ITaxService taxService,
            ICurrencyService currencyService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IWebHelper webHelper,
            ISpecificationAttributeService specificationAttributeService,
            IDateTimeHelper dateTimeHelper,
            IRecentlyViewedProductsService recentlyViewedProductsService,
            ICompareProductsService compareProductsService,
            IWorkflowMessageService workflowMessageService,
            IProductTagService productTagService,
            IOrderReportService orderReportService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            IProductAttributeParser productAttributeParser,
            IShippingService shippingService,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            VendorSettings vendorSettings,
            ShoppingCartSettings shoppingCartSettings,
            LocalizationSettings localizationSettings,
            CustomerSettings customerSettings,
            CaptchaSettings captchaSettings,
            SeoSettings seoSettings,
            ICacheManager cacheManager,
            ISettingService settingService
            )
        {
            
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._vendorService = vendorService;
            this._productTemplateService = productTemplateService;
            this._productAttributeService = productAttributeService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._webHelper = webHelper;
            this._specificationAttributeService = specificationAttributeService;
            this._dateTimeHelper = dateTimeHelper;
            this._recentlyViewedProductsService = recentlyViewedProductsService;
            this._compareProductsService = compareProductsService;
            this._workflowMessageService = workflowMessageService;
            this._productTagService = productTagService;
            this._orderReportService = orderReportService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._productAttributeParser = productAttributeParser;
            this._shippingService = shippingService;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._vendorSettings = vendorSettings;
            this._shoppingCartSettings = shoppingCartSettings;
            this._localizationSettings = localizationSettings;
            this._customerSettings = customerSettings;
            this._captchaSettings = captchaSettings;
            this._seoSettings = seoSettings;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
        }


        


        #region Non-Action
        [NonAction]
        protected virtual IEnumerable<ProductOverviewModel> PrepareProductOverviewModels(IEnumerable<Product> products,
            bool preparePriceModel = true, bool preparePictureModel = true,
            int? productThumbPictureSize = null, bool prepareSpecificationAttributes = false,
            bool forceRedirectionAfterAddingToCart = false)
        {
            return this.PrepareProductOverviewModels(_workContext,
                _storeContext, _categoryService, _productService, _specificationAttributeService,
                _priceCalculationService, _priceFormatter, _permissionService,
                _localizationService, _taxService, _currencyService,
                _pictureService, _webHelper, _cacheManager,
                _catalogSettings, _mediaSettings, products,
                preparePriceModel, preparePictureModel,
                productThumbPictureSize, prepareSpecificationAttributes,
                forceRedirectionAfterAddingToCart);
        }

        [NonAction]
        protected virtual ProductDetailsModel PrepareProductDetailsPageModel(Product product,
            ShoppingCartItem updatecartitem = null, bool isAssociatedProduct = false)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
                .Where(cr => cr.Active)
                .Select(cr => cr.Id)
                .ToList();

            #region Standard properties

            var model = new ProductDetailsModel()
            {
                Id = product.Id,
                Name = product.GetLocalized(x => x.Name),
                ShortDescription = product.GetLocalized(x => x.ShortDescription),
                FullDescription = product.GetLocalized(x => x.FullDescription),
                MetaKeywords = product.GetLocalized(x => x.MetaKeywords),
                MetaDescription = product.GetLocalized(x => x.MetaDescription),
                MetaTitle = product.GetLocalized(x => x.MetaTitle),
                SeName = product.GetSeName(),
                ShowSku = _catalogSettings.ShowProductSku,
                Sku = product.Sku,
                ShowManufacturerPartNumber = _catalogSettings.ShowManufacturerPartNumber,
                FreeShippingNotificationEnabled = _catalogSettings.ShowFreeShippingNotification,
                ManufacturerPartNumber = product.ManufacturerPartNumber,
                ShowGtin = _catalogSettings.ShowGtin,
                Gtin = product.Gtin,
                StockAvailability = product.FormatStockMessage(_localizationService),
                HasSampleDownload = product.IsDownload && product.HasSampleDownload,
            };

            //automatically generate product description?
            if (_seoSettings.GenerateProductMetaDescription && String.IsNullOrEmpty(model.MetaDescription))
            {
                //based on short description
                model.MetaDescription = model.ShortDescription;
            }

            //shipping info
            model.IsShipEnabled = product.IsShipEnabled;
            if (product.IsShipEnabled)
            {
                model.IsFreeShipping = product.IsFreeShipping;
                //delivery date
                var deliveryDate = _shippingService.GetDeliveryDateById(product.DeliveryDateId);
                if (deliveryDate != null)
                {
                    model.DeliveryDate = deliveryDate.GetLocalized(dd => dd.Name);
                }
            }

            //email a friend
            model.EmailAFriendEnabled = _catalogSettings.EmailAFriendEnabled;
            //compare products
            model.CompareProductsEnabled = _catalogSettings.CompareProductsEnabled;

            #endregion

            #region Vendor details

            //vendor
            if (_vendorSettings.ShowVendorOnProductDetailsPage)
            {
                var vendor = _vendorService.GetVendorById(product.VendorId);
                if (vendor != null && !vendor.Deleted && vendor.Active)
                {
                    model.ShowVendor = true;

                    model.VendorModel = new VendorBriefInfoModel()
                    {
                        Id = vendor.Id,
                        Name = vendor.GetLocalized(x => x.Name),
                        SeName = vendor.GetSeName(),
                    };
                }
            }

            #endregion

            #region Page sharing

            if (_catalogSettings.ShowShareButton && !String.IsNullOrEmpty(_catalogSettings.PageShareCode))
            {
                var shareCode = _catalogSettings.PageShareCode;
                if (_webHelper.IsCurrentConnectionSecured())
                {
                    //need to change the addthis link to be https linked when the page is, so that the page doesnt ask about mixed mode when viewed in https...
                    shareCode = shareCode.Replace("http://", "https://");
                }
                model.PageShareCode = shareCode;
            }

            #endregion

            #region Back in stock subscriptions

            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                product.StockQuantity <= 0)
            {
                //out of stock
                model.DisplayBackInStockSubscription = true;
            }

            #endregion

            #region Breadcrumb

            //do not prepare this model for the associated products. any it's not used
            if (_catalogSettings.CategoryBreadcrumbEnabled && !isAssociatedProduct)
            {
                var breadcrumbCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_BREADCRUMB_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
                model.Breadcrumb = _cacheManager.Get(breadcrumbCacheKey, () =>
                {
                    var breadcrumbModel = new ProductDetailsModel.ProductBreadcrumbModel()
                    {
                        Enabled = _catalogSettings.CategoryBreadcrumbEnabled,
                        ProductId = product.Id,
                        ProductName = product.GetLocalized(x => x.Name),
                        ProductSeName = product.GetSeName()
                    };
                    var productCategories = _categoryService.GetProductCategoriesByProductId(product.Id);
                    if (productCategories.Count > 0)
                    {
                        var category = productCategories[0].Category;
                        if (category != null)
                        {
                            foreach (var catBr in category.GetCategoryBreadCrumb(_categoryService, _aclService, _storeMappingService))
                            {
                                breadcrumbModel.CategoryBreadcrumb.Add(new CategorySimpleModel()
                                {
                                    Id = catBr.Id,
                                    Name = catBr.GetLocalized(x => x.Name),
                                    SeName = catBr.GetSeName()
                                });
                            }
                        }
                    }
                    return breadcrumbModel;
                });
            }

            #endregion

            #region Product tags

            //do not prepare this model for the associated products. any it's not used
            if (!isAssociatedProduct)
            {
                var productTagsCacheKey = string.Format(ModelCacheEventConsumer.PRODUCTTAG_BY_PRODUCT_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
                model.ProductTags = _cacheManager.Get(productTagsCacheKey, () =>
                {
                    return product.ProductTags
                        //filter by store
                        .Where(x => _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id) > 0)
                        .Select(x =>
                        {
                            var ptModel = new ProductTagModel()
                            {
                                Id = x.Id,
                                Name = x.GetLocalized(y => y.Name),
                                SeName = x.GetSeName(),
                                ProductCount = _productTagService.GetProductCount(x.Id, _storeContext.CurrentStore.Id)
                            };
                            return ptModel;
                        })
                        .ToList();
                });
            }

            #endregion

            #region Templates

            var templateCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_TEMPLATE_MODEL_KEY, product.ProductTemplateId);
            model.ProductTemplateViewPath = _cacheManager.Get(templateCacheKey, () =>
            {
                var template = _productTemplateService.GetProductTemplateById(product.ProductTemplateId);
                if (template == null)
                    template = _productTemplateService.GetAllProductTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            #endregion

            #region Pictures

            model.DefaultPictureZoomEnabled = _mediaSettings.DefaultPictureZoomEnabled;
            //default picture
            var defaultPictureSize = isAssociatedProduct ?
                _mediaSettings.AssociatedProductPictureSize :
                _mediaSettings.ProductDetailsPictureSize;
            //prepare picture models
            var productPicturesCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DETAILS_PICTURES_MODEL_KEY, product.Id, defaultPictureSize, isAssociatedProduct, _workContext.WorkingLanguage.Id, _webHelper.IsCurrentConnectionSecured(), _storeContext.CurrentStore.Id);
            /*var cachedPictures = _cacheManager.Get(productPicturesCacheKey, () =>
            {
                var pictures = _pictureService.GetPicturesByProductId(product.Id);

                var defaultPictureModel = new PictureModel()
                {
                    ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), defaultPictureSize, !isAssociatedProduct),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), 0, !isAssociatedProduct),
                    Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                    AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                };
                //all pictures
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    pictureModels.Add(new PictureModel()
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                        AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                    });
                }

                return new { DefaultPictureModel = defaultPictureModel, PictureModels = pictureModels };
            });
            model.DefaultPictureModel = cachedPictures.DefaultPictureModel;
            model.PictureModels = cachedPictures.PictureModels;*/


           
                var pictures = _pictureService.GetPicturesByProductId(product.Id);

                var defaultPictureModel = new PictureModel()
                {
                    ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), defaultPictureSize, !isAssociatedProduct),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault(), 0, !isAssociatedProduct),
                    Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                    AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                };
                //all pictures
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    pictureModels.Add(new PictureModel()
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture, _mediaSettings.ProductThumbPictureSizeOnProductDetailsPage),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        Title = string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat.Details"), model.Name),
                        AlternateText = string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat.Details"), model.Name),
                    });
                }



                model.DefaultPictureModel = defaultPictureModel;
                model.PictureModels = pictureModels;



            #endregion

            #region Product price

            model.ProductPrice.ProductId = product.Id;
            if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            {
                model.ProductPrice.HidePrices = false;
                if (product.CustomerEntersPrice)
                {
                    model.ProductPrice.CustomerEntersPrice = true;
                }
                else
                {
                    if (product.CallForPrice)
                    {
                        model.ProductPrice.CallForPrice = true;
                    }
                    else
                    {
                        decimal taxRate = decimal.Zero;
                        decimal oldPriceBase = _taxService.GetProductPrice(product, product.OldPrice, out taxRate);
						decimal finalPriceWithoutDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, _workContext.CurrentCustomer, includeDiscounts: false), out taxRate);
						decimal finalPriceWithDiscountBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, _workContext.CurrentCustomer, includeDiscounts: true), out taxRate);

						decimal oldPrice = _currencyService.ConvertFromPrimaryStoreCurrency(oldPriceBase, _workContext.WorkingCurrency);
                        decimal finalPriceWithoutDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithoutDiscountBase, _workContext.WorkingCurrency);
                        decimal finalPriceWithDiscount = _currencyService.ConvertFromPrimaryStoreCurrency(finalPriceWithDiscountBase, _workContext.WorkingCurrency);

                        if (finalPriceWithoutDiscountBase != oldPriceBase && oldPriceBase > decimal.Zero)
                            model.ProductPrice.OldPrice = _priceFormatter.FormatPrice(oldPrice);

                        model.ProductPrice.Price = _priceFormatter.FormatPrice(finalPriceWithoutDiscount);

                        if (finalPriceWithoutDiscountBase != finalPriceWithDiscountBase)
                            model.ProductPrice.PriceWithDiscount = _priceFormatter.FormatPrice(finalPriceWithDiscount);

                        model.ProductPrice.PriceValue = finalPriceWithoutDiscount;
                        model.ProductPrice.PriceWithDiscountValue = finalPriceWithDiscount;

                        //property for German market
                        //we display tax/shipping info only with "shipping enabled" for this product
                        //we also ensure this it's not free shipping
                        model.ProductPrice.DisplayTaxShippingInfo = _catalogSettings.DisplayTaxShippingInfoProductDetailsPage
                            && product.IsShipEnabled &&
                            !product.IsFreeShipping;

                        //currency code
                        model.ProductPrice.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
                    }
                }
            }
            else
            {
                model.ProductPrice.HidePrices = true;
                model.ProductPrice.OldPrice = null;
                model.ProductPrice.Price = null;
            }
            #endregion

            #region 'Add to cart' model

            model.AddToCart.ProductId = product.Id;
            model.AddToCart.UpdatedShoppingCartItemId = updatecartitem != null ? updatecartitem.Id : 0;

            //quantity
            model.AddToCart.EnteredQuantity = updatecartitem != null ? updatecartitem.Quantity : product.OrderMinimumQuantity;

            //'add to cart', 'add to wishlist' buttons
            model.AddToCart.DisableBuyButton = product.DisableBuyButton || !_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart);
            model.AddToCart.DisableWishlistButton = product.DisableWishlistButton || !_permissionService.Authorize(StandardPermissionProvider.EnableWishlist);
            if (!_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            {
                model.AddToCart.DisableBuyButton = true;
                model.AddToCart.DisableWishlistButton = true;
            }
            //pre-order
            if (product.AvailableForPreOrder)
            {
                model.AddToCart.AvailableForPreOrder = !product.PreOrderAvailabilityStartDateTimeUtc.HasValue ||
                    product.PreOrderAvailabilityStartDateTimeUtc.Value >= DateTime.UtcNow;
                model.AddToCart.PreOrderAvailabilityStartDateTimeUtc = product.PreOrderAvailabilityStartDateTimeUtc;
            }

            //customer entered price
            model.AddToCart.CustomerEntersPrice = product.CustomerEntersPrice;
            if (model.AddToCart.CustomerEntersPrice)
            {
                decimal minimumCustomerEnteredPrice = _currencyService.ConvertFromPrimaryStoreCurrency(product.MinimumCustomerEnteredPrice, _workContext.WorkingCurrency);
                decimal maximumCustomerEnteredPrice = _currencyService.ConvertFromPrimaryStoreCurrency(product.MaximumCustomerEnteredPrice, _workContext.WorkingCurrency);

                model.AddToCart.CustomerEnteredPrice = updatecartitem != null ? updatecartitem.CustomerEnteredPrice : minimumCustomerEnteredPrice;
                model.AddToCart.CustomerEnteredPriceRange = string.Format(_localizationService.GetResource("Products.EnterProductPrice.Range"),
                    _priceFormatter.FormatPrice(minimumCustomerEnteredPrice, false, false),
                    _priceFormatter.FormatPrice(maximumCustomerEnteredPrice, false, false));
            }
            //allowed quantities
            var allowedQuantities = product.ParseAllowedQuatities();
            foreach (var qty in allowedQuantities)
            {
                model.AddToCart.AllowedQuantities.Add(new SelectListItem()
                {
                    Text = qty.ToString(),
                    Value = qty.ToString(),
                    Selected = updatecartitem != null && updatecartitem.Quantity == qty
                });
            }

            #endregion

            #region Gift card

            model.GiftCard.IsGiftCard = product.IsGiftCard;
            if (model.GiftCard.IsGiftCard)
            {
                model.GiftCard.GiftCardType = product.GiftCardType;

                if (updatecartitem == null)
                {
                    model.GiftCard.SenderName = _workContext.CurrentCustomer.GetFullName();
                    model.GiftCard.SenderEmail = _workContext.CurrentCustomer.Email;
                }
                else
                {
                    string giftCardRecipientName, giftCardRecipientEmail, giftCardSenderName, giftCardSenderEmail, giftCardMessage;
                    _productAttributeParser.GetGiftCardAttribute(updatecartitem.AttributesXml,
                        out giftCardRecipientName, out giftCardRecipientEmail,
                        out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                    model.GiftCard.RecipientName = giftCardRecipientName;
                    model.GiftCard.RecipientEmail = giftCardRecipientEmail;
                    model.GiftCard.SenderName = giftCardSenderName;
                    model.GiftCard.SenderEmail = giftCardSenderEmail;
                    model.GiftCard.Message = giftCardMessage;
                }
            }

			#endregion
			#region Product attributes
			//performance optimization
			//We cache a value indicating whether a product has attributes
			IList<ProductAttributeMapping> productAttributeMapping = null;
			string cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_HAS_PRODUCT_ATTRIBUTES_KEY, product.Id);
			var hasProductAttributesCache = _cacheManager.Get<bool?>(cacheKey);
			if (!hasProductAttributesCache.HasValue)
			{
				//no value in the cache yet
				//let's load attributes and cache the result (true/false)
				productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
				hasProductAttributesCache = productAttributeMapping.Count > 0;
				_cacheManager.Set(cacheKey, hasProductAttributesCache, 60);
			}
			if (hasProductAttributesCache.Value && productAttributeMapping == null)
			{
				//cache indicates that the product has attributes
				//let's load them
				productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
			}
			if (productAttributeMapping == null)
			{
				productAttributeMapping = new List<ProductAttributeMapping>();
			}
			foreach (var attribute in productAttributeMapping)
			{
				var attributeModel = new ProductDetailsModel.ProductAttributeModel
				{
					Id = attribute.Id,
					ProductId = product.Id,
					ProductAttributeId = attribute.ProductAttributeId,
					Name = attribute.ProductAttribute.GetLocalized(x => x.Name),
					Description = attribute.ProductAttribute.GetLocalized(x => x.Description),
					TextPrompt = attribute.TextPrompt,
					IsRequired = attribute.IsRequired,
					AttributeControlType = attribute.AttributeControlType,
					DefaultValue = updatecartitem != null ? null : attribute.DefaultValue,
				};
				if (!String.IsNullOrEmpty(attribute.ValidationFileAllowedExtensions))
				{
					attributeModel.AllowedFileExtensions = attribute.ValidationFileAllowedExtensions
						.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
						.ToList();
				}

				if (attribute.ShouldHaveValues())
				{
					//values
					IList<ProductAttributeValue> attributeValues = new List<ProductAttributeValue>();
					if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStockByAttributes)
					{
						IOrderedEnumerable<ProductAttributeValue> attributeCombinations =
							product.ProductAttributeCombinations.Where(c => c.StockQuantity > 0)
								.Select(c => _productAttributeParser.ParseProductAttributeValues(c.AttributesXml).FirstOrDefault())
								.ToList()
								.OrderBy(a => a.Name);
						attributeValues = attributeCombinations.ToList();
					}
					else
					{
						attributeValues = _productAttributeService.GetProductAttributeValues(attribute.Id);
					}

					if (attributeValues.Any())
					{
						foreach (var attributeValue in attributeValues)
						{
							var valueModel = new ProductDetailsModel.ProductAttributeValueModel
							{
								Id = attributeValue.Id,
								Name = attributeValue.GetLocalized(x => x.Name),
								ColorSquaresRgb = attributeValue.ColorSquaresRgb, //used with "Color squares" attribute type
								IsPreSelected = attributeValue.IsPreSelected
							};
							attributeModel.Values.Add(valueModel);

							//display price if allowed
							if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
							{
								decimal taxRate;
								decimal attributeValuePriceAdjustment = _priceCalculationService.GetProductAttributeValuePriceAdjustment(attributeValue);
								decimal priceAdjustmentBase = _taxService.GetProductPrice(product, attributeValuePriceAdjustment, out taxRate);
								decimal priceAdjustment = _currencyService.ConvertFromPrimaryStoreCurrency(priceAdjustmentBase, _workContext.WorkingCurrency);
								if (priceAdjustmentBase > decimal.Zero)
									valueModel.PriceAdjustment = "+" + _priceFormatter.FormatPrice(priceAdjustment, false, false);
								else if (priceAdjustmentBase < decimal.Zero)
									valueModel.PriceAdjustment = "-" + _priceFormatter.FormatPrice(-priceAdjustment, false, false);

								valueModel.PriceAdjustmentValue = priceAdjustment;
							}

							//picture
							var valuePicture = _pictureService.GetPictureById(attributeValue.PictureId);
							if (valuePicture != null)
							{
								valueModel.PictureUrl = _pictureService.GetPictureUrl(valuePicture, defaultPictureSize);
								valueModel.FullSizePictureUrl = _pictureService.GetPictureUrl(valuePicture);
								valueModel.PictureId = valuePicture.Id;
							}
						}
					}
				}

				//set already selected attributes (if we're going to update the existing shopping cart item)
				if (updatecartitem != null)
				{
					switch (attribute.AttributeControlType)
					{
						case AttributeControlType.DropdownList:
						case AttributeControlType.RadioList:
						case AttributeControlType.Checkboxes:
						case AttributeControlType.ColorSquares:
							{
								if (!String.IsNullOrEmpty(updatecartitem.AttributesXml))
								{
									//clear default selection
									foreach (var item in attributeModel.Values)
										item.IsPreSelected = false;

									//select new values
									var selectedValues = _productAttributeParser.ParseProductAttributeValues(updatecartitem.AttributesXml);
									foreach (var attributeValue in selectedValues)
										foreach (var item in attributeModel.Values)
											if (attributeValue.Id == item.Id)
												item.IsPreSelected = true;
								}
							}
							break;
						case AttributeControlType.ReadonlyCheckboxes:
							{
								//do nothing
								//values are already pre-set
							}
							break;
						case AttributeControlType.TextBox:
						case AttributeControlType.MultilineTextbox:
							{
								if (!String.IsNullOrEmpty(updatecartitem.AttributesXml))
								{
									var enteredText = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
									if (enteredText.Count > 0)
										attributeModel.DefaultValue = enteredText[0];
								}
							}
							break;
						case AttributeControlType.Datepicker:
							{
								//keep in mind my that the code below works only in the current culture
								var selectedDateStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
								if (selectedDateStr.Count > 0)
								{
									DateTime selectedDate;
									if (DateTime.TryParseExact(selectedDateStr[0], "D", CultureInfo.CurrentCulture,
														   DateTimeStyles.None, out selectedDate))
									{
										//successfully parsed
										attributeModel.SelectedDay = selectedDate.Day;
										attributeModel.SelectedMonth = selectedDate.Month;
										attributeModel.SelectedYear = selectedDate.Year;
									}
								}

							}
							break;
						default:
							break;
					}
				}

				model.ProductAttributes.Add(attributeModel);
			}

			#endregion

			#region Product specifications

			//do not prepare this model for the associated products. any it's not used
			if (!isAssociatedProduct)
            {
                model.ProductSpecifications = this.PrepareProductSpecificationModel(_workContext,
                    _specificationAttributeService,
                    _cacheManager,
                    product);
            }

            #endregion

            #region Product review overview

            model.ProductReviewOverview = new ProductReviewOverviewModel()
            {
                ProductId = product.Id,
                RatingSum = product.ApprovedRatingSum,
                TotalReviews = product.ApprovedTotalReviews,
                AllowCustomerReviews = product.AllowCustomerReviews
            };

            #endregion

            #region Tier prices

            if (product.HasTierPrices && _permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            {
                model.TierPrices = product.TierPrices
                    .OrderBy(x => x.Quantity)
                    .ToList()
                    .FilterByStore(_storeContext.CurrentStore.Id)
                    .FilterForCustomer(_workContext.CurrentCustomer)
                    .RemoveDuplicatedQuantities()
                    .Select(tierPrice =>
                    {
                        var m = new ProductDetailsModel.TierPriceModel()
                        {
                            Quantity = tierPrice.Quantity,
                        };
                        decimal taxRate = decimal.Zero;
                        decimal priceBase = _taxService.GetProductPrice(product, _priceCalculationService.GetFinalPrice(product, _workContext.CurrentCustomer, decimal.Zero, _catalogSettings.DisplayTierPricesWithDiscounts, tierPrice.Quantity), out taxRate);
                        decimal price = _currencyService.ConvertFromPrimaryStoreCurrency(priceBase, _workContext.WorkingCurrency);
                        m.Price = _priceFormatter.FormatPrice(price, false, false);
                        return m;
                    })
                    .ToList();
            }

            #endregion

            #region Manufacturers

            //do not prepare this model for the associated products. any it's not used
            if (!isAssociatedProduct)
            {
                string manufacturersCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_MANUFACTURERS_MODEL_KEY, product.Id, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
                model.ProductManufacturers = _cacheManager.Get(manufacturersCacheKey, () =>
                {
                    return _manufacturerService.GetProductManufacturersByProductId(product.Id)
                        .Select(x => x.Manufacturer.ToModel())
                        .ToList();
                });
            }
            #endregion

            #region Associated products

            if (product.ProductType == ProductType.GroupedProduct)
            {
                //ensure no circular references
                if (!isAssociatedProduct)
                {
                    var associatedProducts = _productService.GetAssociatedProducts(product.Id, _storeContext.CurrentStore.Id);
                    foreach (var associatedProduct in associatedProducts)
                        model.AssociatedProducts.Add(PrepareProductDetailsPageModel(associatedProduct, null, true));
                }
            }

            #endregion

            return model;
        }
		#endregion

		#region Client Side Methods

		#region Add Button
		[NopHttpsRequirement(SslRequirement.No)]
        public ActionResult AddButton()
        {

            var settingsModel = new BsQuickViewSettingsModel();
            var quickViewsettings = _settingService.LoadSetting<QuickViewSettings>();
            settingsModel.ButtonContainerName = quickViewsettings.ButtonContainerName;
            settingsModel.EnableWidget = quickViewsettings.EnableWidget;
            settingsModel.ShowAlsoPurchased = quickViewsettings.ShowAlsoPurchased;
            settingsModel.ShowRelatedProducts = quickViewsettings.ShowRelatedProducts;
            settingsModel.EnableEnlargePicture = quickViewsettings.EnableEnlargePicture;
            

            return View("QuickViewButton",settingsModel);


        }

        #endregion

        #region Product details page

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ProductDetails(int productId, int updatecartitemid = 0)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                return InvokeHttp404();

            //Is published?
            //Check whether the current user has a "Manage catalog" permission
            //It allows him to preview a product before publishing
            if (!product.Published && !_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return InvokeHttp404();

            //ACL (access control list)
            if (!_aclService.Authorize(product))
                return InvokeHttp404();

            //Store mapping
            if (!_storeMappingService.Authorize(product))
                return InvokeHttp404();

            //visible individually?
            if (!product.VisibleIndividually)
            {
                //is this one an associated products?
                var parentGroupedProduct = _productService.GetProductById(product.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    return RedirectToRoute("Product", new { SeName = parentGroupedProduct.GetSeName() });
                }
                else
                {
                    return RedirectToRoute("HomePage");
                }
            }

            //update existing shopping cart item?
            ShoppingCartItem updatecartitem = null;
            if (_shoppingCartSettings.AllowCartItemEditing && updatecartitemid > 0)
            {
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                updatecartitem = cart.FirstOrDefault(x => x.Id == updatecartitemid);
                //not found?
                if (updatecartitem == null)
                {
                    return RedirectToRoute("Product", new { SeName = product.GetSeName() });
                }
                //is it this product?
                if (product.Id != updatecartitem.ProductId)
                {
                    return RedirectToRoute("Product", new { SeName = product.GetSeName() });
                }
            }

            //prepare the model
            var model = PrepareProductDetailsPageModel(product, updatecartitem, false);

            //save as recently viewed
            _recentlyViewedProductsService.AddProductToRecentlyViewedList(product.Id);

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewProduct", _localizationService.GetResource("ActivityLog.PublicStore.ViewProduct"), product.Name);






            var bsQuickViewModel = new BsQuickViewModel();
            bsQuickViewModel.ProductDetailsModel = model;

            var settingsModel = new BsQuickViewSettingsModel();
            var quickViewsettings = _settingService.LoadSetting<QuickViewSettings>();
            settingsModel.ButtonContainerName = quickViewsettings.ButtonContainerName;
            settingsModel.EnableWidget = quickViewsettings.EnableWidget;
            settingsModel.ShowAlsoPurchased = quickViewsettings.ShowAlsoPurchased;
            settingsModel.ShowRelatedProducts = quickViewsettings.ShowRelatedProducts;
            settingsModel.EnableEnlargePicture = quickViewsettings.EnableEnlargePicture;
            bsQuickViewModel.BsQuickViewSettingsModel = settingsModel;






            /*return View(model.ProductTemplateViewPath, model);*/
            if (model.ProductTemplateViewPath.Equals("ProductTemplate.Simple"))
            {
                return View("QuickViewProductTemplate.Simple", bsQuickViewModel);
            }
            else if (model.ProductTemplateViewPath.Equals("ProductTemplate.Grouped"))
            {
                return View("QuickViewProductTemplate.Grouped", bsQuickViewModel);
            }
            return null;
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(int productId, int? productThumbPictureSize)
        {
            //load and cache report
            var productIds = _cacheManager.Get(string.Format(ModelCacheEventConsumer.PRODUCTS_RELATED_IDS_KEY, productId, _storeContext.CurrentStore.Id),
                () =>
                    _productService.GetRelatedProductsByProductId1(productId).Select(x => x.ProductId2).ToArray()
                    );

            //load products
            var products = _productService.GetProductsByIds(productIds);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            if (products.Count == 0)
                return Content("");

            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();
            return PartialView("QuickViewRelatedProducts", model);
        }

        [ChildActionOnly]
        public ActionResult ProductsAlsoPurchased(int productId, int? productThumbPictureSize)
        {
            if (!_catalogSettings.ProductsAlsoPurchasedEnabled)
                return Content("");

            //load and cache report
            var productIds = _cacheManager.Get(string.Format(ModelCacheEventConsumer.PRODUCTS_ALSO_PURCHASED_IDS_KEY, productId, _storeContext.CurrentStore.Id),
                () =>
                    _orderReportService
                    .GetAlsoPurchasedProductsIds(_storeContext.CurrentStore.Id, productId, _catalogSettings.ProductsAlsoPurchasedNumber)
                    );

            //load products
            var products = _productService.GetProductsByIds(productIds);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            if (products.Count == 0)
                return Content("");

            //prepare model
            var model = PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

            return PartialView("QuickViewProductsAlsoPurchased", model);
        }

        [ChildActionOnly]
        public ActionResult CrossSellProducts(int? productThumbPictureSize)
        {
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();

            var products = _productService.GetCrosssellProductsByShoppingCart(cart, _shoppingCartSettings.CrossSellsNumber);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            if (products.Count == 0)
                return Content("");


            //Cross-sell products are dispalyed on the shopping cart page.
            //We know that the entire shopping cart page is not refresh
            //even if "ShoppingCartSettings.DisplayCartAfterAddingProduct" setting  is enabled.
            //That's why we force page refresh (redirect) in this case
            var model = PrepareProductOverviewModels(products,
                productThumbPictureSize: productThumbPictureSize, forceRedirectionAfterAddingToCart: true)
                .ToList();

            return PartialView(model);
        }

        #endregion

        #endregion

        #region Admin Panel Methods

        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult BsQuickViewConfigure()
        {
            return View("BSQuickViewConfigure");
        }



        #endregion
    }
}