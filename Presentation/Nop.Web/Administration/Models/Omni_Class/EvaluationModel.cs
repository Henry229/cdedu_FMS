using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class EvaluationModel : BaseNopEntityModel
    {
        public EvaluationModel()
        {
            AvailableEvaluationType = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.EvaluationType")]
        [AllowHtml]
        public string EvaluationType { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.Teacher_Id")]
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


        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.Evaluator")]
        [AllowHtml]
        public string Evaluator { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.Score")]
        [AllowHtml]
        public int Score { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.reg_date")]
        [AllowHtml]
        public System.DateTime reg_date { get; set; }



        [NopResourceDisplayName("Admin.Omni_Backoffice.Evaluation.Fields.EvaluationType")]
        public IList<SelectListItem> AvailableEvaluationType { get; set; }



    }
}