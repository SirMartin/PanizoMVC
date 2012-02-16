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

        //
        // GET: /AdminUsuario/

        public ViewResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        //
        // GET: /AdminUsuario/Details/5

        public ViewResult Details(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        //
        // GET: /AdminUsuario/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /AdminUsuario/Create

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.AddObject(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(usuario);
        }
        
        //
        // GET: /AdminUsuario/Edit/5
 
        public ActionResult Edit(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        //
        // POST: /AdminUsuario/Edit/5

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

        //
        // GET: /AdminUsuario/Delete/5
 
        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuarios.Single(u => u.Id == id);
            return View(usuario);
        }

        //
        // POST: /AdminUsuario/Delete/5

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
    }
}