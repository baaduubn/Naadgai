using Page_Navigation_App.Model;
using Page_Navigation_App.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Page_Navigation_App.ViewModel
{
    class ProductVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        private ObservableCollection<string> _yourTestButtonCollection;

        public ObservableCollection<string> YourTestButtonCollection
        {
            get { return _yourTestButtonCollection; }
            set { _yourTestButtonCollection = value; OnPropertyChanged(); }
        }

        public ICommand YourTestCommand { get; set; } // You may need to implement this command logic

        public string ProductAvailability
        {
            get { return _pageModel.ProductStatus; }
            set { _pageModel.ProductStatus = value; OnPropertyChanged(); }
        }

        public ProductVM()
        {
            _pageModel = new PageModel();
            ProductAvailability = "Out of Stock";

            // Initialize and populate the collection with 100 test strings
            YourTestButtonCollection = new ObservableCollection<string>();
            for (int i = 1; i <= 100; i++)
            {
                YourTestButtonCollection.Add($"Test Button {i}");
            }

            YourTestCommand = new RelayCommand(YourTestCommandExecute, YourTestCommandCanExecute);
        }

        // Implement your command logic here
        private void YourTestCommandExecute(object parameter)
        {
            // Your command logic goes here
        }

        private bool YourTestCommandCanExecute(object parameter)
        {
            // Your command can execute logic goes here
            return true; // Change this according to your requirements
        }
    }
}
