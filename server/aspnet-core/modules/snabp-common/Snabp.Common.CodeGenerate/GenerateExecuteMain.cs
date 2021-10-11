using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GenerateLibrary;
using static System.IO.File;

namespace Snabp.Common.CodeGenerate
{
	/// <summary>
	/// 代码生成执行 库
	/// </summary>
	/// <example>
	/// 构造函数
	/// name="projectSrcPath">sln项目的src路径
	/// name="vueModulePath">vue的 module 路径 
	/// name="slnName">解决方案的名称 如 SnAbp.Construction 
	/// name="groupName">组名称 在 permissions.cs 里面 有
	/// <code>
	/// GenerateExecuteMain generateExecuteMain = new GenerateExecuteMain(
	///		projectSrcPath: @"D:\snabp\server\aspnet-core\modules\snabp-construction\src\",
	///		vueModulePath: @"D:\snabp\client\web\vue\modules\",
	///		slnName: "SnAbp.Construction",
	///		groupName: "Construction"
	///	);
	/// </code>
	/// </example>
	public class GenerateExecuteMain
	{
		// sln项目 的 src 路径 

		// vue 的 module 路径 
		private readonly string _vueModulePath;
		private readonly string _slnName; // 解决方案 的名称 
		private readonly string _groupName; // Group 的名称 
		private readonly DirectoryInfo _contractsDirec; // contracts 所在的 csproj  的 路径 
		private DirectoryInfo _efCoreDirec; // efCore 所在的 csproj  的 路径 
		private readonly DirectoryInfo _applicationDirec; // application 所在的 csproj  (放Services用)   
		private readonly string _permissionPath; // 获取Permissions.cs文件路径 
		private readonly string _permissionProviderPath; // 获取PermissionsProvider.cs文件路径 
		private readonly string _hansJsonPath; // 获取Localization 的 json 文件路径 (中文)
		private readonly string _enJsonPath; // 获取Localization 的 json 文件路径 (英文)
		private readonly string _iDbContextPath; // 获取 idbContext 文件路径 
		private readonly string _dbContextPath; // 获取 dbContext 文件路径 
		private readonly string _createExtPath; // 获取 creatingExtensions 文件路径 

		/// <summary>
		/// GenerateExecute 构造函数 
		/// </summary>
		/// <param name="projectSrcPath">sln项目的src路径</param>
		/// <param name="vueModulePath">vue的 module 路径 </param>
		/// <param name="slnName">解决方案的名称 如 SnAbp.Construction </param>
		/// <param name="groupName">组名称 在 permissions.cs 里面 有</param>
		public GenerateExecuteMain(string projectSrcPath, string vueModulePath, string slnName, string groupName)
		{
			string projectSrcPath1 = projectSrcPath;
			_vueModulePath = vueModulePath;
			_slnName = slnName;
			_groupName = groupName;
			DirectoryInfo srcPath = new DirectoryInfo(projectSrcPath1);
			_contractsDirec = srcPath.GetDirectories().FirstOrDefault(x => x.Name.EndsWith(".Contracts"));
			_efCoreDirec = srcPath.GetDirectories().FirstOrDefault(x => x.Name.EndsWith(".EntityFrameworkCore"));
			_applicationDirec = srcPath.GetDirectories().FirstOrDefault(x => x.Name.EndsWith(".Application"));
			_permissionPath = Path.Combine(projectSrcPath1, $"{_slnName}.Application.Contracts", "Permissions",
				$"{_groupName}Permissions.cs");
			_permissionProviderPath = Path.Combine(projectSrcPath1, $"{_slnName}.Application.Contracts", "Permissions",
				$"{_groupName}PermissionDefinitionProvider.cs");
			_hansJsonPath = Path.Combine(projectSrcPath1, $"{_slnName}.Domain.Shared", "Localization", $"{_groupName}",
				"zh-Hans.json");
			_enJsonPath = Path.Combine(projectSrcPath1, $"{_slnName}.Domain.Shared", "Localization", $"{_groupName}",
				"en.json");
			_iDbContextPath = Path.Combine(projectSrcPath1, $"{_slnName}.EntityFrameworkCore", "EntityFrameworkCore",
				$"I{_groupName}DbContext.cs");
			_dbContextPath = Path.Combine(projectSrcPath1, $"{_slnName}.EntityFrameworkCore", "EntityFrameworkCore",
				$"{_groupName}DbContext.cs");
			_createExtPath = Path.Combine(projectSrcPath1, $"{_slnName}.EntityFrameworkCore", "EntityFrameworkCore",
				$"{_groupName}DbContextModelCreatingExtensions.cs");
		}

		/// <summary>
		/// 执行生成 后端
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyInfos"></param>
		/// <param name="onlyGenDbRlt">只生成db相关 的文件 (dbContext,iDbContext,createExtension)</param>
		/// <exception cref="Exception"></exception>
		public void GenApi(Type type, List<PropertyInfo> propertyInfos, bool onlyGenDbRlt = false)
		{
			GenerateIDbContext(type); // 生成 iDbContext
			GenerateDbContext(type); // 生成 dbContext
			GenerateCreateExtension(type); // 生成 dbContext
			GenerateProfile(propertyInfos,type,onlyDto: false);
			GenerateDto(propertyInfos, type); // 生成 Dto

			GenerateIncludeEntity(propertyInfos,type); // 生成 include entities 字符串
			
			GenerateCreateDto(propertyInfos, type); // 生成 CreateDto
			GenerateUpdateDto(propertyInfos, type); // 生成 UpdateDto
			GenerateSearchDto(propertyInfos, type); // 生成 SearchDto
			GenerateIService(type); // 生成 IService
			GenerateService(propertyInfos, type); // 生成 Service
			GenerateProfile(propertyInfos, type); // 生成 Profile
			GeneratePermission(type); // 生成 权限
			GeneratePermissionProvider(type); // 生成 权限Provider
			GenerateLocalization(type); // 生成 语言json 


			Console.WriteLine("生成成功!");
		}

		/// <summary>
		/// 生成 生成db相关 的文件 (dbContext,iDbContext,createExtension)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyInfos"></param>
		public void GenerateDbRlt(Type type, List<PropertyInfo> propertyInfos)
		{
			GenerateIDbContext(type); // 生成 iDbContext
			GenerateDbContext(type); // 生成 dbContext
			GenerateCreateExtension(type); // 生成 dbContext
			GenerateProfile(propertyInfos,type,onlyDto: true);
			GenerateDto(propertyInfos, type); // 生成 Dto
			GenerateIncludeEntity(propertyInfos, type);
			Console.WriteLine("生成 db 相关 文件 成功! ");
		}

		/// <summary>
		/// 生成 q=>q.include(x=>x.XXX) 代码  
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		public void GenerateIncludeEntity(List<PropertyInfo> propInfos, Type type)
		{
			string path = Path.Combine(_efCoreDirec.FullName, "EntityFrameworkCore",
				$"{_groupName}EntityFrameworkCoreModule.cs");
			List<string> lines = ReadAllLines(path).ToList();

			List<LineObj> lineObjs = lines.Select((x,i)=>new LineObj(x,i) ).Where(x => x.Line.Contains("});")).ToList();
			if (lineObjs.Count==0) throw new Exception($"{path} 这个文件有问题!请检查 未找到 包含  }}); 的 行,导致无法定位行");
			
			if (lines.Any(x => x.Contains($"options.Entity<{type.Name}>")))
			{
				Console.WriteLine($"options.Entity<{type.Name}>  已存在!");
			}
			else
			{
				string str = $@"
				// 配置 {type.GetCommentName()} 关联 
				options.Entity<{type.Name}>(x => x.DefaultWithDetailsFunc = q =>q
{propInfos
.Where(prop => prop.GetCustomAttribute<DisplayAttribute>() != null)
.Select(prop => SolveIncludeStr(type, prop))
.StrArr2Str()}
				);
";
				// 找到 行数 
				int lineNumber = lineObjs.Last().Number;

				// 在指定行数前插入
				lines.Insert(lineNumber, str);
				// 重新写如
				WriteAllLines(path, lines);
			}
		}

