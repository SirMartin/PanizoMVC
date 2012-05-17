using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC;
using PanizoMVC.Utilities;
using PanizoMVC.Models.Security;
using PanizoMVC.Models;

namespace PanizoMVC.Controllers
{
    public class BocadilloController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            //Cogemos los bocadillos del restaurante.
            List<Bocadillo> bocadillos = db.Bocadillos.Include("Restaurante").ToList();

            //Los motramos en la vista.
            return View(bocadillos);
        }

        #endregion

        #region Carta

        public ViewResult Carta(int idRestaurante)
        {
            //Cogemos los bocadillos del restaurante.
            List<Bocadillo> bocadillos = db.Bocadillos.Include("Restaurante").Where(g => g.IdRestaurante == idRestaurante).ToList();

            //Los motramos en la vista.
            return View(bocadillos);
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            return View(bocadillo);
        }

        #endregion

        #region Create

        [AuthorizationAttributes.UserAuthorize]
        public ActionResult Create(int idRestaurante)
        {
            ViewData["IdRestaurante"] = idRestaurante;
            return View();
        }

        [HttpPost]
        [AuthorizationAttributes.UserAuthorize]
        public ActionResult Create(Bocadillo bocadillo, FormCollection collection)
        {
            //Rellenamos el idRestaurante.
            bocadillo.IdRestaurante = Convert.ToInt32(collection["IdRestaurante"]);
            //Añadimos la fecha actual como creación.
            bocadillo.FechaCreacion = DateTime.Now;

            //Comprobamos si el modelo es valido y lo guardamos.
            if (ModelState.IsValid)
            {
                db.Bocadillos.AddObject(bocadillo);
                db.SaveChanges();

                //Recuperamos los ingredientes que se han puesto.
                String ingredientesStr = collection["hdnTags"];
                //Lo separamos los ingredientes, quitando antes la coma del final.
                ingredientesStr = ingredientesStr.Substring(0, ingredientesStr.Length - 1);
                List<String> ingredientesList = ingredientesStr.Split(',').ToList();
                //Hacemos el manejo necesario con los ingredientes.
                IngredientesUtilities ingreUtilities = new IngredientesUtilities();
                List<BocadilloIngrediente> ingredientesBocadillo = ingreUtilities.ManageIngredientes(ingredientesList, bocadillo.Id);

                //Agregamos los ingredientes al bocadillo.
                foreach (BocadilloIngrediente item in ingredientesBocadillo)
                {
                    bocadillo.BocadilloIngrediente.Add(item);
                }
                db.SaveChanges();

                //Añadimos el mensaje informando.
                Message msg = new Message()
                {
                    type = TypeMessage.Create,
                    text = String.Format(Resources.Mensajes.txtRestaurantAdded, bocadillo.Nombre, bocadillo.Restaurante.Nombre)
                };
                ViewBag.Message = msg;


                return RedirectToAction("Carta", new { idRestaurante = bocadillo.IdRestaurante });
            }

            //Si hay algún fallo volvemos.
            ViewData["IdRestaurante"] = collection["IdRestaurante"];
            return View(bocadillo);
        }

        #endregion

        #region Edit

        [AuthorizationAttributes.AdminAuthorize]
        public ActionResult Edit(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", bocadillo.IdRestaurante);
            return View(bocadillo);
        }

        //
        // POST: /Bocadillo/Edit/5

        [HttpPost]
        [AuthorizationAttributes.AdminAuthorize]
        public ActionResult Edit(Bocadillo bocadillo)
        {
            if (ModelState.IsValid)
            {
                db.Bocadillos.Attach(bocadillo);
                db.ObjectStateManager.ChangeObjectState(bocadillo, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", bocadillo.IdRestaurante);
            return View(bocadillo);
        }

        #endregion

        #region Delete

        [AuthorizationAttributes.AdminAuthorize]
        public ActionResult Delete(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            return View(bocadillo);
        }

        //
        // POST: /Bocadillo/Delete/5

        [HttpPost, ActionName("Delete")]
        [AuthorizationAttributes.AdminAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Bocadillo bocadillo = db.Bocadillos.Single(b => b.Id == id);
            db.Bocadillos.DeleteObject(bocadillo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Ultimos

        public ActionResult Ultimos(int idRestaurante)
        {
            //Recogemos los 20 restaurantes mas nuevos.
            List<Bocadillo> bocadillos = db.Bocadillos.Where(g => g.IdRestaurante == idRestaurante).OrderByDescending(g => g.FechaCreacion).Take(20).ToList();

            //Los pasamos como modelo.
            ViewData.Model = bocadillos;

            //Pasamos a la vista.
            return View();
        }

        #endregion

        #region Valorados

        public ActionResult Valorados(int idRestaurante)
        {
            //Recogemos todos los bocadillos.
            List<Bocadillo> bocadillos = db.Bocadillos.Where(g => g.IdRestaurante == idRestaurante).ToList();

            //Los pasamos como modelo.
            ViewData.Model = bocadillos;

            //Pasamos a la vista.
            return View();
        }

        #endregion
    }
}