using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Royalty
{
    public class StudentSummaryModel : BaseNopEntityModel
    {
        public StudentSummaryModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Cnt_Book")]
        public int Cnt_Book { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Cnt_Course")]
        public int Cnt_Course { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Cnt_Primary")]
        public int Cnt_Primary { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Cnt_Secondary")]
        public int Cnt_Secondary { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Year")]
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentSummary.Term")]
        public IList<SelectListItem> AvailableTerm { get; set; }

        public bool isadmin { get; set; }
    }




}