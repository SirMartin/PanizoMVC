using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PanizoMVC.Utilities
{
    public class IngredientesUtilities
    {
        //La base de datos.
        private EntrepanDB db = new EntrepanDB();

        /// <summary>
        /// Cogemos la lista de ingredientes y detectamos cuales existen en BBDD y cuales no para añadir los que no existen.
        /// </summary>
        /// <param name="ingredientes">La lista de ingredientes.</param>
        private void DetectNewIngredients(List<String> ingredientes)
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


        /// <summary>
        /// Cogemos los ingredientes que se han puesto al crear el bocadillo y añadimos a BBDD los que no existian
        /// y luego los cogemos todos y los añadimos al bocadillo para crear la relación en BBDD.
        /// </summary>
        /// <param name="ingredientes">La lista de todos los ingredientes añadidos al bocadillo.</param>
        /// <param name="idBocadillo">El id del bocadillo para crear la relación.</param>
        /// <returns>Las entidades de relación entre los ingredientes y el bocadillo.</returns>
        public List<BocadilloIngrediente> ManageIngredientes(List<String> ingredientes, int idBocadillo)
        {
            //Insertamos los ingredientes que falten en BBDD.
            DetectNewIngredients(ingredientes);

            //Ahora que estan todos los ingredientes en BBDD, buscamos los que necesitamos.
            //Para hacer la lista y enviarla al controller, para poder guardarlo en referencia al bocadillo.
            //Que estamos insertando en este momento.
            List<BocadilloIngrediente> ingredientesList = new List<BocadilloIngrediente>();
            foreach (String ingredienteStr in ingredientes)
            {
                //Lo buscamos en BBDD, y lo recuperamos.
                Ingrediente ingrediente = db.Ingredientes.Where(g => g.Nombre.ToLower().Equals(ingredienteStr.ToLower())).FirstOrDefault();
                //Creamos la entidad del bocadillo.
                BocadilloIngrediente ingredienteBocadillo = new BocadilloIngrediente()
                {
                    IdBocadillo = idBocadillo,
                    IdIngrediente = ingrediente.Id,
                    IdUsuario = 7
                };
                //Lo añadimos a la lista.
                ingredientesList.Add(ingredienteBocadillo);
            }

            //Devolvemos los ingredientes.
            return ingredientesList;
        }
    }
}