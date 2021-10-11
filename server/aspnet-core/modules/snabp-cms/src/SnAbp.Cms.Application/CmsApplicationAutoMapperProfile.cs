using AutoMapper;
using SnAbp.Cms.Dtos;
using SnAbp.Cms.Dto.CategoryRltArticle;
using SnAbp.Cms.Entities;
using SnAbp.File.Dtos;

namespace SnAbp.Cms
{
    public class CmsApplicationAutoMapperProfile : Profile
    {
        public CmsApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CategorySimpleDto, Category>();
            CreateMap<Category, CategorySimpleDto>();
            CreateMap<Category, CategoryUpdateEnableDto>();
            CreateMap<CategoryUpdateEnableDto, Category>();

            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();
            CreateMap<Article, ArticleSimpleDto>();
            CreateMap<ArticleSimpleDto, Article>();
            CreateMap<Article, ArticleUpdateDto>();
            CreateMap<ArticleUpdateDto, Article>();

            CreateMap<ArticleAccessory, ArticleAccessoryDto>();
            CreateMap<ArticleAccessoryDto, ArticleAccessory>();
            CreateMap<ArticleAccessory, ArticleAccessorySimpleDto>();
            CreateMap<ArticleAccessorySimpleDto, ArticleAccessory>();

            CreateMap<ArticleCarousel, ArticleCarouselDto>();
            CreateMap<ArticleCarouselDto, ArticleCarousel>();
            CreateMap<ArticleCarousel, ArticleCarouselSimpleDto>();
            CreateMap<ArticleCarouselSimpleDto, ArticleCarousel>();


            CreateMap<CategoryRltArticle, CategoryRltArticleSimpleDto>();
            CreateMap<CategoryRltArticle, string>().ConvertUsing(source => source.Category != null ? source.Category.Title : null);

            CreateMap<CategoryRltArticleSimpleDto, CategoryRltArticle>();
            CreateMap<CategoryRltArticle, CategoryRltArticleUpdateDto>();

            CreateMap<File.Entities.File, FileDto>();
            CreateMap<FileDto, File.Entities.File>();
        }
    }
}