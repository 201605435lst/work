using System;
using System.Collections.Generic;
using System.Linq;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Construction.Services.MaterPlan
{
	/// <summary>
	/// Plan/masterPlan  公共方法 
	/// </summary>
	public static class PlanUtil
	{
		/// <summary>
		/// 压平树
		/// </summary>
		/// <param name="e"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<T> Flatten<T>(List<T> e) where T: IGuidKeyTree<T>
		{
			if (e != null)
				return e.SelectMany(c => Flatten(c.Children)).Concat(e).ToList();
			return new List<T>();
		}
		/// <summary>
		/// 递归克隆一个树
		/// </summary>
		/// <param name="arr"></param>
		/// <param name="callback">自己写的委托</param>
		/// <returns></returns>
		public static List<T> RecUse<T>(List<T> arr ,Action<T,T> callback) where T: IGuidKeyTree<T> ,ICloneable
		{
			T Traverse(T item)
			{
				T newNode = (T) item.Clone(); //克隆一个,避免属性的引用 修改 
				callback(item,newNode);
				newNode.Children = newNode.Children.Count > 0
					? newNode.Children.Select(node => Traverse(node)).ToList()
					: new List<T>();
				return newNode;
			}

			return arr.Select(x => Traverse(x)).ToList();
		}
		public static List<T> CalcDateFilter<T>(T oldItem,PlanContentDateFilterDto filter) where  T : IPlanDto<T>
		{
			//选择了年月 日
			if (filter.Year.HasValue && filter.Month.HasValue && filter.DayStart.HasValue && filter.DayEnd.HasValue)
			{
				Console.WriteLine($"年{filter.Year}月{filter.Month}日{filter.DayStart}");
				DateTime start = new DateTime(filter.Year.Value, filter.Month.Value, filter.DayStart.Value);
				DateTime end = new DateTime(filter.Year.Value, filter.Month.Value, filter.DayEnd.Value);
				return oldItem.Children .Where(x => ( x.Children.Count == 0 && x.PlanStartTime >= start && x.PlanEndTime <= end ) || x.Children.Count>0).ToList();
			}

			//选择了年月
			//选择了年月 日
			if (filter.Year.HasValue && filter.Month.HasValue)
			{
				Console.WriteLine($"年{filter.Year}月{filter.Month}");
				DateTime start = new DateTime(filter.Year.Value, filter.Month.Value, 1); // 获取 月的第一天
				DateTime end = start.AddMonths(1).AddSeconds(-1); // 获取月的最后一天
				Console.WriteLine(end);
				return oldItem.Children.Where(x => ( x.Children.Count == 0 &&x.PlanStartTime >= start && x.PlanEndTime <= end ) || x.Children.Count>0).ToList();
				
			}

			// 选择了年 季度
			if (filter.Year.HasValue && filter.Quarter.HasValue)
			{
				Console.WriteLine($"年{filter.Year}季{filter.Quarter}");
				DateTime start = new DateTime(1970, 1, 1), end = new DateTime(9999, 1, 1);
				switch (filter.Quarter.Value)
				{
					case 1: //第1季度(1~3月)
						start = new DateTime(filter.Year.Value, 1, 1);
						end = new DateTime(filter.Year.Value, 4, 1).AddSeconds(-1);
						break;
					case 2: //第2季度(4~6月)
						start = new DateTime(filter.Year.Value, 4, 1);
						end = new DateTime(filter.Year.Value, 7, 1).AddSeconds(-1);
						break;
					case 3: //第3季度(7~9月)
						start = new DateTime(filter.Year.Value, 7, 1);
						end = new DateTime(filter.Year.Value, 10, 1).AddSeconds(-1);
						break;
					case 4: //第4季度(10~12月)
						start = new DateTime(filter.Year.Value, 10, 1);
						end = new DateTime(filter.Year.Value + 1, 1, 1).AddSeconds(-1);
						break;
				}

				return oldItem.Children.Where(x =>( x.Children.Count == 0 && x.PlanStartTime >= start && x.PlanEndTime <= end )|| x.Children.Count>0).ToList();
			}

			if (filter.Year.HasValue)
			{
				Console.WriteLine($"年{filter.Year}");
				DateTime start = new DateTime(filter.Year.Value, 1, 1); // 获取 今年第一天
				DateTime end = start.AddYears(1).AddSeconds(-1); // 获取今年的最后一天
				return oldItem.Children.Where(x =>( x.Children.Count == 0 && x.PlanStartTime >= start && x.PlanEndTime <= end )|| x.Children.Count>0).ToList();
			}

			return oldItem.Children;
		}
		
	}
}