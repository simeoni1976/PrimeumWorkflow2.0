using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using System;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    /// Classe adaptateur pour le controler DataSet.
    /// </summary>
    public class DataSetAdapter : IDataSetAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        protected IDataSetDomain DataSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDomain>();
            }
        }


        protected IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public DataSetAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.DataSet>> GetAll()
        {
            IEnumerable<ENT.DataSet> lstInter = await DataSetDomain.Get();

            IEnumerable<DTO.DataSet> dtoDs = null;
            if (lstInter != null)
                dtoDs = lstInter.Select(ds => Mapper.Map<ENT.DataSet, DTO.DataSet>(ds));
            else
                dtoDs = new List<DTO.DataSet>();

            return dtoDs;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.DataSet> GetById(long id)
        {
            ENT.DataSet dsEntity = await DataSetDomain.Get(id);

            DTO.DataSet dtoDs = null;
            if (dsEntity != null)
                dtoDs = Mapper.Map<ENT.DataSet, DTO.DataSet>(dsEntity);

            return dtoDs;
        }


        /// <summary>
        /// Ajout un nouveau DataSet.
        /// </summary>
        /// <param name="dataSet">Nouveau DataSet</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.DataSet> AddDataSet(DTO.DataSet dataSet)
        {
            ENT.DataSet entDataSet = Mapper.Map<DTO.DataSet, ENT.DataSet>(dataSet);

            return Mapper.Map<ENT.DataSet, DTO.DataSet>(await DataSetDomain.AddDataSet(entDataSet));
        }

        /// <summary>
        /// Ajoute un DataSetDimension à un DataSet.
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <param name="dataSetId">Id du DataSet cible</param>
        /// <param name="dimensionId">Id de la dimension à associer</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.DataSetDimension> AddDataSetDimension(DTO.DataSetDimension dataSetDimension, long dataSetId, long dimensionId)
        {
            ENT.DataSetDimension entDataSetDimension = Mapper.Map<DTO.DataSetDimension, ENT.DataSetDimension>(dataSetDimension);

            return Mapper.Map<ENT.DataSetDimension, DTO.DataSetDimension>(await DataSetDomain.AddDataSetDimension(entDataSetDimension, dataSetId, dimensionId));
        }


        /// <summary>
        /// Récupére toutes les DistinctValue associées à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        public async Task<IEnumerable<DTO.DistinctValue>> GetAllDistinctValue(long dataSetId)
        {
            return Mapper.Map<IEnumerable<ENT.DistinctValue>, IEnumerable<DTO.DistinctValue>>(await DataSetDomain.GetAllDistinctValue(dataSetId));
        }

        /// <summary>
        /// Récupére les DistinctValue associées à un DataSet d'une dimension
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="dimensionId">Id de la Dimension</param>
        /// <returns>Message de retour</returns>
        public async Task<IEnumerable<DTO.DistinctValue>> GetDistinctValueByDimension(long dataSetId, long dimensionId)
        {
            return Mapper.Map<IEnumerable<ENT.DistinctValue>, IEnumerable<DTO.DistinctValue>>(await DataSetDomain.GetDistinctValueByDimension(dataSetId, dimensionId));
        }

        /// <summary>
        /// Lie une liste de ValueObject à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="valueObjectIds">Liste des ids des ValueObject à lier.</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> BindValueObjectToDataSet(long dataSetId, IEnumerable<long> valueObjectIds)
        {
            return await DataSetDomain.BindValueObjectToDataSet(dataSetId, valueObjectIds);
        }

        /// <summary>
        /// Permet de générer la liste de valeur distinct d'un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        /// <remarks>S'il existe déjà des données, elles sont effacées au profit des nouvelles.</remarks>
        public async Task<HttpResponseMessageResult> InitializeDistinctValue(long dataSetId)
        {
            int nbr = await DataSetDomain.InitializeDistinctValue(dataSetId);
            return new HttpResponseMessageResult() { IsSuccess = (nbr > 0) };
        }

    }
}
