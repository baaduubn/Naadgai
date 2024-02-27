using Page_Navigation_App.Model;
using System;

namespace Page_Navigation_App.ViewModel
{
    class CustomerVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Username => AppData.CurrentUser?.Username ?? "DefaultUsername";
        public string Id => AppData.CurrentUser?.Id ?? "DefaultId";
        public int lvl => AppData.CurrentUser?.lvl ?? 0;
        public int like => AppData.CurrentUser?.love ?? 7468;
        public bool premium => AppData.CurrentUser?.premium ?? false;
        public string premiumDate => (AppData.CurrentUser?.premiumDate != null) ? DateTime.Parse(AppData.CurrentUser.premiumDate).ToString("yyyy-MM-dd") : "2023-12-15";
        public string createdDate => AppData.CurrentUser?.createdDate ?? "2014-09-01";
    }
}
