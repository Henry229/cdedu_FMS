using System;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using Nop.Admin.Infrastructure.Cache;
using Nop.Admin.Models.Home;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Common;
using Nop.Services.Configuration;
using Nop.Services.News;
using Nop.Admin.Models.News;
using Nop.Services.Security;

namespace Nop.Admin.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        #region Fields
        private readonly IStoreContext _storeContext;
        private readonly CommonSettings _commonSettings;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        private readonly INewsService _newsService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public HomeController(IStoreContext storeContext, 
            CommonSettings commonSettings, 
            ISettingService settingService,
            IWorkContext workContext,
            ICacheManager cacheManager, INewsService newsService, IPermissionService permissionService)
        {
            this._storeContext = storeContext;
            this._commonSettings = commonSettings;
            this._settingService = settingService;
            this._workContext = workContext;
            this._cacheManager= cacheManager;
            this._newsService = newsService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            var model = new DashboardModel();
            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            if (_permissionService.Authorize("OmniAdmin"))
                model.Is_Admin = true;
            if (_permissionService.Authorize("OmniProduction"))
                model.Is_Production = true;
            if (_permissionService.Authorize("OmniAccountant"))
                model.Is_Accountant = true;
            if (_permissionService.Authorize("OmniBranch"))
                model.Is_Branch = true;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult NopCommerceNews()
        {
            try
            {
                var newsitems =  _newsService.GetAllNews(0, 0, 0, 10, false);
                            

                var dashbordmodel = new DashboardModel();

                foreach( var newsitem in newsitems)
                {
                    var newsitemmodel = new NewsItemModel();
                    newsitemmodel.Title = newsitem.Title;
                    newsitemmodel.Short = newsitem.Short;
                    newsitemmodel.CreatedOn = newsitem.CreatedOnUtc;

                    dashbordmodel.NewsItemList.Add(newsitemmodel);
                }

                return PartialView(dashbordmodel);
            }
            catch (Exception)
            {
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult NopCommerceNewsHideAdv()
        {
            _commonSettings.HideAdvertisementsOnAdminArea = !_commonSettings.HideAdvertisementsOnAdminArea;
            _settingService.SaveSetting(_commonSettings);
            return Content("Setting changed");
        }

        #endregion
    }
}
