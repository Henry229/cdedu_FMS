using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Nop.Core;

using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security;

using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Controllers
{
    public class Omni_MiscController : BaseAdminController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEncryptionService _encryptionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

		#endregion
        public Omni_MiscController(ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            IThemeProvider themeProvider,  IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._encryptionService = encryptionService;
            this._themeProvider = themeProvider;
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }
        //
        // GET: /Omni_Misc/
        public ActionResult Maps()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("server=localhost; database=nopcommerce; user id=sa; password=guswjd23 ");
            conn.Open();

            System.Data.SqlClient.SqlDataAdapter adapt = new System.Data.SqlClient.SqlDataAdapter("select stud_first_name, stud_last_name, stud_address, stud_city from studentaddress ", conn);
            System.Data.DataSet ds = new System.Data.DataSet("nopcommerce"); 
            adapt.Fill(ds, "address");

            List<string> students = new List<string>();
            List<string> addresses = new List<string>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++ )
            {
                students.Add(ds.Tables[0].Rows[i]["stud_first_name"].ToString() + " " + ds.Tables[0].Rows[i]["stud_last_name"].ToString());
                addresses.Add(ds.Tables[0].Rows[i]["stud_address"].ToString() + " " + ds.Tables[0].Rows[i]["stud_city"].ToString());
            }


            ViewBag.students = students.ToList();
            ViewBag.addresses = addresses.ToList();


            return View();
        }
	}
}