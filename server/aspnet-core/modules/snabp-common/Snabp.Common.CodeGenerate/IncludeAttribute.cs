using System;

namespace GenerateLibrary
{
	/// <summary>
	/// ������ѯ ������ 
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IncludeAttribute : Attribute
	{
		public IncludeAttribute()
		{
		}

		/// <summary>
		/// ��������,Ƕ���õ�(User.Student.Teacher)
		/// </summary>
		public string FullInclude { get; }

		public IncludeAttribute(string fullInclude)
		{
			FullInclude = fullInclude;
		}
	}
}
