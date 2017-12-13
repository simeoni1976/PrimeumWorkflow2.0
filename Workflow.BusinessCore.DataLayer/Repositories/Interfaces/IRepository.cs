using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Workflow.BusinessCore.DataLayer.Repositories.Interfaces
{
    /// <summary>
    /// IRepository generic interface.
    /// Repository generic class.
    /// </summary>
    /// <remarks>
    /// This generic class permits to do very complex scenarios.
    /// This class permits to define all methods for the repository.
    /// </remarks>
    /// <typeparam name="TEntity">Data model</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// This function permits to get all.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// This function permits to get an item by an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetById(Int64 id);

        /// <summary>
        /// This function permits to get all by criteria.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> where);

        /// <summary>
        ///  This function permits to count number of items in an entity.
        /// </summary>
        /// <returns></returns>
        Task<int> Count();

        /// <summary>
        /// This function permits to get items by criterias.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TGroup"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="where"></param>
        /// <param name="firstSelector"></param>
        /// <param name="orderSelector"></param>
        /// <param name="groupSelector"></param>
        /// <param name="selector"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<TReturn>> SearchFor<TResult, TKey, TGroup, TReturn>(
            List<Expression<Func<TEntity, bool>>> where,
            Expression<Func<TEntity, TResult>> firstSelector,
            Expression<Func<TResult, TKey>> orderSelector,
            Func<TResult, TGroup> groupSelector,
            Func<IGrouping<TGroup, TResult>, TReturn> selector,
            long? page = null,
            long? pageSize = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// This function permits to execute a stored procedure.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> ExecuteReader(string query, params object[] parameters);

        /// <summary>
        /// This function permits to execute a stored procedure for update database.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        Task ExecuteSqlCommand(string query, CancellationToken token = default(CancellationToken), params object[] parameters);

        /// <summary>
        /// This function permits to add a new item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        Task<TEntity> Insert(TEntity entity);

        /// <summary>
        /// This function permits to udpdate an item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        Task<TEntity> Update(TEntity entity);

        /// <summary>
        /// This method permits to delete an item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        Task<int> Delete(TEntity entity);

        /// <summary>
        /// Prépare l'entité à être ajouté.
        /// </summary>
        /// <param name="entity">Objet entity.</param>
        void PrepareAddForObject(TEntity entity);

        /// <summary>
        /// Prépare l'entité à être mise à jour.
        /// </summary>
        /// <param name="entity">Objet entity</param>
        void PrepareUpdateForObject(TEntity entity);
    }
}
