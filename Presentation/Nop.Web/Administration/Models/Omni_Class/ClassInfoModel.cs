using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{

    public partial class ClassInfoModel : BaseNopEntityModel
    {
        public ClassInfoModel()
        {
            AvailableYear = new List<SelectListItem>();
            AvailableDayofWeek = new List<SelectListItem>();
            AvailableTerms = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
            AvailableBranchs = new List<SelectListItem>();
            AvailableClassRoom = new List<SelectListItem>();
            AvailableCourse = new List<SelectListItem>();
            AvailableLevelList = new List<SelectListItem>();
            AvailablePayStatusList = new List<SelectListItem>();
            AvailableTeacher = new List<SelectListItem>();
            AvailableClass = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Year")]
        [AllowHtml]
        public string Year { get; set; }
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }
        public IList<SelectListItem> AvailableTerms { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }
        public IList<SelectListItem> AvailableGrade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }
     
        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassTeacher.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassTeacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassTeacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.DayofWeek")]
        [AllowHtml]
        public string DayofWeek { get; set; }
        public IList<SelectListItem> AvailableDayofWeek { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.StartTime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime StartTime { get; set; }
        
        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.EndTime")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime EndTime { get; set; }
        
        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Duration")]
        [AllowHtml]
        public decimal Duration { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Classroom_Id")]
        [AllowHtml]
        public int Classroom_Id { get; set; }
        public IList<SelectListItem> AvailableClassRoom { get; set; }

       
        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Course_Id")]
        [AllowHtml]
        public int Course_Id { get; set; }
        public IList<SelectListItem> AvailableCourse { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Branch")]
        public IList<SelectListItem> AvailableBranchs { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassInfo.Fields.Teacher_Id")]
        public IList<SelectListItem> AvailableTeacher { get; set; }

        public bool editable { get; set; }
        public bool is_Admin { get; set; }
        

        public ClassRoomModel classroompopup { get; set; }
        
        public IList<SelectListItem> AvailablePayStatusList { get; set; }

        public IList<SelectListItem> AvailableLevelList { get; set; }

        public IList<SelectListItem> AvailableClass { get; set; }
   
    }

    

}