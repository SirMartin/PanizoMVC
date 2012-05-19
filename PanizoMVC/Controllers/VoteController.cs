using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC.Models.Security;

namespace PanizoMVC.Controllers
{
    public class VoteController : Controller
    {
        private EntrepanDB db = new EntrepanDB();

        #region Realizar voto

        [HttpPost]
        [AuthorizationAttributes.UserAuthorize]
        public JsonResult Index(int voto, String url)
        {
            String votosJson = "";
            //Diferenciamos si es un voto de restaurante o de bocadillo.
            if (url.Contains("Restaurante"))
            {
                //Es de un restaurante.
                String control = "Restaurante/Details/";
                int index = url.IndexOf(control);
                int idRestaurante = int.Parse(url.Substring(index + control.Length));
                //Cogemos el usuario logueado.
                String[] arrayIdentity = User.Identity.Name.Split(PanizoMVC.Utilities.Constants.IdentitySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int idUsuario = int.Parse(arrayIdentity[1]);
                //Comprobamos si existe ese voto en la BBDD.
                VotosRestaurante votosRestaurante = db.VotosRestaurantes.Where(g => g.IdRestaurante == idRestaurante && g.IdUsuario == idUsuario).FirstOrDefault();
                if (votosRestaurante == null)
                {
                    //Añadimos el voto a la BBDD.
                    votosRestaurante = new VotosRestaurante()
                    {
                        FechaCreacion = DateTime.Now,
                        IdRestaurante = idRestaurante,
                        IdUsuario = idUsuario,
                        Voto = voto,
                        Comentario = String.Empty
                    };
                    db.AddToVotosRestaurantes(votosRestaurante);
                }
                else
                {
                    //Modificamos el voto.
                    votosRestaurante.Voto = voto;
                }

                //Guardamos los cambios.
                db.SaveChanges();

                //Cogemos los votos del restaurante.
                List<VotosRestaurante> votos = db.VotosRestaurantes.Where(g => g.IdRestaurante == idRestaurante).ToList();
                if (votos.Count > 0)
                {
                    decimal sumaVotos = 0;
                    votos.ForEach(g => sumaVotos += g.Voto);
                    //Pasamos los datos al string para mandarlo por json.
                    votosJson = Math.Round((sumaVotos / votos.Count), 2) + "_" + votos.Count;
                }
            }
            else if (url.Contains("Bocadillo"))
            {
                //Es de un bocadillo.
                String control = "Bocadillo/Details/";
                int index = url.IndexOf(control);
                int idBocadillo = int.Parse(url.Substring(index + control.Length));
                //Cogemos el usuario logueado.
                String[] arrayIdentity = User.Identity.Name.Split(PanizoMVC.Utilities.Constants.IdentitySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int idUsuario = int.Parse(arrayIdentity[1]);
                //Comprobamos si existe ese voto en la BBDD.
                VotosBocadillo votosBocadillo = db.VotosBocadillos.Where(g => g.IdBocadillo == idBocadillo && g.IdUsuario == idUsuario).FirstOrDefault();
                if (votosBocadillo == null)
                {
                    //Añadimos el voto a la BBDD.
                    votosBocadillo = new VotosBocadillo()
                    {
                        FechaCreacion = DateTime.Now,
                        IdBocadillo = idBocadillo,
                        IdUsuario = idUsuario,
                        Voto = voto,
                        Comentario = String.Empty
                    };
                    db.AddToVotosBocadillos(votosBocadillo);
                }
                else
                {
                    //Modificamos el voto.
                    votosBocadillo.Voto = voto;
                }

                //Guardamos los cambios.
                db.SaveChanges();

                //Cogemos los votos del bocadillo.
                List<VotosBocadillo> votos = db.VotosBocadillos.Where(g => g.IdBocadillo == idBocadillo).ToList();
                if (votos.Count > 0)
                {
                    decimal sumaVotos = 0;
                    votos.ForEach(g => sumaVotos += g.Voto);
                    //Pasamos los datos al string para mandarlo por json.
                    votosJson = Math.Round((sumaVotos / votos.Count), 2) + "_" + votos.Count;
                }
            }

            //Devolvemos los nuevos votos.
            return Json(votosJson);
        }

        #endregion
    }
}
