using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class SynchronizeDictionary : ISynchronizeDictionary
    {
        private ConcurrentDictionary<string, Object> _lockedObjects = null;

        public SynchronizeDictionary()
        {
            _lockedObjects = new ConcurrentDictionary<string, Object>();
        }

        /// <summary>
        /// Ajoute une sémphore.
        /// </summary>
        /// <returns>Un token non null si ok, null sinon.</returns>
        public string AddLockedObject()
        {
            string token = null;

            try
            {
                Random generatorRnd = new Random((int)DateTime.UtcNow.Ticks);
                MD5 hasher = MD5.Create();

                int alea = 0;
                string tokenTmp = null;
                int cptMax = 10; // Nbr d'essais max

                do
                {
                    alea = generatorRnd.Next();
                    byte[] tokenBytes = hasher.ComputeHash(BitConverter.GetBytes(alea));
                    tokenTmp = tokenBytes.Select(b => b.ToString("X2")).Aggregate((current, next) => current + next);
                    cptMax--;
                    if (_lockedObjects.TryAdd(tokenTmp, new Object()))
                    {
                        token = tokenTmp;
                        break;
                    }
                }
                while (cptMax > 0);
            }
            // TDU - 28/7/2017 - Si on n'utilise pas l'exception, on catch sans exception plus simplement.
            //catch (Exception ex)
            catch
            {
                token = null;
            }

            return token;
        }

        /// <summary>
        /// Donne le sémaphore selon un token.
        /// </summary>
        /// <param name="token">token lié au sémaphore.</param>
        /// <returns>Objet verrouillé non null si ok, null sinon.</returns>
        public Object GetLockedObjectByToken(string token)
        {
            Object lockedObj = null;

            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                _lockedObjects.TryGetValue(token, out lockedObj);
            }
            catch
            {
            }

            return lockedObj;
        }

        /// <summary>
        /// Supprime l'objet verrouillé selon le token lié
        /// </summary>
        /// <param name="token">Token lié à l'objet</param>
        public void DeleteLockedObject(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return ;

            try
            {
                //Object obj = null;
                _lockedObjects.TryRemove(token, out Object obj);
            }
            catch
            {
            }
        }
    }
}
