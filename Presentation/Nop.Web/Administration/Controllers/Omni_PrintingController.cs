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
using Nop.Services.Media;
using Nop.Services.Omni_Backoffice;

using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;

using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core.Domain.Omni_Printing;
using Nop.Services.Omni_Backoffice;
using Nop.Services.Omni_Printing;

using Nop.Admin.Models.Omni_Printing;


namespace Nop.Admin.Controllers
{
    public class Omni_PrintingController : BaseAdminController
    {
        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly IPrintService _printService;
        private readonly IPdfService_Omni _pdfService_omni;
       

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
        private readonly IPictureService _pictureService;

        private readonly IPdfService _pdfService;
        private readonly PdfSettings _pdfSettings;

        #endregion

        #region Constructors

        public Omni_PrintingController(ICodeMasterService codeMasterService, IPrintService printService, IPdfService_Omni pdfService_omni,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService, IPictureService pictureService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IThemeProvider themeProvider, IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext, IPdfService pdfService, PdfSettings pdfSettings)
        {
            this._codeMasterService = codeMasterService;
            this._printService = printService;
            this._pdfService_omni = pdfService_omni;
           
            this._priceFormatter = priceFormatter;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
            this._encryptionService = encryptionService;
            this._pictureService = pictureService;
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


        #region Request

        public ActionResult RequestList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new PrintRequestModel();

            model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.isadmin = true;

            SetAvailableRequestListCombo(model);

            return View(model);
        }

