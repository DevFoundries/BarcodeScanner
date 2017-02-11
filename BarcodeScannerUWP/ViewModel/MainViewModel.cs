using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarcodeScannerUWP.Model;

namespace BarcodeScannerUWP.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IDataService _dataService;
		private IDialogService dialogService;
		private readonly INavigationService _navigationService;
		private string barcode;
		private RelayCommand copyCommand;

		public MainViewModel(
			IDataService dataService,
			IDialogService dialogService,
			INavigationService navigationService)
		{
			this.dialogService = dialogService;
			_dataService = dataService;
			_navigationService = navigationService;
		}

		public string Barcode
		{
			get { return barcode; }
			set { Set(() => Barcode,ref barcode, value); }
		}

		public List<BarcodeData> barcodeData = new List<BarcodeData>();
		public List<BarcodeData> BarcodeData
		{
			get { return this.barcodeData; }
			set { Set(() => BarcodeData, ref barcodeData, value); }
		}

		public void AddBarcodeData(BarcodeData data)
		{
			barcodeData.Add(data);
			RaisePropertyChanged(()=>BarcodeData);
		}

		public RelayCommand CopyCommand
		{
			get { return copyCommand ?? (copyCommand = new RelayCommand(() =>
			{
				DataPackage dataPackage = new DataPackage
				{
					RequestedOperation = DataPackageOperation.Copy
				};
				dataPackage.SetText(this.Barcode);
				this.dialogService.ShowMessage("Barcode copied to clipboard!", "Success");
			}));
			}
		}
	}
}