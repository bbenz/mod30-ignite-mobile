using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TailwindTraders.Mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(MockShoppingCartService))]
namespace TailwindTraders.Mobile.Services
{
    public class MockShoppingCartService : IShoppingCartService
    {
        public async Task AddItemToCart(string userId, BareShoppingCartItemInfo item)
        {
            await Task.CompletedTask;
        }

        public async Task DeleteItemFromCart(string userId, BareShoppingCartItemInfo item)
        {
            await Task.CompletedTask;
        }

        public async Task<List<ShoppingCartItem>> GetCartItemsForUser(string userId)
        {
            return await Task.FromResult(new List<ShoppingCartItem>());
        }

        public async Task<bool> PurchaseCart(string userId)
        {
            return await Task.FromResult(true);
        }
    }
}
