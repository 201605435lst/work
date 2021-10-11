
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SnAbp.Bpm;
using SnAbp.Construction.Dtos.MasterPlan;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Utils.EnumHelper;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Construction.Profiles
{
	public class MasterPlanProfile : Profile
	{

		public MasterPlanProfile()
		{
			CreateMap<MasterPlan, MasterPlanDto>()
				.ForMember(d => d.WorkflowState, o => o.MapFrom(s => GenWorkflowState(s)))
				.ForMember(d => d.StateStr, o => o.MapFrom(s =>s.State.GetDescription()))
				;

			CreateMap<MasterPlanCreateDto, MasterPlan>();
			CreateMap<MasterPlanUpdateDto, MasterPlan>();
		}

		/// <summary>
		/// 根据 审批记录来 判断 审批状态 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private static string GenWorkflowState(MasterPlan s)
		{
			if (s.MasterPlanRltWorkflowInfos.Count == 0)
			{
				return "未提交";
			}

			if (s.MasterPlanRltWorkflowInfos.All(x => x.WorkflowState == WorkflowState.Finished))
			{
				return "已审核";
			}

			return "待审批";
		}
	}
}
