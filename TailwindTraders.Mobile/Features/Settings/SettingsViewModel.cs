using System;
using TailwindTraders.Mobile.Features.Localization;
using TailwindTraders.Mobile.Framework;
using Xamarin.Forms;

namespace TailwindTraders.Mobile.Features.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        private string rootApiUrl;

        public string RootApiUrl
        {
            get => rootApiUrl;
            set => SetAndRaisePropertyChangedIfDifferentValues(ref rootApiUrl, value);
        }

        string storageAccountName;
        public string StorageAccountName
        {
            get => storageAccountName;
            set => SetAndRaisePropertyChangedIfDifferentValues(ref storageAccountName, value);
        }

        string functionAppUrl;
        public string FunctionAppUrl
        {
            get => functionAppUrl;
            set => SetAndRaisePropertyChangedIfDifferentValues(ref functionAppUrl, value);
        }

        public Command SaveCommand { get; }

        public SettingsViewModel()
        {
            rootApiUrl = DefaultSettings.ProductsApiUrl;
            storageAccountName = DefaultSettings.StorageAccountName;
            functionAppUrl = DefaultSettings.FunctionAppUrl;

            SaveCommand = new Command(Save);
        }

        private void Save()
        {
            if (!Uri.IsWellFormedUriString(RootApiUrl, UriKind.Absolute))
            {
                XSnackService.ShowMessage(Resources.Snack_Message_InvalidAbsoluteURL);
                return;
            }

            if (!Uri.IsWellFormedUriString(FunctionAppUrl, UriKind.Absolute))
            {
                XSnackService.ShowMessage("Function App URL incorrect");
                return;
            }

            DefaultSettings.ProductsApiUrl = RootApiUrl;
            DefaultSettings.StorageAccountName = StorageAccountName;
            DefaultSettings.FunctionAppUrl = FunctionAppUrl;

            RestPoolService.UpdateApiUrl(rootApiUrl);

            XSnackService.ShowMessage(Resources.Snack_Message_SettingsSaved);

            var msg = new ProductsServiceUrlMessage() { 
                ProductServiceUrl = rootApiUrl,
                FunctionsAppUrl = functionAppUrl,
                StorageAccoutName = storageAccountName
            };
            MessagingCenter.Send(msg, ProductsServiceUrlMessage.MessageName);
        }
    }
}
