using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;


namespace Nop.Admin.Models.Omni_Item
{
    public partial class ItemSet_DModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet_D.Fields.Set_id")]
        [AllowHtml]
        public int Set_id { get; set; } 

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet_D.Fields.SEQ")]
        [AllowHtml]
        public int SEQ { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet_D.Fields.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet_D.Fields.ItemName")]
        [AllowHtml]
        public string ItemName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet_D.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Check")]
        [AllowHtml]
        public bool Included { get; set; }

    }
}