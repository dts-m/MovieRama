using System.Collections.Generic;
using System.Linq;
using static MovieRama.ModelsView.Home.Definitions;

namespace MovieRama.ModelsView.MoviesSetup
{
    public static class IndexData
    {
        public static IEnumerable<Index> Get(string currentUserId, string orderBy)
        {
            if(string.IsNullOrWhiteSpace(currentUserId))
            {
                return new List<Index>();
            }
            //
            using (var Appdbc = new Models.ApplicationDbContext())
            using (var dbc = new AppDAL.AppDataDbContext())
            {
                var res = (from m in dbc.Movies
                           select new ModelsView.MoviesSetup.Index
                           {
                               Movies = m,
                           }).ToList();

                foreach (var item in res)
                {
                    {
                        var user = (from u in Appdbc.Users
                                    where u.Id == item.Movies.UserId
                                    select new Common.SimpleUserData
                                    {
                                        UserName = u.UserName,
                                        Id = u.Id
                                    })
                                    .ToList()
                                    .DefaultIfEmpty(new Common.SimpleUserData { UserName = "Unknown User", Id = null })
                                    .First();
                        if ((user.Id?.Equals(currentUserId, System.StringComparison.OrdinalIgnoreCase)) ?? false)
                        {
                            item.PostedByUser = "You";
                            item.FromCurrentUser = true;
                        }
                        else
                        {
                            item.PostedByUser = user.UserName;
                            item.FromCurrentUser = false;
                        }
                        item.UserId = user.Id;
                    }
                    {
                        var resDisposition = dbc.MoviesUsersDisposition
                                                .Where(x => x.MovieId.Equals(item.Movies.Id) && x.UserId.Equals(currentUserId))
                                                .Select(x => x.Like);

                        item.UserDisposition = (byte)DispositionResult.NotAvailable;
                        if (resDisposition.Any())
                        {
                            item.UserDisposition = resDisposition.First() ? (byte)DispositionResult.Like : (byte)DispositionResult.Hate;
                        }
                    }

                    var resLikes = dbc.MoviesUsersDisposition
                        .Where(x => x.MovieId.Equals(item.Movies.Id))
                        .GroupBy(x => x.Like)
                        .Select(x => new
                        {
                            disposition = x.Key,
                            count = x.Count()
                        }).ToDictionary(x => x.disposition, x => x.count);

                    //
                    int iVal;
                    item.Likes = 0;
                    item.Hates = 0;
                    if (resLikes.TryGetValue(true, out iVal))
                    {
                        item.Likes = iVal;
                    }
                    if (resLikes.TryGetValue(false, out iVal))
                    {
                        item.Hates = iVal;
                    }
                }

                //
                return SortData(orderBy, res);
            }
        }

        private static List<Index> SortData(string orderBy, List<Index> res)
        {
            if (!int.TryParse(orderBy/*Convert.ToString(Request.QueryString["orderby"])*/, out int intOrder))
            {
                intOrder = (int)CollectionOrderBy.Default;
            }

            var sortOrder = intOrder < (int)CollectionOrderBy.zeta ? (CollectionOrderBy)intOrder : CollectionOrderBy.Likes;
            switch (sortOrder)
            {
                case CollectionOrderBy.Likes:
                    res = res
                        .OrderByDescending(x => x.Movies.DateAdded)
                        .OrderByDescending(x => x.Likes)
                        .ToList();
                    break;
                case CollectionOrderBy.Hates:
                    res = res
                        .OrderByDescending(x => x.Movies.DateAdded)
                        .OrderByDescending(x => x.Hates)
                        .ToList();
                    break;
                case CollectionOrderBy.Date:
                default:
                    res = res
                        .OrderByDescending(x => x.Movies.DateAdded)
                        .ToList();
                    break;
            };
            return res;
        }
    }
}