/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： FolderTest
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 17:19:31
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.File.Entities;
using SnAbp.Identity;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace SnAbp.File
{
    public class FolderTest : File2DomainTestBase
    {
        private readonly IRepository<IdentityUserRltOrganization> _organizationUsersResp;
        private readonly IRepository<Folder, Guid> _folderResp;
       public FolderTest(  )
        {
            // _organizationResp = organizationResp;
            _organizationUsersResp = GetRequiredService< IRepository<IdentityUserRltOrganization>>();
            _folderResp = GetRequiredService<IRepository<Folder, Guid>>(); ;
        }
        [Fact]
        public async void Test1()
        {
            var list = await _folderResp.GetListAsync();
        }
    }
}
