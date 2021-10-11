using System;
using System.ComponentModel;
using System.Reflection;

namespace SnAbp.ConstructionBase.Extension
{
	public static class Utils
	{
		private const string Desc = " desc";

		/// <summary>
		/// guid 不为空
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool GuidNotEmpty(this Guid id)
		{
			return id != Guid.Empty;
		}
	}
}