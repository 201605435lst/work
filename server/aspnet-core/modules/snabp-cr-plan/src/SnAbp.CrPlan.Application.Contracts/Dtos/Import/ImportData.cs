using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos.Import
{
    public class ImportData : IRepairTagKeyDto
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }

        public int PlanType { get; set; }

        public Guid OrgId { get; set; }

        public int Year { get; set; }

        public int? Month { get; set; }
        public string RepairTagKey { get; set; }
    }
}
