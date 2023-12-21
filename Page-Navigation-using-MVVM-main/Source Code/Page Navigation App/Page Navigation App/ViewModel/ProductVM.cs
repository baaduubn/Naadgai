using Page_Navigation_App.Model;
using Page_Navigation_App.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
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
            LoadButtonsFromJson();
           
            // Add more buttons as needed with different image URLs
        }
        private async void LoadButtonsFromJson()
        {
            string jsonUrl = "https://www.baaduu.me/games.json"; // Replace with your JSON URL
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(jsonUrl);
                    GamesContainer gamesContainer = JsonConvert.DeserializeObject<GamesContainer>(json);

                    foreach (var game in gamesContainer.Games)
                    {
                        YourTestButtonCollection.Add(new ButtonData
                        {
                            ThumbnailImageSource = game.ThumbnailImageSource,
                            GameTitle = game.Title,
                            AgeRating = game.AgeRating,
                            Price = game.Price,
                            
                            // Other properties like Price, AgeRating if available
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., network issues, JSON parsing errors
            }
        }
    }
}
