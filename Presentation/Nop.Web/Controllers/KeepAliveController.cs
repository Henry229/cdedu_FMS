﻿using System.Web.Mvc;

namespace Nop.Web.Controllers
{
    public partial class KeepAliveController : BasePublicController
    {

        public ActionResult Index()
        {
            return Content("I am alive!");
        }


        public ActionResult Checkpoint1()
        {
            return Content("I am alive!");
        }

    }
}
