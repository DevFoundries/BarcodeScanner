using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

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
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public const string BarcodeDataPropertyName = "BarcodeData";
        private ObservableCollection<BarcodeData> barcodeData = new ObservableCollection<BarcodeData>();
        public ObservableCollection<BarcodeData> BarcodeData
        {
            get
            {
                return barcodeData;
            }

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

    }

    public class BarcodeData
    {
        public string Barcode { get; set; }
        public string Description { get; set; }

    }
}