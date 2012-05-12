using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC;

namespace PanizoMVC.Controllers.Admin
{
    public class AdminVotosBocadilloController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            var votosbocadillos = db.VotosBocadillos.Include("Bocadillo").Include("Usuario");
            return View(votosbocadillos.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            VotosBocadillo votosbocadillo = db.VotosBocadillos.Single(v => v.Id == id);
            return View(votosbocadillo);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            ViewBag.IdBocadillo = new SelectList(db.Bocadillos, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email");
            return View();
        }

        [HttpPost]
        public ActionResult Create(VotosBocadillo votosbocadillo)
        {
            votosbocadillo.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.VotosBocadillos.AddObject(votosbocadillo);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdBocadillo = new SelectList(db.Bocadillos, "Id", "Nombre", votosbocadillo.IdBocadillo);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosbocadillo.IdUsuario);
            return View(votosbocadillo);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            VotosBocadillo votosbocadillo = db.VotosBocadillos.Single(v => v.Id == id);
            ViewBag.IdBocadillo = new SelectList(db.Bocadillos, "Id", "Nombre", votosbocadillo.IdBocadillo);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosbocadillo.IdUsuario);
            return View(votosbocadillo);
        }

        [HttpPost]
        public ActionResult Edit(VotosBocadillo votosbocadillo)
        {
            if (ModelState.IsValid)
            {
                db.VotosBocadillos.Attach(votosbocadillo);
                db.ObjectStateManager.ChangeObjectState(votosbocadillo, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBocadillo = new SelectList(db.Bocadillos, "Id", "Nombre", votosbocadillo.IdBocadillo);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosbocadillo.IdUsuario);
            return View(votosbocadillo);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            VotosBocadillo votosbocadillo = db.VotosBocadillos.Single(v => v.Id == id);
            return View(votosbocadillo);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            VotosBocadillo votosbocadillo = db.VotosBocadillos.Single(v => v.Id == id);
            db.VotosBocadillos.DeleteObject(votosbocadillo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}