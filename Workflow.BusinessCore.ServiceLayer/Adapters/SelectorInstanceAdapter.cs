using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Selector.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  Selector config adapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Selector config adapter.
    /// </remarks>
    public class SelectorInstanceAdapter : ISelectorInstanceAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private ISelectorInstanceDomain SelectorInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorInstanceDomain>();
            }
        }

        private ISelectorEngine SelectorEngine
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorEngine>();
            }
        }


        private IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public SelectorInstanceAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Modifie les données d'un SelectorInstance. Les données sont d'abord sauvées, puis le SelectorInstance passe en Act et en Constraints.
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <remarks>Les valeurs à modifier sont au format suivant : {id de la cellule}:{nouvelle valeur}</remarks>
        /// <returns>Message à modifier</returns>
        public async Task<HttpResponseMessageResult> SaveData(long selectorInstanceId, IEnumerable<KeyValuePair<long, double>> values)
        {
            return await SelectorEngine.SaveData(selectorInstanceId, values);
        }


        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.SelectorInstance>> GetAll()
        {
            IEnumerable<ENT.SelectorInstance> selectorInstance = await SelectorInstanceDomain.Get();

            IEnumerable<DTO.SelectorInstance> dtoSelectorInstance = null;
            if (selectorInstance != null)
                dtoSelectorInstance = Mapper.Map<IEnumerable<ENT.SelectorInstance>, IEnumerable<DTO.SelectorInstance>>(selectorInstance);
            else
                dtoSelectorInstance = new List<DTO.SelectorInstance>();

            return dtoSelectorInstance;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.SelectorInstance> GetById(long id)
        {
            ENT.SelectorInstance selectorInstance = await SelectorInstanceDomain.Get(id);

            DTO.SelectorInstance dtoSelectorInstance = null;
            if (selectorInstance != null)
                dtoSelectorInstance = Mapper.Map<ENT.SelectorInstance, DTO.SelectorInstance>(selectorInstance);

            return dtoSelectorInstance;
        }
    }
}
