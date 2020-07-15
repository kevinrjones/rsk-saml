using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sp.Controllers
{
    public class Home : Controller
    {
        public IActionResult Logout() => SignOut("cookie");

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ChallengeScheme(string scheme)
        {
            var result = await HttpContext.AuthenticateAsync(scheme);
            if (result.Succeeded && result.Principal != null) return RedirectToAction("Index");

            return Challenge(scheme);
        }

        [Authorize]
        public IActionResult Details()
        {
            return View();
        }
    }
}