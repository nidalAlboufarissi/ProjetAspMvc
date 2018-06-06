using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetAspMvc.Models;


namespace ProjetAspMvc.Controllers
{
    [Authorize]
    public class CommandesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Commandes
        public ActionResult Index()
        {
            ViewBag.e = new SelectList(db.Categories, "refcat", "nomcat");
            return View();
        }
        public ActionResult Panier()
        {
            String s = this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
            string id = db.Users.Where(p => p.Email == s).First().Id;
            List<Commande> c = db.Commandes.Where(p => p.Id == id).ToList();
            double som = 0;
            foreach (var i in c)
            {
               som+= (i.article.PrixU * i.QteArticle);
            }
            ViewBag.s = som;
            return View(c);
        }
        // GET: Commandes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commande commande = db.Commandes.Find(id);
            if (commande == null)
            {
                return HttpNotFound();
            }
            return View(commande);
        }

        // GET: Commandes/Create
        public ActionResult Create()
        {
            ViewBag.NumArticle = new SelectList(db.Articles, "NumArticle", "Designation");
            return View();
        }

        // POST: Commandes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Commande commande)
        {
            commande.DateCmd = DateTime.Now;
            //string s = Session["mail"].ToString();
            String s = this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
            commande.Id = db.Users.Where(p => p.Email == s).First().Id;
                db.Commandes.Add(commande);
                db.SaveChanges();
           Article ar= db.Articles.Where(p => p.NumArticle==commande.NumArticle).First();
            ar.stock -= commande.QteArticle;
            db.SaveChanges();

                return RedirectToAction("Panier");
           
        }

        // GET: Commandes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commande commande = db.Commandes.Find(id);
            if (commande == null)
            {
                return HttpNotFound();
            }
            ViewBag.NumArticle = new SelectList(db.Articles, "NumArticle", "Designation", commande.NumArticle);
            return View(commande);
        }

        // POST: Commandes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumCmd,DateCmd,NumClient,NumArticle,QteArticle")] Commande commande)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commande).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NumArticle = new SelectList(db.Articles, "NumArticle", "Designation", commande.NumArticle);
            return View(commande);
        }

        // GET: Commandes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commande commande = db.Commandes.Find(id);
            if (commande == null)
            {
                return HttpNotFound();
            }
            return View(commande);
        }

        // POST: Commandes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commande commande = db.Commandes.Find(id);
            Article ar = db.Articles.Where(p=>p.NumArticle==commande.NumArticle).First();

            ar.stock += commande.QteArticle;
            db.Commandes.Remove(commande);

            db.SaveChanges();
            return RedirectToAction("Panier");
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
