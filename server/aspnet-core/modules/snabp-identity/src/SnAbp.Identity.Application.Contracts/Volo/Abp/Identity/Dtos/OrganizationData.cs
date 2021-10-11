using SnAbp.Identity;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Identity
{
    public class OrganizationData : FileExportDto
    {
        public OrganizationGetListDto Paramter{ get; set; }
    }
}
