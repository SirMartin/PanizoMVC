using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC.Utilities;
using System.Configuration;
using System.Web.Routing;
using PanizoMVC.Interfaces;
using PanizoMVC.Repositorys;
using System.Web.Security;

namespace PanizoMVC.Controllers
{
    public class TwitterController : BaseController
    {
        #region Repositorios

        public IUsuarioRep usuarioRepository { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (usuarioRepository == null)
            {
                usuarioRepository = new UsuarioRepository();
            }

            base.Initialize(requestContext);
        }

        #endregion

        public ActionResult Index()
        {
            String url = "http://twitter.com/oauth/request_token";

            return View();
        }

        public ActionResult CallBack(string oauth_token, string oauth_verifier, string ReturnUrl)
        {
            if (string.IsNullOrEmpty(oauth_token) || string.IsNullOrEmpty(oauth_verifier))
            {
                UriBuilder builder = new UriBuilder(this.Request.Url);
                builder.Query = string.Concat(
                    builder.Query,
                    string.IsNullOrEmpty(builder.Query) ? string.Empty : "&",
                    "ReturnUrl=",
                    ReturnUrl);

                string token = OAuthUtility.GetRequestToken(
                    ConfigurationManager.AppSettings["TwitterConsumerKey"],
                    ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                    builder.ToString()).Token;

                return Redirect(OAuthUtility.BuildAuthorizationUri(token, true).ToString());
            }

            var tokens = OAuthUtility.GetAccessToken(
        ConfigurationManager.AppSettings["TwitterConsumerKey"],
        ConfigurationManager.AppSettings["TwitterConsumerSecret"],
        oauth_token,
        oauth_verifier);

            using (EntrepanDB dbContext = GetNewDBContext())
            {
                //Asignamos el context.
                usuarioRepository.DBContext = dbContext;

                //Realizamos la busqueda del usuario.
                Usuario usuario = usuarioRepository.GetUsuarioByTwitterUserId(tokens.UserId.ToString());
                if (usuario == null)
                {
                    usuario = new Usuario()
                    {
                        Nick = tokens.ScreenName,
                        TwitterUserId = tokens.UserId.ToString(),
                        TwitterAccessKey = tokens.Token,
                        TwitterAccessSecret = tokens.TokenSecret,
                        FechaCreacion = DateTime.UtcNow
                    };

                    usuarioRepository.AddUsuario(usuario);

                    dbContext.SaveChanges();
                }

                FormsAuthentication.SetAuthCookie(usuario.Nick, false);
            }

            if (string.IsNullOrEmpty(ReturnUrl))
                return Redirect("/");
            else
                return Redirect(ReturnUrl);
        }
    }
}
