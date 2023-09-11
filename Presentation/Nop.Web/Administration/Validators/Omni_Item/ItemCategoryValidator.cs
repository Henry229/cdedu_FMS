
using FluentValidation;
using Nop.Admin.Models.Omni_Item;
using Nop.Services.Localization;

namespace Nop.Admin.Validators.Omni_Item
{
    public class ItemCategoryValidator : AbstractValidator<ItemCategoryModel>
    {
        public ItemCategoryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.CategoryCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.ItemCategory.Fields.CategoryCode.Required"));
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.ItemCategory.Fields.CategoryName.Required"));
        }
    }
}