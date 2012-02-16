using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IRestauranteRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<Restaurante> GetRestaurantes();
        Restaurante GetResturanteById(int idRestaurante);
        Restaurante GetResturanteByCiudad(int idCiudad);
        void AddRestaurante(Restaurante restaurante);
        void DeleteRestaurante(Restaurante restaurante);
    }
}