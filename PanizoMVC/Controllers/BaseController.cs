using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC.Models;

namespace PanizoMVC.Controllers
{
    public class BaseController : Controller
    {
        private EntrepanDB db = new EntrepanDB();

        #region Propiedades Globales

        /// <summary>
        /// Para saber si estamos en debug
        /// </summary>
        public Boolean IsDebug
        {
            get
            {
                return Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsDebug"));
            }
        }

        /// <summary>
        /// Nos da el idciudad, si hemos elegido alguna por subdominio.
        /// Si no será null.
        /// </summary>
        public int? IdCiudad
        {
            get
            {
                int? idCiudad = null;
                String server = Request.ServerVariables["SERVER_NAME"];
                if (!server.Equals("localhost"))
                {
                    String nombreCiudad = server.Remove(server.IndexOf(".entrepan.net"));
                    Ciudad ciudad = db.Ciudades.Where(g => g.Nombre.ToLower().Equals(nombreCiudad.ToLower())).FirstOrDefault();
                    if (ciudad != null)
                    {
                        idCiudad = ciudad.Id;
                    }
                }
                return idCiudad;
            }
        }

        #endregion

        #region Para controlar si cumple los permisos.

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if this action has Authorization attribute
            object[] attributes = filterContext.ActionDescriptor.GetCustomAttributes(true);
            bool permisosOk = true;
            int id = 0;

            //Recogemos al usuario logueado.
            if (!HttpContext.User.Identity.Name.Equals(String.Empty))
            {
                String[] arrayIdentity = HttpContext.User.Identity.Name.Split(PanizoMVC.Utilities.Constants.IdentitySeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                id = Convert.ToInt32(arrayIdentity[1]);
            }
            Usuario usuario = db.Usuarios.Where(g => g.Id == id).FirstOrDefault();

            //importante, hay que ir comparando entrando primero en los roles más poderosos            
            if (attributes.Any(a => a is PanizoMVC.Models.Security.AuthorizationAttributes.UserAuthorize))
            {
                if (usuario == null)
                {
                    permisosOk = false;
                }
            }
            else if (attributes.Any(a => a is PanizoMVC.Models.Security.AuthorizationAttributes.AdminAuthorize))
            {

                if (usuario == null)
                {
                    permisosOk = false;
                }
                else
                {
                    permisosOk = usuario.IsAdmin;
                }
            }

            if (permisosOk)
            {
                return;
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult("No tienes permiso");
            }


            base.OnActionExecuting(filterContext);
        }

        #endregion

        #region Otros Metodos

        private static void RaiseErrorSignal(Exception e)
        {
            var context = System.Web.HttpContext.Current;
            //ErrorSignal.FromContext(context).Raise(e, context);
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            RaiseErrorSignal(filterContext.Exception);
            base.OnException(filterContext);
        }

        #endregion
    }
}
