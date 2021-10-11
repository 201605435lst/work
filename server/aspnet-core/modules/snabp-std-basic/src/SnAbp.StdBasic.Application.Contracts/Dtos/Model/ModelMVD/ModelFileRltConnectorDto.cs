using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class ModelFileRltConnectorDto : EntityDto<Guid>
    {
        /// <summary>
        /// 模型文件Id
        /// </summary>
        public Guid? ModelFileId { get; set; }


        /// <summary>
        /// 连接件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 连接件位置
        /// </summary>
        public string Position { get; set; }

        public bool IsAll { get; set; }

    }
}
