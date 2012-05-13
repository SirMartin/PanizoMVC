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

            List<String> lis = new List<string>();
            foreach (Ingrediente item in ingredientes)
            {
                lis.Add(item.Nombre);
            }

            //Devolvemos el resultado por JSON.
            return Json(lis, JsonRequestBehavior.AllowGet);
        }
    }
}