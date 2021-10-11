using Microsoft.EntityFrameworkCore;
using SnAbp.Cms.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Cms.EntityFrameworkCore
{
    [ConnectionStringName(CmsDbProperties.ConnectionStringName)]
    public class CmsDbContext : AbpDbContext<CmsDbContext>, ICmsDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<Category> Categorys { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAccessory> ArticleAccessorys { get; set; }
        public DbSet<ArticleCarousel> ArticleCarousels { get; set; }
        public DbSet<CategoryRltArticle> CategoryRltArticles { get; set; }

        public CmsDbContext(DbContextOptions<CmsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCms();
        }
    }
}