using Microsoft.AspNetCore.Mvc;

namespace BryceFamily.Web.MVC.Controllers
{
    public class HealthCheckController : Controller
    {
        [Route("healthcheck")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}