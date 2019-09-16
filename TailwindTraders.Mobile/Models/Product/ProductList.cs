using System;
using System.Collections.Generic;
using System.Text;

namespace TailwindTraders.Mobile.Models
{
    public class ProductList
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductBrand> Brands { get; set; }
        public IEnumerable<ProductType> Types { get; set; }

    }
}
