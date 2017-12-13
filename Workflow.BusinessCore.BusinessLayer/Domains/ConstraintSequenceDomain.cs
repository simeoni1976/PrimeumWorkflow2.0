using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Transverse.Environment;
using Microsoft.EntityFrameworkCore;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    public class ConstraintSequenceDomain : IConstraintSequenceDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private ILogger Logger
        {
            get
            {
                return _serviceProvider?.GetService<ILogger<ActionSequenceDomain>>();
            }
        }

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private IConfigVariableDomain ConfigVariableDomain
        {
            get
            {
                return _serviceProvider?.GetService<IConfigVariableDomain>();
            }
        }

        private IValueObjectDomain ValueObjectDomain
        {
            get
            {
                return _serviceProvider?.GetService<IValueObjectDomain>();
            }
        }

        private IGridConfigurationDomain GridConfigurationDomain
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationDomain>();
            }
        }

        /// <summary>
        /// Constructeur, pour l'ID
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services.</param>
        public ConstraintSequenceDomain(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Permet de vérifier toutes les contraintes dans l'ordre d'une séquence de contraintes sur un SelectorInstance.
        /// </summary>
        /// <param name="sequences">Sequences de contraintes</param>
        /// <param name="selectorInstance">SelectorInstance cible</param>
        /// <param name="workflowInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> CheckConstraint(Constraint contrainte, long referenceSequence, int ordreSequence, ConstraintLevelEnum level, SelectorInstance selectorInstance, WorkflowInstance workflowInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            if (contrainte.Type == ConstraintTypeEnum.Hardcode)
            {
                if (contrainte.Name == Constant.CONSTRAINT_TREESUM)
                    return await CheckTreeSum(referenceSequence, ordreSequence, level, selectorInstance, workflowInstance);
                //if (contrainte.Name == Constant.ACTION_SPLIT_COPY)
                //    ;
                //if (contrainte.Name == Constant.ACTION_AGREGATE_PRIMEUM)
                //    ;
            }
            if (contrainte.Type == ConstraintTypeEnum.Dynamic)
            {
                // TODO
            }

            return res;
        }

        /// <summary>
        /// Vérifie une contrainte sur un SelectorInstance.
        /// </summary>
        /// <param name="contrainte">Contrainte</param>
        /// <param name="referenceSequence">Reference de la sequence de contrainte</param>
        /// <param name="ordreSequence">Ordre de la contrainte dans la sequence</param>
        /// <param name="level">Niveau d'alerte si la vérification échoue</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="workflowInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> CheckConstraintSequence(IEnumerable<ConstraintSequence> sequences, SelectorInstance selectorInstance, WorkflowInstance workflowInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            foreach (ConstraintSequence seq in sequences.OrderBy(s => s.Order))
            {
                if (seq.Constraint == null)
                {
                    res.IsSuccess = false;
                    res.Message = $"ConstraintSequenceDomaine.CheckConstraintSequence : Constraint is null for sequence {seq.SequenceName} of SelectorInstance {selectorInstance.Id}";
                    continue;
                }

                Logger.LogInformation($"Starting action {seq.Constraint.Name} of sequence {seq.SequenceName}.");

                res.Append(await CheckConstraint(seq.Constraint, seq.Reference, seq.Order, seq.Level, selectorInstance, workflowInstance, values));
            }

            return res;
        }


        /// <summary>
        /// Ajoute une nouvelle contrainte (indépente des Wrokflow)
        /// </summary>
        /// <param name="constraint">Nouvelle contrainte</param>
        /// <returns>Message de retour</returns>
        public async Task<Constraint> AddConstraint(Constraint constraint)
        {
            if (constraint == null)
                throw new WrongParameterException("ConstraintSequenceDomain.AddConstraint : Constraint is null.");
            int cnt = await UnitOfWork.GetDbContext().Constraint
                .Where(c => c.Name == constraint.Name)
                .CountAsync();
            if (cnt > 0)
                throw new WrongParameterException($"ConstraintSequenceDomain.AddConstraint : Constraint's name ({constraint.Name}) is already existing.");

            UnitOfWork.ConstraintRepository.PrepareAddForObject(constraint);
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ConstraintSequenceDomain.AddConstraint : impossible to save Constraint.");

            return constraint;
        }

        /// <summary>
        /// Ajoute une nouvelle séquence de contrainte.
        /// </summary>
        /// <param name="constraintSequence">Nouvelle SequenceConstraint</param>
        /// <param name="constraintId">Id de la contrainte</param>
        /// <returns>Message de retour</returns>
        public async Task<ConstraintSequence> AddConstraintSequence(ConstraintSequence constraintSequence, long constraintId)
        {
            if (constraintSequence == null)
                throw new WrongParameterException("ConstraintSequenceDomain.AddConstraintSequence : ConstraintSequence is null.");
            Constraint constraint = await UnitOfWork.ConstraintRepository.GetById(constraintId);
            if (constraint == null)
                throw new WrongParameterException($"ConstraintSequenceDomain.AddConstraintSequence : Constraint (id = {constraintId}) don't exist.");

            UnitOfWork.ConstraintSequenceRepository.PrepareAddForObject(constraintSequence);
            constraintSequence.Constraint = constraint;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ConstraintSequenceDomain.AddConstraintSequence : impossible to save ConstraintSequence.");

            return constraintSequence;
        }

        /// <summary>
        /// Ajoute un nouveau ConstraintParameter
        /// </summary>
        /// <param name="constraintParameter">Nouveau ConstraintParameter</param>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>Message de retour</returns>
        public async Task<ConstraintParameter> AddConstraintParameter(ConstraintParameter constraintParameter, long constraintSequenceId)
        {
            if (constraintParameter == null)
                throw new WrongParameterException("ConstraintSequenceDomain.AddConstraintParameter : constraintParameter is null.");
            int cnt = await UnitOfWork.GetDbContext().ConstraintParameter
                .Where(cp => cp.ParameterName == constraintParameter.ParameterName)
                .CountAsync();
            if (cnt > 0)
                throw new WrongParameterException($"ConstraintSequenceDomain.AddConstraintParameter : constraintParameter (parameter name = {constraintParameter.ParameterName}) is already existing.");
            ConstraintSequence constraintSequence = await UnitOfWork.ConstraintSequenceRepository.GetById(constraintSequenceId);
            if (constraintSequence == null)
                throw new WrongParameterException($"ConstraintSequenceDomain.AddConstraintParameter : ConstraintSequence (id = {constraintSequenceId}) don't exist.");

            UnitOfWork.ConstraintParameterRepository.PrepareAddForObject(constraintParameter);
            constraintParameter.ReferenceSequence = constraintSequence.Reference;
            constraintParameter.OrderSequence = constraintSequence.Order;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ConstraintSequenceDomain.AddConstraintParameter : impossible to save ConstraintParameter.");

            return constraintParameter;
        }

        /// <summary>
        /// Récupére toutes les Constraint
        /// </summary>
        /// <returns>Liste de Constraint</returns>
        public async Task<IEnumerable<Constraint>> GetAllConstraint()
        {
            return await UnitOfWork.ConstraintRepository.GetAll();
        }

        /// <summary>
        /// Recupére une Constraint par son Id.
        /// </summary>
        /// <param name="constraintId">Id de la Constraint</param>
        /// <returns>Constraint recherchée</returns>
        public async Task<Constraint> GetConstraint(long constraintId)
        {
            return await UnitOfWork.ConstraintRepository.GetById(constraintId);
        }

        /// <summary>
        /// Récupére toutes les ConstraintSequence
        /// </summary>
        /// <returns>Liste de ConstraintSequence</returns>
        public async Task<IEnumerable<ConstraintSequence>> GetAllConstraintSequence()
        {
            return await UnitOfWork.ConstraintSequenceRepository.GetAll();
        }

        /// <summary>
        /// Récupére une ConstraintSequence par son Id
        /// </summary>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>ConstraintSequence recherchée</returns>
        public async Task<ConstraintSequence> GetConstraintSequence(long constraintSequenceId)
        {
            return await UnitOfWork.ConstraintSequenceRepository.GetById(constraintSequenceId);
        }

        /// <summary>
        /// Récupére tous les ConstraintParameter
        /// </summary>
        /// <returns>Liste des ConstraintParameter</returns>
        public async Task<IEnumerable<ConstraintParameter>> GetAllConstraintParameter()
        {
            return await UnitOfWork.ConstraintParameterRepository.GetAll();
        }

        /// <summary>
        /// Récupére un ConstraintParameter par son Id.
        /// </summary>
        /// <param name="constraintParameterId">Id du ConstraintParameter</param>
        /// <returns>ConstraintParameter recherché</returns>
        public async Task<ConstraintParameter> GetConstraintParameter(long constraintParameterId)
        {
            return await UnitOfWork.ConstraintParameterRepository.GetById(constraintParameterId);
        }



        private async Task<HttpResponseMessageResult> CheckTreeSum(long referenceSequence, int ordreSequence, ConstraintLevelEnum level, SelectorInstance selectorInstance, WorkflowInstance workflowInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            await ConfigVariableDomain.LoadVariables();

            string nomDimension = await GetConstraintParameter(referenceSequence, ordreSequence, Constant.PARAMETER_CONSTRAINT_ALIGNMENTNAME);

            Dictionary<long, Tuple<ValueObject, int, string, bool>> dicVo = await ValueObjectDomain.CreateDictionaryVO(nomDimension, selectorInstance);

            // Construction des arbres
            int topLvl = dicVo.Values.Select(t => t.Item2).Min();
            int bottomLvl = dicVo.Values.Select(t => t.Item2).Max();
            IEnumerable<TreeValueObject> lstTree = ValueObjectDomain.BuildTreeVO(dicVo, nomDimension, topLvl, bottomLvl);

            // On parcours les arbres et on vérifie les valeurs
            HashSet<long> idsAlreadyComputed = new HashSet<long>();
            //Dictionary<long, Tuple<double, double>> outOfConstraint = new Dictionary<long, Tuple<double, double>>();
            Dictionary<long, object> outOfConstraint = new Dictionary<long, object>();
            foreach (TreeValueObject tvo in lstTree)
            {
                string format = await ValueObjectDomain.GetNumericalFormat(tvo.Node.TypeValue, workflowInstance.DataSetId);

                for (int currentLvl = bottomLvl; currentLvl > topLvl; currentLvl--)
                {
                    IEnumerable<TreeValueObject> nodes = TreeValueObject.GetNodesFromLevel(tvo, currentLvl);
                    foreach (TreeValueObject child in nodes)
                    {
                        if ((child.Parent == null) || idsAlreadyComputed.Contains(child.Parent.Node.Id))
                            continue;

                        double sumComputed = child.Parent.Children.Select(subnode => ValueObjectDomain.GetMostCurrentValue(subnode.Node)).Sum();
                        double sumParent = ValueObjectDomain.GetMostCurrentValue(child.Parent.Node);

                        if (!ValueObjectHelper.AlmostEqual(sumComputed, sumParent, format))
                            outOfConstraint.Add(child.Parent.Node.Id, new { expected = sumParent, computed = sumComputed });

                        idsAlreadyComputed.Add(child.Parent.Node.Id);
                    }
                }
            }

            if (outOfConstraint.Count > 0)
            {
                res.Json = JsonConvert.SerializeObject(outOfConstraint);
                res.Message = "Some values are not sum of their children.";

                if ((level == ConstraintLevelEnum.Info) || (level == ConstraintLevelEnum.Warning))
                    res.IsSuccess = true;
                if (level == ConstraintLevelEnum.Error)
                    res.IsSuccess = false;
            }

            return res;
        }


        private async Task<string> GetConstraintParameter(long referenceSequence, int OrderSequence, string parameterName)
        {
            string nomDimension = await UnitOfWork.GetDbContext().ConstraintParameter
                .Where(ap => ap.ReferenceSequence == referenceSequence && ap.OrderSequence == OrderSequence && ap.ParameterName == parameterName)
                .Select(ap => ap.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(nomDimension))
                throw new WrongParameterException($"Constraint.GetConstraintParameter : Parameter {parameterName} not found!");

            return nomDimension;
        }
    }
}
