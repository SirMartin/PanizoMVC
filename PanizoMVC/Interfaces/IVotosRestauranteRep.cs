using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IVotosRestauranteRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<VotosRestaurante> GetVotosRestaurantes();
        VotosRestaurante GetVotoRestauranteById(int idVotoRestaurante);
        VotosRestaurante GetVotoRestauranteByBocadillo(int idRestaurante);
        VotosRestaurante GetVotoRestauranteByUsuario(int idUsuario);
        void AddVotosRestaurante(VotosRestaurante votoRestaurante);
        void DeleteVotosRestaurante(VotosRestaurante votoRestaurante);
    }
}