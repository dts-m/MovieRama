﻿using Microsoft.AspNet.Identity;
using MovieRama.ModelsView.MoviesSetup;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NHome = MovieRama.ModelsView.Home;

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
            return View(IndexData.Get(User?.Identity.GetUserId(), Convert.ToString(Request?.QueryString.Get("orderby"))));
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
            string movieTitle = collection?.GetValue(nameof(AppDAL.pocos.Movies.Title))?.AttemptedValue;
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                throw new HttpException(400, "Title cannot be empty");
            }

            using (var dbc = new AppDAL.AppDataDbContext())
            {
                dbc.Movies.Add(new AppDAL.pocos.Movies()
                {
                    Title = movieTitle,
                    Description = collection.GetValue(nameof(AppDAL.pocos.Movies.Description)).AttemptedValue,
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

        public async Task<ActionResult> UserDisposition()
        {
            // Get items from query string
            if (!int.TryParse(Convert.ToString(Request.QueryString["movieId"]), out int iRequestMovieId))
            {
                throw new HttpException(400, "Invalid Movie");
            }
            if (!int.TryParse(Convert.ToString(Request.QueryString["value"]), out int iRequestDisposition))
            {
                throw new HttpException(400, "Invalid Disposition");
            }
            //
            var currentUserId = User.Identity.GetUserId();
            //
            using (var dbc = new AppDAL.AppDataDbContext())
            {
                // get user who added requested movie
                var IsFromCurrentUser =
                    dbc.Movies
                    .Where(x => x.Id == iRequestMovieId && x.UserId.Equals(currentUserId))
                    .Any();

                if (IsFromCurrentUser)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Get current user disposition
                try
                {
                    var currentUserDispositionForMovie =
                        dbc.MoviesUsersDisposition
                        .Where(x => x.MovieId == iRequestMovieId && x.UserId.Equals(currentUserId))
                        .First();

                    //
                    currentUserDispositionForMovie.DateTimeSet = DateTime.UtcNow;
                    currentUserDispositionForMovie.MovieId = iRequestMovieId;
                    currentUserDispositionForMovie.UserId = currentUserId;
                    currentUserDispositionForMovie.Like = iRequestDisposition == (int)NHome.Definitions.DispositionResult.Like;
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException)
                {
                    dbc.MoviesUsersDisposition.Add(new AppDAL.pocos.MoviesUsersDisposition()
                    {
                        DateTimeSet = DateTime.UtcNow,
                        MovieId = iRequestMovieId,
                        UserId = currentUserId,
                        Like = iRequestDisposition == (int)NHome.Definitions.DispositionResult.Like,
                    });
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, $"Something went wrong. {ex.Message}");
                }

                //
                try
                {
                    await dbc.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, $"Could not save data. {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ViewResult MoviesByUser(string id)
        {
            var mdl = new ModelsView.MoviesSetup.MoviesByUser();

            mdl.UpdateFromSourceForUser(id, User?.Identity.GetUserId());

            return View(mdl);
        } 
    }
}