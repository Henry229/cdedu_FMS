using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Security;

namespace Nop.Plugin.Other.OmniBackOffice
{
    class OmniBackOfficePlugin : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public OmniBackOfficePlugin(IPermissionService permissionService)
        {
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "OmniBackOffice";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Other.OmniBackOffice.Controllers" }, { "area", null } };
        }

        #endregion
    }
}

