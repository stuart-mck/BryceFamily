using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BryceFamily.Web.MVC.Infrastructure.Alerts
{
    public static class AlertExtensions
    {
        const string Alerts = "_Alerts";

        public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                tempData[Alerts] = new List<Alert>();
            }

            return (List<Alert>)tempData[Alerts];
        }

        public static ActionResult WithSuccess(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-success", message);
        }

        public static ActionResult WithInfo(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-info", message);
        }

        public static ActionResult WithWarning(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-warning", message);
        }

        public static ActionResult WithError(this ActionResult result, string message)
        {
            return new AlertDecoratorResult(result, "alert-danger", message);
        }

        public static ActionResult WithModelStateErrors(this ActionResult result, ModelStateDictionary state)
        {
            var msg = new StringBuilder();
            state.Values.ToList().ForEach(v => v.Errors.ToList().ForEach(ve => msg.AppendLine(ve.ErrorMessage)));
            return new AlertDecoratorResult(result, "alert-danger", msg.ToString());
        }
    }
}