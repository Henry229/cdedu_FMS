using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Analysis
{
    public class EnrolSummaryModel : BaseNopEntityModel
    {
        public EnrolSummaryModel()
        {
            AvailableYear1 = new List<SelectListItem>();
            AvailableYear2 = new List<SelectListItem>();
            AvailableBranch = new List<SelectListItem>();
        }


        public int Id { get; set; }

        public string Year1 {get;set;}
        public string Year2 {get;set;}

        public string Branch { get; set; }


        public IList<SelectListItem> AvailableYear1 { get; set; }

        public IList<SelectListItem> AvailableYear2 { get; set; }

        public IList<SelectListItem> AvailableBranch { get; set; }

        public bool isadmin { get; set; }
    }




}