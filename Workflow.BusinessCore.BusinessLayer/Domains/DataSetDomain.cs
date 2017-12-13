using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// DataSet domain class.
    /// </summary>
    public class DataSetDomain : AbstractDomain<DataSet>, IDataSetDomain
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public DataSetDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.DataSetRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This function permits to get a dataset by the name
        /// </summary>
        /// <param name="name">DataSet name</param>
        /// <returns>DataSet</returns>
        public Task<DataSet> Get(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialise les données d'un DataSet.
        /// </summary>
        /// <param name="dataset">DataSet</param>
        /// <returns>Nombre de mise à jour</returns>
        /// <remarks>Met à jour la colonne CurrentValue dans les lignes de la table ValueObject qui appartiennent au DataSet.</remarks>
        public async Task<int> InitializeData(DataSet dataset)
        {
            List<ValueObject> lstVo = await UnitOfWork.GetDbContext().ValueObject
                .Include(vo => vo.DataSet)
                .Where(vo => vo.DataSet.Id == dataset.Id)
                .ToAsyncEnumerable()
                .ToList();

            foreach (ValueObject vo in lstVo)
            {
                UnitOfWork.ValueObjectRepository.PrepareUpdateForObject(vo);
                vo.CurrentValue = vo.InitialValue;
                vo.FutureValue = null;
            }

            return await UnitOfWork.GetDbContext().SaveChangesAsync();
        }

        /// <summary>
        /// Permet de générer la liste de valeur distinct d'un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Nombre d'enregistrement réussi</returns>
        /// <remarks>S'il existe déjà des données, elles sont effacées au profit des nouvelles.</remarks>
        public async Task<int> InitializeDistinctValue(long dataSetId)
        {
            List<Tuple<string, long>> lstDim = await UnitOfWork.GetDbContext().DataSetDimension
                .Include(d => d.DataSet)
                .Include(d => d.Dimension)
                .Where(d => d.DataSet.Id == dataSetId)
                .Select(d => new Tuple<string, long>(d.ColumnName, d.Dimension.Id))
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            List<DistinctValue> lstDistinct = await UnitOfWork.GetDbContext().DistinctValue
                .Where(dv => dv.DataSetId == dataSetId)
                .ToAsyncEnumerable()
                .ToList();
            UnitOfWork.GetDbContext().DistinctValue.RemoveRange(lstDistinct);

            foreach (Tuple<string, long> t in lstDim)
            {
                Expression<Func<ValueObject, string>> selectExpr = PropertyHelper.GetSelectDimension(t.Item1);

                List<string> lst = await UnitOfWork.GetDbContext().ValueObject
                    .Include(vo => vo.DataSet)
                    .Where(vo => vo.DataSet.Id == dataSetId)
                    .Select(selectExpr)
                    .Distinct()
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .ToList();

                foreach (string distinctValue in lst)
                {
                    DistinctValue dv = new DistinctValue() { DataSetId = dataSetId, DimensionId = t.Item2, Value = distinctValue };
                    UnitOfWork.DistinctValueRepository.PrepareAddForObject(dv);
                }
            }

            return await UnitOfWork.GetDbContext().SaveChangesAsync();
        }

        /// <summary>
        /// Ajout un nouveau DataSet.
        /// </summary>
        /// <param name="dataSet">Nouveau DataSet</param>
        /// <returns>Message de retour</returns>
        public async Task<DataSet> AddDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new WrongParameterException("DataSetDomain.AddDataSet : DataSet is null.");
            int cnt = await UnitOfWork.GetDbContext().DataSet
                .Where(d => d.Name == dataSet.Name)
                .CountAsync();
            if (cnt > 0)
                throw new WrongParameterException($"DataSetDomain.AddDataSet : DataSet'name ({dataSet.Name}) already exists.");

            UnitOfWork.DataSetRepository.PrepareAddForObject(dataSet);
            dataSet.WorkflowInstanceId = null;
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("DataSetDomain.AddDataSet : impossible to save DataSet.");

            return dataSet;
        }

        /// <summary>
        /// Ajoute un DataSetDimension à un DataSet.
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <param name="dataSetId">Id du DataSet cible</param>
        /// <param name="dimensionId">Id de la dimension à associer</param>
        /// <returns>Message de retour</returns>
        public async Task<DataSetDimension> AddDataSetDimension(DataSetDimension dataSetDimension, long dataSetId, long dimensionId)
        {
            if (dataSetDimension == null)
                throw new WrongParameterException($"DataSetDomain.AddDataSetDimension : DataSetDimension is null.");
            DataSet dataSet = await UnitOfWork.DataSetRepository.GetById(dataSetId);
            if (dataSet == null)
                throw new WrongParameterException($"DataSetDomain.AddDataSetDimension : DataSet (id = {dataSetId}) don't exist.");
            Dimension dimension = await UnitOfWork.DimensionRepository.GetById(dimensionId);
            if (dimension == null)
                throw new WrongParameterException($"DataSetDomain.AddDataSetDimension : Dimension (id = {dimensionId}) don't exist.");

            UnitOfWork.DataSetDimensionRepository.PrepareAddForObject(dataSetDimension);
            dataSetDimension.DataSet = dataSet;
            dataSetDimension.Dimension = dimension;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("DataSetDomain.AddDataSetDimension : impossible to save DataSetDimension.");

            return dataSetDimension;
        }

        /// <summary>
        /// Récupére toutes les DistinctValue associées à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        public async Task<IEnumerable<DistinctValue>> GetAllDistinctValue(long dataSetId)
        {
            IEnumerable<DistinctValue> lstDV = await UnitOfWork.GetDbContext().DistinctValue
                .Where(dv => dv.DataSetId == dataSetId)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return lstDV;
        }

        /// <summary>
        /// Récupére les DistinctValue associées à un DataSet d'une dimension
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="dimensionId">Id de la Dimension</param>
        /// <returns>Message de retour</returns>
        public async Task<IEnumerable<DistinctValue>> GetDistinctValueByDimension(long dataSetId, long dimensionId)
        {
            IEnumerable<DistinctValue> distinctValues = await UnitOfWork.GetDbContext().DistinctValue
                .Where(dv => dv.DataSetId == dataSetId && dv.DimensionId == dimensionId)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return distinctValues;
        }

        /// <summary>
        /// Lie une liste de ValueObject à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="valueObjectIds">Liste des ids des ValueObject à lier.</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> BindValueObjectToDataSet(long dataSetId, IEnumerable<long> valueObjectIds)
        {
            DataSet dataSet = await UnitOfWork.DataSetRepository.GetById(dataSetId);
            if (dataSet == null)
                throw new WrongParameterException($"DataSetDomain.BindValueObjectToDataSet : DataSet (id = {dataSetId}) don't exist.");

            List<ValueObject> lstVo = await UnitOfWork.GetDbContext().ValueObject
                .Where(vo => valueObjectIds.Contains(vo.Id))
                .ToAsyncEnumerable()
                .ToList();

            foreach (ValueObject valObj in lstVo)
            {
                UnitOfWork.ValueObjectRepository.PrepareUpdateForObject(valObj);

                valObj.DataSet = dataSet;
            }
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return new HttpResponseMessageResult() { IsSuccess = (nbr > 0) };
        }
    }

}
