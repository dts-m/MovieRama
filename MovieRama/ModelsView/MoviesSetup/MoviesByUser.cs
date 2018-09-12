using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieRama.ModelsView.MoviesSetup
{
    public class MoviesByUser
    {
        public IEnumerable<AppDAL.Movies> Movies;

        public string UserName { get; set; }

        public bool IsByCurrentUser { get; set; }

        public void UpdateFromSourceForUser(string userId, string currentUserId)
        {
            IsByCurrentUser = userId.Equals(currentUserId, StringComparison.OrdinalIgnoreCase);

            using (var dbc = new Models.ApplicationDbContext())
            {
                try
                {
                    UserName = dbc.Users
                        .Where(x => x.Id.Equals(userId))
                        .Select(x => x.UserName)
                        .ToList()
                        .First();
                }
                catch(Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException)
                {
                    UserName = "Unknown User";
                }
            }

            using (var dbc = new AppDAL.ApplicationDataDbContext())
            {
                Movies = dbc.Movies
                    .Where(x => x.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(x => x.DateAdded)
                    .ToList();
            }
        }
    }
}