using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Page_Navigation_App.ViewModel
{
    public class GameViewModel
    {
        public string GameTitle { get; set; }
        public string GameDescription { get; set; }
        public string ShortDescription {  get; set; }
        public string repositoryUrl {  get; set; }
        public string ThumbnailImageSource { get; set; }

        public string Price { get; set; }
        public string ScreenShotSource1 {  get; set; }
        public string ScreenShotSource2 {  get; set; }

        public string ScreenShotSource3 {  get; set; }
        public string ScreenShotSource4 {  get; set; }


        public string GameGenre { get; set; }
        // Add other properties as needed
    }

}
