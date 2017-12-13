using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Repositories.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Common
{
    /// <summary>
    /// AbstractDomain abstract class.
    /// </summary>
    /// <typeparam name="TEntity">TEntity</typeparam>
    public abstract class AbstractDomain<TEntity>
        where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="repository">Repository</param>
        public AbstractDomain(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// This method permits to get all elements in the base.
        /// </summary>
        /// <returns>IEnumerable</returns>
        public virtual async Task<IEnumerable<TEntity>> Get()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        /// This method permits to get an element in the base by an id.
        /// </summary>
        /// <param name="id">TEntity id</param>
        /// <returns>TEntity</returns>
        public async Task<TEntity> Get(long id)
        {
            return await _repository.GetById(id);
        }

        /// <summary>
        /// This function permits to count result items.
        /// </summary>
        /// <returns>Long</returns>
        public async Task<long> Count()
        {
            return await _repository.Count(); 
        }

        /// <summary>
        /// This method permits to add an element in the base.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            return await _repository.Insert(entity);
        }

        /// <summary>
        /// This method permits to delete an element in the base.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        public async Task Delete(TEntity entity)
        {
            await _repository.Delete(entity);
        }

        /// <summary>
        /// This method permits to delete an element in the base by an id.
        /// </summary>
        /// <param name="id">TEntity Id</param>
        public async Task Delete(long id)
        {
            await _repository.Delete(await Get(id));
        }

        /// <summary>
        /// This method permits to update an element in the base.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        public async Task<TEntity> Update(TEntity entity)
        {
            return await _repository.Update(entity);
        }
    }
}
