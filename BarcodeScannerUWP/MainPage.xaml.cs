using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using BarcodeScannerUWP.ViewModel;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ZXing.Mobile;

namespace BarcodeScannerUWP
{
	public sealed partial class MainPage
	{
		public MainViewModel Vm => (MainViewModel)DataContext;

		public MainPage()
		{
			InitializeComponent();
			ZXing.Net.Mobile.Forms.WindowsUniversal.ZXingScannerViewRenderer.Init();
		}

		private void SystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
		{
			if (Frame.CanGoBack)
			{
				e.Handled = true;
				Frame.GoBack();
			}
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			base.OnNavigatingFrom(e);
		}

		private async void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			//scanner.TopText = "Hold camera up to barcode";
			//scanner.BottomText = "Camera will automatically scan barcode\r\n\r\nPress the 'Back' button to Cancel";
			//var result = await scanner.Scan(new MobileBarcodeScanningOptions()
			//{
			//	TryHarder = true,
			//});
			//Vm.Barcode = result.Text;
			ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(ViewModelLocator.ScannerPage);
		}
	}
}
