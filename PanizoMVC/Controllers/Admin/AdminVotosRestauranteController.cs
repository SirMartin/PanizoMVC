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
    public class AdminVotosRestauranteController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

        //
        // GET: /AdminVotosRestaurante/

        public ViewResult Index()
        {
            var votosrestaurantes = db.VotosRestaurantes.Include("Restaurante").Include("Usuario");
            return View(votosrestaurantes.ToList());
        }

        //
        // GET: /AdminVotosRestaurante/Details/5

        public ViewResult Details(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            return View(votosrestaurante);
        }

        //
        // GET: /AdminVotosRestaurante/Create

        public ActionResult Create()
        {
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email");
            return View();
        } 

        //
        // POST: /AdminVotosRestaurante/Create

        [HttpPost]
        public ActionResult Create(VotosRestaurante votosrestaurante)
        {
            if (ModelState.IsValid)
            {
                db.VotosRestaurantes.AddObject(votosrestaurante);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }
        
        //
        // GET: /AdminVotosRestaurante/Edit/5
 
        public ActionResult Edit(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }

        //
        // POST: /AdminVotosRestaurante/Edit/5

        [HttpPost]
        public ActionResult Edit(VotosRestaurante votosrestaurante)
        {
            if (ModelState.IsValid)
            {
                db.VotosRestaurantes.Attach(votosrestaurante);
                db.ObjectStateManager.ChangeObjectState(votosrestaurante, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRestaurante = new SelectList(db.Restaurantes, "Id", "Nombre", votosrestaurante.IdRestaurante);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Email", votosrestaurante.IdUsuario);
            return View(votosrestaurante);
        }

        //
        // GET: /AdminVotosRestaurante/Delete/5
 
        public ActionResult Delete(int id)
        {
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            return View(votosrestaurante);
        }

        //
        // POST: /AdminVotosRestaurante/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            VotosRestaurante votosrestaurante = db.VotosRestaurantes.Single(v => v.Id == id);
            db.VotosRestaurantes.DeleteObject(votosrestaurante);
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