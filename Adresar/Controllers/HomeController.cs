using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Adresar.Models;

namespace Adresar.Controllers
{
    public class HomeController : Controller
    {
        private AdresarContext db = new AdresarContext();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Pretraga
        public ActionResult Pretraga()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pretraga(string pretraga)
        {
            var kontakti = from k in db.Kontakti
                            select k;

            if (!String.IsNullOrEmpty(pretraga))
            {
                kontakti = kontakti.Where(s => s.Ime.Contains(pretraga) 
                                                || s.Prezime.Contains(pretraga)
                                                || s.Opis.Contains(pretraga)
                                                || s.Grad.Contains(pretraga)
                                                || s.Brojevi.Any(b=> b.Broj.Contains(pretraga)));
            }

            return View(kontakti.ToList()); 
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KontaktViewModel kontaktVM)
        {
            if (ModelState.IsValid)
            {
                kontaktVM.Kontakt.Brojevi = new List<Unos>();
                kontaktVM.Kontakt.Brojevi.Add(kontaktVM.Unos);
                db.Kontakti.Add(kontaktVM.Kontakt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kontaktVM);
        }

        public JsonResult Detalji(int? str)
        {
            var sviUnosi = new List<Kontakt>();
            if (str.HasValue)
            {
                int preskoci = str.Value * 10;
                sviUnosi = db.Kontakti.Include(k => k.Brojevi).OrderBy(m => m.Prezime)
                                                     .Skip(preskoci).Take(10).ToList();
                if(sviUnosi==null)
                {
                    sviUnosi = db.Kontakti.Include(k => k.Brojevi).OrderBy(m => m.Prezime)
                                                                        .Take(10).ToList();
                }
            }
            else sviUnosi = db.Kontakti.Include(k => k.Brojevi).OrderBy(m => m.Prezime)
                                                                        .Take(10).ToList();

            return Json(
                sviUnosi.Select(x => new
                {
                    id = x.ID,
                    ime = x.Ime,
                    prezime = x.Prezime,
                    grad = x.Grad,
                    opis = x.Opis,
                    slika = x.Slika,
                    brojevi = x.Brojevi.Select(y => new
                    {
                        broj = y.Broj,
                        tip = y.Tip.ToString(),
                        opisBroja = y.Opis
                    }),
                    kolicina = sviUnosi.Count
                }
                    )
            , JsonRequestBehavior.AllowGet);

        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kontakt kontakt = db.Kontakti.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            return View(kontakt);
        }

        

       // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktUnosViewModel kontaktVM = new KontaktUnosViewModel()
            {
                Kontakt = db.Kontakti.Find(id),
                Unosi = db.Brojevi.Where(u => u.KontaktId == id).ToList(),
                PrazanUnos = new Unos() { KontaktId = id.Value}
            };
            if (kontaktVM.Kontakt == null)
            {
                return HttpNotFound();
            }
            return View(kontaktVM);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,Grad,Slika,Opis")] Kontakt kontakt, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0 && image.FileName.Contains(".png"))
                {
                    string newFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
                    image.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Images"), newFileName));
                    kontakt.Slika = "/Images/" + newFileName;
                }

                db.Entry(kontakt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kontakt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajBroj(Unos unos)
        {
            if (ModelState.IsValid)
            {
                db.Brojevi.Add(unos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

      // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kontakt kontakt = db.Kontakti.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            return View(kontakt);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kontakt kontakt = db.Kontakti.Find(id);
            db.Kontakti.Remove(kontakt);
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
