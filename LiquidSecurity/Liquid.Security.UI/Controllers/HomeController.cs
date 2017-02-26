using Liquid.Security.Authentication;
using Liquid.Security.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace Liquid.Security.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var identifier = User.Identity.GetUserId();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var model = new IndexViewModel()
            {
                HasPassword = userManager.HasPassword(identifier),
                Logins = userManager.GetLogins(identifier)
            };
            
            return View(model);
        }
    }
}