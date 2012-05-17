using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PanizoMVC.Models;
using PanizoMVC.Utilities;
using PanizoMVC.Models.Security;

namespace PanizoMVC.Controllers
{
    public class AccountController : BaseController
    {
        private EntrepanDB db = new EntrepanDB();
        EntrepanMembershipProvider EntrepanMembership = new EntrepanMembershipProvider();

        #region Logon

        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (EntrepanMembership.ValidateUser(model.Email, model.Password))
                {
                    Usuario user = EntrepanMembership.GetUserByEmail(model.Email);
                    EntrepanMembership.LogInUser(model.Email, user.Id, user.Nick, user.IsAdmin, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El nombre de usuario o la contraseña es incorrecto.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = EntrepanMembership.CreateUser(model.Email, model.Password, model.Nick);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    Usuario user = EntrepanMembership.GetUserByEmail(model.Email);
                    EntrepanMembership.LogInUser(model.Email, user.Id, user.Nick, user.IsAdmin, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #endregion

        #region Status Codes

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario existe. Por favor introduzca un nombre de usuario distinto.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe una cuenta con esa dirección de e-mail. Por favor introduzca una dirección de e-mail distinta.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña introducida es incorrecta. Por favor introduzca una contraseña correcta.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de e-mail introducida es incorrecta. Por favor compruebela y corríjala.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta a la pregunta para recuperar la contraseña es incorrecta. Por favor compruebela y corríjala.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta suministrada para recuperar su contraseña no es correcta. Por favor compruebela y corríjala.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario elegido no es correcto. Por favor compruebelo y corríjalo.";

                case MembershipCreateStatus.ProviderError:
                    return "El sistema de autenticación ha devuelto un error. Por favor verifique su información y vuelva a probar. Si el problema persiste, por favor contacte con el administrador.";

                case MembershipCreateStatus.UserRejected:
                    return "La creación del usuario ha sido cancelada. Por favor verifique su información y vuelva a probar. Si el problema persiste, por favor contacte con el administrador.";

                default:
                    return "Ha ocurrido un error desconocido. Por favor verifique su información y vuelva a probar. Si el problema persiste, por favor contacte con el administrador.";
            }
        }

        #endregion
    }
}
