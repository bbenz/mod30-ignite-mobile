using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TailwindTraders.Mobile.Features.LogIn;
using TailwindTraders.Mobile.Features.Product;
using TailwindTraders.Mobile.Features.Product.Category;
using TailwindTraders.Mobile.Features.Scanning.AR;
using TailwindTraders.Mobile.Features.Scanning.Photo;
using TailwindTraders.Mobile.Framework;
using TailwindTraders.Mobile.Helpers;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using TailwindTraders.Mobile.Features.Settings;
using Microsoft.AppCenter.Crashes;

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

using TailwindTraders.Mobile.Models;
using Plugin.Media;
using TailwindTraders.Mobile.Features.Shell;
using Plugin.Media.Abstractions;
using Microsoft.Azure.Storage.Auth;
using TailwindTraders.Mobile.Services;

namespace TailwindTraders.Mobile.Features.Home
{
    public class HomeViewModel : BaseStateAwareViewModel<HomeViewModel.State>
    {
        public enum State
        {
            EverythingOK,
            Error,
        }

        private IEnumerable<Tuple<string, string, AsyncCommand>> recommendedProducts;
        private IEnumerable<ProductViewModel> popularProducts;
        private IEnumerable<Models.Product> previouslySeenProducts;

        public HomeViewModel()
        {
            IsBusy = true;

            MessagingCenter.Subscribe<LoginViewModel>(
                this,
                LoginViewModel.LogInFinishedMessage,
                _ => LoadCommand.Execute(null));
        }

        public bool IsNoOneLoggedIn => !AuthenticationService.IsAnyOneLoggedIn;

        public IEnumerable<Tuple<string, string, AsyncCommand>> RecommendedProducts
        {
            get => recommendedProducts;
            set => SetAndRaisePropertyChanged(ref recommendedProducts, value);
        }

        public IEnumerable<ProductViewModel> PopularProducts
        {
            get => popularProducts;
            set => SetAndRaisePropertyChanged(ref popularProducts, value);
        }

        public IEnumerable<Models.Product> PreviouslySeenProducts
        {
            get => previouslySeenProducts;
            set => SetAndRaisePropertyChanged(ref previouslySeenProducts, value);
        }

        public ICommand PhotoCommand => new Command(async() => await TakeOrPickPhoto());

        public ICommand ARCommand => new AsyncCommand(_ => App.NavigateToAsync(new CameraPreviewPage()));

        public ICommand LoadCommand => new AsyncCommand(_ => LoadDataAsync());

        public override async Task InitializeAsync()
        {
            try
            {
                await base.InitializeAsync();

                await AuthenticationService.RefreshSessionAsync();

                if (IsNoOneLoggedIn)
                {
                    await App.NavigateModallyToAsync(new LogInPage());
                    IsBusy = false;
                }
                else
                {
                    await LoadDataAsync();
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override async Task UninitializeAsync()
        {
            await base.UninitializeAsync();
        }

        private async Task LoadDataAsync()
        {
            CurrentState = State.EverythingOK;

            RecommendedProducts = new List<Tuple<string, string, AsyncCommand>>
            {
                Tuple.Create("Power Tools", "recommended_powertools.jpg",
                    new AsyncCommand(_ =>
                    {
                        return App.NavigateToAsync(new ProductCategoryPage(DefaultSettings.Hardware), closeFlyout: false);
                    }
                )),
                Tuple.Create("Electrical", "recommended_lighting.jpg",
                    new AsyncCommand(_ =>
                    {
                        return App.NavigateToAsync(new ProductCategoryPage(DefaultSettings.Home), closeFlyout: false);
                    }
                )),
                Tuple.Create("Plumbing", "recommended_bathrooms.jpg",
                    new AsyncCommand(_ =>
                    {
                        return App.NavigateToAsync(new ProductCategoryPage(DefaultSettings.Plumbing), closeFlyout: false);
                    }
                )),
                Tuple.Create("Garden Center", "recommended_plants.jpg",
                    new AsyncCommand(_ =>
                    {                       
                        return App.NavigateToAsync(new ProductCategoryPage(DefaultSettings.GardenCenter), closeFlyout: false);
                    }
                ))
            };

            var productSvc = DependencyService.Get<IProductService>();

            var homeResult = await TryExecuteWithLoadingIndicatorsAsync(
                productSvc.GetProductsPerCategory("homeappliances")
            );

            if (homeResult.IsError || homeResult.Value == null || homeResult.Value.Products == null)
            {
                CurrentState = State.Error;
                return;
            }

            var popularProductsRaw = homeResult.Value.Products;

            var popularProductsWithCommand = popularProductsRaw.Shuffle().Take(3).Select(
                item => new ProductViewModel(item, FeatureNotAvailableCommand)
            );

            PopularProducts = new List<ProductViewModel>(popularProductsWithCommand);

            var randomProducts = popularProductsRaw.Shuffle().Take(3);
            PreviouslySeenProducts = new List<Models.Product>(randomProducts);
        }
    
        async Task TakeOrPickPhoto()
        {
            //await CrossMedia.Current.Initialize();

            //var action = await Xamarin.Forms.Shell.Current.DisplayActionSheet("Take or Pick photo", "Cancel", "", "Take", "Pick");

            //if (action == "Take")
            //    await TakePhoto();
            //else if (action == "Pick")
            //    await PickPhoto();

            await PickPhoto();
        }

        async Task TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Xamarin.Forms.Shell.Current.DisplayAlert("No Camera", "Taking photos not supported", "OK");
                return;
            }

            var options = new StoreCameraMediaOptions { CompressionQuality = 50, PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large };
            var photo = await CrossMedia.Current.TakePhotoAsync(options);

            if (photo == null)
            {
                await XSnackService.ShowMessageAsync("The taken photo was not sent");
                return;
            }

            IsBusy = true;

            var storage = new StorageService();
            var sas = await storage.GetSharedAccessSignature();

            if (string.IsNullOrEmpty(sas))
            {
                await XSnackService.ShowMessageAsync("There was an error uploading your photo");
                IsBusy = false;
                return;
            }

            var uploadSuccess = await storage.UploadPhoto(photo.GetStream(), sas);

            IsBusy = false;

            if (uploadSuccess)
                await XSnackService.ShowMessageAsync("Photo uploaded to wishlist");
            else
                await XSnackService.ShowMessageAsync("There was an error uploading your photo");
        }

        async Task PickPhoto()
        {
            var s = new StorageService();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Xamarin.Forms.Shell.Current.DisplayAlert("No Photos", "Photo picking not supported", "OK");
                return;
            }

            var options = new PickMediaOptions { CompressionQuality = 50, PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large };
            var photo = await CrossMedia.Current.PickPhotoAsync(options);

            if (photo == null)
                return;

            IsBusy = true;

            var storage = new StorageService();

            var sas = await storage.GetSharedAccessSignature();

            if (string.IsNullOrEmpty(sas))
            {
                await XSnackService.ShowMessageAsync("There was an error uploading your photo");
                IsBusy = false;
                return;
            }

            var uploadSuccess = await storage.UploadPhoto(photo.GetStream(), sas);

            IsBusy = false;

            if (uploadSuccess)
                await XSnackService.ShowMessageAsync("Photo uploaded to wishlist");
            else
                await XSnackService.ShowMessageAsync("There was an error uploading your photo");
        }
    }
}
