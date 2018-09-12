using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRama.ModelsView.Home
{
    public class Definitions
    {
        public enum DispositionResult : byte
        {
            NotAvailable = 0,
            Like,
            Hate
        }

        public enum CollectionOrderBy : byte
        {
            Likes = 0,
            Hates,
            Date,
            zeta,
            Default = Likes,
        }

    }
}