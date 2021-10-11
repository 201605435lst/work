using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompanyName.MyProjectName.EntityFrameworkCore;
using MyCompanyName.MyProjectName.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Microsoft.OpenApi.Models;
using SnAbp.Account;
using SnAbp.Account.Web;
using SnAbp.Alarm;
using SnAbp.Basic;
using SnAbp.Bpm;
using SnAbp.Cms;
using SnAbp.Common;
using SnAbp.CrPlan;
using SnAbp.Emerg;
using SnAbp.File;
using SnAbp.Identity;
using SnAbp.Tasks;
using SnAbp.Problem;
using SnAbp.Resource;
using SnAbp.Project;
using SnAbp.StdBasic;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using SnAbp.Oa;
using SnAbp.Regulation;
using SnAbp.Schedule;
using SnAbp.Material;
using SnAbp.Report;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(MyProjectNameApplicationModule),
        typeof(MyProjectNameEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(SnAbpAccountWebIdentityServerModule),
        typeof(SnAbpIdentityApplicationModule),
        typeof(SnAbpAccountApplicationModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(BpmApplicationModule),
        typeof(CmsApplicationModule),
        typeof(FileApplicationModule),
        typeof(StdBasicApplicationModule),
        typeof(BasicApplicationModule),
        typeof(ResourceApplicationModule),
        typeof(EmergApplicationModule),
        typeof(ProblemApplicationModule),
        typeof(CommonApplicationModule),
        typeof(OaApplicationModule),
        typeof(ProjectApplicationModule),
        typeof(CrPlanApplicationModule),
        typeof(OaApplicationModule),
        typeof(ProjectApplicationModule),
        typeof(AlarmApplicationModule),
        typeof(TasksApplicationModule),
        typeof(ReportApplicationModule),
        typeof(RegulationApplicationModule),
		typeof(MaterialHttpApiModule),
		typeof(ScheduleHttpApiModule)
    )]
    public class MyProjectNameHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            // 设置时间
            Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Local);

            // 设置验证规则
            Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = false;
            });


            ConfigureUrls(configuration);
            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(MyProjectNameApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(SnAbpIdentityApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(FileApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(BpmApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(CmsApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(AlarmApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(StdBasicApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(BasicApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(ResourceApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(EmergApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(ProblemApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(CrPlanApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(CommonApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(OaApplicationModule).Assembly);
                //options.ConventionalControllers.Create(typeof(ProjectApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(ProjectApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(TasksApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(ReportApplicationModule).Assembly);
		            options.ConventionalControllers.Create(typeof(ScheduleApplicationModule).Assembly);
		        options.ConventionalControllers.Create(typeof(MaterialApplicationModule).Assembly);
                options.ConventionalControllers.Create(typeof(RegulationApplicationModule).Assembly);
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "MyProjectName";
                    options.JwtBackChannelHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "MyProjectName API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                //options.Resources
                //    .Add<MyProjectNameResource>("zh-Hans")
                //    .AddVirtualJson("/Localization/MyProjectName");
                //options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                //options.Languages.Add(new LanguageInfo("en", "en", "English"));
                //options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                //options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                //options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                //options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .WithExposedHeaders("Content-Disposition")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/echo",
            //            context => context.Response.WriteAsync("echo"))
            //        .RequireCors(DefaultCorsPolicyName);

            //    endpoints.MapControllers()
            //        .RequireCors(DefaultCorsPolicyName);

            //    endpoints.MapGet("/echo2",
            //        context => context.Response.WriteAsync("echo2"));

            //    endpoints.MapRazorPages();
            //});
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseAbpRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProjectName API");
                options.DefaultModelsExpandDepth(0);
                options.DocExpansion(DocExpansion.None);
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
