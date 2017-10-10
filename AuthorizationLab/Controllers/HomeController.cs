using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationLab.Controllers
{
    [Authorize(Policy = "EmployeeId")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
