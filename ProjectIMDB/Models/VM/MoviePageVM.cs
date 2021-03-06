﻿using ProjectIMDB.Models.ORM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ProjectIMDB.Models.VM
{
    public class MoviePageVM
    {

        public IPagedList<Movie> MovieList { get; set; }
        public List<Person> PersonList { get; set; }
        public Rate Rate { get; set; }
        public Movie MovieDetail { get; set; }
        public List<Genre> GenreList { get; set; }




        //public string Name { get; set; }
        //public string Poster { get; set; }
        //public double Rate { get; set; }
        //public int ReleaseDate { get; set; }
        //public string Description { get; set; }
    }
}
