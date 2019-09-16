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
using Xamarin.Forms;

namespace TailwindTraders.Mobile.Services
{
    public class StorageService : IStorageService
    {
        IMod30FunctionsAPI functionsApi;
        string functionUrl = "https://mod30back-app.azurewebsites.net/api";
        string storageAccountName = "mod30backdemostorage";

        public StorageService()
        {
            functionsApi = RestService.For<IMod30FunctionsAPI>(HttpClientFactory.Create(functionUrl));

            MessagingCenter.Subscribe<ProductsServiceUrlMessage>(this, ProductsServiceUrlMessage.MessageName,
                (msg) => {
                    storageAccountName = msg.StorageAccoutName;

                    functionsApi = RestService.For<IMod30FunctionsAPI>(HttpClientFactory.Create(msg.FunctionsAppUrl));
                });
        }
        public async Task<string> GetSharedAccessSignature()
        {
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                var sas = await client.GetStringAsync("https://mod30back-app.azurewebsites.net/api/getsastoken");

                //var sas = await ret.Content.ReadAsStringAsync();

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

        public async Task<bool> UploadPhoto(Stream photo, string sharedAccessSignature)
        {
            try
            {                
                var creds = new Microsoft.Azure.Storage.Auth.StorageCredentials(sharedAccessSignature);

                var account = new CloudStorageAccount(creds, storageAccountName, "core.windows.net", true);

                var blobClient = account.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference("wishlist");

                var blockBlob = container.GetBlockBlobReference($"TT-{Guid.NewGuid()}.jpg");

                await blockBlob.UploadFromStreamAsync(photo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }

            return true;
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
