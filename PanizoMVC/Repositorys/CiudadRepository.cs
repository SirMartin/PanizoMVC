using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class CiudadRepository : BaseRepository, Interfaces.ICiudadRep
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

        public List<Ciudad> GetCiudades()
        {
            return (from u in _dbContext.Ciudades
                    select u).ToList();
        }

        public Ciudad GetCiudadById(int idCiudad)
        {
            return (from u in _dbContext.Ciudades
                    where u.Id == idCiudad
                    select u).FirstOrDefault();
        }

        public void AddCiudad(Ciudad ciudad)
        {
            _dbContext.AddToCiudades(ciudad);
        }

        public void DeleteCiudad(Ciudad ciudad)
        {
            _dbContext.Ciudades.DeleteObject(ciudad);
        }
    }
}