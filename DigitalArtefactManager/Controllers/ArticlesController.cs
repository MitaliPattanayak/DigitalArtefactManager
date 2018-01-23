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

        // GET: Articles
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
                 {
                return View(db.Articles.ToList());
            }
            else
                return RedirectToAction("Login", "Login");
        }

        // GET: Articles
        public ActionResult IndexEmployee()
        {
            if (Session["UserName"] != null)
            {
                return View(db.Articles.ToList());
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Title,Body,publisher,publishDate")] Article article)
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

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Title,Body,publisher,publishDate")] Article article)
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

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        

        [HttpPost]
        public void Like(int id, bool status)
        {
            using (var db = new DigitalArtefactEntities()) //this dbentities to access class from Model also we will get in wed.config  
            {
                int Aid = id;
                bool LikeStatus = status;
                var articles = db.Articles.FirstOrDefault(x => x.ArticleId == Aid);
                //var toggle = false;
               
                string userName = Session["UserName"].ToString();
                Like like = db.Likes.FirstOrDefault(x => x.ArticleId == Aid && x.UserName == userName);
                
                // here we are checking whether user have done like or dislike    
                
                    like = new Like();
                    like.UserName = Session["userName"].ToString();
                    like.IsLiked = LikeStatus;
                    like.ArticleId = id;
                    
                    db.Likes.Add(like);                
                    db.SaveChanges();
            }
        }

        public int? Getlikecounts(int id) // to count like  
        {
            using (var db = new DigitalArtefactEntities())
            {
                var count = (from x in db.Likes where (x.ArticleId == id && x.UserName != null )select x).Count();
                ViewBag.likeCount = count;
                return count;
            }
        }
        ////To Get DisLike Count  
        public int? Getdislikecounts(int id)
        {
            using (var db = new DigitalArtefactEntities())
            {
                var count = (from x in db.Likes where x.ArticleId == id && x.UserName != null select x).Count();
                return count;
            }
        }
       
    }
}
