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

        #region Index

        public ViewResult Index()
        {
            //Creamos un modelo de columna.
            ColumnModel col1 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/1",
                Titulo = "Añade tu restaurante",
                Texto = "Has descubierto un nuevo restaurante que quieres compartir con todos nosotros, hazlo desde aquí.",
                TextoAbajo = "Añadir Restaurante",
                Action = "Create",
                Controller = "Restaurante"
            };

            ViewBag.Column1 = col1;

            //Creamos un modelo de columna.
            ColumnModel col2 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/3",
                Titulo = "Los + Valorados",
                Texto = "Aquí encontraras el top de restaurantes según vuestros propios votos. No olvides votar a tus favoritos.",
                TextoAbajo = "Ver los mas valorados",
                //Action = "Valorados",
                //Controller = "Restaurante"
                Action = "Index",
                Controller = "AdminRestaurante"
            };

            ViewBag.Column2 = col2;

            //Creamos un modelo de columna.
            ColumnModel col3 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/7",
                Titulo = "Los últimos en llegar",
                Texto = "Quieres ver los últimos restaurante que la gente ha descubierto. Aquí puedes ver las últimas novedades en entrepan.",
                TextoAbajo = "Nuevos Restaurantes",
                Action = "Ultimos",
                Controller = "Restaurante"
            };

            ViewBag.Column3 = col3;

            var restaurantes = db.Restaurantes.Include("Ciudad");
            return View(restaurantes.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            //Cogemos la información del restaurante.
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);

            //Creamos un modelo de columna.
            ColumnModel col1 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/1",
                Titulo = "La carta",
                Texto = "Consulta aquí todos los bocadillos disponibles en el restaurante en cuestión.",
                TextoAbajo = "Ver bocadillos",
                Action = "Index",
                Controller = "Bocadillo",
                Parameters = new System.Web.Routing.RouteValueDictionary(new { IdRestaurante = id })
            };

            ViewBag.Column1 = col1;

            //Creamos un modelo de columna.
            ColumnModel col2 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/3",
                Titulo = "Los + Valorados",
                Texto = "Aquí encontraras el top de los bocadillos del restaurante. Las especialidades del sitio.",
                TextoAbajo = "Ver los mas valorados",
                //Action = "Valorados",
                //Controller = "Bocadillo"
                Action = "Index",
                Controller = "AdminBocadillo",
                Parameters = new System.Web.Routing.RouteValueDictionary(new { IdRestaurante = id })
            };

            ViewBag.Column2 = col2;

            //Creamos un modelo de columna.
            ColumnModel col3 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/7",
                Titulo = "Añadir Bocadillo",
                Texto = "Has estado por aquí y has probado un bocadillo del que nadie ha hablado, es tu oportunidad.",
                TextoAbajo = "Bocata nuevo",
                Action = "Create",
                Controller = "Bocadillo",
                Parameters = new System.Web.Routing.RouteValueDictionary(new { IdRestaurante = id })
            };

            ViewBag.Column3 = col3;

            //Mostramos la vista.
            return View(restaurante);
        }

        #endregion

        #region Create

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

        #endregion

        #region Edit

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

        #endregion

        #region Delete

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

        #endregion

        #region Todos

        public ActionResult Todos()
        {
            //Recogemos todos los restaurantes.
            List<Restaurante> restaurantes = db.Restaurantes.ToList();

            //Los pasamos como modelo.
            ViewData.Model = restaurantes;

            //Pasamos a la vista.
            return View();
        }

        #endregion

        #region Ultimos

        public ActionResult Ultimos()
        {
            //Recogemos los 20 restaurantes mas nuevos.
            List<Restaurante> restaurantes = db.Restaurantes.OrderByDescending(g => g.FechaCreacion).Take(20).ToList();

            //Los pasamos como modelo.
            ViewData.Model = restaurantes;

            //Pasamos a la vista.
            return View();
        }

        #endregion
    }
}