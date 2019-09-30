
using TailwindTraders.Mobile.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace TailwindTraders.Mobile.Features.Settings
{
    public static class DefaultSettings
    {
        public static string Electrical = "Electrical";
        public static string Home = "home";
        public static string Hardware = "diytools";
        public static string Tiles = "sink";
        public static string Hinges = "Hinges";
        public static string GardenCenter = "gardening";
        public static string Plumbing = "kitchen";

        public const string ApiAuthorizationHeader = "Authorization";

        public static string AccessToken = string.Empty;

        public const string AppCenterAndroidSecret = "d34ada1e-794f-407b-bd1d-12737ed0656c";

        public const string AppCenteriOSSecret = "2751b6e7-870e-402e-be82-699c575f48d9";

        public const bool UseFakeAPIs = false;

        public const bool UseFakeAuthentication = DebugMode;

        public const bool ForceAutomaticLogin = false;

        public const bool BreakNetworkRandomly = false;

        public const bool AndroidDebuggable = DebugMode;

        public const bool UseDebugLogging = DebugMode;

        public const bool EnableARDiagnostics = DebugMode;

        public const bool DebugMode =
#if DEBUG 
            true;
#else
            false;
#endif

        //public static string ProductsApiUrl { get; set; } = "https://tailwindtraders-websitecd95.azurewebsites.net/api/v1";
        //public static string StorageAccountName { get; set; } = "mod30backdemostorage";
        //public static string FunctionAppUrl { get; set; } = "https://mod30back-app.azurewebsites.net/api";
        public static string RootProductsWebApiUrl { get; set; } = "https://bemaproduct.azurewebsites.net/api";


        public static string TestUrl
        {
            get => Xamarin.Essentials.Preferences.Get("testurl", "");
            set => Preferences.Set("testurl", value);
        }

        const string prodapikey = "productsapikey";

        public static string ProductsApiUrl
        {
            get => Preferences.Get(prodapikey, "https://tailwindtraders-websitecd95.azurewebsites.net/api/v1");
            set => Preferences.Set(prodapikey, value);
        }

        const string functionapikey = "funcapikey";
        public static string FunctionAppUrl
        {
            get => Preferences.Get(functionapikey, "https://mod30back-app.azurewebsites.net/api");
            set => Preferences.Set(functionapikey, value);
        }

        const string storagekey = "storagekey";
        public static string StorageAccountName
        {
            get => Preferences.Get(storagekey, "mod30backdemostorage");
            set => Preferences.Set(storagekey, value);
        }
    }
}
