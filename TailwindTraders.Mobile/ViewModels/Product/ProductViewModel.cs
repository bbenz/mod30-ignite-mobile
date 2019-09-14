using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.AppCenter.Analytics;
using TailwindTraders.Mobile.Models;

namespace TailwindTraders.Mobile.Features.Product
{
    public class ProductViewModel
    {
        public ProductViewModel(Models.Product product, ICommand command)
        {
            Product = product;
            Command = command;
        }

        public Models.Product Product { get; }

        public ICommand Command { get; }
    }
}
