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
using Nop.Admin.Models.Omni_Basis;
using Nop.Admin.Models.Omni_Class;


namespace Nop.Admin.Controllers
{
    public class Omni_ClassController : BaseAdminController
    {
        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly ICourseMasterService _coursemasterService;
        private readonly IClassService _classService;
        private readonly ICalendarMasterService _calendarService;
        private readonly IPdfService_Class _pdfService_Class;

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



        public Omni_ClassController(ICodeMasterService codeMasterService, ICourseMasterService coursemasterService, IClassService classService,
            ICalendarMasterService calendarService, IPdfService_Class pdfService_Class,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IThemeProvider themeProvider,  IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext, IPdfService pdfService, PdfSettings pdfSettings)
        {
            this._codeMasterService = codeMasterService;
            this._coursemasterService = coursemasterService;
            this._classService = classService;
            this._calendarService = calendarService;
            this._pdfService_Class = pdfService_Class;

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


        #region Teachers

        public ActionResult Teachers()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new TeacherModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableTeacherCombo(model);


            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;

            return View(model);
        }




        private void SetAvailableTeacherCombo(TeacherModel model)
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


            // ordertype, orderstatus, term, year combo
            model.AvailableTitle.Clear();
            model.AvailableGender.Clear();
            model.AvailableWorkingCond.Clear();
            model.AvailableTeachingGrade.Clear();
            model.AvailableSubject.Clear();

