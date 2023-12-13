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

        public ObservableCollection<ButtonData> YourTestButtonCollection { get; } = new ObservableCollection<ButtonData>();

        public ProductVM()
        {
            _pageModel = new PageModel();

            // Button 1 (CS:GO image)
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price="Онцгой",
                AgeRating="17"
            });

            // Button 2 (Dota 2 image)
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://community.tm/attachments/thumb-099-dota-2-3-jpg.15775/", // Replace with the Dota 2 image URL
                ButtonContent = "Dota 2",
                Price = "Үнэгүй",
                AgeRating = "17"

            });
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price = "Үнэгүй",
                AgeRating = "17"
            });
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price = "Үнэгүй",
                AgeRating = "17"
            });
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price = "Үнэгүй",
                AgeRating = "17"
            });
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price = "Free",
                AgeRating = "17"
            });
            YourTestButtonCollection.Add(new ButtonData
            {
                ThumbnailImageSource = "https://steamcdn-a.akamaihd.net/apps/csgo/blog/images/wallpaper_nologo.jpg", // Replace with the CS:GO image URL
                ButtonContent = "CS:GO",
                Price = "Үнэгүй",
                AgeRating = "17"
            });

            // Add more buttons as needed with different image URLs
        }
    }
}
