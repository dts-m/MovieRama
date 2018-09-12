using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using MovieRama.ModelsView.Home;
using static MovieRama.ModelsView.Home.Definitions;

namespace MovieRama.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(MoviesSetupController.Index), "MoviesSetup");
            }
            
            //
            return View(IndexData.Get(User.Identity.GetUserId(), Convert.ToString(Request.QueryString["orderby"])));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}