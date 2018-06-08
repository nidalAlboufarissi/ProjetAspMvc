using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ProjetAspMvc.Models;

namespace ProjetAspMvc.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.categorie);
            return View(articles.ToList());
        }
        
        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient c = new HttpClient();
            HttpClient cl = new HttpClient();
            cl.BaseAddress = new Uri("http://localhost:36411/");
            cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage re = cl.GetAsync("api/values/" + id).Result;
            Article et = re.Content.ReadAsAsync<Article>().Result;
            if (et == null)
            {
                return HttpNotFound();
            }
            return View(et);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.RefCat = new SelectList(db.Categories, "refcat", "nomcat");
            return View();
        }

        // POST: Articles/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleIm article)
        {
            string file = Path.GetFileNameWithoutExtension(article.imageFile.FileName);
            string extension = Path.GetExtension(article.imageFile.FileName);
            file = file + DateTime.Now.ToString("yymmssfff") + extension;
            article.photo = "~/Image/" + file;
            file = Path.Combine(Server.MapPath("~/Image/"), file);
            article.imageFile.SaveAs(file);
            
            if (ModelState.IsValid)
            {
                Article ar = new Article()
                {
                    Designation = article.Designation,
                    photo = article.photo,
                    NumArticle = article.NumArticle,
                    PrixU = article.PrixU,
                    stock = article.stock,
                    RefCat = article.RefCat
                };
                db.Articles.Add(ar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RefCat = new SelectList(db.Categories, "refcat", "nomcat", article.RefCat);
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
            ViewBag.RefCat = new SelectList(db.Categories, "refcat", "nomcat", article.RefCat);
            return View(article);
        }

        // POST: Articles/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleIm article)
        {
            string file = Path.GetFileNameWithoutExtension(article.imageFile.FileName);
            string extension = Path.GetExtension(article.imageFile.FileName);
            file = file + DateTime.Now.ToString("yymmssfff") + extension;
            article.photo = "~/Image/" + file;
            file = Path.Combine(Server.MapPath("~/Image/"), file);
            article.imageFile.SaveAs(file);

            if (ModelState.IsValid)
            {
                Article ar = new Article()
                {
                    
                    Designation = article.Designation,
                    photo = article.photo,
                    NumArticle = article.NumArticle,
                    PrixU = article.PrixU,
                    stock = article.stock,
                    RefCat = article.RefCat
                };
                db.Entry(ar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RefCat = new SelectList(db.Categories, "refcat", "nomcat", article.RefCat);
            return View(article);
        }
        public JsonResult getArticle(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Articles.Where(p => p.RefCat == id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getStock(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Articles.Where(p => p.NumArticle == id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAll()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Articles.ToList(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles ="Admin")]
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
    }
}
