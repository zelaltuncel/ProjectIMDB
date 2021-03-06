﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectIMDB.Models.Attributes;
using ProjectIMDB.Models.ORM.Context;
using ProjectIMDB.Models.ORM.Entities;
using ProjectIMDB.Models.VM;
using X.PagedList;

namespace ProjectIMDB.Areas.Site.Controllers
{
    [Area("Site")]

    public class UserController : BaseController
    {
        private readonly IMDBContext _context;

        public UserController(IMDBContext context) : base(context)
        {
            _context = context;
        }

        [SiteAuth]
        [Route("/watchlist")]
        public IActionResult Watchlist(int? page)

        {
            var pagenumber = page ?? 1;

            int id = Convert.ToInt32(TempData["ID"]);
            UserPageVM model = new UserPageVM();

            model.UserWatch = _context.WatchLists.Include(x => x.Movie).ThenInclude(Movie => Movie.MoviePeople).ThenInclude(MoviePerson => MoviePerson.Person ).Include(x => x.Movie).ThenInclude(Movie =>Movie.Rates).Where(q => q.UserID == id && q.IsDeleted==false).ToList().ToPagedList(pagenumber, 5);

            model.User = _context.Users.FirstOrDefault(q => q.ID == id);

            return View(model);

        }

        [Route("/ratelist")]
        public IActionResult RateList(int? page)

        {
            var pagenumber = page ?? 1;

            int id = Convert.ToInt32(TempData["ID"]);
            UserPageVM model = new UserPageVM();

            model.UserRate = _context.Rates.Include(x => x.Movie).ThenInclude(Movie => Movie.Comments).Include(x => x.Movie).ThenInclude(Movie => Movie.MoviePeople).ThenInclude(MoviePerson => MoviePerson.Person).Where(q => q.UserID == id && q.IsDeleted == false).ToList().ToPagedList(pagenumber, 5);

            model.User = _context.Users.FirstOrDefault(q => q.ID == id);



            return View(model);



        }




        [HttpPost]
        public IActionResult Register(UserRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.UserName = model.username;
                user.EMail = model.email;
                user.Password = model.password;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }

            else
            {
                 TempData["error2"] = "Passwords do not match!";


                return RedirectToAction("Index", "Home");


            }



        }

        public IActionResult Index()
        {
            var model = new EditVM();
            model.UserEdit = new UserVM();
            model.Change = new ChangeVM();
            return View(model);
        }


        [SiteAuth]
        [Route("profile/{id}")]
        public IActionResult Edit(int id)
        {
            UserVM model = new UserVM();

            User user = _context.Users.FirstOrDefault(q => q.ID == id);
         /*   model.UserEdit.id = user.ID; *///dene olmalı mı
            model.username = user.UserName;
            model.email = user.EMail;
            model.name = user.Name;
            model.surname = user.SurName;
            model.country = user.Country;
            model.birthdate = user.BirthDate;
            model.password = user.Password;

            return View("Index", new EditVM { UserEdit = model, Change = null });

        }

        [SiteAuth]
        [HttpPost]
        public IActionResult Edit(UserVM model)
        {
            User user = _context.Users.FirstOrDefault(x => x.ID == model.id);


            if (ModelState.IsValid)
            {

                //user.ID = model.id;  olmalı mı
                user.UserName = model.username;
                user.Name = model.name;
                user.SurName = model.surname;
                user.EMail = model.email;
                user.Country = model.country;
                user.BirthDate = model.birthdate;
                //user.Password = model.newpassword; ??
                _context.SaveChanges();

                TempData["username"] = "Welcome! " + model.username;
                return RedirectToAction("Index", "Home");


            }

            else
            {
                model.username = user.UserName;
                model.email = user.EMail;
                model.name = user.Name;
                model.surname = user.SurName;
                model.country = user.Country;
                model.birthdate = user.BirthDate;
                model.password = user.Password;
                return View("Index", new EditVM { UserEdit = model, Change = null });
            }
        }

        [SiteAuth]
        [HttpPost]
        public IActionResult Change(ChangeVM model)
        {

            User user = _context.Users.FirstOrDefault(x => x.ID == model.id);

            if (user.Password == model.oldpassword && model.newpassword == model.confirmpassword)
            {

                user.Password = model.newpassword;
              
                _context.SaveChanges();


                return RedirectToAction("Index", "Home");  //Nereye gitmeli??
            }

            else
            {

      

                //model.username = user.UserName;
                //model.email = user.EMail;
                //model.name = user.Name;
                //model.surname = user.SurName;
                //model.country = user.Country;
                //model.birthdate = user.BirthDate;
                //model.password = user.Password;

                UserVM model2 = new UserVM();

                model2.username = user.UserName;
                model2.email = user.EMail;
                model2.name = user.Name;
                model2.surname = user.SurName;
                model2.country = user.Country;
                model2.birthdate = user.BirthDate;
                model2.password = user.Password;
                return View("Index", new EditVM { UserEdit = model2, Change = model });



            }


        }



    }
}
