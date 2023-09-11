using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassTeacherPopupModel : BaseNopEntityModel
    {
        public ClassTeacherPopupModel()
        {
            AvailableBranch = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Teacherpop_Id")]
        [AllowHtml]
        public int Teacherpop_Id { get; set; }

   
        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

       


        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }


        //public TeacherModel teacherPopup { get; set; }



    }
}