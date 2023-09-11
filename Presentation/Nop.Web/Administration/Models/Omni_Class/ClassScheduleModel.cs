using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassScheduleModel : BaseNopEntityModel
    {
        public ClassScheduleModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableClassRoom = new List<SelectListItem>();
            AvailableTeacher = new List<SelectListItem>();
         
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.SEQ")]
        [AllowHtml]
        public int SEQ { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Class_Name")]
        [AllowHtml]
        public string Class_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Class_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Class_Date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Class_StartTime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime Class_StartTime { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Class_EndTime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime Class_EndTime { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Classroom_Id")]
        [AllowHtml]
        public int Classroom_Id { get; set; }
        public IList<SelectListItem> AvailableClassRoom { get; set; }

        //[NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Branch")]
        //public IList<SelectListItem> AvailableBranchs { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.YN_Close")]
        [AllowHtml]
        public string YN_Close { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassTeacher.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }
        public IList<SelectListItem> AvailableTeacher { get; set; }
        public IList<SelectListItem> AvailableBranch { get; set; }

        public ClassRoomModel classroompopup { get; set; }
       
        public bool is_Admin { get; set; }

       
    }

}