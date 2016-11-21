﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JabberBCIT.Models;

namespace JabberBCIT.Controllers
{
    // [Authorize] Uncommenting this makes it so you have to login to view the forums
    public class ForumController : Controller
    {
        ChitterDbContext db = ChitterDbContext.Create();
        // GET: Forum
        public ActionResult Index(string tag = "Global")
        {
            ForumPostsViewmodel p = new ForumPostsViewmodel();
            p.Posts = db.ForumPosts.ToList();
            p.Subforums = db.Subforums.ToList();
            
            return View(p);
        }

        public ActionResult CreateForumPost()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateForumPost(ForumPost post, string tag = "Global")
        {
            post.UserID = User.Identity.GetUserId();
            post.PostTimestamp = DateTime.Now;

            try
            {
                Subforum s = (from Subforum in db.Subforums where Subforum.Name == tag select new Subforum()).FirstOrDefault();
                post.Subforum = s;

                db.ForumPosts.Add(post);
                db.SaveChanges();
            }
            finally
            {
                // subforum doesn't exist
            }
            return View();
            db.ForumPosts.Add(post);
            db.SaveChanges();
            ViewThreadViewModel model = new ViewThreadViewModel();
            model.post = post;
            model.comments = db.Comments.Where(x => x.PostID == post.PostID).ToList();
            return View("ViewForumThread", model);
        }

        public ActionResult ViewForumThread(int? id)
        {
            ViewThreadViewModel model = new ViewThreadViewModel();
            model.post = db.ForumPosts.Find(id);
            model.comments = db.Comments.Where(x => x.PostID == id).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult ForumPostPartial(ForumPost p)
        {
            return PartialView();
        }
    }
}