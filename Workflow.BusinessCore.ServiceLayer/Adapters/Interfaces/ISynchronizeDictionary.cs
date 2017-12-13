using System;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    /// Interface pour les réponses synchronisées
    /// </summary>
    public interface ISynchronizeDictionary
    {
        /// <summary>
        /// Donne l'objet verrouillé selon un token.
        /// </summary>
        /// <param name="token">token du sémaphore</param>
        /// <returns></returns>
        Object GetLockedObjectByToken(string token);

        /// <summary>
        /// Ajoute un object verrouillé et renvoie le token associé.
        /// </summary>
        /// <returns>token, non null si ok, null sinon</returns>
        string AddLockedObject();

        /// <summary>
        /// Supprime l'objet verrouillé lié au token
        /// </summary>
        /// <param name="token"></param>
        void DeleteLockedObject(string token);

    }
}
