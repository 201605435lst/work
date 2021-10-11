using System;
using System.Collections.Generic;
using System.Text;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameAppService : ApplicationService
    {
        protected MyProjectNameAppService()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}
