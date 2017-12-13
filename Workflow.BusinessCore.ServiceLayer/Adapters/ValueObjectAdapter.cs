using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Helpers;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  ValueObjectAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the ValueObject adapter.
    /// </remarks>
    /// <typeparam name="T">Value DTO model</typeparam>
    public class ValueObjectAdapter : IValueObjectAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private IValueObjectDomain ValueObjectDomain
        {
            get
            {
                return _serviceProvider?.GetService<IValueObjectDomain>();
            }
        }

        private IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public ValueObjectAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// GetByFilterWithClausesAndSQLScript
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="sort_asc"></param>
        /// <param name="sort_desc"></param>
        /// <param name="grouping"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DTO.ValueObject>> Filter(string[] select, string[] where, string[] sort_asc, string[] sort_desc, bool grouping, int? page, int? pageSize)
        {
            IEnumerable<ENT.ValueObject> valueObjects = await ValueObjectDomain.Filter(select, where, sort_asc, sort_desc, grouping, page, pageSize);

            if (valueObjects != null)
                return Mapper.Map<IEnumerable<ENT.ValueObject>, IEnumerable<DTO.ValueObject>>(valueObjects);
            else
                return new List<DTO.ValueObject>();
        }

        /// <summary>
        /// This function permits to put a value in the grid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public async Task<DTO.ValueObject> Put(long id, DTO.ValueObject dto)
        {
            DTO.ValueObject element = Mapper.Map<ENT.ValueObject, DTO.ValueObject>(await ValueObjectDomain.Get(id));
            if (element.Id != 0)
            {
                return Mapper.Map<ENT.ValueObject, DTO.ValueObject>(
                    await ValueObjectDomain.Update(Mapper.Map<DTO.ValueObject, ENT.ValueObject>(dto)));
            }
            else
                return new DTO.ValueObject();
        }
        
        /// <summary>
        /// Permet de lire les données selon un format configuré par l'opérateur, avec des filtres, des tris, la pagination, etc...
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance concerné</param>
        /// <param name="filter">Chaines à filtrer, l'indexe représente le numéro de colonne sur lequel appliquer le filtre.</param>
        /// <param name="start">Numéro de ligne à partir duquel il faut commencer la selection</param>
        /// <param name="length">Nombre de ligne à sélectionner</param>
        /// <param name="sortCol">Numéro de colonne à trier</param>
        /// <param name="sortDir">Ordre du tri : ASC ou DESC</param>
        /// <returns>Message de retour + données</returns>
        public async Task<HttpResponseMessageResult> ReadData(long selectorInstanceId, string[] filter, int start, int length, int sortCol, string sortDir)
        {
            return await ValueObjectDomain.ReadData(selectorInstanceId, filter, start, length, sortCol, sortDir);
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.ValueObject>> GetAll()
        {
            IEnumerable<ENT.ValueObject> valueObjects = await ValueObjectDomain.Get();

            IEnumerable<DTO.ValueObject> dtoValueObjects = null;
            if (valueObjects != null)
                dtoValueObjects = Mapper.Map<IEnumerable<ENT.ValueObject>, IEnumerable<DTO.ValueObject>>(valueObjects);
            else
                dtoValueObjects = new List<DTO.ValueObject>();

            return dtoValueObjects;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.ValueObject> GetById(long id)
        {
            ENT.ValueObject valueObjects = await ValueObjectDomain.Get(id);

            DTO.ValueObject dtoValueObjects = null;
            if (valueObjects != null)
                dtoValueObjects = Mapper.Map<ENT.ValueObject, DTO.ValueObject>(valueObjects);

            return dtoValueObjects;
        }

        /// <summary>
        /// Permet d'importer une liste de ValueObject.
        /// </summary>
        /// <param name="valueObjects">Liste de ValueObject à enregistrer</param>
        /// <returns>Liste des ids enregistrés</returns>
        public async Task<IEnumerable<long>> Import(IEnumerable<DTO.ValueObject> valueObjects)
        {
            IEnumerable<ENT.ValueObject> entValueObjects = Mapper.Map<IEnumerable<DTO.ValueObject>, IEnumerable<ENT.ValueObject>>(valueObjects);
            return await ValueObjectDomain.Import(entValueObjects);
        }

    }
}