using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Analysis
{
    public class TestSummaryModel : BaseNopEntityModel
    {
        public TestSummaryModel()
        {
            AvailableYear1 = new List<SelectListItem>();
            AvailableYear2 = new List<SelectListItem>();
            AvailableBranch = new List<SelectListItem>();
            AvailableTestType = new List<SelectListItem>();
           
        }


        public int Id { get; set; }

        public string Year1 { get; set; }
        public string Year2 { get; set; }
        public string Branch { get; set; }

        public string TestType { get; set; }

        public int TestNo { get; set; }

        public int Cnt { get; set; }

        public IList<SelectListItem> AvailableYear1 { get; set; }
        public IList<SelectListItem> AvailableYear2 { get; set; }

        public IList<SelectListItem> AvailableBranch { get; set; }
        public IList<SelectListItem> AvailableTestType { get; set; }
        
        public bool isadmin { get; set; }
    }




}