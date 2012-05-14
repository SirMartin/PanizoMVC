using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Utilities
{
    public class IngredientesUtilities
    {

        private EntrepanDB db = new EntrepanDB();

        public void DetectNewIngredients(List<String> ingredientes)
        {
            List<Ingrediente> ingreDB = db.Ingredientes.ToList();
            List<Ingrediente> newIngredientes = new List<Ingrediente>();
            foreach (String ingre in ingredientes)
            {
                bool existeIngrediente = false;
                //Recorremos los ingredientes de BBDD.
                foreach (Ingrediente item in ingreDB)
                {
                    //Comprobamos si el ingrediente de la lista del bocadillo existe ya en BBDD o no.
                    if (ingre.ToLower().Equals(item.Nombre.ToLower()))
                    {
                        //Ya existe en la BBDD.
                        //Saltamos de ese ingrediente.
                        existeIngrediente = true;
                        break;
                    }
                }

                //Comprobamos los que existen y los que no, y los vamos añadiendo.
                if (!existeIngrediente)
                {
                    //No existe, creamos el ingrediente.
                    Ingrediente newIngrediente = new Ingrediente()
                    {
                        Nombre = ingre,
                        Descripcion = ingre,
                        FechaCreacion = DateTime.Now,
                        IdUsuario = 7
                    };
                    //Lo añadimos a la lista para agregar a BBDD.
                    newIngredientes.Add(newIngrediente);
                }
            }

            //Insertamos los elementos en BBDD.
            foreach (Ingrediente item in newIngredientes)
            {
                //Lo insertamos en BBDD.
                db.Ingredientes.AddObject(item);
                db.SaveChanges();
            }
        }

        
        public List<Ingrediente> ManageIngredientes(List<String> ingredientes)
        {
            //Insertamos los ingredientes que falten en BBDD.
            DetectNewIngredients(ingredientes);

            //Ahora que estan todos los ingredientes en BBDD, buscamos los que necesitamos.
            //Para hacer la lista y enviarla al controller, para poder guardarlo en referencia al bocadillo.
            //Que estamos insertando en este momento.
            List<Ingrediente> ingredientesList = new List<Ingrediente>();
            foreach (String ingredienteStr in ingredientes)
            {
                //Lo buscamos en BBDD, y lo recuperamos.
                Ingrediente ingrediente = db.Ingredientes.Where(g => g.Nombre.ToLower().Equals(ingredienteStr.ToLower())).FirstOrDefault();
                //Lo añadimos a la lista.
                ingredientesList.Add(ingrediente);
            }

            return ingredientesList;
        }
    }
}