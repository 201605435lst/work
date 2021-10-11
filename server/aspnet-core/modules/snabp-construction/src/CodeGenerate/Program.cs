using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Snabp.Common.CodeGenerate;
using SnAbp.Construction.Entities;
using SnAbp.Construction.Plans;

namespace CodeGenerate
{
	class Program
	{
		static void Main(string[] args)
		{
			GenerateExecuteMain generateExecuteMain = new GenerateExecuteMain(
				projectSrcPath: @"F:\snabp\server\aspnet-core\modules\snabp-construction\src\", // sln项目的src路径
				vueModulePath: @"F:\snabp\client\web\vue\modules\", // vue的 module 路径 
				slnName: "SnAbp.Construction", // 解决方案的名称 如 SnAbp.Construction 
				groupName: "Construction" // 组名称 在 permissions.cs 里面 有
			);
			// 这里 的 typeof 后面 写 要生成 的类
			Type type = typeof(DispatchTemplate);
			List<PropertyInfo> propInfos = type.GetProperties().ToList();
			// generateExecuteMain.GenerateDbRlt(type, propInfos);
			generateExecuteMain.GenApi(type, propInfos,false);
		 generateExecuteMain.GenVue(type);
			
			// generateExecuteMain.GenerateSearchDto(propInfos,type);
		}
	}
}