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
using Nop.Admin.Models.Omni_Analysis;

namespace Nop.Admin.Controllers
{
    public class Omni_AnalysisController : BaseAdminController
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
        private readonly IReportService _reportService;

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



        public Omni_AnalysisController(ICodeMasterService codeMasterService, IItemCategoryService itemCategoryService, IItemService itemService,
            IItemSetService itemSetService, IOrderService_Omni orderService, IRoyaltyService royaltyservice, ICourseMasterService courseService,
            ICalendarMasterService calendarService, ICampusService campusService, IPdfService_Omni pdfService_omni, IClassService classservice,
            IReportService reportService,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IThemeProvider themeProvider, IPermissionService permissionService,
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
            this._reportService = reportService;

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

        private void SetAvailableEnrolSummaryCombo(EnrolSummaryModel model)
        {

            model.AvailableYear1.Clear();
            model.AvailableYear2.Clear();
            model.AvailableBranch.Clear();

            /*
            model.AvailableYear1.Add(new SelectListItem() { Text = "NOW", Value = "NOW" });
            model.AvailableYear2.Add(new SelectListItem() { Text = "NOW", Value = "NOW" });
            */

            //model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = DateTime.Now.ToString("yyyy") });
            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = "NOW" });
            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-1).ToString("yyyy"), Value = DateTime.Now.AddYears(-1).ToString("yyyy") });
            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-2).ToString("yyyy"), Value = DateTime.Now.AddYears(-2).ToString("yyyy") });

            //model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = DateTime.Now.ToString("yyyy") });
            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = "NOW" });
            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-1).ToString("yyyy"), Value = DateTime.Now.AddYears(-1).ToString("yyyy") });
            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-2).ToString("yyyy"), Value = DateTime.Now.AddYears(-2).ToString("yyyy") });

            if (String.IsNullOrEmpty(_workContext.CurrentCustomer.AdminComment)){
                var branches = _campusService.GetAllCampus().OrderBy(x => x.Campus_Prefix);
                foreach (var x in branches)
                {
                    model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix });
                }
            }
            else
            {
                var branches = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == _workContext.CurrentCustomer.AdminComment);
                foreach (var x in branches)
                {
                    model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix, Selected = (x.Campus_Prefix == _workContext.CurrentCustomer.AdminComment) });
                }
            }
            /*
            var branches = _campusService.GetAllCampus().OrderBy(x => x.Campus_Prefix);
            foreach ( var x in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix });
            }
            */

            /*
            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {

                    case "CM05":
                        model.AvailableYear1.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode});
                        model.AvailableYear2.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                }
            }
            */
        }

        public ActionResult EnrolSummaryY2Y()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var model = new EnrolSummaryModel();

            SetAvailableEnrolSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetEnrolSummaryY2YData(string year1, string year2)
        {

            try
            {
                var enrol_y1 = _reportService.GetAllEnrolSummary(year1, "").Where(x => Convert.ToInt32(x.grade) <= 6)
                    .GroupBy(group => new {group.year, group.branch}).Select(group => new {group.Key.year, group.Key.branch,cnt = group.Sum(a => a.T1 + a.T2 + a.T3 + a.T4)}) ;
                var enrol_y2 = _reportService.GetAllEnrolSummary(year2, "").Where(x => Convert.ToInt32(x.grade) <= 6)
                    .GroupBy(group => new {group.year, group.branch}).Select(group => new {group.Key.year, group.Key.branch,cnt = group.Sum(a => a.T1 + a.T2 + a.T3 + a.T4)}) ;

                var branchlist = _codeMasterService.GetAllCampus().OrderBy(x => x.Campus_Prefix).ToList();

                var studentlist1 = new System.Collections.ArrayList();
                var studentlist2 = new System.Collections.ArrayList();

                foreach(var enrol in branchlist)
                {
                    var temp1 = enrol_y1.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();
                    var temp2 = enrol_y2.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();

                    if ( temp1.Count > 0 )
                    {
                        studentlist1.Add(temp1[0].cnt);
                    }
                    else
                    {
                        studentlist1.Add(0);
                    }

                    if (temp2.Count > 0)
                    {
                        studentlist2.Add(temp2[0].cnt);
                    }
                    else
                    {
                        studentlist2.Add(0);
                    }
                }

                var students = new System.Collections.ArrayList();
                students.Add(studentlist1);
                students.Add(studentlist2);

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        public ActionResult EnrolSummaryT2T()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var model = new EnrolSummaryModel();

            SetAvailableEnrolSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetEnrolSummaryT2TData(string year)
        {

            try
            {
                var enrol_y1 = _reportService.GetAllEnrolSummary(year, "").Where(x => Convert.ToInt32(x.grade) <= 6)
                    .GroupBy(group => new { group.year, group.branch })
                    .Select(group => new { group.Key.year
                        , group.Key.branch
                        , T1 = group.Sum(a => a.T1)
                        , T2 = group.Sum(a => a.T2)
                        , T3 = group.Sum(a => a.T3)
                        , T4 = group.Sum(a => a.T4) 
                    });


                var branchlist = _codeMasterService.GetAllCampus().OrderBy(x => x.Campus_Prefix).ToList();

                var studentlist1 = new System.Collections.ArrayList();
                var studentlist2 = new System.Collections.ArrayList();
                var studentlist3 = new System.Collections.ArrayList();
                var studentlist4 = new System.Collections.ArrayList();

                foreach (var enrol in branchlist)
                {
                    var temp1 = enrol_y1.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();
                    

                    if (temp1.Count > 0)
                    {
                        studentlist1.Add(temp1[0].T1);
                        studentlist2.Add(temp1[0].T2);
                        studentlist3.Add(temp1[0].T3);
                        studentlist4.Add(temp1[0].T4);
                    }
                    else
                    {
                        studentlist1.Add(0);
                        studentlist2.Add(0);
                        studentlist3.Add(0);
                        studentlist4.Add(0);
                    }

                }

                var students = new System.Collections.ArrayList();
                students.Add(studentlist1);
                students.Add(studentlist2);
                students.Add(studentlist3);
                students.Add(studentlist4);

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }




        public ActionResult EnrolSummaryBranch()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new EnrolSummaryModel();

            SetAvailableEnrolSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetEnrolSummaryBranchData1(string year1, string year2, string branch)
        {

            try
            {
                var enrol_y1 = _reportService.GetAllEnrolSummary(year1, branch).Where(x => Convert.ToInt32(x.grade) <= 6)
                    .GroupBy(group => new { group.year, group.branch })
                    .Select(group => new
                    {
                        group.Key.year
                        ,
                        group.Key.branch
                        ,
                        T1 = group.Sum(a => a.T1)
                        ,
                        T2 = group.Sum(a => a.T2)
                        ,
                        T3 = group.Sum(a => a.T3)
                        ,
                        T4 = group.Sum(a => a.T4)
                    });

                var enrol_y2 = _reportService.GetAllEnrolSummary(year2, branch).Where(x => Convert.ToInt32(x.grade) <= 6)
                    .GroupBy(group => new { group.year, group.branch })
                    .Select(group => new
                    {
                        group.Key.year
                        ,
                        group.Key.branch
                        ,
                        T1 = group.Sum(a => a.T1)
                        ,
                        T2 = group.Sum(a => a.T2)
                        ,
                        T3 = group.Sum(a => a.T3)
                        ,
                        T4 = group.Sum(a => a.T4)
                    });


                var studentlist1 = new System.Collections.ArrayList();
                var studentlist2 = new System.Collections.ArrayList();


                var temp1 = enrol_y1.ToList();

                var temp2 = enrol_y2.ToList();


                if (temp1.Count > 0)
                {
                    studentlist1.Add(temp1[0].T1);
                    studentlist1.Add(temp1[0].T2);
                    studentlist1.Add(temp1[0].T3);
                    studentlist1.Add(temp1[0].T4);
                }
                else
                {
                    studentlist1.Add(0);
                    studentlist1.Add(0);
                    studentlist1.Add(0);
                    studentlist1.Add(0);
                }


                if (temp2.Count > 0)
                {
                    studentlist2.Add(temp2[0].T1);
                    studentlist2.Add(temp2[0].T2);
                    studentlist2.Add(temp2[0].T3);
                    studentlist2.Add(temp2[0].T4);
                }
                else
                {
                    studentlist2.Add(0);
                    studentlist2.Add(0);
                    studentlist2.Add(0);
                    studentlist2.Add(0);
                }


                var students = new System.Collections.ArrayList();
                students.Add(studentlist1);
                students.Add(studentlist2);

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult EnrolSummaryGrade()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new EnrolSummaryModel();

            SetAvailableEnrolSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetEnrolSummaryGradeData(string year1, string year2, string branch)
        {

            try
            {
                var enrol_y1 = _reportService.GetAllEnrolSummary(year1, branch).Where(x => Convert.ToInt32(x.grade) <= 6);
                var enrol_y2 = _reportService.GetAllEnrolSummary(year2, branch).Where(x => Convert.ToInt32(x.grade) <= 6);

                var studentlist0 = new System.Collections.ArrayList( new int[8] );
                var studentlist1 = new System.Collections.ArrayList(new int[8]);
                var studentlist2 = new System.Collections.ArrayList(new int[8]);
                var studentlist3 = new System.Collections.ArrayList(new int[8]);
                var studentlist4 = new System.Collections.ArrayList(new int[8]);
                var studentlist5 = new System.Collections.ArrayList(new int[8]);
                var studentlist6 = new System.Collections.ArrayList(new int[8]);

                for (int i = 0; i < 7; i++ )
                {
                    var temp_y1 = enrol_y1.Where(x => Convert.ToInt32(x.grade) == i).ToList();
                    var temp_y2 = enrol_y2.Where(x => Convert.ToInt32(x.grade) == i).ToList();

                    var temparry = studentlist0;
                    switch(i)
                    {
                        case 1: temparry = studentlist1; break;
                        case 2: temparry = studentlist2; break;
                        case 3: temparry = studentlist3; break;
                        case 4: temparry = studentlist4; break;
                        case 5: temparry = studentlist5; break;
                        case 6: temparry = studentlist6; break;
                        default: temparry = studentlist0; break;
                    }


                    if ( temp_y1 != null && temp_y1.Count > 0)
                    {
                        temparry[0] = temp_y1[0].T1;
                        temparry[2] = temp_y1[0].T2;
                        temparry[4] = temp_y1[0].T3;
                        temparry[6] = temp_y1[0].T4;


                    }
                    else
                    {
                        temparry[0] = 0;
                        temparry[2] = 0;
                        temparry[4] = 0;
                        temparry[6] = 0;
                    }


                    if ( temp_y2 != null && temp_y2.Count > 0)
                    {
                        temparry[1] = temp_y2[0].T1;
                        temparry[3] = temp_y2[0].T2;
                        temparry[5] = temp_y2[0].T3;
                        temparry[7] = temp_y2[0].T4;
                    }
                    else
                    {
                        temparry[1] = 0;
                        temparry[3] = 0;
                        temparry[5] = 0;
                        temparry[7] = 0;
                    }

                }

                var students = new System.Collections.ArrayList();
                students.Add(studentlist0);
                students.Add(studentlist1);
                students.Add(studentlist2);
                students.Add(studentlist3);
                students.Add(studentlist4);
                students.Add(studentlist5);
                students.Add(studentlist6);

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        #region Test Summary

        private void SetAvailableTestSummaryCombo(TestSummaryModel model)
        {

            model.AvailableYear1.Clear();
            model.AvailableYear2.Clear();
            model.AvailableBranch.Clear();
            model.AvailableTestType.Clear();
            
            model.AvailableTestType.Add(new SelectListItem() { Text = "OC Trial Test", Value = "OC Trial Test" });
            model.AvailableTestType.Add(new SelectListItem() { Text = "Selective Trial Test", Value = "Selective Trial Test" });

            if (String.IsNullOrEmpty(_workContext.CurrentCustomer.AdminComment))
            {
                var branches = _campusService.GetAllCampus().OrderBy(x => x.Campus_Prefix);
                foreach (var x in branches)
                {
                    model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix });
                }
            }
            else
            {
                var branches = _campusService.GetAllCampus().Where(x => x.Campus_Prefix == _workContext.CurrentCustomer.AdminComment);
                foreach (var x in branches)
                {
                    model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix, Selected = (x.Campus_Prefix == _workContext.CurrentCustomer.AdminComment) });
                }
            }

            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = DateTime.Now.ToString("yyyy") });
            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-1).ToString("yyyy"), Value = DateTime.Now.AddYears(-1).ToString("yyyy") });
            model.AvailableYear1.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-2).ToString("yyyy"), Value = DateTime.Now.AddYears(-2).ToString("yyyy") });

            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.ToString("yyyy"), Value = DateTime.Now.ToString("yyyy") });
            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-1).ToString("yyyy"), Value = DateTime.Now.AddYears(-1).ToString("yyyy") });
            model.AvailableYear2.Add(new SelectListItem() { Text = DateTime.Now.AddYears(-2).ToString("yyyy"), Value = DateTime.Now.AddYears(-2).ToString("yyyy") });

            /*
            var branches = _campusService.GetAllCampus().OrderBy(x => x.Campus_Prefix);
            foreach (var x in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Text = x.Campus_Title, Value = x.Campus_Prefix });
            }
              
             
            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {

                    case "CM05":
                        model.AvailableYear1.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        model.AvailableYear2.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                }
            }
            */
        }

        public ActionResult TestSummary()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var model = new TestSummaryModel();

            SetAvailableTestSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetTestSummaryData(string year1,string year2)
        {
           
            try
            {
                
                var summarylist1 = _reportService.GetAllTestSummary(Convert.ToInt32(year1), "")
                    .Where(x => x.testtype == "OC Trial Test")
                    .GroupBy(group => new { group.myear, group.branch ,group.testtype})
                    .Select(group => new
                    {
                        group.Key.myear
                        ,
                        group.Key.branch
                        ,
                        cntmax = group.Max(a => a.cnt)
                    });

                var summarylist12 = _reportService.GetAllTestSummary(Convert.ToInt32(year2), "")
                    .Where(x => x.testtype == "OC Trial Test")
                    .GroupBy(group => new { group.myear, group.branch, group.testtype })
                    .Select(group => new
                    {
                        group.Key.myear
                        ,
                        group.Key.branch
                        ,
                        cntmax = group.Max(a => a.cnt)
                    });

                var summarylist2 = _reportService.GetAllTestSummary(Convert.ToInt32(year1), "")
                    .Where(x => x.testtype == "Selective Trial Test")
                    .GroupBy(group => new { group.myear, group.branch, group.testtype })
                    .Select(group => new
                    {
                        group.Key.myear
                        ,
                        group.Key.branch
                        ,
                        cntmax = group.Max(a => a.cnt)
                    });

                var summarylist22 = _reportService.GetAllTestSummary(Convert.ToInt32(year2), "")
                    .Where(x => x.testtype == "Selective Trial Test")
                    .GroupBy(group => new { group.myear, group.branch, group.testtype })
                    .Select(group => new
                    {
                        group.Key.myear
                        ,
                        group.Key.branch
                        ,
                        cntmax = group.Max(a => a.cnt)
                    });


                var branchlist = _codeMasterService.GetAllCampus().OrderBy(x => x.Campus_Prefix).ToList();

                var cntlist1 = new System.Collections.ArrayList();
                var cntlist12 = new System.Collections.ArrayList();
                var cntlist2 = new System.Collections.ArrayList();
                var cntlist22 = new System.Collections.ArrayList();
                
                foreach (var enrol in branchlist)
                {
                    var temp1 = summarylist1.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();
                    var temp12 = summarylist12.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();
                    var temp2 = summarylist2.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();
                    var temp22 = summarylist22.Where(x => x.branch.Trim() == enrol.Campus_Prefix).ToList();

                    if (temp1.Count > 0)
                    {
                        cntlist1.Add(temp1[0].cntmax);
                    }
                    else
                    {
                        cntlist1.Add(0);
                    }
                    if (temp12.Count > 0)
                    {
                        cntlist12.Add(temp12[0].cntmax);
                    }
                    else
                    {
                        cntlist12.Add(0);
                    }
                    if (temp2.Count > 0)
                    {
                        cntlist2.Add(temp2[0].cntmax);
                    }
                    else
                    {
                        cntlist2.Add(0);
                    }
                    if (temp22.Count > 0)
                    {
                        cntlist22.Add(temp22[0].cntmax);
                    }
                    else
                    {
                        cntlist22.Add(0);
                    }
                }

                var testsummarylists = new System.Collections.ArrayList();
                testsummarylists.Add(cntlist1);
                testsummarylists.Add(cntlist12);
                testsummarylists.Add(cntlist2);
                testsummarylists.Add(cntlist22);
                
                //return Json(summarylist, JsonRequestBehavior.AllowGet);
                return Json(testsummarylists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        #endregion

        #region Test Summary Branch

        public ActionResult TestSummaryBranch()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new TestSummaryModel();

            SetAvailableTestSummaryCombo(model);

            return View(model);
        }


        public ActionResult GetTestSummaryBranchData1(string year1, string year2, string branch, string testtype)
        {
           
            try
            {
                var testsummarybranch_y1 = _reportService.GetAllTestSummary(Convert.ToInt32(year1), branch).Where(x => x.testtype == testtype).OrderBy(x => x.test_no);

                var testsummarybranch_y2 = _reportService.GetAllTestSummary(Convert.ToInt32(year2), branch).Where(x => x.testtype == testtype).OrderBy(x => x.test_no);


                var testsummarybranchlist1 = new System.Collections.ArrayList();
                var testsummarybranchlist2 = new System.Collections.ArrayList();


                var temp1 = testsummarybranch_y1.ToList();

                var temp2 = testsummarybranch_y2.ToList();

                foreach(var list1 in temp1)
                {
                    testsummarybranchlist1.Add(list1.cnt);
                }
                foreach(var list2 in temp2)
                {
                    testsummarybranchlist2.Add(list2.cnt);
                }

              


                var testsummarylists = new System.Collections.ArrayList();
                testsummarylists.Add(testsummarybranchlist1);
                testsummarylists.Add(testsummarybranchlist2);

                return Json(testsummarylists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        #endregion
    }
}