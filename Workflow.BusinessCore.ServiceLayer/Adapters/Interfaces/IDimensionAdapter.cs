using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IDimensionAdapter : IBaseAdapter<DTO.Dimension>
    {
        /// <summary>
        /// Ajoute une dimension.
        /// </summary>
        /// <param name="dimension">Nouvelle Dimension</param>
        /// <returns>Message de retour</returns>
        Task<Dimension> AddDimension(Dimension dimension);

    }
}