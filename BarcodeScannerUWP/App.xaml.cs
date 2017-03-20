using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BarcodeScannerUWP.Model;
using BarcodeScannerUWP.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.HockeyApp;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Application = Xamarin.Forms.Application;

namespace BarcodeScannerUWP
{
	sealed partial class App
	{
		private ApplicationDataContainer localData;
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
			this.localData = ApplicationData.Current.LocalSettings;
			Microsoft.HockeyApp.HockeyClient.Current.Configure("5e3f1361ff74497ba3da1fbd3b27c9dd");
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs e)
		{
			Microsoft.HockeyApp.HockeyClient.Current.TrackEvent("OnLaunched");

#if DEBUG
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    DebugSettings.EnableFrameRateCounter = true;
            //}
#endif

			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}
				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				rootFrame.Navigate(typeof(MainPage), e.Arguments);
			}
			// Ensure the current window is active
			Window.Current.Activate();
			DispatcherHelper.Initialize();

		}


		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private async void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			var settings = ServiceLocator.Current.GetInstance<MainViewModel>().BarcodeData;
			var settingsString = JsonConvert.SerializeObject(settings);
			var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(ViewModelLocator.DataFile, CreationCollisionOption.ReplaceExisting);
			await Windows.Storage.FileIO.WriteTextAsync(file, settingsString);
			deferral.Complete();
		}
	}
}
