using Nop.Web.Framework.Mvc;
using Nop.Admin.Models;
using System.Collections.Generic;

namespace Nop.Admin.Models.Home
{
    public partial class DashboardModel : BaseNopModel
    {
        public DashboardModel()
        {
            NewsItemList = new List<News.NewsItemModel>();
        }
        public bool IsLoggedInAsVendor { get; set; }

        public bool Is_Admin { get; set; }
        public bool Is_Production { get; set; }
        public bool Is_Accountant { get; set; }
        public bool Is_Branch { get; set; }


        public IList<News.NewsItemModel> NewsItemList { get; set; }
    }
}