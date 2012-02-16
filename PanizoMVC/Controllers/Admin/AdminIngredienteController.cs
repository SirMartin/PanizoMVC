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

        //
        // GET: /AdminIngrediente/

        public ViewResult Index()
        {
            var ingredientes = db.Ingredientes.Include("Usuario");
            return View(ingredientes.ToList());
        }

        //
        // GET: /AdminIngrediente/Details/5

        public ViewResult Details(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            return View(ingrediente);
        }

        //
        // GET: /AdminIngrediente/Create

        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email");
            return View();
        } 

        //
        // POST: /AdminIngrediente/Create

        [HttpPost]
        public ActionResult Create(Ingrediente ingrediente)
        {
            if (ModelState.IsValid)
            {
                db.Ingredientes.AddObject(ingrediente);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", ingrediente.IdUsuario);
            return View(ingrediente);
        }
        
        //
        // GET: /AdminIngrediente/Edit/5
 
        public ActionResult Edit(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", ingrediente.IdUsuario);
            return View(ingrediente);
        }

        //
        // POST: /AdminIngrediente/Edit/5

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

        //
        // GET: /AdminIngrediente/Delete/5
 
        public ActionResult Delete(int id)
        {
            Ingrediente ingrediente = db.Ingredientes.Single(i => i.Id == id);
            return View(ingrediente);
        }

        //
        // POST: /AdminIngrediente/Delete/5

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
    }
}