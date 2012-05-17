using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using Facebook;
using System.Web.Routing;
using PanizoMVC.Models.Security;

namespace PanizoMVC.Controllers
{
    public class FacebookController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();

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
                        //Generamos el usuario.
                        Usuario usuario = new Usuario();
                        usuario.FacebookId = facebookId.ToString();
                        usuario.Nick = (string)me.name;
                        usuario.Email = (string)me.email;
                        usuario.FechaCreacion = DateTime.Now;
                        usuario.IsAdmin = false;

                        //Comprobamos si existe en la BBDD como registrado, si no lo creamos.
                        Usuario userdb = (from u in db.Usuarios
                                          where u.FacebookId.Equals(usuario.FacebookId)
                                          select u).FirstOrDefault();
                        if (userdb == null)
                        {
                            //Es un usuario nuevo. Lo guardamos en la BBDD.
                            db.AddToUsuarios(usuario);
                            dbContext.SaveChanges();
                        }

                        //Logueamos al usuario.
                        EntrepanMembershipProvider EntrepanMembership = new EntrepanMembershipProvider();
                        EntrepanMembership.LogInUser(usuario.FacebookId, usuario.Id, usuario.Nick, usuario.IsAdmin, false);
                    }

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
