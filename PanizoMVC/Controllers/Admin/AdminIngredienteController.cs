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
    public class AdminIngredienteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            var ingredientes = db.Ingredientes.Include("Usuario");
            return View(ingredientes.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            return View(ingrediente);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Ingrediente ingrediente)
        {
            ingrediente.IdUsuario = 7;
            //Añadimos la fecha de creación.
            ingrediente.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Ingredientes.AddObject(ingrediente);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", ingrediente.IdUsuario);
            return View(ingrediente);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", ingrediente.IdUsuario);
            return View(ingrediente);
        }

        [HttpPost]
        public ActionResult Edit(Ingrediente ingrediente)
        {
            if (ModelState.IsValid)
            {
                db.Ingredientes.Attach(ingrediente);
                db.ObjectStateManager.ChangeObjectState(ingrediente, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", ingrediente.IdUsuario);
            return View(ingrediente);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            return View(ingrediente);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            db.Ingredientes.DeleteObject(ingrediente);
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