/**********************************************************************
*******命名空间： SnAbp.File.Repositories
*******类 名 称： CustomRepository
*******类 说 明： 自定义仓储，用来实现特殊的数据库操作
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 17:45:46
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.File.Entities;
using SnAbp.File.EntityFrameworkCore;
using Volo.Abp;

namespace SnAbp.File.Repositories
{
    public class CustomRepository : EfCoreRepository<FileDbContext, Entities.File, Guid>, ICustomRepository
    {
        private readonly IDbContextProvider<FileDbContext> _dbContextProvider;
        public CustomRepository(IDbContextProvider<FileDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public Task UpdateRange(List<Entities.File> files)
        {
             DbContext.Set<Entities.File>()
                .UpdateRange(files);
             DbContext.SaveChanges();
             return null;
        }

        public Task UpdateRange(List<Folder> folders)
        {
            DbContext.Set<Folder>()
                .UpdateRange(folders);
            DbContext.SaveChanges();
            return null;
        }

        public Task UpdateRange(List<Entities.File> files, List<Folder> folders)
        {
            if (files!=null&&files.Any())
            {
                DbContext.Set<Entities.File>()
                    .UpdateRange(files);
            }

            if (folders!=null&&folders.Any())
            {
                DbContext.Set<Folder>()
                    .UpdateRange(folders);
            }
            DbContext.SaveChanges();
            return null;
        }

        public async Task<bool> InsertRange(IEnumerable<FileRltTag> fileRltTags)
        {
            var rltTags = fileRltTags as FileRltTag[] ?? fileRltTags.ToArray();
            // 处理之前需要删除该文件之前的tag
            DbContext.FileRltTag.RemoveRange(DbContext.FileRltTag.Where(a=> rltTags.Select(b=>b.FileId).Contains(a.FileId)));
            DbContext.FileRltTag.AddRange(rltTags);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<FolderRltTag> folderRltTags)
        {
            var rltTags = folderRltTags as FolderRltTag[] ?? folderRltTags.ToArray();
            // 删除之前的关联
            DbContext.FolderRltTag.RemoveRange(DbContext.FolderRltTag.Where(a=>rltTags.Select(b=>b.FolderId).Contains(a.FolderId)));
            DbContext.FolderRltTag.AddRange(rltTags);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<FileRltPermissions> list)
        {
            DbContext.Set<FileRltPermissions>().AddRange(list);
            DbContext.SaveChanges();
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<FileRltShare> list)
        {
            var fileRltShares = list as FileRltShare[] ?? list.ToArray();
            DbContext.Set<FileRltShare>().AddRange(fileRltShares);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<FolderRltPermissions> list)
        {
            DbContext.FolderRltPermissions.AddRange(list);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<Entities.File> list)
        {
            DbContext.Set<Entities.File>().AddRange(list);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertRange(IEnumerable<Folder> list)
        {
            DbContext.Set<Folder>().AddRange(list);
            await DbContext.SaveChangesAsync();
            return true;
           // return Task.FromResult(DbContext.SaveChanges() == list.Count());
        }

        public async Task<bool> InsertRange(IEnumerable<FolderRltShare> list)
        {
            DbContext.Set<FolderRltShare>().AddRange(list);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public Task<bool> Delete<T>(Expression<Func<FileRltShare, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete<T>(Expression<Func<FileRltTag, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete<T>(Expression<Func<T, bool>> expression) where T:class
        {
            // 获取DbSet
            var dbset = _dbContextProvider.GetDbContext().Set<T>();
            var entities = await dbset
                .AsQueryable()
                .Where(expression)
                .ToListAsync();
            dbset.RemoveRange(entities);
            return await DbContext.SaveChangesAsync()==entities.Count;
        }

        public async Task RestoreFolder(Guid[] folderIds)
        {
            // 还原已被删除的数据，包括还原已有的子文件和子文件夹
            using (DataFilter.Disable<ISoftDelete>())
            {
                foreach (var item in folderIds)
                {
                    var folder = await DbContext.Folder.Include(a=>a.Files).SingleAsync(a => a.Id == item);
                    if (folder != null)
                    {
                        folder.IsDeleted = false;
                        folder.Files?.ForEach(b =>
                        {
                            b.IsDeleted = false;
                            DbContext.File.Update(b);
                        });
                        DbContext.Folder.Update(folder);
                        // 更新子文件夹
                        UpdateChildrenState(folder.Id);
                    }
                   
                }
            }            
            await DbContext.SaveChangesAsync();
        }
        
        private void UpdateChildrenState(Guid pid)
        {
            using (DataFilter.Disable<ISoftDelete>())
            {
                var folders = DbContext.Folder.Include(a=>a.Files).Where(a => a.IsDeleted && a.ParentId == pid).ToList();
                if (folders.Any())
                {
                    foreach (var item in folders)
                    {
                        item.IsDeleted = false;
                        DbContext.Folder.Update(item);
                        item.Files?.ForEach(b =>
                        {
                            b.IsDeleted = false;
                            DbContext.File.Update(b);
                        });
                        UpdateChildrenState(item.Id);
                    }
                }
            }
        }

        public async  Task RestoreFile(Guid[] fileids)
        {
            // 更新文件
            using (DataFilter.Disable<ISoftDelete>())
            {
                //  优化文件还原逻辑：
                // 1、还原已删除文件夹中的单个文件，需要在原来的组织节点上重新创建一个文件夹，然后将该文件指向进去。
                foreach (var item in fileids)
                {
                    var file = await DbContext.File.FindAsync(item);
                    if (file != null)
                    {
                        file.IsDeleted = false;
                        DbContext.File.Update(file);
                    }
                }
            }          
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Insert<T>(T t) where T : class
        {
            var dbset = DbContext.Set<T>();
            dbset.Add(t);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateFileVersion(Guid id, Guid ossId)
        {
            var model =await DbContext.Set<FileVersion>()
                .FirstOrDefaultAsync(a => a.Id == id);
            model.OssId = ossId;

            DbContext.Set<FileVersion>()
                .Update(model);
            await DbContext.SaveChangesAsync();
        }
    }
}
