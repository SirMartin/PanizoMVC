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
    public class AdminRestauranteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        //
        // GET: /AdminRestaurante/

        public ViewResult Index()
        {
            var restaurantes = db.Restaurantes.Include("Ciudad");
            return View(restaurantes.ToList());
        }

        //
        // GET: /AdminRestaurante/Details/5

        public ViewResult Details(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            return View(restaurante);
        }

        //
        // GET: /AdminRestaurante/Create

        public ActionResult Create()
        {
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre");
            return View();
        } 

        //
        // POST: /AdminRestaurante/Create

        [HttpPost]
        public ActionResult Create(Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                db.Restaurantes.AddObject(restaurante);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }
        
        //
        // GET: /AdminRestaurante/Edit/5
 
        public ActionResult Edit(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }

        //
        // POST: /AdminRestaurante/Edit/5

        [HttpPost]
        public ActionResult Edit(Restaurante restaurante)
        {
            if (ModelState.IsValid)
            {
                db.Restaurantes.Attach(restaurante);
                db.ObjectStateManager.ChangeObjectState(restaurante, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }

        //
        // GET: /AdminRestaurante/Delete/5
 
        public ActionResult Delete(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            return View(restaurante);
        }

        //
        // POST: /AdminRestaurante/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            db.Restaurantes.DeleteObject(restaurante);
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