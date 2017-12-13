using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Workflow.BusinessCore.ServiceLayer.Helpers
{
    /// <summary>
    /// Solution trouvé par jmw => https://stackoverflow.com/questions/39452054/asp-net-core-with-ef-core-dto-collection-mapping
    /// </summary>
    public static class AutoMapperExtensions
    {
        public static ICollection<TTargetType> ResolveCollection<TSourceType, TTargetType>(
            this IMapper mapper,
            ICollection<TSourceType> sourceCollection,
            ICollection<TTargetType> targetCollection,
            Func<ICollection<TTargetType>, TSourceType, TTargetType> getMappingTargetFromTargetCollectionOrNull)
        {
            if (getMappingTargetFromTargetCollectionOrNull == null)
                return null;

            List<TTargetType> existing = targetCollection == null ? new List<TTargetType>() : targetCollection.ToList();
            if (targetCollection != null)
                targetCollection.Clear();
            return ResolveCollection(mapper, sourceCollection, s => getMappingTargetFromTargetCollectionOrNull(existing, s), t => t);
        }

        private static ICollection<TTargetType> ResolveCollection<TSourceType, TTargetType>(
            IMapper mapper,
            ICollection<TSourceType> sourceCollection,
            Func<TSourceType, TTargetType> getMappingTargetFromTargetCollectionOrNull,
            Func<IList<TTargetType>, ICollection<TTargetType>> updateTargetCollection)
        {
            if (getMappingTargetFromTargetCollectionOrNull == null)
                return null;

            List<TTargetType> updatedTargetObjects = new List<TTargetType>();
            foreach (TSourceType sourceObject in sourceCollection ?? Enumerable.Empty<TSourceType>())
            {
                TTargetType existingTargetObject = getMappingTargetFromTargetCollectionOrNull(sourceObject);
                updatedTargetObjects.Add(existingTargetObject == null
                    ? mapper.Map<TTargetType>(sourceObject)
                    : mapper.Map(sourceObject, existingTargetObject));
            }
            return updateTargetCollection(updatedTargetObjects);
        }

    }
}
