/**********************************************************************
*******命名空间： SnAbp.File.Repositories
*******接口名称： ICustomRepository
*******接口说明： 自定义的文件仓储，用来批量更新
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 17:46:18
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SnAbp.File.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.File.Repositories
{
    public interface ICustomRepository : IRepository<Entities.File, Guid>
    {
        Task UpdateRange(List<Entities.File> files);
        Task UpdateRange(List<Folder> files);
        Task UpdateRange(List<Entities.File> files,List<Folder> folders);
        Task RestoreFile(Guid[] fileIds);
        Task RestoreFolder(Guid[] folderIds);
        Task<bool> Insert<T>(T list) where T:class;
        Task<bool> InsertRange(IEnumerable<FileRltTag> list);
        Task<bool> InsertRange(IEnumerable<FileRltPermissions> list);
        Task<bool> InsertRange(IEnumerable<FileRltShare> list);
        Task<bool> InsertRange(IEnumerable<FolderRltTag> list);
        Task<bool> InsertRange(IEnumerable<FolderRltPermissions> list);
        Task<bool> InsertRange(IEnumerable<FolderRltShare> list);
        Task<bool> InsertRange(IEnumerable<Folder> list);
        Task<bool> InsertRange(IEnumerable<Entities.File> list);

        Task<bool> Delete<T>(Expression<Func<T, bool>> expression) where T : class;

        Task UpdateFileVersion(Guid id, Guid ossId);
    }
}
