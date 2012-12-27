using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using BarcodeScannner.Common;
using Windows.ApplicationModel.DataTransfer;
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
using ZXing;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BarcodeScannner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += SettingsCommandsRequested;
        }

        private async void SettingsCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var privacyStatement = new SettingsCommand("privacy", "Privacy Statement", x => Launcher.LaunchUriAsync(new Uri("http://wbsimms.com/Privacy/Privacy.html")));
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

        private async void GetBarcode_Click(object sender, RoutedEventArgs e)
        {
            string accountKey = "Ubm9A0rJBhbiVmF0EiS4NH2roW8qctxJ4TRj8m1cE20=";

            CameraCaptureUI camera = new CameraCaptureUI();
//            camera.PhotoSettings.CroppedAspectRatio = new Size(16, 9);
            StorageFile file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                var prop = await file.Properties.GetImagePropertiesAsync();
                var stream = await file.OpenReadAsync();
                var image = new WriteableBitmap((int)prop.Width, (int)prop.Height);
                image.SetSource(stream);
                ZXing.BarcodeReader reader = new BarcodeReader();
                reader.TryHarder = true;
                reader.PureBarcode = true;

                var decodedResult = reader.Decode(image);
                if (decodedResult != null)
                {
                    BarcodeResult.Text = decodedResult.Text;
                }
                else
                {
                    Windows.UI.Popups.MessageDialog d =
                        new MessageDialog(
                            "Unable to read barcode. Please try again.\r\nTips\r\n• Ensure the image is in focus\r\n• Crop the image to only the barcode");
                    IUICommand cmd = await d.ShowAsync();
                }
            }

        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dp = new DataPackage();
            dp.SetText(BarcodeResult.Text);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
        }
    }
}
