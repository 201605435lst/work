using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SnAbp.Utils.Validation
{
    public static class ValidationMaxlength
    {
        public static void Validate(object obj)
        {
            var t = obj.GetType();
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes();
                DescriptionAttribute descStr = (DescriptionAttribute)attributes.FirstOrDefault(s => s is DescriptionAttribute);
                foreach (var attribute in attributes)
                {
                    if (attribute is MaxLengthAttribute)
                    {
                        if (attribute is MaxLengthAttribute == false) continue;
                        var maxinumLength = (attribute as MaxLengthAttribute).Length;

                        var propertyValue = property.GetValue(obj) as string;
                        //if (propertyValue == null)
                        //    throw new Exception("exception info");//这里可以自定义，也可以用具体系统异常类
                        if (propertyValue?.Length > maxinumLength)
                            throw new Exception(string.Format("'{0}'的值'{1}'超过了最大长度:{2}", descStr?.Description, propertyValue, maxinumLength));
                    }
                }
            }

        }
    }
}
