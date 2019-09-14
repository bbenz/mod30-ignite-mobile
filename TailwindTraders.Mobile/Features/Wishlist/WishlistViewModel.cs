using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TailwindTraders.Mobile
{
    public class WishlistViewModel : BaseViewModel
    {
        public ObservableRangeCollection<WishlistItem> WishListItems { get; set; }

        public WishlistViewModel()
        {
            WishListItems = new ObservableRangeCollection<WishlistItem>
            {
                new WishlistItem{ Description="asdf", ThumbnailImageUrl="https://mod30backdemostorage.blob.core.windows.net/wishlist/TT-09b2354f-981f-4222-b7c6-8e59331eb96c.jpg"},
                new WishlistItem{ Description="asdf", ThumbnailImageUrl="https://mod30backdemostorage.blob.core.windows.net/wishlist/TT-9d7618cd-716e-40ff-be21-94190f5e966b.jpg"}
            };
        }
    }
}
