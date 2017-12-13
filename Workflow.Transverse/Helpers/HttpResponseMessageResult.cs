using Newtonsoft.Json;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// HttpResponseMessageResult class.
    /// </summary>
    public class HttpResponseMessageResult
    {
        /// <summary>
        /// IsSuccess property.
        /// </summary>
        /// <value>
        /// Gets or sets the IsSuccess value.
        /// </value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// ErrorHasDisplay property.
        /// </summary>
        /// <value>
        /// Gets or sets the ErrorHasDisplay value.
        /// </value>
        public bool ErrorHasDisplay { get; set; }

        /// <summary>
        /// ErrorStatusCode property.
        /// </summary>
        /// <value>
        /// Gets or sets the ErrorStatusCode value.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// ErrorMessage property.
        /// </summary>
        /// <value>
        /// Gets or sets the ErrorMessage value.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Json property.
        /// </summary>
        /// <value>
        /// Gets or sets the Json value.
        /// </value>
        public string Json { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public HttpResponseMessageResult()
        {
            IsSuccess = false;
            ErrorHasDisplay = false;
        }

        /// <summary>
        /// Permet inclure un autre message de résultat dans l'instance actuelle.
        /// </summary>
        /// <param name="other">Autre message de résultat</param>
        public void Append(HttpResponseMessageResult other)
        {
            // Tout doit être en succès, sinon c'est en erreur
            IsSuccess = IsSuccess && other.IsSuccess;
            // On prend l'affichage si jamais c'est demandé.
            ErrorHasDisplay = ErrorHasDisplay && other.ErrorHasDisplay;
            // On concaténe les messages
            if (!string.IsNullOrWhiteSpace(other.Message))
            {
                if (!string.IsNullOrWhiteSpace(Message))
                    Message = string.Format("{0} -- {1}", Message, other.Message);
                else
                    Message = other.Message;
            }
            // On garde l'erreur la plus grande
            StatusCode = System.Math.Max(StatusCode, other.StatusCode);

            // json : on essaie de concaténer si possible
            if (!string.IsNullOrWhiteSpace(other.Json))
            {
                if (!string.IsNullOrWhiteSpace(Json))
                    Json = $"[{Json},{other.Json}]";
                else
                    Json = other.Json;
            }
        }

        /// <summary>
        /// Sérialize un object vers le champs Json.
        /// </summary>
        /// <param name="obj">Objet à sérializer.</param>
        /// <remarks>Les références en loop sont ignorées.</remarks>
        public void GetObjectForJson(object obj)
        {
            Json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}
