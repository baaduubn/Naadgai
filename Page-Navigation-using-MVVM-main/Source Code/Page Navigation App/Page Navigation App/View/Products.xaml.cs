using Page_Navigation_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;

namespace Page_Navigation_App.View
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        
        public Products()
        {
            InitializeComponent();
        }
    

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
         
            var button = sender as Button;
            if (button == null) return;

            var buttonData = button?.CommandParameter as ButtonData;
            if (buttonData == null)
            {
                MessageBox.Show("null bno");
                // Log error or handle the case where buttonData is null
                return;
            }
            string jsonUrl = "https://www.baaduu.me/games.json"; // URL to the JSON file

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(jsonUrl);
                    GamesContainer gamesContainer = JsonConvert.DeserializeObject<GamesContainer>(json);

                    // Find the "Sprout Valley" game data
                    Game sproutValleyData = gamesContainer.Games
                        .FirstOrDefault(g => g.Title == buttonData.GameTitle);

                    if (sproutValleyData != null)
                    {
                        GameViewModel viewModel = new GameViewModel
                        {
                            GameTitle = sproutValleyData.Title,
                            ShortDescription = sproutValleyData.ShortDescription,
                            GameDescription = sproutValleyData.GameDescription,
                            GameGenre = sproutValleyData.GameGenre,
                            Price = sproutValleyData.Price,
                            ThumbnailImageSource = sproutValleyData.ThumbnailImageSource,
                            ScreenShotSource1 = sproutValleyData?.ScreenShotSource1,
                            ScreenShotSource2 = sproutValleyData.ScreenShotSource2,
                            ScreenShotSource3 = sproutValleyData.ScreenShotSource3,
                            ScreenShotSource4 = sproutValleyData.ScreenShotSource4,
                            repositoryUrl = sproutValleyData.repositoryUrl,
                            // ... assign other properties
                        };

                        GameView gameView = new GameView(viewModel);
                        gameView.Show();
                    }
                    else
                    {
                        MessageBox.Show(buttonData.GameTitle);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(buttonData.GameTitle);
                MessageBox.Show($"Error loading game data: {ex.Message}");
            }
        }

    }
}
public class Game
{
    public string Title { get; set; } // Add Title property
    public string ShortDescription { get; set; }
    public string GameDescription { get; set; }
    public string GameGenre { get; set; }
    public string AgeRating {  get; set; }

    public string Price {  get; set; }
    public string ThumbnailImageSource { get; set; }
    public string ScreenShotSource1 { get; set; }
    public string ScreenShotSource2 { get; set; }
    public string ScreenShotSource3 { get; set; }
    public string ScreenShotSource4 { get; set; }
    public string repositoryUrl { get; set; }
    // ... other properties
}
public class GamesContainer
{
    public List<Game> Games { get; set; }
}
