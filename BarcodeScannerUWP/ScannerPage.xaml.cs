using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BarcodeScannerUWP.Model;
using BarcodeScannerUWP.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ZXing.Mobile;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BarcodeScannerUWP
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ScannerPage : Page
	{
		private UIElement overlay;

		public ScannerPage()
		{
			this.InitializeComponent();
			ZXing.Net.Mobile.Forms.WindowsUniversal.ZXingScannerViewRenderer.Init();
			StartScan();
		}

		private async void StartScan()
		{
			if (overlay == null)
			{
				overlay = this.customOverlay.Children[0];
				this.customOverlay.Children.RemoveAt(0);
			}

			MobileBarcodeScanner scanner = new MobileBarcodeScanner();

			scanner.UseCustomOverlay = true;
			scanner.CustomOverlay = overlay;

			this.buttonCancel.Tapped += (sender, args) =>
			{
				ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(ViewModelLocator.MainPage);
			};

			await scanner.Scan().ContinueWith(t =>
			{
				DispatcherHelper.CheckBeginInvokeOnUI(() =>
					ServiceLocator.Current.GetInstance<IDialogService>().ShowMessage(t.Result.Text, "Success","Ok", () =>
					{
						ServiceLocator.Current.GetInstance<MainViewModel>().AddBarcodeData(new BarcodeData(){Barcode = t.Result.Text});
						ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(ViewModelLocator.MainPage);
					})
				)
				;
			});
		}
	}
}
