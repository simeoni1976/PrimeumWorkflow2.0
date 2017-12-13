using AutoMapper;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.ServiceLayer.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects

            CreateMap<ENT.Comment, DTO.Comment>()
                .ForMember(dest => dest.ValueObject, opts => opts.Ignore());
            CreateMap<DTO.Comment, ENT.Comment>()
                .ForMember(dest => dest.ValueObject, opts => opts.Ignore());

            CreateMap<ENT.Criteria, DTO.Criteria>()
                .ForMember(dest => dest.SelectorConfig, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigModifiers, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigValidators, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigModifyData, opts => opts.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.SelectorConfig != null)
                    {
                        dst.SelectorConfig = new DTO.SelectorConfig();
                        dst.SelectorConfig.Id = src.SelectorConfig.Id;
                    }
                });
            CreateMap<DTO.Criteria, ENT.Criteria>()
                .ForMember(dest => dest.SelectorConfig, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigModifiers, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigValidators, opts => opts.Ignore())
                .ForMember(dest => dest.SelectorConfigModifyData, opts => opts.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.SelectorConfig != null)
                    {
                        dst.SelectorConfig = new ENT.SelectorConfig();
                        dst.SelectorConfig.Id = src.SelectorConfig.Id;
                    }
                });

            CreateMap<ENT.CriteriaValues, DTO.CriteriaValues>();
            CreateMap<DTO.CriteriaValues, ENT.CriteriaValues>();

            CreateMap<ENT.DataSet, DTO.DataSet>();
            CreateMap<DTO.DataSet, ENT.DataSet>();

            CreateMap<ENT.DataSetDimension, DTO.DataSetDimension>();
            CreateMap<DTO.DataSetDimension, ENT.DataSetDimension>();

            CreateMap<ENT.Dimension, DTO.Dimension>()
                .ForMember(dest => dest.Criteria, opts => opts.Ignore())
                .ForMember(dest => dest.DataSetDimension, opts => opts.Ignore())
                .ForMember(dest => dest.WorkflowDimension, opts => opts.Ignore());
            CreateMap<DTO.Dimension, ENT.Dimension>()
                .ForMember(dest => dest.Criteria, opts => opts.Ignore())
                .ForMember(dest => dest.DataSetDimension, opts => opts.Ignore())
                .ForMember(dest => dest.WorkflowDimension, opts => opts.Ignore());

            CreateMap<ENT.SelectorConfig, DTO.SelectorConfig>()
                .ForMember(dest => dest.SelectorInstance, opts => opts.Ignore());
            CreateMap<DTO.SelectorConfig, ENT.SelectorConfig>()
                .ForMember(dest => dest.SelectorInstance, opts => opts.Ignore());

            CreateMap<ENT.SelectorInstance, DTO.SelectorInstance>()
                .ForMember(dest => dest.SelectorConfig, opts => opts.Ignore());
            CreateMap<DTO.SelectorInstance, ENT.SelectorInstance>()
                .ForMember(dest => dest.SelectorConfig, opts => opts.Ignore());

            CreateMap<ENT.User, DTO.User>();
            CreateMap<DTO.User, ENT.User>();

            CreateMap<ENT.UserSet, DTO.UserSet>();
            CreateMap<DTO.UserSet, ENT.UserSet>();

            CreateMap<ENT.UserSetUser, DTO.UserSetUser>()
                .ForMember(dest => dest.User, opts => opts.Ignore())
                .ForMember(dest => dest.UserSet, opts => opts.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.User != null)
                    {
                        dst.User = new DTO.User();
                        dst.User.Id = src.User.Id;
                    }
                });
            CreateMap<DTO.UserSetUser, ENT.UserSetUser>()
                .ForMember(dest => dest.User, opts => opts.Ignore())
                .ForMember(dest => dest.UserSet, opts => opts.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.User != null)
                    {
                        dst.User = new ENT.User();
                        dst.User.Id = src.User.Id;
                    }
                });

            CreateMap<ENT.ValueObject, DTO.ValueObject>();
            CreateMap<DTO.ValueObject, ENT.ValueObject>();

            CreateMap<ENT.WorkflowConfig, DTO.WorkflowConfig>();
            CreateMap<DTO.WorkflowConfig, ENT.WorkflowConfig>();

            CreateMap<ENT.WorkflowDimension, DTO.WorkflowDimension>();
            CreateMap<DTO.WorkflowDimension, ENT.WorkflowDimension>();

            CreateMap<ENT.WorkflowInstance, DTO.WorkflowInstance>()
                .ForMember(dest => dest.SelectorInstance, opts => opts.Ignore())
                .ForMember(dest => dest.WorkflowConfig, opts => opts.Ignore())
                .ForMember(dest => dest.ModifyUser, opts => opts.Ignore())
                .ForMember(dest => dest.ValidateUser, opts => opts.Ignore());
            CreateMap<DTO.WorkflowInstance, ENT.WorkflowInstance>()
                .ForMember(dest => dest.SelectorInstance, opts => opts.Ignore())
                .ForMember(dest => dest.WorkflowConfig, opts => opts.Ignore())
                .ForMember(dest => dest.ModifyUser, opts => opts.Ignore())
                .ForMember(dest => dest.ValidateUser, opts => opts.Ignore());

            CreateMap<ENT.ConditionnedCriteria, DTO.ConditionnedCriteria>();
            CreateMap<DTO.ConditionnedCriteria, ENT.ConditionnedCriteria>();

            CreateMap<ENT.ConditionnedCriteriaValues, DTO.ConditionnedCriteriaValues>();
            CreateMap<DTO.ConditionnedCriteriaValues, ENT.ConditionnedCriteriaValues>();

            CreateMap<ENT.DistinctValue, DTO.DistinctValue>();
            CreateMap<DTO.DistinctValue, ENT.DistinctValue>();

            CreateMap<ENT.GridConfig, DTO.GridConfig>();
            CreateMap<DTO.GridConfig, ENT.GridConfig>();

            CreateMap<ENT.GridDimensionConfig, DTO.GridDimensionConfig>()
                .ForMember(dest => dest.GridColumn, opts => opts.Ignore())
                .ForMember(dest => dest.GridRow, opts => opts.Ignore())
                .ForMember(dest => dest.GridFixed, opts => opts.Ignore());
            CreateMap<DTO.GridDimensionConfig, ENT.GridDimensionConfig>()
                .ForMember(dest => dest.GridColumn, opts => opts.Ignore())
                .ForMember(dest => dest.GridRow, opts => opts.Ignore())
                .ForMember(dest => dest.GridFixed, opts => opts.Ignore());

            CreateMap<ENT.GridValueConfig, DTO.GridValueConfig>()
                .ForMember(dest => dest.GridDimensionConfig, opts => opts.Ignore());
            CreateMap<DTO.GridValueConfig, ENT.GridValueConfig>()
                .ForMember(dest => dest.GridDimensionConfig, opts => opts.Ignore());

            CreateMap<ENT.Action, DTO.Action>();
            CreateMap<DTO.Action, ENT.Action>();

            CreateMap<ENT.ActionSequence, DTO.ActionSequence>()
                .ForMember(dest => dest.Action, opts => opts.Ignore());
            CreateMap<DTO.ActionSequence, ENT.ActionSequence>()
                .ForMember(dest => dest.Action, opts => opts.Ignore());

            CreateMap<ENT.ActionParameter, DTO.ActionParameter>();
            CreateMap<DTO.ActionParameter, ENT.ActionParameter>();

            CreateMap<ENT.Constraint, DTO.Constraint>();
            CreateMap<DTO.Constraint, ENT.Constraint>();

            CreateMap<ENT.ConstraintSequence, DTO.ConstraintSequence>()
                .ForMember(dest => dest.Constraint, opts => opts.Ignore());
            CreateMap<DTO.ConstraintSequence, ENT.ConstraintSequence>()
                .ForMember(dest => dest.Constraint, opts => opts.Ignore());

            CreateMap<ENT.ConstraintParameter, DTO.ConstraintParameter>();
            CreateMap<DTO.ConstraintParameter, ENT.ConstraintParameter>();
        }
    }
}
