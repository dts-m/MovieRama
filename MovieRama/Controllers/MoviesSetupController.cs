using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MovieRama.ModelsView.MoviesSetup;

namespace MovieRama.Controllers
{
    [Authorize]
    public class MoviesSetupController : Controller
    {
        // GET: MoviesSetup
        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction(nameof(MoviesSetupController.Index), "Home");
            //}

            //
            return View(IndexData.Get(User.Identity.GetUserId(), Convert.ToString(Request.QueryString["orderby"])));
        }

        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FormCollection collection)
        {
            string movieTitle = collection.GetValue(nameof(AppDAL.Movies.Title)).AttemptedValue;
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                throw new HttpException(400, "Title cannot be empty");
            }

            using (var dbc = new AppDAL.ApplicationDataDbContext())
            {
                dbc.Movies.Add(new AppDAL.Movies() {
                    Title = movieTitle,
                    Description = collection.GetValue(nameof(AppDAL.Movies.Description)).AttemptedValue,
                    UserId = User.Identity.GetUserId()
                });

                try
                {
                    await dbc.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, $"Could not save data. {ex.Message}");
                }
            }

            return View();
        }
    }
}