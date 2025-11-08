using Microsoft.AspNetCore.Mvc;

namespace TicketManagementSystem.API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
