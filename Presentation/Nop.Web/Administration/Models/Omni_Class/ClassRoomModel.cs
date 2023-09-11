using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    
    public partial class ClassRoomModel : BaseNopEntityModel
    {
        public ClassRoomModel()
        {
            AvailableBranch = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassRoom.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassRoom.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassRoom.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassRoom.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }

        public bool isadmin { get; set; }

    }
}