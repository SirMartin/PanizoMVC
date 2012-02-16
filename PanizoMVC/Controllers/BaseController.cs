using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PanizoMVC.Controllers
{
    public class BaseController : Controller
    {
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

        #endregion

        protected EntrepanDB GetNewDBContext()
        {
            EntrepanDB db = new EntrepanDB();
            db.ContextOptions.LazyLoadingEnabled = false;
            return db;
        }

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

    }
}
