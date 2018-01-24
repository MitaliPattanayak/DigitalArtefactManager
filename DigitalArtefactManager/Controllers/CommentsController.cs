using DigitalArtefactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalArtefactManager.Controllers
{
    public class CommentsController : Controller
    {
        private DigitalArtefactEntities db = new DigitalArtefactEntities();

        //To post a comment on button click
        [HttpPost]
        public void PostComment(int Aid, string comments)
        {
            if (Session["UserName"] != null)
            {
                if (ModelState.IsValid)
                {
                    Comment comment = new Comment();
                    comment.CommentedBy = Session["UserName"].ToString();
                    comment.CommentedOn = DateTime.Now;
                    comment.CommentBody = comments;
                    comment.ArticleId = Aid;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                }

            }
        }

        //To get the comments for an article
        public ActionResult Comments(int AId)
        {
            return View(db.Comments.Where(a => a.ArticleId==AId).OrderByDescending(c => c.CommentedOn).ToList());
        }
    }
}