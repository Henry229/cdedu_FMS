using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Common;

using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.Common;

using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;

using Nop.Core.Domain.Omni_Backoffice;
using Nop.Services.Omni_Backoffice;
using Nop.Admin.Models.Omni_Item;
using Nop.Admin.Models.Omni_Order;
using Nop.Admin.Models.Omni_Royalty;

namespace Nop.Admin.Controllers
{
    public class Omni_RoyaltyController : BaseAdminController
    {
        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly IItemCategoryService _itemCategoryService;
        private readonly IItemService _itemService;
        private readonly IItemSetService _itemSetService;
        private readonly IOrderService_Omni _orderService;
        private readonly IRoyaltyService _royaltyService;
        private readonly ICourseMasterService _courseService;
        private readonly ICalendarMasterService _calendarService;
        private readonly ICampusService _campusService;
        private readonly IPdfService_Omni _pdfService_omni;
        private readonly IClassService _classService;

        private readonly IPriceFormatter _priceFormatter;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEncryptionService _encryptionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;

        private readonly IPdfService _pdfService;
        private readonly PdfSettings _pdfSettings;

        #endregion

        
        #region Constructors



        public Omni_RoyaltyController(ICodeMasterService codeMasterService, IItemCategoryService itemCategoryService, IItemService itemService,
            IItemSetService itemSetService, IOrderService_Omni orderService, IRoyaltyService royaltyservice, ICourseMasterService courseService,
            ICalendarMasterService calendarService, ICampusService campusService, IPdfService_Omni pdfService_omni, IClassService classservice,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IThemeProvider themeProvider,  IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext, IPdfService pdfService, PdfSettings pdfSettings)
        {
            this._codeMasterService = codeMasterService;
            this._itemCategoryService = itemCategoryService;
            this._itemService = itemService;
            this._itemSetService = itemSetService;
            this._orderService = orderService;
            this._royaltyService = royaltyservice;
            this._courseService = courseService;
            this._calendarService = calendarService;
            this._campusService = campusService;
            this._pdfService_omni = pdfService_omni;
            this._classService = classservice;

            this._priceFormatter = priceFormatter;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._encryptionService = encryptionService;
            this._themeProvider = themeProvider;
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._workContext = workContext;

            this._currencyService = currencyService;
            this._currencySettings = currencySettings;

            this._pdfService = pdfService;
            this._pdfSettings = pdfSettings;
        }

        #endregion 

