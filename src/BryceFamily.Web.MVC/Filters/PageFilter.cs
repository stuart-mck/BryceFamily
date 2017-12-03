using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Filters
{
    public class PageFilter : ActionFilterAttribute
    {
        private readonly string pageName;
        private readonly string className;

        public PageFilter(string pageName, string className)
        {
            this.pageName = pageName;
            this.className = className;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var viewBag = ((Controller)context.Controller).ViewBag;
            viewBag.PageName = pageName;
            viewBag.PageClass = className;

            base.OnActionExecuting(context);
        }
    }
}
