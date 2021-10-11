using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.EnumHelper
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum obj)
        {
            string objName = obj.ToString();
            Type t = obj.GetType();
            System.Reflection.FieldInfo fi = t.GetField(objName);
            System.ComponentModel.DescriptionAttribute[] arrDesc = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (arrDesc == null || arrDesc.Length==0)
            {
                return "";
            }
            return arrDesc[0].Description;
        }
        public static void GetEnum<T>(string a, ref T t)
        {
            foreach (T b in Enum.GetValues(typeof(T)))
            {
                if (GetDescription(b as Enum) == a)
                    t = b;
            }
        }
    }
}
