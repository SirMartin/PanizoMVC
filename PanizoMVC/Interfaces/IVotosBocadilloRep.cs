using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IVotosBocadilloRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<VotosBocadillo> GetVotosBocadillos();
        VotosBocadillo GetVotoBocadilloById(int idVotoBocadillo);
        VotosBocadillo GetVotoBocadilloByBocadillo(int idBocadillo);
        VotosBocadillo GetVotoBocadilloByUsuario(int idUsuario);
        void AddVotosBocadillo(VotosBocadillo votoBocadillo);
        void DeleteVotosBocadillo(VotosBocadillo votoBocadillo);
    }
}