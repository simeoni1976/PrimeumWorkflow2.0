using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Workflow.Transverse.Environment;
using Microsoft.EntityFrameworkCore;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// User domain class
    /// </summary>
    public class UserSetDomain : AbstractDomain<UserSet>, IUserSetDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private IDataSetDimensionDomain DataSetDimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDimensionDomain>();
            }
        }
        private ICriteriaValuesDomain CriteriaValuesDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaValuesDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public UserSetDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.UserSetRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsers(long userSetId)
        {
            //UserSet userSet = await UnitOfWork.UserSetRepository.GetById(userSetId);
            //IEnumerable<UserSetUser> userSetUsers = await UnitOfWork.UserSetUserRepository.Find(f => f.UserSet == userSet);
            //IEnumerable<User> users = await UnitOfWork.UserRepository.Find(f => f.UserSetUser == userSetUsers);

            //return users.ToList();

            // TODO : a refaire -> comparaison de deux listes : ?

            return null;
        }

        /// <summary>
        /// Donne les utilisateurs (dans une liste de UserSetUser) chargé d'un droit par rapport aux CriteriaValues passés en paramétre.
        /// </summary>
        /// <param name="criteriaValues">Liste de criteriaValues</param>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="right">Droit recherché</param>
        /// <returns>Liste des utilisateurs.</returns>
        /// <remarks>La liste de UserSetUser peut contenir 0, 1 ou n utilisateurs (non distinct).</remarks>
        public async Task<IEnumerable<UserSetUser>> GetUsersByRight(IEnumerable<IEnumerable<CriteriaValues>> criteriaValues, long dataSetId, RightEnum right)
        {
            Dictionary<long, DataSetDimension> dimIds = await DataSetDimensionDomain.GetDimensionColumns(dataSetId);
            Dictionary<long, IEnumerable<CriteriaValues>> dico = CriteriaValuesDomain.GetCriteriaValuesByDimension(criteriaValues);

            List<Expression<Func<UserSetUser, bool>>> where = new List<Expression<Func<UserSetUser, bool>>>();

            foreach (long dimensionId in dico.Keys)
            {
                string nomDimension = dimIds.Values.Where(d => d.Dimension.Id == dimensionId).Select(d => d.ColumnName).FirstOrDefault();
                IEnumerable<string> values = dico[dimensionId].Select(cv => cv.Value);

                if (values.Count() > 1)
                    where.Add(GetFilterByDimension(nomDimension, values));
                else
                    if (values.Count() == 1)
                    where.Add(GetFilterByDimension(nomDimension, values.ElementAt(0)));
            }

            IQueryable<UserSetUser> dbQuery = UnitOfWork.GetDbContext().Set<UserSetUser>();
            IEnumerable<UserSetUser> lstUsu = await where
                .Aggregate(dbQuery, (current, predicate) => current.Where(predicate))
                .Where(usu => (usu.Right & right) == right)
                .Include(usu => usu.User)
                .Include(usu => usu.UserSet)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return lstUsu;
        }


        /// <summary>
        /// Ajoute un UserSet.
        /// </summary>
        /// <param name="userSet">Nouveau UserSet</param>
        /// <returns>UserSet enregistré</returns>
        public override async Task<UserSet> Add(UserSet userSet)
        {
            if (userSet == null)
                throw new WrongParameterException("UserSetDomain.Add : UserSet is null.");

            UnitOfWork.UserSetRepository.PrepareAddForObject(userSet);

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("UserSetDomain.Add : impossible to save UserSet.");

            return userSet;
        }


        /// <summary>
        /// Lie les utilisateurs donnés en entrée à un UserSet.
        /// </summary>
        /// <param name="userSetId">Id de l'UserSet</param>
        /// <param name="userSetUser">Liste des liens d'utilisateur</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> BindUserToUserSet(long userSetId, IEnumerable<UserSetUser> userSetUser)
        {
            if (userSetUser == null)
                throw new WrongParameterException("UserSetDoamin.BindUserToUserSet : List's UserSetUser is null.");
            UserSet userSet = await UnitOfWork.UserSetRepository.GetById(userSetId);
            if (userSet == null)
                throw new WrongParameterException($"UserSetDoamin.BindUserToUserSet : no UserSet for id = {userSetId}");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            foreach (UserSetUser usu in userSetUser)
            {
                if (usu.User == null)
                {
                    res.Message += $"UserSetUser (id = {usu.Id}) has not set User id.";
                    continue;
                }
                User u = await UnitOfWork.UserRepository.GetById(usu.User.Id);
                if (u == null)
                {
                    res.Message += $"UserSetUser's User (id = {usu.User.Id}) don't exist.";
                    continue;
                }

                UnitOfWork.UserSetUserRepository.PrepareAddForObject(usu);
                usu.UserSet = userSet;
                usu.User = u;
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();
            if (nbr <= 0)
                throw new DatabaseException("UserSetDoamin.BindUserToUserSet : impossible to save UserSetUser.");

            return res;
        }

        private Expression<Func<UserSetUser, bool>> GetFilterByDimension(string nomDimension, IEnumerable<string> values)
        {
            if (nomDimension == Constant.DATA_DIMENSION_1)
                return usu => values.Contains(usu.Position1);
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return usu => values.Contains(usu.Position2);
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return usu => values.Contains(usu.Position3);
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return usu => values.Contains(usu.Position4);
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return usu => values.Contains(usu.Position5);
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return usu => values.Contains(usu.Position6);
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return usu => values.Contains(usu.Position7);
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return usu => values.Contains(usu.Position8);
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return usu => values.Contains(usu.Position9);
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return usu => values.Contains(usu.Position10);
            return null;
        }

        private Expression<Func<UserSetUser, bool>> GetFilterByDimension(string nomDimension, string value)
        {
            if (nomDimension == Constant.DATA_DIMENSION_1)
                return usu => value == usu.Position1;
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return usu => value == usu.Position2;
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return usu => value == usu.Position3;
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return usu => value == usu.Position4;
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return usu => value == usu.Position5;
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return usu => value == usu.Position6;
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return usu => value == usu.Position7;
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return usu => value == usu.Position8;
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return usu => value == usu.Position9;
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return usu => value == usu.Position10;
            return null;
        }
    }
}
