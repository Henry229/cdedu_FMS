using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Basis;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Basis
{
    [Validator(typeof(CodeMasterValidator))]
    public partial class CodeMasterModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.CodeType")]
        [AllowHtml]
        public string CodeType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.TypeName")]
        [AllowHtml]
        public string TypeName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.DetailCode")]
        [AllowHtml]
        public string DetailCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.DetailName")]
        [AllowHtml]
        public string DetailName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.String_01")]
        [AllowHtml]
        public string String_01 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.String_02")]
        [AllowHtml]
        public string String_02 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.String_03")]
        [AllowHtml]
        public string String_03 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.Val_01")]
        [AllowHtml]
        public int Val_01 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.Val_02")]
        [AllowHtml]
        public int Val_02 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.Val_03")]
        [AllowHtml]
        public int Val_03 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.SortOrder")]
        [AllowHtml]
        public int SortOrder { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CodeMaster.Fields.YN_Use")]
        [AllowHtml]
        public string YN_Use { get; set; }

    }
}