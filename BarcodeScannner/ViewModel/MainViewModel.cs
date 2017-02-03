using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using BarcodeScannner.Messages;

namespace BarcodeScannner.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
	    private INavigationService navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			this.navigationService = navigationService;
			Messenger.Default.Register<BarcodeMessage>(this, (b) =>
			{
				this.BarcodeResult = b.Barcode;
				this.AddBarcodeData(new BarcodeData() {Barcode = b.Barcode});
			});
		}

        public const string BarcodeDataPropertyName = "BarcodeData";
        private ObservableCollection<BarcodeData> barcodeData = new ObservableCollection<BarcodeData>();
        public ObservableCollection<BarcodeData> BarcodeData
        {
            get {return barcodeData;}

            set
            {
                if (barcodeData == value)
                {
                    return;
                }

                barcodeData = value;
                RaisePropertyChanged(BarcodeDataPropertyName);
            }
        }

        public void AddBarcodeData(BarcodeData data)
        {
            barcodeData.Add(data);
            RaisePropertyChanged(BarcodeDataPropertyName);
        }

	    private string barcodeResult;

	    public string BarcodeResult
	    {
		    get { return barcodeResult;}
		    set
		    {
			    if (barcodeResult == value) return;
			    barcodeResult = value;
				RaisePropertyChanged(()=>BarcodeResult);
		    }
	    }

    }

    public class BarcodeData
    {
        public string Barcode { get; set; }
        public string Description { get; set; }

    }
}