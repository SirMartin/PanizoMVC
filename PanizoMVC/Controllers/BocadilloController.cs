using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC;

namespace PanizoMVC.Controllers
{
    public class BocadilloController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        //
        // GET: /Bocadillo/

        public ViewResult Index()
        {
            var bocadillos = db.Bocadillos.Include("Restaurante");
            return View(bocadillos.ToList());
        }

        //
        // GET: /Bocadillo/Details/5

        public ViewResult Details(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            return View(bocadillo);
        }

        //
        // GET: /Bocadillo/Create

        public ActionResult Create()
        {
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre");
            return View();
        } 

        //
        // POST: /Bocadillo/Create

        [HttpPost]
        public ActionResult Create(Bocadillo bocadillo)
        {
            if (ModelState.IsValid)
            {
                db.Bocadillos.AddObject(bocadillo);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", bocadillo.IdRestaurante);
            return View(bocadillo);
        }
        
        //
        // GET: /Bocadillo/Edit/5
 
        public ActionResult Edit(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", bocadillo.IdRestaurante);
            return View(bocadillo);
        }

        //
        // POST: /Bocadillo/Edit/5

        [HttpPost]
        public ActionResult Edit(Bocadillo bocadillo)
        {
            if (ModelState.IsValid)
            {
                db.Bocadillos.Attach(bocadillo);
                db.ObjectStateManager.ChangeObjectState(bocadillo, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", bocadillo.IdRestaurante);
            return View(bocadillo);
        }

        //
        // GET: /Bocadillo/Delete/5
 
        public ActionResult Delete(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            return View(bocadillo);
        }

        //
        // POST: /Bocadillo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            db.Bocadillos.DeleteObject(bocadillo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}