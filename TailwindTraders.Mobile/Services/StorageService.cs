using Microsoft.Azure.Storage;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TailwindTraders.Mobile.Features.Common;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using TailwindTraders.Mobile.Helpers;

namespace TailwindTraders.Mobile.Services
{
    public class StorageService : IStorageService
    {
        IMod30FunctionsAPI functionsApi;
        string functionUrl = "https://mod30back-app.azurewebsites.net/api";

        public StorageService()
        {
            functionsApi = RestService.For<IMod30FunctionsAPI>(HttpClientFactory.Create(functionUrl));
        }
        public async Task<string> GetSharedAccessSignature()
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                var ret = await client.GetAsync("https://mod30back-app.azurewebsites.net/api/getsastoken");

                var sas = await ret.Content.ReadAsStringAsync();

                return sas;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return "";
        }

        public async Task<IEnumerable<WishlistItem>> GetWishlistItems()
        {
            return await functionsApi.GetWishlistItems();
        }

        public Task<IEnumerable<WishlistItem>> GetWishlistItemsWithCaptions()
        {
            throw new NotImplementedException();
        }

        public async Task UploadPhoto(Stream photo, string sharedAccessSignature)
        {
            //var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mod30backdemostorage;AccountKey=aELn/4qCCnyMgOepniTEjJOMz0YrrSEvGbovEwuOZfbJUdHyNLadiw0ij+r7qIVv4Ah2VVMp0x//uMCva/LVGw==;EndpointSuffix=core.windows.net");

            var creds = new Microsoft.Azure.Storage.Auth.StorageCredentials(sharedAccessSignature);

            var account = new CloudStorageAccount(creds, true);

            var blobClient = account.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference("wishlist");

            var blockBlob = container.GetBlockBlobReference($"TT-{Guid.NewGuid()}.jpg");

            await blockBlob.UploadFromStreamAsync(photo);
        }
    }

    public interface IMod30FunctionsAPI
    {
        [Get("/getwishlist")]
        Task<IEnumerable<WishlistItem>> GetWishlistItems();

        [Get("/getsastoken")]
        Task<string> GetSasToken();
    }
}
