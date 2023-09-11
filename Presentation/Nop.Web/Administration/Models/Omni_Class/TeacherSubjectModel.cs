using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class TeacherSubjectModel : BaseNopEntityModel
    {
        public TeacherSubjectModel()
        {
            AvailableSubject = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Check")]
        [AllowHtml]
        public bool Check { get; set; }



        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherSubject.Fields.Subject")]
        public IList<SelectListItem> AvailableSubject { get; set; }


        public TeacherModel teacherPopup { get; set; }



    }
}