        private void SetAvailableEnrollmentCombo(EnrollmentModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableCourseList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "" });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = ""});
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add( new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;

            foreach (var course in courses)
            {
                model.AvailableCourseList.Add(new SelectListItem() { Value = course.Id.ToString(), Text = course.CourseName });
            }

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                }
            }
        }

        public ActionResult EnrollmentList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var model = new EnrollmentModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableEnrollmentCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetEnrollmentList(DataSourceRequest command,
            EnrollmentModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "GROP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var courses = _courseService.GetAllCourseMasters();
            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");
            var enrollments = _royaltyService.GetAllEnrollments(model.BranchCode, model.Course_Id);

            var codes = _codeMasterService.GetAllCodeMasters("OR04", "Y");
            var termweek = _calendarService.GetAllCalendarMasters().Where(x => x.Year == model.Year).Where(x=> x.Term == model.Term).ToList();
            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "NORMAL");   // 일반코스인것만 카운트
            // 테스트인것도 엔롤 수정할수 있도록 수정하는데.. 테스트번호를 어떻게 할것인가 문제임 화면 따로 뽑을 것인가 말것인가

            var ordersetgroup = ordersets
                .Join(itemsets, x => x.ItemSet_Id, y => y.Id, (x, y) => new { Order_Id = x.Order_Id, Qty = x.Qty, Week = x.Week, SetCategory = y.SetCategory, Term = y.Term, Grade = y.Grade, Level = y.Level, SetName = y.SetName, Course_Id = x.Course_Id })
                .Join(orders, x => x.Order_Id, y => y.Id,
                (x, y) => new { Year = y.Year, Term = y.Term, BranchCode = y.BranchCode, Course_Id = x.Course_Id, SetCategory = x.SetCategory, Grade = x.Grade, Level = x.Level, SetName = x.SetName, Week = x.Week, Qty = x.Qty })
                .Join(coursetype, x => x.SetCategory, y => y.DetailCode,
                (x, y) => new { Year = x.Year, Term = x.Term, BranchCode = x.BranchCode, Course_Id = x.Course_Id, SetCategory = x.SetCategory, Grade = x.Grade, Level = x.Level, SetName = x.SetName, Week = x.Week, Qty = x.Qty })
                .Join(courses, x => x.Course_Id, y => y.Id, 
                (x, y) => new { Year = x.Year, Term = x.Term, BranchCode = x.BranchCode, Course_Id = x.Course_Id, SetCategory = x.SetCategory, Grade = x.Grade, Level = y.Level, SetName = y.CourseName, Week = x.Week, Qty = x.Qty })
                .GroupBy(x => new { x.BranchCode, x.Course_Id, x.Year, x.Term, x.SetCategory, x.Grade, x.Level, x.SetName })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    BranchCode = group.Key.BranchCode,
                    Course_Id = group.Key.Course_Id,
                    SetName = group.Key.SetName,
                    SetCategory = group.Key.SetCategory,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    Qty1 = group.Sum(a => a.Week == 1 ? a.Qty : 0),
                    Qty2 = group.Sum(a => a.Week == 2 ? a.Qty : 0),
                    Qty3 = group.Sum(a => a.Week == 3 ? a.Qty : 0),
                    Qty4 = group.Sum(a => a.Week == 4 ? a.Qty : 0),
                    Qty5 = group.Sum(a => a.Week == 5 ? a.Qty : 0),
                    Qty6 = group.Sum(a => a.Week == 6 ? a.Qty : 0),
                    Qty7 = group.Sum(a => a.Week == 7 ? a.Qty : 0),
                    Qty8 = group.Sum(a => a.Week == 8 ? a.Qty : 0),
                    Qty9 = group.Sum(a => a.Week == 9 ? a.Qty : 0),
                    Qty10 = group.Sum(a => a.Week == 10 ? a.Qty : 0),
                    Qty11 = group.Sum(a => a.Week == 11 ? a.Qty : 0),
                    Qty12 = group.Sum(a => a.Week == 12 ? a.Qty : 0),
                    Qty13 = group.Sum(a => a.Week == 13 ? a.Qty : 0),
                    Qty14 = group.Sum(a => a.Week == 14 ? a.Qty : 0),
                    Qty15 = group.Sum(a => a.Week == 15 ? a.Qty : 0),
                    Qty16 = group.Sum(a => a.Week == 16 ? a.Qty : 0),
                    Earlybird_W1 = group.Sum(a => a.Week == 98 ? a.Qty : 0),
                    Earlybird_W2 = group.Sum(a => a.Week == 99 ? a.Qty : 0),
                    QtyTotal = group.Sum(a => a.Qty),
                    Code = "OR04"
                })
                .Join(codes, x => x.Code, y => y.CodeType, (x, y) => new
                {
                    BranchCode = x.BranchCode,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = x.Grade,
                    SetCategory = x.SetCategory,
                    Level = x.Level,
                    Course_Id = x.Course_Id,
                    SetName = x.SetName,
                    QtyType = y.DetailCode
                    ,
                    Qty1 = x.Qty1,
                    Qty2 = x.Qty2,
                    Qty3 = x.Qty3,
                    Qty4 = x.Qty4,
                    Qty5 = x.Qty5,
                    Qty6 = x.Qty6,
                    Qty7 = x.Qty7,
                    Qty8 = x.Qty8,
                    Qty9 = x.Qty9,
                    Qty10 = x.Qty10,
                    Qty11 = x.Qty11,
                    Qty12 = x.Qty12,
                    Qty13 = x.Qty13,
                    Qty14 = x.Qty14,
                    Qty15 = x.Qty15
                    ,
                    Qty16 = x.Qty16,
                    QtyTotal = x.QtyTotal,
                    Earlybird_W1 = x.Earlybird_W1,
                    Earlybird_W2 = x.Earlybird_W2
                })
                .OrderBy(y => y.BranchCode)
                .ThenBy(y => y.Grade)
                .ThenBy(y => y.SetCategory)
                .ThenBy(y => y.Level);


            int id = 1;

            var gridModel = new DataSourceResult
            {
                Data = ordersetgroup.Select(x =>
                {
                    var enrollmentModel = new EnrollmentModel()
                    {
                        BranchCode = x.BranchCode,
                        Year = x.Year,
                        Term = x.Term,
                        SetName = x.SetName,
                        Course_Id = x.Course_Id,
                        QtyType = x.QtyType,
                        Week1 = x.Qty1,
                        Week2 = x.Qty2,
                        Week3 = x.Qty3,
                        Week4 = x.Qty4,
                        Week5 = x.Qty5,
                        Week6 = x.Qty6,
                        Week7 = x.Qty7,
                        Week8 = x.Qty8,
                        Week9 = x.Qty9,
                        Week10 = x.Qty10,
                        Week11 = x.Qty11,
                        Week12 = x.Qty12,
                        Week13 = x.Qty13,
                        Week14 = x.Qty14,
                        Week15 = x.Qty15,
                        Week16 = x.Qty16,
                        WeekTotal = x.QtyTotal,
                        Earlybird_W1 = x.Earlybird_W1,
                        Earlybird_W2 = x.Earlybird_W2,
                        WeekDiff = 0,
                        cnt_Week = (termweek.Count > 0 ? termweek[0].Week : 14)
                    };

                    id++;

                    enrollmentModel.Id = id;

                    if ( x.QtyType == "MODY")
                    {
                        var enrolls = enrollments.Where(y => y.Course_Id == x.Course_Id).Where(y => y.BranchCode == x.BranchCode);

                        var weekdiff = enrollmentModel.WeekTotal;

                        var weektotal = 0;

                        foreach(var enroll in enrolls)
                        {
                            switch(enroll.Week)
                            {
                                case 1: enrollmentModel.Week1 = enroll.Qty_Modified; break;
                                case 2: enrollmentModel.Week2 = enroll.Qty_Modified; break;
                                case 3: enrollmentModel.Week3 = enroll.Qty_Modified; break;
                                case 4: enrollmentModel.Week4 = enroll.Qty_Modified; break;
                                case 5: enrollmentModel.Week5 = enroll.Qty_Modified; break;
                                case 6: enrollmentModel.Week6 = enroll.Qty_Modified; break;
                                case 7: enrollmentModel.Week7 = enroll.Qty_Modified; break;
                                case 8: enrollmentModel.Week8 = enroll.Qty_Modified; break;
                                case 9: enrollmentModel.Week9 = enroll.Qty_Modified; break;
                                case 10: enrollmentModel.Week10 = enroll.Qty_Modified; break;
                                case 11: enrollmentModel.Week11 = enroll.Qty_Modified; break;
                                case 12: enrollmentModel.Week12 = enroll.Qty_Modified; break;
                                case 13: enrollmentModel.Week13 = enroll.Qty_Modified; break;
                                case 14: enrollmentModel.Week14 = enroll.Qty_Modified; break;
                                case 15: enrollmentModel.Week15 = enroll.Qty_Modified; break;
                                case 16: enrollmentModel.Week16 = enroll.Qty_Modified; break;
                                case 98: enrollmentModel.Earlybird_W1 = enroll.Qty_Modified; break;
                                case 99: enrollmentModel.Earlybird_W2 = enroll.Qty_Modified; break;
                            }
                        }

                        enrollmentModel.WeekTotal = enrollmentModel.Week1 + enrollmentModel.Week2 + enrollmentModel.Week3 + enrollmentModel.Week4 + enrollmentModel.Week5
                            + enrollmentModel.Week6 + enrollmentModel.Week7 + enrollmentModel.Week8 + enrollmentModel.Week9 + enrollmentModel.Week10 + enrollmentModel.Week11
                            + enrollmentModel.Week12 + enrollmentModel.Week13 + enrollmentModel.Week14 + enrollmentModel.Week15 + enrollmentModel.Week16;
                        enrollmentModel.WeekDiff = weekdiff - enrollmentModel.WeekTotal;
                    }

                    return enrollmentModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = ordersetgroup.ToList().Count
            };

            return Json(gridModel);

        }


        public ActionResult EnrollmentList_TEST()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var model = new TestRoyaltyModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableTestRoyaltyCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetEnrollmentList_TEST(DataSourceRequest command,
            TestRoyaltyModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "TEST", "").Where(x => x.Status.Substring(0, 3) != "ST0");

            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "TEST");   // 테스트인것만 카운트
            var courses = _courseService.GetAllCourseMasters().Where(x => x.Term == model.Term).Join(coursetype, x => x.CourseCategory, y => y.DetailCode, (x, y)
                => new
                {
                    Year = x.Year,
                    Term = x.Term,
                    Grade = x.Grade,
                    Level = x.Level,
                    CourseCategory = x.CourseCategory,
                    CourseName = x.CourseName,
                    CourseFee = x.CourseFee,
                    TotalWeek = x.TotalWeek,
                    StartWeek = x.StartWeek,
                    Id = x.Id
                }); 

            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");
            var enrollments = _royaltyService.GetAllEnrollments(model.BranchCode, model.Course_Id);

            var codes = _codeMasterService.GetAllCodeMasters("OR05", "Y");
            var termweek = _calendarService.GetAllCalendarMasters().Where(x => x.Year == model.Year).Where(x => x.Term == model.Term).ToList();


            var discount = _codeMasterService.GetAllCodeMasters("RY03", "Y");
            var campus = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType, y => y.DetailCode, (x, y)
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, Discount2 = y.Val_02, RoyaltyType = y.DetailName });



            var ordersetgroup = ordersets
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new { Order_Id = x.Order_Id, Qty = x.Qty, Week = x.Week, SetCategory = y.CourseCategory, Term = y.Term, Grade = y.Grade, Level = y.Level, CourseName = y.CourseName, Course_Id = x.Course_Id, CourseWeek = y.TotalWeek, StartWeek = y.StartWeek, CourseFee = y.CourseFee })  // startweek 코스수정완료후 변경
                .Join(orders, x => x.Order_Id, y => y.Id,
                (x, y) => new { Year = y.Year, Term = y.Term, BranchCode = y.BranchCode, Course_Id = x.Course_Id, SetCategory = x.SetCategory, Grade = x.Grade, Level = x.Level, CourseName = x.CourseName, Week = x.Week, Qty = x.Qty, CourseWeek = x.CourseWeek, StartWeek = x.StartWeek, CourseFee = x.CourseFee })
                .Join(coursetype, x => x.SetCategory, y => y.DetailCode,
                (x, y) => new { Year = x.Year, Term = x.Term, BranchCode = x.BranchCode, Course_Id = x.Course_Id, SetCategory = x.SetCategory, Grade = x.Grade, Level = x.Level, CourseName = x.CourseName, Week = x.Week, Qty = x.Qty, CourseWeek = x.CourseWeek, StartWeek = x.StartWeek, CourseFee = x.CourseFee })
                .GroupBy(x => new { x.BranchCode, x.Course_Id, x.Year, x.Term, x.SetCategory, x.Grade, x.Level, x.CourseName, x.CourseWeek, x.StartWeek, x.CourseFee })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    BranchCode = group.Key.BranchCode,
                    Course_Id = group.Key.Course_Id,
                    CourseName = group.Key.CourseName,
                    SetCategory = group.Key.SetCategory,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek,
                    CourseFee = group.Key.CourseFee,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    Qty1 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 1 ? a.Qty : 0),
                    Qty2 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 2 ? a.Qty : 0),
                    Qty3 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 3 ? a.Qty : 0),
                    Qty4 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 4 ? a.Qty : 0),
                    Qty5 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 5 ? a.Qty : 0),
                    Qty6 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 6 ? a.Qty : 0),
                    Qty7 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 7 ? a.Qty : 0),
                    Qty8 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 8 ? a.Qty : 0),
                    Qty9 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 9 ? a.Qty : 0),
                    Qty10 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 10 ? a.Qty : 0),
                    Qty11 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 11 ? a.Qty : 0),
                    Qty12 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 12 ? a.Qty : 0),
                    Qty13 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 13 ? a.Qty : 0),
                    Qty14 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 14 ? a.Qty : 0),
                    Qty15 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 15 ? a.Qty : 0),
                    Qty16 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 16 ? a.Qty : 0),
                    QtyTotal = group.Sum(a => a.Qty),
                    Code = "OR05"
                })
                .Join(codes, x => x.Code, y => y.CodeType, (x, y) => new
                {
                    BranchCode = x.BranchCode,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = x.Grade,
                    SetCategory = x.SetCategory,
                    Level = x.Level,
                    Course_Id = x.Course_Id,
                    CourseName = x.CourseName,
                    TotalWeek = x.CourseWeek,
                    StartWeek = x.StartWeek,
                    CourseFee = x.CourseFee,
                    CourseWeek = x.CourseWeek,
                    Gubun = y.DetailCode,
                    Qty1 = (y.DetailCode == "TENO") ? x.StartWeek : x.Qty1,
                    Qty2 = (y.DetailCode == "TENO") ? (x.CourseWeek < 2) ? 0 : x.StartWeek + 1 : x.Qty2,
                    Qty3 = (y.DetailCode == "TENO") ? (x.CourseWeek < 3) ? 0 : x.StartWeek + 2 : x.Qty3,
                    Qty4 = (y.DetailCode == "TENO") ? (x.CourseWeek < 4) ? 0 : x.StartWeek + 3 : x.Qty4,
                    Qty5 = (y.DetailCode == "TENO") ? (x.CourseWeek < 5) ? 0 : x.StartWeek + 4 : x.Qty5,
                    Qty6 = (y.DetailCode == "TENO") ? (x.CourseWeek < 6) ? 0 : x.StartWeek + 5 : x.Qty6,
                    Qty7 = (y.DetailCode == "TENO") ? (x.CourseWeek < 7) ? 0 : x.StartWeek + 6 : x.Qty7,
                    Qty8 = (y.DetailCode == "TENO") ? (x.CourseWeek < 8) ? 0 : x.StartWeek + 7 : x.Qty8,
                    Qty9 = (y.DetailCode == "TENO") ? (x.CourseWeek < 9) ? 0 : x.StartWeek + 8 : x.Qty9,
                    Qty10 = (y.DetailCode == "TENO") ? (x.CourseWeek < 10) ? 0 : x.StartWeek + 9 : x.Qty10,
                    Qty11 = (y.DetailCode == "TENO") ? (x.CourseWeek < 11) ? 0 : x.StartWeek + 10 : x.Qty11,
                    Qty12 = (y.DetailCode == "TENO") ? (x.CourseWeek < 12) ? 0 : x.StartWeek + 11 : x.Qty12,
                    Qty13 = (y.DetailCode == "TENO") ? (x.CourseWeek < 13) ? 0 : x.StartWeek + 12 : x.Qty13,
                    Qty14 = (y.DetailCode == "TENO") ? (x.CourseWeek < 14) ? 0 : x.StartWeek + 13 : x.Qty14,
                    Qty15 = (y.DetailCode == "TENO") ? (x.CourseWeek < 15) ? 0 : x.StartWeek + 14 : x.Qty15,
                    Qty16 = (y.DetailCode == "TENO") ? (x.CourseWeek < 16) ? 0 : x.StartWeek + 15 : x.Qty16,
                    QtyTotal = (y.DetailCode == "TENO") ? 0 : x.QtyTotal
                })
                .OrderBy(y => y.BranchCode)
                .ThenBy(y => y.Grade)
                .ThenBy(y => y.SetCategory)
                .ThenBy(y => y.Level);

            StreamWriter writer = new StreamWriter(Server.MapPath("Log.txt"), true);
            int maxweek = 0;

            if (ordersetgroup.ToList().Count() > 0)
                maxweek = ordersetgroup.Max(x => x.TotalWeek);


            int id = 1;

            var gridModel = new DataSourceResult
            {
                Data = ordersetgroup.Select(x =>
                {
                    var testroyaltyModel = new TestRoyaltyModel()
                    {
                        BranchCode = x.BranchCode,
                        Year = x.Year,
                        Term = x.Term,
                        CourseName = x.CourseName,
                        Course_Id = x.Course_Id,
                        Gubun = x.Gubun,
                        Week1 = x.Qty1,
                        Week2 = x.Qty2,
                        Week3 = x.Qty3,
                        Week4 = x.Qty4,
                        Week5 = x.Qty5,
                        Week6 = x.Qty6,
                        Week7 = x.Qty7,
                        Week8 = x.Qty8,
                        Week9 = x.Qty9,
                        Week10 = x.Qty10,
                        Week11 = x.Qty11,
                        Week12 = x.Qty12,
                        Week13 = x.Qty13,
                        Week14 = x.Qty14,
                        Week15 = x.Qty15,
                        Week16 = x.Qty16,
                        WeekTotal = x.QtyTotal,
                        cnt_Week = maxweek,
                        TestFee = x.CourseFee,
                        Royalty = x.CourseFee * x.QtyTotal
                    };

                    id++;

                    testroyaltyModel.Id = id;

                    if (x.Gubun == "MODY")
                    {
                        var enrolls = enrollments.Where(y => y.Course_Id == x.Course_Id).Where(y => y.BranchCode == x.BranchCode);

                        for ( int i = x.StartWeek; i < 16 + x.StartWeek ; i++) 
                        {
                            var enroll = enrolls.Where(y => y.Week == i).ToList();

                            if (enroll.Count > 0)
                            {
                                switch (enroll[0].Week - x.StartWeek + 1)
                                {
                                    case 1: testroyaltyModel.Week1 = enroll[0].Qty_Modified; break;
                                    case 2: testroyaltyModel.Week2 = enroll[0].Qty_Modified; break;
                                    case 3: testroyaltyModel.Week3 = enroll[0].Qty_Modified; break;
                                    case 4: testroyaltyModel.Week4 = enroll[0].Qty_Modified; break;
                                    case 5: testroyaltyModel.Week5 = enroll[0].Qty_Modified; break;
                                    case 6: testroyaltyModel.Week6 = enroll[0].Qty_Modified; break;
                                    case 7: testroyaltyModel.Week7 = enroll[0].Qty_Modified; break;
                                    case 8: testroyaltyModel.Week8 = enroll[0].Qty_Modified; break;
                                    case 9: testroyaltyModel.Week9 = enroll[0].Qty_Modified; break;
                                    case 10: testroyaltyModel.Week10 = enroll[0].Qty_Modified; break;
                                    case 11: testroyaltyModel.Week11 = enroll[0].Qty_Modified; break;
                                    case 12: testroyaltyModel.Week12 = enroll[0].Qty_Modified; break;
                                    case 13: testroyaltyModel.Week13 = enroll[0].Qty_Modified; break;
                                    case 14: testroyaltyModel.Week14 = enroll[0].Qty_Modified; break;
                                    case 15: testroyaltyModel.Week15 = enroll[0].Qty_Modified; break;
                                    case 16: testroyaltyModel.Week16 = enroll[0].Qty_Modified; break;
                                }
                            }

                        }

                        testroyaltyModel.WeekTotal = testroyaltyModel.Week1 + testroyaltyModel.Week2 + testroyaltyModel.Week3 + testroyaltyModel.Week4 + testroyaltyModel.Week5
                            + testroyaltyModel.Week6 + testroyaltyModel.Week7 + testroyaltyModel.Week8 + testroyaltyModel.Week9 + testroyaltyModel.Week10 + testroyaltyModel.Week11
                            + testroyaltyModel.Week12 + testroyaltyModel.Week13 + testroyaltyModel.Week14 + testroyaltyModel.Week15 + testroyaltyModel.Week16;
                    }

                    else if (x.Gubun == "ACCU")  
                    {
                        var enrolls = enrollments.Where(y => y.Course_Id == x.Course_Id).Where(y => y.BranchCode == x.BranchCode);

                        var enrollcum = 0;

                        for (int i = x.StartWeek; i < x.CourseWeek + x.StartWeek ; i++)
                        {
                            var enroll = enrolls.Where(y => y.Week == i).ToList();



                            if (enroll.Count > 0)
                            {
                                enrollcum += enroll[0].Qty_Modified;
                            }
                            else 
                            {
                                switch (i - x.StartWeek + 1)
                                {
                                    case 1: enrollcum +=  testroyaltyModel.Week1; break;
                                    case 2: enrollcum += testroyaltyModel.Week2; break;
                                    case 3: enrollcum += testroyaltyModel.Week3; break;
                                    case 4: enrollcum += testroyaltyModel.Week4; break;
                                    case 5: enrollcum += testroyaltyModel.Week5; break;
                                    case 6: enrollcum += testroyaltyModel.Week6; break;
                                    case 7: enrollcum += testroyaltyModel.Week7; break;
                                    case 8: enrollcum += testroyaltyModel.Week8; break;
                                    case 9: enrollcum += testroyaltyModel.Week9; break;
                                    case 10: enrollcum += testroyaltyModel.Week10; break;
                                    case 11: enrollcum += testroyaltyModel.Week11; break;
                                    case 12: enrollcum += testroyaltyModel.Week12; break;
                                    case 13: enrollcum += testroyaltyModel.Week13; break;
                                    case 14: enrollcum += testroyaltyModel.Week14; break;
                                    case 15: enrollcum += testroyaltyModel.Week15; break;
                                    case 16: enrollcum += testroyaltyModel.Week16; break;
                                }
                            }

                            if (i > x.TotalWeek + x.StartWeek)
                            {
                                enrollcum = 0;
                            }


                            switch (i - x.StartWeek + 1)
                            {
                                case 1: testroyaltyModel.Week1 = enrollcum; break;
                                case 2: testroyaltyModel.Week2 = enrollcum; break;
                                case 3: testroyaltyModel.Week3 = enrollcum; break;
                                case 4: testroyaltyModel.Week4 = enrollcum; break;
                                case 5: testroyaltyModel.Week5 = enrollcum; break;
                                case 6: testroyaltyModel.Week6 = enrollcum; break;
                                case 7: testroyaltyModel.Week7 = enrollcum; break;
                                case 8: testroyaltyModel.Week8 = enrollcum; break;
                                case 9: testroyaltyModel.Week9 = enrollcum; break;
                                case 10: testroyaltyModel.Week10 = enrollcum; break;
                                case 11: testroyaltyModel.Week11 = enrollcum; break;
                                case 12: testroyaltyModel.Week12 = enrollcum; break;
                                case 13: testroyaltyModel.Week13 = enrollcum; break;
                                case 14: testroyaltyModel.Week14 = enrollcum; break;
                                case 15: testroyaltyModel.Week15 = enrollcum; break;
                                case 16: testroyaltyModel.Week16 = enrollcum; break;
                            }


                        }

                        testroyaltyModel.WeekTotal = enrollcum;
                    }


                    return testroyaltyModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = ordersetgroup.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult EnrollmentUpdate_TEST(TestRoyaltyModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            if (model.Gubun != "MODY")
            {
                return Json(new DataSourceResult() { Errors = "data can't be modified. Please update student qty only" });
            }


            var closing = _royaltyService.GetClosing(model.Year, model.Term, "TEST", model.BranchCode);

            if (closing != null && closing.YN_Closing == "Y")
            {
                return Json(new DataSourceResult() { Errors = "data can't be modified. Term order closed" });
            }

            // order데이터

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "TEST", "")
                .Where(x => x.Status.Substring(0, 3) != "ST0" );

            var ordersets = _orderService.GetAllOrderbyItemSets().Where(x => x.Course_Id == model.Course_Id);

            var courseorder = orders.Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { y.Week, y.Qty })
                .GroupBy(x => new { x.Week }).Select(group => new { Week = group.Key.Week, Qty = group.Sum(a => a.Qty) });

            // 기존데이터
            var enroll_before = _royaltyService.GetAllEnrollments(model.BranchCode, model.Course_Id);


            // 새로 입력

            var qty = 0;
            var course = _courseService.GetCourseMasterById(model.Course_Id);
            
            for (int i = course.StartWeek; i < course.StartWeek + 16; i++)
            {
                switch (i - course.StartWeek + 1)
                {
                    case 1: qty = model.Week1; break;
                    case 2: qty = model.Week2; break;
                    case 3: qty = model.Week3; break;
                    case 4: qty = model.Week4; break;
                    case 5: qty = model.Week5; break;
                    case 6: qty = model.Week6; break;
                    case 7: qty = model.Week7; break;
                    case 8: qty = model.Week8; break;
                    case 9: qty = model.Week9; break;
                    case 10: qty = model.Week10; break;
                    case 11: qty = model.Week11; break;
                    case 12: qty = model.Week12; break;
                    case 13: qty = model.Week13; break;
                    case 14: qty = model.Week14; break;
                    case 15: qty = model.Week15; break;
                    case 16: qty = model.Week16; break; 
                }

                var enroll_before_week = enroll_before.Where(x => x.Week == i).ToList();
                var courseorder_week = courseorder.Where(x => x.Week == i).ToList();

                var orderqty = 0;
                if (courseorder_week.Count > 0)
                    orderqty = courseorder_week[0].Qty;

                var beforeqty = 0;
                if (enroll_before_week.Count > 0 )
                {
                    beforeqty = enroll_before_week[0].Qty_Modified;
                }

                //일단 만들어놓고
                var enroll_new = new Enrollment(model.BranchCode, model.Course_Id, i, 0, qty, DateTime.Now, _workContext.CurrentCustomer.Username);

                if ( enroll_before_week.Count != 0 )  // 있었다면
                {
                    if ( qty != beforeqty)
                    {
                        foreach (var enroll in enroll_before_week)
                        {
                            _royaltyService.DeleteEnrollment(enroll);
                        }

                        if ( qty != orderqty )
                        {
                            _royaltyService.InsertEnrollment(enroll_new);
                        }
                    }
                }
                else  //없었다면 
                {
                    if ( qty != orderqty )
                    {
                        _royaltyService.InsertEnrollment(enroll_new);
                    }

                }


            }


            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult EnrollmentUpdate(EnrollmentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            if (model.QtyType != "MODY" )
            {
                return Json(new DataSourceResult() { Errors = "data can't be modified. Please update student qty only" });
            }

            if (model.Week1 < model.Earlybird_W1 || model.Week2 < model.Earlybird_W2)
            {
                return Json(new DataSourceResult() { Errors = "The early bird exceed the number of enrolment" });
            }

            var closing = _royaltyService.GetClosing(model.Year, model.Term, "ROYL", model.BranchCode);

            if ( closing != null && closing.YN_Closing == "Y")
            {
                return Json(new DataSourceResult() { Errors = "data can't be modified. Term order closed" });
            }

            // order데이터

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "GROP", "")
                .Where(x => x.Status.Substring(0, 3) != "ST0" );
            var ordersets = _orderService.GetAllOrderbyItemSets().Where(x => x.Course_Id == model.Course_Id);

            var courseorder = orders.Join(ordersets , x => x.Id, y => y.Order_Id, (x,y) => new {  y.Week, y.Qty } )
                .GroupBy( x => new {x.Week}).Select(group => new {Week = group.Key.Week, Qty = group.Sum(a => a.Qty)});

            // 기존데이터
            var enroll_before = _royaltyService.GetAllEnrollments(model.BranchCode, model.Course_Id);

            // 새로 입력
            var qty = 0;

            for (int i = 1; i <= 99; i++ )
            {
                switch (i) {
                    case 1: qty = model.Week1; break;
                    case 2: qty = model.Week2; break;
                    case 3: qty = model.Week3; break;
                    case 4: qty = model.Week4; break;
                    case 5: qty = model.Week5; break;
                    case 6: qty = model.Week6; break;
                    case 7: qty = model.Week7; break;
                    case 8: qty = model.Week8; break;
                    case 9: qty = model.Week9; break;
                    case 10: qty = model.Week10; break;
                    case 11: qty = model.Week11; break;
                    case 12: qty = model.Week12; break;
                    case 13: qty = model.Week13; break;
                    case 14: qty = model.Week14; break;
                    case 15: qty = model.Week15; break;
                    case 16: qty = model.Week16; break;
                    case 17: qty = model.Earlybird_W1; i = 98; break;
                    case 99: qty = model.Earlybird_W2; break;
                }

                var enroll_before_week = enroll_before.Where(x => x.Week == i).ToList();
                var courseorder_week = courseorder.Where(x => x.Week == i).ToList();

                var orderqty = 0;
                if (courseorder_week.Count > 0)
                    orderqty = courseorder_week[0].Qty;

                var beforeqty = 0;
                if (enroll_before_week.Count > 0)
                {
                    beforeqty = enroll_before_week[0].Qty_Modified;
                }

                //일단 만들어놓고
                var enroll_new = new Enrollment(model.BranchCode, model.Course_Id, i, 0, qty, DateTime.Now, _workContext.CurrentCustomer.Username);

                if (enroll_before_week.Count != 0)  // 있었다면
                {
                    if ( qty != beforeqty)
                    {
                        foreach (var enroll in enroll_before_week)
                        {
                            _royaltyService.DeleteEnrollment(enroll);
                        }

                        if (qty != orderqty)
                        {
                            _royaltyService.InsertEnrollment(enroll_new);
                        }
                    }
                }
                else  //없었다면 
                {
                    if (qty != orderqty)
                    {
                        _royaltyService.InsertEnrollment(enroll_new);
                    }

                }

            }

            return new NullJsonResult();
        }



        private void SetAvailableMeterialCombo(MaterialFeeModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                }
            }
        }

        public ActionResult MaterialRoyaltyList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new MaterialFeeModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableMeterialCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetMaterialFeeList(DataSourceRequest command,
            MaterialFeeModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "GROP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var courses = _courseService.GetAllCourseMasters();
            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");

            var discount = _codeMasterService.GetAllCodeMasters("OR03", "Y");
            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "NORMAL");  // 일반코스인것만 카운트

            var teacherdiscount = discount.Where( x => x.DetailCode == "TCHR").ToList()[0].Val_01;
            var incentivediscount = discount.Where( x => x.DetailCode == "STUD").ToList()[0].Val_01;
            var onlinefacility = discount.Where(x => x.DetailCode == "STUD").ToList()[0].Val_02;  // 처음엔 기준정보에서 가져왔으나 코스마다 따로 설정으로 변경


            var bookorders = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Qty_T = y.Qty_Teacher, Qty_N = y.Qty_NewBook, isHalf = y.is_Half, CourseId = y.Course_Id })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.CourseId })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Qty = group.Sum(a => a.isHalf == "N" ? a.Qty : 0),
                    Qty_H = group.Sum(a => a.isHalf == "Y" ? a.Qty : 0),
                    Qty_T = group.Sum(a => a.Qty_T),
                    Qty_N = group.Sum(a => a.Qty_N)
                })
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    BranchCode = x.Branch,
                    CourseType = y.CourseCategory,
                    Year = x.Year,
                    Term = x.Term,
                    Course_Id = x.Course_Id,
                    Course_Name = y.CourseName,
                    MaterialFee = y.BookFee,
                    NewBookFee = y.NewBookFee,
                    Qty_Student = x.Qty,
                    Qty_Half = x.Qty_H,
                    Qty_Teacher = x.Qty_T,
                    Qty_NewBook = x.Qty_N,
                    Grade = y.Grade,
                    Subject = (y.CourseCategory == "HIGH") ? y.Level : "",     // 하이스쿨은 레벨이 과목역할을 하니까 따로 분리한다 과목필드를 만들껄 그랬나.. 싶기도하다
                    Facility = (y.CourseCategory == "HIGH") ? 0 : y.Facility // 하이스쿨은 온라인 없음
                })
                .Join(coursetype, x => x.CourseType, y => y.DetailCode, (x, y)
                    => new { x.BranchCode, x.CourseType, x.Year, x.Term, x.Course_Id, x.Course_Name, x.MaterialFee, x.Qty_Student, x.Qty_Half, x.Qty_Teacher, x.Qty_NewBook, x.Grade, x.Subject, x.Facility, x.NewBookFee })
                .GroupBy(group => new { group.Year, group.Term, group.Grade, group.CourseType, group.BranchCode, group.MaterialFee, group.Subject, group.Facility, group.NewBookFee })
                .Select(group => new {
                    BranchCode = group.Key.BranchCode,
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    MaterialFee = group.Key.MaterialFee,
                    facility = group.Key.Facility,
                    Qty_Student = group.Sum(a => a.Qty_Student),
                    Qty_Half = group.Sum(a => a.Qty_Half),
                    Qty_Teacher = group.Sum(a => a.Qty_Teacher),
                    Qty_NewBook = group.Sum(a => a.Qty_NewBook),
                    Course_Name = group.Max(a => a.Course_Name).Split(' ')[0] + " " + group.Max(a => a.Course_Name).Split(' ')[1],
                    NewBookFee = group.Key.NewBookFee,
                    CourseType = group.Key.CourseType
                })
                .OrderBy(x => x.BranchCode)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.Course_Name);


            int id = 1;
            

            var gridModel = new DataSourceResult
            {
                Data = bookorders.Select(x =>
                {
                    var materialfeeModel = new MaterialFeeModel()
                    {
                        BranchCode = x.BranchCode,
                        Course_Name = x.Course_Name,
                        MaterialFee = x.MaterialFee,
                        Facility = x.facility,
                        FeewithOutFacility = x.MaterialFee - x.facility,
                        Qty_Student = x.Qty_Student,
                        Qty_Half = x.Qty_Half,
                        Qty_Teacher = x.Qty_Teacher,
                        Qty_NewBook = x.Qty_NewBook,
                        Amt_Student = (x.MaterialFee - x.facility) * x.Qty_Student,
                        Amt_Half = (x.MaterialFee - x.facility) * x.Qty_Half * Convert.ToDecimal( 0.5 ),
                        Amt_Teacher = x.MaterialFee * x.Qty_Teacher * teacherdiscount / 100,
                        Amt_NewBook = x.NewBookFee * x.Qty_NewBook,
                        Discount = Math.Round((((x.MaterialFee - x.facility) * x.Qty_Student) + (x.NewBookFee * x.Qty_NewBook) + (x.MaterialFee - x.facility) * x.Qty_Half * Convert.ToDecimal(0.5)
                                     //   + (x.MaterialFee - x.facility) * x.Qty_Teacher * teacherdiscount / 100) * incentivediscount / 100,2)
                                    ) * incentivediscount / 100, 2),
                        Amt_BeforeDiscount = Math.Round((((x.MaterialFee - x.facility) * x.Qty_Student) + (x.NewBookFee * x.Qty_NewBook) + (x.MaterialFee - x.facility) * x.Qty_Half * Convert.ToDecimal(0.5)
                                        + (x.MaterialFee - x.facility) * x.Qty_Teacher * teacherdiscount / 100),2),
                        Amt_Total = 0
                    };

                    if (x.CourseType == "HIGH" || x.Term.Substring(0, 1) == "V")// 하이스쿨은 인센티브 없음 holiday도 없음
                    {
                        materialfeeModel.Discount = 0;
                        
                    }

                    //회사 운영방침 변경에 따라, NSW FMS 업데이트가 필요 -> 브라이언실장
                    //2022년 term 3 부터 교재 로열티에 제공되던 5 % 브랜치 서포트 비용이 없어짐.
                    //VIC는 0으로 되어 있고 NSW만 반영처리함.
                    //(H)Incentives 값은 0으로 셋팅 요청에 따라 처리
                    //2022.07.30 이메일로 요청받음.
                    materialfeeModel.Discount = 0;

                    materialfeeModel.Amt_Total = materialfeeModel.Amt_BeforeDiscount - materialfeeModel.Discount + (materialfeeModel.Qty_Student + materialfeeModel.Qty_Half) * materialfeeModel.Facility;

                    return materialfeeModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = bookorders.ToList().Count
            };

            return Json(gridModel);

        }

        private void SetAvailableCourseRoyaltyCombo(CourseRoyaltyModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;


            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                }
            }
        }

        public ActionResult CourseRoyaltyList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new CourseRoyaltyModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableCourseRoyaltyCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetCourseRoyaltyList(DataSourceRequest command,
            CourseRoyaltyModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "GROP", "").Where(x => x.Status.Substring(0,3) != "ST0");
            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "NORMAL");   // 일반코스인것만 카운트
            var courses = _courseService.GetAllCourseMasters().Where(x => x.Term == model.Term && x.Year == model.Year).Join(coursetype, x => x.CourseCategory, y => y.DetailCode, (x, y)
                            => new
                            {
                                Id = x.Id,
                                Year = x.Year,
                                Term = x.Term,
                                Grade = x.Grade,
                                Level = x.Level,
                                CourseCategory = x.CourseCategory,
                                CourseName = x.CourseName,
                                CourseFee = x.CourseFee,
                                TotalWeek = x.TotalWeek
                            }); 
            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");

            var discount = _codeMasterService.GetAllCodeMasters("RY01", "Y");
            var earlybird = _codeMasterService.GetAllCodeMasters("OR03", "Y").Where(x => x.DetailCode == "EARL").ToList();  
            
            var campus = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType, y => y.DetailCode, (x, y)
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, RoyaltyType = y.DetailName });
            var campus2 = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType_HI, y => y.DetailCode, (x, y)  // 하이스쿨용 로열티
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, RoyaltyType = y.DetailName });

            try
            {
                // Enrol 내역 집계
                var enrolls = _royaltyService.GetAllEnrollments(model.BranchCode, 0)
                    .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                    {
                        Year = y.Year,
                        Term = y.Term,
                        Grade = y.Grade,
                        Level = y.Level,
                        CourseType = y.CourseCategory,
                        CourseName = y.CourseName,
                        CourseFee = y.CourseFee,
                        Branch = x.BranchCode,
                        CourseId = x.Course_Id,
                        Week = x.Week,
                        CourseWeek = y.TotalWeek,
                        Qty = x.Qty_Modified
                    }).Where(x => x.Year == model.Year)
                    .GroupBy(group => new
                    {
                        group.Branch,
                        group.CourseId,
                        group.CourseWeek,
                        group.Week,
                        group.Year,
                        group.CourseType,
                        group.Term,
                        group.Grade,
                        group.Level,
                        group.CourseFee
                    })
                    .Select(group => new
                    {
                        Year = group.Key.Year,
                        Term = group.Key.Term,
                        Grade = group.Key.Grade,
                        Level = group.Key.Level,
                        CourseType = group.Key.CourseType,
                        CourseName = group.Max(a => a.CourseName).Split(' ')[0] + " " + group.Max(a => a.CourseName).Split(' ')[1],
                        Branch = group.Key.Branch,
                        CourseId = group.Key.CourseId,
                        CourseFee = group.Key.CourseFee,
                        CourseWeek = group.Key.CourseWeek,
                        Week = group.Key.Week,
                        Qty = group.Sum(a => a.Qty),
                        Qty_Cum = group.Sum(a => a.Week >= 98 ? 0 : (a.CourseWeek - a.Week + 1) * a.Qty)
                    });

                // order 집계
                var courseorder = orders
                    .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Week = y.Week, CourseId = y.Course_Id })
                    .Join(courses, x => x.CourseId, y => y.Id, (x, y) => new
                    {
                        Branch = x.Branch,
                        Year = x.Year,
                        Term = x.Term,
                        Grade = y.Grade,
                        Level = y.Level,
                        Order_Id = x.Order_Id,
                        Qty = x.Qty,
                        Week = x.Week,
                        CourseId = x.CourseId,
                        CourseName = y.CourseName,
                        CourseFee = y.CourseFee,
                        CourseWeek = y.TotalWeek,
                        CourseType = y.CourseCategory
                    })
                    .GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseId, group.Level, group.CourseType, group.Week })
                    .Select(group => new
                    {
                        Year = group.Key.Year,
                        Term = group.Key.Term,
                        Grade = group.Key.Grade,
                        Level = group.Key.Level,
                        CourseType = group.Key.CourseType,
                        Branch = group.Key.Branch,
                        Course_Id = group.Key.CourseId,
                        Week = group.Key.Week,
                        Qty = group.Sum(a => a.Qty),
                        Qty_Cum = group.Sum(a => (a.CourseWeek - a.Week + 1) * a.Qty),
                        CourseName = group.Max(a => a.CourseName).Split(' ')[0] + " " + group.Max(a => a.CourseName).Split(' ')[1],
                        CourseFee = group.Key.CourseFee,
                        CourseWeek = group.Key.CourseWeek
                    }).ToList();

                // order와 enrol 을 집계하여 최종결과 산출

                // enroll이 있으면 지운다
                for (int i = courseorder.Count - 1; i >= 0; i-- )
                {
                    //Course Royalty 에서 All Branch 결과 틀리게 나오는 것 수정
                    var enroll = enrolls.Where(x => x.Week == courseorder[i].Week && x.CourseId == courseorder[i].Course_Id && x.Branch == courseorder[i].Branch).ToList();
                    if ( enroll.Count > 0 )
                        courseorder.Remove(courseorder[i]);
                }
                // enroll을 넣는다

                foreach (var enroll in enrolls)
                {
                    courseorder.Add(new
                    {
                        Year = enroll.Year,
                        Term = enroll.Term,
                        Grade = enroll.Grade,
                        Level = enroll.Level,
                        CourseType = enroll.CourseType,
                        Branch = enroll.Branch,
                        Course_Id = enroll.CourseId,
                        Week = enroll.Week,
                        Qty = enroll.Qty,
                        Qty_Cum = enroll.Qty_Cum,
                        CourseName = enroll.CourseName,
                        CourseFee = enroll.CourseFee,
                        CourseWeek = enroll.CourseWeek
                    });
                }

                var final = courseorder.Select(x => new
                            {
                                Year = x.Year,
                                Term = x.Term,
                                Grade = x.Grade,
                                Level = x.Level,
                                CourseType = x.CourseType,
                                Branch = x.Branch,
                                Course_Id = x.Course_Id,
                                Qty = x.Week >= 98 ? 0 : x.Qty,
                                Qty_Cum = x.Qty_Cum,
                                CourseName = x.CourseName,
                                CourseFee = x.CourseFee,
                                CourseWeek = x.CourseWeek,
                                Early_W1 = x.Week == 98 ? x.Qty : 0,
                                Early_W2 = x.Week == 99 ? x.Qty : 0,
                                Subject = (x.CourseType == "HIGH") ? x.Level : ""

                            });

                var final2 = final.GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseName, group.Subject, group.CourseType })
                    .Select(group => new
                    {
                        group.Key.Year,
                        group.Key.Term,
                        group.Key.Grade,
                        group.Key.Branch,
                        group.Key.CourseType,
                        Qty_Cum = group.Sum(a => a.Qty_Cum),
                        CourseName = group.Key.CourseName,
                        CourseFee = group.Key.CourseFee,
                        CourseWeek = group.Key.CourseWeek,
                        Early_W1 = group.Sum(a => a.Early_W1),
                        Early_W2 = group.Sum(a => a.Early_W2),
                    }).OrderBy(x => x.Branch)
                    .ThenBy(x => x.Grade)
                    .ThenBy(x => x.CourseName);



                var gridModel = new DataSourceResult
                {
                    Data = final2.Select(x =>
                    {
                        var courseroyaltyModel = new CourseRoyaltyModel()
                        {
                            BranchCode = x.Branch,
                            Course_Id = 0,
                            Course_Name = x.CourseName,
                            TotalWeek = x.CourseWeek,
                            Attendence = x.Qty_Cum,
                            Tuition_Unit = x.CourseFee,
                            Discount = 0

                        };



                        courseroyaltyModel.TotalTuition = courseroyaltyModel.Tuition_Unit * courseroyaltyModel.Attendence;
                        courseroyaltyModel.Earlybird = x.Early_W1 * courseroyaltyModel.Tuition_Unit * courseroyaltyModel.TotalWeek * earlybird[0].Val_02 / 100
                           + x.Early_W2 * courseroyaltyModel.Tuition_Unit * (courseroyaltyModel.TotalWeek - 1) * earlybird[0].Val_02 / 100;  // 얼리버드 디스카운트
                        courseroyaltyModel.CourseFee_ExGST = (courseroyaltyModel.TotalTuition - courseroyaltyModel.Earlybird) / Convert.ToDecimal(1.1);

                        var royaltytype = campus.Where(c => c.Branch == courseroyaltyModel.BranchCode).ToList();

                        if (x.CourseType == "HIGH")
                        {
                            royaltytype = campus2.Where(c => c.Branch == courseroyaltyModel.BranchCode).ToList();
                        }
                        courseroyaltyModel.RoyaltyType = royaltytype[0].RoyaltyType;
                        courseroyaltyModel.NetRoyalty = courseroyaltyModel.CourseFee_ExGST * royaltytype[0].Discount / 100;
                        courseroyaltyModel.CourseRoyalty_IncGST = courseroyaltyModel.NetRoyalty * Convert.ToDecimal(1.1);

                        return courseroyaltyModel;
                    })
                    .AsQueryable()
                    .Filter(filter)
                    .Sort(sort),
                    Total = courseorder.ToList().Count

                                
                };
                return Json(gridModel);
            }catch(Exception ex)
            {
                string msg = ex.Message;
            }

            return Json(null);

        }

        private void SetAvailableTestRoyaltyCombo(TestRoyaltyModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;


            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                }
            }
        }

        public ActionResult TestRoyaltyList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new TestRoyaltyModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableTestRoyaltyCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetTestRoyaltyList(DataSourceRequest command,
            TestRoyaltyModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "TEST", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "TEST");   // 테스트인것만 카운트
            var courses = _courseService.GetAllCourseMasters().Where(x => x.Term == model.Term).Join(coursetype, x => x.CourseCategory, y => y.DetailCode, (x, y)
                            => new
                            {
                                Id = x.Id,
                                Year = x.Year,
                                Term = x.Term,
                                Grade = x.Grade,
                                Level = x.Level,
                                CourseCategory = x.CourseCategory,
                                CourseName = x.CourseName,
                                CourseFee = x.CourseFee,
                                TotalWeek = x.TotalWeek,
                                StartWeek = x.StartWeek
                            }); 
            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");
            var enrollments = _royaltyService.GetAllEnrollments(model.BranchCode, model.Course_Id);

            var codes = _codeMasterService.GetAllCodeMasters("OR04", "Y");
            var termweek = _calendarService.GetAllCalendarMasters().Where(x => x.Year == model.Year).Where(x => x.Term == model.Term).ToList();


            var discount = _codeMasterService.GetAllCodeMasters("RY03", "Y");
            var campus = _campusService.GetAllCampus().Join(discount, x => x.RoyaltyType, y => y.DetailCode, (x, y)
                => new { Branch = x.Campus_Prefix, Discount = y.Val_01, Discount2 = y.Val_02, RoyaltyType = y.DetailName });




            // Enrol 내역 집계
            var enrolls = _royaltyService.GetAllEnrollments(model.BranchCode, 0)
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    Year = y.Year,
                    Term = y.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    CourseType = y.CourseCategory,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    Branch = x.BranchCode,
                    CourseId = x.Course_Id,
                    Week = x.Week,
                    CourseWeek = y.TotalWeek,
                    StartWeek = y.StartWeek,
                    Qty = x.Qty_Modified
                }).Where(x => x.Year == model.Year)
                .GroupBy(group => new
                {
                    group.Branch,
                    group.CourseId,
                    group.CourseWeek,
                    group.Week,
                    group.Year,
                    group.CourseType,
                    group.Term,
                    group.Grade,
                    group.Level,
                    group.CourseFee,
                    group.StartWeek
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    CourseName = group.Max(a => a.CourseName),
                    Branch = group.Key.Branch,
                    CourseId = group.Key.CourseId,
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => a.Week >= 98 ? 0 : (a.CourseWeek - a.Week + 1) * a.Qty)
                });


            // order 집계
            var courseorder = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Week = y.Week, CourseId = y.Course_Id })
                .Join(courses, x => x.CourseId, y => y.Id, (x, y) => new
                {
                    Branch = x.Branch,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = y.Grade,
                    Level = y.Level,
                    Order_Id = x.Order_Id,
                    Qty = x.Qty,
                    Week = x.Week,
                    CourseId = x.CourseId,
                    CourseName = y.CourseName,
                    CourseFee = y.CourseFee,
                    CourseWeek = y.TotalWeek,
                    StartWeek = y.StartWeek,
                    CourseType = y.CourseCategory
                })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.Grade, group.CourseFee, group.CourseWeek, group.CourseId, group.Level, group.CourseType, group.Week, group.StartWeek })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    CourseType = group.Key.CourseType,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Week = group.Key.Week,
                    Qty = group.Sum(a => a.Qty),
                    Qty_Cum = group.Sum(a => (a.CourseWeek - a.Week + 1) * a.Qty),
                    CourseName = group.Max(a => a.CourseName),
                    CourseFee = group.Key.CourseFee,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek
                }).ToList();


            // order와 enrol 을 집계하여 최종결과 산출

            // enroll이 있으면 지운다
            for (int i = courseorder.Count - 1; i >= 0; i--)
            {
                var enroll = enrolls.Where(x => x.Week == courseorder[i].Week && x.CourseId == courseorder[i].Course_Id && x.Branch == courseorder[i].Branch).ToList();
                if (enroll.Count > 0)
                    courseorder.Remove(courseorder[i]);
            }
            // enroll을 넣는다

            foreach (var enroll in enrolls)
            {
                courseorder.Add(new
                {
                    Year = enroll.Year,
                    Term = enroll.Term,
                    Grade = enroll.Grade,
                    Level = enroll.Level,
                    CourseType = enroll.CourseType,
                    Branch = enroll.Branch,
                    Course_Id = enroll.CourseId,
                    Week = enroll.Week,
                    Qty = enroll.Qty,
                    Qty_Cum = enroll.Qty_Cum,
                    CourseName = enroll.CourseName,
                    CourseFee = enroll.CourseFee,
                    CourseWeek = enroll.CourseWeek,
                    StartWeek = enroll.StartWeek
                });
            }



            var final = courseorder
                .GroupBy(x => new { x.Branch, x.Course_Id, x.Year, x.Term, x.CourseType, x.Grade, x.Level, x.CourseName, x.CourseWeek, x.StartWeek, x.CourseFee })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    BranchCode = group.Key.Branch,
                    Course_Id = group.Key.Course_Id,
                    CourseName = group.Key.CourseName,
                    SetCategory = group.Key.CourseType,
                    CourseWeek = group.Key.CourseWeek,
                    StartWeek = group.Key.StartWeek,
                    CourseFee = group.Key.CourseFee,
                    Grade = group.Key.Grade,
                    Level = group.Key.Level,
                    Qty1 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 1 ? a.Qty : 0), OQty1 = 0,
                    Qty2 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 2 ? a.Qty : 0), OQty2 = 0,
                    Qty3 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 3 ? a.Qty : 0), OQty3 = 0,
                    Qty4 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 4 ? a.Qty : 0), OQty4 = 0,
                    Qty5 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 5 ? a.Qty : 0),
                    OQty5 = 0,
                    Qty6 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 6 ? a.Qty : 0),
                    OQty6 = 0,
                    Qty7 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 7 ? a.Qty : 0),
                    OQty7 = 0,
                    Qty8 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 8 ? a.Qty : 0),
                    OQty8 = 0,
                    Qty9 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 9 ? a.Qty : 0),
                    OQty9 = 0,
                    Qty10 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 10 ? a.Qty : 0),
                    OQty10 = 0,
                    Qty11 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 11 ? a.Qty : 0),
                    OQty11 = 0,
                    Qty12 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 12 ? a.Qty : 0),
                    OQty12 = 0,
                    Qty13 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 13 ? a.Qty : 0),
                    OQty13 = 0,
                    Qty14 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 14 ? a.Qty : 0),
                    OQty14 = 0,
                    Qty15 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 15 ? a.Qty : 0),
                    OQty15 = 0,
                    Qty16 = group.Sum(a => a.Week - group.Key.StartWeek + 1 == 16 ? a.Qty : 0),
                    OQty16 = 0,
                    QtyTotal = group.Sum(a => a.Qty),
                    OQtyTotal = 0,
                    Code = "OR04"
                })
                .Join(codes, x => x.Code, y => y.CodeType, (x, y) => new
                {
                    BranchCode = x.BranchCode,
                    Year = x.Year,
                    Term = x.Term,
                    Grade = x.Grade,
                    SetCategory = x.SetCategory,
                    Level = x.Level,
                    Course_Id = x.Course_Id,
                    CourseName = x.CourseName,
                    TotalWeek = x.CourseWeek,
                    StartWeek = x.StartWeek,
                    CourseFee = x.CourseFee,
                    QtyType = y.DetailCode,
                    Qty1 = ( y.DetailCode == "BOOK") ? x.StartWeek : (y.DetailCode == "OQTY") ? x.OQty1 : x.Qty1,
                    Qty2 = (x.CourseWeek < 2) ? 0: (y.DetailCode == "BOOK") ? x.StartWeek + 1 : (y.DetailCode == "OQTY") ? x.OQty2 : x.Qty2 + x.Qty1,
                    Qty3 = (x.CourseWeek < 3) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 2 : (y.DetailCode == "OQTY") ? x.OQty3 : x.Qty3 + x.Qty2 + x.Qty1,
                    Qty4 = (x.CourseWeek < 4) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 3 : (y.DetailCode == "OQTY") ? x.OQty4 : x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty5 = (x.CourseWeek < 5) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 4 : (y.DetailCode == "OQTY") ? x.OQty5 : x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty6 = (x.CourseWeek < 6) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 5 : (y.DetailCode == "OQTY") ? x.OQty6 : x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty7 = (x.CourseWeek < 7) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 6 : (y.DetailCode == "OQTY") ? x.OQty7 : x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty8 = (x.CourseWeek < 8) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 7 : (y.DetailCode == "OQTY") ? x.OQty8 : x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty9 = (x.CourseWeek < 9) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 8 : (y.DetailCode == "OQTY") ? x.OQty9 : x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty10 = (x.CourseWeek < 10) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 9 : (y.DetailCode == "OQTY") ? x.OQty10 : x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty11 = (x.CourseWeek < 11) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 10 : (y.DetailCode == "OQTY") ? x.OQty11 : x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty12 = (x.CourseWeek < 12) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 11 : (y.DetailCode == "OQTY") ? x.OQty12 : x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty13 = (x.CourseWeek < 13) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 12 : (y.DetailCode == "OQTY") ? x.OQty13 : x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty14 = (x.CourseWeek < 14) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 13 : (y.DetailCode == "OQTY") ? x.OQty14 : x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty15 = (x.CourseWeek < 15) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 14 : (y.DetailCode == "OQTY") ? x.OQty15 : x.Qty15 + x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    Qty16 = (x.CourseWeek < 16) ? 0 : (y.DetailCode == "BOOK") ? x.StartWeek + 15 : (y.DetailCode == "OQTY") ? x.OQty16 : x.Qty16 + x.Qty15 + x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                    QtyTotal = (y.DetailCode == "BOOK") ? 0 : (y.DetailCode == "OQTY") ? x.OQtyTotal : x.QtyTotal
                })
                .OrderBy(y => y.BranchCode)
                .ThenBy(y => y.Grade)
                .ThenBy(y => y.SetCategory)
                .ThenBy(y => y.Level);


            int maxweek = 0;

            if (final.ToList().Count() > 0)
                maxweek = final.Max(x => x.TotalWeek);


            int id = 1;

            var gridModel = new DataSourceResult
            {
                Data = final.Select(x =>
                {
                    var testroyaltyModel = new TestRoyaltyModel()
                    {
                        BranchCode = ( x.QtyType == "BOOK") ? x.BranchCode : " ",
                        CourseName = ( x.QtyType == "BOOK") ? x.CourseName : "",
                        Course_Id = x.Course_Id,
                        Gubun = x.QtyType,
                        Week1 = x.Qty1,
                        Week2 = x.Qty2,
                        Week3 = x.Qty3,
                        Week4 = x.Qty4,
                        Week5 = x.Qty5,
                        Week6 = x.Qty6,
                        Week7 = x.Qty7,
                        Week8 = x.Qty8,
                        Week9 = x.Qty9,
                        Week10 = x.Qty10,
                        Week11 = x.Qty11,
                        Week12 = x.Qty12,
                        Week13 = x.Qty13,
                        Week14 = x.Qty14,
                        Week15 = x.Qty15,
                        Week16 = x.Qty16,
                        WeekTotal = (x.QtyType == "BOOK") ? 0 : x.Qty16 + x.Qty15 + x.Qty14 + x.Qty13 + x.Qty12 + x.Qty11 + x.Qty10 + x.Qty9 + x.Qty8 + x.Qty7 + x.Qty6 + x.Qty5 + x.Qty4 + x.Qty3 + x.Qty2 + x.Qty1,
                        cnt_Week = maxweek,
                        TestFee =  (x.QtyType == "MODY") ? x.CourseFee : (x.QtyType == "OQTY") ? 5 :0,
                        Royalty = (x.QtyType == "MODY") ? x.CourseFee * x.QtyTotal : (x.QtyType == "OQTY") ? 5 *x.QtyTotal : 0
                    };
 
                    testroyaltyModel.Royalty = testroyaltyModel.WeekTotal * testroyaltyModel.TestFee;


                    id++;

                    testroyaltyModel.Id = id;


                    return testroyaltyModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = final.ToList().Count
            };

            return Json(gridModel);

        }


        public ActionResult PdfInvoice(string typeyeartermbranch)
        {
            string type = typeyeartermbranch.Substring(0,4);
            string year = typeyeartermbranch.Substring(4,4);
            string term = typeyeartermbranch.Substring(8,4);
            string branch = typeyeartermbranch.Substring(12,2);

            switch(type)
            {
                case "BOOK": return PdfInvoiceMaterial(year, term, branch); 
                case "ROYL": return PdfInvoiceRoyalty(year, term, branch); 
                case "TEST": return PdfInvoiceTestfee(year, term, branch); 
                case "ITEM": return PdfInvoiceItem(year, term, branch); 
                case "PA__": return PdfInvoicePA(year, term, branch); 
                case "SUPP": return PdfInvoiceSupple(year, term, branch); 
                default: return Content("not specified type of invoice"); 
            }

        }


        public ActionResult PdfInvoiceMaterial(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (year == "" || term == "" || branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "BOOK").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "B" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintMaterialInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }



            return File(bytes, "application/pdf", filename);
        }

        public ActionResult PdfInvoiceRoyalty(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (year == "" || term == "" || branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "ROYL").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }



            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "R" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintRoyaltyInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }

            

            return File(bytes, "application/pdf", filename);
        }

        public ActionResult PdfInvoiceTestfee(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if ( year == "" || term == "" || branch == "" )
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "TEST").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "T" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintTestfeeInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }

            

            return File(bytes, "application/pdf", filename);
        }

        public ActionResult PdfInvoiceSupple(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (year == "" || term == "" || branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "SUPP").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "S" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintSupplefeeInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }

            return File(bytes, "application/pdf", filename);
        }


        public ActionResult PdfInvoicePA(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (year == "" || term == "" || branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "PA__").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "P" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintPAfeeInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }



            return File(bytes, "application/pdf", filename);
        }


        public ActionResult PdfInvoiceItem(string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (year == "" || term == "" || branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closinglist = _royaltyService.GetAllClosings(branch, year, term).Where(x => x.Type == "ITEM").ToList();

            if (closinglist == null || closinglist.Count <= 0)
            {
                return Content("The invoice is not ready. Please confirm firstly.");
            }

            if (closinglist[0].YN_Closing != "Y")
            {
                return Content("The invoice is modified. Please confirm firstly.");
            }


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + "I" + closinglist[0].SEQ.ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintItemfeeInvoiceToPdf(stream, year, term, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }



            return File(bytes, "application/pdf", filename);
        }

        public ActionResult PdfInvoiceMarketing(string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            if (branch == "")
            {
                return Content("not specified the condition of year, term and branch.");
            }

            var campuslist = _campusService.GetAllCampus();
            


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + DateTime.Now.Year.ToString().Substring(2, 2) 
                        + "NMKT" + (1 + DateTime.Now.Month).ToString("00");

            string filename = "INVOICE_" + InvoiceNo + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintMarketingfeeInvoiceToPdf(stream, branch, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }



            return File(bytes, "application/pdf", filename);
        }

        [HttpPost]
        public string ConfirmInvoice(string year, string term, string branch, string type)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch")
                && !_permissionService.Authorize("OmniAccountant"))
                return "Only authorized account can confirm a invoice.";

            if (year == "" || term == "" || branch == "")
            {
                return "not specified the condition of year, term and branch.";
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closing = _royaltyService.GetClosing(year, term, type, branch);

            if (closing == null)
            {
                closing = new Closing(
                        year, term, type, branch, 1, "Y", "N", "N", 0, 0, DateTime.Now, "", "", ""
                    );
                _royaltyService.InsertClosing(closing);

                return null;
            }
            else
            {

                if (closing.YN_Closing == "Y")
                {
                    return "The invoice is already confirmed .";
                }
                else
                {
                    closing.YN_Closing = "Y";
                    closing.reg_date = DateTime.Now;
                    closing.SEQ = closing.SEQ + 1;
                    closing.reg_source = _workContext.CurrentCustomer.Username;
                    _royaltyService.UpdateClosing(closing);
                }

                return "";
            }
        }


        private void SetAvailableSuppleCombo(SuppleFeeModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                }
            }
        }

        public ActionResult SuppleFeeList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new SuppleFeeModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableSuppleCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetSuppleFeeList(DataSourceRequest command,
            SuppleFeeModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "SUPP", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var courses = _courseService.GetAllCourseMasters();
            var ordersets = _orderService.GetAllOrderbyItemSets();
            var itemsets = _itemSetService.GetAllItemSets(model.Term, "", "");

            var discount = _codeMasterService.GetAllCodeMasters("OR03", "Y");    // 디스카운트 없음
            var coursetype = _codeMasterService.GetAllCodeMasters("CM04", "Y").Where(x => x.String_01 == "SUPPL");  // supplementary코스인것만 카운트

            var teacherdiscount = discount.Where(x => x.DetailCode == "TCHR").ToList()[0].Val_01;
            var incentivediscount = discount.Where(x => x.DetailCode == "STUD").ToList()[0].Val_01;


            var bookorders = orders
                .Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Qty_T = y.Qty_Teacher, isHalf = y.is_Half, CourseId = y.Course_Id })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.CourseId })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    Course_Id = group.Key.CourseId,
                    Qty = group.Sum(a => a.isHalf == "N" ? a.Qty : 0),
                    Qty_H = group.Sum(a => a.isHalf == "Y" ? a.Qty : 0),
                    Qty_T = group.Sum(a => a.Qty_T)
                })
                .Join(courses, x => x.Course_Id, y => y.Id, (x, y) => new
                {
                    BranchCode = x.Branch,
                    CourseType = y.CourseCategory,
                    Year = x.Year,
                    Term = x.Term,
                    Course_Id = x.Course_Id,
                    Course_Name = y.CourseName,
                    MaterialFee = y.BookFee,
                    Qty_Student = x.Qty,
                    Qty_Half = x.Qty_H,
                    Qty_Teacher = x.Qty_T,
                    Grade = y.Grade,
                    Subject = y.Level ,     // 레벨이 과목역할을 하니까 따로 분리한다 과목필드를 만들껄 그랬나.. 싶기도하다
                    Facility = (y.CourseCategory == "HIGH") ? 0 : y.Facility // 하이스쿨은 온라인 없음
                })
                .Join(coursetype, x => x.CourseType, y => y.DetailCode, (x, y) 
                    => new { x.BranchCode, x.CourseType, x.Year, x.Term, x.Course_Id, x.Course_Name, x.MaterialFee, x.Qty_Student, x.Qty_Half, x.Qty_Teacher, x.Grade, x.Subject, x.Facility })
                .GroupBy(group => new { group.Year, group.Term, group.Grade, group.CourseType, group.BranchCode, group.MaterialFee, group.Subject, group.Facility })
                .Select(group => new
                {
                    BranchCode = group.Key.BranchCode,
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Grade = group.Key.Grade,
                    MaterialFee = group.Key.MaterialFee,
                    facility = group.Key.Facility,
                    Qty_Student = group.Sum(a => a.Qty_Student),
                    Qty_Half = group.Sum(a => a.Qty_Half),
                    Qty_Teacher = group.Sum(a => a.Qty_Teacher),
                    Course_Name = group.Max(a => a.Course_Name),
                    CourseType = group.Key.CourseType
                })
                .OrderBy(x => x.BranchCode)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.Course_Name);


            int id = 1;


            var gridModel = new DataSourceResult
            {
                Data = bookorders.Select(x =>
                {
                    var materialfeeModel = new SuppleFeeModel()
                    {
                        BranchCode = x.BranchCode,
                        Course_Name = x.Course_Name,
                        MaterialFee = x.MaterialFee,

                        Qty_Student = x.Qty_Student,
                        Qty_Half = x.Qty_Half,
                        Qty_Teacher = x.Qty_Teacher,
                        Amt_Student = (x.MaterialFee ) * x.Qty_Student,
                        Amt_Half = (x.MaterialFee ) * x.Qty_Half * Convert.ToDecimal(0.5),
                        Amt_Teacher = x.MaterialFee * x.Qty_Teacher * teacherdiscount / 100,

                        Amt_Total = 0
                    };


                    materialfeeModel.Amt_Total = materialfeeModel.Amt_Student + materialfeeModel.Amt_Half + materialfeeModel.Amt_Teacher;

                    return materialfeeModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = bookorders.ToList().Count
            };

            return Json(gridModel);
        }



        private void SetAvailablePASalesCombo(PASalesModel model)
        {

            model.AvailableYear.Clear();
            model.AvailableTerm.Clear();
            model.AvailableBranch.Clear();
            model.AvailableLevel.Clear();

            model.AvailableTerm.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYear.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranch.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });
            model.AvailableLevel.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var branch in branchs)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.Branch = _workContext.CurrentCustomer.AdminComment;

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                    case "CM03":
                        model.AvailableLevel.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                }
            }
        }

        public ActionResult PASalesList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniPALicensee"))
                return AccessDeniedView();

            var model = new PASalesModel();

            SetAvailablePASalesCombo(model);

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniPALicensee") )
                model.is_admin = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult GetPASalesList(DataSourceRequest command,
            PASalesModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniPALicensee"))
                return AccessDeniedView();

            // 미처리된 PA가 있으면 그것부터 본테이블에 넣고 시작

            var notprocessed = _royaltyService.GetAllPA_Sales().ToList();

            var campus = _codeMasterService.GetAllCampus();

            foreach (var pasale in notprocessed)
            {
                var paorder = new Order_PASales(
                    pasale.Sales_id, pasale.Branch, "", "", pasale.Title, "", pasale.Price, pasale.List_Price, pasale.Payment, pasale.Stud_id, pasale.Purchase_Date, DateTime.Now, "system"
                    );

                // 결제일자 = IF일자 에 해당하는 텀정보 넣고 타이틀도 수정

                DateTime IFDate = new DateTime(pasale.reg_date.Year, pasale.reg_date.Month, pasale.reg_date.Day);
                var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_PA == "Y");
                string curTerm = "";
                string curYear = "";
                foreach (var term in calendar)
                {
                    curTerm = term.Term;
                    curYear = term.Year;
                }

                paorder.Year = curYear;
                paorder.Term = curTerm;

                // Branch omni는 licensee 나머지는 그대로 branchcode 
                if (pasale.Branch == "15")
                {
                    var licensee = campus.Where(x => x.LicenseeCode == pasale.Stud_Advisor);
                    if (licensee != null && licensee.ToList().Count > 0)
                    {
                        paorder.BranchCode = licensee.ToList()[0].Campus_Prefix;
                    }
                }

                // Title이 깨지는 경우가 있으니 정규화한다.
                paorder.Title = System.Text.RegularExpressions.Regex.Replace(pasale.Title, @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", String.Empty);


                _royaltyService.InsertOrder_PASales(paorder);

                var origin = _royaltyService.GetAllPA_Sales().Where(x => x.Id == pasale.Id).ToList()[0];
                origin.IF_Flag = "Y";
                _royaltyService.UpdatePA_Sales(origin);


                // pa 인 경우(reg_source = 'system-pa') Order 생성 
                if (pasale.reg_source.ToUpper() == "SYSTEM-PA")
                { 

                    var order = new Order_Omni(paorder.BranchCode, paorder.Purchase_Date, paorder.Purchase_Date,  paorder.Purchase_Date, "PA__", "ST01", paorder.Year, paorder.Term, paorder.Price, 0, "", DateTime.Now, "system");
                    _orderService.InsertOrder(order);

                    var orderset = new OrderbyItemSet(order.Id, paorder.Id, 1, 1, 0, paorder.Sales_id, 1, "N","", DateTime.Now, "system");
                    _orderService.InsertOrderbyItemSet(orderset);
                }
            }

            var paorders = _royaltyService.GetAllOrder_PASales(model.Branch, model.Year, model.Term);

            int id = 1;

            var gridModel = new DataSourceResult
            {
                Data = paorders.Select(x =>
                {
                    var PAModel = new PASalesModel()
                    {
                        Id = x.Id,
                        Branch = x.BranchCode,
                        Year = x.Year,
                        Term = x.Term,
                        Title = x.Title,
                        Level = String.IsNullOrEmpty(x.Level) ? "" : x.Level,
                        Price = x.Price,
                        List_Price = x.List_Price,
                        Diff_Price = x.List_Price - x.Price,
                        Payment = x.Payment,
                        Stud_id = x.Stud_id,
                        Purchase_Date = x.Purchase_Date
                    };

                    return PAModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = paorders.ToList().Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult PASalesUpdate(PASalesModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniPALicensee"))
                return AccessDeniedView();

            var paorder = _royaltyService.GetOrder_PASalesById(model.Id);
            if (paorder == null)
                return Json(new DataSourceResult() { Errors = "There is no paorder with the specified code" });

            var closing4 = _royaltyService.GetClosing(paorder.Year, paorder.Term, "PA__", paorder.BranchCode);


            if (closing4 != null && closing4.YN_Closing == "Y")
            {
                return Json(new DataSourceResult() { Errors = "order can't be deleted. PA order closed" });
            }




            //item = model.ToEntity(item);
            paorder.Price = model.Price;
            paorder.Level = model.Level;
            paorder.Payment = model.Payment;
            paorder.Year = model.Year;
            paorder.Term = model.Term;

            paorder.reg_date = DateTime.Now;
            paorder.reg_source = _workContext.CurrentCustomer.Username;

            _royaltyService.UpdateOrder_PASales(paorder);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult PASalesDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniPALicensee"))
                return AccessDeniedView();

            var paorder = _royaltyService.GetOrder_PASalesById(id);
            if (paorder == null)
                return Json(new DataSourceResult() { Errors = "There is no paorder with the specified code" });


            var closing4 = _royaltyService.GetClosing(paorder.Year, paorder.Term, "PA__", paorder.BranchCode);


            if (closing4 != null && closing4.YN_Closing == "Y")
            {
                return Json(new DataSourceResult() { Errors = "order can't be deleted. PA order closed" });
            }




            //using (var scope = new System.Transactions.TransactionScope())
            //{

            _royaltyService.DeleteOrder_PASales(paorder);

            //    scope.Complete();
            //}

            return new NullJsonResult();
        }




        public ActionResult PAFeeList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new PASalesModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniPALicensee") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailablePASalesCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetPAFeeList(DataSourceRequest command,
            PASalesModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniPALicensee") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var paorders = _royaltyService.GetAllOrder_PASales(model.Branch, model.Year, model.Term);

            var paroyalty = _codeMasterService.GetAllCodeMasters("RY02", "Y").ToList()[0];

            var paratio = paroyalty.Val_01;
            var otherratio = paroyalty.Val_02; 

            int id = 1;


            var gridModel = new DataSourceResult
            {
                Data = paorders.Select(x =>
                {
                    var pafeeModel = new PASalesModel()
                    {
                        Branch = x.BranchCode,
                        Year = x.Year,
                        Term = x.Term,
                        Title = x.Title,
                        Price = x.Price,
                        Payment = x.Payment,
                        Stud_id = x.Stud_id,
                        Stud_Name = "",
                        Amt_Total = 0
                    };

                    var students = _classService.GetAllMembers("", "", "", "", x.Stud_id, "").ToList();

                    string stud_name = "";
                    if ( students.Count > 0 )
                    {
                        stud_name = students[0].stud_first_name + " " + students[0].stud_last_name;
                    }

                    pafeeModel.Stud_Name = stud_name;

                    if (x.Title.Contains("PA+"))
                    {
                        pafeeModel.Amt_Total = x.Payment.ToUpper() == "BANK" ? x.Price * Convert.ToDecimal(paratio) / 100 : x.Price * Convert.ToDecimal(paratio) / 100 * -1;
                    }
                    else
                    {
                        pafeeModel.Amt_Total = x.Payment.ToUpper() == "BANK" ? x.Price * Convert.ToDecimal(100 - otherratio) / 100 : x.Price * Convert.ToDecimal(otherratio) / 100 * -1;
                    }

                    return pafeeModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = paorders.ToList().Count
            };

            return Json(gridModel);
        }


        private void SetAvailableItemCombo(ItemFeeModel model)
        {

            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var courses = _courseService.GetAllCourseMasters();

            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }
            model.BranchCode = _workContext.CurrentCustomer.AdminComment;

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Term == x.DetailCode) });
                        break;
                    case "CM05":
                        model.AvailableYearList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Year == x.DetailCode) });
                        break;
                }
            }
        }

        public ActionResult ItemFeeList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                 && !_permissionService.Authorize("OmniAccountant") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new ItemFeeModel();

            //관리자 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction") || _permissionService.Authorize("OmniAccountant"))
                model.is_admin = true;

            SetAvailableItemCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetItemFeeList(DataSourceRequest command,
            SuppleFeeModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction")
                && !_permissionService.Authorize("OmniBranch") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "ITEM", "").Where(x => x.Status.Substring(0, 3) != "ST0");
            var order_ds = _orderService.GetAllOrder_Ds(model.BranchCode, model.Year, model.Term, "ITEM", "");


            var bookorders = orders
                .Join(order_ds, x => x.Id, y => y.Order_Id, (x, y) => new { Branch = x.BranchCode, Year = x.Year, Term = x.Term, Order_Id = x.Id, Qty = y.Qty, Price = y.UnitPrice, ItemCode = y.ItemCode })
                .GroupBy(group => new { group.Branch, group.Year, group.Term, group.ItemCode, group.Price })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Term = group.Key.Term,
                    Branch = group.Key.Branch,
                    ItemCode = group.Key.ItemCode,
                    Price = group.Key.Price,
                    Qty = group.Sum(a => a.Qty)

                })
                .OrderBy(x => x.Branch)
                .ThenBy(x => x.ItemCode);
            int id = 1;


            var gridModel = new DataSourceResult
            {
                Data = bookorders.Select(x =>
                {
                    var itemfeeModel = new ItemFeeModel()
                    {
                        BranchCode = x.Branch,
                        ItemCode = x.ItemCode,
                        Price = x.Price,
                        Qty = x.Qty,
                        Amt_Total = x.Price * x.Qty
                    };

                    itemfeeModel.ItemName = _itemService.GetItemByCode(itemfeeModel.ItemCode).ItemName;

                    return itemfeeModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = bookorders.ToList().Count
            };

            return Json(gridModel);
        }



        [HttpPost]
        public string GetInvoiceNo(string year, string term, string branch, string type)
        {

            if (year == "" || term == "" || branch == "")
            {
                return "not specified the condition of year, term and branch.";
            }

            var campuslist = _campusService.GetAllCampus();
            var termlist = _codeMasterService.GetAllCodeMasters("CM02", "Y");
            var closing = _royaltyService.GetClosing(year, term, type, branch);

            if (closing == null)
                return "";


            var InvoiceNo = campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Campus_EN
                        + year.Substring(2, 2) + termlist.Where(a => a.DetailCode == term).ToList()[0].String_01
                        + type.Substring(0, 1) + closing.SEQ.ToString("00") + (closing.YN_Closing != "Y" ? "(not confirmed)" : "");

            return InvoiceNo + "|" + campuslist.Where(a => a.Campus_Prefix == branch).ToList()[0].Email;
        }


        private void SetAvailableClosingCombo(ClosingModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title });
            }


            // ordertype, orderstatus, term, year combo
            model.AvailableType.Clear();
            model.AvailableYear.Clear();
            model.AvailableTerm.Clear();
            model.AvailableType.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "RY04":
                        model.AvailableType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Type) });
                        break;
                    case "CM02":
                        model.AvailableTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == curTerm) });
                        break;
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == curYear) });
                        break;
                }
            }
        }

        public ActionResult ClosingList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var model = new ClosingModel();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniAccountant"))
                model.isadmin = true;
            else model.isadmin = false;

            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClosingCombo(model);


            return View(model);
        }

        [HttpPost]
        public ActionResult GetClosingList(DataSourceRequest command,
            ClosingModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();
            if (model.Type == null)
                model.Type = "";

            var closings = _royaltyService.GetAllClosings( model.Branch, model.Year, model.Term).Where( x => x.Type.Contains(model.Type) );

            // invoiceno = branch(2) + year(2) + term(2) + type(1) + seq(2)

            var campuslist = _campusService.GetAllCampus();
            var term = _codeMasterService.GetAllCodeMasters("CM02", "Y");


            try
            {
                var gridModel = new DataSourceResult
                {
                    Data = closings.Select(x =>
                    {
                        var closinglist = new ClosingModel()
                        {
                            Id = x.Id,
                            Branch = x.Branch,
                            Year = x.Year != null ? x.Year : "",
                            Term = x.Term != null ? x.Term : "",
                            Type = x.Type,
                            SEQ = x.SEQ,
                            YN_Closing = String.IsNullOrEmpty(x.YN_Closing) ? "N" : x.YN_Closing,
                            YN_Approval = String.IsNullOrEmpty(x.YN_Approval) ? "N" : x.YN_Approval,
                            YN_Paid = String.IsNullOrEmpty(x.YN_Paid) ? "N" : x.YN_Paid,
                            Amnt_adjust = x.Amnt_adjust == null ? 0 : x.Amnt_adjust,
                            Amnt_Freight = x.Amnt_Freight == null ? 0 : x.Amnt_Freight,
                            Remarks = x.Remarks,
                            DueDate = x.DueDate == null ? "" : x.DueDate,
                            reg_date = x.reg_date
                        };


                        closinglist.InvoiceNo = campuslist.Where(a => a.Campus_Prefix == x.Branch).ToList()[0].Campus_EN
                            + x.Year.Substring(2, 2) + term.Where(a => a.DetailCode == x.Term).ToList()[0].String_01
                            + x.Type.Substring(0, 1) + x.SEQ.ToString("00");

                        return closinglist;
                    })
                    .AsQueryable()
                    .Filter(filter)
                    .Sort(sort),
                    Total = closings.ToList().Count
                };

                return Json(gridModel);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        public ActionResult UpdateClosing(ClosingModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var closing = _royaltyService.GetClosing(model.Year, model.Term, model.Type, model.Branch);

            if (closing == null)
                return Json(new DataSourceResult() { Errors = "There is closing data with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            if ( model.YN_Closing == "N")
            {
                model.YN_Approval = "N";
            }

            if ( String.IsNullOrEmpty(model.DueDate))
            {
                model.DueDate = "";
            }


            //item = model.ToEntity(item);
            closing.YN_Closing = model.YN_Closing;
            closing.YN_Approval = model.YN_Approval;
            closing.YN_Paid = model.YN_Paid;
            closing.Amnt_adjust = model.Amnt_adjust;
            closing.Amnt_Freight = model.Amnt_Freight;
            closing.Remarks = model.Remarks;
            closing.DueDate = model.DueDate;
            closing.reg_source = _workContext.CurrentCustomer.Username;
            _royaltyService.UpdateClosing(closing);

            return new NullJsonResult();
        }


        [HttpPost]
        public string MakeEarlyBird(string year, string term, string branch)
        {

            if (year == "" || term == "" || branch == "")
            {
                return "not specified the condition of year, term and branch.";
            }

            var enroll = _royaltyService.GetAllEnrollments(branch, 0).Where(x => x.BranchCode == branch && ( x.Week == 1 || x.Week == 98 || x.Week == 99) );
            var course = _courseService.GetAllCourseMasters().Where(x => x.Year == year && x.Term == term);

            var orders = _orderService.GetAllOrders(branch, year, term, "GROP", "");
            var ordersets = _orderService.GetAllOrderbyItemSets().Where(x => x.Week == 1);

            var ordergroups = orders.Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new {x.BranchCode, y.Course_Id, y.Qty })
                .GroupBy(x => new { x.BranchCode, x.Course_Id }).Select( group => new { group.Key.BranchCode, group.Key.Course_Id, qty =  group.Sum( a => a.Qty)});



            // 98, 99은 일단 지운다.

            var oldearlybirds = enroll.Where(x => x.Week == 98 || x.Week == 99)
                .Join(course, x => x.Course_Id, y => y.Id, (x,y) => new {x.Id});

            foreach( var oldearlybird in oldearlybirds )
            {
                var delenroll = _royaltyService.GetEnrollmentById(oldearlybird.Id);
                _royaltyService.DeleteEnrollment(delenroll);
            }

            var newenrolls = enroll.Where(x => x.Week == 1)
                .Join(course, x => x.Course_Id, y => y.Id, (x,y) => new {x.Id, x.Course_Id, x.BranchCode});

            // 1기준으로 98을 넣어준다.
            foreach (var newfirst in newenrolls)
            {
                var newenroll = _royaltyService.GetEnrollmentById(newfirst.Id);

                var newearly = new Enrollment(newenroll.BranchCode, newenroll.Course_Id, 98, newenroll.Qty_Book
                    , newenroll.Qty_Modified, DateTime.Now, _workContext.CurrentCustomer.Username);

                _royaltyService.InsertEnrollment(newearly);
            }

            // order 가 있고 enroll이 없는 경우 order기준으로 얼리버드를 넣어준다.

            foreach( var ordergroup in ordergroups)
            {
                var newenrollcheck = newenrolls.Where(x => x.Course_Id == ordergroup.Course_Id && x.BranchCode == ordergroup.BranchCode).ToList();
                
                if ( newenrollcheck.Count == 0 )  // 없으면
                {
                    var newearly = new Enrollment(ordergroup.BranchCode, ordergroup.Course_Id, 98, 0
                    , ordergroup.qty, DateTime.Now, _workContext.CurrentCustomer.Username);

                    _royaltyService.InsertEnrollment(newearly);
                }
            }

            return "Good Job";
        }


        private void SetAvailableStudentSummaryCombo(StudentSummaryModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title });
            }

            model.AvailableYear.Clear();
            model.AvailableTerm.Clear();
            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y");
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == curTerm) });
                        break;
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == curYear) });
                        break;
                }
            }
        }



        public ActionResult StudentSummary()
        {
            if (!_permissionService.Authorize("OmniAdmin") )
                return AccessDeniedView();

            var model = new StudentSummaryModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableStudentSummaryCombo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult GetStudentSummary(DataSourceRequest command,
            StudentSummaryModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") )
                return AccessDeniedView();

            var studentsummary = _royaltyService.GetStudentSummary(model.Year, model.Term);


            try
            {
                var gridModel = new DataSourceResult
                {
                    Data = studentsummary.Select(x =>
                    {
                        var summary = new StudentSummaryModel()
                        {
                            Branch = x.Branch,
                            Year = x.Year != null ? x.Year : "",
                            Term = x.Term != null ? x.Term : "",
                            Cnt_Book = x.Book,
                            Cnt_Course = x.Course,
                            Cnt_Primary = x.Primary,
                            Cnt_Secondary = x.Secondary
                        };


                        return summary;
                    })
                    .AsQueryable()
                    .Filter(filter)
                    .Sort(sort),
                    Total = studentsummary.TotalCount
                };

                return Json(gridModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetStudentChartData(string year, string term)
        {

            try
            {
                var students = new System.Collections.ArrayList();
                var tempterm = _calendarService.GetAllCalendarMasters().Where(x => x.Year == year && x.Term == term).ToList();
                CalendarMaster curterm = null;

                if ( tempterm.Count > 0)
                {
                    curterm = tempterm[0];
                }

                var othertems = _calendarService.GetAllCalendarMasters()
                    .Where(x => x.StartDate < curterm.StartDate && x.Term.Substring(0,1) == curterm.Term.Substring(0,1))
                    .OrderByDescending(x => x.StartDate).ToList();

                var studentsummary = _royaltyService.GetStudentSummary("", "").OrderBy(x => x.Term).ToList();

                var studentsummary1 = studentsummary.Where(x => x.Year == year && x.Term == term).OrderBy(x => x.Branch).ToList();

                var termstudents1 = new System.Collections.ArrayList();
                foreach(var enrol in studentsummary1)
                {
                    termstudents1.Add(enrol.Course);
                }
                students.Add(termstudents1);



                var studentssummary2 = studentsummary.Where(x => x.Year == othertems[0].Year && x.Term == othertems[0].Term).OrderBy(x => x.Branch).ToList();

                var termstudents2 = new System.Collections.ArrayList();
                foreach (var enrol in studentsummary1)
                {
                    var branchenrol = studentssummary2.Where(x => x.Branch == enrol.Branch).ToList();
                    if (branchenrol.Count > 0)
                        termstudents2.Add(branchenrol[0].Course);
                    else termstudents2.Add(0);
                }

                students.Insert(0,termstudents2);

                var studentssummary3 = studentsummary.Where(x => x.Year == othertems[1].Year && x.Term == othertems[1].Term).OrderBy(x => x.Branch).ToList();

                var termstudents3 = new System.Collections.ArrayList();
                foreach (var enrol in studentsummary1)
                {
                    var branchenrol = studentssummary3.Where(x => x.Branch == enrol.Branch).ToList();
                    if (branchenrol.Count > 0)
                        termstudents3.Add(branchenrol[0].Course);
                    else termstudents3.Add(0);
                }

                students.Insert(0, termstudents3);


                var studentssummary4 = studentsummary.Where(x => x.Year == othertems[2].Year && x.Term == othertems[2].Term).OrderBy(x => x.Branch).ToList();

                var termstudents4 = new System.Collections.ArrayList();
                foreach (var enrol in studentsummary1)
                {
                    var branchenrol = studentssummary4.Where(x => x.Branch == enrol.Branch).ToList();
                    if (branchenrol.Count > 0)
                        termstudents4.Add(branchenrol[0].Course);
                    else termstudents4.Add(0);
                }

                students.Insert(0, termstudents4);


                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }



        private void SetAvailableMarketingFeeCombo(MarketingFeeModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

        }

        public ActionResult MarketingFeeList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var model = new MarketingFeeModel();


            SetAvailableMarketingFeeCombo(model);


            return View(model);
        }


        [HttpPost]
        public ActionResult GetMarketingFeeList(DataSourceRequest command,
            MarketingFeeModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfee = _royaltyService.GetAllMarketingFees(model.Branch);


            var gridModel = new DataSourceResult
            {
                Data = marketingfee.Select(x =>
                {
                    var feelist = new MarketingFeeModel()
                    {
                        Id = x.Id,
                        Branch = x.Branch,
                        IssueDate = x.IssueDate,
                        Item = x.Item,
                        Amount = x.Amount,
                        Remarks = x.Remarks,
                        YN_Paid = x.YN_Paid,
                        PaidAmount = 0,
                        Balance = x.Amount

                    };

                    var payamount = _royaltyService.GetAllMarketingFeePayments(x.Id).Sum(a => a.Amount);
                    if (payamount == null) payamount = 0;

                    feelist.PaidAmount = payamount;
                    feelist.Balance = feelist.Amount - payamount;

                    return feelist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = marketingfee.TotalCount
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult MarketingFeeInsert([Bind(Exclude = "Id")] MarketingFeeModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var branchcode = model.Branch;

            if (branchcode == null || branchcode == "")
            {
                return Json(new DataSourceResult() { Errors = "Branch value is not assigned " });
            }

            var marketingfee = new MarketingFee( branchcode, model.IssueDate, model.Item, model.Amount, model.Remarks, "N", DateTime.Now, _workContext.CurrentCustomer.Username);

            _royaltyService.InsertMarketingFee(marketingfee);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult MarketingFeeUpdate(MarketingFeeModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfee = _royaltyService.GetMarketingFeeById(model.Id);
            if (marketingfee == null)
                return Json(new DataSourceResult() { Errors = "There is no marketingfee with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            //item = model.ToEntity(item);
            marketingfee.Branch = model.Branch;
            marketingfee.IssueDate = model.IssueDate;
            marketingfee.Item = model.Item;
            marketingfee.Amount = model.Amount;
            marketingfee.Remarks = model.Remarks;
            marketingfee.YN_Paid = model.YN_Paid;

            marketingfee.reg_date = DateTime.Now;
            marketingfee.reg_source = _workContext.CurrentCustomer.Username;

            _royaltyService.UpdateMarketingFee(marketingfee);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult MarketingFeeDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfee = _royaltyService.GetMarketingFeeById(id);
            if (marketingfee == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no marketingfee with the specified code" });


            //using (var scope = new System.Transactions.TransactionScope())
            //{

            // 등록된 입금정보가 있으면 같이 지운다.
            var marketingfeepayment = _royaltyService.GetAllMarketingFeePayments(id);
            foreach (var payment in marketingfeepayment)
            {
                _royaltyService.DeleteMarketingFeePayment(payment);
            }


            _royaltyService.DeleteMarketingFee(marketingfee);


            //    scope.Complete();
            //}

            return new NullJsonResult();
        }

        public ActionResult MarketingFeePaymentList(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfee = _royaltyService.GetMarketingFeeById(Id);
            if (marketingfee == null)
                return Json(new DataSourceResult() { Errors = "No marketingfee could be loaded with the specified Code" });


            var model = new MarketingFeePaymentModel();
            model.Fee_id = Id;

            return View(model); 
        }

        [HttpPost]
        public ActionResult GetMarketingFeePaymentList(DataSourceRequest command,
            MarketingFeePaymentModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {

            var payments = _royaltyService.GetAllMarketingFeePayments(model.Fee_id);

            var list = payments.Select(x =>
            {
                var marketingfeepaymentlist = new MarketingFeePaymentModel()
                {
                    Fee_id = model.Fee_id,
                    PayDate = x.PayDate,
                    Amount = x.Amount,
                    Remarks = x.Remarks
                };

                return marketingfeepaymentlist;
            })
            .OrderBy(x => x.PayDate)
            .AsQueryable().ToList();

            try
            {
                var gridModel = new DataSourceResult
                {
                    Data = list,
                    Total = list.Count
                };
                return Json(gridModel);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }

        }

        [HttpPost]
        public ActionResult MarketingFeePaymentInsert([Bind(Exclude = "Id")] MarketingFeePaymentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();


            var marketingfeepayment = new MarketingFeePayment(model.Fee_id, model.PayDate, model.Amount, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

            _royaltyService.InsertMarketingFeePayment(marketingfeepayment);

            return new NullJsonResult();
        }

        private void UpdateMarketingFeeBalance(int fee_id)
        {
            var marketingfee = _royaltyService.GetMarketingFeeById(fee_id);
            var payamount = _royaltyService.GetAllMarketingFeePayments(fee_id).Sum(a => a.Amount);
            if (payamount == null) payamount = 0;

            if ( marketingfee.Amount == payamount)
            {
                marketingfee.YN_Paid = "Y";
            }
            else
            {
                marketingfee.YN_Paid = "N";
            }

            _royaltyService.UpdateMarketingFee(marketingfee);
        }


        [HttpPost]
        public ActionResult MarketingFeepaymentUpdate(MarketingFeePaymentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfeepayment = _royaltyService.GetMarketingFeePaymentById(model.Id);
            if (marketingfeepayment == null)
                return Json(new DataSourceResult() { Errors = "There is no marketingfeepayment with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            //item = model.ToEntity(item);

            marketingfeepayment.PayDate = model.PayDate;
            marketingfeepayment.Amount = model.Amount;
            marketingfeepayment.Remarks = model.Remarks;
            marketingfeepayment.reg_date = DateTime.Now;
            marketingfeepayment.reg_source = _workContext.CurrentCustomer.Username;

            _royaltyService.UpdateMarketingFeePayment(marketingfeepayment);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult MarketingFeePaymentDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniFranchiseAdmin") && !_permissionService.Authorize("OmniAccountant"))
                return AccessDeniedView();

            var marketingfeepayment = _royaltyService.GetMarketingFeePaymentById(id);
            if (marketingfeepayment == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no marketingfeepayment with the specified code" });

            //using (var scope = new System.Transactions.TransactionScope())
            //{

            _royaltyService.DeleteMarketingFeePayment(marketingfeepayment);

            //    scope.Complete();
            //}

            return new NullJsonResult();
        }




	}
}