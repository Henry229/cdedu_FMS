using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Admin
{
    public class OmniContentResult
    {
        public static ContentResult ReturnMsg(string strMsg)
        {
            var retval = new ContentResult();

            retval.Content = "<div class='msg'><h2>" + strMsg + "</h2></div>";
            retval.Content += "<div class='goback'><b>" + "<a href='javascript:history.go(-1)'>Back to previous page<a>" + "</b></div>";
            return retval;
        }
    }
}