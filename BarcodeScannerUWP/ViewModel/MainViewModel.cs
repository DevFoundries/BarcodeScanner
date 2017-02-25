using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using BarcodeScannerUWP.Model;
using Newtonsoft.Json;

namespace BarcodeScannerUWP.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IDataService _dataService;
		private IDialogService dialogService;
		private readonly INavigationService _navigationService;
		private string barcode;
		private RelayCommand copyCommand;
		private RelayCommand<int> removeCommand;

		public MainViewModel(
			IDataService dataService,
			IDialogService dialogService,
			INavigationService navigationService)
		{
			this.dialogService = dialogService;
			_dataService = dataService;
			_navigationService = navigationService;
			Init();
		}

		private async Task Init()
		{
			await LoadData();
		}

		public string Barcode
		{
			get { return barcode; }
			set { Set(() => Barcode,ref barcode, value); }
		}

		public ObservableCollection<BarcodeData> barcodeData = new ObservableCollection<BarcodeData>();
		public ObservableCollection<BarcodeData> BarcodeData
		{
			get { return this.barcodeData; }
			set { Set(() => BarcodeData, ref barcodeData, value); }
		}

		public void AddBarcodeData(BarcodeData data)
		{
			data.Id = barcodeData.Count;
			barcodeData.Insert(0,data);
			this.Barcode = data.Barcode;
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

		public RelayCommand<int> RemoveCommand
		{
			get
			{
				return removeCommand ?? (removeCommand = new RelayCommand<int>((i) =>
				{
					var record = barcodeData.FirstOrDefault(x => x.Id == i);
					BarcodeData.Remove(record);
					RaisePropertyChanged(()=>BarcodeData);
				}));
			}
		}

		private RelayCommand fileSaveCommand;
		public RelayCommand FileSaveCommand
		{
			get
			{
				return fileSaveCommand ?? (fileSaveCommand = new RelayCommand(async () =>
				{
					FileSavePicker saver = new FileSavePicker();
					saver.SuggestedStartLocation = PickerLocationId.Desktop;
					saver.FileTypeChoices.Add("CSV", new List<string>() {".csv"});
					saver.SuggestedFileName = "BarcodeData";
					var file = await saver.PickSaveFileAsync();
					if (file != null)
					{
						StringBuilder sb = new StringBuilder("BarcodeId,Barcode,Description\r\n");
						foreach (var row in BarcodeData)
						{
							sb.AppendLine(String.Join(",", row.Id, row.Barcode, row.Description));
						}

						Windows.Storage.CachedFileManager.DeferUpdates(file);
						await Windows.Storage.FileIO.WriteTextAsync(file,sb.ToString());
						var status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
						if (status == FileUpdateStatus.Complete)
						{
							await this.dialogService.ShowMessage("File saved", "Success");
						}
						else
						{
							await this.dialogService.ShowError("Unable to save file","Error","Ok",null);
						}
					}
				}));
			}
		}

		public async Task LoadData()
		{
			var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
			var exists = files.Any(x => x.Name == ViewModelLocator.DataFile);
			if (exists)
			{
				var file = files.FirstOrDefault(x => x.Name == ViewModelLocator.DataFile);
//				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(ViewModelLocator.DataFile);
				var text = await FileIO.ReadTextAsync(file);
				if (string.IsNullOrEmpty(text) || text == "null")
				{
					await ServiceLocator.Current.GetInstance<IDialogService>()
						.ShowError("We found a data file, but there was nothing in it.", "Error", "Ok", null);
					return;
				}

				var data = JsonConvert.DeserializeObject<ObservableCollection<BarcodeData>>(text);
				if (data == null)
				{
					await ServiceLocator.Current.GetInstance<IDialogService>()
						.ShowError("Unable to load data. Please reinstall.", "Ooops!", "Ok", null);
					return;

				}
				this.BarcodeData = data;
			}
		}
	}
}