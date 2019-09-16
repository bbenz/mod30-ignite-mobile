using System;
using System.Collections.Generic;
using System.Text;

namespace TailwindTraders.Mobile.Models
{
    public class Product
    {
        const string ImagePath = "https://ttstorageucrqili3hgqvk.blob.core.windows.net/product-detail/";
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string RevisedImageUrl
        {
            get
            {
                return ImageUrl;
            }
        }
        public ProductBrand Brand { get; set; }
        public ProductType Type { get; set; }
        public IEnumerable<ProductFeature> Features { get; set; }
        public int StockUnits { get; set; } = 100;
    }
}
