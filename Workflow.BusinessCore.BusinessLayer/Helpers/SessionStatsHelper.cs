using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Workflow.Transverse.Environment;
using Microsoft.EntityFrameworkCore.Storage;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// Classe helper pour simplifier la manipulation de la session "courte" (durée de vie de l'appel http)
    /// </summary>
    public static class SessionStatsHelper
    {

        /// <summary>
        /// Enregistre dans la session "courte" (durée de vie de l'appel http) un objet, primitif ou de type classe.
        /// </summary>
        /// <param name="key">Chaine clé permettant d'identifier l'objet</param>
        /// <param name="obj">Référence de l'objet (si de type classe) ou valeur à enregistrer</param>
        /// <param name="serviceProvider">Fournisseur de service, permettant d'accéder au contexte http.</param>
        public static void HttpHitSaveItem(string key, object obj, IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            IHttpContextAccessor httpC = serviceProvider.GetService<IHttpContextAccessor>();
            if (httpC.HttpContext.Items.ContainsKey(key))
                httpC.HttpContext.Items[key] = obj;
            else
                httpC.HttpContext.Items.Add(key, obj);
        }

        /// <summary>
        /// Récupére un objet enregistré dans la session "courte" (durée de vie de l'appel http)
        /// </summary>
        /// <typeparam name="T">Type de l'objet</typeparam>
        /// <param name="key">Chaine clé permettant d'identifier l'objet</param>
        /// <param name="serviceProvider">Fournisseur de service, permettant d'accéder au contexte http.</param>
        /// <returns>Objet enregistré si existant, valeur par défaut sinon.</returns>
        public static T HttpHitGetItem<T>(string key, IServiceProvider serviceProvider)
        {
            T obj = default(T);
            if (string.IsNullOrWhiteSpace(key))
                return obj;

            try
            {
                IHttpContextAccessor httpC = serviceProvider.GetService<IHttpContextAccessor>();
                if (httpC.HttpContext.Items.ContainsKey(key))
                    obj = (T)httpC.HttpContext.Items[key];
            }
            catch
            {
            }

            return obj;
        }


        /// <summary>
        /// Enregistre en session "courte" (durée de vie de l'appel http) la transaction de base de données courante.
        /// </summary>
        /// <param name="transaction">Transaction de la base de données</param>
        /// <param name="serviceProvider">Fournisseur de service, permettant d'accéder au contexte http.</param>
        public static void HttpHitSaveDBTransaction(IDbContextTransaction transaction, IServiceProvider serviceProvider)
        {
            HttpHitSaveItem(Constant.S_TRANSACTION_OBJECT, transaction, serviceProvider);
        }

        /// <summary>
        /// Récupére de la session "courte" (durée de vie de l'appel http) la transaction de base de donnés courtante.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service, permettant d'accéder au contexte http.</param>
        /// <returns>Transaction de la base de données courante</returns>
        public static IDbContextTransaction HttpHitGetDBTransaction(IServiceProvider serviceProvider)
        {
            return HttpHitGetItem<IDbContextTransaction>(Constant.S_TRANSACTION_OBJECT, serviceProvider);
        }

    }
}
