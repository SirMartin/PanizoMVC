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
    public class AdminUsuarioController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            usuario.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Usuarios.AddObject(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(usuario);
        }

        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Attach(usuario);
                db.ObjectStateManager.ChangeObjectState(usuario, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            db.Usuarios.DeleteObject(usuario);
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