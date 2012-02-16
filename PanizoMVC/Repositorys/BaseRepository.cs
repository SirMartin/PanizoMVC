using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Objects;

namespace PanizoMVC.Repositorys
{
    public class BaseRepository
    {
        private EntrepanDB CreateNew()
        {
            EntrepanDB db = new EntrepanDB();
            return db;
        }

        private static void context_SavingChanges(object sender, EventArgs e)
        {

            // Ensure that we are passed an ObjectContext
            EntrepanDB context = sender as EntrepanDB;
            if (context != null)
            {
                // Validate the state of each entity in the context
                // before SaveChanges can succeed.
                foreach (ObjectStateEntry entry in
                    context.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added | EntityState.Modified))
                {
                    // Find an object state entry for a SalesOrderHeader object. 
                    if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(Bocadillo)))
                    {
                        Bocadillo bocadillo = entry.Entity as Bocadillo;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(Ciudad)))
                    {
                        Ciudad ciudad = entry.Entity as Ciudad;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(Ingrediente)))
                    {
                        Ingrediente ingrediente = entry.Entity as Ingrediente;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(Restaurante)))
                    {
                        Restaurante restaurante = entry.Entity as Restaurante;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(Usuario)))
                    {
                        Usuario usuario = entry.Entity as Usuario;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(VotosBocadillo)))
                    {
                        VotosBocadillo votosBocadillo = entry.Entity as VotosBocadillo;
                    }
                    else if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(VotosRestaurante)))
                    {
                        VotosRestaurante votosRestaurante = entry.Entity as VotosRestaurante;
                    }
                }
            }
        }
    }
}