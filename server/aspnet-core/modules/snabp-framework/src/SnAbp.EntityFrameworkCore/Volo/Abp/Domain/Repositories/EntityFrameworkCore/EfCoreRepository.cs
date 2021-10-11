using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SnAbp.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore.DependencyInjection;
using SnAbp.MultiProject;
using SnAbp.MultiProject.MultiProject;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Domain.Repositories.EntityFrameworkCore
{
    public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity>.DbContext => DbContext.As<DbContext>();

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        protected virtual AbpEntityOptions<TEntity> AbpEntityOptions => _entityOptionsLazy.Value;

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly Lazy<AbpEntityOptions<TEntity>> _entityOptionsLazy;
        public EfCoreRepository(IDbContextProvider<TDbContext>
            dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;

            _entityOptionsLazy = new Lazy<AbpEntityOptions<TEntity>>(
                () => ServiceProvider
                          .GetRequiredService<IOptions<AbpEntityOptions>>()
                          .Value
                          .GetOrNull<TEntity>() ?? AbpEntityOptions<TEntity>.Empty
            );

        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            // 实现多项目的自动添加
            try
            {
                var currentProject = ServiceProvider.GetService<ICurrentProject>();
                if (currentProject != null && currentProject.Id.HasValue && IsMutliProject(entity.GetType()))
                {
                    var props = entity.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        if (prop.Name == "ProjectTagId")
                        {
                            prop.SetValue(entity, currentProject.Id);
                        }
                    }
                }

                var organization = ServiceProvider.GetService<IOrganizationRoot>();
                if (organization != null && organization.OrganizationRootId.HasValue && IsOrganization(entity.GetType()))
                {
                    var props = entity.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        if (prop.Name == "OrganizationRootTagId")
                        {
                            prop.SetValue(entity, organization.OrganizationRootId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var savedEntity = DbSet.Add(entity).Entity;
            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
            return savedEntity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return updatedEntity;
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await WithDetails().ToListAsync(GetCancellationToken(cancellationToken));
            }
            else if (IsMutliProject(typeof(TEntity)) && IsOrganization(typeof(TEntity)))
            {
                return await DbSet
                    .AsQueryable<TEntity>()
                    .Where(GetOrganizationAndProjectCondition())
                    .ToListAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                return await DbSet
                    .AsQueryable<TEntity>()
                    .WhereIf(IsMutliProject(typeof(TEntity)), GetMutliProjectCondation())
                    .WhereIf(IsOrganization(typeof(TEntity)), GetOrganizationCondation())
                    .ToListAsync(GetCancellationToken(cancellationToken));
            }
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            // 改写 ，判断是否存在所项目
            var data = DbSet                 
                  .AsQueryable<TEntity>();

            if(IsMutliProject(typeof(TEntity)) && IsOrganization(typeof(TEntity)))
            {
                data = data.Where(GetOrganizationAndProjectCondition());
            }
            else
            {
                data = data.WhereIf(IsMutliProject(typeof(TEntity)), GetMutliProjectCondation())
                .WhereIf(IsOrganization(typeof(TEntity)), GetOrganizationCondation());
            }
            return data;
               
        }

        
        /// <summary>
        /// 是否是多项目
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private bool IsMutliProject(Type entityType)
        {
            // 判断是否存在 ProjectTagId 字段，用来确认多项目
            if (typeof(TEntity).GetProperties().Any(a => a.Name.Equals("ProjectTagId")))
            {
                var currentProject = ServiceProvider.GetService<ICurrentProject>();
                if (currentProject != null && currentProject.Id.HasValue)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool IsOrganization(Type entityType)
        {
            // 判断是否存在 ProjectTagId 字段，用来确认多项目
            if (typeof(TEntity).GetProperties().Any(a => a.Name.Equals("OrganizationRootTagId")))
            {
                var currentProject = ServiceProvider.GetService<IOrganizationRoot>();
                if (currentProject != null && currentProject.OrganizationRootId.HasValue)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private Expression<Func<TEntity, bool>> GetOrganizationAndProjectCondition()
        {
            var currentProject = ServiceProvider.GetService<ICurrentProject>();
            var organization = ServiceProvider.GetService<IOrganizationRoot>();

            if (currentProject.Id.HasValue && organization.OrganizationRootId.HasValue)
            {
                Expression<Func<TEntity, bool>> multiProjectFilter = e => EF.Property<Guid>(e, "ProjectTagId") == currentProject.Id&&EF.Property<Guid>(e, "OrganizationRootTagId") == organization.OrganizationRootId;
                return multiProjectFilter;
            }
           
            return _ => true;
        }

        /// <summary>
        /// 获取多项目查询条件
        /// </summary>
        /// <returns></returns>
        private Expression<Func<TEntity, bool>> GetMutliProjectCondation()
        {
            var currentProject = ServiceProvider.GetService<ICurrentProject>();
            if (currentProject.Id.HasValue)
            {
                Expression<Func<TEntity, bool>> multiProjectFilter = e => EF.Property<Guid>(e, "ProjectTagId") == currentProject.Id;
                return multiProjectFilter;
            }
            return _ => true;
        }

        private Expression<Func<TEntity, bool>> GetOrganizationCondation()
        {
            var organization = ServiceProvider.GetService<IOrganizationRoot>();
            if (organization.OrganizationRootId.HasValue)
            {
                Expression<Func<TEntity, bool>> multiProjectFilter = e => EF.Property<Guid>(e, "OrganizationRootTagId") == organization.OrganizationRootId;
                return multiProjectFilter;
            }
            return _ => true;
        }

        public override async Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails()
                    .Where(predicate)
                      .WhereIf(IsMutliProject(typeof(TEntity)), GetMutliProjectCondation())
                      .WhereIf(IsOrganization(typeof(TEntity)), GetOrganizationCondation())
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
                : await DbSet
                    .Where(predicate)
                      .WhereIf(IsMutliProject(typeof(TEntity)), GetMutliProjectCondation())
                      .WhereIf(IsOrganization(typeof(TEntity)), GetOrganizationCondation())
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext
                .Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext
                .Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<TEntity> WithDetails()
        {
            if (AbpEntityOptions.DefaultWithDetailsFunc == null)
            {
                return base.WithDetails();
            }

            return AbpEntityOptions.DefaultWithDetailsFunc(GetQueryable());
        }

        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }
    }

    public class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>,
        IEfCoreRepository<TEntity, TKey>,
        ISupportsExplicitLoading<TEntity, TKey>

        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TKey>
    {
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var data = WithDetails();
            if (data.Any())
            {
                if (includeDetails)
                {
                    var entity = data.FirstOrDefault(e => e.Id.Equals(id));
                    return entity;
                }
                else
                {
                    return await DbSet.FindAsync(new object[] { id }, GetCancellationToken(cancellationToken));
                }
            }
            else
            {
                return null;
            }

        }

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }
    }
}