		private string SolveIncludeStr(Type type, PropertyInfo prop)
		{
			DisplayType displayType = prop.GetCustomAttribute<DisplayAttribute>().DisplayType;
			switch (displayType)
			{
				case DisplayType.Dictionary:
					return $"\t\t\t\t\t.Include({type.Name.LowerCaseFirstLetter()} => {type.Name.LowerCaseFirstLetter()}.{prop.Name})\n";
				case DisplayType.Entity:
					return $"\t\t\t\t\t.Include({type.Name.LowerCaseFirstLetter()} => {type.Name.LowerCaseFirstLetter()}.{prop.Name})\n";
				case DisplayType.EntityList:
					return $"\t\t\t\t\t.Include({type.Name.LowerCaseFirstLetter()} => {type.Name.LowerCaseFirstLetter()}.{prop.Name})\n";
				case DisplayType.EntityParent:
					return $"\t\t\t\t\t.Include({type.Name.LowerCaseFirstLetter()} => {type.Name.LowerCaseFirstLetter()}.{prop.Name})\n";
				case DisplayType.EntityChildrenList:
					return $"\t\t\t\t\t.Include({type.Name.LowerCaseFirstLetter()} => {type.Name.LowerCaseFirstLetter()}.{prop.Name})\n";
				default:
					return "";
			}
		}

		/// <summary>
		/// 生成 vue 代码 
		/// </summary>
		/// <param name="type"></param>
		public void GenVue(Type type)
		{
			// 根据类型 获取除继承基类 以外 的所有 属性 
			List<PropertyInfo> propInfos = type
				.GetProperties()
				.ToList();
			string componentFolder = ($"Sm{_groupName}").ToUnderLine("-");
			string componentFolderPath = Path.Combine(_vueModulePath, "components", componentFolder);
			string apiFolderPath = Path.Combine(_vueModulePath, "components", "sm-api", componentFolder);
		    // D:\snabp\client\web\vue\modules\components\_permissions\sm-constuction.js	
			string permissionJsPath= Path.Combine(_vueModulePath, "components", "_permissions",("Sm"+ _groupName).ToUnderLine("-")+".js");
			string componentName = ($"Sm{_groupName}{type.Name}").ToUnderLine("-");
			Console.WriteLine(componentName);
			// sm-construction-base-standard
			string childFolderPath = CheckVueComponentFolder(componentFolderPath, componentName);
			string demoPath = Path.Combine(childFolderPath, "demo");
			if (!Directory.Exists(demoPath))
			{
				Directory.CreateDirectory(demoPath);
			}

			string stylePath = Path.Combine(childFolderPath, "style");
			if (!Directory.Exists(stylePath))
			{
				Directory.CreateDirectory(stylePath);
			}

			GenerateBasicMd(demoPath, componentName); // 生成 demo/basic.md
			GenerateIndexVue(demoPath, componentName, type); // 生成 demo/index.vue
			GenerateStyleIndexJs(stylePath); // 生成 style/index.js
			GenerateStyleIndexLess(stylePath, componentName); // 生成 style/index.less
			GenerateEnUsMd(childFolderPath); // 生成 index.en-US.md
			GenerateZhCnMd(childFolderPath); // 生成 index.zh-CN.md
			GenerateIndexJs(childFolderPath, type); // 生成 index.js
			GenerateApiJs(apiFolderPath, type); // 生成 ApiXxx.js
			GeneratePermissionJs(permissionJsPath,type); // 生成 _permission/sm-groupName.js
			GenerateSiteModules(type); //  生成  site\modules\constructionBase.js   
			GenerateDemoJs(); //生成 motules/site/demo.js
			GenerateDemoRoutesJs(); //生成 motules/site/demoRoutes.js
			GenerateComponentIndexJs(type); //生成 modules/components/index.js
			GenerateComponentStyleJs(type); //生成 modules/components/style.js

			List<PropertyInfo> dtoProps =
				propInfos.Where(x => x.GetCustomAttribute<DisplayAttribute>() != null).ToList();
			List<PropertyInfo> searchProps =
				propInfos.Where(x => x.GetCustomAttribute<SearchAttribute>() != null).ToList();
			List<PropertyInfo> createProps =
				propInfos.Where(x => x.GetCustomAttribute<CreateAttribute>() != null).ToList();


			GenerateMainJsx(childFolderPath, type, dtoProps, searchProps); // 生成 主要 的 jsx 文件 
			GenerateModalJsx(childFolderPath, type, createProps); // 生成 modal  jsx 文件 (添加编辑)

			
			Console.WriteLine("生成 前端 代码 (vue) 成功 !");
		}

		private void GenerateComponentStyleJs(Type type)
		{
			string componentStyleJs = Path.Combine(_vueModulePath, "components", "style.js");
			if (!Exists(componentStyleJs)) throw new Exception($"文件不存在!{componentStyleJs}");
			List<string> lines = ReadAllLines(componentStyleJs).ToList();
// // 施工计划搭配甘特图
// import './sm-construction/sm-construction-plan-with-gantt/style';
			string smGroup = $"Sm{_groupName}".ToUnderLine("-");
			string smGroupEntity = $"Sm{_groupName}{type.Name}".ToUnderLine("-");
			lines.Add($"// {type.GetCommentName()}");
			lines.Add($"import './{smGroup}/{smGroupEntity}/style';");
		}

		private void GenerateComponentIndexJs(Type type)
		{
			string componentIndexJs = Path.Combine(_vueModulePath, "components", "index.js");
			if (!Exists(componentIndexJs)) throw new Exception($"文件不存在!{componentIndexJs}");
			//读取所有行
			List<string> lines = ReadAllLines(componentIndexJs).ToList();
			List<LineObj> lineObjs = lines.Select((x,i)=>new LineObj(x,i)).ToList();
			string mark = $"Sm{_groupName}{type.Name}";
			if (lines.Any(x=>x.Contains(mark)))
			{
				Console.WriteLine($"component/index.js文件里已存在 import  和...{mark}");
			}
			else
			{
				List<LineObj> list = lineObjs.Where(x => x.Line.Contains("};")).ToList();
				LineObj bottom = list[list.Count - 2]; //找到倒数第二个 };
				LineObj middle = lineObjs.Last(x=>x.Line.Contains("];")); //找到倒数第1个 ];(也只有这一个)
				LineObj top = lineObjs.Last(x=>x.Line.Contains("import {")); //找到倒数第1个 import
				string bottomStr = $" //{type.GetCommentName()}\n Sm{_groupName}{type.Name},";
				string middleStr = $" //{type.GetCommentName()}\n Sm{_groupName}{type.Name},";
				string smGroup = $"Sm{_groupName}".ToUnderLine("-");
				string smGroupEntity = $"Sm{_groupName}{type.Name}".ToUnderLine("-");
				string topStr = $"//{type.GetCommentName()}\nimport {{default as Sm{_groupName}{type.Name}}} from './{smGroup}/{smGroupEntity}';";
				lines.Insert(bottom.Number,bottomStr);
				lines.Insert(middle.Number,middleStr);
				lines.Insert(top.Number,topStr);
				WriteAllLines(componentIndexJs,lines);
			}
		}

		private void GenerateDemoJs()
		{
			string demoJsFilePath = Path.Combine(_vueModulePath, "site", "demo.js");
			if (!Exists(demoJsFilePath)) throw new Exception($"文件不存在!{demoJsFilePath}");
			//读取所有行
			List<string> lines = ReadAllLines(demoJsFilePath).ToList();
			List<LineObj> lineObjs = lines.Select((x,i)=>new LineObj(x,i)).ToList();

			if (lines.Any(x=>x.Contains(_groupName.LowerCaseFirstLetter())))
			{
				Console.WriteLine($"demo.js文件里已存在 import {{ modules as {_groupName.LowerCaseFirstLetter()}}} 和...{_groupName.LowerCaseFirstLetter()}");
			}
			else
			{
				LineObj lineObj = lineObjs.Last(x => x.Line.Contains("};"));
				LineObj lineObj2 = lineObjs.Last(x => x.Line.Contains("import {"));
				lines.Insert(lineObj.Number,$"  ...{_groupName.LowerCaseFirstLetter()},\n");
				lines.Insert(lineObj2.Number,$"import {{ modules as {_groupName.LowerCaseFirstLetter()} }} from './modules/{_groupName.LowerCaseFirstLetter()}';\n");
				WriteAllLines(demoJsFilePath, lines);// 重新写入
			}
			
		}
		private void GenerateDemoRoutesJs()
		{
			string demoJsFilePath = Path.Combine(_vueModulePath, "site", "demoRoutes.js");
			if (!Exists(demoJsFilePath)) throw new Exception($"文件不存在!{demoJsFilePath}");
			//读取所有行
			List<string> lines = ReadAllLines(demoJsFilePath).ToList();
			List<LineObj> lineObjs = lines.Select((x,i)=>new LineObj(x,i)).ToList();
			if (lines.Any(x=>x.Contains(_groupName.LowerCaseFirstLetter())))
			{
				Console.WriteLine($"demo.js文件里已存在 import {{ demo as {_groupName.LowerCaseFirstLetter()}}} 和...{_groupName.LowerCaseFirstLetter()}");
			}
			else
			{
				LineObj lineObj = lineObjs.Last(x => x.Line.Contains("];"));
				LineObj lineObj2 = lineObjs.Last(x => x.Line.Contains("import {"));
				lines.Insert(lineObj.Number,$"  ...{_groupName.LowerCaseFirstLetter()},\n");
				lines.Insert(lineObj2.Number,$"import {{ demo as {_groupName.LowerCaseFirstLetter()} }} from './modules/{_groupName.LowerCaseFirstLetter()}';\n");
				WriteAllLines(demoJsFilePath, lines);// 重新写入
			}
			
		}

