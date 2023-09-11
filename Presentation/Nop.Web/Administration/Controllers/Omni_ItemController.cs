using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Nop.Core;
using Nop.Core.Domain.Directory;

using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Catalog;
using Nop.Services.Directory;

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
    public class Omni_ItemController  : BaseAdminController
    {

        #region Fields

        private readonly ICodeMasterService _codeMasterService;
        private readonly IItemCategoryService _itemCategoryService;
        private readonly IItemService _itemService;
        private readonly IItemSetService _itemSetService;
        private readonly IOrderService_Omni _orderService;
        private readonly ICourseMasterService _coursemasterService;
        private readonly ICalendarMasterService _calendarService;

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

		#endregion

		#region Constructors



        public Omni_ItemController(ICodeMasterService codeMasterService, IItemCategoryService itemCategoryService, IItemService itemService,
            IItemSetService itemSetService, IOrderService_Omni orderService, ICourseMasterService coursemasterService, ICalendarMasterService calendarservice,
            IPriceFormatter priceFormatter, ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
            IEncryptionService encryptionService,
            ICurrencyService currencyService, CurrencySettings currencySettings,
            IThemeProvider themeProvider,  IPermissionService permissionService,
            IWebHelper webHelper, IWorkContext workContext)
        {
            this._codeMasterService = codeMasterService;
            this._itemCategoryService = itemCategoryService;
            this._itemService = itemService;
            this._itemSetService = itemSetService;
            this._orderService = orderService;
            this._coursemasterService = coursemasterService;
            this._calendarService = calendarservice;

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
        }

        #endregion 

        
        #region Item

        public ActionResult ItemCategory()
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            return View();
        }



        [HttpPost]
        public ActionResult ItemCategory(DataSourceRequest command,
            Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var itemcategorys = _itemCategoryService.GetAllItemCategorys(command.Page - 1, command.PageSize);


                

            var gridModel = new DataSourceResult
            {
                Data = itemcategorys.Select(x =>
                {

                    var itemCategoryModel = new ItemCategoryModel()
                    {
                        Id = x.Id,
                        CategoryCode = x.CategoryCode,
                        CategoryName = x.CategoryName,
                        Remarks = x.Remarks
                    };
                    return itemCategoryModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),
                Total = itemcategorys.TotalCount
            };

            return Json(gridModel);
        }


        [HttpPost]
        public ActionResult ItemCategoryUpdate(ItemCategoryModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            if (model.CategoryCode != null)
                model.CategoryCode = model.CategoryCode.Trim();
            if (model.CategoryName != null)
                model.CategoryName = model.CategoryName.Trim();
            if (model.Remarks != null)
                model.Remarks = model.Remarks.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var itemcategory = _itemCategoryService.GetItemCategoryById(model.Id);
            if (itemcategory == null)
                return Json(new DataSourceResult() { Errors = "No itemcategory could be loaded with the specified Code" });

            //using (var scope = new System.Transactions.TransactionScope())
            //{

                //setting name or store has been changed
                _itemCategoryService.DeleteItemCategory(itemcategory);

                _itemCategoryService.InsertItemCategory(new ItemCategory(
                            model.CategoryCode,
                            model.CategoryName,
                            model.Remarks,
                            DateTime.Now,
                            _workContext.CurrentCustomer.Username)
                    );

            //    scope.Complete();
            //}


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult ItemCategoryInsert([Bind(Exclude = "Id")] ItemCategoryModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            if (model.CategoryCode != null)
                model.CategoryCode = model.CategoryCode.Trim();
            if (model.CategoryName != null)
                model.CategoryName = model.CategoryName.Trim();
            if (model.Remarks != null)
                model.Remarks = model.Remarks.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            _itemCategoryService.InsertItemCategory(new ItemCategory(
                        model.CategoryCode,
                        model.CategoryName,
                        model.Remarks,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username)
                );


            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult ItemCategoryDelete(ItemCategoryModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var itemcategory = _itemCategoryService.GetItemCategoryById(model.Id);
            if (itemcategory == null)
                return Json(new DataSourceResult() { Errors = "No itemcategory could be loaded with the specified Code" });
            _itemCategoryService.DeleteItemCategory(itemcategory);


            return new NullJsonResult();
        }

        #endregion


        # region Item

        private void SetAvailableItemListCombo(ItemListModel model)
        {


            model.ItemCategoryList.Clear();
            model.AvailableGradeList.Clear();
            model.AvailableTermList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "NO Term", Value = "", Selected = true });
            model.AvailableGradeList.Add(new SelectListItem() { Text = "NO Grade", Value = "", Selected = true });
            model.ItemCategoryList.Add(new SelectListItem() { Text = "NO Category", Value = "", Selected = true });



            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y").ToList();
            if ( calendar.Count > 0)
            {
                model.Term = calendar[0].Term;
            }
            var codemasters = _codeMasterService.GetAllCodeMasters();
            var categorys = this._itemCategoryService.GetAllItemCategorys();

            model.ItemCategoryList.Add(new SelectListItem() { Value = "", Selected = true, Text = "All" });
            foreach (ItemCategory category in categorys)
            {
                model.ItemCategoryList.Add(new SelectListItem() { Value = category.CategoryCode, Text = category.CategoryName });
            }

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01":
                        model.AvailableGradeList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }


        }

        public ActionResult ItemList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var model = new ItemListModel();
            var categorys = this._itemCategoryService.GetAllItemCategorys();


            //Edit 권한
            if (_permissionService.Authorize("OmniAdmin") || _permissionService.Authorize("OmniProduction"))
                model.editable = true;


            SetAvailableItemListCombo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult GetItemList(DataSourceRequest command,
            ItemListModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") && !_permissionService.Authorize("OmniBranch"))
                return AccessDeniedView();

            var categorys = _itemCategoryService.GetAllItemCategorys();

            var items = _itemService.GetAllItems(model.ItemCategoryId, model.ItemCode, model.ItemName, model.Term, model.Grade, command.Page - 1, command.PageSize);

            var tomorrow = new DateTime( DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day);
            var itemqty = _orderService.GetAllStocks("").Where( x => x.TranDate < tomorrow).GroupBy(x => new { x.ItemCode }).Select( group => new {
                ItemCode = group.Key.ItemCode,
                InQty = group.Sum( a => a.InQty),
                OutQty = group.Sum(a => a.OutQty),
                BalQty =  group.Sum( a => a.InQty) - group.Sum(a => a.OutQty)
            });

            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var itemListModel = new ItemListModel()
                    {
                        Id = x.Id,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        ItemCategoryId = x.ItemCategory,
                        Grade = x.Grade,
                        Term = x.Term
                    };

                    var itembalance = itemqty.Where(y => y.ItemCode == x.ItemCode).ToList();

                    itemListModel.Qty_Balance = itembalance.Count == 0 ? 0 : itembalance[0].BalQty;

                    return itemListModel;
                })
                .AsQueryable()
                .Join(categorys, x => x.ItemCategoryId, y => y.CategoryCode, (x,y) => new {ItemCode = x.ItemCode, ItemName = x.ItemName, Term = x.Term, Grade = x.Grade, ItemCategoryName = y.CategoryName, Id = x.Id, Qty_Balance = x.Qty_Balance})
                .Filter(filter)
                .Sort(sort),
                Total = items.TotalCount
            };

            return Json(gridModel);

        }

        private void SetAvailableItemCombo(ItemModel model)
        {

            model.AvailableCategorys.Clear();
            model.AvailableGrades.Clear();
            model.AvailableTerms.Clear();
            model.AvailableLevels.Clear();
            model.AvailableSubjects.Clear();

            model.AvailableLevels.Add(new SelectListItem() { Text = "NO Level", Value = "", Selected = true });
            model.AvailableTerms.Add(new SelectListItem() { Text = "NO Term", Value = "", Selected = true });
            model.AvailableGrades.Add(new SelectListItem() { Text = "NO Grade", Value = "", Selected = true });
            model.AvailableCategorys.Add(new SelectListItem() { Text = "NO Category", Value = "", Selected = true });
            model.AvailableSubjects.Add(new SelectListItem() { Text = "NO Subject", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();

            foreach (var x  in codemasters)
            {
                switch( x.CodeType)
                {
                    case "CM03" :
                        model.AvailableLevels.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM02" :
                        model.AvailableTerms.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01" :
                        model.AvailableGrades.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                    case "CM07":
                        model.AvailableSubjects.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }

            var categorys = _itemCategoryService.GetAllItemCategorys();
            foreach ( var y in categorys)
            {
                model.AvailableCategorys.Add(new SelectListItem() { Text = y.CategoryName, Value = y.CategoryCode, Selected = (y.CategoryCode == model.ItemCategory) });
            }

        }


        public ActionResult CreateItem()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction") )
                return AccessDeniedView();

            var model = new ItemModel();
            SetAvailableItemCombo(model);
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateItem(ItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            string itemcode = _itemService.GenerateItemCode(model.ItemCategory);

            var item = new Item(
                        itemcode,
                        model.ItemName,
                        model.ItemCategory,
                        model.DT_From,
                        model.DT_To,
                        model.UnitPrice,
                        model.UnitPrice_Half,
                        model.Grade,
                        model.Term,
                        model.Level,
                        model.Subject,
                        model.Remarks,
                        DateTime.Now,
                        _workContext.CurrentCustomer.Username);

            // Check ItemCode dup
            if (_itemService.CheckDup(item))
            {
                // dup error
                return Json(new DataSourceResult() { Errors = "Same itemcode exists" });
            }
            

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }
            

            _itemService.InsertItem(item);

            SetAvailableItemCombo(model);
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            return continueEditing ? RedirectToAction("ItemEdit", new { id = model.Id }) : RedirectToAction("ItemList");

        }

        public ActionResult EditItem(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            var item = _itemService.GetItemById(id);
            if (item == null)
                //No gift card found with the specified id
                return RedirectToAction("ItemList");

            var model = new Nop.Admin.Models.Omni_Item.ItemModel() { 
                Id = item.Id,
                ItemCode = item.ItemCode,
                ItemName = item.ItemName,
                ItemCategory = item.ItemCategory,
                DT_From = item.DT_From,
                DT_To = item.DT_To,
                Grade = item.Grade,
                Term = item.Term,
                Level = item.Level,
                Subject = item.Subject,
                UnitPrice = item.UnitPrice,
                UnitPrice_Half = item.UnitPrice_Half,
                Remarks = item.Remarks

            };
            SetAvailableItemCombo(model);
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult EditItem(ItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            var item = _itemService.GetItemById(model.Id);


            if (ModelState.IsValid)
            {
                //item = model.ToEntity(item);
                item.ItemName = model.ItemName;
                item.ItemCategory = model.ItemCategory;
                item.DT_From = model.DT_From;
                item.DT_To = model.DT_To;
                item.UnitPrice = model.UnitPrice;
                item.UnitPrice_Half = model.UnitPrice_Half;
                item.Grade = model.Grade;
                item.Term = model.Term;
                item.Level = model.Level;
                item.Subject = model.Subject;
                item.Remarks = model.Remarks;
                item.reg_date = DateTime.Now;
                item.reg_source = _workContext.CurrentCustomer.Username;
                _itemService.UpdateItem(item);


                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    SetAvailableItemCombo(model);

                    return RedirectToAction("EditItem", item.Id);
                }
                else
                {
                    return RedirectToAction("ItemList");
                }
            }

            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var item = _itemService.GetItemById(id);
            if (item == null)
                //No gift card found with the specified id
                return RedirectToAction("ItemList");

            _itemService.DeleteItem(item);


            SuccessNotification(_localizationService.GetResource("Admin.Item.Deleted"));
            return RedirectToAction("ItemList");
        }

        #endregion



        # region ItemSet

        private void SetAvailableItemSetCombo(ItemSetModel model)
        {

            model.AvailableSetCategoryList.Clear();
            model.AvailableGradeList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableLevelList.Clear();
            model.AvailableCourseList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableGradeList.Add(new SelectListItem() { Text = "All Grade", Value = "", Selected = true });
            model.AvailableLevelList.Add(new SelectListItem() { Text = "All Level", Value = "", Selected = true });
            model.AvailableSetCategoryList.Add(new SelectListItem() { Text = "All Category", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();

            var calendar = _calendarService.GetAllCalendarMasters().Where(x => x.YN_Active == "Y").ToList();

            if ( calendar.Count > 0)
            {
                model.Term = calendar[0].Term;
            }

            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM04":
                        model.AvailableSetCategoryList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.SetCategory) });
                        break;
                    case "CM03":
                        model.AvailableLevelList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01":
                        model.AvailableGradeList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }

        }

        public ActionResult ItemSetList()
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            var model = new ItemSetModel();


            SetAvailableItemSetCombo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult GetItemSetList(DataSourceRequest command,
            ItemSetModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var itemsets = _itemSetService.GetAllItemSets(model.Term, model.Grade, model.SetCategory, command.Page - 1, 1000).ToList();

            if ( !String.IsNullOrEmpty(model.SetName))
            {
                itemsets = itemsets.Where(x => x.SetName.Contains(model.SetName)).ToList();
            }


            var gridModel = new DataSourceResult
            {
                Data = itemsets.Select(x =>
                {
                    var itemListModel = new ItemSetModel()
                    {
                        Id = x.Id,
                        SetName = x.SetName,
                        SetCategory = x.SetCategory,
                        Term = x.Term,
                        Grade = x.Grade,
                        Level = x.Level,
                        Remarks = x.Remarks
                    };


                    var itemset_ds = _itemSetService.GetAllItemSet_Ds(x, 0, 10000);
                    itemListModel.ItemCnt = itemset_ds.Count;

                    return itemListModel;
                })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort),

                Total = itemsets.Count
            };

            return Json(gridModel);

        }


        [HttpPost]
        public ActionResult ItemSetUpdate(ItemSetModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            if (model.SetName != null)
                model.SetName = model.SetName.Trim();
            if (model.Remarks != null)
                model.Remarks = model.Remarks.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var itemset = _itemSetService.GetItemSetById(model.Id);
            if (itemset == null)
                return Json(new DataSourceResult() { Errors = "No itemset could be loaded with the specified Code" });


            if (ModelState.IsValid)
            {
                //item = model.ToEntity(item);
                itemset.SetName = model.SetName;
                itemset.SetCategory = model.SetCategory;
                itemset.Term = model.Term;
                itemset.Grade = model.Grade;
                itemset.Level = model.Level;
                itemset.Remarks = model.Remarks;
                itemset.Course = model.Course;
                itemset.reg_date = DateTime.Now;
                itemset.reg_source = _workContext.CurrentCustomer.Username;

                _itemSetService.UpdateItemSet(itemset);
            }

            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ItemSetInsert([Bind(Exclude = "Id")] ItemSetModel model)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            if (model.SetName != null)
                model.SetName = model.SetName.Trim();
            if (model.Remarks != null)
                model.Remarks = model.Remarks.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var itemset = new ItemSet(
            model.SetCategory,
            model.SetName,
            model.Term,
            model.Grade,
            model.Level,
            model.Course,
            model.Remarks,
            DateTime.Now,
            _workContext.CurrentCustomer.Username);

            _itemSetService.InsertItemSet( itemset );


            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult ItemSetDelete(int id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            var itemset = _itemSetService.GetItemSetById(id);
            if (itemset == null)
                //No gift card found with the specified id
                return RedirectToAction("ItemSetList");

            //using (var scope = new System.Transactions.TransactionScope())
            //{

                // 등록된 아이템이 있으면 같이 지운다.
                var itemset_dall = _itemSetService.GetAllItemSet_Ds(itemset);
                foreach (var itemset_d in itemset_dall)
                {
                    _itemSetService.DeleteItemSet_D(itemset_d);
                }

                _itemSetService.DeleteItemSet(itemset);

            //    scope.Complete();
            //}


            //SuccessNotification(_localizationService.GetResource("Admin.ItemSet.Deleted"));
            return new NullJsonResult();
        }

        private void SetAvailableItemSet_DCombo(ItemSetModel model)
        {

            model.AvailableSetCategoryList.Clear();
            model.AvailableGradeList.Clear();
            model.AvailableTermList.Clear();
            model.AvailableLevelList.Clear();
            model.AvailableCourseList.Clear();

            model.AvailableTermList.Add(new SelectListItem() { Text = "All Term", Value = "", Selected = true });
            model.AvailableGradeList.Add(new SelectListItem() { Text = "All Grade", Value = "", Selected = true });
            model.AvailableLevelList.Add(new SelectListItem() { Text = "All Level", Value = "", Selected = true });
            model.AvailableSetCategoryList.Add(new SelectListItem() { Text = "All Category", Value = "", Selected = true });

            var codemasters = _codeMasterService.GetAllCodeMasters();


            foreach (var x in codemasters)
            {
                switch (x.CodeType)
                {
                    case "CM04":
                        model.AvailableSetCategoryList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.SetCategory) });
                        break;
                    case "CM03":
                        model.AvailableLevelList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Level) });
                        break;
                    case "CM02":
                        model.AvailableTermList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Term) });
                        break;
                    case "CM01":
                        model.AvailableGradeList.Add(new SelectListItem() { Text = x.DetailName, Value = x.DetailCode, Selected = (x.DetailCode == model.Grade) });
                        break;
                }
            }

        }


        public ActionResult EditItemSet_D(int Id)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            var itemset = _itemSetService.GetItemSetById(Id);
            if (itemset == null)
                return Json(new DataSourceResult() { Errors = "No itemset could be loaded with the specified Code" });

            var model = new ItemSetModel() { 
                Id = itemset.Id,
                SetName = itemset.SetName,
                SetCategory = itemset.SetCategory,
                Term = itemset.Term,
                Grade = itemset.Grade,
                Level = itemset.Level
            };

            SetAvailableItemSet_DCombo(model);       

            return View(model);
        }

        
        [HttpPost]
        public ActionResult GetItemSet_DList(DataSourceRequest command,
            ItemSetModel model, Nop.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize("OmniAdmin"))
                return AccessDeniedView();

            var itemset = _itemSetService.GetItemSetById(model.Id);
            var itemset_d = _itemSetService.GetAllItemSet_Ds(itemset);
            var items = _itemService.GetTermItems(model.Term, model.Grade, command.Page - 1, 100).Where( x => ( itemset.SetCategory == "ZTST") ? x.ItemCategory == "TEPA" : x.ItemCategory != "TEPA" );

            int maxseq = 0;
            if (itemset_d.Count > 0)
                maxseq = itemset_d.Max(s => s.Seq);


            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var itemset_dmodel = new ItemSet_DModel()
                    {
                        Set_id = itemset.Id,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        Level = x.Level,
                        SEQ = 0,
                        Included = false
                    };

                    foreach(var precheck in itemset_d)
                    {
                        if ( precheck.ItemCode == itemset_dmodel.ItemCode) // 기존 있던거면
                        {
                            itemset_dmodel.Id = precheck.Id;
                            itemset_dmodel.Remarks = precheck.Remarks;
                            itemset_dmodel.Included = true;
                            itemset_dmodel.SEQ = precheck.Seq;
                            break;
                        }
                    }

                    return itemset_dmodel;
                })
                .AsQueryable(),

                Total = items.ToList().Count
            };

            return Json(gridModel);

        }

        [HttpPost]
        public ActionResult ItemSet_DUpdate(ItemSet_DModel itemset_d)
        {
            if (!_permissionService.Authorize("OmniAdmin") && !_permissionService.Authorize("OmniProduction"))
                return AccessDeniedView();

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
                }


                //using (var scope = new System.Transactions.TransactionScope())
                //{
                    var itemset_dd = _itemSetService.GetItemSet_DById(itemset_d.Id);
                    if (itemset_d.Included == true)
                    {
                        if (itemset_dd == null) // insert
                        {
                            var itemset = _itemSetService.GetItemSetById(itemset_d.Set_id);
                            var itemset_dall = _itemSetService.GetAllItemSet_Ds(itemset);

                            int maxseq = 0;
                            if (itemset_dall.Count > 0)
                                maxseq = itemset_dall.Max(s => s.Seq);

                            var itemset_d_new = new ItemSet_D()
                            {
                                Set_Id = itemset_d.Set_id,
                                Seq = maxseq + 1,
                                ItemCode = itemset_d.ItemCode,
                                Remarks = itemset_d.Remarks,
                                reg_source = _workContext.CurrentCustomer.Username,
                                reg_date = DateTime.Now
                            };

                            _itemSetService.InsertItemSet_D(itemset_d_new);

                        }
                        else // update
                        {
                            itemset_dd.ItemCode = itemset_d.ItemCode;
                            itemset_dd.Remarks = itemset_d.Remarks;
                            itemset_dd.reg_source = _workContext.CurrentCustomer.Username;
                            itemset_dd.reg_date = DateTime.Now;
                            _itemSetService.UpdateItemSet_D(itemset_dd);
                        }
                    }
                    else  // delete
                    {
                        _itemSetService.DeleteItemSet_D(itemset_dd);
                    }

                //    scope.Complete();
                //}
            }catch(Exception ex)
            {
                return Json(new DataSourceResult() { Errors = ex.Message });
            }

            return new NullJsonResult();
        }


        #endregion


    }
}