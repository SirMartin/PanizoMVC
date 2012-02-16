using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Repositorys
{
    public class IngredienteRepository : BaseRepository, Interfaces.IIngredienteRep
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
        
        public List<Ingrediente> GetIngredientes()
        {
            return (from u in _dbContext.Ingredientes
                    select u).ToList();
        }

        public Ingrediente GetIngredienteById(int idIngrediente)
        {
            return (from u in _dbContext.Ingredientes
                    where u.Id == idIngrediente
                    select u).FirstOrDefault();
        }

        public void AddIngrediente(Ingrediente ingrediente)
        {
            _dbContext.AddToIngredientes(ingrediente);
        }

        public void DeleteIngrediente(Ingrediente ingrediente)
        {
            _dbContext.Ingredientes.DeleteObject(ingrediente);
        }
    }
}