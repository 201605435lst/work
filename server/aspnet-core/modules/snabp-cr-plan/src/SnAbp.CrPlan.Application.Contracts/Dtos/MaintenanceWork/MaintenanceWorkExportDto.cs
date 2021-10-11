using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace SnAbp.CrPlan.Dtos.MaintenanceWork
{
    [Serializable]
    public class MaintenanceWorkExportDto
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream File { get; set; } 

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
    }
}
