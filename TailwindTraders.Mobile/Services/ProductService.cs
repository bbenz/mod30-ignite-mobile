using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TailwindTraders.Mobile.Features.Product;
using MonkeyCache.SQLite;
using TailwindTraders.Mobile.Framework;
using Xamarin.Forms;
using Refit;
using TailwindTraders.Mobile.Features.Common;
using TailwindTraders.Mobile;
using System.Linq;
using TailwindTraders.Mobile.Helpers;
using TailwindTraders.Mobile.Models;
using TailwindTraders.Mobile.Features.Settings;

[assembly: Dependency(typeof(ProductService))]
namespace TailwindTraders.Mobile
{
    public class ProductService : IProductService
    {
        readonly IConnectivityService connectivityService;
        IProductsWebAPI webAPI = null;
        readonly string allProductsKey = "allProducts";

        string productsBaseUrl = string.Empty;// "https://tailwindtraders-websitecd95.azurewebsites.net/api/v1";

        RefitSettings productRefitSettings;

        public ProductService()
        {
            connectivityService = DependencyService.Get<IConnectivityService>();

            productsBaseUrl = DefaultSettings.ProductsApiUrl;

            productRefitSettings = new RefitSettings { ContentSerializer = new JsonContentSerializer(ProductPerDTOJsonConverter.Settings) };                        
        }

        public async Task<ProductList> GetProductsAsync()
        {
            webAPI = null;

            webAPI = RestService.For<IProductsWebAPI>(
                HttpClientFactory.Create(productsBaseUrl), productRefitSettings
            );

            var allProducts = await webAPI.GetProductsAsync();

            return allProducts;
        }

        public async Task<Product> GetProductDetailAsync(long productId)
        {
            webAPI = null;

            webAPI = RestService.For<IProductsWebAPI>(
                HttpClientFactory.Create(productsBaseUrl), productRefitSettings
            );

            var product = await webAPI.GetDetailAsync(productId);

            return product;
        }

        public async Task<ProductList> GetProductsPerCategory(string category)
        {
            webAPI = RestService.For<IProductsWebAPI>(
                HttpClientFactory.Create(productsBaseUrl), productRefitSettings
            );

            var categoryProducts = await webAPI.GetProductsByCategoryAsync(category);

            return categoryProducts;
        }
    }

    public interface IProductsWebAPI
    {
        [Get("/products/{id}")]
        Task<Product> GetDetailAsync(long id);

        [Get("/products")]
        Task<ProductList> GetProductsAsync();

        [Get("/products")]
        Task<ProductList> GetProductsByCategoryAsync([AliasAs("type")]string category);
    }
}
