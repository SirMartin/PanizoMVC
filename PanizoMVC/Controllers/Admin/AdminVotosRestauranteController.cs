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
    public class AdminVotosRestauranteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            var votosrestaurantes = db.VotosRestaurantes.Include("Restaurante").Include("Usuario");
            return View(votosrestaurantes.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            return View(votosrestaurante);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email");
            return View();
        }

        [HttpPost]
        public ActionResult Create(VotosRestaurante votosrestaurante)
        {
            votosrestaurante.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.VotosRestaurantes.AddObject(votosrestaurante);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }

        [HttpPost]
        public ActionResult Edit(VotosRestaurante votosrestaurante)
        {
            if (ModelState.IsValid)
            {
                db.VotosRestaurantes.Attach(votosrestaurante);
                db.ObjectStateManager.ChangeObjectState(votosrestaurante, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            return View(votosrestaurante);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            db.VotosRestaurantes.DeleteObject(votosrestaurante);
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