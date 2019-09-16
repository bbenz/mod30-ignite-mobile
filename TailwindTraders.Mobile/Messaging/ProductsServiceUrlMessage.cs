using System;
using System.Collections.Generic;
using System.Text;

namespace TailwindTraders.Mobile
{
    public class ProductsServiceUrlMessage
    {
        public const string MessageName = "productserviceurlchanged";

        public string ProductServiceUrl { get; set; }
        public string StorageAccoutName { get; set; }
        public string FunctionsAppUrl { get; set; }
    }
}
