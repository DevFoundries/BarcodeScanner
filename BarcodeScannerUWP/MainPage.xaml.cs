using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
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
			this.SizeChanged += MainPage_SizeChanged;
		}

		private void MainPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{

			foreach (var item in this.BarcodeListView.Items)
			{
				ListViewItem control = this.BarcodeListView.ContainerFromItem(item) as ListViewItem;
				Grid g = control?.ContentTemplateRoot as Grid;
				if (g == null) continue;
				g.Width = this.ActualWidth - 20;
			}
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

		private void RemoveEntry(object sender, TappedRoutedEventArgs e)
		{
		    Microsoft.HockeyApp.HockeyClient.Current.TrackEvent("Entry Removed");
            var b = sender as Button;
			if (b == null) return;
			var id = Int32.Parse(b.CommandParameter.ToString());
			//var stackPanel = b.Parent as StackPanel;

			//this.BarcodeListView.Items.RemoveAt(id);
			Vm.RemoveCommand.Execute(id);
	//		this.BarcodeListView.ItemsSource = Vm.BarcodeData;
		}
	}
}
