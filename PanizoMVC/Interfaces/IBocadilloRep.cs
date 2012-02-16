using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IBocadilloRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<Bocadillo> GetBocadillos();
        Bocadillo GetBocadilloById(int idBocadillo);
        Bocadillo GetBocadilloByRestaurant(int idRestaurant);
        void AddBocadillo(Bocadillo bocadillo);
        void DeleteBocadillo(Bocadillo bocadillo);
    }
}