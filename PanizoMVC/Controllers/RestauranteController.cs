using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC;
using PanizoMVC.Models;

namespace PanizoMVC.Controllers
{
    public class RestauranteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        //
        // GET: /Restaurante/

        public ViewResult Index()
        {
            //Creamos un modelo de columna.
            ColumnModel col1 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/1",
                Titulo = "Añade tu restaurante",
                Texto = "Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. ultricies eget, tempor sit amet, ante. Mauris placerat eleifend leo.",
                TextoAbajo = "Añadir Restaurante",
                Action = "Create",
                Controller = "Restaurante"
            };

            ViewBag.Column1 = col1;

            //Creamos un modelo de columna.
            ColumnModel col2 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/3",
                Titulo = "Añade tu restaurante",
                Texto = "Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. ultricies eget, tempor sit amet, ante. Mauris placerat eleifend leo.",
                TextoAbajo = "Ver los mas valorados",
                Action = "Valorados",
                Controller = "Restaurante"
            };

            ViewBag.Column2 = col2;

            //Creamos un modelo de columna.
            ColumnModel col3 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/7",
                Titulo = "Los últimos en llegar",
                Texto = "Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. ultricies eget, tempor sit amet, ante. Mauris placerat eleifend leo.",
                TextoAbajo = "Nuevos Restaurantes",
                Action = "Ultimos",
                Controller = "Restaurante"
            };

            ViewBag.Column3 = col3;

            var restaurantes = db.Restaurantes.Include("Ciudad");
            return View(restaurantes.ToList());
        }

        //
        // GET: /Restaurante/Details/5

        public ViewResult Details(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            return View(restaurante);
        }

        //
        // GET: /Restaurante/Create

        public ActionResult Create()
        {
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre");
            return View();
        } 

        //
        // POST: /Restaurante/Create

        [HttpPost]
        public ActionResult Create(Restaurante restaurante)
        {
            //Añadimos la ciudad y la fecha de creación.
            restaurante.FechaCreacion = DateTime.Now;
            restaurante.IdCiudad = 1;
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
        // GET: /Restaurante/Edit/5
 
        public ActionResult Edit(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }

        //
        // POST: /Restaurante/Edit/5

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
        // GET: /Restaurante/Delete/5
 
        public ActionResult Delete(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            return View(restaurante);
        }

        //
        // POST: /Restaurante/Delete/5

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