        private void SetAvailableRequestListCombo(PrintRequestModel model)
        {

            model.isadmin = false;
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.isadmin = true;

            if (String.IsNullOrEmpty(model.Status))
                model.StatusVal = 0;
            else
                model.StatusVal = Int32.Parse(model.Status.Substring(2, 2));

            model.AvailableStatus.Clear();
            model.AvailableStatus.Add(new SelectListItem() { Text = "ALL", Value = "", Selected = true });

            model.FromDate = DateTime.Now.AddDays(-30);
            model.ToDate = DateTime.Now;


            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "PR01":
                        model.AvailableStatus.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode });
                        break;
                }
            }

            model.RequestItem.AvailableItemType.Clear();
            model.RequestItem.AvailableItemType.Add(new SelectListItem() { Text = "Select Type", Value = "0", Selected = true });

            var items = _printService.GetAllItems();
            foreach(var item in items)
            {
                model.RequestItem.AvailableItemType.Add(new SelectListItem() { Text = item.ItemName, Value = item.Id.ToString() });
            }




            model.RequestItem.AvailableSize.Clear();
            model.RequestItem.AvailableSize.Add(new SelectListItem() { Text = "Select option", Value = "0", Selected = true });

            model.RequestItem.AvailableGsm.Clear();
            model.RequestItem.AvailableGsm.Add(new SelectListItem() { Text = "Select option", Value = "0", Selected = true });

            model.RequestItem.AvailableColor.Clear();
            model.RequestItem.AvailableColor.Add(new SelectListItem() { Text = "Select option", Value = "0", Selected = true });

            model.RequestItem.AvailableDocStyle.Clear();
            model.RequestItem.AvailableDocStyle.Add(new SelectListItem() { Text = "Select option", Value = "0", Selected = true });



            var specs = _printService.GetAllSpecs();

            foreach (var spec in specs)
            {

                    switch(spec.SpecName.ToUpper())
                    {
                        case "SIZE": model.RequestItem.AvailableSize.Add(new SelectListItem() { Text = spec.Specification, Value = spec.Id.ToString(), Selected = (spec.Id == model.RequestItem.PrintSize) });
                            break;
                        case "GSM": model.RequestItem.AvailableGsm.Add(new SelectListItem() { Text = spec.Specification, Value = spec.Id.ToString(), Selected = (spec.Id == model.RequestItem.PrintGsm) });
                            break;
                        case "COLOR": model.RequestItem.AvailableColor.Add(new SelectListItem() { Text = spec.Specification, Value = spec.Id.ToString(), Selected = (spec.Id == model.RequestItem.PrintColor) });
                            break;
                        case "DOCSTYLE": model.RequestItem.AvailableDocStyle.Add(new SelectListItem() { Text = spec.Specification, Value = spec.Id.ToString(), Selected = (spec.Id == model.RequestItem.PrintDocStyle) });
                            break;
                    }
 
            }
        }

        public ActionResult GetOptionList(int itemtype, string option)
        {
            var options = _printService.GetAllItemSpecs(itemtype);
            var specname = _printService.GetAllSpecs().Where( x => x.SpecName == option);

            var combination = options.Join(specname, x => x.Spec_Id, y => y.Id, (x, y) => new { x.Spec_Id, y.Specification });

            return Json(combination, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRequestList(DataSourceRequest command,
            PrintRequestModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();
            
            string userid = _workContext.CurrentCustomer.Username;
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                userid = "";


            var reqlist = _printService.GetAllRequests(model.FromDate, model.ToDate, model.Status, model.User_Id);



            try
            {
                var gridModel = new DataSourceResult
                {
                    Data = reqlist.Select(x =>
                    {
                        var req = new PrintRequestModel()
                        {
                            Id = x.Id,
                            ReqDate = x.ReqDate,
                            Status = x.Status,
                            ContentText = x.ContentText,
                            Title = x.Title
                        };

                        return req;
                    })
                    .AsQueryable()
                    .Filter(filter)
                    .Sort(sort),
                    Total = reqlist.TotalCount
                };
                return Json(gridModel);

            }
            catch (Exception ex)
            {
                string t = ex.Message;
            }

            return Json(null);
        }



        public ActionResult CreateRequest()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new PrintRequestModel();
            model.ReqDate = DateTime.Now;
            model.DueDate = DateTime.Now.AddDays(7);
            SetAvailableRequestListCombo(model);

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateRequest(PrintRequestModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            model.ReqDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            model.DueDate = new DateTime(model.DueDate.Year, model.DueDate.Month, model.DueDate.Day);

            var request = new PrintRequest(
                model.Title,
                model.ContentText,
                model.ReqDate,
                model.DueDate,
                "ST10",
                0,
                _workContext.CurrentCustomer.Username,
                DateTime.Now,
                _workContext.CurrentCustomer.Username);

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            _printService.InsertRequest(request);

            if (model.RequestItem != null)
            {
                model.RequestItem.Req_Id = request.Id;
                UpdateRequestItem(model.RequestItem);
            }

            SetAvailableRequestListCombo(model);

            return RedirectToAction("EditRequest", new { id = request.Id });

        }

        public ActionResult EditRequest(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();


            // model.classroompopup = new ClassRoomModel();
            var request = _printService.GetRequestById(id);
            if (request == null)
                //No gift card found with the specified id
                return RedirectToAction("RequestList");

            var model = new Nop.Admin.Models.Omni_Printing.PrintRequestModel()
            {

                Id = request.Id,
                Status = request.Status,
                ReqDate = request.ReqDate,
                DueDate = request.DueDate,
                User_Id = request.User_Id,
                Title = request.Title,
                ContentText = request.ContentText,

            };

            var specs = _printService.GetAllSpecs();

            var items = _printService.GetAllRequestItemSpecs( request.Id ).ToList();
            var itemtype = _printService.GetAllRequestItems(request.Id).ToList();

            if ( items.Count > 0)
            {

                model.RequestItem = new Nop.Admin.Models.Omni_Printing.PrintRequestItemModel()
                {
                    Id = itemtype[0].Id,
                    Req_Id = request.Id,
                    Remarks = "",
                    Item_Type = itemtype[0].Item_Type,
                    PictureId = itemtype[0].File_Id
                };


                for( int i = 0; i < items.Count; i++ )
                {
                    var item = items[i];

                    string option = specs.Where(x => x.Id == item.Spec_Id).ToList()[0].SpecName;
                    switch(option.ToUpper())
                    {
                        case "SIZE": model.RequestItem.PrintSize = item.Spec_Id; break;
                        case "GSM": model.RequestItem.PrintGsm = item.Spec_Id; break;
                        case "COLOR": model.RequestItem.PrintColor = item.Spec_Id; break;
                        case "DOCSTYLE": model.RequestItem.PrintDocStyle = item.Spec_Id; break;
                    }
                }
            }

            var quote = _printService.GetAllQuotations(request.Id).ToList();
            if (quote.Count > 0)
            {
                model.Quotation = new Nop.Admin.Models.Omni_Printing.PrintQuotationModel()
                {
                    Id = quote[0].Id,
                    Req_Id = request.Id,
                    Due_Date = quote[0].Due_date,
                    ContentText = quote[0].ContentText,
                    Quot_Amount = quote[0].Quot_Amount,
                    PictureId = quote[0].File_Id
                };
            }


            SetAvailableRequestListCombo(model);

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult EditRequest(PrintRequestModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();


            var request = _printService.GetRequestById(model.Id);
            var statusval = Int32.Parse(request.Status.Substring(2, 2));


            if (ModelState.IsValid)
            {
                if (model.StatusVal < 20)
                {
                    request.Title = model.Title;
                    request.ContentText = model.ContentText;
                    request.DueDate = model.DueDate;

                    _printService.UpdateRequest(request);
                }

                if (model.RequestItem != null && statusval < 20)
                {
                    model.RequestItem.Req_Id = request.Id;
                    UpdateRequestItem(model.RequestItem);
                }

                if (model.Quotation != null && statusval >= 20 && statusval < 30)
                {
                    model.Quotation.Req_Id = request.Id;
                    UpdateQuotation(model.Quotation);
                }


                //selected tab
                SaveSelectedTabIndex();

                SetAvailableRequestListCombo(model);

                return RedirectToAction("EditRequest", request.Id);
            }

            //model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        private void UpdateRequestItem(PrintRequestItemModel model)
        {
            // item save
            var item = _printService.GetRequestItemById(model.Id);
            if (item != null)
            {
                item.Item_Type = model.Item_Type;
                //file later

                _printService.UpdateRequestItem(item);
            }
            else
            {
                item = new PrintRequestItem(model.Req_Id, model.Item_Type, model.Remarks, 0, DateTime.Now, _workContext.CurrentCustomer.Username);
                _printService.InsertRequestItem(item);
            }


            var items = _printService.GetAllRequestItemSpecs(model.Req_Id).ToList();
            var specs = _printService.GetAllSpecs().ToList();
            var itemspec = items.Join(specs, x => x.Spec_Id, y => y.Id, (x, y) => new {x.Id, x.Req_Id, x.Item_Id, x.Spec_Id, y.SpecName });

            //size
            var specsize = itemspec.Where(x => x.SpecName == "SIZE").ToList();
            if (model.PrintSize != null && model.PrintSize > 0)  // value
            {
                if (specsize.Count == 0) // new 
                {
                    var newitem = new PrintRequestItemSpec(model.Req_Id, item.Id, model.PrintSize, DateTime.Now, _workContext.CurrentCustomer.Username);
                    _printService.InsertRequestItemSpec(newitem);
                }
                else{  // exists
                    var updateitemspec = _printService.GetRequestItemSpecById(specsize[0].Id);
                    updateitemspec.Spec_Id = model.PrintSize;
                    _printService.UpdateRequestItemSpec(updateitemspec);
                }
            }
            else{   // no value
                if (specsize.Count != 0) // new 
                {
                    var deleteditemspec = _printService.GetRequestItemSpecById(specsize[0].Id);
                    _printService.DeleteRequestItemSpec(deleteditemspec);
                }
            }

            //gsm
            var specgsm = itemspec.Where(x => x.SpecName == "GSM").ToList();
            if (model.PrintGsm != null && model.PrintGsm > 0)  // value
            {
                if (specgsm.Count == 0) // new 
                {
                    var newitem = new PrintRequestItemSpec(model.Req_Id, item.Id, model.PrintGsm, DateTime.Now, _workContext.CurrentCustomer.Username);
                    _printService.InsertRequestItemSpec(newitem);
                }
                else{  // exists
                    var updateitemspec = _printService.GetRequestItemSpecById(specgsm[0].Id);
                    updateitemspec.Spec_Id = model.PrintGsm;
                    _printService.UpdateRequestItemSpec(updateitemspec);
                }
            }
            else{   // no value
                if (specgsm.Count != 0) // new 
                {
                    var deleteditemspec = _printService.GetRequestItemSpecById(specgsm[0].Id);
                    _printService.DeleteRequestItemSpec(deleteditemspec);
                }
            }

            //color
            var speccolor = itemspec.Where(x => x.SpecName == "COLOR").ToList();
            if (model.PrintColor != null && model.PrintColor > 0)  // value
            {
                if (speccolor.Count == 0) // new 
                {
                    var newitem = new PrintRequestItemSpec(model.Req_Id, item.Id, model.PrintColor, DateTime.Now, _workContext.CurrentCustomer.Username);
                    _printService.InsertRequestItemSpec(newitem);
                }
                else
                {  // exists
                    var updateitemspec = _printService.GetRequestItemSpecById(speccolor[0].Id);
                    updateitemspec.Spec_Id = model.PrintColor;
                    _printService.UpdateRequestItemSpec(updateitemspec);
                }
            }
            else
            {   // no value
                if (speccolor.Count != 0) // new 
                {
                    var deleteditemspec = _printService.GetRequestItemSpecById(speccolor[0].Id);
                    _printService.DeleteRequestItemSpec(deleteditemspec);
                }
            }


            //docstyle
            var specdocstyle = itemspec.Where(x => x.SpecName == "DOCSTYLE").ToList();
            if (model.PrintDocStyle != null && model.PrintDocStyle > 0)  // value
            {
                if (specdocstyle.Count == 0) // new 
                {
                    var newitem = new PrintRequestItemSpec(model.Req_Id, item.Id, model.PrintDocStyle, DateTime.Now, _workContext.CurrentCustomer.Username);
                    _printService.InsertRequestItemSpec(newitem);
                }
                else
                {  // exists
                    var updateitemspec = _printService.GetRequestItemSpecById(specdocstyle[0].Id);
                    updateitemspec.Spec_Id = model.PrintDocStyle;
                    _printService.UpdateRequestItemSpec(updateitemspec);
                }
            }
            else
            {   // no value
                if (specdocstyle.Count != 0) // new 
                {
                    var deleteditemspec = _printService.GetRequestItemSpecById(specdocstyle[0].Id);
                    _printService.DeleteRequestItemSpec(deleteditemspec);
                }
            }

        }

        private void UpdateQuotation(PrintQuotationModel model)
        {
            var quotation = _printService.GetQuotationById(model.Id);

            if ( quotation == null || quotation.Id == 0)  //insert
            {
                quotation = new PrintQuotation(model.Req_Id, model.Due_Date, model.ContentText, model.Quot_Amount, 0, DateTime.Now, _workContext.CurrentCustomer.Username);
                _printService.InsertQuotation(quotation);
            }
            else  // update
            {
                quotation.Due_date = model.Due_Date;
                quotation.ContentText = model.ContentText;
                quotation.Quot_Amount = model.Quot_Amount;

                _printService.UpdateQuotation(quotation);
            }

            //change status 

            var req = _printService.GetRequestById(model.Req_Id);
            req.Status = "ST25";
            _printService.UpdateRequest(req);

        }

        /* can't delete currently
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniClassManager"))
                return AccessDeniedView();
            //1.check class enroll
            var classenrols = _classService.GetAllClassEnrols(id);
            if (classenrols.Count > 0)
            {
                return Json(new DataSourceResult() { Errors = "There is class enrol member." });
            }
            //2.delete class schedule
            var classschedule = _classService.GetAllClassSchedules(id).ToList();
            foreach (var schedule in classschedule)
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
        */






        #endregion


        #region Process Management
        public ActionResult ConfirmRequest( int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var request = _printService.GetRequestById(id);

            if ( request.Status != "ST10")
            {
                return Content("This request can not be confirmed");
            }

            request.Status = "ST20";
            _printService.UpdateRequest(request);

            return RedirectToAction("EditRequest", new { id = id });
        }

        public ActionResult ConfirmQuotation(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var request = _printService.GetRequestById(id);

            if (request.Status != "ST25")
            {
                return Content("This quotation can not be confirmed");
            }

            request.Status = "ST30";
            _printService.UpdateRequest(request);

            return RedirectToAction("EditRequest", new { id = id });
        }


        public ActionResult PrintRequest(int id)
        {
            if (!_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var request = _printService.GetRequestById(id);

            if (request.Status != "ST30")
            {
                return Content("This request can not be printed");
            }

            request.Status = "ST40";
            _printService.UpdateRequest(request);

            return RedirectToAction("EditRequest", new { id = id });
        }


        public ActionResult CompleteRequest(int id)
        {
            if (!_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var request = _printService.GetRequestById(id);

            if (request.Status != "ST40")
            {
                return Content("This request can not be printed");
            }

            request.Status = "ST50";
            _printService.UpdateRequest(request);

            return RedirectToAction("EditRequest", new { id = id });
        }


        public ActionResult CancelRequest(int id)
        {
            if (!_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var request = _printService.GetRequestById(id);


            request.Status = "ST88";
            _printService.UpdateRequest(request);

            return RedirectToAction("EditRequest", new { id = id });
        }


        public ActionResult PdfInvoice(int reqid)
        {
            var req = _printService.GetRequestById(reqid); 
            string filename = "Invoice_" + req.Id.ToString("#####") + "-" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService_omni.PrintingInvoice(stream, reqid);
                bytes = stream.ToArray();
            }

            //Response.AppendHeader("content-disposition", "inline; filename=" + filename);
            return File(bytes, "application/pdf", filename);//new FileStreamResult(stream, "application/pdf"); // 
        }


        #endregion


        #region FileUpload

        public ActionResult PrintPictureAdd(int PictureId, int ItemId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            if (PictureId == 0)
                throw new ArgumentException();

            var requestitem = _printService.GetRequestItemById(ItemId);
            if (requestitem == null)
                throw new ArgumentException("No requestitem found with the specified id");

            requestitem.File_Id = PictureId;

            _printService.UpdateRequestItem(requestitem);
            _pictureService.SetSeoFilename(PictureId, _pictureService.GetPictureSeName("requestitem" + ItemId));

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PrintPictureList(DataSourceRequest command, int requestitemId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var printPictures = _printService.GetAllRequestItems(0).Where(x => x.Id == requestitemId && x.File_Id > 0).ToList();

            var requestitemmodel = printPictures
                .Select(x =>
                {
                    var itemmodel =  new PrintRequestItemModel()
                    {
                        Id = x.Id,
                        Req_Id = x.Req_Id,
                        Item_Type = x.Item_Type,
                        Remarks = "",
                        PrintSize = 0,
                        PrintGsm = 0,
                        PrintColor = 0,
                        PrintDocStyle = 0,
                        PictureId = x.File_Id,
                        PictureUrl = _pictureService.GetPictureUrl(x.File_Id),
                        isadmin = false
                    };

                    return itemmodel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = requestitemmodel,
                Total = requestitemmodel.Count
            };

            return Json(gridModel);
        }


        [HttpPost]
        public ActionResult PrintPictureDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var requestitem = _printService.GetRequestItemById(id);
            if (requestitem == null)
                throw new ArgumentException("No requestitem found with the specified id");

            var pictureid = requestitem.File_Id;
            requestitem.File_Id = 0;
            _printService.UpdateRequestItem(requestitem);

            var picture = _pictureService.GetPictureById(pictureid);
            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }




        public ActionResult QuotationPictureAdd(int PictureId, int quotId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            if (PictureId == 0)
                throw new ArgumentException();

            var quotation = _printService.GetQuotationById(quotId);
            if (quotation == null)
                throw new ArgumentException("No quotation found with the specified id");

            quotation.File_Id = PictureId;

            _printService.UpdateQuotation(quotation);
            _pictureService.SetSeoFilename(PictureId, _pictureService.GetPictureSeName("quotation" + quotId));

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult QuotationPictureList(DataSourceRequest command, int quotationId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var quotPictures = _printService.GetAllQuotations(0).Where(x => x.Id == quotationId && x.File_Id > 0).ToList();

            var quotmodel = quotPictures
                .Select(x =>
                {
                    var qmodel = new PrintQuotationModel()
                    {
                        Id = x.Id,
                        Req_Id = x.Req_Id,
                        PictureId = x.File_Id,
                        PictureUrl = _pictureService.GetPictureUrl(x.File_Id),
                        isadmin = false
                    };

                    return qmodel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = quotmodel,
                Total = quotmodel.Count
            };

            return Json(gridModel);
        }


        [HttpPost]
        public ActionResult QuotationPictureDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var quotation = _printService.GetQuotationById(id);
            if (quotation == null)
                throw new ArgumentException("No quotation found with the specified id");


            var pictureid = quotation.File_Id;
            quotation.File_Id = 0;
            _printService.UpdateQuotation(quotation);

            var picture = _pictureService.GetPictureById(pictureid);
            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }


        #endregion

        #region  Dashboard

        [ChildActionOnly]
        public ActionResult IncompletePrintingList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");

            return PartialView();
        }

        [HttpPost]
        public ActionResult IncompletePrintingList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");

            var requests = _printService.GetAllRequests(new DateTime(1900, 1, 1), DateTime.Now, "", "").Where(x => Int32.Parse(x.Status.Substring(2, 2)) >= 20 && Int32.Parse(x.Status.Substring(2, 2)) < 50);


            var status = _codeMasterService.GetAllCodeMasters("PR01", "Y");


            var model = requests.Join(status, x => x.Status, y => y.DetailCode, (x, y) => new { x.Id, x.ReqDate, x.DueDate, x.User_Id, x.Title, Status = y.DetailName })
                .OrderByDescending(x => x.ReqDate).ThenByDescending(x => x.Id).ToList();

            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }

        #endregion

    }
}