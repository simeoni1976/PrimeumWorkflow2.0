using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// User domain class
    /// </summary>
    public class UserDomain : AbstractDomain<User>, IUserDomain
    {
        private readonly IServiceProvider _serviceProvider;


        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }


        private ISelectorInstanceDomain SelectorInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorInstanceDomain>();
            }
        }



        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public UserDomain(IServiceProvider serviceProvider, IUnitOfWork unitOfWork) : base(unitOfWork.UserRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This function permits to get the roles from an user.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>User</returns>
        public async Task<List<UserSet>> GetUserSets(string employeeId)
        {
            IEnumerable<User> users = await UnitOfWork.UserRepository.Find(f => f.EmployeeID == employeeId);
            IEnumerable<UserSetUser> userSetUsers =  await UnitOfWork.UserSetUserRepository.Find(f => f.User == users.First());
            IEnumerable<UserSet> userSets = await UnitOfWork.UserSetRepository.Find(f => f.UserSetUser == userSetUsers);

            return userSets.ToList();
        }

        /// <summary>
        /// Renvoit la liste des SelectorInstance dont le loginModificator est modificateur (en attente de modification).
        /// </summary>
        /// <param name="modificatorId">Id de l'utilisateur recherché</param>
        /// <param name="workflowInstanceId">Id du workflowInstance cible</param>
        /// <returns>Liste des SelectorInstance</returns>
        public async Task<IEnumerable<SelectorInstance>> GetSelectorInstanceByModificator(long modificatorId, long workflowInstanceId)
        {
            List<SelectorInstance> lstSel = new List<SelectorInstance>();

            User modificateur = await UnitOfWork.UserRepository.GetById(modificatorId);
            if (modificateur == null)
                return lstSel;

            lstSel = await UnitOfWork.GetDbContext().SelectorInstance
                .Include(si => si.WorkflowInstance)
                .Where(si => 
                    si.WorkflowInstance.Id == workflowInstanceId && 
                    si.ModifyerId == modificateur.Id && 
                    (si.Status == SelectorStateEnum.Init || si.Status == SelectorStateEnum.Constraint))
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return lstSel;
        }

        /// <summary>
        /// Enregistre une série de nouveaux utilisateurs.
        /// </summary>
        /// <param name="users">Liste de nouveaux utilisateurs.</param>
        /// <returns>Renvoit la liste des ids des utilisateurs.</returns>
        public async Task<IEnumerable<long>> Import(IEnumerable<User> users)
        {
            if (users == null)
                throw new WrongParameterException("UserDomain.Import : List's users is null.");

            List<User> usersToAdd = new List<User>();
            foreach (User u in users)
            {
                if (u == null)
                    continue;

                int cnt = await UnitOfWork.GetDbContext().Users
                    .Where(ud => ud.Email == u.Email || ud.EmployeeID == u.EmployeeID || ud.Login == u.Login)
                    .CountAsync();
                if (cnt > 0)
                    continue;

                if (!StringHelper.IsValidEmail(u.Email))
                    continue;

                UnitOfWork.UserRepository.PrepareAddForObject(u);
                usersToAdd.Add(u);
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();
            if (nbr <= 0)
                throw new DatabaseException("UserDomain.Import : impossible to save Users.");

            List<long> lstIds = new List<long>();
            foreach (User u in usersToAdd)
                lstIds.Add(u.Id);

            return lstIds;
        }

        /// <summary>
        /// Récupére un utilisateur par son login.
        /// </summary>
        /// <param name="login">Login recherché</param>
        /// <returns>Utilisateur</returns>
        public async Task<User> GetByLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new WrongParameterException("UserDomain.GetByLogin : login can't be null.");
            User user = await UnitOfWork.GetDbContext().Users
                .Where(u => u.Login == login)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return user;
        }

        /// <summary>
        /// Récupére les WorkflowInstances dans lesquels un utilisateur opére.
        /// </summary>
        /// <param name="userId">Id utilisateur</param>
        /// <returns>Liste de WorkflowInstance</returns>
        public async Task<IEnumerable<WorkflowInstance>> GetWorkflowInstance(long userId)
        {
            User user = await UnitOfWork.UserRepository.GetById(userId);
            if (user == null)
                throw new WrongParameterException($"UserDomain.GetWorkflowInstance : User with id = {userId} don't exist.");
            List<long> dsIds = await UnitOfWork.GetDbContext().UserSetUser
                .Include(usu => usu.User)
                .Include(usu => usu.UserSet)
                .Where(usu => usu.User.Id == user.Id)
                .Select(usu => usu.UserSet.Id)
                .ToAsyncEnumerable()
                .ToList();

            List<WorkflowInstance> wfIns = null;
            if ((dsIds != null) && (dsIds.Count > 0))
                wfIns = await UnitOfWork.GetDbContext().WorkflowInstance
                    .Where(wf => dsIds.Contains(wf.UserSetId) && wf.Status == WorkflowStateEnum.Current)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .ToList();

            return wfIns;
        }

        /// <summary>
        /// Récupére les SelectorInstance pour lesquels l'utilisateur est un validateur.
        /// </summary>
        /// <param name="validatorId">Id de l'utilisateur validateur</param>
        /// <param name="workflowInstanceId">Id du WorkflowInstance</param>
        /// <returns>Liste des SelectorInstance</returns>
        public async Task<IEnumerable<SelectorInstance>> GetSelectorInstanceByValidator(long validatorId, long workflowInstanceId)
        {
            List<SelectorInstance> lstSel = new List<SelectorInstance>();

            User validateur = await UnitOfWork.UserRepository.GetById(validatorId);
            if (validateur == null)
                return lstSel;

            lstSel = await UnitOfWork.GetDbContext().SelectorInstanceUser
                .Include(siu => siu.SelectorInstance)
                .ThenInclude(si => si.WorkflowInstance)
                .Where(siu => siu.UserId == validatorId &&
                              siu.SelectorInstance.WorkflowInstance.Id == workflowInstanceId &&
                              siu.SelectorInstance.Status == SelectorStateEnum.Validate)
                .Select(siu => siu.SelectorInstance)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            return lstSel;
        }

    }
}
