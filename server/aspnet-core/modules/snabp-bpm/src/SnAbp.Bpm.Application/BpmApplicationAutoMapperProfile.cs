using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Dtos.Test;
using SnAbp.Bpm.Entities;
using System;
using System.Linq;
using SnAbp.Identity;
using Volo.Abp.AutoMapper;

namespace SnAbp.Bpm
{
    public class BpmApplicationAutoMapperProfile : Profile
    {
        public BpmApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            // WorkflowTemplate
            CreateMap<WorkflowTemplateCreateDto, WorkflowTemplate>();
            CreateMap<WorkflowTemplateUpdateDto, WorkflowTemplate>();
            CreateMap<WorkflowTemplate, WorkflowTemplateDto>();
            CreateMap<WorkflowTemplate, WorkflowTemplateDetailDto>();
            CreateMap<Member, WorkflowTemplateRltMember>().ConvertUsing((source) => new WorkflowTemplateRltMember(Guid.NewGuid())
            {
                MemberId = source.Id,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<WorkflowTemplateRltMember, Member>().ConvertUsing((source) => new Member()
            {
                Id = source.MemberId,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<Member, WorkflowTemplateRltMember>().ConvertUsing((source) => new WorkflowTemplateRltMember(Guid.NewGuid())
            {
                MemberId = source.Id,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<WorkflowTemplateRltMember, Member>().ConvertUsing((source) => new Member()
            {
                Id = source.MemberId,
                Type = source.Type,
                Name = source.Name
            });

            // FormTemplate
            CreateMap<FormTemplate, FormTemplateDto>();
            CreateMap<FormTemplate, FormTemplateSimpleDto>();
            CreateMap<FormTemplate, FormTemplateDetailDto>();

            // FlowTemplate
            CreateMap<FlowTemplate, FlowTemplateDto>();
            CreateMap<FlowTemplate, FlowTemplateSimpleDto>();
            CreateMap<FlowTemplate, FlowTemplateDetailDto>();

            CreateMap<FlowTemplateNode, FlowTemplateNodeDto>();
            CreateMap<FlowTemplateNodeDto, FlowTemplateNode>();
            CreateMap<FlowTemplateStep, FlowTemplateStepDto>();
            CreateMap<FlowTemplateStepDto, FlowTemplateStep>();

            CreateMap<Member, FlowTemplateNodeRltMember>().ConvertUsing((source) => new FlowTemplateNodeRltMember(Guid.NewGuid())
            {
                MemberId = source.Id,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<FlowTemplateNodeRltMember, Member>().ConvertUsing((source) => new Member()
            {
                Id = source.MemberId,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<MemberDto, FlowTemplateNodeRltMember>().ConvertUsing((source) => new FlowTemplateNodeRltMember(Guid.NewGuid())
            {
                MemberId = source.Id,
                Type = source.Type,
                Name = source.Name
            });
            CreateMap<FlowTemplateNodeRltMember, MemberDto>().ConvertUsing((source) => new MemberDto()
            {
                Id = source.MemberId,
                Type = source.Type,
                Name = source.Name
            });

            // Workflow 

            CreateMap<Workflow, WorkflowDto>();
            CreateMap<WorkflowDto, Workflow>();

            CreateMap<Workflow, WorkflowSimpleDto>();
            CreateMap<Workflow, WorkflowDetailDto>();
            CreateMap<WorkflowSimple, WorkflowSimpleDto>();
            CreateMap<WorkflowSimple, WorkflowDetailDto>();
            CreateMap<WorkflowDetail, WorkflowSimpleDto>();
            CreateMap<WorkflowDetail, WorkflowDetailDto>();

            CreateMap<object, Test>().ConvertUsing(x => convert(x));

            CreateMap<WorkflowTemplateGroupDetailDto, WorkflowTemplateGroup>();
            CreateMap<WorkflowTemplateGroupCreateDto, WorkflowTemplateGroup>();
            CreateMap<WorkflowTemplateGroup, WorkflowTemplateGroupDetailDto>();
            CreateMap<WorkflowTemplateGroup, WorkflowTemplateGroupCreateDto>();

            CreateMap<IdentityUserDto, IdentityUser>();
            CreateMap<IdentityUser, IdentityUserDto>();

            CreateMap<WorkflowProcessDto, WorkflowDetailDto>();
            CreateMap<WorkflowDetailDto, WorkflowProcessDto>();
        }

        private Test convert(object obj)
        {
            string Type = (string)((JToken)obj)["name"];

            Test rst = null;

            switch (Type)
            {
                case "TestA":
                    rst = (TestA)JsonConvert.DeserializeObject(obj.ToString(), typeof(TestA));
                    break;

                case "TestB":
                    rst = (TestB)JsonConvert.DeserializeObject(obj.ToString(), typeof(TestB));
                    break;
            }

            return rst;
        }
    }
}