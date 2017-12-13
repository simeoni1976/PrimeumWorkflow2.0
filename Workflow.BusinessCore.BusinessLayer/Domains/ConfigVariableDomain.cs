using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Workflow.Transverse.Environment;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    public class ConfigVariableDomain : IConfigVariableDomain
    {
        private readonly IServiceProvider _serviceProvider;
        private IUnitOfWork _unitOfWork = null;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                if ((_unitOfWork == null) && (_serviceProvider != null))
                    _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
                return _unitOfWork;
            }
        }

        private bool _hasDataLoaded = false;
        private string _format = null;
        private char _alignmentChar;

        /// <summary>
        /// Constructeur pour le DI.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ConfigVariableDomain(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public string Format
        {
            get
            {
                return _format;
            }
        }

        public char AlignmentChar
        {
            get
            {
                return _alignmentChar;
            }
        }

        /// <summary>
        /// Charge les variables de conf de la base
        /// </summary>
        public async Task LoadVariables()
        {
            if (!_hasDataLoaded)
            {
                _format = await GetVariable(Constant.CONFIGURATION_NAME_FORMAT);
                string tmpAlignmentChar = await GetVariable(Constant.CONFIGURATION_NAME_SEPARATION_CHAR);
                if (string.IsNullOrEmpty(tmpAlignmentChar))
                    _alignmentChar = tmpAlignmentChar[0];
                else
                    _alignmentChar = Constant.DEFAULT_SEPARATOR_TREE;

                _hasDataLoaded = true;
            }
        }



        private async Task<string> GetVariable(string name)
        {
            return await UnitOfWork.GetDbContext().ConfigVariable
                .Where(c => c.Name == name)
                .Select(c => c.Value)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


    }
}
