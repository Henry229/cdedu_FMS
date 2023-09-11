using System.Web.Mvc;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class Omni_FrontController : BasePublicController
    {
        // GET: Omni_Front
        public ActionResult EnrolStudent()
        {
            return View();
        }
    }
}