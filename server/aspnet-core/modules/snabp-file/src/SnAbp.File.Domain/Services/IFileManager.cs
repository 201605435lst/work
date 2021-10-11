/**********************************************************************
*******命名空间： SnAbp.File.Services
*******接口名称： IFileManagerService
*******接口说明： 领域层 文件管理接口定义，实现一些底层的操作，让应用层调用
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 10:14:25
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SnAbp.File.Entities;
using SnAbp.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace SnAbp.File.Services
{
    public interface IFileManager:IDomainService
    {
        Task<List<Organization>> GetOrganizations(Guid userId);
        Task<List<Organization>> GetOrganizations(Expression<Func<Organization,bool>> expression);


        Task<Folder> GetFolder(Expression<Func<Folder, bool>> expression);
        Task<Folder> GetFolder(Guid id);

        Task<List<Folder>> GetFolderList(Guid?[] orgIds);

        /// <summary>
        /// 根据id获取文件夹信息
        /// </summary>
        /// <param name="id">节点id，组织id或文件夹id</param>
        /// <param name="type">节点类型</param>
        /// <code>组织id：type=1</code>
        /// <code>文件夹id：type=2</code>
        /// <code>标签id：type=3</code>
        /// <returns></returns>
        IQueryable<Folder> GetFolderList(Guid id, int type);

        IQueryable<Folder> GetFolderList(Guid id, int type, Expression<Func<Folder, bool>> expression);

        IQueryable<Folder> GetFolderList(Guid id, int type, ResourceQueryType queryType);

        /// <summary>
        /// 根据条件查询文件夹,查询类型<see cref="ResourceQueryType"></see>
        /// </summary>
        /// <param name="type">查询的类型</param>
        /// <param name="expression">过滤条件，可以为空</param>
        /// <returns></returns>
        IQueryable<Folder> GetFolderList(ResourceQueryType type, Expression<Func<Folder, bool>> expression = null);
        IQueryable<Folder> GetFolderList(Expression<Func<Folder, bool>> expression = null);
        /// <summary>
        /// 根据id获取文件信息
        /// </summary>
        /// <param name="id">节点id，组织id或文件夹id</param>
        /// <param name="type">节点类型</param>
        /// <code>组织id：type=1</code>
        /// <code>文件夹id：type=2</code>
        /// <code>标签id：type=3</code>
        /// <returns></returns>
        IQueryable<Entities.File> GetFileList(Guid id, int type);
        IQueryable<Entities.File> GetFileList(Guid id, int type, Expression<Func<Entities.File, bool>> expression);
        IQueryable<Entities.File> GetFileList(Guid id, int type, ResourceQueryType queryType);

        /// <summary>
        ///  根据条件查询文件,查询类型<see cref="ResourceQueryType"></see>
        /// </summary>
        /// <param name="type">查询类型</param>
        /// <param name="expression">过滤条件</param>
        /// <returns></returns>
        IQueryable<Entities.File> GetFileList(ResourceQueryType type, Expression<Func<Entities.File, bool>> expression = null);
        IQueryable<Entities.File> GetFileList(Expression<Func<Entities.File, bool>> expression = null);
        IQueryable<Entities.File> GetFileList(IQueryable<Folder> queryable, ResourceQueryType type);
        IQueryable<FileRltPermissions> GetList(Expression<Func<FileRltPermissions, bool>> expression);
        IQueryable<FolderRltPermissions> GetList(Expression<Func<FolderRltPermissions, bool>> expression);
        IQueryable<FileRltShare> GetList(Expression<Func<FileRltShare, bool>> expression);
        IQueryable<FolderRltShare> GetList(Expression<Func<FolderRltShare, bool>> expression);

        /// <summary>
        /// 获取在存在文件夹中的文件集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Entities.File> GetInFolderFiles(Guid id, int type);

        /// <summary>
        /// 根据组织id和文件夹id获取文件的逻辑路径
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        Task<string> GetResourcePath(Guid? organizationId, Guid? folderId,string folderName="");
        
        Task<Entities.File> GetFile(Expression<Func<Entities.File, bool>> expression);
        Task<Entities.File> GetFile(Guid id);

        /// <summary>
        /// 批量添加文件标签关联
        /// </summary>
        /// <param name="fileRltTags"></param>
        /// <returns></returns>
        Task<bool> CreateResourceRltTag(IEnumerable<FileRltTag> fileRltTags);

        /// <summary>
        /// 批量添加文件夹标签关联
        /// </summary>
        /// <param name="fileRltTags"></param>
        /// <returns></returns>
        Task<bool> CreateResourceRltTag(IEnumerable<FolderRltTag> fileRltTags);
        
        Task<bool> Insert(Entities.File file);
        Task<bool> Insert(IEnumerable<Entities.File> files);
        Task<bool> Insert(Folder folder);
        Task<bool> Insert(IEnumerable<Folder> folders);
        Task<bool> Insert(IEnumerable<FolderRltPermissions> list);
        Task<bool> Insert(IEnumerable<FileRltPermissions> list);
        Task<bool> Insert(IEnumerable<FileRltShare> list);
        Task<bool> Insert(IEnumerable<FolderRltShare> list);
        Task<bool> Update(Entities.File file);
        Task<bool> Update(IEnumerable<Entities.File> files);
        Task<bool> Update(Folder folder);
        Task<bool> Update(IEnumerable<Folder> folders);

        Task<bool> Delete (Expression<Func<Entities.File, bool>> expression);
        Task<bool> Delete (List<Guid> fileIds, List<Guid> folderIds,bool isTure=false);
        Task<bool> Delete (Expression<Func<Folder, bool>> expression);
        Task<bool> Delete<T> (Expression<Func<T, bool>> expression) where T : class;
        Task Clear();

        /// <summary>
        /// 根据文件流及需要保存的文件名称创建文件，并上传到oss对象存储服务.
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名称，需要带文件后缀名，例如xxx.txt</param>
        /// <returns>返回oss中的存储路径地址</returns>
        Task<FileDomainDto> CreateFileByStream(Stream stream, string fileName);

        Task<FileDomainDto> GetFileInfos(Guid id);

    }
}
    