using BarcodeScannner.Common;
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
using BarcodeScannner.ViewModel;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BarcodeScannner
{
	/// <summary>
	/// A basic page that provides characteristics common to most applications.
	/// </summary>
	public sealed partial class ScannerPage : Page
	{


		public ScannerPage()
		{
			this.InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Disabled;
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(ViewModelLocator.MainPage);
		}
	}
}
