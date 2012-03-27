using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using Facebook;
using PanizoMVC.Interfaces;
using System.Web.Routing;
using PanizoMVC.Repositorys;

namespace PanizoMVC.Controllers
{
    public class FacebookController : BaseController
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

        private const string logoffUrlDebug = "http://localhost:50649/";
        private const string redirectUrlDebug = "http://localhost:50649/Facebook/OAuth";
        private const string logoffUrl = "http://www.entrepan.net/";
        private const string redirectUrl = "http://www.entrepan.net/Facebook/OAuth";

        public ActionResult LogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            if (IsDebug)
            {
                oAuthClient.RedirectUri = new Uri(redirectUrlDebug);
            }
            else
            {
                oAuthClient.RedirectUri = new Uri(redirectUrl);
            }
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl } });
            return Redirect(loginUri.AbsoluteUri);
        }

        //
        // GET: /Account/OAuth/

        public ActionResult OAuth(string code, string state)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
                    if (IsDebug)
                    {
                        oAuthClient.RedirectUri = new Uri(redirectUrlDebug);
                    }
                    else
                    {
                        oAuthClient.RedirectUri = new Uri(redirectUrl);
                    }
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    DateTime expiresOn = DateTime.MaxValue;

                    if (tokenResult.ContainsKey("expires"))
                    {
                        DateTimeConvertor.FromUnixTime(tokenResult.expires);
                    }

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me?fields=id,name");
                    long facebookId = Convert.ToInt64(me.id);

                    using (EntrepanDB dbContext = GetNewDBContext())
                    {
                        //Asignamos el context.
                        usuarioRepository.DBContext = dbContext;

                        //Generamos el usuario.
                        Usuario usuario = new Usuario();
                        usuario.FacebookId = facebookId.ToString();
                        usuario.Nick = (string)me.name;
                        usuario.Email = (string)me.email;
                        usuario.FechaCreacion = DateTime.Now;

                        //Comprobamos si existe en la BBDD como registrado, si no lo creamos.
                        Usuario userdb = usuarioRepository.GetUsuarioByFacebookUserId(usuario.FacebookId);
                        if (userdb == null)
                        {
                            //Es un usuario nuevo. Lo guardamos en la BBDD.
                            usuarioRepository.AddUsuario(usuario);
                            dbContext.SaveChanges();
                        }
                    }

                    FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);

                    // prevent open redirection attack by checking if the url is local.
                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOff/

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            var oAuthClient = new FacebookOAuthClient();
            if (IsDebug)
            {
                oAuthClient.RedirectUri = new Uri(logoffUrlDebug);
            }
            else
            {
                oAuthClient.RedirectUri = new Uri(logoffUrl);
            }
            //var logoutUrl = oAuthClient.GetLogoutUrl();
            //return Redirect(logoutUrl.AbsoluteUri);
            return Redirect("http://www.google.es");
        }
    }
}
