using SnAbp.Identity;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Identity.Dtos
{
    public class IdentityUserData : FileExportDto
    {
        public GetIdentityUsersInput Paramter { get; set; }
    }
}
