using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PanizoMVC.Utilities;

namespace PanizoMVC.Models.Security
{
    public class EntrepanMembershipProvider : System.Web.Security.MembershipProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public System.Web.Security.MembershipCreateStatus CreateUser(string email, string password, string nick)
        {
            EntrepanDB db = new EntrepanDB();

            Usuario user = new Usuario()
            {
                Email = email,
                Nick = nick,
                Password = password,
                FechaCreacion = DateTime.Now,
                IsAdmin = false
            };

            //Comprobamos que el email este libre.
            var existEmail = (from u in db.Usuarios
                              where u.Email == email
                              select u).FirstOrDefault();

            if ((existEmail != null))
            {
                //Existe el email.
                return System.Web.Security.MembershipCreateStatus.DuplicateEmail;
            }

            try
            {
                //Intentamos guardar el usuario.
                db.Usuarios.AddObject(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return System.Web.Security.MembershipCreateStatus.UserRejected;
            }

            //Devolvemos que se ha guardado correctamente.
            return System.Web.Security.MembershipCreateStatus.Success;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public Usuario GetUserByEmail(String email)
        {
            EntrepanDB db = new EntrepanDB();
            Usuario user = (from u in db.Usuarios
                                         where u.Email == email
                                         select u).FirstOrDefault();

            return user;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Buscamos usuarios que coincidan en email y password y que esten validados por el admin.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            EntrepanDB db = new EntrepanDB();
            var user = (from u in db.Usuarios
                        where u.Email == username && u.Password == password
                        select u).FirstOrDefault();

            return (user != null);
        }

        /// <summary>
        /// Logueamos el usuario para Entrepan.
        /// </summary>
        /// <param name="identificadorUnico">El email del usuario, o el ID de facebook o el de Twitter.</param>
        /// <param name="id">El ID del usuario en la BBDD.</param>
        /// <param name="nick">El nombre del usuario.</param>
        /// <param name="isAdmin">Si es administrador o no.</param>
        /// <param name="createPersistentCookie">Si creamos la cookie que permanezca o no.</param>
        public void LogInUser(String identificadorUnico, int id, String nick, bool isAdmin, bool createPersistentCookie)
        {
            //Creamos la identidad.
            String identity = identificadorUnico + Constants.IdentitySeparator + id + Constants.IdentitySeparator + nick + Constants.IdentitySeparator + isAdmin.ToString();
            //Logueamos con la cookie.
            FormsAuthentication.SetAuthCookie(identity, createPersistentCookie);
        }
    }
}