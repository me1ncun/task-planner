using Microsoft.AspNetCore.Mvc;

namespace taskplanner_user_service.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}