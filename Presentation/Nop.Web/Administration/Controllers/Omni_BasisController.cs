using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Nop.Core;
using Nop.Core.Domain.Customers;

using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Catalog;
using Nop.Services.Customers;

using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;

using Nop.Core.Domain.Omni_Backoffice;
using Nop.Services.Omni_Backoffice;
using Nop.Admin.Models.Omni_Basis;
using Nop.Admin.Models.Omni_Item;



namespace Nop.Admin.Controllers
{
    public partial class Omni_BasisController : BaseAdminController
    {


        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly IItemCategoryService _itemCategoryService;
        private readonly IItemService _itemService;
        private readonly ICourseMasterService _courseMasterService;
        private readonly ICampusService _campusService;
        private readonly ICalendarMasterService _calendarMasterService;

        private readonly IPriceFormatter _priceFormatter;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEncryptionService _encryptionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        private readonly ICustomerRegistrationService _customerRegistrationService;  //password
        private readonly CustomerSettings _customerSettings;

        #endregion

        #region Constructors



        public Omni_BasisController(ICodeMasterService codeMasterService, ICourseMasterService courseMasterService, IItemCategoryService itemCategoryService, IItemService itemService,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService, ICustomerRegistrationService customerRegistrationService, CustomerSettings customerSettings,
            IThemeProvider themeProvider, IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext, ICalendarMasterService calendarMasterService, ICampusService campusService)
        {
            this._codeMasterService = codeMasterService;
            this._itemCategoryService = itemCategoryService;
            this._itemService = itemService;
            this._courseMasterService = courseMasterService;
            this._calendarMasterService = calendarMasterService;
            this._campusService = campusService;

            this._priceFormatter = priceFormatter;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._encryptionService = encryptionService;
            this._themeProvider = themeProvider;
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._workContext = workContext;

            this._customerRegistrationService = customerRegistrationService;
            this._customerSettings = customerSettings;
        }

        #endregion


        #region Basic Pages

        public ActionResult PageNotFound()
        {
            return View();
        }
        #endregion

        #region password
        public ActionResult ChangePassword()
        {


            var customer = _workContext.CurrentCustomer;

            var model = new ChangePasswordModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {

            var customer = _workContext.CurrentCustomer;

            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(customer.Email,
                    true, _customerSettings.DefaultPasswordFormat, model.NewPassword, model.OldPassword);
                var changePasswordResult = _customerRegistrationService.ChangePassword(changePasswordRequest);
                if (changePasswordResult.Success)
                {
                    model.Result = _localizationService.GetResource("Account.ChangePassword.Success");
                    return View(model);
                }
                else
                {
                    foreach (var error in changePasswordResult.Errors)
                        ModelState.AddModelError("", error);
                }
            }


            //If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region CODEMASTER

        public ActionResult CodeMaster()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            return View();
        }



