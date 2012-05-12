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
    public class IngredientesController : Controller
    {
        private EntrepanDB db = new EntrepanDB();

        public ActionResult Index()
        {
            //Recuperamos de BBDD los ingredientes.
            List<Ingrediente> ingredientes = db.Ingredientes.ToList();

            List<TagIngrediente> lista = new List<TagIngrediente>();
            foreach (Ingrediente item in ingredientes)
            {
                TagIngrediente tag = new TagIngrediente()
                {
                    label = item.Nombre,
                    value = item.Descripcion
                };
                lista.Add(tag);
            }
            String con = "hola\ncosa\npedo\n\noceh";
            //Devolvemos el resultado por JSON.
            //return Json(con, JsonRequestBehavior.AllowGet);
            return Content(con);
        }
    }
}