            model.AvailableTitle.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableGender.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableWorkingCond.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTeachingGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableSubject.Add(new SelectListItem() { Value = "", Selected = true, Text = "ALL" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL01":
                        model.AvailableTitle.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Title) });
                        break;
                    case "CL02":
                        model.AvailableGender.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Gender) });
                        break;
                    case "CL03":
                        model.AvailableWorkingCond.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.WorkingCond) });
                        break;
                    case "CL06":
                        model.AvailableTeachingGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.TeachingGrade) });
                        break;
                    case "CL07":
                        model.AvailableSubject.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                }
            }
        }




        [HttpPost]
        public ActionResult GetTeacherList(DataSourceRequest command,
            TeacherModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachers = _classService.GetAllTeachers("", model.FirstName, model.LastName, model.Subject);

            if (!String.IsNullOrEmpty(model.Subject))
            {

                for (int i = teachers.ToList().Count; i > 0 ; i-- )
                {
                    var teacher = teachers.ToList()[i-1];
                    var subjects = _classService.GetAllTeacherSubjects(teacher.Id).Where(x => x.Subject == model.Subject).ToList();

                    if (subjects.Count <= 0)
                    {
                        teachers.Remove(teacher);
                    }
                }
            }



            if (!String.IsNullOrEmpty(model.Branch))
            {

                for (int i = teachers.ToList().Count; i > 0; i--)
                {
                    var teacher = teachers.ToList()[i - 1];
                    var branches = _classService.GetAllTeacherBranchs(teacher.Id).Where(x => x.Branch == model.Branch).ToList();

                    if (branches.Count <= 0)
                    {
                        teachers.Remove(teacher);
                    }
                }
            }


            var gridModel = new DataSourceResult
            {
                Data = teachers.Select(x =>
                {
                    var orderlist = new TeacherModel()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Gender = x.Gender,
                        TeacherRole = x.TeacherRole,
                        TutorRole = x.TutorRole,
                        MarkerRole = x.MarkerRole,
                        TeachingGrade = x.TeachingGrade,
                        Subject = x.Subject,
                        WorkingCond = x.WorkingCond,
                        Address = x.Address,
                        Mobile = x.Mobile,
                        HomePhone = x.HomePhone,
                        eMail = x.eMail,
                        Branch = x.Branch,
                        Remarks = x.Remarks

                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = teachers.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult TeacherInsert([Bind(Exclude = "Id")] TeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //var branchcode = model.BranchCode;

            //if (branchcode == null || branchcode == "")
            //{
            //    branchcode = _workContext.CurrentCustomer.AdminComment;
            //}

            if (model.Mobile == null) model.Mobile = "";
            if (model.Remarks == null) model.Remarks = "";
            if (model.HomePhone == null) model.HomePhone = "";
            if (model.Address == null) model.Address = "";
            if (model.eMail == null) model.eMail = "";
            model.Subject = "";
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            if (model.Branch == null) model.Branch = "";

            var teacher = new Teacher(model.Title, model.FirstName, model.LastName, model.Gender, model.TeacherRole, model.TutorRole, model.MarkerRole
                , model.TeachingGrade, "SUBJECT", model.WorkingCond, model.Address
                , model.Mobile, model.HomePhone, model.eMail, model.Branch, DateTime.Now, _workContext.CurrentCustomer.Username, model.Remarks);

            try
            {
                _classService.InsertTeacher(teacher);

                if (model.Branch != "")
                {
                    var teacherbranch = new TeacherBranch(teacher.Id, teacher.Branch, DateTime.Now, _workContext.CurrentCustomer.Username);
                    _classService.InsertTeacherBranch(teacherbranch);
                }
            }
            catch(Exception ex)
            {
                string abc = ex.Message;
            }

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult TeacherUpdate(TeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(model.Id);
            if (teacher == null)
                return Json(new DataSourceResult() { Errors = "There is no teacher with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            teacher.Title = model.Title;
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.Gender = model.Gender;
            teacher.TeacherRole = model.TeacherRole;
            teacher.TutorRole = model.TutorRole;
            teacher.MarkerRole = model.MarkerRole;
            teacher.TeachingGrade = model.TeachingGrade;
            //teacher.Subject = model.Subject;
            teacher.WorkingCond = model.WorkingCond;
            teacher.Mobile = model.Mobile;
            teacher.HomePhone = model.HomePhone;
            teacher.eMail = model.eMail;
            //teacher.Branch = model.Branch;
            teacher.Address = model.Address;
            teacher.Remarks = model.Remarks;
            teacher.reg_date = DateTime.Now;
            teacher.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateTeacher(teacher);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TeacherDelete(TeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(model.Id);
            if (teacher == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no teacher with the specified code" });

            _classService.DeleteTeacher(teacher);

            return new NullJsonResult();
        }

        #endregion


        #region TeacherCareer

        private void SetAvailableTeacherCareerCombo(TeacherCareerModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableCareerType.Clear();
            model.AvailableCareerType.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL04":
                        model.AvailableCareerType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.CareerType) });
                        break;
                }
            }
        }




        public ActionResult TeacherCareer(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(Id);
            if (teacher == null)
                return Content("No teacher could be loaded with the specified Code");

            var title = _codeMasterService.GetAllCodeMasters("CL01", "Y").Where(x => x.DetailCode == teacher.Title);

            var model = new TeacherCareerModel();
            model.Teacher_Id = teacher.Id;
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            model.Title = title.ToList()[0].DetailName;

            SetAvailableTeacherCareerCombo(model);

            return View(model);

        }



        [HttpPost]
        public ActionResult GetTeacherCareerList(DataSourceRequest command,
            TeacherCareerModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachercareers = _classService.GetAllTeacherCareers(model.Teacher_Id).OrderBy(x => x.FromDate);

            var gridModel = new DataSourceResult
            {
                Data = teachercareers.Select(x =>
                {
                    var orderlist = new TeacherCareerModel()
                    {
                        Id = x.Id,
                        Teacher_Id = x.Teacher_Id,
                        CareerType = x.CareerType,
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        Remarks = x.Remarks
                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = teachercareers.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult TeacherCareerInsert([Bind(Exclude = "Id")] TeacherCareerModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (model.Class_Id == null) model.Class_Id = 0;

            var teachercareer = new TeacherCareer(model.Teacher_Id, model.CareerType, model.FromDate, model.ToDate, model.Class_Id
                , DateTime.Now, _workContext.CurrentCustomer.Username, model.Remarks);

            _classService.InsertTeacherCareer(teachercareer);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult TeacherCareerUpdate(TeacherCareerModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachercareer = _classService.GetTeacherCareerById(model.Id);
            if (teachercareer == null)
                return Json(new DataSourceResult() { Errors = "There is no teachercareer with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            teachercareer.CareerType = model.CareerType;
            teachercareer.FromDate = model.FromDate;
            teachercareer.ToDate = model.ToDate;
            teachercareer.Class_Id = model.Class_Id;
            teachercareer.Remarks = model.Remarks;
            teachercareer.reg_date = DateTime.Now;
            teachercareer.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateTeacherCareer(teachercareer);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TeacherCareerDelete(TeacherCareerModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachercareer = _classService.GetTeacherCareerById(model.Id);
            if (teachercareer == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no teachercareer with the specified code" });

            _classService.DeleteTeacherCareer(teachercareer);

            return new NullJsonResult();
        }

        #endregion


        #region TeacherSubject

        private void SetAvailableTeacherSubjectCombo(TeacherSubjectModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableSubject.Clear();
            model.AvailableSubject.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL07":
                        model.AvailableSubject.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Subject) });
                        break;
                }
            }
        }




        public ActionResult TeacherSubject(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(Id);
            if (teacher == null)
                return Content("No teacher could be loaded with the specified Code");

            var title = _codeMasterService.GetAllCodeMasters("CL01", "Y").Where(x => x.DetailCode == teacher.Title);

            var model = new TeacherSubjectModel();
            model.Teacher_Id = teacher.Id;
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            model.Title = title.ToList()[0].DetailName;

            /*popup*/
            model.teacherPopup = new TeacherModel();
            if (_permissionService.Authorize("OmniAdmin"))
                model.teacherPopup.is_Admin = true;
            SetAvailableTeacherCombo(model.teacherPopup);
            /*popup end*/

            SetAvailableTeacherSubjectCombo(model);

            return View(model);

        }



        [HttpPost]
        public ActionResult GetTeacherSubjectList(DataSourceRequest command,
            TeacherSubjectModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachersubjects = _classService.GetAllTeacherSubjects(model.Teacher_Id).OrderBy(x => x.Teacher_Id);

            var subjectlist = _codeMasterService.GetAllCodeMasters("CL07", "Y");

            foreach( var subjectcode in subjectlist)
            {
                if (teachersubjects.Where(y => y.Subject == subjectcode.DetailCode).ToList().Count > 0)
                {
                    subjectcode.String_01 = "Y";
                }
                else
                {
                    subjectcode.String_01 = "N";
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = subjectlist.Select(x =>
                {
                    var orderlist = new TeacherSubjectModel()
                    {
                        Id = x.Id,
                        Teacher_Id = model.Teacher_Id,
                        Subject = x.DetailCode,
                        Check = (x.String_01 == "Y") ? true : false
                    };

                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = teachersubjects.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult TeacherSubjectUpdate(TeacherSubjectModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachersubjects = _classService.GetAllTeacherSubjects(model.Teacher_Id);

            if ( model.Check )
            {
                // Insert
                TeacherSubject teachersubject = new TeacherSubject( model.Teacher_Id, model.Subject, DateTime.Now, _workContext.CurrentCustomer.Username);
                _classService.InsertTeacherSubject(teachersubject);
            }
            else
            {
                // delete
                TeacherSubject teachersubject = teachersubjects.Where( x => x.Subject == model.Subject ).ToList()[0];
                _classService.DeleteTeacherSubject(teachersubject);
            }
            return new NullJsonResult();
        }

        #endregion


        #region TeacherBranch


        private void SetAvailableTeacherBranchCombo(TeacherBranchModel model)
        {
            // branch combo

            var branches = this._codeMasterService.GetAllCampus();
            model.AvailableBranch.Clear();
            model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }
        }




        public ActionResult TeacherBranch(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(Id);
            if (teacher == null)
                return Content("No teacher could be loaded with the specified Code");

            var title = _codeMasterService.GetAllCodeMasters("CL01", "Y").Where(x => x.DetailCode == teacher.Title);

            var model = new TeacherBranchModel();
            model.Teacher_Id = teacher.Id;
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            model.Title = title.ToList()[0].DetailName;
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            model.campuspopup = new CampusModel();


            SetAvailableTeacherBranchCombo(model);

            return View(model);

        }



        [HttpPost]
        public ActionResult GetTeacherBranchList(DataSourceRequest command,
            TeacherBranchModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacherbranchs = _classService.GetAllTeacherBranchs(model.Teacher_Id).OrderBy(x => x.Teacher_Id);

            var gridModel = new DataSourceResult
            {
                Data = teacherbranchs.Select(x =>
                {
                    var orderlist = new TeacherBranchModel()
                    {
                        Id = x.Id,
                        Teacher_Id = model.Teacher_Id,
                        Branch = x.Branch,
                        BranchPop = x.Branch,
                        Check = true
                    };

                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = teacherbranchs.ToList().Count
            };

            return Json(gridModel);

        }



        [HttpPost]
        public ActionResult TeacherBranchInsert([Bind(Exclude = "Id")] TeacherBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacherbranch = new TeacherBranch(model.Teacher_Id, model.Branch, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertTeacherBranch(teacherbranch);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult TeacherBranchUpdate(TeacherBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacherbranch = _classService.GetTeacherBranchById(model.Id);
            if (teacherbranch == null)
                return Json(new DataSourceResult() { Errors = "There is no teacherbranch with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            teacherbranch.Branch = model.Branch;
            teacherbranch.reg_date = DateTime.Now;
            teacherbranch.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateTeacherBranch(teacherbranch);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult TeacherBranchDelete(TeacherBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacherbranch = _classService.GetTeacherBranchById(model.Id);
            if (teacherbranch == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no teacherbranch with the specified code" });

            _classService.DeleteTeacherBranch(teacherbranch);

            return new NullJsonResult();
        }

        #endregion


        #region Evaluation

        private void SetAvailableEvaluationCombo(EvaluationModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableEvaluationType.Clear();
            model.AvailableEvaluationType.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL05":
                        model.AvailableEvaluationType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.EvaluationType  ) });
                        break;
                }
            }
        }




        public ActionResult Evaluation(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(Id);
            if (teacher == null)
                return Content("No teacher could be loaded with the specified Code");

            var title = _codeMasterService.GetAllCodeMasters("CL01", "Y").Where(x => x.DetailCode == teacher.Title);

            var model = new EvaluationModel();
            model.Teacher_Id = teacher.Id;
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            model.Title = title.ToList()[0].DetailName;

            SetAvailableEvaluationCombo(model);

            return View(model);

        }



        [HttpPost]
        public ActionResult GetEvaluationList(DataSourceRequest command,
            EvaluationModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var evaluations = _classService.GetAllEvaluations(model.Teacher_Id).OrderBy(x => x.reg_date);

            var gridModel = new DataSourceResult
            {
                Data = evaluations.Select(x =>
                {
                    var orderlist = new EvaluationModel()
                    {
                        Id = x.Id,
                        EvaluationType = x.EvaluationType,
                        Teacher_Id = x.Teacher_Id,
                        Class_Id = x.Class_Id,
                        Evaluator = x.Evaluator,
                        Remarks = x.Remarks,
                        Score = x.Score,
                        reg_date = x.reg_date
                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = evaluations.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult EvaluationInsert([Bind(Exclude = "Id")] EvaluationModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (model.Class_Id == null) model.Class_Id = 0;

            var evaluation = new Evaluation(model.EvaluationType, model.Teacher_Id, model.Class_Id, model.Evaluator, model.Score,
                 DateTime.Now, _workContext.CurrentCustomer.Username, model.Remarks);

            _classService.InsertEvaluation(evaluation);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult EvaluationUpdate(EvaluationModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var evaluation = _classService.GetEvaluationById(model.Id);
            if (evaluation == null)
                return Json(new DataSourceResult() { Errors = "There is no evaluation with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            evaluation.EvaluationType = model.EvaluationType;
            evaluation.Evaluator = model.Evaluator;
            evaluation.Score = model.Score;
            evaluation.Class_Id = model.Class_Id;
            evaluation.Remarks = model.Remarks;
            evaluation.reg_date = DateTime.Now;
            evaluation.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateEvaluation(evaluation);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult EvaluationDelete(EvaluationModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var evaluation = _classService.GetEvaluationById(model.Id);
            if (evaluation == null)
                return Json(new DataSourceResult() { Errors = "There is no evaluation with the specified code" });

            _classService.DeleteEvaluation(evaluation);

            return new NullJsonResult();
        }


        #endregion


        #region TeacherCV



        private void SetAvailableTeacherCVCombo(TeacherSubjectModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableSubject.Clear();
            model.AvailableSubject.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL07":
                        model.AvailableSubject.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Subject) });
                        break;
                }
            }
        }




        public ActionResult TeacherCV(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teacher = _classService.GetTeacherById(Id);
            if (teacher == null)
                return Content("No teacher could be loaded with the specified Code");

            var title = _codeMasterService.GetAllCodeMasters("CL01", "Y").Where(x => x.DetailCode == teacher.Title);

            var model = new TeacherSubjectModel();
            model.Teacher_Id = teacher.Id;
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            model.Title = title.ToList()[0].DetailName;

            SetAvailableTeacherSubjectCombo(model);

            return View(model);

        }



        [HttpPost]
        public ActionResult GetTeacherCVList(DataSourceRequest command,
            TeacherSubjectModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachersubjects = _classService.GetAllTeacherSubjects(model.Teacher_Id).OrderBy(x => x.Teacher_Id);

            var subjectlist = _codeMasterService.GetAllCodeMasters("CL07", "Y");

            foreach (var subjectcode in subjectlist)
            {
                if (teachersubjects.Where(y => y.Subject == subjectcode.DetailCode).ToList().Count > 0)
                {
                    subjectcode.String_01 = "Y";
                }
                else
                {
                    subjectcode.String_01 = "N";
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = subjectlist.Select(x =>
                {
                    var orderlist = new TeacherSubjectModel()
                    {
                        Id = x.Id,
                        Teacher_Id = model.Teacher_Id,
                        Subject = x.DetailCode,
                        Check = (x.String_01 == "Y") ? true : false
                    };

                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = teachersubjects.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult TeacherCVUpdate(TeacherSubjectModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var teachersubjects = _classService.GetAllTeacherSubjects(model.Teacher_Id);

            if (model.Check)
            {
                // Insert
                TeacherSubject teachersubject = new TeacherSubject(model.Teacher_Id, model.Subject, DateTime.Now, _workContext.CurrentCustomer.Username);
                _classService.InsertTeacherSubject(teachersubject);
            }
            else
            {
                // delete
                TeacherSubject teachersubject = teachersubjects.Where(x => x.Subject == model.Subject).ToList()[0];
                _classService.DeleteTeacherSubject(teachersubject);
            }
            return new NullJsonResult();
        }



        /*   I am not sure delete this or not
        private void SetAvailableEvaluationCombo(EvaluationModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");

            model.AvailableEvaluationType.Clear();
            model.AvailableEvaluationType.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });


            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "CL07":
                        model.AvailableEvaluationType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.EvaluationType) });
                        break;
                }
            }
        }
         * 
         * */
        #endregion


        #region ClassRooms

        public ActionResult ClassRooms()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassRoomModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClassRoomCombo(model);
            
            model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin"))
                model.isadmin = true;
            
            return View(model);
        }




        private void SetAvailableClassRoomCombo(ClassRoomModel model)
        {
            //var codelist = this._codeMasterService.GetAllCodeMasters("", "Y"); 
            var branches = this._codeMasterService.GetAllCampus();
            //var popupbranches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            //model.AvailablePopupBranch.Clear();

            model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
           // model.AvailablePopupBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            var branchs = _codeMasterService.GetAllCampus();
                       

             foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
               
            }

            //var popupbranch = _codeMasterService.GetAllCampus();

            //foreach (var popupcampus in popupbranch)
            //{
                
            //    model.AvailablePopupBranch.Add(new SelectListItem() { Value = popupcampus.Campus_Prefix, Text = popupcampus.Campus_Title, Selected = (popupcampus.Campus_Prefix == model.PopupBranch) });
            //}
            // ordertype, orderstatus, term, year combo
            // model.AvailableTitle.Clear();

            // model.AvailableTitle.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });



        }




        [HttpPost]
        public ActionResult GetClassRoomList(DataSourceRequest command,
            ClassRoomModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            //if (model.PopupBranch != null) model.Branch = model.PopupBranch;
               
            var classrooms = _classService.GetAllClassRooms(model.Branch,model.Title);
          
            var gridModel = new DataSourceResult
            {
                Data = classrooms.Select(x =>
                {
                    var orderlist = new ClassRoomModel()
                    {
                        Id = x.Id,
                        //Roompop_Id = x.Id,
                        Branch = x.Branch,
                        Title = x.Title,
                        Remarks = x.Remarks

                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classrooms.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ClassRoomInsert([Bind(Exclude = "Id")] ClassRoomModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //var branchcode = model.BranchCode;

            //if (branchcode == null || branchcode == "")
            //{
            //    branchcode = _workContext.CurrentCustomer.AdminComment;
            //}

            if (model.Remarks == null) model.Remarks = "";

            var classroom = new ClassRoom(model.Id, model.Branch, model.Title, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassRoom(classroom);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassRoomUpdate(ClassRoomModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classroom = _classService.GetClassRoomById(model.Id);
            if (classroom == null)
                return Json(new DataSourceResult() { Errors = "There is no classroom with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            classroom.Branch = model.Branch;
            classroom.Title = model.Title;
            classroom.Remarks = model.Remarks;
            classroom.reg_date = DateTime.Now;
            classroom.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassRoom(classroom);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassRoomDelete(ClassRoomModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classroom = _classService.GetClassRoomById(model.Id);
            if (classroom == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no classroom with the specified code" });

            _classService.DeleteClassRoom(classroom);

            return new NullJsonResult();
        }

        #endregion


        #region ClassInfos

        public ActionResult ClassList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager") )
                return AccessDeniedView();

            var model = new ClassInfoModel();
            //var categorys = this._classService.GetAllItemCategorys();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniClassManager"))
                model.editable = true;

            //model.is_Admin = false;
            model.Branch = _workContext.CurrentCustomer.AdminComment;

            //if (( model.Branch == null) || (model.Branch == ""))
            //{
            //    model.is_Admin = true;
            //}
            SetAvailableClassListCombo(model, true);
            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;

            return View(model);
        }

        private void SetAvailableClassListCombo(ClassInfoModel model, bool newmodel)
        {
            model.AvailableBranchs.Clear();
            model.AvailableTerms.Clear();
            model.AvailableDayofWeek.Clear();
            model.AvailableYear.Clear();
            model.AvailableClassRoom.Clear();
            model.AvailableCourse.Clear();
            model.AvailableTeacher.Clear();
            model.AvailableGrade.Clear();

            model.AvailableTerms.Add(new SelectListItem() { Text = "NO Term", Value = "", Selected = true });
            model.AvailableBranchs.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableDayofWeek.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableClassRoom.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableCourse.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableTeacher.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });

            if (newmodel)
            {
                string curYear = DateTime.Now.Year.ToString();

                var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y");
                string curTerm = "";
                foreach (var term in calendar)
                {
                    curTerm = term.Term;
                    curYear = term.Year;
                }

                model.Year = curYear;
                model.Term = curTerm;
            }
            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTerms.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM08":
                        model.AvailableDayofWeek.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.DayofWeek) });
                        break;
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.DayofWeek) });
                        break;

                }
            }
            //var codelist = this._codeMasterService.GetAllCodeMasters("", "Y"); 
            var branches = this._codeMasterService.GetAllCampus();


            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranchs.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

            var Rooms = this._classService.GetAllClassRooms(model.Branch, "");

            foreach (var room in Rooms)
            {
              
                model.AvailableClassRoom.Add(new SelectListItem() { Value = Convert.ToString(room.Id), Text = room.Title, Selected = (room.Id == model.Classroom_Id) });
                //model.AvailableClassRoom.Add(new SelectListItem() { Value = room.Title, Text = Convert.ToString(room.Id) });
            }

            var coursemasters = this._coursemasterService.GetAllCourseMasters("", "", "", "").Where(x => x.CourseID_P == 0);

            foreach (var cour in coursemasters)
            {
                model.AvailableCourse.Add(new SelectListItem() { Value = Convert.ToString(cour.Id), Text = cour.CourseName, Selected = (cour.Id == model.Course_Id) });
            }

            var teacher = this._classService.GetAllTeachers("", "", "", ""); // .GetAllCourseMasters(model.Year, model.Term, "", "").Where(x => x.CourseID_P == 0);

            foreach (var classteacher in teacher)
            {
                model.AvailableTeacher.Add(new SelectListItem() { Value = Convert.ToString(classteacher.Id) , Text = classteacher.FirstName+classteacher.LastName , Selected = (classteacher.Id == model.Teacher_Id) });
            }

        }

        public ActionResult GetCourseList(string year, string term, string grade)
        {
            var courses = _coursemasterService.GetAllCourseMasters(year, term, "", grade);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassList(string branch, string year, string term, string grade)
        {
            var classlist = _classService.GetAllClassInfos(branch, year, term).Where(x => x.Grade == grade).OrderBy(x => x.Grade).ThenBy(x => x.Name);

            return Json(classlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetClassList(DataSourceRequest command,
            ClassInfoModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager") )
                return AccessDeniedView();
            model.is_Admin = false;
           
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;

           var classes = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term).Where(x => true);

            if ( !String.IsNullOrEmpty(model.DayofWeek) )
            {
                classes = classes.Where(x => x.DayofWeek == model.DayofWeek);
            }

            if (!String.IsNullOrEmpty(model.Grade))
            {
                classes = classes.Where(x => x.Grade == model.Grade);
            }

            classes = classes.OrderBy(x => x.DayofWeek).ThenBy(x => x.Grade).ThenBy(x => x.StartTime);

           try
           {
               var gridModel = new DataSourceResult
               {
                   Data = classes.Select(x =>
                   {
                       var classList = new ClassInfoModel()
                       {
                           Id = x.Id,
                           Year = x.Year,
                           Term = x.Term,
                           Grade = x.Grade,
                           Course_Id = x.Course_Id,
                           Branch = x.Branch,
                           Name = x.Name,
                           DayofWeek = x.DayofWeek,
                           StartTime = x.StartTime,
                           EndTime = x.EndTime,
                           Classroom_Id = x.Classroom_Id
                       };
                       var classteachers = _classService.GetAllClassTeachers(x.Id).ToList();
                       if (classteachers.Count > 0)
                       {
                           var teacher = _classService.GetTeacherById(classteachers[0].Teacher_Id);

                           if (teacher.FirstName != null)
                           {
                               classList.FirstName = teacher.FirstName +' '+ teacher.LastName;
                               classList.LastName = teacher.LastName;
                           }
                       }
                      
                       return classList;
                   })
                   .AsQueryable()
                   .Filter(filter)
                   .Sort(sort),
                   Total = classes.ToList().Count
               };
               return Json(gridModel);

           }catch(Exception ex)
           {
               string t = ex.Message;
           }

           return Json(null);
        }


        public ActionResult MakeCheckedSchedule(string selectedIds)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();

            foreach (var id in ids)
            {
                MakeSchedules(id);
            }


            return RedirectToAction("ClassList");
        }


        private void SetAvailableClassCombo(ClassInfoModel model)
        {
            model.AvailableYear.Clear();
            model.AvailableTerms.Clear();
            model.AvailableGrade.Clear();
            model.AvailableBranchs.Clear();
            model.AvailableDayofWeek.Clear();
            model.AvailableClassRoom.Clear();
            model.AvailableCourse.Clear();
            model.AvailableLevelList.Clear();
            model.AvailablePayStatusList.Clear();
            model.AvailableTeacher.Clear();
            model.AvailableClass.Clear();

            model.AvailableYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select"});
            model.AvailableTerms.Add(new SelectListItem() { Value = "", Selected = true, Text = "NO Term", });
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "NO Grade", });
            model.AvailableBranchs.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableDayofWeek.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableClassRoom.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableCourse.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableLevelList.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailablePayStatusList.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableTeacher.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });


            if ( model.Id > 0 && !String.IsNullOrEmpty(model.Grade) )
            {
                var classlist = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term).Where(x => x.Grade == model.Grade);
                foreach(var classinfo in classlist)
                {
                    model.AvailableClass.Add(new SelectListItem() { Text = classinfo.Name, Value = classinfo.Id.ToString() });
                }
            }


            string curYear = DateTime.Now.Year.ToString();
            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y");
            foreach (var term in calendar)
            {
                model.Term = term.Term;
                model.Year = term.Year;
            }
            //var calendarmasters = _calendarMasterService.GetAllCalendarMasters();

            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                    case "CM02":
                        model.AvailableTerms.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM05":
                        model.AvailableYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM08":
                        model.AvailableDayofWeek.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.DayofWeek) });
                        break;
                    case "CM03":
                        if (x.String_01 == "WEMG")
                        {
                            model.AvailableLevelList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.DayofWeek) });
                        }
                        break;
                    case "CL09":
                        model.AvailablePayStatusList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.DayofWeek) });
                        break;
                }
            }

            var branches = this._codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranchs.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

            var Rooms = this._classService.GetAllClassRooms(model.Branch, "");

            foreach (var room in Rooms)
            {
                model.AvailableClassRoom.Add(new SelectListItem() { Value = Convert.ToString(room.Id), Text = room.Title, Selected = (room.Id == model.Classroom_Id) });
            }

            var coursemasters = this._coursemasterService.GetAllCourseMasters("", "", "", "").Where(x => x.CourseID_P == 0);

            foreach (var cour in coursemasters)
            {
                model.AvailableCourse.Add(new SelectListItem() { Value = Convert.ToString(cour.Id), Text = cour.CourseName, Selected = (cour.Id == model.Course_Id) });
            }
            
            var teacher = this._classService.GetAllTeachers("", "", "", ""); 

            foreach (var classteacher in teacher)
            {
                var teachersubjects = _classService.GetAllTeacherSubjects(classteacher.Id).ToList();
                if (teachersubjects.Count > 0)
                {
                    classteacher.Subject = teachersubjects[0].Subject;

                    //var subjectstring = "";

                    //foreach(var sub in teachersubjects)
                    //{

                    //}


                    //orderlist.Subject = 
                }
                model.AvailableTeacher.Add(new SelectListItem() { Value = Convert.ToString(classteacher.Id), Text = classteacher.FirstName + classteacher.LastName + " ( " + classteacher.Subject + " ) " , Selected = (classteacher.Id == model.Teacher_Id) });
            }
        }


        public ActionResult CreateClass()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassInfoModel();
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClassCombo(model);

            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;
           
           
            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateClass(ClassInfoModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //string itemcode = _itemService.GenerateItemCode(model.ItemCategory);
            model.StartTime = new DateTime(1900, 1, 1, model.StartTime.Hour, model.StartTime.Minute, model.StartTime.Second);
            model.EndTime = new DateTime(1900, 1, 1, model.EndTime.Hour, model.EndTime.Minute, model.EndTime.Second);

            var classinfo = new ClassInfo(
                        model.Year,
                        model.Term,
                        model.Branch,
                        model.Grade,
                        model.Name,
                        model.DayofWeek,
                        model.StartTime,
                        model.EndTime,
                        model.Duration,
                        model.Classroom_Id,
                        model.Course_Id,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username);

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }


            _classService.InsertClassInfo(classinfo);

            SetAvailableClassCombo(model);
      
            return continueEditing ? RedirectToAction("ClassEdit", new { id = model.Id }) : RedirectToAction("ClassList");

        }

        public ActionResult EditClass(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
           // model.classroompopup = new ClassRoomModel();
            var classinfo = _classService.GetClassInfoById(id);
            if (classinfo == null)
                //No gift card found with the specified id
                return RedirectToAction("ClassList");

            var model = new Nop.Admin.Models.Omni_Class.ClassInfoModel()
            {
                
                Id = classinfo.Id,
                Year = classinfo.Year,
                Term = classinfo.Term,
                Grade = classinfo.Grade,
                Name = classinfo.Name,
                Course_Id = classinfo.Course_Id,
                Branch = classinfo.Branch,
                DayofWeek = classinfo.DayofWeek,
                StartTime = classinfo.StartTime,
                EndTime = classinfo.EndTime,
                Duration = classinfo.Duration,
                Classroom_Id = classinfo.Classroom_Id
               
            };
            SetAvailableClassCombo(model);
          
            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult EditClass(ClassInfoModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            //model.classroompopup = new ClassRoomModel();
            //SetAvailableClassRoomCombo(model.classroompopup);
            
            var classinfo = _classService.GetClassInfoById(model.Id);


            if (ModelState.IsValid)
            {
                model.StartTime = new DateTime(1900, 1, 1, model.StartTime.Hour, model.StartTime.Minute, model.StartTime.Second);
                model.EndTime = new DateTime(1900, 1, 1, model.EndTime.Hour, model.EndTime.Minute, model.EndTime.Second);

                classinfo.Year = model.Year;
                classinfo.Term = model.Term;
                classinfo.Grade = model.Grade;
                classinfo.Branch = model.Branch;
                classinfo.Name = model.Name;
                classinfo.DayofWeek = model.DayofWeek;
                classinfo.StartTime = model.StartTime;
                classinfo.EndTime = model.EndTime;
                classinfo.Duration = model.Duration;
                classinfo.Classroom_Id = model.Classroom_Id;
                classinfo.Course_Id = model.Course_Id;
                classinfo.reg_date = DateTime.Now;
                classinfo.reg_source = _workContext.CurrentCustomer.Username;
                _classService.UpdateClassInfo(classinfo);


                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    SetAvailableClassCombo(model);

                    return RedirectToAction("EditClass", classinfo.Id);
                }
                else
                {
                    return RedirectToAction("ClassList");
                }
            }
            //model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            
            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            //1.check class enroll
            var classenrols = _classService.GetAllClassEnrols(id);
            if(classenrols.Count > 0)
            {
                return Json(new DataSourceResult() { Errors = "There is class enrol member." });
            }
            //2.delete class schedule
            var classschedule = _classService.GetAllClassSchedules(id).ToList();
            foreach(var schedule in classschedule)
            {
                _classService.DeleteClassSchedule(schedule);
            }
            //3.delete class info
            var classinfo = _classService.GetClassInfoById(id);
            if (classinfo == null)
                //No gift card found with the specified id
                return RedirectToAction("ClassList");

            _classService.DeleteClassInfo(classinfo);


            SuccessNotification(_localizationService.GetResource("Admin.Class.Deleted"));
            return RedirectToAction("ClassList");
        }
        #endregion

        
        #region ClassTeacher

        public ActionResult ClassTeachers()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassTeacherModel();

            SetAvailableClassTeacherCombo(model);

           return View(model);
        }




        private void SetAvailableClassTeacherCombo(ClassTeacherModel model)
        {
            //var codelist = this._codeMasterService.GetAllCodeMasters("", "Y"); 
            //var branches = this._codeMasterService.GetAllCampus();
           // var teachers = this._classService.GetAllTeachers("", "", "", "");
           // model.AvailableTeacher.Clear();
            //// branch combo
            //model.AvailableBranch.Clear();
            //model.AvailableBranch.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            //var branchs = _codeMasterService.GetAllCampus();

            //foreach (var campus in branches)
            //{
            //    model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            //}


            // ordertype, orderstatus, term, year combo
            // model.AvailableTitle.Clear();

            // model.AvailableTitle.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });



        }




        [HttpPost]
        public ActionResult GetClassTeacherList(DataSourceRequest command,
            ClassTeacherModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classteachers = _classService.GetAllClassTeachers(model.Class_Id);
            
            var gridModel = new DataSourceResult
            {
                Data = classteachers.Select(x =>
                {
                    var classteacherlist = new ClassTeacherModel()
                    {
                        Id = x.Id,
                        Class_Id = x.Class_Id,
                        Teacher_Id = x.Teacher_Id,
                        Duration = x.Duration,
                        Remark = x.Remarks

                    };
                    if (x.Teacher_Id != null)
                    {
                        var teacher = _classService.GetTeacherById(x.Teacher_Id);
                        if (teacher.FirstName != null)
                        {
                            classteacherlist.FirstName = teacher.FirstName;
                            classteacherlist.LastName = teacher.LastName;
                        }
                        var teachersubjects = _classService.GetAllTeacherSubjects(x.Teacher_Id).ToList();
                        if (teachersubjects.Count > 0)
                        {
                            classteacherlist.Subject = teachersubjects[0].Subject;

                            //var subjectstring = "";

                            //foreach(var sub in teachersubjects)
                            //{

                            //}


                            //orderlist.Subject = 
                        }
                        //var teacher = _classService.GetTeacherById(x.Teacher_Id); 
                    }
                    return classteacherlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classteachers.ToList().Count// TotalCount
            };
            
            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult ClassTeacherInsert([Bind(Exclude = "Id")] ClassTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            
            if (model.Remark == null) model.Remark = "";

            var classteacher = new ClassTeacher(model.Class_Id, model.Teacher_Id, model.Duration, model.Remark, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassTeacher(classteacher);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassTeacherUpdate(ClassTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classteacher = _classService.GetClassTeacherById(model.Id);
            if (classteacher == null)
                return Json(new DataSourceResult() { Errors = "There is no classteacher with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            classteacher.Class_Id = model.Class_Id;
            classteacher.Teacher_Id = model.Teacher_Id;
            classteacher.Duration = model.Duration;
            classteacher.Remarks = model.Remark;
            classteacher.reg_date = DateTime.Now;
            classteacher.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassTeacher(classteacher);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassTeacherDelete(ClassTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classteacher = _classService.GetClassTeacherById(model.Id);
            if (classteacher == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no classteacher with the specified code" });

            _classService.DeleteClassTeacher(classteacher);

            return new NullJsonResult();
        }

        #endregion


        #region ClassEnrol

        public ActionResult ClassEnrols( string Stud_Id, string branch )
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassEnrolModel();
            var student = _classService.GetAllMembers(branch, "", "", "", Stud_Id, "").ToList();

            if ( student.Count != 1)
            {
                return Content("No Student could be loaded with the specified Code");
            }

            model.Stud_Id = student[0].stud_id;
            model.FirstName = student[0].stud_first_name;
            model.LastName = student[0].stud_last_name;
            model.Grade = "YR" + Int32.Parse(student[0].grade).ToString("00");
            model.Branch = student[0].branch.Trim();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y");
            string curTerm = "";
            string curYear = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;

            if (_workContext.CurrentCustomer.AdminComment == "")
            {
                model.is_admin = true;
            }


            SetAvailableClassEnrolCombo(model);

            return View(model);
        }

        public ActionResult ClassEnrols2(string Stud_Id, string branch, string year, string term, string grade)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassEnrolModel();
            var student = _classService.GetAllMembers(branch, "", "", "", Stud_Id, "").ToList();

            if (student.Count != 1)
            {
                return Content("No Student could be loaded with the specified Code");
            }

            model.Stud_Id = student[0].stud_id;
            model.FirstName = student[0].stud_first_name;
            model.LastName = student[0].stud_last_name;
            model.Grade = grade;
            model.Branch = student[0].branch.Trim();

            if (_workContext.CurrentCustomer.AdminComment == "")
            {
                model.is_admin = true;
            }

            model.Year = year;
            model.Term = term;

            SetAvailableClassEnrolCombo(model);

            return View("ClassEnrols", model);
        }




        private void SetAvailableClassEnrolCombo(ClassEnrolModel model)
        {
            model.AvailableYearList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableBranchList.Clear();
            model.AvailablePayStatusList.Clear();
            model.AvailableDiscount_Code.Clear();

            model.AvailablePayMethod.Clear();
            model.AvailableClassList.Clear();
            model.AvailableLevel.Clear();
            model.AvailableGrade.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYearList.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableBranchList.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });
            model.AvailablePayStatusList.Add(new SelectListItem() { Text = "N/A", Value = "", Selected = true });
            //model.AvailableDiscount_Code.Add(new SelectListItem() { Text = "Not discounted", Value = "NOTA", Selected = true });
            model.AvailablePayMethod.Add(new SelectListItem() { Text = "Not Selected", Value = "", Selected = true });
            model.AvailableClassList.Add(new SelectListItem() { Text = "Not Selected", Value = "0", Selected = true });
            model.AvailableLevel.Add(new SelectListItem() { Text = "Not Selected", Value = "XXXX", Selected = true });
            model.AvailableGrade.Add(new SelectListItem() { Text = "Not Selected", Value = "", Selected = true });


            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();
            var classlist = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term).Where(x => x.Grade == model.Grade);

            foreach (var classinfo in classlist)
            {
                model.AvailableClassList.Add(new SelectListItem() { Value = classinfo.Id.ToString(), Text = classinfo.Name });
            }


            foreach (var branch in branchs)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }



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
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Grade == x.DetailCode) });
                        break;
                    case "CL09":
                        model.AvailablePayStatusList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                    case "CL08":
                        model.AvailableDiscount_Code.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                    case "CL10":
                        model.AvailablePayMethod.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                    case "CM03":
                        if (x.String_01 == "WEMG")
                        {
                            model.AvailableLevel.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        }
                        break;
                }
            }

        }




        [HttpPost]
        public ActionResult GetClassEnrolList(DataSourceRequest command,
            ClassEnrolModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var courses = _coursemasterService.GetAllCourseMasters(model.Year, model.Term, "", model.Grade)
                .OrderByDescending(x => x.CourseCategory.Replace("MG", "WE").Replace('Z', 'A'));

            var classenrols = _classService.GetAllClassEnrols(0).Where(x => x.Stud_Id == model.Stud_Id);

            var gridModel = new DataSourceResult
            {
                Data = courses.Select(x =>
                {
                    var orderlist = new ClassEnrolModel()
                    {
                        Id = x.Id,
                        Year = model.Year,
                        Term = model.Term,
                        Grade = model.Grade,
                        Stud_Id = model.Stud_Id,
                        Course_Id = x.Id,
                        Course_Name = x.CourseName,
                        Unit_Price = x.CourseFee,
                        MaterialFee = x.BookFee,
                        HalfBook = "N",
                        Level = "XXXX",
                        Class_Id = 0,
                        AttendWeek = x.TotalWeek,
                        Discount_Code = "N.A.",
                        Discount_Rate = 0,
                        PaymentMethod = "",
                        TotalAmount = 0,
                        Remark = "",
                        PayStatus = "P000"  // not paid
                    };

                    var lastenrol = classenrols.Where(y => y.Course_Id == orderlist.Course_Id).ToList();

                    if (lastenrol.Count > 0)
                    {
                        orderlist.Id = lastenrol[0].Id;
                        orderlist.Class_Id = lastenrol[0].Class_Id;
                        orderlist.AttendWeek = lastenrol[0].AttendWeek;
                        orderlist.HalfBook = lastenrol[0].HalfBook;
                        orderlist.Discount_Code = lastenrol[0].Discount_Code;
                        orderlist.Discount_Rate = lastenrol[0].Discount_Rate;
                        orderlist.Level = lastenrol[0].Level;
                        orderlist.PaymentMethod = lastenrol[0].PayMethod;
                        orderlist.Remark = lastenrol[0].Remarks;
                        orderlist.PayStatus = lastenrol[0].PayStatus;
                        // ( 코스금액 * 할인(EB, SB1...) + Material ) * CR surcharge
                        orderlist.TotalAmount = lastenrol[0].TotalAmount;   // Credit Surcharge
                    }


                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = courses.ToList().Count
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ClassEnrolInsert([Bind(Exclude = "Id")] ClassEnrolModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //var branchcode = model.BranchCode;

            //if (branchcode == null || branchcode == "")
            //{
            //    branchcode = _workContext.CurrentCustomer.AdminComment;
            //}

            //Current Enrol term
            var enrolterm = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y").ToList();
            var paymethodlist = _codeMasterService.GetAllCodeMasters("CL10", "Y");

            if (enrolterm.Count != 1)
            {
                return Json(new DataSourceResult() { Errors = "There is no calendar for enrolment currently" });
            }

            if (!String.IsNullOrEmpty(model.Discount_Code))
            {
                model.Discount_Rate = Convert.ToDecimal(_codeMasterService.GetAllCodeMasters("CL08", "Y").Where(x => x.DetailCode == model.Discount_Code).ToList()[0].Val_01);
            }


            if (model.Remark == null) model.Remark = "";
            var paymethod = Convert.ToDecimal(paymethodlist.Where(x => x.DetailCode == model.PaymentMethod).ToList()[0].String_01);

            // ( 코스금액 * 할인(EB, SB1...) + Material ) * CR surcharge
            var TotalAmount = Convert.ToDecimal((((model.Unit_Price * Convert.ToDecimal(model.AttendWeek))     // 코스합계금액
                * (Convert.ToDecimal(100.0) - (model.Discount_Rate)) / Convert.ToDecimal(100.0)   // 할인
                + (model.MaterialFee * Convert.ToDecimal(model.HalfBook == "Y" ? 0.5 : 1.0)))    // Material
                * (Convert.ToDecimal(1) + paymethod / Convert.ToDecimal(100))).ToString("#####.00"));   // Credit Surcharge


            var classenrol = new ClassEnrol(model.Id, enrolterm[0].Year, enrolterm[0].Term, model.Class_Id, model.Stud_Id, model.Course_Id, model.Unit_Price, model.AttendWeek
                , model.MaterialFee, model.HalfBook, model.Level, model.Discount_Code, model.Discount_Rate, model.PaymentMethod, TotalAmount, "N", "N", model.Remark
                , model.PayStatus, DateTime.Now, _workContext.CurrentCustomer.Username);
            //var classteacher = new ClassTeacher(model.Id, model.Class_Id, model.Teacher_Id, model.Remark, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassEnrol(classenrol);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassEnrolUpdate(ClassEnrolModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var enrolterm = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y").ToList();
            var paymethodlist = _codeMasterService.GetAllCodeMasters("CL10", "Y");
            if (enrolterm.Count != 1)
            {
                return Json(new DataSourceResult() { Errors = "There is no calendar for enrolment currently" });
            }

            if (!String.IsNullOrEmpty(model.Discount_Code))
            {
                model.Discount_Rate = Convert.ToDecimal( _codeMasterService.GetAllCodeMasters("CL08", "Y").Where(x => x.DetailCode == model.Discount_Code).ToList()[0].Val_01 );
            }


            var classenrol = _classService.GetAllClassEnrols(0).Where(x => x.Stud_Id == model.Stud_Id && x.Course_Id == model.Course_Id).ToList();
            if (model.Remark == null) model.Remark = "";

            // 기존데이터가 없으면 입력
            if (classenrol.Count == 0)
            {
                if (model.Class_Id != null && model.Class_Id != 0)
                {
                    var paymethod = Convert.ToDecimal( paymethodlist.Where(x => x.DetailCode == model.PaymentMethod).ToList()[0].String_01 );

                    var TotalAmount = Convert.ToDecimal((((model.Unit_Price * Convert.ToDecimal(model.AttendWeek))     // 코스합계금액
    * (Convert.ToDecimal(100.0) - (model.Discount_Rate)) / Convert.ToDecimal(100.0)   // 할인
    + (model.MaterialFee * Convert.ToDecimal(model.HalfBook == "Y" ? 0.5 : 1.0)))    // Material
    * (Convert.ToDecimal(1) + paymethod / Convert.ToDecimal(100))).ToString("#####.00"));   // Credit Surcharge

                    var newclassenrol = new ClassEnrol(model.Id, enrolterm[0].Year, enrolterm[0].Term, model.Class_Id, model.Stud_Id, model.Course_Id, model.Unit_Price, model.AttendWeek
                                    , model.MaterialFee, model.HalfBook, model.Level, model.Discount_Code, model.Discount_Rate, model.PaymentMethod, TotalAmount, "N", "N", model.Remark
                                    , model.PayStatus, DateTime.Now, _workContext.CurrentCustomer.Username);

                    _classService.InsertClassEnrol(newclassenrol);


                    if (newclassenrol.PayStatus == "P020")  // 완납일 경우 자동으로 지불정보를 넣어준다.
                    {
                        var newenrolpay = new ClassEnrol_Pay(0, newclassenrol.Id, 1, DateTime.Now, newclassenrol.TotalAmount, "", DateTime.Now, _workContext.CurrentCustomer.Username);
                        _classService.InsertClassEnrol_Pay(newenrolpay);
                    }


                }

            }
            else  // 있으면 수정/삭제 
            {

                if (model.Class_Id != null && model.Class_Id != 0)  //수정
                {
                    var paymethod = Convert.ToDecimal(paymethodlist.Where(x => x.DetailCode == model.PaymentMethod).ToList()[0].String_01);

                    var TotalAmount = Convert.ToDecimal((((model.Unit_Price * Convert.ToDecimal(model.AttendWeek))     // 코스합계금액
    * (Convert.ToDecimal(100.0) - (model.Discount_Rate)) / Convert.ToDecimal(100.0)   // 할인
    + (model.MaterialFee * Convert.ToDecimal(model.HalfBook == "Y" ? 0.5 : 1.0)))    // Material
    * (Convert.ToDecimal(1) + paymethod / Convert.ToDecimal(100))).ToString("#####.00"));   // Credit Surcharge

                    var updateenrol = classenrol[0];
                    updateenrol.Class_Id = model.Class_Id;
                    updateenrol.AttendWeek = model.AttendWeek;
                    updateenrol.HalfBook = model.HalfBook;
                    updateenrol.Level = model.Level;
                    updateenrol.Discount_Code = model.Discount_Code;
                    updateenrol.Discount_Rate = model.Discount_Rate;
                    updateenrol.PayMethod = model.PaymentMethod;
                    updateenrol.TotalAmount = TotalAmount;
                    updateenrol.Remarks = model.Remark;
                    updateenrol.PayStatus = model.PayStatus;
                    updateenrol.reg_date = DateTime.Now;
                    updateenrol.reg_source = _workContext.CurrentCustomer.Username;

                    _classService.UpdateClassEnrol(updateenrol);
                }
                else
                {   // 삭제


                    // 기존페이먼트 정보가 있으면 같이 삭제



                    var delenrol = classenrol[0];
                    _classService.DeleteClassEnrol(delenrol);
                }
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassEnrolDelete(ClassEnrolModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classenrol = _classService.GetClassEnrolById(model.Id);
            if (classenrol == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no classenrol with the specified code" });

            _classService.DeleteClassEnrol(classenrol);

            return new NullJsonResult();
        }


        #endregion


        #region ClassEnrolPay

        public ActionResult ClassEnrolPay(int enrol_Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassEnrolPayModel();

            var enrol = _classService.GetClassEnrolById(enrol_Id);
            var course = _coursemasterService.GetCourseMasterById(enrol.Course_Id);



            if (enrol == null)
            {
                return Content("No enrolment could be loaded with the specified Code");
            }

            var student = _classService.GetAllMembers("", "", "", "", enrol.Stud_Id, "").ToList();

            if ( student.Count != 1)
            {
                return Content("No student could be loaded with the specified Code");
            }

            var payment = _classService.GetAllClassEnrol_Pays(enrol_Id);

            model.Stud_Id = student[0].stud_id;
            model.Stud_Name = student[0].stud_first_name + ", " + student[0].stud_last_name; 
            model.Grade = "YEAR " + Int32.Parse(student[0].grade).ToString("00").Replace("00", "K");
            model.Id_Enrol = enrol_Id;
            model.CourseName = course.CourseName.Replace("OLY ", "").Replace("ADV ", "").Replace("ELITE", "");
            model.TotalAmount = enrol.TotalAmount;


            return View(model);
        }

        [HttpPost]
        public ActionResult GetClassEnrolPayList(DataSourceRequest command,
            ClassEnrolPayModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var enrolpay = _classService.GetAllClassEnrol_Pays(model.Id_Enrol);



            var gridModel = new DataSourceResult
            {
                Data = enrolpay.Select(x =>
                {
                    var paylist = new ClassEnrolPayModel()
                    {
                        Id = x.Id,
                        Id_Enrol = x.Id_Enrol,
                        Seq = x.Seq,
                        PayDate = x.PayDate,
                        PayAmount = x.PayAmount,
                        Remark = x.Remarks,
                        
                    };


                    return paylist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = enrolpay.ToList().Count
            };

            return Json(gridModel);

        }


        public void CheckSumAmount(int Id_Enrol)
        {
            var enrol = _classService.GetClassEnrolById(Id_Enrol);
            var pays = _classService.GetAllClassEnrol_Pays(Id_Enrol);

            var sumpay = pays.Sum(x => x.PayAmount);


            if ( sumpay == 0 )  //미납
            {
                enrol.PayStatus = "P000";
            }
            else if ( sumpay == enrol.TotalAmount)   // 완납
            {
                enrol.PayStatus = "P020";
            }
            else    // 부분납
            {
                enrol.PayStatus = "P010";
            }

            _classService.UpdateClassEnrol(enrol);
        }

        [HttpPost]
        public ActionResult ClassEnrolPayInsert([Bind(Exclude = "Id")] ClassEnrolPayModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (model.Remark == null) model.Remark = "";

            var seqs = _classService.GetAllClassEnrol_Pays(model.Id_Enrol).OrderBy(x => x.Seq).ToList();
            var seq = 0;

            if (seqs.Count <= 0) seq = 0;
            else seq = seqs[seqs.Count - 1].Seq;
            
            seq++;

            var enrolpay = new ClassEnrol_Pay(model.Id, model.Id_Enrol, seq, model.PayDate, model.PayAmount, model.Remark, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassEnrol_Pay(enrolpay);

            CheckSumAmount(model.Id_Enrol);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassEnrolPayUpdate(ClassEnrolPayModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var enrolpay = _classService.GetClassEnrol_PayById(model.Id);

            if ( enrolpay == null)
            {
                return Json(new DataSourceResult() { Errors = "There is no enrolpayment with the specified code" });
            }

            if (model.Remark == null) model.Remark = "";

            enrolpay.PayAmount = model.PayAmount;
            enrolpay.PayDate = model.PayDate;
            enrolpay.Remarks = model.Remark;
            enrolpay.reg_date = DateTime.Now;
            enrolpay.reg_source = _workContext.CurrentCustomer.Username;

            _classService.UpdateClassEnrol_Pay(enrolpay);

            CheckSumAmount(model.Id_Enrol);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassEnrolPayDelete(ClassEnrolPayModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var enrolpay = _classService.GetClassEnrol_PayById(model.Id);

            if (enrolpay == null)
            {
                return Json(new DataSourceResult() { Errors = "There is no enrolpayment with the specified code" });
            }

            _classService.DeleteClassEnrol_Pay(enrolpay);

            CheckSumAmount(model.Id_Enrol);

            return new NullJsonResult();
        }

        public ActionResult ClassEnrolPay_Receipt(int pay_Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var payment = _classService.GetClassEnrol_PayById(pay_Id);
            var enrol = _classService.GetClassEnrolById(payment.Id_Enrol);


            string filename = "Receipt_" + enrol.Stud_Id + "_" + payment.Seq.ToString() + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_Class.PrintReceiptToPdf(stream, pay_Id);
                bytes = stream.ToArray();
            }



            return File(bytes, "application/pdf", filename);
        }


        #endregion


        #region student
        public ActionResult ClassStudent()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new StudentModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if ( String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                model.is_Admin = true;
            }

            SetAvailableStudentCombo(model);

            return View(model);
        }




        private void SetAvailableStudentCombo(StudentModel model)
        {

            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }


            // Grade combo
            model.AvailableGrade.Clear();
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            //var calendarmasters = _calendarMasterService.GetAllCalendarMasters();

            var codemasters = _codeMasterService.GetAllCodeMasters("CM01", "Y");

            foreach (var x in codemasters)
            {
                 model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
            }

        }


        [HttpPost]
        public ActionResult GetClassStudentList(DataSourceRequest command,
            StudentModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var members = _classService.GetAllMembers(model.Branch, model.Grade, model.FirstName, model.LastName, model.Stud_Id, model.ID_Number);

            var gridModel = new DataSourceResult
            {
                Data = members.Select(x =>
                {
                    var orderlist = new StudentModel()
                    {
                        Id = x.Id,
                        Stud_Id = x.stud_id,
                        FirstName = x.stud_first_name,
                        LastName = x.stud_last_name,
                        Grade = x.grade != "" ? "YR" + (("00" + x.grade).Substring(("00" + x.grade).Length-2,2)) : ""  ,
                        BirthDay = x.stud_birth_day + "/" + x.stud_birth_month + "/" + x.stud_birth_year,
                        SchoolName = x.stud_school_name,
                        Branch = String.IsNullOrEmpty( x.branch ) ? "" : x.branch.Trim(),
                        ID_Number = x.id_number
                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = members.TotalCount
            };

            return Json(gridModel);

        }

        #endregion


        #region classschedule

        public ActionResult ClassSchedule()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassScheduleModel();
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClassScheduleCombo(model);

            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;

            //model.ClassTeacherPopup = new TeacherModel();

            return View(model);
        }




        private void SetAvailableClassScheduleCombo(ClassScheduleModel model)
        {
            //model.AvailableBranchs.Clear();
            model.AvailableClassRoom.Clear();
            model.AvailableTeacher.Clear();
            
            //model.AvailableBranchs.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableClassRoom.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });
            model.AvailableTeacher.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });

            
            var codemasters = _codeMasterService.GetAllCodeMasters();

            var Rooms = this._classService.GetAllClassRooms(model.Branch, "");

            foreach (var room in Rooms)
            {
                model.AvailableClassRoom.Add(new SelectListItem() { Value = Convert.ToString(room.Id), Text = room.Title, Selected = (room.Id == model.Classroom_Id) });
            }

            var teacher = this._classService.GetAllTeachers("", "", "", "");

            foreach (var classteacher in teacher)
            {
                var teachersubjects = _classService.GetAllTeacherSubjects(classteacher.Id).ToList();
                if (teachersubjects.Count > 0)
                {
                    classteacher.Subject = teachersubjects[0].Subject;

                    //var subjectstring = "";

                    //foreach(var sub in teachersubjects)
                    //{

                    //}


                    //orderlist.Subject = 
                }
                model.AvailableTeacher.Add(new SelectListItem() { Value = Convert.ToString(classteacher.Id), Text = classteacher.FirstName + classteacher.LastName + " ( " + classteacher.Subject + " ) ", Selected = (classteacher.Id == model.Teacher_Id) });
            }
    
        }

        [HttpPost]
        public ActionResult GetClassScheduleList(DataSourceRequest command,
            ClassScheduleModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedules = _classService.GetAllClassSchedules(model.Class_Id);

            int temp = 1;

            var gridModel = new DataSourceResult
            {
                Data = classschedules.Select(x =>
                {
                    var orderlist = new ClassScheduleModel()
                    {
                        Id = x.Id,
                        SEQ = temp++,
                        Class_Id = x.Class_Id,
                        Class_Date = x.Class_Date,
                        Class_StartTime = x.Class_StartTime,
                        Class_EndTime = x.Class_EndTime,
                        Classroom_Id = x.Classroom_Id,
                        Remarks = (x.Remarks == null ? "" : x.Remarks)

                    };

                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classschedules.TotalCount
            };

            return Json(gridModel);

        }

        public ActionResult CreateClassSchedule()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassScheduleModel();
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClassScheduleCombo(model);
            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateClassSchedule(ClassScheduleModel model, bool continueEditing, int class_id)
        //public ActionResult CreateClassSchedule(int class_id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //string itemcode = _itemService.GenerateItemCode(model.ItemCategory);
           // var model = new ClassScheduleModel();
           
            model.Class_Id = class_id;
             
            var classschedule = new ClassSchedule(
                        model.Class_Id,
                        model.Class_Date,
                        model.Class_StartTime,
                        model.Class_EndTime,
                        model.Classroom_Id,
                        //model.Duration,
                        "N",
                        model.Remarks,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username);

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }


            _classService.InsertClassSchedule(classschedule);

            SetAvailableClassScheduleCombo(model);
            //model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            return continueEditing ? RedirectToAction("ClassScheduleEdit", new { id = model.Id }) : RedirectToAction("ClassList");

        }

        public ActionResult EditClassSchedule(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            // model.classroompopup = new ClassRoomModel();
            var classschedule = _classService.GetClassScheduleById(id);
            if (classschedule == null)
                //No gift card found with the specified id
                return RedirectToAction("ClassList");
            var classinfo = _classService.GetClassInfoById(classschedule.Class_Id);

            var model = new Nop.Admin.Models.Omni_Class.ClassScheduleModel()
            {
                Id = classschedule.Id,
                Class_Id = classschedule.Class_Id,
                Class_Date = classschedule.Class_Date,
                Class_StartTime = classschedule.Class_StartTime,
                Class_EndTime = classschedule.Class_EndTime,
                YN_Close = classschedule.YN_Close,
                Remarks = classschedule.Remarks,
                Classroom_Id = classschedule.Classroom_Id,
                Class_Name = classinfo.Name

            };
            model.Branch = _workContext.CurrentCustomer.AdminComment;
            SetAvailableClassScheduleCombo(model);
            model.is_Admin = false;
            if (_permissionService.Authorize("OmniAdmin"))
                model.is_Admin = true;


            MakeRollcall(classschedule.Id);

            return View(model);
        }

        private void MakeRollcall(int classscheduleid)
        {

            var classschedule = _classService.GetClassScheduleById(classscheduleid);

            var classenrol = _classService.GetAllClassEnrols(classschedule.Class_Id);
            if (classenrol != null && classschedule.YN_Close != "Y" && classschedule.Class_Date <= DateTime.Now.AddDays(7))
            {
                foreach (var stud in classenrol)
                {
                    var exist_stud = _classService.GetAllClassScheduleRollcalls(classschedule.Class_Id, classschedule.Id).Where(x => x.Stud_Id == stud.Stud_Id).ToList();

                    if (exist_stud.Count < 1)
                    {
                        var classschedulerollcall = new ClassScheduleRollcall(
                                  classschedule.Class_Id
                                  , classschedule.Id
                                  , stud.Stud_Id
                                  , "N"
                                  , ""
                                  , 0
                                  , DateTime.Now
                                  , _workContext.CurrentCustomer.Username);
                        _classService.InsertClassScheduleRollcall(classschedulerollcall);
                    }
                }
            }
        }


        private void MakeAllRollcall(int classid)
        {

            var classinfo = _classService.GetClassInfoById(classid);
            var classenrol = _classService.GetAllClassEnrols(classid);

            var classschedulelist = _classService.GetAllClassSchedules(classid);

            foreach (var classschedule in classschedulelist)
            {

                if (classenrol != null && classschedule.YN_Close != "Y" && classschedule.Class_Date <= DateTime.Now.AddDays(7))
                {
                    foreach (var stud in classenrol)
                    {
                        var exist_stud = _classService.GetAllClassScheduleRollcalls(classschedule.Class_Id, classschedule.Id).Where(x => x.Stud_Id == stud.Stud_Id).ToList();

                        if (exist_stud.Count < 1)
                        {
                            var classschedulerollcall = new ClassScheduleRollcall(
                                      classschedule.Class_Id
                                      , classschedule.Id
                                      , stud.Stud_Id
                                      , "N"
                                      , ""
                                      , 0
                                      , DateTime.Now
                                      , _workContext.CurrentCustomer.Username);
                            _classService.InsertClassScheduleRollcall(classschedulerollcall);
                        }
                    }
                }
            }
        }


        

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult EditClassSchedule(ClassScheduleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            //model.classroompopup = new ClassRoomModel();
            //SetAvailableClassRoomCombo(model.classroompopup);

            var classschedule = _classService.GetClassScheduleById(model.Id);
 
            if (ModelState.IsValid)
            {
                classschedule.Class_Id = model.Class_Id;
                classschedule.Class_Date = model.Class_Date;
                classschedule.Class_StartTime = new DateTime(1900,1,1,model.Class_StartTime.Hour,model.Class_StartTime.Minute,model.Class_StartTime.Second);
                classschedule.Class_EndTime = new DateTime(1900,1,1,model.Class_EndTime.Hour,model.Class_EndTime.Minute,model.Class_EndTime.Second);
                classschedule.Remarks = model.Remarks == null ? "" : model.Remarks;
                classschedule.Classroom_Id = model.Classroom_Id;
                classschedule.reg_date = DateTime.Now;
                classschedule.reg_source = _workContext.CurrentCustomer.Username;
                _classService.UpdateClassSchedule(classschedule);


                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    SetAvailableClassScheduleCombo(model);

                    return RedirectToAction("EditClass", classschedule.Id);
                }
                else
                {
                    return RedirectToAction("ClassList");
                }
            }
            //model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteClassSchedule(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            // model.classroompopup = new ClassRoomModel();
            var classschedule = _classService.GetClassScheduleById(id);
            if (classschedule == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no ClassSchedule with the specified code" });


            _classService.DeleteClassSchedule(classschedule);

            return new NullJsonResult();

        }


       
        #endregion

       
        #region ClassScheduleTeacher

        public ActionResult ClassScheduleTeachers()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassScheduleTeacherModel();

            //SetAvailableClassScheduleTeacherCombo(model);

            //model.classscheduleteacherpopup = new ClassTeacherPopupModel();
            //model.ClassTeacherPopup = new TeacherModel();

            return View(model);
        }

       
        private void SetAvailableClassScheduleTeacherCombo(ClassScheduleTeacherModel model)
        {
            //model.AvailableTeacher.Clear();

            //model.AvailableTeacher.Add(new SelectListItem() { Value = "", Selected = true, Text = "Select" });

            //var teacher = this._classService.GetAllTeachers("", "", "", "");

            //foreach (var classteacher in teacher)
            //{
            //    var teachersubjects = _classService.GetAllTeacherSubjects(classteacher.Id).ToList();
            //    if (teachersubjects.Count > 0)
            //    {
            //        classteacher.Subject = teachersubjects[0].Subject;

            //        //var subjectstring = "";

            //        //foreach(var sub in teachersubjects)
            //        //{

            //        //}


            //        //orderlist.Subject = 
            //    }
            //    model.AvailableTeacher.Add(new SelectListItem() { Value = Convert.ToString(classteacher.Id), Text = classteacher.FirstName + classteacher.LastName + " ( " + classteacher.Subject + " ) ", Selected = (classteacher.Id == model.Teacher_Id) });
            //}
       
        }




        [HttpPost]
        public ActionResult GetClassScheduleTeacherList(DataSourceRequest command,
            ClassScheduleTeacherModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classscheduleteachers = _classService.GetAllClassScheduleTeachers(model.Class_D_Id);
            //var teacher = _classService.GetTeacherById(Int32.Parse(model.Teacher_Id1231));//Int32.Parse(model.Teacher_Id1231)); 

            var gridModel = new DataSourceResult
            {
                Data = classscheduleteachers.Select(x =>
                {
                    var classscheduleteacherlist = new ClassScheduleTeacherModel()
                    {
                        Id = x.Id,
                        Class_D_Id = x.Class_D_Id,
                        Teacher_Id = x.Teacher_Id,
                        Duration = x.Duration,
                        Remark = x.Remarks
                        //ClassTeacherPop = Convert.ToString(x.Teacher_Id)

                    };

                    //if (x.Teacher_Id != null)
                    //{
                    //    var teacher = _classService.GetTeacherById(x.Teacher_Id);
                    //    if (teacher.FirstName != null)
                    //    {
                    //        classscheduleteacherlist.FirstName = teacher.FirstName;
                    //        classscheduleteacherlist.LastName = teacher.LastName;
                    //    }
                    //    var teachersubjects = _classService.GetAllTeacherSubjects(x.Teacher_Id).ToList();
                    //    if (teachersubjects.Count > 0)
                    //    {
                    //        classscheduleteacherlist.Subject = teachersubjects[0].Subject;

                    //        //var subjectstring = "";

                    //        //foreach(var sub in teachersubjects)
                    //        //{

                    //        //}


                    //        //orderlist.Subject = 
                    //    }
                    //    //var teacher = _classService.GetTeacherById(x.Teacher_Id); 
                    //}

                        return classscheduleteacherlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classscheduleteachers.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ClassScheduleTeacherInsert([Bind(Exclude = "Id")] ClassScheduleTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            //model = new ClassTeacherPopupModel;

            //var teacher_Id = model.Teacher_Id;

            //if (branchcode == null || branchcode == "")
            //{
            //    branchcode = _workContext.CurrentCustomer.AdminComment;
            //}

            if (model.Remark == null) model.Remark = "";

            var classscheduleteacher = new ClassScheduleTeacher(model.Class_D_Id, model.Teacher_Id, model.Duration, model.Remark, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassScheduleTeacher(classscheduleteacher);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassScheduleTeacherUpdate(ClassScheduleTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classscheduleteacher = _classService.GetClassScheduleTeacherById(model.Id);
            if (classscheduleteacher == null)
                return Json(new DataSourceResult() { Errors = "There is no classscheduleteacher with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            classscheduleteacher.Class_D_Id = model.Class_D_Id;
            classscheduleteacher.Teacher_Id = model.Teacher_Id;
            classscheduleteacher.Duration = model.Duration;
            classscheduleteacher.Remarks = model.Remark;
            classscheduleteacher.reg_date = DateTime.Now;
            classscheduleteacher.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassScheduleTeacher(classscheduleteacher);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassScheduleTeacherDelete(ClassScheduleTeacherModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classscheduleteacher = _classService.GetClassScheduleTeacherById(model.Id);
            if (classscheduleteacher == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no classscheduleteacher with the specified code" });

            _classService.DeleteClassScheduleTeacher(classscheduleteacher);

            return new NullJsonResult();
        }

        #endregion

        
        #region classschedulerollcall

        public ActionResult ClassScheduleRollcall()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassScheduleRollcallModel();

            return View(model);
        }




       
        [HttpPost]
        public ActionResult GetClassScheduleRollcallList(DataSourceRequest command,
            ClassScheduleRollcallModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedule = _classService.GetClassScheduleById(model.Class_D_Id);
            model.Class_Id = classschedule.Class_Id;
            var classschedulerollcalls = _classService.GetAllClassScheduleRollcalls(model.Class_Id, model.Class_D_Id).OrderBy(x => x.Stud_Id);

            var gridModel = new DataSourceResult
            {
                Data = classschedulerollcalls.Select(x =>
                {
                    var classschedulerollcalllist = new ClassScheduleRollcallModel()
                    {
                        Id = x.Id,
                        Class_Id = x.Class_Id,
                        Class_D_Id = x.Class_D_Id,
                        Stud_Id = x.Stud_Id,
                        Stud_No = x.Stud_Id,
                        Stud_Name = x.Stud_Id,
                        Attend = (!String.IsNullOrEmpty(x.Attend) && x.Attend == "Y") ? "1" : "0",
                        Remarks = x.Remarks,
                        Attend2 = ( !String.IsNullOrEmpty(x.Attend) && x.Attend == "Y") ? 1 : 0

                    };

                    var students = _classService.GetAllMembers("", "", "", "", x.Stud_Id, "").ToList();
                    if ( students.Count > 0 )
                    {
                        classschedulerollcalllist.Stud_Name = students[0].stud_first_name + " " + students[0].stud_last_name;
                        classschedulerollcalllist.Stud_No = students[0].id_number;
                    }
                    else
                    {
                        classschedulerollcalllist.Stud_Name = classschedulerollcalllist.Stud_Id;
                        classschedulerollcalllist.Stud_Id = "_MAKEUP";
                        
                    }

                    return classschedulerollcalllist;
                })
                
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classschedulerollcalls.ToList().Count
               
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ClassScheduleRollcallInsert([Bind(Exclude = "Id")] ClassScheduleRollcallModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            var classschedule = _classService.GetClassScheduleById(model.Class_D_Id);

            model.Class_Id = classschedule.Class_Id;
            if ( model.Stud_Id == "_MAKEUP")
            {
                model.Stud_Name = "";
                model.Remarks = "";
            }


            //Current Enrol term
            var classschedulerollcall = new ClassScheduleRollcall(model.Class_Id, model.Class_D_Id, model.Stud_Id, model.Attend, model.Remarks, 0, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertClassScheduleRollcall(classschedulerollcall);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ClassScheduleRollcallUpdate(ClassScheduleRollcallModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedulerollcall = _classService.GetClassScheduleRollcallById(model.Id);
            if (classschedulerollcall == null)
                return Json(new DataSourceResult() { Errors = "There is no classschedulerollcall with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            
            if (model.Stud_Id == "_MAKEUP")
            {
                model.Stud_Id = "[" + model.Stud_Name.Replace("[", "").Replace("]", "") + "]";
            }

            classschedulerollcall.Stud_Id = model.Stud_Id;
            classschedulerollcall.Attend = (String.IsNullOrEmpty(model.Attend) || model.Attend == "0") ? "N" : "Y" ;
            classschedulerollcall.Remarks = model.Remarks;
            classschedulerollcall.reg_date = DateTime.Now;
            classschedulerollcall.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassScheduleRollcall(classschedulerollcall);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassScheduleRollcallDelete(ClassScheduleRollcallModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedulerollcall = _classService.GetClassScheduleRollcallById(model.Id);
            if (classschedulerollcall == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no classschedulerollcall with the specified code" });

            _classService.DeleteClassScheduleRollcall(classschedulerollcall);

            return new NullJsonResult();
        }
       



        #endregion


        #region make schedules


        
        public ActionResult MakeSchedules(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            
            var classinfo = _classService.GetClassInfoById(id);
            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.Year == classinfo.Year).Where(x => x.Term == classinfo.Term).ToList();
            var classschedules = _classService.GetAllClassSchedules(id);
            /* make schedules */
            if (classschedules.Count == 0)
            {
                DateTime csdate = calendar[0].StartDate;



                int termweek = calendar[0].Week;

                string termcheck =  calendar[0].Term.Substring(0,1);
                if (termcheck == "T")
                {
                    if (classinfo.DayofWeek != "0007")
                    {
                        csdate = csdate.AddDays(Int32.Parse(classinfo.DayofWeek));
                    }

                     //term1 -4
                    for (int i = 1; i <= termweek; i++)
                    {
                        var classschedule = new ClassSchedule(
                               id
                               , csdate
                               , classinfo.StartTime
                               , classinfo.EndTime
                               , classinfo.Classroom_Id
                               , "N"
                               , ""
                               , DateTime.Now
                               , _workContext.CurrentCustomer.Username);
                        _classService.InsertClassSchedule(classschedule);

                        csdate = csdate.AddDays(7);

                        if (csdate > calendar[0].EndDate)
                            break;

                    }
                }
                else
                {
                    //holiday
                    for (int i = 1; i <= termweek; i++)
                    {
                        DayOfWeek vweek = csdate.DayOfWeek;
                        if (vweek.Equals(DayOfWeek.Saturday) || vweek.Equals(DayOfWeek.Sunday))
                        { 
                            csdate = csdate.AddDays(1);
                            termweek++;
                        }
                        else
                        {
                            var classschedule = new ClassSchedule(
                                   id
                                   , csdate
                                   , classinfo.StartTime
                                   , classinfo.EndTime
                                   , classinfo.Classroom_Id
                                   , "N"
                                   , ""
                                   , DateTime.Now
                                   , _workContext.CurrentCustomer.Username);
                            _classService.InsertClassSchedule(classschedule);

                            csdate = csdate.AddDays(1);
                        }

                        if (csdate > calendar[0].EndDate)
                            break;

                    }
                }
                //return null;
            }
            /* end make schedules */
            /* make schedule teachers */
            var classteachers = _classService.GetAllClassTeachers(id);
            var schedules = _classService.GetAllClassSchedules(id);
            if (classteachers.Count > 0)
            {
                foreach (var classdteacher in schedules)
                {
                    foreach (var teacher in classteachers)
                    {
                        var scheduleteacher = _classService.GetAllClassScheduleTeachers(classdteacher.Id).Where(x => x.Teacher_Id == teacher.Teacher_Id).ToList();

                        if (scheduleteacher.Count == 0)
                        {
                            var classscheduleteacher = new ClassScheduleTeacher(
                                       classdteacher.Id
                                       , teacher.Teacher_Id
                                       , teacher.Duration
                                       , ""
                                       , DateTime.Now
                                       , _workContext.CurrentCustomer.Username);
                            _classService.InsertClassScheduleTeacher(classscheduleteacher);
                        }
                    }
                }
            }
            /* end make schedule teachers */
            return new NullJsonResult();
            
        }

        #endregion


        #region EnrolmentTab

        [HttpPost]
        public ActionResult GetClassEnrolListByClass(DataSourceRequest command,
            ClassEnrolModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();


            var classenrols = _classService.GetAllClassEnrols(model.Class_Id).OrderBy(x => x.Stud_Id);

            var gridModel = new DataSourceResult
            {
                Data = classenrols.Select(x =>
                {
                    var enrollist = new ClassEnrolModel()
                    {
                        Id = x.Id,
                        Year = x.Year,
                        Term = x.Term,
                        Level = x.Level,
                        Stud_Id = x.Stud_Id,
                        Course_Id = x.Course_Id,
                        Class_Id = x.Class_Id,
                        Chk_FM =String.IsNullOrEmpty(x.Chk_FM) ? "N" : x.Chk_FM,
                        Chk_HD =String.IsNullOrEmpty(x.Chk_HD) ? "N" : x.Chk_HD,
                        Remark =String.IsNullOrEmpty(x.Remarks) ? "" : x.Remarks,
                        PayStatus = x.PayStatus  // not paid
                    };


                    var student = _classService.GetAllMembers("", "", "", "", enrollist.Stud_Id, "").ToList()[0];

                    enrollist.FirstName = student.stud_first_name;
                    enrollist.LastName = student.stud_last_name;
                    enrollist.HomePhone = student.Telephone;
                    enrollist.Mobile = student.MobilePhone;
                    enrollist.Mobile2 = student.ParentMobilePhone;
                    

                    return enrollist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = classenrols.ToList().Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult ClassEnrolUpdateByClass(ClassEnrolModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();


            var classenrol = _classService.GetClassEnrolById(model.Id);
            if (model.Remark == null) model.Remark = "";

            classenrol.Class_Id = model.Class_Id;
            classenrol.Level = model.Level;
            classenrol.Remarks = model.Remark;
            classenrol.PayStatus = model.PayStatus;
            classenrol.Chk_FM = String.IsNullOrEmpty(model.Chk_FM) ? "N" : model.Chk_FM;
            classenrol.Chk_HD = String.IsNullOrEmpty(model.Chk_HD) ? "N" : model.Chk_HD;
            classenrol.reg_date = DateTime.Now;
            classenrol.reg_source = _workContext.CurrentCustomer.Username;

            _classService.UpdateClassEnrol(classenrol);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ClassEnrolDeleteByClass(ClassEnrolModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();


            var classenrol = _classService.GetAllClassEnrols(0).Where(x => x.Stud_Id == model.Stud_Id && x.Course_Id == model.Course_Id).ToList();

            _classService.DeleteClassEnrol(classenrol[0]);

            return new NullJsonResult();
        }


        public ActionResult PdfRollcall(int id_class)
        {
            var classinfo = _classService.GetClassInfoById(id_class);
            string filename = "RollCall_" + classinfo.Name + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_Class.PrintRollcallSheetToPdf(stream, id_class);
                bytes = stream.ToArray();
               
            }

            //Response.AppendHeader("content-disposition", "inline; filename=" + filename);
            return File(bytes, "application/pdf", filename);//new FileStreamResult(stream, "application/pdf"); // 
        }

        public ActionResult PdfEnrolment(int id_class)
        {
            var classinfo = _classService.GetClassInfoById(id_class);
            string filename = "Enrolment_" + classinfo.Name + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_Class.PrintEnrolmentSheetToPdf(stream, id_class);
                bytes = stream.ToArray();
            }

            return File(bytes, "application/pdf", filename);
        }

        public ActionResult PdfNamelist(int id_class)
        {
            var classinfo = _classService.GetClassInfoById(id_class);
            string filename = "NameList_" + classinfo.Name + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_Class.PrintNameSheetToPdf(stream, id_class);
                bytes = stream.ToArray();
            }

            return File(bytes, "application/pdf", filename);
        }


        public ActionResult PdfClassTest(int id_class)
        {
            var classinfo = _classService.GetClassInfoById(id_class);
            string filename = "ClassTest_" + classinfo.Name + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_Class.PrintClassTestSheetToPdf(stream, id_class);
                bytes = stream.ToArray();

            }

            //Response.AppendHeader("content-disposition", "inline; filename=" + filename);
            return File(bytes, "application/pdf", filename);//new FileStreamResult(stream, "application/pdf"); // 
        }


        #endregion


        #region TodayClass

        public ActionResult TodayClassList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassScheduleModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                model.is_Admin = true;
            }

            SetAvailableTodayClassCombo(model);

            return View(model);
        }




        private void SetAvailableTodayClassCombo(ClassScheduleModel model)
        {
            model.Class_Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

            //class room combo
            model.AvailableClassRoom.Clear();
            var classrooms = _classService.GetAllClassRooms("", "");

            foreach (var classroom in classrooms)
            {
                model.AvailableClassRoom.Add(new SelectListItem() { Value = classroom.Id.ToString(), Text = classroom.Title});
            }



        }


        [HttpPost]
        public ActionResult GetTodayClassList(DataSourceRequest command,
            ClassScheduleModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var todayclass = _classService.GetAllClassSchedules(0).Where(x => x.Class_Date == model.Class_Date).OrderBy(x => x.Class_StartTime);
            var curterm = _calendarService.GetAllCalendarMasters().Where(x => x.StartDate <= model.Class_Date && x.EndDate >= model.Class_Date).ToList();
            
            string year = "";
            string term = "";

            if ( curterm.Count > 0 )
            {
                year = curterm[0].Year;
                term = curterm[0].Term;
            }

            var classinfo = _classService.GetAllClassInfos(model.Branch, year, term);

            var finalclass = todayclass.Join(classinfo, x => x.Class_Id, y => y.Id, (x, y)
                => new { x.Id, x.Class_StartTime, x.Class_EndTime, x.Class_Date, y.Name, x.Class_Id, x.Remarks, x.Classroom_Id, x.YN_Close });


            var gridModel = new DataSourceResult
            {
                Data = finalclass.Select(x =>
                {
                    var list = new ClassScheduleModel()
                    {
                        Id = x.Id,
                        Class_Id = x.Class_Id,
                        Class_Name = x.Name,
                        Class_StartTime = x.Class_StartTime,
                        Class_EndTime = x.Class_EndTime,
                        Classroom_Id = x.Classroom_Id,
                        YN_Close = String.IsNullOrEmpty(x.YN_Close) ? "N" : x.YN_Close,
                        Remarks = x.Remarks
                    };

                    var rollcall = _classService.GetAllClassScheduleRollcalls(list.Class_Id, list.Id).ToList();
                    int attend = 0;
                    for (int i = 0; i < rollcall.Count; i++ )
                    {
                        if (rollcall[i].Attend == "Y")
                            attend++;
                    }

                    list.Remarks = attend.ToString() + "/" + rollcall.Count.ToString();


                    return list;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = todayclass.ToList().Count
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult UpdateTodayClass(ClassScheduleModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedule = _classService.GetClassScheduleById(model.Id);
            if (classschedule == null)
                return Json(new DataSourceResult() { Errors = "There is no classschedule with the specified code" });


            classschedule.YN_Close = model.YN_Close;
            classschedule.reg_date = DateTime.Now;
            classschedule.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassSchedule(classschedule);

            return new NullJsonResult();
        }


        #endregion



        #region TodayStudentList

        public ActionResult TodayStudentList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassStudentModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                model.is_Admin = true;
            }

            SetAvailableTodayStudentCombo(model);

            return View(model);
        }




        private void SetAvailableTodayStudentCombo(ClassStudentModel model)
        {
            model.Class_Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

            // Grade combo
            model.AvailableGrade.Clear();
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            //var calendarmasters = _calendarMasterService.GetAllCalendarMasters();

            var codemasters = _codeMasterService.GetAllCodeMasters("CM01", "Y");

            foreach (var x in codemasters)
            {
                model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
            }

            // status

            model.AvailableStatus.Clear();
            model.AvailableStatus.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableStatus.Add(new SelectListItem() { Value = "Y",  Text = "Y" });
            model.AvailableStatus.Add(new SelectListItem() { Value = "N", Text = "N" });
            model.AvailableStatus.Add(new SelectListItem() { Value = "M", Text = "M" });


            // time type

            model.AvailableTimeType.Clear();
            model.AvailableTimeType.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableTimeType.Add(new SelectListItem() { Value = "AM", Text = "AM" });
            model.AvailableTimeType.Add(new SelectListItem() { Value = "PM", Text = "PM" });


            string curYear = DateTime.Now.Year.ToString();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.StartDate <= model.Class_Date && x.EndDate >= model.Class_Date).ToList();
            string curTerm = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

     
            model.AvailableClass.Clear();

            var classlist = _classService.GetAllClassInfos(model.Branch, curYear, curTerm).OrderBy(x => x.Grade);
            var class_d_list = _classService.GetAllClassSchedules(0);

            var classlist2 = classlist.Join(class_d_list, x => x.Id, y => y.Class_Id, (x, y) => new { y.Id, Class_Id = x.Id, x.Name, y.Class_Date, x.Grade })
                .OrderBy(x => x.Grade).ThenBy(x => x.Class_Date);


            foreach (var classinfo in classlist2)
            {
                model.AvailableClass.Add(new SelectListItem() { Value = classinfo.Grade + "|" + classinfo.Class_Id.ToString() + "|" + classinfo.Id.ToString(), Text = classinfo.Class_Date.ToString("dd/MM)") + classinfo.Name });
            }

        }


        [HttpPost]
        public ActionResult GetTodayStudentList(DataSourceRequest command,
            ClassStudentModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var todayclass = _classService.GetAllClassSchedules(0).Where(x => x.Class_Date == model.Class_Date).OrderBy(x => x.Class_StartTime);
            var rolcalllist = _classService.GetAllClassScheduleRollcalls(0, 0);
            

            string year = "";
            string term = "";

            var curterm = _calendarService.GetAllCalendarMasters().Where(x => x.StartDate <= model.Class_Date && x.EndDate >= model.Class_Date).ToList();
            if (curterm.Count > 0)
            {
                year = curterm[0].Year;
                term = curterm[0].Term;
            }

            var classinfo = _classService.GetAllClassInfos(model.Branch, year, term);

            var finallist = todayclass.Join(classinfo, x => x.Class_Id, y => y.Id, (x, y)
                => new { x.Id, y.Name, x.Class_Id, x.Classroom_Id, y.Branch, y.Grade, x.Class_StartTime })
                .Join(rolcalllist, x => x.Id, y => y.Class_D_Id, (x, y) => new {y.Id, Class_d_Id = x.Id, x.Class_Id, x.Name, Status = y.Attend, x.Branch, x.Grade, y.Stud_Id, y.Remarks, x.Class_StartTime}) ;

            if ( !String.IsNullOrEmpty(model.Grade) )
            {
                finallist = finallist.Where(x => x.Grade == model.Grade);
            }

            var pm = new DateTime(1900, 1, 1, 12, 0, 0);

            if (model.TimeType == "AM")
            {
                finallist = finallist.Where(x => x.Class_StartTime < pm);
            }
            else if (model.TimeType == "PM")
            {
                finallist = finallist.Where(x => x.Class_StartTime >= pm);
            }


            if ( model.Status == "Y")
            {
                finallist = finallist.Where(x => x.Status == "Y");
            }
            else if ( model.Status == "N")
            {
                finallist = finallist.Where(x => String.IsNullOrEmpty(x.Status) || x.Status == "N");
            }
            else if ( model.Status == "M")
            {
                finallist = finallist.Where(x => x.Stud_Id.Contains("["));
            }


            finallist = finallist.OrderBy(x => x.Grade).ThenBy(x => x.Class_StartTime).ThenBy(x => x.Class_Id);


            var gridModel = new DataSourceResult
            {
                Data = finallist.Select(x =>
                {
                    var list = new ClassStudentModel()
                    {
                        Id = x.Id,
                        Branch = x.Branch,
                        Class_Id =  x.Class_Id,
                        Class_Id_Mod = x.Grade + "|" + x.Class_Id.ToString() + "|" + x.Class_d_Id.ToString(),
                        Class_Name = x.Name,
                        Class_D_Id = x.Class_d_Id,
                        Status = x.Status,
                        Remarks = x.Remarks,
                        Grade = x.Grade,
                        HomePhone = "",
                        Mobile1 = "",
                        Mobile2 = "",
                        FirstName = "",
                        LastName = "",
                        Stud_Id = x.Stud_Id
                        
                    };

                    var students =  _classService.GetAllMembers("", "", "", "", x.Stud_Id, "").ToList();

                    if ( students.Count > 0)
                    {
                        list.HomePhone = students[0].Telephone;
                        list.Mobile1 = students[0].MobilePhone;
                        list.Mobile2 = students[0].ParentMobilePhone;
                        list.FirstName = students[0].stud_first_name;
                        list.LastName = students[0].stud_last_name;
                    }
                    else
                    {
                        list.Stud_Id = "_MAKEUP";
                        list.FirstName = x.Stud_Id;
                    }

                    return list;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = finallist.ToList().Count
            };

            return Json(gridModel);

        }

        public ActionResult GetClassListcombo(string branch, string curdate)
        {

            DateTime curdate2 = new DateTime(Int32.Parse(curdate.Substring(0,4)), Int32.Parse(curdate.Substring(5,2)), Int32.Parse(curdate.Substring(8,2)));
            var termdate = _calendarService.GetAllCalendarMasters().Where(x => x.StartDate <= curdate2 && x.EndDate >= curdate2).ToList();

            var classes = _classService.GetAllClassInfos(branch, termdate[0].Year, termdate[0].Term);

            return Json(classes, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateTodayStudent(ClassStudentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classschedulerollcall = _classService.GetClassScheduleRollcallById(model.Id);
            if (classschedulerollcall == null)
                return Json(new DataSourceResult() { Errors = "There is no classschedulerollcall with the specified code" });

            classschedulerollcall.Remarks = model.Remarks;
            classschedulerollcall.reg_date = DateTime.Now;
            classschedulerollcall.reg_source = _workContext.CurrentCustomer.Username;

            int makeup_id = 0;

            if (model.Grade + "|" + model.Class_Id.ToString() + "|" + model.Class_D_Id.ToString() != model.Class_Id_Mod)  // create makeup
            {
                var tempvar  = model.Class_Id_Mod.Split('|');
                var newclass_d_id = 0;

                if (tempvar == null || tempvar.Length < 3)
                {
                    return Json(new DataSourceResult() { Errors = "There is no classschedulerollcall with the specified code" });
                }
                else
                {
                    newclass_d_id = Int32.Parse(tempvar[2]);
                }


                //get schedule 
                var makeupschedule = _classService.GetClassScheduleById(newclass_d_id);

                var makeupclass = _classService.GetClassInfoById(makeupschedule.Class_Id);
                classschedulerollcall.Remarks = "MU " + makeupclass.Name;

                var studentname = "[" + model.FirstName + " " + model.LastName + "]";

                ClassScheduleRollcall makeuprollcall = new ClassScheduleRollcall(makeupschedule.Class_Id, makeupschedule.Id, studentname, "N", "From " + model.Class_Name, model.Id, DateTime.Now, _workContext.CurrentCustomer.Username);
                _classService.InsertClassScheduleRollcall(makeuprollcall);

                makeup_id = makeuprollcall.Id;

            }

            classschedulerollcall.Makeup_Id = makeup_id;

            _classService.UpdateClassScheduleRollcall(classschedulerollcall);

            return new NullJsonResult();
        }

        #endregion


        #region Parents



        public ActionResult Parents()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ParentModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                //    model.is_Admin = true;
            }
            SetAvailableParentCombo(model);

            model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin"))
                model.isadmin = true;

            return View(model);
        }




        private void SetAvailableParentCombo(ParentModel model)
        {
            model.AvailableBranchs.Clear();

           // model.AvailableBranchs.Add(new SelectListItem() { Value = "", Selected = true });



            //var codelist = this._codeMasterService.GetAllCodeMasters("", "Y"); 
            var branches = this._codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranchs.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

        }



        [HttpPost]
        public ActionResult GetParentList(DataSourceRequest command,
            ParentModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var parents = _classService.GetAllParents(model.Branch, model.FirstName, model.LastName);

            var gridModel = new DataSourceResult
            {
                Data = parents.Select(x =>
                {
                    var parentslist = new ParentModel()
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Branch = x.Branch,
                        PhoneNo = x.PhoneNo,
                        MobileNo1 = x.MobileNo1,
                        MobileNo2 = x.MobileNo2,
                        Email = x.Email,
                        Remarks = x.Remarks

                    };
                    return parentslist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = parents.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ParentInsert([Bind(Exclude = "Id")] ParentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (model.Remarks == null) model.Remarks = "";

            var parent = new Parent(model.Id, model.FirstName, model.LastName, model.Branch, model.PhoneNo, model.MobileNo1, model.MobileNo2, model.Email, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertParent(parent);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ParentUpdate(ParentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var parent = _classService.GetParentById(model.Id);
            if (parent == null)
                return Json(new DataSourceResult() { Errors = "There is no parent with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            parent.FirstName = model.FirstName;
            parent.LastName = model.LastName;
            parent.Branch = model.Branch;
            parent.PhoneNo = model.PhoneNo;
            parent.MobileNo1 = model.MobileNo1;
            parent.MobileNo2 = model.MobileNo2;
            parent.Email = model.Email;
            parent.Remarks = model.Remarks;
            parent.reg_date = DateTime.Now;
            parent.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateParent(parent);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ParentDelete(ParentModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var parent = _classService.GetParentById(model.Id);
            if (parent == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no parent with the specified code" });

            _classService.DeleteParent(parent);

            return new NullJsonResult();
        }

        #endregion


        #region SiblingManage

        public ActionResult SiblingManage(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new SiblingManageModel();
            model.Parent_Id = Id;

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                //    model.is_Admin = true;
            }
            SetAvailableSiblingManageCombo(model);
            model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin"))
                model.isadmin = true;

           
            return View(model);
        }

        private void SetAvailableSiblingManageCombo(SiblingManageModel model)
        {
            model.AvailableBranchs.Clear();
            model.AvailableStudents.Clear();
            model.AvailableStudentNames.Clear();

            var branches = this._codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranchs.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }

            var students = this._classService.GetAllMembers(model.Branch, "", "", "", "", "");
            //+ "(" + stud.id_number + ")"
            foreach (var stud in students)
            {
                model.AvailableStudents.Add(new SelectListItem() { Value = Convert.ToString(stud.Id), Text = stud.stud_id , Selected = (stud.stud_id == model.Stud_Id) });
                model.AvailableStudentNames.Add(new SelectListItem() { Value =stud.stud_id, Text = stud.stud_first_name+' '+ stud.stud_last_name, Selected = (stud.stud_id == model.Stud_Id) });
            }

        }




        [HttpPost]
        public ActionResult GetSiblingManageList(DataSourceRequest command,
            SiblingManageModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var siblingmanages = _classService.GetAllSiblingManages(model.Parent_Id);


            var gridModel = new DataSourceResult
            {
                Data = siblingmanages.Select(x =>
                {
                    var siblingmanageslist = new SiblingManageModel()
                    {
                        Id = x.Id,
                        Seq = x.Seq,
                        Parent_Id = x.Parent_Id,
                        Id_Number = x.Id_Number,
                        Stud_Id = x.Stud_Id,
                        Remarks = x.Remarks

                    };

                    var students = _classService.GetAllMembers("", "", "", "", x.Stud_Id, "").ToList();

                    if (students.Count > 0)
                    {
                        siblingmanageslist.FirstName = students[0].stud_first_name + ' ' + students[0].stud_last_name;
                        
                    }
                    return siblingmanageslist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = siblingmanages.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult SiblingManageInsert([Bind(Exclude = "Id")] SiblingManageModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            var member = _classService.GetAllMembers(model.Branch, "", "", "", model.Stud_Id, "").ToList();
            model.Id_Number = member[0].id_number;

            if (model.Remarks == null) model.Remarks = "";

            var sibling = _classService.GetAllSiblingManages(model.Parent_Id);
            int maxseq = 0;
            if (sibling.Count > 0)
            {
                maxseq = sibling.Max(s => s.Seq);
                maxseq++;
            }
            else { maxseq = 1; }
            model.Seq = maxseq;

            var siblingmanage = new SiblingManage(model.Id, model.Parent_Id, model.Seq, model.Stud_Id, model.Id_Number, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertSiblingManage(siblingmanage);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult SiblingManageUpdate(SiblingManageModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var siblingmanage = _classService.GetSiblingManageById(model.Id);
            if (siblingmanage == null)
                return Json(new DataSourceResult() { Errors = "There is no siblingmanage with the specified code" });


            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }
            var member = _classService.GetAllMembers(model.Branch, "", "", "", model.Stud_Id, "").ToList();
            model.Id_Number = member[0].id_number;

            siblingmanage.Parent_Id = model.Parent_Id;
            siblingmanage.Seq = model.Seq;
            siblingmanage.Stud_Id = model.Stud_Id;
            siblingmanage.Id_Number = model.Id_Number;
            siblingmanage.Remarks = model.Remarks;
            siblingmanage.reg_date = DateTime.Now;
            siblingmanage.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateSiblingManage(siblingmanage);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult SiblingManageDelete(SiblingManageModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var siblingmanage = _classService.GetSiblingManageById(model.Id);
            if (siblingmanage == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no siblingmanage with the specified code" });

            _classService.DeleteSiblingManage(siblingmanage);

            return new NullJsonResult();
        }

        #endregion


        #region Additional info

        public ActionResult AdditionalInfo()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new StudentModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                model.is_Admin = true;
            }

            SetAvailableAdditionalInfoCombo(model);

            return View(model);
        }




        private void SetAvailableAdditionalInfoCombo(StudentModel model)
        {

            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranch.Clear();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranch.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }


            // Grade combo
            model.AvailableGrade.Clear();
            model.AvailableGrade.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            //var calendarmasters = _calendarMasterService.GetAllCalendarMasters();

            var codemasters = _codeMasterService.GetAllCodeMasters("CM01", "Y");

            foreach (var x in codemasters)
            {
                model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
            }

        }



        #endregion
        

        #region AdditionInfo

        public ActionResult AddInfo(string Stud_Id, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new AdditionInfoModel();

            var student = _classService.GetAllMembers(branch, "", "", "", Stud_Id, "").ToList();
            if (student.Count != 1)
            {
                return Content("No Student could be loaded with the specified Code");
            }

            var additioninfos = _classService.GetAllAdditionInfos(student[0].stud_id).ToList();

            if (additioninfos.Count > 0)
            {
                model.Id = additioninfos[0].Id;
                model.Stud_Id = additioninfos[0].Stud_Id;
                model.FirstName = student[0].stud_first_name;
                model.LastName = student[0].stud_last_name;
                model.Grade = "YR" + Int32.Parse(student[0].grade).ToString("00");
                model.Actual_Grade = additioninfos[0].Actual_Grade;
                model.Branch = student[0].branch.Trim();
                model.Remarks = additioninfos[0].Remarks;
            }
            else
            {
                model.Stud_Id = student[0].stud_id;
                model.FirstName = student[0].stud_first_name;
                model.LastName = student[0].stud_last_name;
                model.Grade = "YR" + Int32.Parse(student[0].grade).ToString("00");
                model.Branch = student[0].branch.Trim();

                if (model.Remarks == null) model.Remarks = "";
                if (model.Actual_Grade == null) model.Actual_Grade = "";

                var additioninfo = new AdditionInfo(model.Id, model.Stud_Id, model.Actual_Grade, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

                _classService.InsertAdditionInfo(additioninfo);
            }

            var studentbranchs = _classService.GetAllStudentBranchs(student[0].stud_id, "").ToList();

            if (studentbranchs.Count > 0)
            {
                //foreach (var sbranch in studentbranchs)
                //{
                //    //model.Id = sbranch.Id;
                //    model.Stud_Id = sbranch.Stud_Id;
                //    //model.FirstName = student[0].stud_first_name;
                //    //model.LastName = student[0].stud_last_name;
                //    model.Branch = sbranch.Branch; //studentbranchs[0].Branch.Trim();
                //    //model.Remarks = sbranch.Remarks;//studentbranchs[0].Remarks;
                //}
            }
            else
            {
                model.Stud_Id = student[0].stud_id;
                model.FirstName = student[0].stud_first_name;
                model.LastName = student[0].stud_last_name;
                model.Branch = student[0].branch.Trim();

                //if (model.Remarks == null) model.Remarks = "";

                var studentbranch = new StudentBranch(model.Stud_Id, model.Branch, "", DateTime.Now, _workContext.CurrentCustomer.Username);

                _classService.InsertStudentBranch(studentbranch);
            }

            if (_workContext.CurrentCustomer.AdminComment == "")
            {
                model.is_admin = true;
            }


            SetAvailableAddInfoCombo(model);

            return View(model);
        }

        private void SetAvailableAddInfoCombo(AdditionInfoModel model)
        {

            model.AvailableBranchs.Clear();
            model.AvailableGrade.Clear();
            model.AvailableActual_Grade.Clear();

            model.AvailableBranchs.Add(new SelectListItem() { Text = "All Branch", Value = "", Selected = true });
            model.AvailableGrade.Add(new SelectListItem() { Text = "Not Selected", Value = "", Selected = true });
            model.AvailableActual_Grade.Add(new SelectListItem() { Text = "Selected", Value = "", Selected = true });


            var codemasters = _codeMasterService.GetAllCodeMasters();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var branch in branchs)
            {
                model.AvailableBranchs.Add(new SelectListItem() { Value = branch.Campus_Prefix, Text = branch.Campus_Title });
            }



            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Grade == x.DetailCode) });
                        model.AvailableActual_Grade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Grade == x.DetailCode) });
                        break;


                }
            }

        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult AddInfo(AdditionInfoModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            var memberid = _classService.GetAllAdditionInfos(model.Stud_Id).ToList();
            if (memberid.Count != 1)
            {
                return Content("No Addition Member ID could be loaded with the specified Code");
            }
            model.Id = memberid[0].Id;

            var additioninfo = _classService.GetAdditionInfoById(model.Id);

            if (model.Remarks == null) model.Remarks = "";
            //if (ModelState.IsValid)
            //{
            additioninfo.Actual_Grade = model.Actual_Grade;
            additioninfo.Remarks = model.Remarks;
            additioninfo.reg_date = DateTime.Now;
            additioninfo.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateAdditionInfo(additioninfo);


            if (continueEditing)
            {
                //selected tab
                SaveSelectedTabIndex();

                SetAvailableAddInfoCombo(model);

                return RedirectToAction("AddInfo", new { stud_id = model.Stud_Id, branch = model.Branch });
            }
            else
            {
                return RedirectToAction("AdditionalInfo");
            }
            //}

            return View(model);
        }

        [HttpPost]
        public ActionResult GetStudentBranchList(DataSourceRequest command,
            StudentBranchModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var studentbranchs = _classService.GetAllStudentBranchs(model.Stud_Id, "");
            var student = _classService.GetAllMembers(model.Branch, "", "", "", model.Stud_Id, "").ToList();
            var gridModel = new DataSourceResult
            {
                Data = studentbranchs.Select(x =>
                {
                    var studentbranchlist = new StudentBranchModel()
                    {
                        Id = x.Id,
                        Stud_Id = x.Stud_Id,
                        Branch = String.IsNullOrEmpty(x.Branch) ? "" : x.Branch.Trim(),
                        Remarks = x.Remarks
                    };
                    studentbranchlist.FirstName = student[0].stud_first_name;
                    studentbranchlist.LastName = student[0].stud_last_name;

                    return studentbranchlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = studentbranchs.TotalCount
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult StudentBranchInsert([Bind(Exclude = "Id")] StudentBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            if (model.Remarks == null) model.Remarks = "";
            var studentbranch = new StudentBranch(model.Stud_Id, model.Branch, model.Remarks, DateTime.Now, _workContext.CurrentCustomer.Username);

            _classService.InsertStudentBranch(studentbranch);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult StudentBranchUpdate(StudentBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var studentbranch = _classService.GetStudentBranchById(model.Id);
            if (studentbranch == null)
                return Json(new DataSourceResult() { Errors = "There is no studentbranch with the specified code" });


            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }
            if (model.Remarks == null) model.Remarks = "";
            studentbranch.Branch = model.Branch;
            studentbranch.Remarks = model.Remarks;
            studentbranch.reg_date = DateTime.Now;
            studentbranch.reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateStudentBranch(studentbranch);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult StudentBranchDelete(StudentBranchModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var studentbranch = _classService.GetStudentBranchById(model.Id);
            if (studentbranch == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no studentbranch with the specified code" });

            _classService.DeleteStudentBranch(studentbranch);

            return new NullJsonResult();
        }

        #endregion





        #region RollCallList

        public ActionResult RollCallList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var model = new ClassRollCallAccumModel();

            model.Branch = _workContext.CurrentCustomer.AdminComment;

            if (String.IsNullOrEmpty(model.Branch))
            {
                model.Branch = "22";
                model.is_admin = true;
            }

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Enrol == "Y");
            string curTerm = "";
            string curYear = "";
            foreach (var term in calendar)
            {
                curTerm = term.Term;
                curYear = term.Year;
            }

            model.Year = curYear;
            model.Term = curTerm;

            SetAvailableRollcallListCombo(model);

            return View(model);
        }




        private void SetAvailableRollcallListCombo(ClassRollCallAccumModel model)
        {

            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranchList.Clear();
            var branchs = _codeMasterService.GetAllCampus();

            foreach (var campus in branches)
            {
                model.AvailableBranchList.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.Branch) });
            }


            model.AvailableYear.Clear();
            model.AvailableTerm.Clear();
            model.AvailableGrade.Clear();

            model.AvailableTerm.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableYear.Add(new SelectListItem() { Text = "All Year", Value = "", Selected = true });
            model.AvailableGrade.Add(new SelectListItem() { Text = "Not Selected", Value = "", Selected = true });


            var codemasters = _codeMasterService.GetAllCodeMasters();

            model.Grade = "YR05";
            var year5class = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term).Where(x => x.Grade == model.Grade).OrderBy(x => x.Id).ToList();

            if ( year5class != null && year5class.Count > 0)
            {
                model.Class_Id = year5class[0].Id;
            }
            
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
                    case "CM01":
                        model.AvailableGrade.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (model.Grade == x.DetailCode) });
                        break;

                }
            }

            model.AvailableClass.Clear();
            model.AvailableClass.Add(new SelectListItem() { Text = "Not Selected", Value = "0", Selected = true });
            var classlist = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term);

            foreach (var classinfo in classlist)
            {
                model.AvailableClass.Add(new SelectListItem() { Value = classinfo.Id.ToString(), Text = classinfo.Name });
            }


        }


        [HttpPost]
        public ActionResult GetRollcallList(DataSourceRequest command,
            ClassRollCallAccumModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            var classid = model.Class_Id;

            if ( classid == null || classid == 0)
            {
                model.Grade = "YR05";
                var year5class = _classService.GetAllClassInfos(model.Branch, model.Year, model.Term).Where(x => x.Grade == model.Grade).OrderBy(x => x.Id).ToList();

                if (year5class != null && year5class.Count > 0)
                {
                    model.Class_Id = year5class[0].Id;
                }
                else
                {
                    return Json(new DataSourceResult() { Errors = "Please select a class to retreive a view." });
                }
            }

            var classinfo = _classService.GetClassInfoById(model.Class_Id);
            var classschedule = _classService.GetAllClassSchedules(model.Class_Id).OrderBy(x => x.Class_Date).Select((x, i) => new { Id = x.Id, Rank = i++, YN_Close = x.YN_Close });


            MakeAllRollcall(model.Class_Id);


            var rollcalllist_raw = _classService.GetAllClassScheduleRollcalls(model.Class_Id, 0);

            var rollcalllist = rollcalllist_raw.Select(x => 
                {
                    var rollcallitem = new ClassScheduleRollcall(x.Class_Id, x.Class_D_Id, x.Stud_Id, x.Attend, x.Remarks, x.Makeup_Id, x.reg_date, x.reg_source);
                    if ( rollcallitem.Makeup_Id != 0 )
                    {
                        var makeroll = _classService.GetClassScheduleRollcallById(rollcallitem.Makeup_Id);
                        rollcallitem.Remarks = rollcallitem.Remarks.Replace("MU", "").Replace("Year", "") + "(" + makeroll.Attend + ")";
                    }
        
                    return rollcallitem;
                }
                );

            var studentlist = rollcalllist.Join(classschedule, x => x.Class_D_Id, y => y.Id, (x,y)
                => new { x.Class_Id, x.Class_D_Id, x.Stud_Id, y.Rank, Attent = x.Attend.Replace("N", "0").Replace("Y", "1")
                    , U1 = (y.Rank == 0) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U2 = (y.Rank == 1) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U3 = (y.Rank == 2) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U4 = (y.Rank == 3) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U5 = (y.Rank == 4) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U6 = (y.Rank == 5) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U7 = (y.Rank == 6) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U8 = (y.Rank == 7) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U9 = (y.Rank == 8) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                    , U10 = (y.Rank == 9 ) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : ""): "0|"
                    , U11 = (y.Rank == 10 ) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : ""): "0|"
                    , U12 = (y.Rank == 11) ? x.Attend.Replace("N", "0").Replace("Y", "1") + "|" + (x.Makeup_Id != 0 ? x.Remarks : "") : "0|"
                }).ToList()
                .GroupBy(x => new { x.Stud_Id })
                .Select(group => new
                {
                    Stud_Id = group.Key.Stud_Id,
                    U1 = group.Max(a => a.U1),
                    U2 = group.Max(a => a.U2),
                    U3 = group.Max(a => a.U3),
                    U4 = group.Max(a => a.U4),
                    U5 = group.Max(a => a.U5),
                    U6 = group.Max(a => a.U6),
                    U7 = group.Max(a => a.U7),
                    U8 = group.Max(a => a.U8),
                    U9 = group.Max(a => a.U9),
                    U10 = group.Max(a => a.U10),
                    U11 = group.Max(a => a.U11),
                    U12 = group.Max(a => a.U12)
                                                                    
                }).OrderBy(x => x.Stud_Id.Replace("[", "ZZZZZZZZZZZZZZZZzZZZZZZZZ"));






            var gridModel = new DataSourceResult
            {
                Data = studentlist.Select(x =>
                {
                    var list = new ClassRollCallAccumModel()
                    {
                        Id = 0,
                        Branch = model.Branch,
                        Class_Id = model.Class_Id,
                        Stud_Id = x.Stud_Id,
                        Stud_No = "",
                        Stud_Name = "",
                        Attend_U1 = x.U1.Substring(0,1),
                        Attend2_U1 = x.U1.Split('|')[1],
                        Attend_U2 = x.U2.Substring(0, 1),
                        Attend2_U2 = x.U2.Split('|')[1],
                        Attend_U3 = x.U3.Substring(0, 1),
                        Attend2_U3 = x.U3.Split('|')[1],
                        Attend_U4 = x.U4.Substring(0, 1),
                        Attend2_U4 = x.U4.Split('|')[1],
                        Attend_U5 = x.U5.Substring(0, 1),
                        Attend2_U5 = x.U5.Split('|')[1],
                        Attend_U6 = x.U6.Substring(0, 1),
                        Attend2_U6 = x.U6.Split('|')[1],
                        Attend_U7 = x.U7.Substring(0, 1),
                        Attend2_U7 = x.U7.Split('|')[1],
                        Attend_U8 = x.U8.Substring(0, 1),
                        Attend2_U8 = x.U8.Split('|')[1],
                        Attend_U9 = x.U9.Substring(0, 1),
                        Attend2_U9 = x.U9.Split('|')[1],
                        Attend_U10 = x.U10.Substring(0, 1),
                        Attend2_U10 = x.U10.Split('|')[1],
                        Attend_U11 = x.U11.Substring(0, 1),
                        Attend2_U11 = x.U11.Split('|')[1],
                        Attend_U12 = x.U12.Substring(0, 1),
                        Attend2_U12 = x.U12.Split('|')[1],
                    };

                    var students = _classService.GetAllMembers("", "", "", "", x.Stud_Id, "").ToList();

                    if (students.Count > 0)
                    {
                        list.Stud_No = students[0].id_number;
                        list.Stud_Name = students[0].stud_first_name + " " + students[0].stud_last_name;
                    }
                    else
                    {
                        list.Stud_Id = "_MAKEUP";
                        list.Stud_Name = x.Stud_Id;
                    }
                   

                    return list;
                })
                .AsQueryable(),
                Total = studentlist.ToList().Count
            };

            return Json(gridModel);

        }



        [HttpPost]
        public ActionResult UpdateRollcall(ClassRollCallAccumModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();

            if (model.Stud_Id == "_MAKEUP")
            {
                model.Stud_Id = "[" + model.Stud_Name.Replace("[", "").Replace("]", "") + "]";
            }

            var classsschedule = _classService.GetAllClassSchedules(model.Class_Id).OrderBy(x => x.Class_Date).ToList();
            var classrollcall = _classService.GetAllClassScheduleRollcalls(model.Class_Id, 0).Where(x => x.Stud_Id == model.Stud_Id);

            var scheduleitem = classsschedule[0];
            var attend = "0";
            var modattend = "0";
            int i = 0;

            foreach(var schedule in classsschedule)
            {
                attend = classrollcall.Where(x => x.Class_D_Id == schedule.Id).ToList()[0].Attend.Replace("Y", "1").Replace("N", "0");
                scheduleitem = classsschedule[i];
                modattend = "0";


                switch(i)
                {
                    case 0: modattend = model.Attend_U1; break;
                    case 1: modattend = model.Attend_U2; break;
                    case 2: modattend = model.Attend_U3; break;
                    case 3: modattend = model.Attend_U4; break;
                    case 4: modattend = model.Attend_U5; break;
                    case 5: modattend = model.Attend_U6; break;
                    case 6: modattend = model.Attend_U7; break;
                    case 7: modattend = model.Attend_U8; break;
                    case 8: modattend = model.Attend_U9; break;
                    case 9: modattend = model.Attend_U10; break;
                    case 10: modattend = model.Attend_U11; break;
                    case 11: modattend = model.Attend_U12; break;
                }

                if (attend != modattend)
                    break;

                i++;
            }


            var rollcall = classrollcall.Where(x => x.Class_D_Id == scheduleitem.Id).ToList();

            if ( rollcall.Count <= 0)
            {
                return Json(new DataSourceResult() { Errors = "There is no classschedulerollcall with the specified code" });
            }

            rollcall[0].Attend = modattend == "1" ? "Y" : "N";
            rollcall[0].reg_date = DateTime.Now;
            rollcall[0].reg_source = _workContext.CurrentCustomer.Username;
            _classService.UpdateClassScheduleRollcall(rollcall[0]);

            return new NullJsonResult();
        }

        #endregion

    }




}