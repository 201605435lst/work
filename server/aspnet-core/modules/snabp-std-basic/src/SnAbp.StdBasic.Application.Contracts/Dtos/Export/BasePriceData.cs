﻿using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.StdBasic.Dtos.Export
{
    public class BasePriceData : FileExportDto
    {
        public BasePriceGetListDto Paramter { get; set; }
    }
}
