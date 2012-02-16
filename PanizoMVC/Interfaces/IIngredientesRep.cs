using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Interfaces
{
    public interface IIngredienteRep
    {
        EntrepanDB DBContext
        {
            get;
            set;
        }

        List<Ingrediente> GetIngredientes();
        Ingrediente GetIngredienteById(int idIngrediente);
        void AddIngrediente(Ingrediente ingrediente);
        void DeleteIngrediente(Ingrediente ingrediente);
    }
}