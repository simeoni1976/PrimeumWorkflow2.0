using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Helpers
{
    /// <summary>
    /// Classe d'extension de la classe de base des entités.
    /// </summary>
    public static class ExendsBaseEntity
    {
        /// <summary>
        /// Initialise les dates et l'utilisateur.
        /// </summary>
        /// <param name="ent">Entité cible</param>
        /// <param name="user">Utilisateur modificateur</param>
        /// <returns>Entité cible modifiée</returns>
        public static BaseEntity InitDatesAndUser(this BaseEntity ent, string user)
        {
            ent.AddedDate = DateTime.UtcNow;
            ent.ModifiedDate = DateTime.UtcNow;
            ent.Username = user;
            return ent;
        }

        /// <summary>
        /// Régle la date de modification d'une entité.
        /// </summary>
        /// <param name="ent">Entité cible</param>
        /// <param name="user">Utilisateur modificateur</param>
        /// <returns>Entité cible modifiée</returns>
        public static BaseEntity SetModifiedDate(this BaseEntity ent, string user)
        {
            ent.ModifiedDate = DateTime.UtcNow;
            ent.Username = user;
            return ent;
        }

        /// <summary>
        /// Régle uniquement la date de modification d'une entitée
        /// </summary>
        /// <param name="ent">Entité cible</param>
        /// <returns>Entité cible modifiée</returns>
        public static BaseEntity SetModifiedDate(this BaseEntity ent)
        {
            ent.ModifiedDate = DateTime.UtcNow;
            return ent;
        }

    }
}
