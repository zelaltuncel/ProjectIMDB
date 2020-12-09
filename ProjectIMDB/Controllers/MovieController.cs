﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIMDB.Models.ORM.Context;
using ProjectIMDB.Models.ORM.Entities;
using ProjectIMDB.Models.VM;

namespace ProjectIMDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMDBContext _context;

        public MovieController(IMDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<MovieVM> movies = _context.Movies.Include(x => x.MovieGenres).ThenInclude(MovieGenres => MovieGenres.Genre).Where(q => q.IsDeleted == false).Select(q => new MovieVM()
            {
                id = q.ID,
                name = q.Name,
                duration = q.Duration,
                releasedate = q.ReleaseDate,
                posterurl = q.PosterURL,
                adddate = q.AddDate,
                updatedate = q.UpdateDate,
                moviegenres = q.MovieGenres.Where(x => x.IsDeleted == false).ToList()

            })  .ToList();

            return View(movies);
        }

        public IActionResult Add()
        {
            MovieVM model = new MovieVM();
            model.genres = _context.Genres.ToList();

            return View(model);
        }


        [HttpPost]
        public IActionResult Add(MovieVM model, int[] genrearray)
        {
            if (ModelState.IsValid)
            {
                Movie movie = new Movie();
                movie.Name = model.name;
                movie.Duration = model.duration;
                movie.ReleaseDate = model.releasedate;
                movie.PosterURL = model.posterurl;

                _context.Movies.Add(movie);
                _context.SaveChanges();

                int MovieID = movie.ID;

                foreach (var item in genrearray)
                {
                    MovieGenre movieGenre = new MovieGenre();
                    movieGenre.MovieID = MovieID;
                    movieGenre.GenreID = item;

                    _context.MovieGenres.Add(movieGenre);
                }
                _context.SaveChanges();

                return RedirectToAction("Index", "Movie");
            }

            else
            {
                return View();

            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Movie movie = _context.Movies.FirstOrDefault(q => q.ID == id);
            movie.IsDeleted = true;
            _context.SaveChanges();

            return Json("Silme işlemi başarılı!!");
        }


        public IActionResult Edit(int id)
        {
            Movie movie = _context.Movies.Include(x => x.MovieGenres).ThenInclude(MovieGenre => MovieGenre.Genre).FirstOrDefault(x => x.ID == id);
            MovieVM model = new MovieVM();

            model.id = movie.ID;
            model.name = movie.Name;
            model.duration = movie.Duration;
            model.releasedate = movie.ReleaseDate;
            model.posterurl = movie.PosterURL;
            model.genres = _context.Genres.ToList();
            model.moviegenres = movie.MovieGenres.Where(x => x.IsDeleted == false).ToList();

            return View(model);


        }

        [HttpPost]
        public IActionResult Edit(MovieVM model, int[] genrearray)
        {
            Movie movie = _context.Movies.Include(x => x.MovieGenres).ThenInclude(MovieGenre => MovieGenre.Genre).FirstOrDefault(x => x.ID == model.id);

            if (ModelState.IsValid)
            {
                movie.Name = model.name;
                movie.Duration = model.duration;
                movie.ReleaseDate = model.releasedate;
                movie.PosterURL = model.posterurl;
                movie.UpdateDate = model.updatedate;

                _context.SaveChanges();

                int MovieID = movie.ID;
                List<MovieGenre> movie2 = movie.MovieGenres.ToList();

                foreach (var item in movie2)
                {
                    item.IsDeleted = true;
                }

                foreach (var item in genrearray)
                {
                    MovieGenre movieGenre = new MovieGenre();
                    movieGenre.GenreID = item;
                    movieGenre.MovieID = MovieID;

                    _context.MovieGenres.Add(movieGenre);
                }
                
                _context.SaveChanges();

                return RedirectToAction("Index", "Movie");
            }
            else
            {
                return View();
            }

        }

    }
}
