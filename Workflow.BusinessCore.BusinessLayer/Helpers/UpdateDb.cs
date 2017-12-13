using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class UpdateDb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateDbEntryAsync<T>(T entity, IUnitOfWork unitOfWork, params Expression<Func<T, object>>[] properties) where T : class
        {
            try
            {
                var db = unitOfWork.GetDbContext();

                var entry = db.Entry(entity);
                db.Set<T>().Attach(entity);
                foreach (var property in properties)
                    entry.Property(property).IsModified = true;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpdateDbEntryAsync exception: " + ex.Message);
                return false;
            }
        }
    }
}
