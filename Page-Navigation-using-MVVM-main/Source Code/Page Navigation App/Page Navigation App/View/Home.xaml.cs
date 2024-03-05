﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Page_Navigation_App.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private List<GameInfo> gameInfos;
        public Home()
        {
            InitializeComponent();
            InitializeTimer();
            gameInfos = new List<GameInfo>
            {
                new GameInfo
                {
                    GameTitle = "Микроны босс",
                    AdditionalText = "Нэгэн цагт шугамд явж байсан нийтийн тээврийн жолоочийн аж амьдралыг өгүүлнэ",
                    ImagePaths = "https://raw.githubusercontent.com/baaduubn/Micronii-boss/main/Screenshots/Screenshot%202024-03-05%20154832.png"
                },
                new GameInfo
                {
                    GameTitle = "Мазаалай",
                    AdditionalText = "Мазаалайг алс холоос ирсэн тусламжийн шуудан адал явдалтай аялалд мордуулна",
                    ImagePaths = "https://raw.githubusercontent.com/baaduubn/baaduubn.github.io/main/Screenshot_2024-03-05_06-52-54.png"
                },
                new GameInfo
                {
                    GameTitle = "Ломбард",
                    AdditionalText = "Мөнгөтэй болох болох амархан аргад ломбардын газар ажиллуулах орох уу? та баяжих болов уу",
                    ImagePaths = "https://raw.githubusercontent.com/baaduubn/baaduubn.github.io/main/istockphoto-1496061010-612x612.jpg"
                }
            };

            InitializeTimer();
            UpdateImage(); // Update the UI with initial data
        }
        private void OpenLink(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is string url)
            {
                try
                {
                    // Open the default web browser with the specified URL
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    // Handle the exception, log it, or show an error message
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No URL provided.");
            }
        }

       

        private int currentIndex = 0;
        private DispatcherTimer timer;
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5); // Adjust the interval as needed
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Move to the next image in the list
            currentIndex = (currentIndex + 1) % gameInfos.Count;
            UpdateImage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Handle button click if needed
        }

        private void UpdateImage()
        {
            if (gameInfos.Count > 0 && currentIndex < gameInfos.Count)
            {
                // Update the image content of the button
                GameInfo currentGameInfo = gameInfos[currentIndex];
                string imagePath = currentGameInfo.ImagePaths;

                textBlockGameTitle.Text = currentGameInfo.GameTitle;
                textBlockAdditionalText.Text = currentGameInfo.AdditionalText;
                Uri imageUri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                BitmapImage bitmapImage = new BitmapImage(imageUri);
                imageSliderButton.Content = bitmapImage;
            }
        }
    }

}
[System.Serializable]
public class GameInfo
{
    public string ImagePaths { get; set; }
    public string GameTitle { get; set; }
    public string AdditionalText { get; set; }
}
