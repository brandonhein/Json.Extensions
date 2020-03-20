using Microsoft.AspNetCore.Mvc;

namespace Json.Extension.Sample.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
