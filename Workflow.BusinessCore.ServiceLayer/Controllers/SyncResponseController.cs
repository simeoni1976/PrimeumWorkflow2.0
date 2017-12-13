using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// Global controller for synchronize async responses.
    /// </summary>
    [Route("api/[controller]")]
    [ActionLog, ExceptionLog]
    public class SyncResponseController : Controller
    {
        private ISynchronizeDictionary _lockedObjects = null;

        /// <summary>
        /// Constructeur pour DI.
        /// </summary>
        /// <param name="lockedObjects">Dictionnaire des objets verrouillés</param>
        public SyncResponseController(ISynchronizeDictionary lockedObjects)
        {
            _lockedObjects = lockedObjects;
        }


        /// <summary>
        /// Permet d'attendre la réponse d'un autre appel WebAPI.
        /// </summary>
        /// <param name="token">token de l'appel attendu</param>
        /// <returns>Reponse informant si la réponse est compléte ou non.</returns>
        [HttpPost]
        public async Task<ResponseMessage> GetSynchronizedReturn(string token)
        {
            Object lockedObj = _lockedObjects.GetLockedObjectByToken(token);

            bool isCompleted = false;
            ResponseMessage rm = new ResponseMessage();

            if (lockedObj != null)
            {
                try
                {
                    isCompleted = await Task.Run(() =>
                    {
                        bool hasCatched = false;
                        try
                        {
                            hasCatched = Monitor.TryEnter(lockedObj, Constant.SYNCRESPONSES_TIMEOUT);
                        }
                        finally
                        {
                            if (hasCatched)
                                Monitor.Exit(lockedObj);
                        }
                        return hasCatched;
                    });
                }
                catch (Exception ex)
                {
                    rm.GetExceptionMessage(ex);
                }
            }
            else
                rm.Message = "No token available";

            rm.IsCompleted = isCompleted;
            return rm;
        }
    }
}