        [HttpPost]
        public ActionResult CodeMaster(DataSourceRequest command,
            Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var codemasters = _codeMasterService
                .GetAllCodeMasters("", command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = codemasters.Select(x =>
                {

                    var codeMasterModel = new CodeMasterModel()
                    {
                        Id = x.Id,
                        CodeType = x.CodeType,
                        TypeName = x.TypeName,
                        DetailCode = x.DetailCode,
                        DetailName = x.DetailName,
                        String_01 = x.String_01,
                        String_02 = x.String_02,
                        String_03 = x.String_03,
                        Val_01 = x.Val_01,
                        Val_02 = x.Val_02,
                        Val_03 = x.Val_03,
                        SortOrder = x.SortOrder,
                        YN_Use = x.YN_Use
                    };
                    return codeMasterModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = codemasters.TotalCount
            };


            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult CodeMasterUpdate(CodeMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            if (model.CodeType != null)
                model.CodeType = model.CodeType.Trim();
            if (model.TypeName != null)
                model.TypeName = model.TypeName.Trim();
            if (model.DetailCode != null)
                model.DetailCode = model.DetailCode.Trim();
            if (model.DetailName != null)
                model.DetailName = model.DetailName.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var codemaster = _codeMasterService.GetCodeMasterById(model.Id);
            if (codemaster == null)
                return Content("No codemaster could be loaded with the specified Code");


            //setting name or store has been changed
            _codeMasterService.DeleteCodeMaster(codemaster);

            _codeMasterService.InsertCodeMaster(new CodeMaster(
                        model.CodeType,
                        model.TypeName,
                        model.DetailCode,
                        model.DetailName,
                        model.String_01,
                        model.String_02,
                        model.String_03,
                        model.Val_01,
                        model.Val_02,
                        model.Val_03,
                        model.SortOrder,
                        model.YN_Use,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username)
                );


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CodeMasterInsert([Bind(Exclude = "Id")] CodeMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            if (model.CodeType != null)
                model.CodeType = model.CodeType.Trim();
            if (model.TypeName != null)
                model.TypeName = model.TypeName.Trim();
            if (model.DetailCode != null)
                model.DetailCode = model.DetailCode.Trim();
            if (model.DetailName != null)
                model.DetailName = model.DetailName.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            _codeMasterService.InsertCodeMaster(new CodeMaster(
                        model.CodeType,
                        model.TypeName,
                        model.DetailCode,
                        model.DetailName,
                        model.String_01,
                        model.String_02,
                        model.String_03,
                        model.Val_01,
                        model.Val_02,
                        model.Val_03,
                        model.SortOrder,
                        model.YN_Use,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username)
                );


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CodeMasterDelete(CodeMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var codeMaster = _codeMasterService.GetCodeMasterById(model.Id);
            if (codeMaster == null)
                throw new ArgumentException("No setting found with the specified id");
            _codeMasterService.DeleteCodeMaster(codeMaster);


            return new NullJsonResult();
        }

        #endregion

        #region COURSEMASTER
        /*
        public ActionResult CourseMaster()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            return View();
        }
        */
        public ActionResult CourseMaster()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new CourseMasterModel();


            SetAvailableCourseCombo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult GetCourseMasterList(DataSourceRequest command,
            CourseMasterModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var coursemasters = _courseMasterService
                .GetAllCourseMasters( model.Year, model.Term, model.CourseCategory, model.Grade, command.Page - 1, 1000).ToList();

            if (!String.IsNullOrEmpty(model.CourseName))
            {
                coursemasters = coursemasters.Where(x => x.CourseName.Contains(model.CourseName)).ToList();
            }

            var gridModel = new DataSourceResult
            {
                Data = coursemasters.Select(x =>
                {

                    var courseMasterModel = new CourseMasterModel()
                    {
                        //model=domain
                        Id = x.Id,
                        CourseName = x.CourseName,
                        CourseCategory = x.CourseCategory,
                        CourseID_P = x.CourseID_P,
                        Year = x.Year,
                        Term = x.Term,
                        Grade = x.Grade,
                        Level = x.Level,
                        StartWeek = x.StartWeek,
                        TotalWeek = x.TotalWeek,
                        CourseFee = x.CourseFee,
                        Facility = x.Facility,
                        Remarks = x.Remarks,
                        YN_Use = x.YN_Use,
                        BookFee = x.BookFee,
                        reg_date = x.reg_date,
                        NewBookFee=x.NewBookFee
                        //reg_source = x.reg_source
                    };
                    return courseMasterModel;
                }).OrderBy( y => y.Grade).ThenBy( y => y.CourseCategory)
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = coursemasters.Count
            };


            return Json(gridModel);
        }

        [HttpPost]
        //public ActionResult CourseMasterUpdate([Bind(Exclude = "Id")] CourseMasterModel model)
        public ActionResult CourseMasterUpdate(CourseMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();
            /*
            if (model.CodeType != null)
                model.CodeType = model.CodeType.Trim();
            if (model.TypeName != null)
                model.TypeName = model.TypeName.Trim();
            if (model.DetailCode != null)
                model.DetailCode = model.DetailCode.Trim();
            if (model.DetailName != null)
                model.DetailName = model.DetailName.Trim();
            */
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var coursemaster = _courseMasterService.GetCourseMasterById(model.Id);
            if (coursemaster == null)
                return Content("No coursemaster could be loaded with the specified Code");

            coursemaster.CourseName = model.CourseName;
            coursemaster.CourseCategory = model.CourseCategory;
            coursemaster.CourseID_P = 0;
            coursemaster.Year = model.Year;
            coursemaster.Term = model.Term;
            coursemaster.Grade = model.Grade;
            coursemaster.Level = model.Level;
            coursemaster.StartWeek = model.StartWeek;
            coursemaster.TotalWeek = model.TotalWeek;
            coursemaster.CourseFee = model.CourseFee;
            coursemaster.Remarks = model.Remarks;
            coursemaster.YN_Use = model.YN_Use;
            coursemaster.reg_date = DateTime.Now;
            coursemaster.reg_source = _workContext.CurrentCustomer.Username;
            coursemaster.BookFee = model.BookFee;
            coursemaster.Facility = model.Facility;
            coursemaster.NewBookFee = model.NewBookFee;

            _courseMasterService.UpdateCourseMaster(coursemaster);
            /*
            //setting name or store has been changed
            _courseMasterService.DeleteCourseMaster(coursemaster);

            _courseMasterService.InsertCourseMaster(new CourseMaster(
                        model.CourseName,
                        model.CourseCategory,
                        model.Year,
                        model.Term,
                        model.Grade,
                        model.Level,
                        model.TotalWeek,
                        model.CourseFee,
                        model.Remarks,
                        model.YN_Use,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username,
                        //model.reg_source
                        model.BookFee
                       )
                );
            */

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CourseMasterInsert([Bind(Exclude = "Id")] CourseMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();


            _courseMasterService.InsertCourseMaster(new CourseMaster(
                        model.CourseName,
                        model.CourseCategory,
                        0,
                        model.Year,
                        model.Term,
                        model.Grade,
                        model.Level,
                        model.StartWeek,
                        model.TotalWeek,
                        model.CourseFee,
                        model.Remarks,
                        model.YN_Use,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username,
                        //model.reg_source
                        model.BookFee,
                        model.Facility,
                        model.NewBookFee
                       )
                );


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CourseMasterDelete(CourseMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var courseMaster = _courseMasterService.GetCourseMasterById(model.Id);
            if (courseMaster == null)
                throw new ArgumentException("No setting found with the specified id");
            _courseMasterService.DeleteCourseMaster(courseMaster);


            return new NullJsonResult();
        }

        #endregion

        #region CALENDARMASTER
        /*
        public ActionResult CalendarMaster()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            return View();
        }
        */
        public ActionResult CalendarMaster()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new CalendarMasterModel();

            SetAvailableCalendarCombo(model);


            return View(model);
        }


        [HttpPost]
        public ActionResult CalendarMaster(DataSourceRequest command,
            Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var calendarmasters = _calendarMasterService
                .GetAllCalendarMasters(command.Page - 1, 65536).OrderBy(x => x.Year).ThenBy(x => x.StartDate).ToList();

            var gridModel = new DataSourceResult
            {
                Data = calendarmasters.Select(x =>
                {

                    var courseMasterModel = new CalendarMasterModel()
                    {
                        //model=domain
                        Id = x.Id,
                        Year = x.Year,
                        Term = x.Term,
                        Week = x.Week,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        YN_Active = String.IsNullOrEmpty(x.YN_Active) ? "N" : x.YN_Active,
                        YN_PA = String.IsNullOrEmpty(x.YN_PA) ? "N" : x.YN_PA,
                        YN_Enrol = String.IsNullOrEmpty(x.YN_Enrol) ? "N" : x.YN_Enrol,
                        reg_date = x.reg_date,
                        //reg_source = x.reg_source,
                        Remarks = x.Remarks
                    };
                    return courseMasterModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = calendarmasters.Count
            };


            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult CalendarMasterUpdate(CalendarMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();
            /*
            if (model.CodeType != null)
                model.CodeType = model.CodeType.Trim();
            if (model.TypeName != null)
                model.TypeName = model.TypeName.Trim();
            if (model.DetailCode != null)
                model.DetailCode = model.DetailCode.Trim();
            if (model.DetailName != null)
                model.DetailName = model.DetailName.Trim();
            */
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var calendarmaster = _calendarMasterService.GetCalendarMasterById(model.Id);
            if (calendarmaster == null)
                return Content("No calendarmaster could be loaded with the specified Code");

            calendarmaster.Year=model.Year;
            calendarmaster.Term=model.Term;
            calendarmaster.Week=model.Week;
            calendarmaster.StartDate=model.StartDate;
            calendarmaster.EndDate=model.EndDate;
            calendarmaster.YN_Active = model.YN_Active;
            calendarmaster.YN_PA = model.YN_PA;
            calendarmaster.YN_Enrol = model.YN_Enrol;
            calendarmaster.reg_date=model.reg_date;
            calendarmaster.reg_source=_workContext.CurrentCustomer.Username;
            calendarmaster.Remarks=model.Remarks;

            _calendarMasterService.UpdateCalendarMaster(calendarmaster);

            /*
            //setting name or store has been changed
            _calendarMasterService.DeleteCalendarMaster(calendarmaster);

            _calendarMasterService.InsertCalendarMaster(new CalendarMaster(
                        model.Year,
                        model.Term,
                        model.Week,
                        model.StartDate,
                        model.EndDate,
                        model.reg_date,
                        //model.reg_source,
                        _workContext.CurrentCustomer.Username,
                        model.Remarks
                       )
                );
            */

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CalendarMasterInsert([Bind(Exclude = "Id")] CalendarMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();
            /*
            if (model.CodeType != null)
                model.CodeType = model.CodeType.Trim();
            if (model.TypeName != null)
                model.TypeName = model.TypeName.Trim();
            if (model.DetailCode != null)
                model.DetailCode = model.DetailCode.Trim();
            if (model.DetailName != null)
                model.DetailName = model.DetailName.Trim();
            */
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            _calendarMasterService.InsertCalendarMaster(new CalendarMaster(
                        model.Year,
                        model.Term,
                        model.Week,
                        model.StartDate,
                        model.EndDate,
                        model.YN_Active,
                        model.YN_PA,
                        model.YN_Enrol,
                        model.reg_date,
                        //model.reg_source,
                        _workContext.CurrentCustomer.Username,
                        model.Remarks
                       )
                );


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CalendarMasterDelete(CalendarMasterModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var courseMaster = _calendarMasterService.GetCalendarMasterById(model.Id);
            if (courseMaster == null)
                throw new ArgumentException("No setting found with the specified id");
            _calendarMasterService.DeleteCalendarMaster(courseMaster);


            return new NullJsonResult();
        }

        #endregion


        # region Campus

        public ActionResult Campus()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new CampusModel();
            var categorys = this._itemCategoryService.GetAllItemCategorys();

            SetAvailableCampusCombo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult GetCampus(DataSourceRequest command,
            CampusModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var campus = _campusService.GetAllCampus(model.Campus_Title, command.Page - 1, 1000);

            var gridModel = new DataSourceResult
            {
                Data = campus.Select(x =>
                {

                    var compusModel = new CampusModel()
                    {
                        Id = x.Id,
                        Campus_Prefix = x.Campus_Prefix,
                        Campus_Title = x.Campus_Title,
                        Campus_EN = x.Campus_EN,
                        Suburb = x.Suburb,
                        State = x.State,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        PostCode = x.PostCode,
                        ABN = x.ABN,
                        PhoneNo = x.PhoneNo,
                        Email = x.Email,
                        RoyaltyType = x.RoyaltyType,
                        RoyaltyType_HI = x.RoyaltyType_HI,
                        RoyaltyType_PA = x.RoyaltyType_PA,
                        RoyaltyType_TT = x.RoyaltyType_TT,
                        YN_Use = x.YN_Use,
                        LicenseeCode = x.LicenseeCode,
                        reg_date = x.reg_date
                        //reg_source = x.reg_source
                    };
                    return compusModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = campus.TotalCount
            };
            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult CampusInsert([Bind(Exclude = "Id")] CampusModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            /*
            var branchcode = model.BranchCode;

            if (branchcode == null || branchcode == "")
            {
                branchcode = _workContext.CurrentCustomer.AdminComment;
            }
            */
            var campus = new Campus(
            model.Campus_Prefix,
            model.Campus_Title,
            model.Campus_EN,
            model.Suburb,
            model.State,
            model.Address1,
            model.Address2,
            model.PostCode,
            model.ABN,
            model.PhoneNo,
            model.Email,
            model.RoyaltyType,
            model.RoyaltyType_HI,
            model.RoyaltyType_PA,
            model.RoyaltyType_TT,
            model.YN_Use,
            model.LicenseeCode,
            DateTime.Now,
            _workContext.CurrentCustomer.Username);

            _campusService.InsertCampus(campus);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult CampusUpdate(CampusModel model)
        //public ActionResult CampusUpdate([Bind(Exclude = "Id")] CampusModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var campus = _campusService.GetCampusById(model.Id);

            if (campus == null)
                return Json(new DataSourceResult() { Errors = "There is no campus with the specified code" });
            //model.reg_date = campus.reg_date;

            //item = model.ToEntity(item);
            campus.Campus_Prefix=model.Campus_Prefix;
            campus.Campus_Title=model.Campus_Title;
            campus.Campus_EN = model.Campus_EN;
            campus.Suburb=model.Suburb;
            campus.State=model.State;
            campus.Address1=model.Address1;
            campus.Address2=model.Address2;
            campus.PostCode=model.PostCode;
            campus.ABN = model.ABN;
            campus.PhoneNo=model.PhoneNo;
            campus.Email=model.Email;
            campus.RoyaltyType=model.RoyaltyType;
            campus.RoyaltyType_HI = model.RoyaltyType_HI;
            campus.RoyaltyType_PA=model.RoyaltyType_PA;
            campus.RoyaltyType_TT=model.RoyaltyType_TT;
            campus.YN_Use=model.YN_Use;
            campus.LicenseeCode = model.LicenseeCode;
            campus.reg_date=DateTime.Now;
            campus.reg_source=_workContext.CurrentCustomer.Username;


            _campusService.UpdateCampus(campus);


            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult CampusDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var campus = _campusService.GetCampusById(id);
            if (campus == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no campus with the specified code" });

            //using (var scope = new System.Transactions.TransactionScope())
            //{

            _campusService.DeleteCampus(campus);

            //    scope.Complete();
            //}

            return new NullJsonResult();
        }

        #endregion    


        # region ETC

        private void SetAvailableCourseCombo(CourseMasterModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var courses = this._courseMasterService.GetAllCourseMasters().Where(x => x.CourseID_P == 0);
            //var branches = this._codeMasterService.GetAllCampus();
            //var codelist = _codeMasterService.GetAllCodeMasters();

            // ordertype, orderstatus, term, year combo
            model.AvailableYear.Clear();
            model.AvailableGrade.Clear();
            model.AvailableTerm.Clear();
            model.AvailableLevel.Clear();
            model.AvailableCourseCategory.Clear();

            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableLevel.Add(new SelectListItem() { Text = "NO Level", Value = "", Selected = true });
            model.AvailableCourseCategory.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            var calendar = _calendarMasterService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y").ToList();
            if ( calendar.Count > 0)
            {
                model.Year = calendar[0].Year;
                model.Term = calendar[0].Term;
            }

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                    case "CM02":
                        model.AvailableTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM03":
                        model.AvailableLevel.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM04":
                        model.AvailableCourseCategory.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                }
            }
        }

        private void SetAvailableCalendarCombo(CalendarMasterModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            //var branches = this._codeMasterService.GetAllCampus();
            //var codelist = _codeMasterService.GetAllCodeMasters();

            // ordertype, orderstatus, term, year combo
            model.AvailableYear.Clear();
            model.AvailableTerm.Clear();
            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM02":
                        model.AvailableTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                }
            }
        }

        private void SetAvailableCampusCombo(CampusModel model)
        {
            /***********************************************************************
             * 검색
            ***********************************************************************/
            /*
            var suburb = this._codeMasterService.GetAllCampus();
            
            model.AvailableSuburb.Clear();
            model.AvailableSuburb.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in suburb)
            {
                model.AvailableSuburb.Add(new SelectListItem() { Value = campus.Suburb, Text = campus.Suburb });
            }
            */
            /***********************************************************************
             * grid 의 리스트 박스에서 사용
            ***********************************************************************/
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableRoyaltyType.Clear();
            model.AvailableRoyaltyType_PA.Clear();
            model.AvailableRoyaltyType_TT.Clear();

            model.AvailableRoyaltyType.Add(new SelectListItem() { Text = "NO Royalty", Value = "", Selected = true });
            model.AvailableRoyaltyType_PA.Add(new SelectListItem() { Text = "NO Royalty", Value = "", Selected = true });
            model.AvailableRoyaltyType_TT.Add(new SelectListItem() { Text = "NO Royalty", Value = "", Selected = true });

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "RY01":
                        model.AvailableRoyaltyType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.RoyaltyType) });
                        break;
                    case "RY02":
                        model.AvailableRoyaltyType_PA.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.RoyaltyType_PA) });
                        break;
                    case "RY03":
                        model.AvailableRoyaltyType_TT.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.RoyaltyType_TT) });
                        break;
                }
            }

        }

        #endregion





    }
}
