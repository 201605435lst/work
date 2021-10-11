using System;
using Snabp.Common.CodeGenerate;

namespace GenerateCode
{
	class Program
	{
		static void Main(string[] args)
		{
			GenerateExecuteMain generateExecuteMain = new GenerateExecuteMain(
				projectSrcPath: @"D:\snabp\server\aspnet-core\modules\snabp-construction\src\", // sln项目的src路径
				vueModulePath: @"D:\snabp\client\web\vue\modules\", // vue的 module 路径 
				slnName: "SnAbp.Construction", // 解决方案的名称 如 SnAbp.Construction 
				groupName: "Construction" // 组名称 在 permissions.cs 里面 有
			);
			// 这里 的 typeof 后面 写 要生成 的类
			// Type type = typeof(PlanMaterial);
			// List<PropertyInfo> propInfos = type.GetProperties().ToList();
			// generateExecuteMain.GenerateDbRlt(type, propInfos);
			// generateExecuteMain.GenApi(type, propInfos,false);
			// generateExecuteMain.GenVue(type);
			
			// generateExecuteMain.GenerateSearchDto(propInfos,type);
		}
	}
}