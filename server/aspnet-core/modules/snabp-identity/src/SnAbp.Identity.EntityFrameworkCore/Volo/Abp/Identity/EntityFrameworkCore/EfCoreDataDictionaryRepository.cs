/**********************************************************************
*******命名空间： Volo.Abp.Identity.EntityFrameworkCore
*******类 名 称： EfDataDictionaryRepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 17:50:50
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SnAbp.Identity;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public class EfCoreDataDictionaryRepository : EfCoreRepository<IIdentityDbContext, DataDictionary, Guid>, IDataDictionaryRepository
    {
        public EfCoreDataDictionaryRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default) => base.DeleteAsync(id, autoSave, cancellationToken);

        public override Task DeleteAsync(DataDictionary entity, bool autoSave = false, CancellationToken cancellationToken = default) => base.DeleteAsync(entity, autoSave, cancellationToken);

        public override Task<DataDictionary> FindAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default) => base.FindAsync(id, includeDetails, cancellationToken);

        public override Task<DataDictionary> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default) => base.GetAsync(id, includeDetails, cancellationToken);

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default) => base.GetCountAsync(cancellationToken);

        public override Task<List<DataDictionary>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default) => base.GetListAsync(includeDetails, cancellationToken);

        public override Task<DataDictionary> InsertAsync(DataDictionary entity, bool autoSave = false, CancellationToken cancellationToken = default) => base.InsertAsync(entity, autoSave, cancellationToken);

        public override Task<DataDictionary> UpdateAsync(DataDictionary entity, bool autoSave = false, CancellationToken cancellationToken = default) => base.UpdateAsync(entity, autoSave, cancellationToken);
    }
}
