using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using GenerateLibrary;
using Newtonsoft.Json;

namespace Snabp.Common.CodeGenerate
{
	public static class Util
	{
		
		/// <summary>
		/// 将object 打印成 json 
		/// </summary>
		/// <param name="obj"></param>
		public static void JsonCW(this object obj)
		{
			string serializeObject = JsonConvert.SerializeObject(obj, Formatting.Indented,
				new JsonSerializerSettings()
					{ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
			Console.WriteLine(serializeObject);
		}
		
		/// <summary>
		/// 获取程序集名称 
		/// </summary>
		/// <returns></returns>
		public static string GetAssemblyName()
		{
			return Assembly.GetExecutingAssembly().GetName().Name;
		}
		
		/// <summary>
		/// 获取 注释名称 
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static string GetCommentName(this Type prop)
		{
			CommentAttribute commentAttribute = prop.GetCustomAttribute<CommentAttribute>();
			return commentAttribute == null ? "未写注释" : commentAttribute.Name;
		}
		
		/// <summary>
		/// 获取 注释名称 
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static string GetCommentName(this PropertyInfo prop)
		{
			CommentAttribute commentAttribute = prop.GetCustomAttribute<CommentAttribute>();
			return commentAttribute == null ? "未写注释" : commentAttribute.Name;
		}
		/// <summary>
		/// 将 末尾是id 的 字符串去掉
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string RemoveStrEndId (this string input)
		{
			if (input.Trim().ToLower().EndsWith("id"))
			{
				return  input.Trim().Substring(0, input.Trim().Length - 2);
			}

			return input;
		}


		/// <summary>
		/// string[] 转 字符串 
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="insertStr">间隔的字符是什么</param>
		/// <returns></returns>
		public static string StrArr2Str(this IEnumerable<string> strings,string insertStr="")
		{
			string join = string.Join(insertStr, strings);
			return join;
		}
		/// <summary>
		/// 首字母小写
		/// </summary>
		/// <returns></returns>
		public static string LowerCaseFirstLetter(this string input)
		{
			return input.Substring(0, 1).ToLower() + input.Substring(1);
		}

		/// <summary>
		/// 驼峰转下划线
		/// </summary>
		/// <param name="camelClassName"></param>
		/// <param name="splitChar"></param>
		/// <example>
		/// ConstructionBase ---> construction_base
		/// </example>
		/// <returns></returns>
		public static string ToUnderLine(this string camelClassName,string splitChar="_")
		{
			string strItemTarget = ""; //目标字符串
			for (int j = 0; j < camelClassName.Length; j++) //strItem是原始字符串
			{
				string temp = camelClassName[j].ToString();
				if (Regex.IsMatch(temp, "[A-Z]"))
				{
					temp = splitChar + temp.ToLower();
				}

				strItemTarget += temp;
			}
			if (strItemTarget.StartsWith(splitChar))
			{
				string substring = strItemTarget.Substring(1, strItemTarget.Length - 1);
				return substring;
			}

			return strItemTarget;
		}
		
		
		/// <summary>
		/// System 转换下字符串
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static string SystemStrConvert(this PropertyInfo prop)
		{
			string returnStr = "";
			if (prop.PropertyType.Name == "Int32") returnStr = "int";
			if (prop.PropertyType.Name == "Int64") returnStr = "long";
			if (prop.PropertyType.Name == "String") returnStr = "string";
			if (prop.PropertyType.Name == "Boolean") returnStr = "bool";
			// list 开头的 则 转成 dto 
			if (prop.PropertyType.Name.StartsWith("List")) returnStr = $"List<{prop.GetDtoListTName()}>";

			return prop.PropertyType.Name;
		}
		
		public static string GetDtoListTName(this PropertyInfo prop)
		{
			IncludeAttribute includeAttribute = prop.GetCustomAttribute<IncludeAttribute>();
			if (includeAttribute == null) return "error";
			// thenInclude 为空则代表 这个是 一级关联  
			if (includeAttribute.FullInclude == null)
			{
				return prop.Name + "Dto";
			}

			// 否则 是 二级 关联 
			return includeAttribute.FullInclude + "Dto";
		}

		
	}
}
