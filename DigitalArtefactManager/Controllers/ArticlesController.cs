using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DigitalArtefactManager.Models;

namespace DigitalArtefactManager.Controllers
{
    public class ArticlesController : Controller
    {
        private DigitalArtefactEntities db = new DigitalArtefactEntities();

        // GET: Articles order by created date
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View(db.Articles.OrderByDescending(c => c.publishDate).ToList());
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles
        public ActionResult IndexEmployee()
        {
            if (Session["UserName"] != null)
            {
                return View(db.Articles.OrderByDescending(c=>c.publishDate).ToList());
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserName"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Article article = db.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(article);
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles/Details/5
        public ActionResult ReadArticles(int? id)
        {
            
            if (Session["UserName"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Article article = db.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                ViewBag.likeCount = Getlikecounts(id);
                return View(article);
            }
            else
                return RedirectToAction("Login", "Login");
        }


        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Title,Body,publisher,publishDate")] Article article)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    article.publisher = Session["UserName"].ToString();
                    article.publishDate = DateTime.Now;
                    db.Articles.Add(article);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(article);
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Article article = db.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Title,Body,publisher,publishDate")] Article article)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    article.publisher = Session["UserName"].ToString();
                    article.publishDate = DateTime.Now;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(article);
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
             if (Session["UserName"] != null)
             {
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Article article = db.Articles.Find(id);
                if (article == null)
                {
                    return HttpNotFound();
                }
                return View(article);
            }
            else
            return RedirectToAction("Login", "Login");
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserName"] != null)
            {
                try
                {
                    Article article = db.Articles.Find(id);
                    db.Articles.Remove(article);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            else
                return RedirectToAction("Login", "Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Like an article on Like icon click
        [HttpPost]
        public int Like(int id, bool status)
        {
            using (var db = new DigitalArtefactEntities())  
            {
                int Aid = id;
                bool LikeStatus = status;
                string userName = Session["UserName"].ToString();


                var articles = db.Articles.FirstOrDefault(x => x.ArticleId == Aid);
                Like like = db.Likes.FirstOrDefault(x => x.ArticleId == Aid && x.UserName == userName);

                
                    like = new Like();
                    like.UserName = Session["userName"].ToString();
                    like.IsLiked = LikeStatus;
                    like.ArticleId = id;

                    if (articles.likeCount == null) // if no one has done like or dislike and first time any one doing like and dislike then assigning 1 and                                                                                0    
                    {
                        articles.likeCount = 1;
                    }
                    else
                        articles.likeCount = articles.likeCount + 1;

                    db.Likes.Add(like);
                    db.SaveChanges();
                 int count = Getlikecounts(articles.ArticleId);
                return count;
            }
        }

        //Get like counts for an article
            public int Getlikecounts(int? ArticleId) // to count like by article id and user  
            {
                using (var db = new DigitalArtefactEntities())
                {
                string userName = Session["UserName"].ToString();

                var count = (from x in db.Likes where (x.ArticleId == ArticleId && x.UserName == userName) select x).Count();
                    ViewBag.likeCount = count;
                    return count;
                }
            }

        // GET: Articles by like counts
        public ActionResult MostLiked()
        {
            if (Session["UserName"] != null)
            {
                return View(db.Articles.OrderByDescending(c => c.likeCount).ToList());
            }
            else
                return RedirectToAction("Login", "Login");
        }

    }
    }