		private void GenerateSiteModules(Type type)
		{
			string jsFilePath = Path.Combine(_vueModulePath, "site", "modules", $"{_groupName.LowerCaseFirstLetter()}.js");
			if (Exists(jsFilePath)) // 文件是否存在 ,不存在 创建 
			{
				List<string> lines = ReadAllLines(jsFilePath).ToList();
				if (!lines.Any(x => x.Contains("export const modules"))) throw new Exception($"{jsFilePath} 这个文件有问题!请检查 未找到 export const modules  开头的 行,导致无法定位行");
				if (!lines.Any(x => x.Contains("export const demo"))) throw new Exception($"{jsFilePath} 这个文件有问题!请检查 未找到 export const demo  开头的 行,导致无法定位行");
				List<LineObj> lineObjs = lines.Select((x, i) => new LineObj(x,i) ).Where(x => x.Line.Contains("export const modules")).ToList();
				if (lines.Any(x => x.Contains($"const Sm{_groupName}{type.Name}")))
				{
					Console.WriteLine($"const Sm{_groupName}{type.Name} 已存在!");
				}
				else
				{
					string str = $@"
const Sm{_groupName}{type.Name} = {{
  category: 'Modules',
  type: '{_groupName}',
  title: 'Sm{_groupName}{type.Name}',
  subtitle: '{type.GetCommentName()}',
  demos: [
    {{
      path: '{$"Sm{_groupName}{type.Name}".ToUnderLine("-")}',
      component: () => import('../../components/{$"Sm{_groupName}".ToUnderLine("-")}/{$"Sm{_groupName}{type.Name}".ToUnderLine("-")}/demo/index.vue'),
    }},
    {{
      path: '{$"Sm{_groupName}{type.Name}".ToUnderLine("-")}-cn',
      component: () => import('../../components/{$"Sm{_groupName}".ToUnderLine("-")}/{$"Sm{_groupName}{type.Name}".ToUnderLine("-")}/demo/index.vue'),
    }},
  ],
}};";
					// 找到 行数 
					int lineNumber = lineObjs.Last().Number;
					lines.Insert(lineNumber, str);
					WriteAllLines(jsFilePath, lines);// 重新写入
					
					// 再读一遍
					List<string> lines2 = ReadAllLines(jsFilePath).ToList();
					List<LineObj> lineObjs2 = lines2.Select((x, i) => new LineObj(x, i)).ToList();
					LineObj lineObj2 = lineObjs2.Last(x => x.Line.Contains("};"));
					string str2 = $"\tSm{_groupName}{type.Name},\n";
					lines2.Insert(lineObj2.Number, str2);
					WriteAllLines(jsFilePath, lines2);// 重新写入
					
					// 最后读一遍
					List<string> lines3 = ReadAllLines(jsFilePath).ToList();
					List<LineObj> lineObjs3 = lines3.Select((x, i) => new LineObj(x, i)).ToList();
					LineObj lineObj3 = lineObjs3.Last(x => x.Line.Contains("];"));
					string str3 = $"\t...Sm{_groupName}{type.Name}.demos,\n";
					lines3.Insert(lineObj3.Number, str3);
					WriteAllLines(jsFilePath, lines3);// 重新写入
				}
			}
			else
			{
				string str = $@"
export const modules = {{
}};
export const demo = [
];
";
				WriteAllText(jsFilePath, str);
				GenerateSiteModules(type);
			}
		}

		public void GenerateModalJsx(string childFolderPath, Type type, List<PropertyInfo> createProps)
		{
			string str = $@"
import './style';
import {{ form as formConfig }} from '../../_utils/config';
import * as utils from '../../_utils/utils';
import {{ ModalStatus }} from '../../_utils/enum';
import {{ requestIsSuccess, vIf }} from '../../_utils/utils';

import Api{type.Name} from '../../sm-api/{$"Sm{_groupName}".ToUnderLine("-")}/Api{type.Name}';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let api{type.Name} = new Api{type.Name}();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用


// 表单字段 
const formFields = [{createProps.Select(prop => $@"'{prop.Name.LowerCaseFirstLetter()}'").StrArr2Str(", ")}];
export default {{
  name: 'Sm{_groupName}{type.Name}Modal',
  props: {{
    axios: {{ type: Function, default: null }},
  }},
  data() {{
    return {{
      status: ModalStatus.Hide, // 模态框状态
      form: {{}}, // 表单
      record: {{}}, // 表单绑定的对象
      dicTypes: [],

    }};
  }},
  computed: {{
    title() {{
      return utils.getModalTitle(this.status); // 计算模态框的标题变量
    }},
    visible() {{
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    }},
  }},
  async created() {{
    this.initAxios();
    this.form = this.$form.createForm(this, {{}});// 创建表单
    this.getDicTypes(); // 获取字典类型列表
  }},
  methods: {{
    initAxios() {{
      api{type.Name} = new Api{type.Name}(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    }},
    // 添加
    add() {{
      this.status = ModalStatus.Add;
      this.$nextTick(() => {{
        this.form.resetFields();
      }});
    }},
    // 关闭模态框
    close() {{
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    }},
    // 编辑
    edit(record) {{
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {{
        this.form.setFieldsValue({{ ...utils.objFilterProps(record, formFields) }});
      }});
    }},
    // 获取 工程类型 列表
    async getDicTypes() {{
      let res = await apiDataDictionary.getValues({{ groupCode: 'Profession.' }});
      if (requestIsSuccess(res) && res.data) {{
        this.dicTypes = res.data;
      }}
    }},

    // 数据提交
    ok() {{
      this.form.validateFields(async (err, values) => {{
        if (!err) {{
          // err  是 表单不通过 的 错误  values 是表单内容{{}}
          // console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {{
            response = await api{type.Name}.create({{ ...values }}); // 添加{type.GetCommentName()}
          }} else if (this.status === ModalStatus.Edit) {{
            response = await api{type.Name}.update(this.record.id, {{ ...values }}); // 编辑{type.GetCommentName()}
          }}
          if (utils.requestIsSuccess(response)) {{
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }}
        }}
      }});
    }},
  }},
  render() {{
    return (
      <a-modal
        title={{`{type.GetCommentName()}${{this.title}}`}}
        visible={{this.visible}}
        onCancel={{this.close}}
        destroyOnClose={{true}}
        onOk={{this.ok}}
      >
        <a-form form={{this.form}}>
{createProps.Select(CreateFormItemFromProps).StrArr2Str()}
        </a-form>
      </a-modal>
    );
  }},
}};
";
			WriteAllText(Path.Combine(childFolderPath, $"Sm{_groupName}{type.Name}Modal.jsx"), str);
		}

		/// <summary>
		/// 根据 props 属性来创建 前端 modal 的 form-item
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public string CreateFormItemFromProps(PropertyInfo prop)
		{
			CreateType createType = prop.GetCustomAttribute<CreateAttribute>().CreateType;
			switch (createType)
			{
				case CreateType.StringInput:
					return $@"
          <a-form-item
            label='{prop.GetCommentName().RemoveStrEndId()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}
          >
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
                initialValue: '',
                rules: [{{ required: true, message: '请输入{prop.GetCommentName().RemoveStrEndId()}！', whitespace: true }}],
              }})(<a-input disabled={{this.status === ModalStatus.View}} placeholder={{'请输入{prop.GetCommentName().RemoveStrEndId()}'}} />)
            }}
          </a-form-item>
