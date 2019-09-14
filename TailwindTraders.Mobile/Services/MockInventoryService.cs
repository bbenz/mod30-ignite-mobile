using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using TailwindTraders.Mobile.Services;

using Xamarin.Forms;

[assembly: Dependency(typeof(MockInventoryService))]
namespace TailwindTraders.Mobile.Services
{
    public class MockInventoryService : IInventoryService
    {
        public async Task<long> DecrementInventory(string sku)
        {
            return await Task.FromResult(1); 
        }

        public async Task<long> GetCurrentInventory(string sku)
        {
            return await Task.FromResult(1);
        }

        public async Task<long> IncrementInventory(string sku)
        {
            return await Task.FromResult(1);
        }
    }
}
