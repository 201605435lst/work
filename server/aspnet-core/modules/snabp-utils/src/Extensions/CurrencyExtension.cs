/**********************************************************************
*******命名空间： SnAbp.Utils.Extensions
*******类 名 称： CurrencyExtension
*******类 说 明： 通用数据扩展封装
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/16 9:35:25
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using SnAbp.Utils.DataImport;

namespace SnAbp.Utils
{
    public static class CurrencyExtension
    {
        /// <summary>
        /// 读取指定导入dto中的文件流信息
        /// </summary>
        /// <param name="fileDto"></param>
        /// <returns></returns>
        public static Stream ReadFileStream(this FileUploadDto fileDto) => fileDto.File?.OpenReadStream();

        /// <summary>
        /// 检查文件流是否为空
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream CheckNull(this Stream stream) =>
            stream ?? throw new ArgumentNullException(paramName: nameof(stream),"文件流为空");

        /// <summary>
        /// 检查转换后集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> CheckNull<T>(this List<T> list)=>
            list ?? throw new ArgumentNullException(paramName: nameof(CheckNull), "未将对象实例化");

        /// <summary>
        /// 将文件dto 转换成工作表
        /// <see cref="ReadFileStream"/> or <seealso cref="CheckNull"/>
        /// </summary>
        /// <param name="fileDto">上传的文件</param>
        /// <returns></returns>
        public static IWorkbook ConvertToWorkbook(this FileUploadDto fileDto)
        {
            try
            {
                var stream = fileDto.ReadFileStream().CheckNull();
                var fileName = fileDto.File.FileName;
                return ExcelHelper.ExcelHelper.GetWorkbookByStream(stream, fileName);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(paramName: nameof(ConvertToWorkbook), "文件流为空");
            }
        }

        /// <summary>
        ///  将数据表转为泛型对象
        ///  <see cref="ExcelHelper.ExcelHelper.SheetToList"/>
        /// </summary>
        /// <typeparam name="T">与工作表列对应数量的泛型实体类</typeparam>
        /// <param name="sheet">休要转换工作表</param>
        /// <param name="rowIndex">工作表中有效数据的起始索引</param>
        /// <returns></returns>
        public static List<T> TryTransToList<T>(this ISheet sheet, int rowIndex = 0) where T : class,new()
        {
            try
            {
                return ExcelHelper.ExcelHelper.SheetToList<T>(sheet, rowIndex);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(paramName: nameof(TryTransToList), "数据转换异常，请检查源文件");
            }
        }

        public static int FindIndex<T>(this List<T> list, object item)
        {
            return list.FindIndex(a => a.Equals(item));
        }

        /// <summary>
        /// 获取组织机构的编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<string> GetParentOrganizationCodeArray(this string code)
        {
            if (string.IsNullOrEmpty(code)) return null;
            if (!code.Contains(".")) return null;
            var codes = code.Split('.');
            return codes.Select((a, b) => string.Join(".", codes.Take(b + 1))).ToList();
        }

    }
}
