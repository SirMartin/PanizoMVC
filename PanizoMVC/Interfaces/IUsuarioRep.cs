using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IUsuarioRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<Usuario> GetUsuarios();
        Usuario GetUsuarioByTwitterUserId(String idTwitterUser);
        Usuario GetUsuarioById(int idTwitterUsuario);
        Usuario GetUsuarioByEmail(String email);
        Boolean IsLoginCorrecto(String email, String pass);
        void AddUsuario(Usuario usuario);
        void DeleteUsuario(Usuario usuario);
    }
}