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
    public class AdminCiudadController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        //
        // GET: /AdminCiudad/

        public ViewResult Index()
        {
            return View(db.Ciudades.ToList());
        }

        //
        // GET: /AdminCiudad/Details/5

        public ViewResult Details(int id)
        {
            Ciudad ciudad = db.Ciudades.Single(c => c.Id == id);
            return View(ciudad);
        }

        //
        // GET: /AdminCiudad/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /AdminCiudad/Create

        [HttpPost]
        public ActionResult Create(Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                db.Ciudades.AddObject(ciudad);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(ciudad);
        }
        
        //
        // GET: /AdminCiudad/Edit/5
 
        public ActionResult Edit(int id)
        {
            Ciudad ciudad = db.Ciudades.Single(c => c.Id == id);
            return View(ciudad);
        }

        //
        // POST: /AdminCiudad/Edit/5

        [HttpPost]
        public ActionResult Edit(Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                db.Ciudades.Attach(ciudad);
                db.ObjectStateManager.ChangeObjectState(ciudad, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ciudad);
        }

        //
        // GET: /AdminCiudad/Delete/5
 
        public ActionResult Delete(int id)
        {
            Ciudad ciudad = db.Ciudades.Single(c => c.Id == id);
            return View(ciudad);
        }

        //
        // POST: /AdminCiudad/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Ciudad ciudad = db.Ciudades.Single(c => c.Id == id);
            db.Ciudades.DeleteObject(ciudad);
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