";
				case CreateType.NumberInput:
					return $@"
          <a-form-item
            label='{prop.GetCommentName()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}>
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
            	initialValue: 0,
            	rules: [
            	  {{ pattern: /^[0-9]\d*$/, message: '请输入正确数字' }},
            	],
              }})(
            	<a-input-number
            	  style='width:100%'
            	  min={{0}}
            	  precision={{0}}
            	  placeholder={{'请输入{prop.GetCommentName()}'}}
            	/>,
              )
            }}
          </a-form-item>

";
				case CreateType.GuidSelect:
					return $@"
          <a-form-item
            label='{prop.GetCommentName().RemoveStrEndId()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}>
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
            	initialValue: undefined,
            	rules: [{{ required: true, message: '请选择{prop.GetCommentName().Replace("id", "")}' }}],
              }})(
            	<a-select placeholder={{'请选择{prop.GetCommentName().Replace("id", "")}'}}>
            	  {{this.dicTypes.map(x => (<a-select-option value={{x.id}}>{{x.name}}</a-select-option>))}}
            	</a-select>,
              )
            }}
          
          </a-form-item>
";
				case CreateType.DatePicker:
					return $@"
          <a-form-item
            label='{prop.GetCommentName()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}
          >
            <a-date-picker
              placeholder={{this.status === ModalStatus.View ? '' : '计划{prop.GetCommentName()}'}}
              disabled={{this.status === ModalStatus.View}}
              style='width: 100%'
              v-decorator={{[
                '{prop.Name.LowerCaseFirstLetter()}',
                {{
                  initialValue: null,
                  rules: [
                    {{
                      required: true,
                      message: '请选择{prop.GetCommentName()}',
                    }},
                  ],
                }},
              ]}}
            />
          </a-form-item>
";
				case CreateType.EnumSelect:
					// TODO  这里先拿 字段(dicTypes) 列表 代替下 后面 打算 获取枚举的成员列表 做一个 list 来遍历
					return $@"
          <a-form-item
            label='{prop.GetCommentName().Replace("id", "")}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}>
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
            	initialValue: undefined,
            	rules: [{{ required: true, message: '请选择{prop.GetCommentName().Replace("id", "")}' }}],
              }})(
            	<a-select placeholder={{'请选择{prop.GetCommentName().Replace("id", "")}'}}>
            	  {{this.dicTypes.map(x => (<a-select-option value={{x.id}}>{{x.name}}</a-select-option>))}}
            	</a-select>,
              )
            }}
          
          </a-form-item>
";
				case CreateType.DecimalInput:
					return $@"
          <a-form-item
            label='{prop.GetCommentName()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}>
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
                initialValue: 0,
                rules: [{{ required: true, message: '请输入{prop.GetCommentName()}!' }}],
              }})(
            	<a-input-number
            	  style='width:100%'
            	  min={{0}}
            	  precision={{2}}
            	  placeholder={{'{prop.GetCommentName()}'}}
            	/>,
              )
            }}
          </a-form-item>
";

				case CreateType.BoolSwitch:
					// todo 先用 输入框, 后面 用 antd 的 switch 组件 来 做
					return $@"
          <a-form-item
            label='{prop.GetCommentName()}'
            label-col={{formConfig.labelCol}}
            wrapper-col={{formConfig.wrapperCol}}
          >
            {{
              this.form.getFieldDecorator('{prop.Name.LowerCaseFirstLetter()}', {{
                initialValue: '',
                rules: [{{ required: true, message: '请输入{prop.GetCommentName()}！', whitespace: true }}],
              }})(<a-input disabled={{this.status === ModalStatus.View}} placeholder={{'请输入{prop.GetCommentName()}'}} />)
            }}
          </a-form-item>
