using Microsoft.AspNetCore.Mvc;

namespace El_Cuervo_Gym_Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}
