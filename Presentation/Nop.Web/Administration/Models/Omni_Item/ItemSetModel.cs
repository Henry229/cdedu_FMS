using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Item
{
    public partial class ItemSetModel : BaseNopEntityModel
    {
        public ItemSetModel()
        {
            AvailableSetCategoryList = new List<SelectListItem>();
            AvailableGradeList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableLevelList = new List<SelectListItem>();
            AvailableCourseList = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.SetName")]
        [AllowHtml]
        public string SetName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.SetCategory")]
        [AllowHtml]
        public string SetCategory { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Course")]
        [AllowHtml]
        public int Course { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.ItemCnt")]
        [AllowHtml]
        public int ItemCnt { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.SetCategory")]
        public IList<SelectListItem> AvailableSetCategoryList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        public IList<SelectListItem> AvailableGradeList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        public IList<SelectListItem> AvailableTermList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Level")]
        public IList<SelectListItem> AvailableLevelList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Course")]
        public IList<SelectListItem> AvailableCourseList { get; set; }

    }
}