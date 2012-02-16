using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class RestauranteRepository : BaseRepository, Interfaces.IRestauranteRep
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
        
        public List<Restaurante> GetRestaurantes()
        {
            return (from u in _dbContext.Restaurantes
                    select u).ToList();
        }

        public Restaurante GetResturanteById(int idRestaurante)
        {
            return (from u in _dbContext.Restaurantes
                    where u.Id == idRestaurante
                    select u).FirstOrDefault();
        }

        public Restaurante GetResturanteByCiudad(int idCiudad)
        {
            return (from u in _dbContext.Restaurantes
                    where u.IdCiudad == idCiudad
                    select u).FirstOrDefault();
        }

        public void AddRestaurante(Restaurante restaurante)
        {
            _dbContext.AddToRestaurantes(restaurante);
        }

        public void DeleteRestaurante(Restaurante restaurante)
        {
            _dbContext.Restaurantes.DeleteObject(restaurante);
        }
    }
}