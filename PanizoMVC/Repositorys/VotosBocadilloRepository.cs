using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class VotosBocadilloRepository : BaseRepository, Interfaces.IVotosBocadilloRep
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
        
        public List<VotosBocadillo> GetVotosBocadillos()
        {
            return (from u in _dbContext.VotosBocadillos
                    select u).ToList();
        }

        public VotosBocadillo GetVotoBocadilloById(int idVotoBocadillo)
        {
            return (from u in _dbContext.VotosBocadillos
                    where u.Id == idVotoBocadillo
                    select u).FirstOrDefault();
        }

        public VotosBocadillo GetVotoBocadilloByBocadillo(int idBocadillo)
        {
            return (from u in _dbContext.VotosBocadillos
                    where u.IdBocadillo == idBocadillo
                    select u).FirstOrDefault();
        }

        public VotosBocadillo GetVotoBocadilloByUsuario(int idUsuario)
        {
            return (from u in _dbContext.VotosBocadillos
                    where u.IdUsuario == idUsuario
                    select u).FirstOrDefault();
        }

        public void AddVotosBocadillo(VotosBocadillo votoBocadillo)
        {
            _dbContext.AddToVotosBocadillos(votoBocadillo);
        }

        public void DeleteVotosBocadillo(VotosBocadillo votoBocadillo)
        {
            _dbContext.VotosBocadillos.DeleteObject(votoBocadillo);
        }
    }
}