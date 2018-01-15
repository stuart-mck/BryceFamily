using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;
using System.IO;

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

        protected static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
