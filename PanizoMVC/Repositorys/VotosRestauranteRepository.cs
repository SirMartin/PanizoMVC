using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class VotosRestauranteRepository : BaseRepository, Interfaces.IVotosRestauranteRep
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

        public List<VotosRestaurante> GetVotosRestaurantes()
        {
            return (from u in _dbContext.VotosRestaurantes
                    select u).ToList();
        }

        public VotosRestaurante GetVotoRestauranteById(int idVotoRestaurante)
        {
            return (from u in _dbContext.VotosRestaurantes
                    where u.Id == idVotoRestaurante
                    select u).FirstOrDefault();
        }

        public VotosRestaurante GetVotoRestauranteByBocadillo(int idRestaurante)
        {
            return (from u in _dbContext.VotosRestaurantes
                    where u.IdRestaurante == idRestaurante
                    select u).FirstOrDefault();
        }

        public VotosRestaurante GetVotoRestauranteByUsuario(int idUsuario)
        {
            return (from u in _dbContext.VotosRestaurantes
                    where u.IdUsuario == idUsuario
                    select u).FirstOrDefault();
        }

        public void AddVotosRestaurante(VotosRestaurante votoRestaurante)
        {
            _dbContext.AddToVotosRestaurantes(votoRestaurante);
        }

        public void DeleteVotosRestaurante(VotosRestaurante votoRestaurante)
        {
            _dbContext.VotosRestaurantes.DeleteObject(votoRestaurante);
        }
    }
}