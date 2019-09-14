using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TailwindTraders.Mobile.Services
{
    public interface IStorageService
    {
        Task UploadPhoto(Stream photo, string sharedAccessSignature);

        Task<string> GetSharedAccessSignature();

        Task<IEnumerable<WishlistItem>> GetWishlistItems();

        Task<IEnumerable<WishlistItem>> GetWishlistItemsWithCaptions();
    }
}
