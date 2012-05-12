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

        #region Index

        public ViewResult Index(int idRestaurante)
        {
            //Cogemos los bocadillos del restaurante.
            List<Bocadillo> bocadillos = db.Bocadillos.Include("Restaurante").Where(g=> g.IdRestaurante == idRestaurante).ToList();

            //Los motramos en la vista.
            return View(bocadillos);
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            return View(bocadillo);
        }

        #endregion

        #region Create

        public ActionResult Create(int idRestaurante)
        {
            ViewBag.IdRestaurante = idRestaurante;
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

        #endregion

        #region Edit

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

        #endregion

        #region Delete

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

        #endregion

        #region Ultimos

        public ActionResult Ultimos(int idRestaurante)
        {
            //Recogemos los 20 restaurantes mas nuevos.
            List<Bocadillo> bocadillos = db.Bocadillos.Where(g => g.IdRestaurante == idRestaurante).OrderByDescending(g => g.FechaCreacion).Take(20).ToList();

            //Los pasamos como modelo.
            ViewData.Model = bocadillos;

            //Pasamos a la vista.
            return View();
        }

        #endregion

        #region Valorados

        public ActionResult Valorados(int idRestaurante)
        {
            //Recogemos todos los bocadillos.
            List<Bocadillo> bocadillos = db.Bocadillos.Where(g => g.IdRestaurante == idRestaurante).ToList();

            //Los pasamos como modelo.
            ViewData.Model = bocadillos;

            //Pasamos a la vista.
            return View();
        }

        #endregion
    }
}