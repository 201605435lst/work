using Microsoft.EntityFrameworkCore;
using SnAbp.Cms.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Cms.EntityFrameworkCore
{
    [ConnectionStringName(CmsDbProperties.ConnectionStringName)]
    public interface ICmsDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Category> Categorys { get; set; }
        DbSet<Article> Articles { get; set; }
        DbSet<ArticleAccessory> ArticleAccessorys { get; set; }
        DbSet<ArticleCarousel> ArticleCarousels { get; set; }
        DbSet<CategoryRltArticle> CategoryRltArticles { get; set; }

    }
}