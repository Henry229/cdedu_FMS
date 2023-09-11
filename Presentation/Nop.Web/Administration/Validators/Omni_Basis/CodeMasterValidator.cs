
using FluentValidation;
using Nop.Admin.Models.Omni_Basis;
using Nop.Services.Localization;

namespace Nop.Admin.Validators.Omni_Basis
{
    public class CodeMasterValidator : AbstractValidator<CodeMasterModel>
    {
        public CodeMasterValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.CodeType).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.CodeType.Required"));
            RuleFor(x => x.TypeName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.TypeName.Required"));
            RuleFor(x => x.DetailCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.DetailCode.Required"));
            RuleFor(x => x.DetailName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.DetailName.Required"));
            RuleFor(x => x.YN_Use).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.YN_Use.Required"));

            RuleFor(x => x.CodeType).Length(4).WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.CodeType.Length"));
            RuleFor(x => x.DetailCode).Length(4).WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.CodeMaster.Fields.DetailCode.Length"));
        }
    }
}