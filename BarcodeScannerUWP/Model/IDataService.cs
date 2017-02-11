using System.Threading.Tasks;

namespace BarcodeScannerUWP.Model
{
	public interface IDataService
	{
		Task<DataItem> GetData();
	}
}