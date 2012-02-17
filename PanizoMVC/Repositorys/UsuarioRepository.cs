using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class UsuarioRepository : BaseRepository, Interfaces.IUsuarioRep
    {
        EntrepanDB _dbContext = null;

        public EntrepanDB DBContext
        {
            get
            {
                return _dbContext;
            }

            set
            {
                _dbContext = value;
            }
        }

        public List<Usuario> GetUsuarios()
        {
            return (from u in _dbContext.Usuarios
                    select u).ToList();
        }

        public Usuario GetUsuarioById(int idUsuario)
        {
            return (from u in _dbContext.Usuarios
                    where u.Id == idUsuario
                    select u).FirstOrDefault();
        }

        public Usuario GetUsuarioByTwitterUserId(String idTwitterUser)
        {
            return (from u in _dbContext.Usuarios
                    where u.TwitterUserId.Equals(idTwitterUser)
                    select u).FirstOrDefault();
        }

        public Usuario GetUsuarioByEmail(string email)
        {
            return (from u in _dbContext.Usuarios
                    where u.Email.ToUpper().Equals(email.ToUpper())
                    select u).FirstOrDefault();
        }

        public bool IsLoginCorrecto(string email, string pass)
        {
            //Cogemos el usuario por e-mail.
            Usuario user = GetUsuarioByEmail(email);

            if (user != null)
            {
                //Comprobamos la contraseña.
                if (user.Password.Equals(pass))
                {
                    //Es correcto.
                    return true;
                }
                else
                {
                    //No es correcto.
                    return false;
                }
            }
            else
            {
                //No es correcto.
                return false;
            }
        }

        public void AddUsuario(Usuario usuario)
        {
            _dbContext.AddToUsuarios(usuario);
        }

        public void DeleteUsuario(Usuario usuario)
        {
            _dbContext.Usuarios.DeleteObject(usuario);
        }
    }
}