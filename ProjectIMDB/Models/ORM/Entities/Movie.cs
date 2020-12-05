﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectIMDB.Models.ORM.Entities
{
    public class Movie: Base
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterURL { get; set; }


        public List<Comment> Comments { get; set; }
        public List<Rate> Rates { get; set; }
        public List<WatchList> WatchLists { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
        public List<MovieStar> MovieStars { get; set; }
        public List<MovieScenarist> MovieScenarists { get; set; }
        public List<MovieDirector> MovieDirectors { get; set; }


    }
}