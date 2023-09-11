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

namespace Nop.Admin.Controllers
{
    public class Omni_OrderController : BaseAdminController
    {
        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly IItemCategoryService _itemCategoryService;
        private readonly IItemService _itemService;
        private readonly IItemSetService _itemSetService;
        private readonly IOrderService_Omni _orderService;
        private readonly ICourseMasterService _courseService;
        private readonly ICalendarMasterService _calendarService;
        private readonly IRoyaltyService _royaltyService;

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



        public Omni_OrderController(ICodeMasterService codeMasterService, IItemCategoryService itemCategoryService, IItemService itemService,
            IItemSetService itemSetService, IOrderService_Omni orderService, ICourseMasterService courseService, ICalendarMasterService calendarService,
            IRoyaltyService royaltyService,
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
            this._courseService = courseService;
            this._calendarService = calendarService;
            this._royaltyService = royaltyService;

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

        
        # region Order

        private void SetAvailableOrderCombo(OrderListModel model, bool newmodel)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranchCode.Clear();
            model.AvailableBranchCode.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranchCode.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.BranchCode) });
            }


            // ordertype, orderstatus, term, year combo
            model.AvailableOrderType.Clear();
            model.AvailableOrderYear.Clear();
            model.AvailableOrderTerm.Clear();
            model.AvailableOrderStatus.Clear();
            model.AvailableOrderType.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            model.AvailableOrderYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableOrderTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableOrderStatus.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });


            if (newmodel)
            {
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
            }

            foreach (var x in codelist)
            {
                switch (x.CodeType)
                {
                    case "OR01":
                        model.AvailableOrderType.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.OrderType) });
                        break;
                    case "OR02":
                        model.AvailableOrderStatus.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Status) });
                        break;
                    case "CM02":
                        model.AvailableOrderTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM05":
                        model.AvailableOrderYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                }
            }
        }

        public ActionResult OrderList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new OrderListModel();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.isadmin = true;
            else model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.issuperadmin = true;
            else model.issuperadmin = false;

            model.BranchCode = _workContext.CurrentCustomer.AdminComment;
            SetAvailableOrderCombo(model, true);
            

            return View(model);
        }

        public ActionResult OrderList_r()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new OrderListModel();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.isadmin = true;
            else model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.issuperadmin = true;
            else model.issuperadmin = false;

            model.BranchCode = _workContext.CurrentCustomer.AdminComment;
            SetAvailableOrderCombo(model, true);


            return View("OrderList", model);
        }

        public ActionResult OrderList2(OrderListModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.isadmin = true;
            else model.isadmin = false;

            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.issuperadmin = true;
            else model.isadmin = false;

            model.BranchCode = _workContext.CurrentCustomer.AdminComment;
            SetAvailableOrderCombo(model, false);


            return View("OrderList", model);
        }

        [HttpPost]
        public ActionResult GetOrderList(DataSourceRequest command,
            OrderListModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, model.OrderType, model.Status);



            var gridModel = new DataSourceResult
            {
                Data = orders.Select(x =>
                {
                    var orderlist = new OrderListModel()
                    {
                        Id = x.Id,
                        BranchCode = x.BranchCode,
                        /*Edited by Karen See 22/02/2021 */
                        OrderDate = x.OrderDate,
                        Confirm_Date = x.confirm_date,
                        /*End*/
                        EstimateDate = x.EstimateDate,
                        OrderType = x.OrderType,
                        Term = x.Term != null ? x.Term : "",
                        Year = x.Year != null ? x.Year : "",
                        Week = Convert.ToInt32(x.Week),
                        Status = x.Status,
                        SumPrice = x.SumPrice,
                        print_date = x.print_date,
                        deliver_date = x.deliver_date
                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = orders.TotalCount
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult OrderInsert([Bind(Exclude = "Id")] OrderListModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var branchcode = model.BranchCode;

            if ( branchcode == null || branchcode == "" )
            {
                branchcode = _workContext.CurrentCustomer.AdminComment;
            }

            if ((model.OrderType == "GROP" || model.OrderType == "SUPP" ) && model.Week < 1)
            {
                return Json(new DataSourceResult() { Errors = "The week can not be 0 for term order " });
            }

            var order = new Order_Omni(
            branchcode,
            DateTime.Now,
            null,           
            DateTime.Now.AddDays(7),
            model.OrderType,
            "ST00",
            model.Year,
            model.Term,
            0,
            model.Week,
            "",
            DateTime.Now,
            _workContext.CurrentCustomer.Username);

            _orderService.InsertOrder(order);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult OrderUpdate(OrderListModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "There is no order with the specified code" });
            model.OrderDate = order.OrderDate;

            if ((order.OrderType == "GROP" || order.OrderType == "SUPP") && order.Week < 1)
            {
                return Json(new DataSourceResult() { Errors = "The week can not be 0 for term order " });
            }

            if (order.Status != "ST00" )
            {
                return Json(new DataSourceResult() { Errors = "Order can't be deleted. It's on processing" });
            }

            if (  !ModelState.IsValid ) 
            {

                    return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

                //item = model.ToEntity(item);
                order.BranchCode = model.BranchCode;
                order.OrderType = model.OrderType;
                order.Term = model.Term;
                order.Year = model.Year;
                order.Week = model.Week;
                order.reg_date = DateTime.Now;
                order.reg_source = _workContext.CurrentCustomer.Username;

                _orderService.UpdateOrder(order);


            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult OrderDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no order with the specified code" });

            if ( order.Status != "ST00" )
            {
                return Json(new DataSourceResult() { Errors = "Order can't be deleted. It's on processing" });
            }

            //using (var scope = new System.Transactions.TransactionScope())
            //{

                // 등록된 오더상세정보가 있으면 같이 지운다.
                var order_ds = _orderService.GetAllOrder_Ds(order);
                foreach (var order_d in order_ds)
                {
                    _orderService.DeleteOrder_D(order_d);
                }
                var order_groups = _orderService.GetAllOrderbyItemSets(order);
                foreach (var order_group in order_groups)
                {
                    _orderService.DeleteOrderbyItemSet(order_group);
                }


                _orderService.DeleteOrder(order);

            //    scope.Complete();
            //}

            return new NullJsonResult();
        }


        private void SetAvailableOrderGroupCombo(OrderGroupModel model)
        {
            // ordertype, orderstatus, term, year combo
            model.AvailableSetCategorys.Clear();
            model.AvailableGrades.Clear();
            model.AvailableLevels.Clear();
            model.AvailableTerms.Clear();
            model.AvailableSetCategorys.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableGrades.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableLevels.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTerms.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM04":
                        model.AvailableSetCategorys.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.SetCategory) });
                        break;
                    case "CM03":
                        model.AvailableLevels.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM02":
                        model.AvailableTerms.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01":
                        model.AvailableGrades.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }
        }

        private void SetAvailableOrderItemCombo2(OrderItemModel model)
        {
            // ordertype, orderstatus, term, year combo
            model.AvailableItems.Clear();
            model.AvailableItems.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            model.AvailableItemPrices.Clear();

            
            var items = _itemService.GetAllItems("", "", "", "", "").Where(x => x.UnitPrice > 0 ).OrderBy(x => x.ItemName);
            

            foreach (var x in items)
            {
                string strText = x.ItemName + (!String.IsNullOrEmpty(x.Level) && x.Level != "____" ? "(" + x.Level + ")" : "");
                model.AvailableItems.Add(new SelectListItem() { Text = strText, Value = x.ItemCode, Selected = (x.ItemCode == model.ItemCode) });
                model.AvailableItemPrices.Add(new SelectListItem() { Text = x.UnitPrice.ToString("$#####.00"), Value = x.ItemCode, Selected = (x.ItemCode == model.ItemCode) });
            }
        }


        public ActionResult EditOrder(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(Id);
            if (order == null)
                return Content("No order could be loaded with the specified Code");

            bool editable = false;
            if (order.Status == "ST00" )
                editable = true;

            if (order.OrderType == "GROP" || order.OrderType == "SUPP" || order.OrderType == "PA__")  // Term 아이템셋 주문이면
            {
                var model = new OrderGroupModel();
                model.Order_Id = Id;
                model.is_Editable = editable;
                model.Week = Convert.ToInt32(order.Week);
                SetAvailableOrderGroupCombo(model);
                return View("EditGroupOrder", model);
            }
            else if (order.OrderType == "ITEM")  // 개별 아이템 주문이면
            {
                var model = new OrderItemModel();
                model.Order_Id = Id;
                if (order.Week != null)
                {
                    model.Week = Convert.ToInt32(order.Week);
                }
                model.is_Editable = editable;
                model.Order_Type = "ITEM";
                SetAvailableOrderItemCombo2(model);
                return View("EditItemOrder", model);
            }
            else if (order.OrderType == "TEST")  // 테스트 주문이면
            {
                var model = new OrderGroupModel();
                model.Order_Id = Id;
                model.is_Editable = editable;
                model.Week = Convert.ToInt32(order.Week);
                model.Order_Type = "TEST";
                SetAvailableOrderGroupCombo(model);
                return View("EditGroupOrder", model);
            }
            else
            {
                return Content("No order could be loaded with the specified Code");
            }
        }



        [HttpPost]
        public ActionResult GetOrderGroupList(DataSourceRequest command,
            OrderGroupModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Order_Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "No order could be loaded with the specified Code" });


            if ( order.OrderType == "PA__")
            {
                return GetOrderGroupList_PA(order);
            }
            else
            {
                return GetOrderGroupList_NORMAL(order);
            }

        }

        private ActionResult GetOrderGroupList_NORMAL(Order_Omni order)
        {
            var ordertype = _codeMasterService.GetAllCodeMasters("CM04", "Y");
            var ordertype2 = "NORMAL";

            if (order.OrderType == "GROP") ordertype2 = "NORMAL";
            if (order.OrderType == "TEST") ordertype2 = "TEST";
            if (order.OrderType == "SUPP") ordertype2 = "SUPPL";
            if (order.OrderType == "PA__") ordertype2 = "PA";

            var orders = _orderService.GetAllOrderbyItemSets(order, 0, 1000);
            var itemsets = _itemSetService.GetAllItemSets(order.Term, "", "", 0, 1000)
                .Join(ordertype, x => x.SetCategory, y => y.DetailCode, (x, y) => new { x.Id, x.SetName, x.SetCategory, x.Grade, x.Term, x.Level, y.String_01, x.Remarks })
                .Where(x => x.String_01 == ordertype2 && (String.IsNullOrEmpty(x.Remarks) || x.Remarks.ToUpper() != "INACTIVE"));
            
            var list = itemsets.Select(x =>
                    {
                        var ordergroup = new OrderGroupModel()
                        {
                            Id = x.Id,
                            ItemSet_Id = x.Id,
                            SetName = x.SetName,
                            SetCategory = x.SetCategory,
                            Grade = x.Grade,
                            Grade2 = x.Grade,
                            Term = x.Term,
                            Level = x.Level,
                            Order_Id = order.Id,
                            is_Half = "N",
                            Remarks = "",
                            Week = (order.OrderType == "GROP" || order.OrderType == "SUPP") ? Convert.ToInt32(order.Week) : 0,
                            Qty = 0,
                            Qty_Teacher = 0,
                            Qty_NewBook =0
                        };


                        foreach (var precheck in orders)
                        {
                            if (precheck.ItemSet_Id == x.Id)  // 기존에 있던거면
                            {
                                ordergroup.is_Half = precheck.is_Half;
                                ordergroup.Remarks = precheck.Remarks;
                                ordergroup.Week = (order.OrderType == "GROP" || order.OrderType == "SUPP") ? Convert.ToInt32(order.Week) : precheck.Week;
                                ordergroup.Qty = precheck.Qty;
                                ordergroup.Qty_Teacher = precheck.Qty_Teacher;
                                ordergroup.Qty_NewBook = precheck.Qty_NewBook;
                                ordergroup.Id = precheck.Id;
                                break;

                            }
                        }

                        return ordergroup;
                    })
                    .OrderBy(x => x.Grade)
                    .ThenByDescending(x => x.SetCategory)
                    .ThenBy(x => x.Level)
                    .AsQueryable().ToList();

            if (order.Status != "ST00")
            {
                for(int k = list.Count-1; k >= 0; k-- )
                {
                    if (list[k].Qty == 0 && list[k].Qty_Teacher == 0 && list[k].Qty_NewBook == 0)
                    {
                        list.RemoveAt(k);
                    }
                }
            }

            string grade = "";
            int i = 0;
            foreach (var item in list)
            {
                if (item.Grade == grade)
                {
                    list[i].Grade2 = "   ";
                }

                grade = item.Grade;
                i++;
            }

            try
            {
                var gridModel = new DataSourceResult
                {
                    Data = list,
                    Total = list.Count
                };
                return Json(gridModel);

            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }

        }

        private ActionResult GetOrderGroupList_PA(Order_Omni order)
        {
            var ordertype = _codeMasterService.GetAllCodeMasters("CM04", "Y");
            var ordertype2 = "PA";

            var orders = _orderService.GetAllOrderbyItemSets(order, 0, 1000);


            var list = orders.Select(x =>
            {
                var ordergroup = new OrderGroupModel()
                {
                    ItemSet_Id = x.Id,
                    SetName = "",
                    SetCategory = "",
                    Grade = "",
                    Grade2 = "",
                    Term = "",
                    Level = "",
                    Order_Id = order.Id,
                    is_Half = "N",
                    Remarks = "",
                    Week = (order.OrderType == "GROP" || order.OrderType == "SUPP") ? Convert.ToInt32(order.Week) : 0,
                    Qty = 1,
                    Qty_Teacher = 0,
                    Qty_NewBook = 0
                };

                var paorder = _royaltyService.GetOrder_PASalesById(x.ItemSet_Id);

                ordergroup.SetName = paorder.Title + (String.IsNullOrEmpty(paorder.Level) ? "" : "-" + paorder.Level);

                return ordergroup;
            })
            .OrderBy(x => x.SetName)
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
        public ActionResult OrderGroupUpdate(OrderGroupModel ordergroup)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
                }

                //using (var scope = new System.Transactions.TransactionScope())
                //{

                    var realordergroup = _orderService.GetOrderbyItemSetById(ordergroup.Id);

                    // same order check
                    if (realordergroup != null)
                    {
                        if (realordergroup.Order_Id != ordergroup.Order_Id)
                        {
                            realordergroup = null;
                        }

                    }

                    var order = _orderService.GetOrderById(ordergroup.Order_Id);
                    var course = _courseService.GetAllCourseMasters()
                        .Where(x => x.Year == order.Year)
                        .Where(x => x.Term == order.Term)
                        .Where(x => x.CourseCategory == ordergroup.SetCategory)
                        .Where(x => x.Grade == ordergroup.Grade)
                        .Where(x => x.Level == ordergroup.Level).ToList();

                // comprehensive course 인 경우 레벨을 선택하지 않고 general 코스로 설정한다.
                    if (ordergroup.SetCategory == "WE__" || ordergroup.SetCategory == "WEG_" || ordergroup.SetCategory == "WEMG" || ordergroup.SetCategory == "MG__")
                    {
                        course = _courseService.GetAllCourseMasters()
                        .Where(x => x.Year == order.Year)
                        .Where(x => x.Term == order.Term)
                        .Where(x => x.CourseCategory == ordergroup.SetCategory)
                        .Where(x => x.Grade == ordergroup.Grade)
                        .Where(x => x.Level == "____").ToList();
                    }

                    if (course.Count != 1)
                    {
                        return Json(new DataSourceResult() { Errors = "Course is not assigned" });
                    }


                    if (ordergroup.Qty != 0 || ordergroup.Qty_Teacher != 0 || ordergroup.Qty_NewBook != 0)
                    {
                        if (realordergroup == null) // insert
                        {

                            var ordergroupall = _orderService.GetAllOrderbyItemSets(order);

                            var orderbyitemset = new OrderbyItemSet()
                            {
                                Order_Id = ordergroup.Order_Id,
                                ItemSet_Id = ordergroup.ItemSet_Id,
                                Qty = ordergroup.Qty,
                                Qty_Teacher = ordergroup.Qty_Teacher,
                                Qty_NewBook=ordergroup.Qty_NewBook,
                                Course_Id = course[0].Id,
                                Week = ordergroup.Week, //- course[0].StartWeek + 1,
                                is_Half = ordergroup.is_Half,
                                Remarks = ordergroup.Remarks,
                                reg_date = DateTime.Now,
                                reg_source = _workContext.CurrentCustomer.Username
                            };
                            _orderService.InsertOrderbyItemSet(orderbyitemset);
                            InsertOrder_D(orderbyitemset);

                        }
                        else // update
                        {
                            realordergroup.Qty = ordergroup.Qty;
                            realordergroup.Qty_Teacher = ordergroup.Qty_Teacher;
                            realordergroup.Qty_NewBook = ordergroup.Qty_NewBook;
                            realordergroup.Course_Id = course[0].Id;
                            realordergroup.Week = ordergroup.Week;
                            realordergroup.is_Half = ordergroup.is_Half;
                            realordergroup.Remarks = ordergroup.Remarks;
                            realordergroup.reg_date = DateTime.Now;
                            realordergroup.reg_source = _workContext.CurrentCustomer.Username;
                            _orderService.UpdateOrderbyItemSet(realordergroup);

                            DeleteOrder_D(realordergroup);
                            InsertOrder_D(realordergroup);
                        }
                    }
                    else  // delete
                    {
                        _orderService.DeleteOrderbyItemSet(realordergroup);
                        DeleteOrder_D(realordergroup);
                    }

                    var masterorder = _orderService.GetOrderById(ordergroup.Order_Id);
                    CalculatePrice(masterorder);

                    _orderService.UpdateOrder(masterorder);

                //    scope.Complete();
                //}

            }
            catch (Exception ex)
            {
                return Json(new DataSourceResult() { Errors = ex.Message });
            }
            return new NullJsonResult();
        }


        private void InsertOrder_D(OrderbyItemSet orderitemset)
        {
            // 아이템셋에 해당하는 오더 가져와서 넣기
            var itemset = _itemSetService.GetItemSetById(orderitemset.ItemSet_Id);
            var itemset_ds = _itemSetService.GetAllItemSet_Ds(itemset, 0, 1000);

            var order = _orderService.GetOrderById(orderitemset.Order_Id);
            var order_ds = _orderService.GetAllOrder_Ds(order);

            //var teacherdiscount = _codeMasterService.GetAllCodeMasters("OR03", "Y").Where( x => x.DetailCode == "TCHR" ).ToList();


            int maxseq = 0;
            if (order_ds.Count > 0)
                maxseq = order_ds.Max(s => s.SEQ);

            foreach (var itemset_d in itemset_ds)
            {
                var item = _itemService.GetItemByCode(itemset_d.ItemCode);
                maxseq++;
                var order_d = new Order_D()
                {
                    Order_Id = orderitemset.Order_Id,
                    SEQ = maxseq,
                    ItemCode = itemset_d.ItemCode,
                    ItemSet_Id = itemset_d.Set_Id,
                    ItemSet_Seq = itemset_d.Seq,
                    Qty = orderitemset.Qty,
                    UnitPrice =  0, //  그룹주문인경우 권별 책값은 없다.( orderitemset.is_Half == "Y" ) ? item.UnitPrice_Half : item.UnitPrice,
                    is_Half = orderitemset.is_Half,
                    reg_date = DateTime.Now,
                    reg_source = _workContext.CurrentCustomer.Username
                };

                _orderService.InsertOrder_D(order_d);

                if (  orderitemset.Qty_Teacher > 0) // 선생주문도 있으면 추가한다 
                {
                    maxseq++;
                    var order_d_teacher = new Order_D()
                    {
                        Order_Id = orderitemset.Order_Id,
                        SEQ = maxseq,
                        ItemCode = itemset_d.ItemCode,
                        ItemSet_Id = itemset_d.Set_Id,
                        ItemSet_Seq = itemset_d.Seq,
                        Qty = orderitemset.Qty_Teacher,
                        UnitPrice = 0, // //  그룹주문인경우 권별 책값은 없다.  item.UnitPrice * Convert.ToDecimal( teacherdiscount[0].Val_01 ) / 100,        // 디스카운트적용 
                        is_Half = "N",                     // 선생은 하프북 없음
                        reg_date = DateTime.Now,
                        reg_source = _workContext.CurrentCustomer.Username
                    };
                    _orderService.InsertOrder_D(order_d_teacher);
                }
            }
        }

        private void DeleteOrder_D(OrderbyItemSet orderitemset)
        {
            // 아이템셋에 해당하는 오더 지우기

            var order = _orderService.GetOrderById(orderitemset.Order_Id);
            var order_ds = _orderService.GetAllOrder_Ds(order);


            foreach (var order_d in order_ds)
            {
               if ( order_d.ItemSet_Id == orderitemset.ItemSet_Id)
               {
                   _orderService.DeleteOrder_D(order_d);
               }     
            }
        }

        private void CalculatePrice(Order_Omni order)
        {
            var order_ds = _orderService.GetAllOrder_Ds(order);

            decimal sumprice = 0;
            foreach( var order_d in order_ds)
            {
                sumprice = sumprice + (order_d.UnitPrice * order_d.Qty); 
            }

            order.SumPrice = sumprice;
        }


        private void SetAvailableOrderItemCombo(OrderItemModel model)
        {
            // ordertype, orderstatus, term, year combo
            model.AvailableGrades.Clear();
            model.AvailableLevels.Clear();
            model.AvailableTerms.Clear();
            model.AvailableGrades.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableLevels.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableTerms.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM03":
                        model.AvailableLevels.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM02":
                        model.AvailableTerms.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01":
                        model.AvailableGrades.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }
        }

        public ActionResult ItemList(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(Id);
            if (order == null)
                return Content("No order could be loaded with the specified Code");


            var model = new OrderItemModel();
            model.Order_Id = Id;

            SetAvailableOrderItemCombo(model);
            return View(model);
        }



        [HttpPost]
        public ActionResult GetOrderItemList(DataSourceRequest command,
            OrderGroupModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Order_Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "No order could be loaded with the specified Code" });

            var orderitems = _orderService.GetAllOrder_Ds(order).GroupBy(x => new {x.ItemCode, x.Order_Id, x.ItemSet_Id, x.ItemSet_Seq})
                .Select(group => new {group.Key.ItemCode, group.Key.Order_Id, group.Key.ItemSet_Id, group.Key.ItemSet_Seq, is_Half = group.Max(a => a.is_Half), Qty = group.Sum(a => a.Qty)});
            var orderset = _orderService.GetAllOrderbyItemSets(order);

            try
            {

                var gridModel = new DataSourceResult
                {
                    Data = orderitems.Select(x =>
                    {
                        var ordergroup = new OrderItemModel()
                        {
                            Order_Id = order.Id,
                            ItemCode = x.ItemCode,
                            ItemName = "",
                            is_Half = x.is_Half,
                            Qty = x.Qty,
                            Qty_Half = 0,
                            Qty_Teacher = 0,
                            Qty_NewBook=0,
                            Seq = Convert.ToInt32( (x.ItemSet_Seq == null ? 0 : x.ItemSet_Seq) )
                        };
                        var item = _itemService.GetItemByCode(ordergroup.ItemCode);
                        ordergroup.ItemName = item.ItemName;
                        ordergroup.Term = item.Term;
                        ordergroup.Grade = item.Grade;
                        ordergroup.Level = item.Level;

                        var itemset = orderset.Where(c => c.ItemSet_Id == x.ItemSet_Id);
                        if (itemset.ToList().Count > 0)
                        {
                            ordergroup.Week = itemset.ToList()[0].Week;
                            ordergroup.Qty = itemset.ToList()[0].Qty;
                            ordergroup.Qty_Teacher = itemset.ToList()[0].Qty_Teacher;
                            ordergroup.Qty_NewBook = itemset.ToList()[0].Qty_NewBook;
                        }

                        return ordergroup;
                    })
                    .GroupBy(x => new { x.Order_Id, x.Term, x.ItemCode, x.ItemName, x.Grade, x.Level, x.Week} )
                    .Select(group => new
                    {
                        Order_Id = group.Key.Order_Id,
                        Term = group.Key.Term,
                        ItemCode = group.Key.ItemCode,
                        ItemName = group.Key.ItemName,
                        Grade = group.Key.Grade,
                        Level = group.Key.Level,
                        Week = group.Key.Week,
                        Qty = group.Sum( a => a.Qty ),
                        Qty_Half = group.Sum( a => a.Qty_Half),
                        Qty_Teacher = group.Sum( a => a.Qty_Teacher),
                        Qty_NewBook = group.Sum(a => a.Qty_NewBook)
                    })
                    .OrderBy(x => x.Grade)
                    .ThenBy(x => x.ItemName)
                    .ThenBy(x => x.Level)
                    .AsQueryable()
                    .Filter(filter)
                    .Sort(sort),
                    Total = orderitems.ToList().Count()
                };
                return Json(gridModel);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }


        public ActionResult PdfInvoice(int orderId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            //a vendor does not have access to this functionality
            if (_workContext.CurrentVendor != null)
                return RedirectToAction("Edit", "Order", new { id = orderId });

            var order = _orderService.GetOrderById(orderId);
            var orders = new List<Order_Omni>();
            orders.Add(order);
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintOrdersToPdf(stream, orders, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }
            return File(bytes, "application/pdf", string.Format("order_{0}.pdf", order.Id));
        }


        [HttpPost]
        public ActionResult GetItemOrderList(DataSourceRequest command,
            OrderItemModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Order_Id);
            var order_ds = _orderService.GetAllOrder_Ds(order);

            var gridModel = new DataSourceResult
            {
                Data = order_ds.Select(x =>
                {
                    var orderlist = new OrderItemModel()
                    {
                        Id = x.Id,
                        Order_Id = x.Order_Id,
                        Seq = x.SEQ,
                        ItemCode = x.ItemCode,
                        Qty = x.Qty,
                        Remarks = x.Remarks
                    };
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = order_ds.TotalCount
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult ItemOrderInsert([Bind(Exclude = "Id")] OrderItemModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Order_Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "There is no order with the specified code" });
            var order_ds = _orderService.GetAllOrder_Ds(order);

            int maxseq = 0;
            if (order_ds.Count > 0)
                maxseq = order_ds.Max(s => s.SEQ);

            var item = _itemService.GetItemByCode(model.ItemCode);
            maxseq++;


            var order_d = new Order_D()
            {                
                Order_Id = model.Order_Id,
                SEQ = maxseq,
                ItemCode = model.ItemCode,
                Qty = model.Qty,
                UnitPrice =  item.UnitPrice,
                is_Half = "N",
                Remarks = model.Remarks,
                reg_date = DateTime.Now,
                reg_source = _workContext.CurrentCustomer.Username
            };

            //using (var scope = new System.Transactions.TransactionScope())
            //{

                _orderService.InsertOrder_D(order_d);

            

                CalculatePrice(order);

            //    scope.Complete();
            //}

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ItemOrderUpdate(OrderItemModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var order = _orderService.GetOrderById(model.Order_Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "There is no order with the specified code" });

            var order_d = _orderService.GetOrder_DById(model.Id);
            if (order_d == null)
                return Json(new DataSourceResult() { Errors = "There is no order item with the specified code" });

            var item = _itemService.GetItemByCode(model.ItemCode);

            order_d.ItemCode = model.ItemCode;
            order_d.Qty = model.Qty;
            order_d.UnitPrice = item.UnitPrice;
            order_d.Remarks = item.Remarks;
            order_d.reg_date = DateTime.Now;
            order_d.reg_source = _workContext.CurrentCustomer.Username;

            //using (var scope = new System.Transactions.TransactionScope())
            //{
                _orderService.UpdateOrder_D(order_d);

                CalculatePrice(order);
            //    scope.Complete();
            //}

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult ItemOrderDelete(OrderItemModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();


            var order_d = _orderService.GetOrder_DById(model.Id);
            if (order_d == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no order item with the specified code" });

            var order = _orderService.GetOrderById(order_d.Order_Id);
            if (order == null)
                return Json(new DataSourceResult() { Errors = "There is no order with the specified code" });


            //using (var scope = new System.Transactions.TransactionScope())
            //{
                _orderService.DeleteOrder_D(order_d);

                CalculatePrice(order);
            //    scope.Complete();
            //}

            return new NullJsonResult();
        }

        public ActionResult ProduceOrder(string selectedIds, string returnurl, string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            //a vendor cannot export orders
            var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();

            //using (var scope = new System.Transactions.TransactionScope())
            //{


                foreach (var id in ids)
                {
                    var order = _orderService.GetOrderById(id);


                    var closing1 = _royaltyService.GetClosing(order.Year, order.Term, "ROYL", order.BranchCode);
                    var closing2 = _royaltyService.GetClosing(order.Year, order.Term, "BOOK", order.BranchCode);
                    var closing3 = _royaltyService.GetClosing(order.Year, order.Term, "TEST", order.BranchCode);
                    var closing4 = _royaltyService.GetClosing(order.Year, order.Term, "PA__", order.BranchCode);

                    if (order.OrderType == "GROP" && closing1 != null && closing1.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Term order closed");
                    }

                    if (order.OrderType == "GROP" && closing2 != null && closing2.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Term order closed");
                    }

                    if (order.OrderType == "TEST" && closing3 != null && closing3.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Test order closed");
                    }
                    if (order.OrderType == "PA__" && closing4 != null && closing4.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. PA order closed");
                    }



                    if (order.Status != "ST01")
                    {


                        return OmniContentResult.ReturnMsg("Not proper order to produce");
                    }
                        

                    order.Status = "ST10";  // producing
                    order.print_date = DateTime.Now;


                    //AddStock(order);

                    _orderService.UpdateOrder(order);
                }

            //    scope.Complete();
            //}

                if (String.IsNullOrEmpty(returnurl))
                {
                    var model = new OrderListModel();
                    model.Year = year;
                    model.Term = term;
                    model.BranchCode = branch;

                    return RedirectToAction("OrderList2", model);
                }
                else if (returnurl == "Admin")
                {
                    return RedirectToAction("", "Admin");
                }

                return RedirectToAction(returnurl);
        }

        public ActionResult DeliverOrder(string selectedIds, string returnurl, string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            //a vendor cannot export orders
            var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();

            //using (var scope = new System.Transactions.TransactionScope())
            //{
                foreach (var id in ids)
                {
                    var order = _orderService.GetOrderById(id);

                    var closing1 = _royaltyService.GetClosing(order.Year, order.Term, "ROYL", order.BranchCode);
                    var closing2 = _royaltyService.GetClosing(order.Year, order.Term, "BOOK", order.BranchCode);
                    var closing3 = _royaltyService.GetClosing(order.Year, order.Term, "TEST", order.BranchCode);
                    var closing4 = _royaltyService.GetClosing(order.Year, order.Term, "PA__", order.BranchCode);

                    if (order.OrderType == "GROP" && closing1 != null && closing1.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Term order closed");
                    }

                    if (order.OrderType == "GROP" && closing2 != null && closing2.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Term order closed");
                    }

                    if (order.OrderType == "TEST" && closing3 != null && closing3.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. Test order closed");
                    }
                    if (order.OrderType == "PA__" && closing4 != null && closing4.YN_Closing == "Y")
                    {
                        return OmniContentResult.ReturnMsg("order can't be changed. PA order closed");
                    }

                    /*   일정기간 이전 제한 로직은 일단 넣지 않도록
                    if ( order.OrderType == "GROP" && order.Week != 1)
                    {
                        var cal = _calendarService.GetAllCalendarMasters().Where(x => x.Year == order.Year && x.Term == order.Term).ToList();

                        if ( cal.Count > 0)
                        {
                            if (  DateTime.Now.AddDays(7)  <  cal[0].StartDate.AddDays( (Convert.ToInt32(order.Week)-1) * 7)  )
                            {
                                return OmniContentResult.ReturnMsg("can't deliver more than a week in advance");
                            }
                        }
                    }*/


                    if (order.Status.Substring(0, 3) != "ST1")
                        return Json(new DataSourceResult() { Errors = "Not proper order to deliever" });

                    order.Status = "ST20";  // producing
                    order.deliver_date = DateTime.Now;

                    //SubstractStock(order);
                    _orderService.UpdateOrder(order);
                }
            //    scope.Complete();
            //}

                if (String.IsNullOrEmpty(returnurl))
                {
                    var model = new OrderListModel();
                    model.Year = year;
                    model.Term = term;
                    model.BranchCode = branch;

                    return RedirectToAction("OrderList2", model);
                }
                else if (returnurl == "Admin")
                {
                    return RedirectToAction("", "Admin");
                }

                return RedirectToAction(returnurl);
        }


        public ActionResult OrderOrder(string selectedIds, string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            //a vendor cannot export orders
            var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();

            //using (var scope = new System.Transactions.TransactionScope())
            //{
            foreach (var id in ids)
            {
                var order = _orderService.GetOrderById(id);

                var closing1 = _royaltyService.GetClosing(order.Year, order.Term, "ROYL", order.BranchCode);
                var closing2 = _royaltyService.GetClosing(order.Year, order.Term, "BOOK", order.BranchCode);
                var closing3 = _royaltyService.GetClosing(order.Year, order.Term, "TEST", order.BranchCode);
                var closing4 = _royaltyService.GetClosing(order.Year, order.Term, "PA__", order.BranchCode);

                if (order.OrderType == "GROP" && closing1 != null && closing1.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("Order can't be confirmed. Term order is closed. Please contact franchise manager to confirm your order. ");
                }

                if (order.OrderType == "GROP" && closing2 != null && closing2.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("Order can't be confirmed. Term order is closed. Please contact franchise manager to confirm your order. ");
                }

                if (order.OrderType == "TEST" && closing3 != null && closing3.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("Order can't be confirmed. Term order is closed. Please contact franchise manager to confirm your order. ");
                }

                if (order.OrderType == "PA__" && closing4 != null && closing4.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("Order can't be confirmed. Term order is closed. Please contact franchise manager to confirm your order. ");
                }


                if (order.Status != "ST00")
                    return OmniContentResult.ReturnMsg("Not proper order to confirm");


                order.Status = "ST01";  // ordered
                order.confirm_date = DateTime.Now;
                /*added by Karen See 22/02/2021*/
                order.EstimateDate = DateTime.Now.AddDays(7);
                /*END*/
                _orderService.UpdateOrder(order);

            }
            //    scope.Complete();
            //}

            var model = new OrderListModel();
            model.Year = year;
            model.Term = term;
            model.BranchCode = branch;

            //return RedirectToAction("OrderList2", model);
            return RedirectToAction("OrderList_r", model);
        }


        public ActionResult CancelOrder(string selectedIds, string returnurl, string year, string term, string branch)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            //a vendor cannot export orders
            var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();

            //using (var scope = new System.Transactions.TransactionScope())
            //{


            foreach (var id in ids)
            {
                var order = _orderService.GetOrderById(id);


                var closing1 = _royaltyService.GetClosing(order.Year, order.Term, "ROYL", order.BranchCode);
                var closing2 = _royaltyService.GetClosing(order.Year, order.Term, "BOOK", order.BranchCode);
                var closing3 = _royaltyService.GetClosing(order.Year, order.Term, "TEST", order.BranchCode);
                var closing4 = _royaltyService.GetClosing(order.Year, order.Term, "PA__", order.BranchCode);

                if (order.OrderType == "GROP" && closing1 != null && closing1.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("order can't be canceled. Term order closed");
                }

                if (order.OrderType == "GROP" && closing2 != null && closing2.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("order can't be canceled. Term order closed");
                }

                if (order.OrderType == "TEST" && closing3 != null && closing3.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("order can't be canceled. Test order closed");
                }

                if (order.OrderType == "PA__" && closing4 != null && closing4.YN_Closing == "Y")
                {
                    return OmniContentResult.ReturnMsg("order can't be canceled. PA order closed");
                }


                order.Status = "ST00";  // producing


                //AddStock(order);

                _orderService.UpdateOrder(order);
            }

            //    scope.Complete();
            //}

            if (String.IsNullOrEmpty(returnurl))
            {
                var model = new OrderListModel();
                model.Year = year;
                model.Term = term;
                model.BranchCode = branch;

                return RedirectToAction("OrderList2", model);
            }
            else if (returnurl == "Admin")
            {
                return RedirectToAction("", "Admin");
            }

            return RedirectToAction(returnurl);
        }

        private void AddStock(Order_Omni order)
        {
            var order_ds = _orderService.GetAllOrder_Ds(order);

            foreach ( var order_d in order_ds)
            {
                var item = _itemService.GetItemByCode(order_d.ItemCode);

                if (item.ItemCategory == "BK01" || item.ItemCategory == "BK03")  // 자체제작책 과 PA책은 완료시 입고처리
                {

                    var stockin = new Stock()
                    {
                        ItemCode = order_d.ItemCode,
                        TranDate = DateTime.Now,
                        InQty = order_d.Qty,
                        OutQty = 0,
                        Source_Type = "Order",
                        Source_Id = order_d.Order_Id,
                        Source_Seq = order_d.SEQ,
                        Remarks = "Producing Order No : " + order_d.Order_Id.ToString() + "/ branch : " + order.BranchCode,
                        reg_date = DateTime.Now,
                        reg_source = _workContext.CurrentCustomer.Username
                    };

                    _orderService.InsertStock(stockin);
                }
            }
        }

        private void SubstractStock(Order_Omni order)
        {
            var order_ds = _orderService.GetAllOrder_Ds(order);

            foreach (var order_d in order_ds)   // 배달시 모든책 출고처리
            {
                var stockin = new Stock()
                {
                    ItemCode = order_d.ItemCode,
                    TranDate = DateTime.Now,
                    InQty = 0,
                    OutQty = order_d.Qty,
                    Source_Type = "Order",
                    Source_Id = order_d.Order_Id,
                    Source_Seq = order_d.SEQ,
                    Remarks = "Delievery Order No : " + order_d.Order_Id.ToString() + "/ branch : " + order.BranchCode,
                    reg_date = DateTime.Now,
                    reg_source = _workContext.CurrentCustomer.Username
                };

                _orderService.InsertStock(stockin);
            }
        }


        [HttpPost]
        public ActionResult StockList(int ItemId)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var item = _itemService.GetItemById(ItemId);

            var stocks = _orderService.GetAllStocks(item.ItemCode);

            var remain = 0;

            var gridModel = new DataSourceResult
            {
                Data = stocks.Select(x =>
                {
                    var stocklist = new StockModel()
                    {
                        Id = x.Id,
                        ItemCode = x.ItemCode,
                        TranDate = x.TranDate,
                        InQty = x.InQty,
                        OutQty = x.OutQty,
                        RemainQty = remain + x.InQty - x.OutQty,
                        Remark = x.Remarks
                    };

                    remain = stocklist.RemainQty;
                    return stocklist;
                })
                .AsQueryable(),
                Total = stocks.TotalCount
            };

            return Json(gridModel);

        }




        [HttpPost]
        public ActionResult StockInsert([Bind(Exclude = "Id")] StockModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var stock = new Stock(
                model.ItemCode,
                model.TranDate,
                model.InQty,
                model.OutQty,
                "MANUAL", 0, 0, 
                model.Remark, DateTime.Now, _workContext.CurrentCustomer.Username
                );
            _orderService.InsertStock(stock);

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult StockUpdate(StockModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var stock = _orderService.GetStockById(model.Id);
            if (stock == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no stock record with the specified code" });

            if (stock.Source_Type != "MANUAL")
            {
                return Json(new DataSourceResult() { Errors = "Can't modify unless manual entry." });
            }

            if (!ModelState.IsValid)
            {

                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });

            }

            //item = model.ToEntity(item);

            stock.InQty = model.InQty;
            stock.OutQty = model.OutQty;
            stock.Remarks = model.Remark;
            stock.TranDate = model.TranDate;

            stock.Source_Type = "MANUAL";
            stock.reg_source = _workContext.CurrentCustomer.Username;
            stock.reg_date = DateTime.Now;

            _orderService.UpdateStock(stock);

            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult StockDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var stock = _orderService.GetStockById(id);
            if (stock == null)
                //No gift card found with the specified id
                return Json(new DataSourceResult() { Errors = "There is no stock record with the specified code" });

            if ( stock.Source_Type != "MANUAL")
            {
                return Json(new DataSourceResult() { Errors = "Can't delete unless manual entry." });
            }

            _orderService.DeleteStock(stock);

            return new NullJsonResult();
        }

        [ChildActionOnly]
        public ActionResult OrderIncompleteReport()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");

            return PartialView();
        }

        [HttpPost]
        public ActionResult OrderIncompleteReportList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");




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


                // Order 생성
                
                // pa 인 경우(reg_source = 'system-pa') Order 생성 
                if (pasale.reg_source.ToUpper() == "SYSTEM-PA")
                {
                    var order = new Order_Omni(paorder.BranchCode, paorder.Purchase_Date, null,  paorder.Purchase_Date , paorder.Purchase_Date, "PA__", "ST01", paorder.Year, paorder.Term, paorder.Price, 0, "", DateTime.Now, "system");

                    _orderService.InsertOrder(order);

                    var orderset = new OrderbyItemSet(order.Id, paorder.Id, 1, 1, 0, paorder.Sales_id, 1, "N", "", DateTime.Now, "system");
                    _orderService.InsertOrderbyItemSet(orderset);
                }
            }





            var orders = _orderService.GetAllOrders("", "", "", "", "").Where(x => x.Status == "ST01" || x.Status == "ST10");

            var status = _codeMasterService.GetAllCodeMasters("OR02", "Y");
            var ordertype = _codeMasterService.GetAllCodeMasters("OR01", "");

            /*Edited by Karen See 22/02/2021 */
            //var model = orders.Join(status, x => x.Status, y => y.DetailCode, (x, y) => new { x.Id, x.BranchCode, x.OrderDate, x.EstimateDate,  Status = y.DetailName, x.OrderType, x.Week, x.Term })
            //.Join(campus, x => x.BranchCode, y => y.Campus_Prefix, (x, y) => new { x.Id, Branch = y.Campus_Title, x.OrderDate, x.EstimateDate, x.Status, x.OrderType, x.Week, x.Term })
            //.Join(ordertype, x => x.OrderType, y => y.DetailCode, (x, y) => new { x.Id, x.Branch, x.OrderDate, x.EstimateDate, x.Status, OrderType = y.DetailName + (x.Term.Substring(0,1) == "V" ? "(H)" : ""), Week = x.Week })
            //.OrderByDescending(x => x.OrderDate).ToList();

            var model = orders.Join(status, x => x.Status, y => y.DetailCode, (x, y) => new { x.Id, x.BranchCode, x.OrderDate, x.confirm_date, x.EstimateDate, Status = y.DetailName, x.OrderType, x.Week, x.Term })
                .Join(campus, x => x.BranchCode, y => y.Campus_Prefix, (x, y) => new { x.Id, Branch = y.Campus_Title, x.OrderDate, x.confirm_date, x.EstimateDate, x.Status, x.OrderType, x.Week, x.Term })
                .Join(ordertype, x => x.OrderType, y => y.DetailCode, (x, y) => new { x.Id, x.Branch, x.OrderDate, x.confirm_date, x.EstimateDate, x.Status, OrderType = y.DetailName + (x.Term.Substring(0, 1) == "V" ? "(H)" : ""), Week = x.Week })
                .OrderByDescending(x => x.confirm_date)
                .ThenBy(x => x.EstimateDate).ToList();
            /*End*/

            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }


        [ChildActionOnly]
        public ActionResult ShortageItemsReport()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");

            return PartialView();
        }

        [HttpPost]
        public ActionResult ShortageItemsReportList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return Content("");

            var items = _itemService.GetAllItems("BK02", "", "", "", "");

            var stocks = _orderService.GetAllStocks("")
                .GroupBy(x => new { x.ItemCode }).Select(group => new { group.Key.ItemCode, Qty = group.Sum(c => c.InQty - c.OutQty) });




            var model = items.Join(stocks, x => x.ItemCode, y => y.ItemCode, (x, y) => new { x.ItemCode, x.ItemName, y.Qty })
                .Where(x => x.Qty <= 0).OrderBy(x => x.Qty).ToList();

            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }

        [ChildActionOnly]
        public ActionResult MyorderReport()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return Content("");

            return PartialView();
        }

        [HttpPost]
        public ActionResult MyorderReportList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return Content("");


            var branch = _workContext.CurrentCustomer.AdminComment;

            if ( branch == null || branch == "")
            {
                return Content("");
            }

            var orders = _orderService.GetAllOrders(branch, "", "", "", "", 0, 10);

            var status = _codeMasterService.GetAllCodeMasters("OR02", "Y");
            var ordertype = _codeMasterService.GetAllCodeMasters("OR01", "Y");

            /*Edited by Karen See 22/02/2021 */
            /*var model = orders.Join(status, x => x.Status, y => y.DetailCode, (x, y) => new { x.Id, x.OrderType, x.OrderDate, x.EstimateDate, Status = y.DetailName })
                .Join(ordertype, x => x.OrderType, y => y.DetailCode, (x, y) => new {x.Id, OrderType = y.DetailName, x.OrderDate, x.EstimateDate, x.Status })
                .OrderByDescending(x => x.OrderDate).ToList();*/
            var model = orders.Join(status, x => x.Status, y => y.DetailCode, (x, y) => new { x.Id, x.OrderType, x.OrderDate, x.confirm_date, x.EstimateDate, Status = y.DetailName })
                .Join(ordertype, x => x.OrderType, y => y.DetailCode, (x, y) => new { x.Id, OrderType = y.DetailName, x.OrderDate, x.confirm_date, x.EstimateDate, x.Status })
                .OrderByDescending(x => x.OrderDate)
                .ThenBy(x => x.EstimateDate).ToList();

            var gridModel = new DataSourceResult
            {
                Data = model,
                Total = model.Count
            };

            return Json(gridModel);
        }


        #endregion



        #region printingpaper


        private void SetAvailablePrintingPaperCombo(PrintingPaperModel model)
        {
            var codelist = this._codeMasterService.GetAllCodeMasters("", "Y");
            var branches = this._codeMasterService.GetAllCampus();

            // branch combo
            model.AvailableBranchCode.Clear();
            model.AvailableBranchCode.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });

            foreach (var campus in branches)
            {
                model.AvailableBranchCode.Add(new SelectListItem() { Value = campus.Campus_Prefix, Text = campus.Campus_Title, Selected = (campus.Campus_Prefix == model.BranchCode) });
            }


            // ordertype, orderstatus, term, year combo

            model.AvailableOrderYear.Clear();
            model.AvailableOrderTerm.Clear();

            model.AvailableOrderYear.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });
            model.AvailableOrderTerm.Add(new SelectListItem() { Value = "", Selected = true, Text = "N/A" });

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
                        model.AvailableOrderTerm.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Year) });
                        break;
                    case "CM05":
                        model.AvailableOrderYear.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                }
            }
        }

        public ActionResult PrintingPaperList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var model = new PrintingPaperModel();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin"))
                model.isadmin = true;
            else model.isadmin = false;


            model.BranchCode = _workContext.CurrentCustomer.AdminComment;
            SetAvailablePrintingPaperCombo(model);

            return View(model);
        }


        [HttpPost]
        public ActionResult GetPrintingPaperList(DataSourceRequest command,
            PrintingPaperModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();



            var ordersets = _orderService.GetAllOrderbyItemSets();
            var orders = _orderService.GetAllOrders(model.BranchCode, model.Year, model.Term, "TEST", "");



            var orders2 = orders.Join(ordersets, x => x.Id, y => y.Order_Id, (x, y) => new { x.Year, x.Term, x.BranchCode, y.Course_Id, y.Qty, y.Qty_Teacher, TestNo = Convert.ToInt32(y.Week), CourseName = "" });
            var orders3 = orders2.Where(x => 1 != 1).ToList();

            // 나머지 번호 자동생성
            foreach(var order in orders2)
            {
                var course = _courseService.GetAllCourseMasters().Where( y => y.Id == order.Course_Id).ToList()[0];
                int seq = 0;
                while ( order.TestNo + seq < course.TotalWeek + course.StartWeek)
                {
                    var order_temp = new {order.Year, order.Term, order.BranchCode, order.Course_Id, order.Qty, order.Qty_Teacher, TestNo =  order.TestNo + seq, CourseName = course.CourseName };
                    orders3.Add(order_temp);
                    seq++;
                }
            }

            var orders4 = orders3
                .GroupBy(x => new {x.Year, x.Term, x.BranchCode, x.Course_Id, x.CourseName, x.TestNo})
                .Select(group => new {group.Key.Year, group.Key.Term, group.Key.BranchCode, group.Key.Course_Id, group.Key.CourseName, group.Key.TestNo
                    , Qty = group.Sum(a => a.Qty), Qty_Teacher = group.Sum(a => a.Qty_Teacher)})
                    .OrderBy(x => x.Year).ThenBy(x => x.Term).ThenBy(x=>x.BranchCode).ThenBy(x=>x.Course_Id).ThenBy(x=>x.TestNo);


            var print = _orderService.GetAllPrintingPapers(model.BranchCode, model.Year, model.Term);


            int id = 1;

            var gridModel = new DataSourceResult
            {
                Data = orders4.Select(x =>
                {
                    var orderlist = new PrintingPaperModel()
                    {
                        Id = id,
                        BranchCode = x.BranchCode,
                        Term = x.Term != null ? x.Term : "",
                        Year = x.Year != null ? x.Year : "",
                        Course_Id = x.Course_Id,
                        Course = x.CourseName,
                        TestNo = x.TestNo,
                        Qty = x.Qty,
                        Qty_Teacher = x.Qty_Teacher,
                        YN_Print = "N"

                    };

                    var pre_print = print.Where(y => y.TestNo == x.TestNo && y.Course_Id == x.Course_Id && y.BranchCode == x.BranchCode).ToList();
                    if ( pre_print.Count > 0 && pre_print[0].Qty == x.Qty && pre_print[0].Qty_Teacher == x.Qty_Teacher)
                    {
                        orderlist.YN_Print = "Y";
                    }


                    id++;
                    return orderlist;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = orders.TotalCount
            };

            return Json(gridModel);

        }




        [HttpPost]
        public ActionResult PrintingPaperUpdate(PrintingPaperModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var printinglist = _orderService.GetAllPrintingPapers(model.BranchCode, model.Year, model.Term)
                .Where(x => x.Course_Id == model.Course_Id && x.TestNo == model.TestNo).ToList();

            var printing = new PrintingPaper();

            if ( printinglist.Count > 0 )
            {
                printing = printinglist[0];

                printing.Qty = model.Qty;
                printing.Qty_Teacher = model.Qty_Teacher;
                printing.reg_date = DateTime.Now;
                printing.reg_source = _workContext.CurrentCustomer.Username;

                if ( model.YN_Print == "Y")
                {
                    _orderService.UpdatePrintingPaper(printing);
                }
                else
                {
                    _orderService.DeletePrintingPaper(printing);
                }
            }
            else
            {
                printing.Year = model.Year;
                printing.Term = model.Term;
                printing.BranchCode = model.BranchCode;
                printing.Course_Id = model.Course_Id;
                printing.TestNo = model.TestNo;
                printing.Qty = model.Qty;
                printing.Qty_Teacher = model.Qty_Teacher;
                printing.reg_date = DateTime.Now;
                printing.reg_source = _workContext.CurrentCustomer.Username;

                if ( model.YN_Print == "Y")
                {
                    _orderService.InsertPrintingPaper(printing);
                }
            }




            return new NullJsonResult();
        }



        #endregion 



    }
}