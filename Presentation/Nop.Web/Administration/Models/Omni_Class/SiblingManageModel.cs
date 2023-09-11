using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    
    public partial class SiblingManageModel : BaseNopEntityModel
    {
        public SiblingManageModel()
        {
            AvailableBranchs = new List<SelectListItem>();
            AvailableStudents = new List<SelectListItem>();
            AvailableStudentNames = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Parent_Id")]
        [AllowHtml]
        public int Parent_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Seq")]
        [AllowHtml]
        public int Seq { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Id_Number")]
        [AllowHtml]
        public string Id_Number { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        
        public bool isadmin { get; set; }

        //[NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Member_Id")]
        //[AllowHtml]
        //public int Member_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.SiblingManage.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }
        public IList<SelectListItem> AvailableBranchs { get; set; }
        public IList<SelectListItem> AvailableStudents { get; set; }
        public IList<SelectListItem> AvailableStudentNames { get; set; }
    }
}