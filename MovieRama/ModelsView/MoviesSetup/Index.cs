﻿using System;
using System.Web;
using Microsoft.AspNet.Identity;

namespace MovieRama.ModelsView.MoviesSetup
{
    public class Index
    {
        public AppDAL.Movies Movies { get; set; }

        public AppDAL.MoviesUsersDisposition MoviesUsersDisposition { get; set; }

        public string PostedByUser { get; set; }

        public string UserId { get; set; }

        public bool FromCurrentUser { get; set; }

        // 0: No Disposition
        // 1: Like
        // 2: Hate
        public byte UserDisposition { get; set; }

        public int Likes { get; set; }

        public int Hates { get; set; }
    }
}