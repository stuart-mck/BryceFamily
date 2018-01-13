using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;

namespace BryceFamily.Web.MVC.Controllers
{
    public class BaseController : Controller
    {
        private readonly string pageName;
        private readonly string className;

        public BaseController(string pageName, string className)
        {
            this.pageName = pageName;
            this.className = className;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.PageName = pageName;
            ViewBag.PageClass = className;
            base.OnActionExecuted(context);
        }

        protected CancellationToken GetCancellationToken()
        {
            return CancellationToken.None;
        }
    }
}
