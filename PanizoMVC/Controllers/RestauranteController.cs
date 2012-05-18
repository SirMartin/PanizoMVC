using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC;
using PanizoMVC.Models;
using PanizoMVC.Models.Security;

namespace PanizoMVC.Controllers
{
    public class RestauranteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        #region Index

        public ViewResult Index()
        {
            #region Añadimos las columnas.

            ColumnModel[] columns = GetColumnsForRestaurant();
            ViewBag.Column1 = columns[0];
            ViewBag.Column2 = columns[1];
            ViewBag.Column3 = columns[2];

            #endregion

            var restaurantes = db.Restaurantes.Include("Ciudad");

            return View(restaurantes.ToList());
        }

        #endregion

        #region Details

        public ViewResult Details(int id)
        {
            //Cogemos la información del restaurante.
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);

            //Cogemos los votos del restaurante.
            List<VotosRestaurante> votos = db.VotosRestaurantes.Where(g => g.IdRestaurante == id).ToList();
            int sumaVotos = 0;
            votos.ForEach(g => sumaVotos += g.Voto);
            //Pasamos los datos de los votos en el viewbag.
            ViewBag.TotalVotosRestaurante = votos.Count;
            ViewBag.VotosRestaurante = sumaVotos;

            //Cogemos el usuario logueado.
            String[] arrayIdentity = User.Identity.Name.Split(PanizoMVC.Utilities.Constants.IdentitySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int idUsuario = int.Parse(arrayIdentity[1]);

            //Recogemos si el usuario actual tiene voto o no.
            VotosRestaurante voto = votos.Where(g => g.IdUsuario == idUsuario).FirstOrDefault();
            if (voto != null)
            {
                ViewData["VotoActual"] = voto.Voto;
            }
            else
            {
                ViewData["VotoActual"] = 0;
            }

            //Creamos un modelo de columna.
            ColumnModel col1 = new ColumnModel()
            {
                UrlImage = "http://lorempixel.com/282/150/food/1",
                Titulo = "La carta",
                Texto = "Consulta aquí todos los bocadillos disponibles en el restaurante en cuestión.",
                TextoAbajo = "Ver bocadillos",
                Action = "Carta",
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
                Action = "Valorados",
                Controller = "Bocadillo",
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

        [AuthorizationAttributes.UserAuthorize]
        public ActionResult Create()
        {
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [AuthorizationAttributes.UserAuthorize]
        public ActionResult Create(Restaurante restaurante)
        {
            //Añadimos la ciudad y la fecha de creación.
            restaurante.FechaCreacion = DateTime.Now;
            restaurante.IdCiudad = 1;
            if (ModelState.IsValid)
            {
                db.Restaurantes.AddObject(restaurante);
                db.SaveChanges();

                //Añadimos el mensaje informando.
                Message msg = new Message()
                {
                    type = TypeMessage.Create,
                    text = String.Format(Resources.Mensajes.txtRestaurantAdded, restaurante.Nombre)
                };
                ViewBag.Message = msg;

                #region Añadimos las columnas.

                ColumnModel[] columns = GetColumnsForRestaurant();
                ViewBag.Column1 = columns[0];
                ViewBag.Column2 = columns[1];
                ViewBag.Column3 = columns[2];

                #endregion

                return View("Index");
            }

            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }

        #endregion

        #region Edit

        [AuthorizationAttributes.UserAuthorize]
        public ActionResult Edit(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "Id", "Nombre", restaurante.IdCiudad);
            return View(restaurante);
        }

        //
        // POST: /Restaurante/Edit/5

        [HttpPost]
        [AuthorizationAttributes.UserAuthorize]
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

        [AuthorizationAttributes.AdminAuthorize]
        public ActionResult Delete(int id)
        {
            Restaurante restaurante = db.Restaurantes.Single(r => r.Id == id);
            return View(restaurante);
        }

        //
        // POST: /Restaurante/Delete/5

        [HttpPost, ActionName("Delete")]
        [AuthorizationAttributes.AdminAuthorize]
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

        #region Columnas Restaurante

        private ColumnModel[] GetColumnsForRestaurant()
        {
            List<ColumnModel> columns = new List<ColumnModel>();

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

            columns.Add(col1);

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

            columns.Add(col2);

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

            columns.Add(col3);

            return columns.ToArray();
        }

        #endregion
    }
}