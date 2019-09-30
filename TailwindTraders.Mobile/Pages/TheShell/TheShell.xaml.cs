using System;
using System.Threading.Tasks;

using Microsoft.AppCenter.Crashes;

namespace TailwindTraders.Mobile.Features.Shell
{
    public partial class TheShell
    {
        public static readonly TimeSpan TimeFlyoutCloses = TimeSpan.FromSeconds(0.5f);

        public TheShell()
        {
            InitializeComponent();

            BindingContext = new TheShellViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var didCrash = await Crashes.HasCrashedInLastSessionAsync();

            if (didCrash)
            {
                var crashReport = await Crashes.GetLastSessionCrashReportAsync();

                Crashes.TrackError(crashReport.Exception);
            }
        }

        internal async Task CloseFlyoutAsync()
        {
            FlyoutIsPresented = false;
            await Task.Delay(TimeFlyoutCloses);
        }
    }
}
