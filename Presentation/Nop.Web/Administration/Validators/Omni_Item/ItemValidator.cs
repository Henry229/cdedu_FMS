
using FluentValidation;
using Nop.Admin.Models.Omni_Item;
using Nop.Services.Localization;

namespace Nop.Admin.Validators.Omni_Item
{
    public class ItemValidator : AbstractValidator<ItemModel>
    {
        public ItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ItemCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.Item.Fields.ItemCode.Required"));
            RuleFor(x => x.ItemName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.Item.Fields.ItemName.Required"));
            RuleFor(x => x.ItemCategory).NotEmpty().WithMessage(localizationService.GetResource("Admin.Omni_Backoffice.Item.Fields.ItemCategory.Required"));
        }
    }
}