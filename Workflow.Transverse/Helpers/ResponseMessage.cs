using System;
using System.Text;
using Workflow.Transverse.Environment;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// Classe de message de réponse pour les appels au WebAPI du BusinessCore.
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// L'appel s'est terminé avec succés.
        /// </summary>
        public bool IsSuccess { get; set; }


        /// <summary>
        /// Message de la réponse (facultatif).
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if ((_message != null) && (_message.Length > Constant.RESPONSE_MAX_SIZE_MESSAGE))
                    _message = _message.Substring(0, Constant.RESPONSE_MAX_SIZE_MESSAGE) + "...";
            }
        }

        /// <summary>
        /// La réponse est compléte : l'appel rend rapidement la main. Sinon, il faut interroger l'interface de SyncResponses.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Pourcentage d'avancement, si le traitement n'est pas complet.
        /// </summary>
        /// <remarks>Est compris entre 0 et 100.</remarks>
        public double Progression
        {
            get
            {
                if (IsCompleted)
                    return 100;
                return _progression;
            }
            set
            {
                if ((value >= 0) && (value <= 100))
                    _progression = value;
                if (_progression == 100)
                    IsCompleted = true;
            }
        }

        /// <summary>
        /// Capture l'exception en paramètre et créé un nouveau message.
        /// </summary>
        /// <param name="ex">Exception </param>
        public void GetExceptionMessage(Exception ex)
        {
            string msgTmp = "";

            while (ex != null)
            {
                msgTmp += ExtractExceptionMessage(ex);
                ex = ex.InnerException;
            }

            Message = msgTmp;
        }

        private double _progression = 0;
        private string _message = null;

        /// <summary>
        /// Extrait un message texte d'une exception, sans profondeur (pas de parcours d'InnerException).
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Texte de l'exception</returns>
        private string ExtractExceptionMessage(Exception ex)
        {
            if (ex == null)
                return "";

            StringBuilder msg = new StringBuilder();

            msg.AppendLine(ex.Message);
            msg.AppendLine(ex.Source);
            msg.AppendLine($"HResult: {ex.HResult}");
            msg.AppendLine(ex.StackTrace);

            return msg.ToString();
        }
    }
}
