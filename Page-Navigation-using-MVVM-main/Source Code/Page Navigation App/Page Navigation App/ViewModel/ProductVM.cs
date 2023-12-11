using Page_Navigation_App.Model;
using Page_Navigation_App.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Page_Navigation_App.ViewModel
{
    class ProductVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public string ProductAvailability
        {
            get { return _pageModel.ProductStatus; }
            set { _pageModel.ProductStatus = value; OnPropertyChanged(); }
        }

        public ProductVM()
        {
            _pageModel = new PageModel();
            ProductAvailability = "Out of Stock";
        }
    }
}
