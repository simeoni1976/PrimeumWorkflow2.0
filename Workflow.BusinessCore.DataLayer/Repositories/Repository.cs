using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.BusinessCore.DataLayer.Helpers;
using Workflow.BusinessCore.DataLayer.Repositories.Interfaces;

namespace Workflow.BusinessCore.DataLayer.Repositories
{
    /// <summary>
    /// Repository generic class.
    /// </summary>
    /// <typeparam name="TEntity">Data model</typeparam>
    /// <remarks>
    /// This class permits to do all the entity events.
    /// </remarks>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationContext _dbContext;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Repository()
        {
            _dbContext = new ApplicationContext();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Repository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        ///  This function permits to get all.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// This function permits to get an item by an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetById(long id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// This function permits to get an item by criterias.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> where)
        {
            return await _dbContext.Set<TEntity>().Where(where).ToAsyncEnumerable().ToList();
        }

        /// <summary>
        ///  This function permits to count number of items in an entity.
        /// </summary>
        /// <returns></returns>
        public async Task<int> Count()
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }

        /// <summary>
        /// This function permits to get items by LinQ parameters.
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
        public async Task<IEnumerable<TReturn>> SearchFor<TResult, TKey, TGroup, TReturn>(
            List<Expression<Func<TEntity, bool>>> where,
            Expression<Func<TEntity, TResult>> firstSelector,
            Expression<Func<TResult, TKey>> orderSelector,
            Func<TResult, TGroup> groupSelector,
            Func<IGrouping<TGroup, TResult>, TReturn> selector,
            long? page = null,
            long? pageSize = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TReturn> enumerable;
            IQueryable<TEntity> dbQuery = _dbContext.Set<TEntity>();

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> property in includes)
                dbQuery = dbQuery.Include(property);

            enumerable = where
                .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                .Select(firstSelector)
                .OrderBy(orderSelector)
                .GroupBy(groupSelector)
                .Select(selector);

            //Paging
            if (page != null && page > 0)
            {
                enumerable = enumerable.Skip(((int)page - 1) * (int)pageSize);
            }

            if (pageSize != null && pageSize > 0)
            {
                enumerable = enumerable.Take((int)pageSize);
            }

            return await Task.FromResult(enumerable);
        }

        /// <summary>
        /// This function permits to execute a simple query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> ExecuteReader(string query, params object[] parameters)
        {
            List<TEntity> results = new List<TEntity>();
            DbConnection connection = _dbContext.Database.GetDbConnection();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                    if (reader.HasRows)
                    {
                        results = MapToList<TEntity>(reader);
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                connection.Close();
            }

            return results;
        }

        /// <summary>
        /// This function permits to execute a query for update or delete.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task ExecuteSqlCommand(string query, CancellationToken token = default(CancellationToken), params object[] parameters)
        {
            await _dbContext.Database.ExecuteSqlCommandAsync(query, token, parameters);
        }

        /// <summary>
        /// This function permits to add a new item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        public async Task<TEntity> Insert(TEntity entity)
        {
            // TODO : passer le nom de l'utilisateur (voir avec l'authentification sur les WebAPI).
            entity.InitDatesAndUser(string.IsNullOrWhiteSpace(entity.Username) ? "" : entity.Username);
            _dbContext.Entry(entity).State = EntityState.Added;
            _dbContext.Set<TEntity>().Add(entity);

            //Permit to insert a identity value
            //IRelationalEntityTypeAnnotations mapping = _dbContext.Model.FindEntityType(typeof(TEntity).FullName).Relational();
            //string nomTable = mapping.TableName;
            //_dbContext.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT {nomTable} ON");
            int nbr = await _dbContext.SaveChangesAsync();
            //_dbContext.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT {nomTable} OFF");

            return entity;
        }

        /// <summary>
        /// This function permits to udpdate an item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        /// <returns>TEntity</returns>
        public async Task<TEntity> Update(TEntity entity)
        {
            entity.SetModifiedDate();
            TEntity ent = _dbContext.Set<TEntity>().Local.FirstOrDefault(c => c.Id == entity.Id);
            if (ent != null)
                _dbContext.Entry<TEntity>(ent).State = EntityState.Detached;
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            int nbr = await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// This method permits to delete an item.
        /// </summary>
        /// <param name="entity">TEntity</param>
        public async Task<int> Delete(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Prépare l'entité à être ajouté.
        /// </summary>
        /// <param name="entity">Objet entity.</param>
        public void PrepareAddForObject(TEntity entity)
        {
            // TODO : passer le nom de l'utilisateur (voir avec l'authentification sur les WebAPI).
            entity.InitDatesAndUser(string.IsNullOrWhiteSpace(entity.Username) ? "" : entity.Username);
            _dbContext.Entry(entity).State = EntityState.Added;
            _dbContext.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Prépare l'entité à être mise à jour.
        /// </summary>
        /// <param name="entity">Objet entity</param>
        public void PrepareUpdateForObject(TEntity entity)
        {
            entity.SetModifiedDate();
            TEntity ent = _dbContext.Set<TEntity>().Local.FirstOrDefault(c => c.Id == entity.Id);
            if (ent != null)
                _dbContext.Entry<TEntity>(ent).State = EntityState.Detached;
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }


        /// <summary>
        /// This funciton permits to map data from DataReader to a List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private List<T> MapToList<T>(DbDataReader dr)
        {
            List<T> RetVal = null;
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();
            try
            {
                if (dr != null && dr.HasRows)
                {
                    RetVal = new List<T>();
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (dr.Read())
                    {
                        T newObject = Activator.CreateInstance<T>();
                        for (int Index = 0; Index < dr.FieldCount; Index++)
                        {
                            if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                            {
                                var Info = PropDict[dr.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = dr.GetValue(Index);
                                    Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                }
                            }
                        }
                        RetVal.Add(newObject);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RetVal;
        }

        /// <Summary>
        /// This function permits to map data from DataReader to an object.
        /// </Summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dr">Data Reader</param>
        /// <returns>Object having data from Data Reader</returns>
        private T MapToSingle<T>(DbDataReader dr) where T : new()
        {
            T RetVal = new T();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();
            try
            {
                if (dr != null && dr.HasRows)
                {
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    dr.Read();
                    for (int Index = 0; Index < dr.FieldCount; Index++)
                    {
                        if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                        {
                            var Info = PropDict[dr.GetName(Index).ToUpper()];
                            if ((Info != null) && Info.CanWrite)
                            {
                                var Val = dr.GetValue(Index);
                                Info.SetValue(RetVal, (Val == DBNull.Value) ? null : Val, null);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RetVal;
        }
    }
} 

