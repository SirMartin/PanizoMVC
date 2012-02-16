using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface ICiudadRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<Ciudad> GetCiudades();
        Ciudad GetCiudadById(int idCiudad);
        void AddCiudad(Ciudad ciudad);
        void DeleteCiudad(Ciudad ciudad);
    }
}