";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void GenerateMainJsx(string childFolderPath, Type type, List<PropertyInfo> dtoProps,
			List<PropertyInfo> searchProps)
		{
			string str = $@"
import './style';
import {{ requestIsSuccess }} from '../../_utils/utils';
import moment from 'moment'; // 用于时间处理 
import Api{type.Name} from '../../sm-api/{$"Sm{_groupName}".ToUnderLine("-")}/Api{type.Name}';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import {{pagination as paginationConfig, tips as tipsConfig }} from '../../_utils/config';
import Sm{_groupName}{type.Name}Modal from './Sm{_groupName}{type.Name}Modal';



let api{type.Name} = new Api{type.Name}();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查类型用

export default {{
  name: 'Sm{_groupName}{type.Name}',
  props: {{
    axios: {{ type: Function, default: null }},
    bordered: {{ type: Boolean, default: true }},
    showOperator: {{ type: Boolean, default: true }}, // 是否显示操作栏()
    showSelectRow: {{ type: Boolean, default: false }}, // 是否显示选择栏
  }},
  data() {{
    return {{
      queryParams: {{
{searchProps
	.Select(GenerateJsxQueryParams)
	.Distinct() // searchKey: '', // 模糊搜索 "; 这个可能会有多个,重复的去掉
	.StrArr2Str()}
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
      }},
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      select{type.Name}Ids: [], // 选择的 {type.GetCommentName()} ids (选择框模式的时候用)
      dicTypes: [],
    }};
  }},
  computed: {{
    columns() {{
      let baseColumns = [
        {{
          title: '序号', dataIndex: 'id', width: 100, customRender: (text, record, index) => {{
            let result = `${{index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}}`;
            return (<span>{{result}}</span>);
          }},
        }},
{
	dtoProps
		.Where(x => !x.Name.EndsWith("Id")) // 属性是id结尾的就不遍历了
		.Select(SolveColumnItem)
		.StrArr2Str()
}
      ];
      return this.showOperator ? [
		...baseColumns,
        {{
          title: '操作', width: 180, customRender: (record) => {{
            return (
              <div>
                <div style='display:inline' onClick={{() => this.edit(record)}}><a>编辑</a></div>
                <div style='display:inline;margin-left:10px' onClick={{() => this.remove(record)}}><a>删除</a></div>
              </div>
            );
          }},
        }},
      ] : baseColumns;
    }},
  }},

  created() {{
    this.initAxios();
    this.refresh();
    this.getDicTypes();
  }},
  methods: {{
    // 初始化axios,将apiStandard实例化
    initAxios() {{
      api{type.Name} = new Api{type.Name}(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    }},
    // 刷新获取list 
    async refresh() {{
      this.loading = true;
      let res = await api{type.Name}.getList({{
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      }});
      if (requestIsSuccess(res) && res.data) {{
        this.list = res.data.items;
        // console.log(res.data.items);
        this.queryParams.totalCount = res.data.totalCount;
      }}
      this.loading = false;
    }},
    // 获取 类型 列表
    async getDicTypes() {{
      let res = await apiDataDictionary.getValues({{ groupCode: 'Progress.ProjectType' }});
      if (requestIsSuccess(res) && res.data) {{
        this.dicTypes = res.data;
      }}
    }},

    // 分页事件
    async onPageChange(page, pageSize) {{
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {{
        this.refresh();
      }}
    }},
    // 添加(打开添加模态框)
    add() {{
      this.$refs.Sm{_groupName}{type.Name}Modal.add();
    }},
    // 添加(打开添加模态框) 123
    edit(record) {{
      this.$refs.Sm{_groupName}{type.Name}Modal.edit(record);
    }},
    remove(record) {{
      const _this = this;
      this.$confirm({{
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{{tipsConfig.remove.content}}</div>,
        okType: 'danger',
        onOk() {{
          return new Promise(async (resolve, reject) => {{
            const response = await api{type.Name}.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          }});
        }},
      }});
    }},
  }},
  render() {{
    return (
      <div>
        {{/* 操作区 */}}
        <sc-table-operator
          onSearch={{() => {{
            this.refresh();
          }}}}
          onReset={{() => {{
            this.queryParams.searchKey = '';
            this.queryParams.pageIndex = 1;
            this.refresh();
          }}}}
        >
          <a-form-item label='关键字'>
            <a-input
              placeholder={{'请输入关键字'}}
              value={{this.queryParams.searchKey}}
              onInput={{async event => {{
                this.queryParams.searchKey = event.target.value;
                this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.refresh();
              }}}}
            />
          </a-form-item>
          <a-form-item label='类型'>
            <a-select
              placeholder={{'请选择类型'}}
              value={{this.queryParams.typeId}}
              onChange={{(val) => {{
                this.queryParams.typeId = val;
                this.refresh();
              }}}}>
              {{this.dicTypes.map(x => (<a-select-option value={{x.id}}>{{x.name}}</a-select-option>))}}
            </a-select>
          </a-form-item>



          <template slot='buttons'>
            <a-button type='primary' size='small' icon='plus' onClick={{() => this.add()}}>
              添加
            </a-button>
          </template>
        </sc-table-operator>
        {{/*展示区*/}}
        <a-table
          dataSource={{this.list}}
          rowKey={{record => record.id}}
          size='small'
          columns={{this.columns}}
          loading={{this.loading}}
          bordered={{this.bordered}}
          pagination={{false}}
          rowSelection={{this.showSelectRow?{{
            selectedRowKeys:this.select{type.Name}Ids,
            columnWidth: 30,
            onChange: (selectedRowKeys, selectedRows) => {{
              this.select{type.Name}Ids = selectedRowKeys;
            }},
          }}:undefined}}

        />

        {{/*分页*/}}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={{this.queryParams.totalCount}}
          pageSize={{this.queryParams.maxResultCount}}
          current={{this.queryParams.pageIndex}}
          onChange={{this.onPageChange}}
          onShowSizeChange={{this.onPageChange}}
          showSizeChanger={{true}}
          showQuickJumper={{true}}
          size={{this.isSimple || this.isFault ? 'small' : ''}}
          showTotal={{paginationConfig.showTotal}}
        />

        {{/*添加/编辑模板*/}}
        <Sm{_groupName}{type.Name}Modal
          ref='Sm{_groupName}{type.Name}Modal'
          axios={{this.axios}}
          onSuccess={{async () => {{
            this.list = []; // 有树状table的话,给数组清空,不然table 默认不展开 ……
            await this.refresh();
          }}}}
        />
      </div>
    );
  }},



}};
";

			WriteAllText(Path.Combine(childFolderPath, $"Sm{_groupName}{type.Name}.jsx"), str);
		}

		/// <summary>
		/// 生成 Column 的item 
		/// </summary>
		/// <param name="prop"></param>
		/// <example>{ title: '计划名称', dataIndex: 'name' }, </example>
		/// <returns></returns>
		public string SolveColumnItem(PropertyInfo prop)
		{
			DisplayType displayType = prop.GetCustomAttribute<DisplayAttribute>().DisplayType;
			switch (displayType)
			{
				case DisplayType.String:
					break;
				case DisplayType.Int:
					break;
				case DisplayType.DateTime:
					return $@"
        {{
          title: '{prop.GetCommentName()}', dataIndex: '{prop.Name.LowerCaseFirstLetter()}', customRender: (text, record, index) => {{
            return (<span>{{moment(text).format(""YYYY-MM-DD"") }}</span>);
          }},
        }},
";
				case DisplayType.Bool:
					break;
				case DisplayType.Guid:
					break;
				case DisplayType.Dictionary:
					break;
				case DisplayType.Entity:
					return $@"
        {{
          title: '{prop.GetCommentName()}', dataIndex: '{prop.Name.LowerCaseFirstLetter()}', customRender: (text, record, index) => {{
            return (<span>{{text.name}}</span>);
          }},
        }},
";
				case DisplayType.EntityParent:
					break;
				case DisplayType.EntityChildrenList:
					break;
				case DisplayType.EntityList:
					break;
				case DisplayType.Decimal:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return $@"
        {{ title: '{prop.GetCommentName()}', dataIndex: '{prop.Name.LowerCaseFirstLetter()}' }}, ";
		}

		/// <summary>
		/// 生成 前端 jsx 的 data 的 queryParams:{}
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public string GenerateJsxQueryParams(PropertyInfo prop)
		{
			SearchType searchType = prop.GetCustomAttribute<SearchAttribute>().SearchType;
			switch (searchType)
			{
				case SearchType.BlurSearch:
					return $@"
        searchKey: '', // 模糊搜索 ";
				case SearchType.IdSearch:
					return $@"
        {prop.Name.LowerCaseFirstLetter()}: undefined, // 根据{prop.GetCommentName()} 搜索 ";
				case SearchType.TimeBetweenSearch:
					// 这个后面有空在写
					return "";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void GeneratePermissionJs(string permissionPath,Type type)
		{
			// 检查有没有 
			if (!File.Exists(permissionPath)) //不存在 就创建 
			{
				string str = $@"
import Common from './common';
const {{ Create, Update, Delete, Dot, Import, Detail, Export, UpdateEnable }} = Common;

// {_groupName}的权限设置
export const GroupName = '{_groupName}';

// {type.GetCommentName()}
const GroupName{type.Name} = GroupName + Dot + '{type.Name}';
export const {type.Name} = {{
  Default: GroupName{type.Name},
  Create : GroupName{type.Name} + Dot + Create,
  Update : GroupName{type.Name} + Dot + Update,
  Delete : GroupName{type.Name} + Dot + Delete,
  Detail : GroupName{type.Name} + Dot + Detail,
}};

";
			WriteAllText(permissionPath, str);
			}
			else // 存在就在最 底下 追加 
			{
				string str = $@"
// {type.GetCommentName()}
const GroupName{type.Name} = GroupName + Dot + '{type.Name}';
export const {type.Name} = {{
  Default: GroupName{type.Name},
  Create : GroupName{type.Name} + Dot + Create,
  Update : GroupName{type.Name} + Dot + Update,
  Delete : GroupName{type.Name} + Dot + Delete,
  Detail : GroupName{type.Name} + Dot + Detail,
}};
";
				File.AppendAllText(permissionPath, str);				
			}
		}

		public void GenerateApiJs(string apiFolderPath, Type type)
		{
			if (!Directory.Exists(apiFolderPath))
			{
				Directory.CreateDirectory(apiFolderPath);
			}

			string str = $@"
import qs from 'qs';


let url = '/api/app/{type.Name.LowerCaseFirstLetter()}';

export default class Api{type.Name} {{
  constructor(axios) {{
    this.axios = axios || null;
  }}

  // 查询 {type.GetCommentName()} 列表
  async getList(params) {{
    return await this.axios({{
      url: `${{url}}/getList`,
      method: 'get',
      params,
    }});
  }}

  // 根据id 查询单个{type.GetCommentName()}
  async get(id) {{
    return await this.axios({{
      url: `${{url}}/get`,
      method: 'get',
      params: {{ id }},
    }});
  }}

  // 添加{type.GetCommentName()}
  async create(data) {{
    return await this.axios({{
      url: `${{url}}/create`,
      method: 'post',
      data,
    }});
  }}

  // 编辑{type.GetCommentName()}
  async update(id, data) {{
    return await this.axios({{
      url: `${{url}}/update`,
      method: 'put',
      params: {{ id }},
      data,
    }});
  }}

  // 删除
  async delete(id) {{
    return await this.axios({{
      url: `${{url}}/delete`,
      method: 'delete',
      params: {{ id }},
    }});
  }}
  // 删除 多个
  async deleteRange(ids) {{
    return await this.axios({{
      url: `${{url}}/deleteRange`,
      method: 'delete',
      params: {{ ids }},
      paramsSerializer: params => {{ // http 请求中 params 里面 传递 重复参数(数组) 具体 类似这样 ?ids=1&ids=2&ids=3
        return qs.stringify(params, {{
          arrayFormat: 'repeat',
        }});
      }},
    }});
  }}
}}
";
			WriteAllText(Path.Combine(apiFolderPath, $"Api{type.Name}.js"), str);
		}

		public void GenerateIndexJs(string childFolderPath, Type type)
		{
			string str = $@"
import Sm{_groupName}{type.Name} from './Sm{_groupName}{type.Name}';

Sm{_groupName}{type.Name}.install = function(Vue) {{
  Vue.component(Sm{_groupName}{type.Name}.name, Sm{_groupName}{type.Name});
}};
export default Sm{_groupName}{type.Name};
            ";
			WriteAllText(Path.Combine(childFolderPath, "index.js"), str);
		}

		public void GenerateZhCnMd(string childFolderPath)
		{
			string str = $@"
## API

| Property | Description                                          | Type     | Default |
| -------- | ---------------------------------------------------- | -------- | ------- |
| axios    | the instance function that from project axios.create | function |

            ";
			WriteAllText(Path.Combine(childFolderPath, "index.zh-CN.md"), str);
		}

		public void GenerateEnUsMd(string childFolderPath)
		{
			string str = $@"
## API

| Property | Description                                          | Type     | Default |
| -------- | ---------------------------------------------------- | -------- | ------- |
| axios    | the instance function that from project axios.create | function |


";
			WriteAllText(Path.Combine(childFolderPath, "index.en-US.md"), str);
		}

		public void GenerateStyleIndexLess(string stylePath, string componentName)
		{
			string str = $@"
.{componentName} {{
}}
";
			WriteAllText(Path.Combine(stylePath, "index.less"), str);
		}

		public void GenerateStyleIndexJs(string stylePath)
		{
			string str = "import './index.less'";
			WriteAllText(Path.Combine(stylePath, "index.js"), str);
		}

		public void GenerateIndexVue(string path, string componentChildrenFolder, Type type)
		{
			string str = $@"
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {{
  cn: `# {componentChildrenFolder}` ,
  us: `# {componentChildrenFolder}` ,
}};

export default {{
  title: 'Sm{_groupName}{type.Name}',
  render() {{
    return (
      <div id=""components-{componentChildrenFolder}-demo"">
        <md cn={{md.cn}} us={{md.us}} />
        <Basic />
        <api>
          <CN slot=""cn"" />
          <US />
        </api>
      </div>
    );
  }},
}};
</script>

<style></style>
";

			// 写如文件 
			WriteAllText(Path.Combine(path, "index.vue"), str);
		}

		/// <summary>
		/// 生成 basic.md
		/// </summary>
		/// <param name="path"></param>
		/// <param name="componentName"></param>
		public void GenerateBasicMd(string path, string componentName)
		{
			string str = $@"
<cn>
#### 基本用法
</cn>

<us>
#### 基本用法
</us>

```tpl
<template>
  <div>
    <{componentName} :axios=""axios""/>
  </div>

</template>
<script>
import axios from '@/utils/axios.js'

export default {{
  data(){{
    return {{
      axios
    }}
  }},
  created(){{
  }},
  methods: {{
  }}
}}
</script>
*** 
";

			// 写如文件 
			WriteAllText(Path.Combine(path, "basic.md"), str);
		}

		public void GenerateCreateExtension(Type type)
		{
			List<string> lines = ReadAllLines(_createExtPath).ToList();
			var list = lines.Select((x, i) => new {x, i}).Where(x => x.x.Contains("optionsAction?.Invoke(options);"))
				.ToList();
			if (list.Count == 0)
			{
				Console.WriteLine($"{_createExtPath} 这个文件有问题!请检查 未找到 optionsAction?.Invoke(options);  开头的 行,导致无法定位行");
			}
			else
			{
				if (lines.Any(x => x.Contains($"builder.Entity<{type.Name}>(b =>")))
				{
					Console.WriteLine($"builder.Entity<{type.Name}>(b =>  已存在!");
				}
				else
				{
					string str = $@"
			builder.Entity<{type.Name}>(b =>
			{{
				b.ToTable(options.TablePrefix + nameof({type.Name}), options.Schema);
				b.ConfigureByConvention();
				b.ConfigureFullAudited(); // 全部配置下审计属性，不写报错 
			}});
";

					// 找到 行数 
					int lineNumber = list.Last().i;

					// 在指定行数前插入
					lines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_createExtPath, lines);
				}
			}
		}

		public void GenerateDbContext(Type type)
		{
			List<string> lines = ReadAllLines(_dbContextPath).ToList();
			var list = lines.Select((x, i) => new {x, i}).Where(x => x.x.Contains("public DbSet<")).ToList();
			if (list.Count == 0)
			{
				Console.WriteLine($"{_dbContextPath} 这个文件有问题!请检查 未找到 public DbSet<  开头的 行,导致无法定位行");
			}
			else
			{
				if (lines.Any(x => x.Contains($"public DbSet<{type.Name}> ")))
				{
					Console.WriteLine($"public DbSet<{type.Name}>  dbSet已存在!");
				}
				else
				{
					string str = $@"
		public DbSet<{type.Name}> {type.Name}s {{ get; set; }}
                            ";

					// 找到 行数 
					int lineNumber = list.Last().i;

					// 在指定行数前插入(加一行插入)
					lines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_dbContextPath, lines);
				}
			}
		}

		public void GenerateIDbContext(Type type)
		{
			List<string> lines = ReadAllLines(_iDbContextPath).ToList();
			var list = lines.Select((x, i) => new {x, i}).Where(x => x.x.Contains("public DbSet<")).ToList();
			if (list.Count == 0)
			{
				Console.WriteLine($"{_iDbContextPath} 这个文件有问题!请检查 未找到 public DbSet<  开头的 行,导致无法定位行");
			}
			else
			{
				if (lines.Any(x => x.Contains($"public DbSet<{type.Name}> ")))
				{
					Console.WriteLine($"public DbSet<{type.Name}>  dbSet已存在!");
				}
				else
				{
					string str = $@"
		public DbSet<{type.Name}> {type.Name}s {{ get; set; }}
                ";

					// 找到 行数 
					int lineNumber = list.Last().i;

					// 在指定行数前插入(加一行插入)
					lines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_iDbContextPath, lines);
				}
			}
		}

		public void GenerateLocalization(Type type)
		{
			List<string> hansLines = ReadAllLines(_hansJsonPath).ToList();
			var hansList = hansLines.Select((x, i) => new {x, i}).Where(x => x.x.Contains(_groupName)).ToList();
			List<string> enLines = ReadAllLines(_enJsonPath).ToList();
			var enList = enLines.Select((x, i) => new {x, i}).Where(x => x.x.Contains(_groupName)).ToList();
			if (hansList.Count == 0)
			{
				Console.WriteLine($"{_hansJsonPath} 这个json 语言文件 有问题!请检查 未找到 {_groupName}   开头的 行");
			}
			else
			{
				if (hansLines.Any(x => x.Contains($"Permission:{type.Name}")))
				{
					Console.WriteLine($"Permission:{type.Name} 这个字段 已存在");
				}
				else
				{
					string str = $@"
        ""Permission:{type.Name}"": ""{type.GetCommentName()}"",
                ";
					// 找到 行数 
					int lineNumber = hansList.Last().i;

					// 在指定行数前插入(加一行插入)
					hansLines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_hansJsonPath, hansLines);
				}
			}

			if (enList.Count == 0)
			{
				Console.WriteLine($"{_hansJsonPath} 这个文件有问题!请检查 未找到 PermissionGroupDefinition moduleGroup  开头的 行");
			}
			else
			{
				if (enLines.Any(x => x.Contains($"Permission:{type.Name}")))
				{
					Console.WriteLine($"Permission:{type.Name} 这个字段 已存在");
				}
				else
				{
					string str = $@"
		""Permission:{type.Name}"": ""{type.Name}"",
                            ";
					// 找到 行数 
					int lineNumber = enList.Last().i;

					// 在指定行数前插入(加一行插入)
					enLines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_enJsonPath, enLines);
				}
			}
		}

		/// <summary>
		/// 生成权限 provider
		/// </summary>
		/// <param name="type"></param>
		public void GeneratePermissionProvider(Type type)
		{
			List<string> lines = ReadAllLines(_permissionProviderPath).ToList();
			var list = lines.Select((x, i) => new {x, i}).Where(x => x.x.Contains("var myGroup")).ToList();
			if (list.Count == 0)
			{
				Console.WriteLine($"{_permissionProviderPath} 这个文件有问题!请检查 未找到 var myGroup  开头的 行");
			}
			else
			{
				if (lines.Any(x => x.Contains($"PermissionDefinition {type.Name.LowerCaseFirstLetter()}Permission")))
				{
					Console.WriteLine($"PermissionDefinition {type.Name.LowerCaseFirstLetter()}Permission 已存在!");
				}
				else
				{
					string str = $@"
			// 定义{type.GetCommentName()}的权限
            PermissionDefinition {type.Name.LowerCaseFirstLetter()}Permission= myGroup.AddPermission({_groupName}Permissions.{type.Name}Permission.Default, L(""Permission:{type.Name}""));
            {type.Name.LowerCaseFirstLetter()}Permission.AddChild({_groupName}Permissions.{type.Name}Permission.Create, L(""Permission:Create""));
            {type.Name.LowerCaseFirstLetter()}Permission.AddChild({_groupName}Permissions.{type.Name}Permission.Update, L(""Permission:Update""));
            {type.Name.LowerCaseFirstLetter()}Permission.AddChild({_groupName}Permissions.{type.Name}Permission.Detail, L(""Permission:Detail""));
            {type.Name.LowerCaseFirstLetter()}Permission.AddChild({_groupName}Permissions.{type.Name}Permission.Delete, L(""Permission:Delete""));
";
					// 找到 行数 
					int lineNumber = list.Last().i;

					// 在指定行数前插入(加一行插入)
					lines.Insert(lineNumber + 1, str);
					// 重新写如
					WriteAllLines(_permissionProviderPath, lines);
				}
			}
		}

		/// <summary>
		/// 生成权限
		/// </summary>
		/// <param name="type"></param>
		public void GeneratePermission(Type type)
		{
			List<string> lines = ReadAllLines(_permissionPath).ToList();
			// lines.Select((x,i)=>$"{i}:{x}").ToList().ForEach(x=>{ Console.WriteLine(x); });
			if (lines.Any(x => x.Contains($"class {type.Name}")))
			{
				Console.WriteLine($"权限{type.Name}已存在");
				return;
			}

			var list = lines.Select((x, i) => new {x, i}).Where(x => x.x.Contains("public static string[]")).ToList();
			if (list.Count == 0)
			{
				Console.WriteLine($"{_permissionPath} 这个文件有问题! 为找到 public static string[] 开头的行 请检查");
			}
			else
			{
				string str = $@"
        public static class {type.Name}Permission
        {{
            public const string Default = GroupName + "".{type.Name}"";

			public const string Create = Default + "".Create"";
			public const string Update = Default + "".Update"";
			public const string Detail = Default + "".Detail"";
			public const string Delete = Default + "".Delete"";
		}}
";

				// 找到 行数 
				int lineNumber = list.Last().i;
				// 在指定行数前插入
				lines.Insert(lineNumber, str);
				// 重新写如
				WriteAllLines(_permissionPath, lines);
			}
		}

		/// <summary>
		/// 生成 Profile
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		/// <param name="onlyDto">只含有 dto 的profile,不含 createDto 和 updateDto</param>
		public void GenerateProfile(List<PropertyInfo> propInfos, Type type,bool onlyDto=false)
		{
			string profileFolder = CheckProfileFolder(_applicationDirec);
			string str = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using {_slnName}.Dtos.{type.Name};
using {_slnName}.Entities;
using Volo.Abp.Domain.Repositories;

namespace {_slnName}.Profiles
{{
	public class {type.Name}Profile : Profile
	{{

		public {type.Name}Profile()
		{{
			CreateMap<{type.Name}, {type.Name}Dto>()
				//.ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project.Name))
				;
{(
	// 如果 onlyDto 为 true ,就不生成 下面两行啦
	onlyDto ? "" : $@"
			CreateMap<{type.Name}CreateDto, {type.Name}>();
			CreateMap<{type.Name}UpdateDto, {type.Name}>();
 "
)}

		}}

	}}
}}
";


			WriteAllText(Path.Combine(profileFolder, $"{type.Name}Profile.cs"), str);
		}

		/// <summary>
		/// 生成 Service
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		public void GenerateService(List<PropertyInfo> propInfos, Type type)
		{
			string serviceFolder = CheckServiceFolder(_applicationDirec);
			string str = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using {_slnName}.Dtos.{type.Name};
using {_slnName}.Entities;
using {_slnName}.IServices;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace {_slnName}.Services
{{

	/// <summary>
	/// {type.GetCommentName()} Service 
	/// </summary>
	// [Authorize]
	public class {type.Name}AppService : CrudAppService<
			{type.Name}, {type.Name}Dto, Guid, {type.Name}SearchDto, {type.Name}CreateDto,
			{type.Name}UpdateDto>,
		I{type.Name}AppService
	{{

		public {type.Name}AppService(IRepository<{type.Name}, Guid> repository) : base(repository)
		{{
			repository.GetListAsync();
		}}

		/// <summary>
		/// 更新前的数据验证
		/// </summary>
		protected override void MapToEntity({type.Name}UpdateDto updateInput, {type.Name} entity)
		{{
			Console.WriteLine(""更新前验证数据"");
			base.MapToEntity(updateInput, entity);
		}}
		/// <summary>
		/// 创建前的数据验证
		/// </summary>
		protected override {type.Name} MapToEntity({type.Name}CreateDto updateInput)
		{{
			Console.WriteLine(""创建前验证数据"");
			return base.MapToEntity(updateInput);
		}}


		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name=""input""></param>
		/// <returns></returns>
		protected override IQueryable<{type.Name}> CreateFilteredQuery({type.Name}SearchDto input)
		{{
			IQueryable<{type.Name}> query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

			// 这里自己手动 写吧,实在是拼不动了…… 
			// query = query
			// 	.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) ||x.Content.Contains(input.SearchKey)  ) // 模糊查询
			// 	.WhereIf(input.ProjectId.HasValue, x => x.ProjectId == input.ProjectId); // 根据xx类型查询 

			return query;
		}}
		/// <summary>
		/// 删除多个 {type.GetCommentName()}
		/// </summary>
		/// <param name=""ids""></param>
		/// <returns></returns>
		public async Task<bool> DeleteRange(List<Guid> ids)
		{{
			await Repository.DeleteAsync(x => ids.Contains(x.Id));
			return true;
		}}


	}}
}}
";


			WriteAllText(Path.Combine(serviceFolder, $"{type.Name}AppService.cs"), str);
		}

		/// <summary>
		/// 生成 IService
		/// </summary>
		/// <param name="type"></param>
		public void GenerateIService(Type type)
		{
			string iServiceFolder = CheckIServiceFolder(_contractsDirec);
			string str = $@"
using System;
using {_slnName}.Dtos.{type.Name};
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace {_slnName}.IServices
{{
	/// <summary>
	/// {type.GetCommentName()} IService 接口
	/// </summary>
	public interface I{type.Name}AppService : ICrudAppService< 
		{type.Name}Dto,
		Guid,
		{type.Name}SearchDto,
		{type.Name}CreateDto,
		{type.Name}UpdateDto
	>
	{{
		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);
	}}
}}
";
			WriteAllText(Path.Combine(iServiceFolder, $"I{type.Name}AppService.cs"), str);
		}

		/// <summary>
		/// 生成 SearchDto 
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		public void GenerateSearchDto(List<PropertyInfo> propInfos, Type type)
		{
			string entityDtoFolder = CheckDtosFolder(_contractsDirec, type);
			string str = $@"
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace {_slnName}.Dtos.{type.Name} 
{{
	/// <summary>
	/// {type.GetCommentName()} SearchDto (搜索Dto) 
	/// </summary>
	public class {type.Name}SearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{{
{propInfos
	.Where(x => x.GetCustomAttribute<SearchAttribute>() != null)
	.Select(SolveSearchDtoItem)
	.Distinct()
	.StrArr2Str()
}
	}}
}}
";
			WriteAllText(Path.Combine(entityDtoFolder, $"{type.Name}SearchDto.cs"), str);
		}

		private static string SolveSearchDtoItem(PropertyInfo prop)
		{
			SearchType searchType = prop.GetCustomAttribute<SearchAttribute>().SearchType;
			switch (searchType)
			{
				case SearchType.BlurSearch:
					return $@"
		/// <summary>
		/// 模糊搜索 
		/// </summary>
		public string SearchKey {{ get; set; }} 
                           ";
				case SearchType.IdSearch:
					return $@"
		/// <summary>
		/// 根据 {prop.GetCommentName()} 搜索
		/// </summary>
		public Guid? {prop.Name} {{ get; set; }} 
                           ";
				case SearchType.TimeBetweenSearch:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return "";
		}

		/// <summary>
		/// 生成 UpdateDto 
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		public void GenerateUpdateDto(List<PropertyInfo> propInfos, Type type)
		{
			string entityDtoFolder = CheckDtosFolder(_contractsDirec, type);
			string str = $@"
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace {_slnName}.Dtos.{type.Name}
{{
	/// <summary>
	/// {type.GetCommentName()} UpdateDto (更新Dto) 
	/// </summary>
	public class {type.Name}UpdateDto 
	{{
{propInfos
	.Where(prop => prop.GetCustomAttribute<CreateAttribute>() != null)
	.Select(CreateDtoProp)
	.StrArr2Str()}
	}}
}}
";
			WriteAllText(Path.Combine(entityDtoFolder, $"{type.Name}UpdateDto.cs"), str);
		}

		/// <summary>
		/// 生成 Create Dto 
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void GenerateCreateDto(List<PropertyInfo> propInfos, Type type)
		{
			string entityDtoFolder = CheckDtosFolder(_contractsDirec, type);
			string str = $@"
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace {_slnName}.Dtos.{type.Name}
{{
	/// <summary>
	/// {type.GetCommentName()} CreateDto (添加Dto) 
	/// </summary>
	public class {type.Name}CreateDto 
	{{
{propInfos
	.Where(prop => prop.GetCustomAttribute<CreateAttribute>() != null)
	.Select(CreateDtoProp)
	.StrArr2Str()}
	}}
}}
";
			WriteAllText(Path.Combine(entityDtoFolder, $"{type.Name}CreateDto.cs"), str);
		}

		public string CreateDtoProp(PropertyInfo prop)
		{
			CreateType createType = prop.GetCustomAttribute<CreateAttribute>().CreateType;
			string str = $@"
		/// <summary>
		/// {prop.GetCommentName()}
		/// </summary>
";
			switch (createType)
			{
				case CreateType.StringInput:
					str+= $"\t\tpublic string {prop.Name} {{get;set;}} ";
					break;
				case CreateType.NumberInput:
					str+= $"\t\tpublic int {prop.Name} {{get;set;}}";
					break;
				case CreateType.GuidSelect:
					str+= $"\t\tpublic Guid? {prop.Name} {{get;set;}}";
					break;
				case CreateType.DatePicker:
					str+= $"\t\tpublic DateTime {prop.Name} {{get;set;}}";
					break;
				case CreateType.EnumSelect:
					str+= $"\t\tpublic {prop.PropertyType.Name} {prop.Name} {{get;set;}}";
					break;
				case CreateType.DecimalInput:
					str+= $"\t\tpublic double {prop.Name} {{get;set;}}";
					break;
				case CreateType.BoolSwitch:
					str+= $"\t\tpublic bool {prop.Name} {{get;set;}}";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return str;
		}

		/// <summary>
		/// 生成dto 
		/// </summary>
		/// <param name="propInfos"></param>
		/// <param name="type"></param>
		public void GenerateDto(List<PropertyInfo> propInfos, Type type)
		{
			string entityDtoFolder = CheckDtosFolder(_contractsDirec, type);
			string dtoStr = $@"
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
namespace {_slnName}.Dtos.{type.Name}
{{

	/// <summary>
	/// {type.GetCommentName()} dto 
	/// </summary>
	public class {type.Name}Dto : EntityDto<Guid>
	{{
{propInfos
	.Where(x => x.GetCustomAttribute<DisplayAttribute>() != null)
	.Select(prop => SolveDtoProps(prop, type))
	.StrArr2Str()}
	}}
}}";
			WriteAllText(Path.Combine(entityDtoFolder, $"{type.Name}Dto.cs"), dtoStr);
		}

		public string SolveDtoProps(PropertyInfo prop, Type type)
		{
			DisplayAttribute displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
			DisplayType displayType = displayAttr.DisplayType;
			string str = $@"
		/// <summary>
		/// {prop.GetCommentName()}
		/// </summary>
";
			switch (displayType)
			{
				case DisplayType.String:
					str+=  $"\t\tpublic string {prop.Name} {{get;set;}}";
					break;
				case DisplayType.Int:
					str+= $"\t\tpublic int {prop.Name} {{get;set;}}";
					break;
				case DisplayType.Guid:
					str+= $"\t\tpublic Guid? {prop.Name} {{get;set;}}";
					break;
				case DisplayType.Dictionary:
					str+= $"\t\tpublic string {prop.Name} {{get;set;}}";
					break;
				case DisplayType.Entity:
					str+= $"\t\tpublic {prop.PropertyType.Name}Dto {prop.Name} {{get;set;}}";
					break;
				case DisplayType.EntityList:
					str+= $"\t\tpublic List<{displayAttr.EntityName}Dto> {prop.Name} {{get;set;}} = new List<{displayAttr.EntityName}Dto>(); // 数组类型最好给初始值,不然容易报null的错" ;
					break;
				case DisplayType.Bool:
					str+= $"\t\tpublic bool {prop.Name} {{get;set;}}";
					break;
				case DisplayType.EntityParent:
					str+= $"\t\tpublic {type.Name}Dto {prop.Name} {{get;set;}}";
					break;
				case DisplayType.EntityChildrenList:
					str+= $"\t\tpublic List<{type.Name}Dto> {prop.Name} {{get;set;}} = new List<{type.Name}Dto>(); // 数组类型最好给初始值,不然容易报null的错";
					break;
				case DisplayType.DateTime: 
					str+= $"\t\tpublic DateTime {prop.Name} {{get;set;}}";
					break;
				case DisplayType.Decimal:
					str+= $"\t\tpublic double {prop.Name} {{get;set;}}";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return str;
		}

		/// <summary>
		/// 检查 dtos 文件夹及 dtos/entity 文件夹是否存在 ,不存在 就创建 
		/// </summary>
		/// <param name="contractsDirec"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public string CheckDtosFolder(DirectoryInfo contractsDirec, Type type)
		{
			// 检查Dtos 文件夹有没有
			string dtosPath = Path.Combine(contractsDirec.FullName, "Dtos");
			if (!Directory.Exists(dtosPath))
			{
				Directory.CreateDirectory(dtosPath);
			}

			string entityDtoFolder = Path.Combine(dtosPath, type.Name);
			if (!Directory.Exists(entityDtoFolder))
			{
				Directory.CreateDirectory(entityDtoFolder);
			}

			return entityDtoFolder;
		}

		/// <summary>
		/// 检查 sm-groupName 文件夹及 sm-groupName/entity 文件夹是否存在 ,不存在 就创建 
		/// </summary>
		/// <param name="componentFolder"></param>
		/// <param name="componentChildrenFolder"></param>
		/// <returns></returns>
		public string CheckVueComponentFolder(string componentFolder, string componentChildrenFolder)
		{
			if (!Directory.Exists(componentFolder))
			{
				Directory.CreateDirectory(componentFolder);
			}

			string childPath = Path.Combine(componentFolder, componentChildrenFolder);
			if (!Directory.Exists(childPath))
			{
				Directory.CreateDirectory(childPath);
			}

			return childPath;
		}

		/// <summary>
		/// 检查 IServices 文件夹是否存在 ,不存在 就创建 
		/// </summary>
		/// <param name="contractsDirec"></param>
		/// <returns></returns>
		public string CheckIServiceFolder(DirectoryInfo contractsDirec)
		{
			// 检查Dtos 文件夹有没有
			string iServicePath = Path.Combine(contractsDirec.FullName, "IServices");
			if (!Directory.Exists(iServicePath))
			{
				Directory.CreateDirectory(iServicePath);
			}

			return iServicePath;
		}

		/// <summary>
		/// 检查 Services 文件夹是否存在 ,不存在 就创建 
		/// </summary>
		/// <param name="contractsDirec"></param>
		/// <returns></returns>
		public string CheckServiceFolder(DirectoryInfo contractsDirec)
		{
			// 检查Dtos 文件夹有没有
			string servicePath = Path.Combine(contractsDirec.FullName, "Services");
			if (!Directory.Exists(servicePath))
			{
				Directory.CreateDirectory(servicePath);
			}

			return servicePath;
		}

		/// <summary>
		/// 检查 Profiles 文件夹是否存在 ,不存在 就创建 
		/// </summary>
		/// <param name="contractsDirec"></param>
		/// <returns></returns>
		public string CheckProfileFolder(DirectoryInfo contractsDirec)
		{
			// 检查profiles 文件夹有没有
			string servicePath = Path.Combine(contractsDirec.FullName, "Profiles");
			if (!Directory.Exists(servicePath))
			{
				Directory.CreateDirectory(servicePath);
			}

			return servicePath;
		}
	}

	/// <summary>
	/// 行对象 
	/// </summary>
	public class LineObj
	{
		public LineObj(string line, int number)
		{
			Line = line;
			Number = number;
		}

		/// <summary>
		/// 行内容 
		/// </summary>
		public string  Line { get; set; }
		/// <summary>
		/// 行号 
		/// </summary>
		public int  Number { get; set; }
		
	}
}