using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TailwindTraders.Mobile.Features.Product;
using TailwindTraders.Mobile.Models;

namespace TailwindTraders.Mobile
{
    public interface IProductService
    {
        Task<ProductList> GetProductsAsync();
        Task<Product> GetProductDetailAsync(long productId);
        Task<ProductList> GetProductsPerCategory(string category);
    }
}
