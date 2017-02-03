using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using BarcodeScannner.Common;
using BarcodeScannner.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Views;
using ZXing;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarcodeScannner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();

        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += SettingsCommandsRequested;
        }

        private async void SettingsCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var privacyStatement = new SettingsCommand("privacy", "Privacy Statement", x => Launcher.LaunchUriAsync(new Uri("http://wbsimms.com/Privacy")));
            args.Request.ApplicationCommands.Clear();
            args.Request.ApplicationCommands.Add(privacyStatement);
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void GetBarcode_Click(object sender, RoutedEventArgs e)
        {
			ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(ViewModelLocator.ScannerPage);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dp = new DataPackage();
            if (!string.IsNullOrEmpty(BarcodeResult.Text))
            {
                dp.SetText(BarcodeResult.Text);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
            }
        }

        private void Search(object sender, PointerRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BarcodeResult.Text)) return;
            SearchPane.GetForCurrentView().Show(BarcodeResult.Text);
        }
    }
}
