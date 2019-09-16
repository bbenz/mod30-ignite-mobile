using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TailwindTraders.Mobile.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TailwindTraders.Mobile
{
    public class WishlistViewModel : BaseViewModel
    {
        IStorageService storageService;

        ObservableCollection<WishlistItem> wishlistitems;
        public ObservableCollection<WishlistItem> WishListItems {
            get => wishlistitems;
            set => SetProperty(ref wishlistitems, value);
        }

        public WishlistViewModel()
        {
            storageService = new StorageService();

            RefreshWishlistCommand = new Command(async () => await ExecuteRefreshWishlistCommand());

            WishListItems = new ObservableCollection<WishlistItem>();
        }

        public ICommand RefreshWishlistCommand { get; }

        public async Task ExecuteRefreshWishlistCommand()
        {
            var returnedItems = await storageService.GetWishlistItems();

            WishListItems = new ObservableCollection<WishlistItem>(returnedItems);
        }
    }
}
