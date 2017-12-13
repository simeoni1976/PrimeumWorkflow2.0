using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow.Frontal.BusinessLogic.Services;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Configuration
{
    public class ConfigInjectionManager
    {
        public const string HTTP_VERB_POST = "POST";
        public const string HTTP_VERB_PUT = "PUT";
        public const string HTTP_VERB_GET = "GET";

        public const string JSON_KEY_SERVICENAME = "ServiceName";
        public const string JSON_KEY_HTTPVERB = "HttpVerb";
        public const string JSON_KEY_SUBSERVICENAME = "SubServiceName";
        public const string JSON_KEY_BODY = "Body";
        public const string JSON_KEY_QUERYSTRING = "Querystring";
        public const string JSON_KEY_RETURN = "Return";

        private List<Tuple<string, string, string, IEnumerable<KeyValuePair<string, string>>, KeyValuePair<string, string>, string>> _jsonServiceObjects = null;
        private Dictionary<string, long> _references = null;
        private Regex _regId = null;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ConfigInjectionManager()
        {
            _jsonServiceObjects = new List<Tuple<string, string, string, IEnumerable<KeyValuePair<string, string>>, KeyValuePair<string, string>, string>>();
            _references = new Dictionary<string, long>();
            _regId = new Regex(@"(?<id>ID_[\w_]*)");
        }

        /// <summary>
        /// Prend un texte json contenant la configuration et le prépare pour le traitement.
        /// </summary>
        /// <param name="plainText">Texte contenant le json</param>
        public HttpResponseMessageResult GetJson(string plainText)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            try
            {
                JToken jsonTokens = JToken.Parse(plainText);

                int cpt = 1;

                foreach (JToken token in jsonTokens.Children())
                {
                    try
                    {
                        string serviceName = token[JSON_KEY_SERVICENAME].ToString();
                        string httpVerb = token[JSON_KEY_HTTPVERB]?.ToString() ?? HTTP_VERB_POST;
                        string subServiceName = token[JSON_KEY_SUBSERVICENAME]?.ToString();
                        List<KeyValuePair<string, string>> lstQuery = null;
                        if (token[JSON_KEY_QUERYSTRING]?.HasValues ?? false)
                            lstQuery = DeserializeListKeyValuePair(token[JSON_KEY_QUERYSTRING] as JObject);

                        string jsonBody = null;
                        if (token[JSON_KEY_BODY] != null)
                            jsonBody = token[JSON_KEY_BODY].ToString();

                        KeyValuePair<string, string> pairedReturn = new KeyValuePair<string, string>(null, null); // Type struct, la variable garde en mémoire les valeurs précédentes si on ne l'initialise pas.
                        if (token[JSON_KEY_RETURN]?.HasValues ?? false)
                            pairedReturn = DeserializeKeyValuePair(token[JSON_KEY_RETURN] as JObject);

                        _jsonServiceObjects.Add(Tuple.Create(serviceName, httpVerb, subServiceName, lstQuery as IEnumerable<KeyValuePair<string, string>>, pairedReturn, jsonBody));
                    }
                    catch (Exception ex)
                    {
                        res.IsSuccess = false;
                        res.Message += $"Service n° {cpt} - Erreur sur la lecture d'un service : {ex.Message}";
                    }
                    cpt++;
                }
            }
            catch (Exception glEx)
            {
                res.IsSuccess = false;
                res.Message += $"Erreur sur la lecture du fichier : {glEx.Message}";
            }
            return res;
        }

        /// <summary>
        /// Effectue l'injection des services demandés.
        /// </summary>
        /// <param name="factory">Factory pour la communication avec le BusinessCore.</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> InjectConfig(ServiceFacade factory)
        {
            int cpt = 1;
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            foreach (Tuple<string, string, string, IEnumerable<KeyValuePair<string, string>>, KeyValuePair<string, string>, string> elt in _jsonServiceObjects)
            {
                try
                {
                    string serviceName = elt.Item1;
                    string httpVerb = elt.Item2;
                    string subServiceName = elt.Item3;
                    List<KeyValuePair<string, string>> qryStr = null;
                    KeyValuePair<string, string> pairedReturn = elt.Item5;
                    string body = elt.Item6;

                    AbstractServiceFactory service = factory[serviceName];
                    if (service == null)
                    {
                        res.IsSuccess = false;
                        res.Message += $"Service n° {cpt} - Le nom de service [{serviceName}] n'est pas connu.";
                        return res;
                    }
                    if (!string.IsNullOrWhiteSpace(pairedReturn.Value) && _references.ContainsKey(pairedReturn.Value))
                    {
                        res.IsSuccess = false;
                        res.Message += $"Service n° {cpt} - Une référence ({pairedReturn.Value}) existe déjà.";
                        return res;
                    }

                    // Transforme le body 
                    body = ReplaceGoodIds(body);
                    // Transforme les querystring
                    if (elt.Item4 != null)
                    {
                        qryStr = new List<KeyValuePair<string, string>>();
                        foreach (KeyValuePair<string, string> kvp in elt.Item4)
                            qryStr.Add(new KeyValuePair<string, string>(kvp.Key, ReplaceGoodIds(kvp.Value)));
                    }

                    res.Message += $"Appel au service : [{serviceName}]/{subServiceName} ({httpVerb})"; // Globalisation
                    HttpResponseMessageResult call = new HttpResponseMessageResult() { IsSuccess = true };
                    if (httpVerb == HTTP_VERB_POST)
                        call = await service.Post(subServiceName, qryStr, body);
                    if (httpVerb == HTTP_VERB_PUT)
                        call = await service.Put(subServiceName, qryStr, body);
                    if (httpVerb == HTTP_VERB_GET)
                        call = await service.Get(subServiceName, qryStr);

                    if ((call != null) && !call.IsSuccess)
                    {
                        res.IsSuccess = false;
                        res.Message += $"Service n° {cpt} - Retour en erreur : {call.Message}";
                        res.Append(call);
                        return res;
                    }

                    JToken returnObj = null;
                    if ((call != null) && !string.IsNullOrWhiteSpace(call.Json))
                        returnObj = JsonConvert.DeserializeObject(call.Json, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }) as JToken;
                    if (!string.IsNullOrWhiteSpace(pairedReturn.Key) && !string.IsNullOrWhiteSpace(pairedReturn.Value) && (returnObj != null) && (returnObj[pairedReturn.Key] != null))
                    {
                        long id = JsonConvert.DeserializeObject<long>(returnObj[pairedReturn.Key].ToString());
                        _references.Add(pairedReturn.Value, id);
                    }

                    res.Append(call);
                }
                catch (Exception ex)
                {
                    res.IsSuccess = false;
                    res.Message += $"Service n° {cpt} - Exception : {ex.Message}";
                }
                cpt++;
            }
            return res;
        }

        private string ReplaceGoodIds(string strBrut)
        {
            if (string.IsNullOrWhiteSpace(strBrut))
                return strBrut;

            StringBuilder sbStr = new StringBuilder(strBrut);

            Match mRes = _regId.Match(strBrut);
            while (mRes.Success)
            {
                string key = mRes.Value;
                if (_references.ContainsKey(key))
                    sbStr.Replace(key, _references[key].ToString());

                mRes = mRes.NextMatch();
            }

            return sbStr.ToString();
        }

        private KeyValuePair<string, string> DeserializeKeyValuePair(JObject jobj)
        {
            return DeserializeListKeyValuePair(jobj).FirstOrDefault();
        }

        private List<KeyValuePair<string, string>> DeserializeListKeyValuePair(JObject jobj)
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();

            if ((jobj?.Properties()?.Count() ?? 0) > 0)
            {
                foreach (JProperty prop in jobj.Properties())
                {
                    if (prop == null)
                        continue;

                    string keyR = prop.Name;
                    string valueR = jobj[keyR]?.ToString();
                    lst.Add(new KeyValuePair<string, string>(keyR, valueR));
                }
            }

            return lst;
        }


    }
}
