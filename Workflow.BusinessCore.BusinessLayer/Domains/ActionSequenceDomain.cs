using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Transverse.Environment;
using Microsoft.EntityFrameworkCore;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    public class ActionSequenceDomain : IActionSequenceDomain
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
        /// Constructeur par défaut
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ActionSequenceDomain(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Permet d'exécuter toutes les actions, dans l'ordre, d'une séquence sur un SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="sequences">Sequence d'actions</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> ExecuteActionSequence(IEnumerable<ActionSequence> sequences, SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            foreach (ActionSequence seq in sequences.OrderBy(s => s.Order))
            {
                if (seq.Action == null)
                {
                    res.IsSuccess = false;
                    res.Message = $"ActionSequenceDomaine.ExecuteActionSequence : Action is null for sequence {seq.SequenceName} of SelectorInstance {selectorInstance.Id}";
                    continue;
                }

                Logger.LogInformation($"Starting action {seq.Action.Name} of sequence {seq.SequenceName}.");

                res.Append(await ExecuteAction(seq.Action, seq.Reference, seq.Order, selectorInstance, wfInstance, values));
            }

            return res;
        }

        /// <summary>
        /// Exécute une action sur un SelectorInstance
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="referenceSequence">Référence de la séquence d'action</param>
        /// <param name="OrderSequence">Numéro d'ordre de l'action dans la séquence</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> ExecuteAction(ENT.Action action, long referenceSequence, int OrderSequence, SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            if (action.Type == ActionTypeEnum.Hardcode)
            {
                if (action.Name == Constant.ACTION_SPLIT_PRIMEUM)
                    return await ExecuteSplitPrimeum(referenceSequence, OrderSequence, selectorInstance, wfInstance);
                if (action.Name == Constant.ACTION_SPLIT_COPY)
                    return await ExecuteSplitCopy(referenceSequence, OrderSequence, selectorInstance, wfInstance);
                if (action.Name == Constant.ACTION_AGREGATE_PRIMEUM)
                    return await ExecuteAggregatePrimeum(referenceSequence, OrderSequence, selectorInstance, wfInstance);
            }
            if (action.Type == ActionTypeEnum.Dynamic)
            {
                // TODO
            }

            return res;
        }


        /// <summary>
        /// Ajoute une action en base, indépendamment d'un workflow.
        /// </summary>
        /// <param name="action">Nouvelle action</param>
        /// <returns>Message de retour</returns>
        public async Task<ENT.Action> AddAction(ENT.Action action)
        {
            if (action == null)
                throw new WrongParameterException("ActionSequenceDomain.AddAction : Action is null.");
            int cnt = await UnitOfWork.GetDbContext().Action
                .Where(a => a.Name == action.Name)
                .CountAsync();
            if (cnt > 0)
                throw new WrongParameterException($"ActionSequenceDomain.AddAction : Action ({action.Name}) is already existing.");

            UnitOfWork.ActionRepository.PrepareAddForObject(action);

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ActionSequenceDomain.AddAction : Impossible to save new Action.");

            return action;
        }

        /// <summary>
        /// Ajoute une nouvelle action dans une séquence (existante ou non)
        /// </summary>
        /// <param name="actionSequence">Nouvelle ActionSequence</param>
        /// <param name="actionId">Id de l'Action à exécuter</param>
        /// <returns>Message de retour</returns>
        public async Task<ActionSequence> AddActionSequence(ActionSequence actionSequence, long actionId)
        {
            if (actionSequence == null)
                throw new WrongParameterException("ActionSequenceDomain.AddAction : ActionSequence is null.");
            ENT.Action action = await UnitOfWork.GetDbContext().Action
                .Where(a => a.Id == actionId)
                .FirstOrDefaultAsync();
            if (action == null)
                throw new WrongParameterException($"ActionSequenceDomain.AddAction : Unknown Action with id = {actionId}.");

            UnitOfWork.ActionSequenceRepository.PrepareAddForObject(actionSequence);
            actionSequence.Action = action;
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ActionSequenceDomain.AddAction : impossible to save ActionSequence.");

            return actionSequence;
        }


        /// <summary>
        /// Ajoute un paramètre dans l'action d'une séquence d'action.
        /// </summary>
        /// <param name="actionParameter">ActionParameter à ajouter</param>
        /// <param name="actionSequenceId">Id de la séquence d'action cible</param>
        /// <returns>Message de retour</returns>
        public async Task<ActionParameter> AddActionParameter(ActionParameter actionParameter, long actionSequenceId)
        {
            if (actionParameter == null)
                throw new WrongParameterException("ActionSequenceDomain.AddAction : ActionParameter is null.");
            ActionSequence actionSequence = await UnitOfWork.GetDbContext().ActionSequence
                .Where(aseq => aseq.Id == actionSequenceId)
                .FirstOrDefaultAsync();
            if (actionSequence == null)
                throw new WrongParameterException($"ActionSequenceDomain.AddAction : ActionSequence don't exist with id = {actionSequenceId}.");

            UnitOfWork.ActionParameterRepository.PrepareAddForObject(actionParameter);
            actionParameter.ReferenceSequence = actionSequence.Reference;
            actionParameter.OrderSequence = actionSequence.Order;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ActionSequenceDomain.AddAction : Impossible to save ActionParameter.");

            return actionParameter;
        }

        /// <summary>
        /// Récupére toutes les actions existantes.
        /// </summary>
        /// <returns>Liste d'actions</returns>
        public async Task<IEnumerable<ENT.Action>> GetAllAction()
        {
            return await UnitOfWork.ActionRepository.GetAll();
        }

        /// <summary>
        /// Récupére une action par son Id
        /// </summary>
        /// <param name="actionId">Id de l'action</param>
        /// <returns>Action</returns>
        public async Task<ENT.Action> GetAction(long actionId)
        {
            return await UnitOfWork.ActionRepository.GetById(actionId);
        }

        /// <summary>
        /// Récupére toutes les ActionSequence.
        /// </summary>
        /// <returns>Liste d'ActionSequence</returns>
        public async Task<IEnumerable<ActionSequence>> GetAllActionSequence()
        {
            return await UnitOfWork.ActionSequenceRepository.GetAll();
        }

        /// <summary>
        /// Récupére une ActionSequence par son Id
        /// </summary>
        /// <param name="actionSequenceId">Id de l'ActionSequence</param>
        /// <returns>ActionSequence</returns>
        public async Task<ActionSequence> GetActionSequence(long actionSequenceId)
        {
            return await UnitOfWork.ActionSequenceRepository.GetById(actionSequenceId);
        }

        /// <summary>
        /// Récupére tous les ActionParameter
        /// </summary>
        /// <returns>Liste d'ActionParameter</returns>
        public async Task<IEnumerable<ActionParameter>> GetAllActionParameter()
        {
            return await UnitOfWork.ActionParameterRepository.GetAll();
        }

        /// <summary>
        /// Récupére un ActionParameter par son Id
        /// </summary>
        /// <param name="actionParameterId">Id de l'ActionParameter</param>
        /// <returns>ActionParameter</returns>
        public async Task<ActionParameter> GetActionParameter(long actionParameterId)
        {
            return await UnitOfWork.ActionParameterRepository.GetById(actionParameterId);
        }



        #region Split

        /// <summary>
        /// Méthode de split hardcodé
        /// </summary>
        /// <param name="referenceSequence">Référence de la séquence d'action</param>
        /// <param name="OrderSequence">Numéro d'ordre de l'action dans la séquence</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        private async Task<HttpResponseMessageResult> ExecuteSplitPrimeum(long referenceSequence, int OrderSequence, SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            // Récupération du nom de la dimension de la secto et du dictionnaire des objects.
            string nomDimension = await Split_GetDimensionName(referenceSequence, OrderSequence, wfInstance);
            Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo = await ValueObjectDomain.CreateDictionaryVO(nomDimension, selectorInstance);
            HashSet<KeyValuePair<long, double>> modifiedValues = new HashSet<KeyValuePair<long, double>>();

            // On créé la queue pour parcourir l'arbre procéduralement.
            Queue<ValueObject> voToSplit = new Queue<ValueObject>();
            foreach (ValueObject vo in dicAllVo.Values.Where(val => val.Item4).OrderBy(val => val.Item2).Select(val => val.Item1).ToList())
                voToSplit.Enqueue(vo);

            // Parcours des noeuds
            while (voToSplit.Count > 0)
            {
                ValueObject node = voToSplit.Dequeue();
                IEnumerable<ValueObject> children = GetChildren(node, nomDimension, dicAllVo);

                string format = await ValueObjectDomain.GetNumericalFormat(node.TypeValue, wfInstance.DataSetId);
                if ((children != null) && (children.Count() > 0))
                    Split_ApplyDivisionOnChildren(node, children, voToSplit, dicAllVo, modifiedValues, format);
            }

            // On sauve les modifications
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            // Mise à jour de la table temporaire
            res.Append(await GridConfigurationDomain.SaveDataInTemporyTable(selectorInstance, wfInstance, modifiedValues));

            return res;
        }

        /// <summary>
        /// Seconde méthode de répartition, par copie de valeur sur les noeuds inférieurs (typiquement pour des valeurs qui ne sont pas sommables).
        /// </summary>
        /// <param name="referenceSequence">Référence de la séquence d'action</param>
        /// <param name="OrderSequence">Numéro d'ordre de l'action dans la séquence</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        private async Task<HttpResponseMessageResult> ExecuteSplitCopy(long referenceSequence, int OrderSequence, SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            // Récupération du nom de la dimension de la secto et du dictionnaire des objects.
            string nomDimension = await Split_GetDimensionName(referenceSequence, OrderSequence, wfInstance);
            Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo = await ValueObjectDomain.CreateDictionaryVO(nomDimension, selectorInstance);
            HashSet<KeyValuePair<long, double>> modifiedValues = new HashSet<KeyValuePair<long, double>>();

            // On créé la queue pour parcourir l'arbre procéduralement.
            Queue<ValueObject> voToSplit = new Queue<ValueObject>();
            foreach (ValueObject vo in dicAllVo.Values.Where(val => val.Item4).OrderBy(val => val.Item2).Select(val => val.Item1).ToList())
                voToSplit.Enqueue(vo);

            // Parcours des noeuds
            while (voToSplit.Count > 0)
            {
                ValueObject node = voToSplit.Dequeue();
                IEnumerable<ValueObject> children = GetChildren(node, nomDimension, dicAllVo);

                if ((children != null) && (children.Count() > 0))
                    Split_CopyToOnChildren(node, children, voToSplit, dicAllVo, modifiedValues);
            }

            // On sauve les modifications
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            // Mise à jour de la table temporaire
            res.Append(await GridConfigurationDomain.SaveDataInTemporyTable(selectorInstance, wfInstance, modifiedValues));

            return res;
        }


        private async Task<string> Split_GetDimensionName(long referenceSequence, int OrderSequence, WorkflowInstance wfInstance)
        {
            string nomDimension = await GetActionParameter(referenceSequence, OrderSequence, Constant.PARAMETER_ACTION_SPLIT_PRIMEUM_ALIGNMENTNAME);
            await GetIdDimensionTree(nomDimension, Constant.PARAMETER_ACTION_SPLIT_PRIMEUM_ALIGNMENTNAME, wfInstance);

            return nomDimension;
        }


        private void Split_ApplyDivisionOnChildren(ValueObject parent, IEnumerable<ValueObject> children, Queue<ValueObject> qVoMod, Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo, HashSet<KeyValuePair<long, double>> modifiedValues, string format)
        {
            IEnumerable<ValueObject> childrenToApply = children.Where(vo => !dicAllVo[vo.Id].Item4);
            double total = ValueObjectDomain.GetMostCurrentValue(parent);
            double subTotalUnmodified = children.Where(vo => dicAllVo[vo.Id].Item4).Select(vo => ValueObjectDomain.GetMostCurrentValue(vo))?.Sum() ?? 0;
            double realTotal = total - subTotalUnmodified;
            double sumOldTargets = childrenToApply.Select(vo => ValueObjectDomain.GetMostCurrentValue(vo))?.Sum() ?? 0;
            double quota = sumOldTargets == 0 ? 0 : realTotal / sumOldTargets;

            double precision = ValueObjectHelper.GetPrecisionAbs(format);

            if (precision == 0)
                precision = 1;

            // Split
            double totalP = realTotal / precision;
            double deltaSum = 0;
            Dictionary <long, double> expectedValues = new Dictionary<long, double>();

            foreach (ValueObject child in childrenToApply)
            {
                if (expectedValues.ContainsKey(child.Id))
                    continue;

                double oldValue = ValueObjectDomain.GetMostCurrentValue(child) / precision;
                double newValue = oldValue * quota;
                double delta = newValue;
                newValue = Math.Floor(newValue);
                delta -= newValue;
                deltaSum += delta;

                expectedValues.Add(child.Id, newValue);
                qVoMod.Append(child);
            }

            // Arrondi
            double deltaSumRounded = Math.Round(deltaSum);
            foreach (KeyValuePair<long, double> kvp in expectedValues.OrderByDescending(k => k.Value))
            {
                double newValue = kvp.Value;

                if (deltaSumRounded >= 1)
                {
                    newValue += 1;
                    deltaSumRounded -= 1;
                }

                newValue = newValue * precision;
                dicAllVo[kvp.Key].Item1.FutureValue = newValue;
                modifiedValues.Add(new KeyValuePair<long, double>(kvp.Key, newValue));
            }
        }


        private void Split_CopyToOnChildren(ValueObject parent, IEnumerable<ValueObject> children, Queue<ValueObject> qVoMod, Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo, HashSet<KeyValuePair<long, double>> modifiedValues)
        {
            IEnumerable<ValueObject> childrenToCopy = children.Where(vo => !dicAllVo[vo.Id].Item4);
            double value = ValueObjectDomain.GetMostCurrentValue(parent);

            foreach (ValueObject child in childrenToCopy)
            {
                child.FutureValue = value;
                qVoMod.Append(child);
                modifiedValues.Add(new KeyValuePair<long, double>(child.Id, value));
            }
        }

        #endregion // Split


        #region Réagrégate

        private async Task<HttpResponseMessageResult> ExecuteAggregatePrimeum(long referenceSequence, int OrderSequence, SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            await ConfigVariableDomain.LoadVariables();

            string nomDimension = await GetActionParameter(referenceSequence, OrderSequence, Constant.PARAMETER_ACTION_AGGREGATE_PRIMEUM_ALIGNMENTNAME);
            long idDimension = await GetIdDimensionTree(nomDimension, Constant.PARAMETER_ACTION_AGGREGATE_PRIMEUM_ALIGNMENTNAME, wfInstance);

            // niveau utiisateur
            CriteriaValues cvTree = await UnitOfWork.GetDbContext().CriteriaValues
                .Include(cv => cv.Criteria)
                .ThenInclude(c => c.Dimension)
                .Where(cv => cv.SelectorInstanceModifier != null && cv.SelectorInstanceModifier.Id == selectorInstance.Id && cv.Criteria.Dimension.Id == idDimension)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            string valueTopUser = cvTree?.Value;
            int levelTopUser = 0;
            if (!string.IsNullOrWhiteSpace(valueTopUser))
                levelTopUser = valueTopUser.Split(ConfigVariableDomain.AlignmentChar).Length;

            Dictionary<long, Tuple<ValueObject, int, string, bool>> dicVo = await ValueObjectDomain.CreateDictionaryVO(nomDimension, selectorInstance);

            // Construction des arbres
            int topLvl = dicVo.Values.Select(t => t.Item2).Min();
            int bottomLvl = dicVo.Values.Select(t => t.Item2).Max();
            IEnumerable<TreeValueObject> lstTree = ValueObjectDomain.BuildTreeVO(dicVo, nomDimension, topLvl, bottomLvl);

            // On part du dernier niveau editable pour remonter jusqu'au niveau user.
            int bottom = dicVo.Values.Where(t => t.Item4).Select(t => t.Item2).Max();

            HashSet<long> idsAlreadyComputed = new HashSet<long>();
            HashSet<KeyValuePair<long, double>> modifiedValues = new HashSet<KeyValuePair<long, double>>();
            foreach (TreeValueObject tvo in lstTree)
            {
                for (int currentLvl = bottom; currentLvl > levelTopUser; currentLvl--)
                {
                    IEnumerable<TreeValueObject> nodes = TreeValueObject.GetNodesFromLevel(tvo, currentLvl);
                    foreach (TreeValueObject child in nodes)
                    {
                        if ((child.Parent == null) || idsAlreadyComputed.Contains(child.Parent.Node.Id) || (child.Parent.Level <= levelTopUser))
                            continue;

                        double sum = child.Parent.Children.Select(subnode => ValueObjectDomain.GetMostCurrentValue(subnode.Node)).Sum();
                        child.Parent.Node.FutureValue = sum;
                        modifiedValues.Add(new KeyValuePair<long, double>(child.Parent.Node.Id, sum));
                        idsAlreadyComputed.Add(child.Parent.Node.Id);
                    }
                }
            }

            // On sauve les modifications
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            // Mise à jour de la table temporaire
            res.Append(await GridConfigurationDomain.SaveDataInTemporyTable(selectorInstance, wfInstance, modifiedValues));

            return res;
        }



        #endregion // Réagrégate

        private async Task<string> GetActionParameter(long referenceSequence, int OrderSequence, string parameterName)
        {
            string nomDimension = await UnitOfWork.GetDbContext().ActionParameter
                .Where(ap => ap.ReferenceSequence == referenceSequence && ap.OrderSequence == OrderSequence && ap.ParameterName == parameterName)
                .Select(ap => ap.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(nomDimension))
                throw new WrongParameterException($"Action.GetActionParameter : Parameter {parameterName} not found!");

            return nomDimension;
        }

        private async Task<long> GetIdDimensionTree(string nomDimension, string parameterName, WorkflowInstance wfInstance)
        {
            DataSetDimension dsd = await UnitOfWork.GetDbContext().DataSetDimension
                .Include(d => d.DataSet)
                .Include(d => d.Dimension)
                .Where(d => d.DataSet.Id == wfInstance.DataSetId && d.ColumnName == nomDimension && d.Dimension.TypeDimension == DimensionTypeEnum.Tree)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (dsd == null)
                throw new WrongParameterException($"Action.CheckIsDimensionTree : Parameter {parameterName} = [{nomDimension}] doen't exist or isn't a tree dimension!");

            return dsd.Dimension.Id;
        }


        private IEnumerable<ValueObject> GetChildren(ValueObject node, string nomDimension, Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo)
        {
            if ((node == null) || (dicAllVo == null) || !dicAllVo.ContainsKey(node.Id))
                return null;

            IEnumerable<Expression<Func<ValueObject, bool>>> filtres = ValueObjectDomain.GetFilterForTreeDimension(node, nomDimension);

            int levelNode = dicAllVo[node.Id].Item2;
            string valueNode = dicAllVo[node.Id].Item3;

            IQueryable<ValueObject> qryVo = dicAllVo.Values
                .Where(val => (val.Item2 == levelNode + 1) && val.Item3.StartsWith(valueNode))
                .Select(val => val.Item1)
                .AsQueryable();

            return filtres.Aggregate(qryVo, (current, predicate) => current.Where(predicate)).ToList();
        }

    }

}
