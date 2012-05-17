using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PanizoMVC.Utilities;
using System.Configuration;
using System.Web.Routing;
using System.Web.Security;
using PanizoMVC.Models.Security;

namespace PanizoMVC.Controllers
{
    public class TwitterController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

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

            //Realizamos la busqueda del usuario.
            Usuario usuario = (from u in db.Usuarios
                               where u.TwitterUserId.Equals(tokens.UserId.ToString())
                               select u).FirstOrDefault();

            if (usuario == null)
            {
                usuario = new Usuario()
                {
                    Nick = tokens.ScreenName,
                    TwitterUserId = tokens.UserId.ToString(),
                    TwitterAccessKey = tokens.Token,
                    TwitterAccessSecret = tokens.TokenSecret,
                    FechaCreacion = DateTime.UtcNow,
                    IsAdmin = false
                };

                db.AddToUsuarios(usuario);

                db.SaveChanges();
            }

            //Logueamos al usuario.
            EntrepanMembershipProvider EntrepanMembership = new EntrepanMembershipProvider();
            EntrepanMembership.LogInUser(usuario.TwitterUserId, usuario.Id, usuario.Nick, usuario.IsAdmin, false);


            if (string.IsNullOrEmpty(ReturnUrl))
                return Redirect("/");
            else
                return Redirect(ReturnUrl);
        }
    }
}
