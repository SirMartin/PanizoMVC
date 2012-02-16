using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class BocadilloRepository : BaseRepository, Interfaces.IBocadilloRep
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
        
        public List<Bocadillo> GetBocadillos()
        {
            return (from u in _dbContext.Bocadillos
                    select u).ToList();
        }

        public Bocadillo GetBocadilloById(int idBocadillo)
        {
            return (from u in _dbContext.Bocadillos
                    where u.Id == idBocadillo
                    select u).FirstOrDefault();
        }

        public Bocadillo GetBocadilloByRestaurant(int idRestaurant)
        {
            return (from u in _dbContext.Bocadillos
                    where u.IdRestaurante == idRestaurant
                    select u).FirstOrDefault();
        }

        public void AddBocadillo(Bocadillo bocadillo)
        {
            _dbContext.AddToBocadillos(bocadillo);
        }

        public void DeleteBocadillo(Bocadillo bocadillo)
        {
            _dbContext.Bocadillos.DeleteObject(bocadillo);
        }
    }
}