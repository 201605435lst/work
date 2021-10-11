using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    ApplicationName = table.Column<string>(maxLength: 96, nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    TenantName = table.Column<string>(nullable: true),
                    ImpersonatorUserId = table.Column<Guid>(nullable: true),
                    ImpersonatorTenantId = table.Column<Guid>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    ClientId = table.Column<string>(maxLength: 64, nullable: true),
                    CorrelationId = table.Column<string>(maxLength: 64, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    HttpMethod = table.Column<string>(maxLength: 16, nullable: true),
                    Url = table.Column<string>(maxLength: 256, nullable: true),
                    Exceptions = table.Column<string>(maxLength: 4000, nullable: true),
                    Comments = table.Column<string>(maxLength: 256, nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    JobName = table.Column<string>(maxLength: 128, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(nullable: false, defaultValue: (short)0),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    NextTryTime = table.Column<DateTime>(nullable: false),
                    LastTryTime = table.Column<DateTime>(nullable: true),
                    IsAbandoned = table.Column<bool>(nullable: false, defaultValue: false),
                    Priority = table.Column<byte>(nullable: false, defaultValue: (byte)15)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_ClaimTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    Regex = table.Column<string>(maxLength: 512, nullable: true),
                    RegexDescription = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ValueType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_ClaimTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_DataDictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Key = table.Column<string>(maxLength: 100, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    IsStatic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_DataDictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_App_DataDictionary_Sn_App_DataDictionary_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_FeatureValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_FeatureValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(maxLength: 2000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    ProtocolType = table.Column<string>(maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(nullable: false),
                    RequireConsent = table.Column<bool>(nullable: false),
                    AllowRememberConsent = table.Column<bool>(nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(nullable: false),
                    RequirePkce = table.Column<bool>(nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    BackChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    AllowOfflineAccess = table.Column<bool>(nullable: false),
                    IdentityTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(nullable: false),
                    ConsentLifetime = table.Column<int>(nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(nullable: false),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    AccessTokenType = table.Column<int>(nullable: false),
                    EnableLocalLogin = table.Column<bool>(nullable: false),
                    IncludeJwtId = table.Column<bool>(nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(nullable: false),
                    ClientClaimsPrefix = table.Column<string>(maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(maxLength: 200, nullable: true),
                    UserSsoLifetime = table.Column<int>(nullable: true),
                    UserCodeType = table.Column<string>(maxLength: 100, nullable: true),
                    DeviceCodeLifetime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerDeviceFlowCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerDeviceFlowCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerIdentityResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false),
                    Properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerIdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerPersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_PermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_PermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2048, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowTemplateGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_WorkflowTemplateGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_WorkflowTemplateGroup_Sn_Bpm_WorkflowTemplateGroup_P~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Bpm_WorkflowTemplateGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Common_Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    FullCode = table.Column<string>(nullable: true),
                    Deep = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PinYin = table.Column<string>(nullable: true),
                    PinYinPrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Common_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Common_Area_Sn_Common_Area_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Common_Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_Folder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsShare = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_Folder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_Folder_Sn_File_Folder_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_File_Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_OssServer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ConnName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Enable = table.Column<bool>(nullable: false),
                    EndPoint = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    AccessKey = table.Column<string>(nullable: true),
                    AccessSecret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_OssServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuditLogActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    AuditLogId = table.Column<Guid>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(maxLength: 128, nullable: true),
                    Parameters = table.Column<string>(maxLength: 2000, nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditLogId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ChangeTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<byte>(nullable: false),
                    EntityTenantId = table.Column<Guid>(nullable: true),
                    EntityId = table.Column<string>(maxLength: 128, nullable: false),
                    EntityTypeFullName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 79, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    CSRGCode = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Nature = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 255, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: true),
                    SealImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_App_Organization_Sn_App_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_App_Organization_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Surname = table.Column<string>(maxLength: 64, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false, defaultValue: false),
                    AccessFailedCount = table.Column<int>(nullable: false, defaultValue: 0),
                    PositionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_App_Users_Sn_App_DataDictionary_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerApiClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerApiClaims", x => new { x.ApiResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerApiClaims_Sn_App_IdentityServerApiReso~",
                        column: x => x.ApiResourceId,
                        principalTable: "Sn_App_IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerApiScopes",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerApiScopes", x => new { x.ApiResourceId, x.Name });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerApiScopes_Sn_App_IdentityServerApiReso~",
                        column: x => x.ApiResourceId,
                        principalTable: "Sn_App_IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerApiSecrets",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    ApiResourceId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerApiSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerApiSecrets_Sn_App_IdentityServerApiRes~",
                        column: x => x.ApiResourceId,
                        principalTable: "Sn_App_IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientClaims",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientClaims", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientClaims_Sn_App_IdentityServerClie~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientCorsOrigins",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Origin = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientCorsOrigins", x => new { x.ClientId, x.Origin });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientCorsOrigins_Sn_App_IdentityServe~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientGrantTypes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    GrantType = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientGrantTypes", x => new { x.ClientId, x.GrantType });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientGrantTypes_Sn_App_IdentityServer~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientIdPRestrictions",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientIdPRestrictions", x => new { x.ClientId, x.Provider });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientIdPRestrictions_Sn_App_IdentityS~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientPostLogoutRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientPostLogoutRedirectUris", x => new { x.ClientId, x.PostLogoutRedirectUri });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientPostLogoutRedirectUris_Sn_App_Id~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientProperties",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientProperties", x => new { x.ClientId, x.Key });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientProperties_Sn_App_IdentityServer~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    RedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientRedirectUris", x => new { x.ClientId, x.RedirectUri });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientRedirectUris_Sn_App_IdentityServ~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientScopes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Scope = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientScopes", x => new { x.ClientId, x.Scope });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientScopes_Sn_App_IdentityServerClie~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerClientSecrets",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerClientSecrets", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerClientSecrets_Sn_App_IdentityServerCli~",
                        column: x => x.ClientId,
                        principalTable: "Sn_App_IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerIdentityClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    IdentityResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerIdentityClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerIdentityClaims_Sn_App_IdentityServerId~",
                        column: x => x.IdentityResourceId,
                        principalTable: "Sn_App_IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 1024, nullable: true),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_App_RoleClaims_Sn_App_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Sn_App_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_TenantConnectionStrings",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Value = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_TenantConnectionStrings", x => new { x.TenantId, x.Name });
                    table.ForeignKey(
                        name: "FK_Sn_App_TenantConnectionStrings_Sn_App_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Sn_App_Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    IsStatic = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    WebHookUrl = table.Column<string>(nullable: true),
                    WorkflowTemplateGroupId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_WorkflowTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_WorkflowTemplate_Sn_Bpm_WorkflowTemplateGroup_Workfl~",
                        column: x => x.WorkflowTemplateGroupId,
                        principalTable: "Sn_Bpm_WorkflowTemplateGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_File",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    FolderId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Size = table.Column<decimal>(nullable: false),
                    IsShare = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_File_Sn_File_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Sn_File_Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FolderRltPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FolderId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    View = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Use = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FolderRltPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FolderRltPermissions_Sn_File_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Sn_File_Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FolderRltShare",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FolderId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    View = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Use = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FolderRltShare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FolderRltShare_Sn_File_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Sn_File_Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FolderRltTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false),
                    FolderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FolderRltTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FolderRltTag_Sn_File_Folder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Sn_File_Folder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_File_FolderRltTag_Sn_File_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Sn_File_Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    EntityChangeId = table.Column<Guid>(nullable: false),
                    NewValue = table.Column<string>(maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 128, nullable: false),
                    PropertyTypeFullName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_OrganizationRltRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_OrganizationRltRoles", x => new { x.OrganizationId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Sn_App_OrganizationRltRoles_Sn_App_Organization_Organizatio~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_App_OrganizationRltRoles_Sn_App_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Sn_App_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_UserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 1024, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_App_UserClaims_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_UserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 64, nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ProviderKey = table.Column<string>(maxLength: 196, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_UserLogins", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_Sn_App_UserLogins_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_UserRltOrganization",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    OrganizationId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_UserRltOrganization", x => new { x.OrganizationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Sn_App_UserRltOrganization_Sn_App_Organization_Organization~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_App_UserRltOrganization_Sn_App_Organization_Organizatio~1",
                        column: x => x.OrganizationId1,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_App_UserRltOrganization_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Sn_App_UserRoles_Sn_App_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Sn_App_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_App_UserRoles_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Sn_App_UserTokens_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Message_Notice_Notice",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Process = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Message_Notice_Notice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Message_Notice_Notice_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_App_IdentityServerApiScopeClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 200, nullable: false),
                    ApiResourceId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityServerApiScopeClaims", x => new { x.ApiResourceId, x.Name, x.Type });
                    table.ForeignKey(
                        name: "FK_Sn_App_IdentityServerApiScopeClaims_Sn_App_IdentityServerAp~",
                        columns: x => new { x.ApiResourceId, x.Name },
                        principalTable: "Sn_App_IdentityServerApiScopes",
                        principalColumns: new[] { "ApiResourceId", "Name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FormTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: false),
                    FormItems = table.Column<string>(type: "jsonb", nullable: true),
                    Config = table.Column<string>(type: "jsonb", nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_FormTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_FormTemplate_Sn_Bpm_WorkflowTemplate_WorkflowTemplat~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowTemplateRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkflowTemplateId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_WorkflowTemplateRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_WorkflowTemplateRltMember_Sn_Bpm_WorkflowTemplate_Wo~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Cms_Article",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(maxLength: 100, nullable: true),
                    ThumbId = table.Column<Guid>(nullable: true),
                    Author = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Cms_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_Article_Sn_File_File_ThumbId",
                        column: x => x.ThumbId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Cms_Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Code = table.Column<string>(maxLength: 30, nullable: true),
                    Summary = table.Column<string>(maxLength: 200, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ThumbId = table.Column<Guid>(nullable: true),
                    Enable = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Cms_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_Category_Sn_Cms_Category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Cms_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_Category_Sn_File_File_ThumbId",
                        column: x => x.ThumbId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FileRltPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    View = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Use = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FileRltPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileRltPermissions_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FileRltShare",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Edit = table.Column<bool>(nullable: false),
                    View = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Use = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FileRltShare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileRltShare_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FileRltTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FileRltTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileRltTag_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileRltTag_Sn_File_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Sn_File_Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_File_FileVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    FileId = table.Column<Guid>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Size = table.Column<decimal>(nullable: false),
                    OssId = table.Column<Guid>(nullable: false),
                    OssUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_FileVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileVersion_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_File_FileVersion_Sn_File_OssServer_OssId",
                        column: x => x.OssId,
                        principalTable: "Sn_File_OssServer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    FormTemplateId = table.Column<Guid>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_FlowTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_FlowTemplate_Sn_Bpm_FormTemplate_FormTemplateId",
                        column: x => x.FormTemplateId,
                        principalTable: "Sn_Bpm_FormTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Cms_ArticleAccessory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ArticleId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Cms_ArticleAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_ArticleAccessory_Sn_Cms_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Sn_Cms_Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_ArticleAccessory_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Cms_ArticleCarousel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ArticleId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Cms_ArticleCarousel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_ArticleCarousel_Sn_Cms_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Sn_Cms_Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_ArticleCarousel_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Cms_CategoryRltArticle",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    ArticleId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Cms_CategoryRltArticle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_CategoryRltArticle_Sn_Cms_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Sn_Cms_Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Cms_CategoryRltArticle_Sn_Cms_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Sn_Cms_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplateNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FlowTemplateId = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Size = table.Column<List<float>>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    FormItemPermisstions = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_FlowTemplateNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_FlowTemplateNode_Sn_Bpm_FlowTemplate_FlowTemplateId",
                        column: x => x.FlowTemplateId,
                        principalTable: "Sn_Bpm_FlowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplateStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FlowTemplateId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Source = table.Column<Guid>(nullable: false),
                    Target = table.Column<Guid>(nullable: false),
                    SourceAnchor = table.Column<int>(nullable: false),
                    TargetAnchor = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    State = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_FlowTemplateStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_FlowTemplateStep_Sn_Bpm_FlowTemplate_FlowTemplateId",
                        column: x => x.FlowTemplateId,
                        principalTable: "Sn_Bpm_FlowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_Workflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    FlowTemplateId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_Workflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_Workflow_Sn_Bpm_FlowTemplate_FlowTemplateId",
                        column: x => x.FlowTemplateId,
                        principalTable: "Sn_Bpm_FlowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplateNodeRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FlowTemplateNodeId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_FlowTemplateNodeRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_FlowTemplateNodeRltMember_Sn_Bpm_FlowTemplateNode_Fl~",
                        column: x => x.FlowTemplateNodeId,
                        principalTable: "Sn_Bpm_FlowTemplateNode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: false),
                    FormValues = table.Column<string>(type: "jsonb", nullable: true),
                    SourceNodeId = table.Column<Guid>(nullable: true),
                    TargetNodeId = table.Column<Guid>(nullable: true),
                    StepState = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_WorkflowData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_WorkflowData_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowStateRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    MemberId = table.Column<Guid>(nullable: false),
                    MemberType = table.Column<int>(nullable: false),
                    Group = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    WorkflowId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Bpm_WorkflowStateRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Bpm_WorkflowStateRltMember_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Message_Bpm_BpmRltMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    WorkflowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    ProcessorId = table.Column<Guid>(nullable: false),
                    SponsorId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Process = table.Column<bool>(nullable: false),
                    Group = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Message_Bpm_BpmRltMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Message_Bpm_BpmRltMessage_Sn_App_Users_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Message_Bpm_BpmRltMessage_Sn_App_Users_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Message_Bpm_BpmRltMessage_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Message_Bpm_BpmRltMessage_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_AuditLogId",
                table: "AbpAuditLogActions",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_Executio~",
                table: "AbpAuditLogActions",
                columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_AuditLogId",
                table: "AbpEntityChanges",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_DataDictionary_ParentId",
                table: "Sn_App_DataDictionary",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_FeatureValues_Name_ProviderName_ProviderKey",
                table: "Sn_App_FeatureValues",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerClients_ClientId",
                table: "Sn_App_IdentityServerClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerDeviceFlowCodes_DeviceCode",
                table: "Sn_App_IdentityServerDeviceFlowCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerDeviceFlowCodes_Expiration",
                table: "Sn_App_IdentityServerDeviceFlowCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerDeviceFlowCodes_UserCode",
                table: "Sn_App_IdentityServerDeviceFlowCodes",
                column: "UserCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerPersistedGrants_Expiration",
                table: "Sn_App_IdentityServerPersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_IdentityServerPersistedGrants_SubjectId_ClientId_Type",
                table: "Sn_App_IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Organization_Code",
                table: "Sn_App_Organization",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Organization_ParentId",
                table: "Sn_App_Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Organization_TypeId",
                table: "Sn_App_Organization",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_OrganizationRltRoles_RoleId_OrganizationId",
                table: "Sn_App_OrganizationRltRoles",
                columns: new[] { "RoleId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_PermissionGrants_Name_ProviderName_ProviderKey",
                table: "Sn_App_PermissionGrants",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_RoleClaims_RoleId",
                table: "Sn_App_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Roles_NormalizedName",
                table: "Sn_App_Roles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Settings_Name_ProviderName_ProviderKey",
                table: "Sn_App_Settings",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Tenants_Name",
                table: "Sn_App_Tenants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_UserClaims_UserId",
                table: "Sn_App_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_UserLogins_LoginProvider_ProviderKey",
                table: "Sn_App_UserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_UserRltOrganization_OrganizationId1",
                table: "Sn_App_UserRltOrganization",
                column: "OrganizationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_UserRltOrganization_UserId_OrganizationId",
                table: "Sn_App_UserRltOrganization",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_UserRoles_RoleId_UserId",
                table: "Sn_App_UserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Users_Email",
                table: "Sn_App_Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Users_NormalizedEmail",
                table: "Sn_App_Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Users_NormalizedUserName",
                table: "Sn_App_Users",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Users_PositionId",
                table: "Sn_App_Users",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_App_Users_UserName",
                table: "Sn_App_Users",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_FlowTemplate_FormTemplateId",
                table: "Sn_Bpm_FlowTemplate",
                column: "FormTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_FlowTemplateNode_FlowTemplateId",
                table: "Sn_Bpm_FlowTemplateNode",
                column: "FlowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_FlowTemplateNodeRltMember_FlowTemplateNodeId",
                table: "Sn_Bpm_FlowTemplateNodeRltMember",
                column: "FlowTemplateNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_FlowTemplateStep_FlowTemplateId",
                table: "Sn_Bpm_FlowTemplateStep",
                column: "FlowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_FormTemplate_WorkflowTemplateId",
                table: "Sn_Bpm_FormTemplate",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_Workflow_FlowTemplateId",
                table: "Sn_Bpm_Workflow",
                column: "FlowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowData_WorkflowId",
                table: "Sn_Bpm_WorkflowData",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowStateRltMember_WorkflowId",
                table: "Sn_Bpm_WorkflowStateRltMember",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowTemplate_Key",
                table: "Sn_Bpm_WorkflowTemplate",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowTemplate_WorkflowTemplateGroupId",
                table: "Sn_Bpm_WorkflowTemplate",
                column: "WorkflowTemplateGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowTemplateGroup_ParentId",
                table: "Sn_Bpm_WorkflowTemplateGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Bpm_WorkflowTemplateRltMember_WorkflowTemplateId",
                table: "Sn_Bpm_WorkflowTemplateRltMember",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_Article_ThumbId",
                table: "Sn_Cms_Article",
                column: "ThumbId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_ArticleAccessory_ArticleId",
                table: "Sn_Cms_ArticleAccessory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_ArticleAccessory_FileId",
                table: "Sn_Cms_ArticleAccessory",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_ArticleCarousel_ArticleId",
                table: "Sn_Cms_ArticleCarousel",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_ArticleCarousel_FileId",
                table: "Sn_Cms_ArticleCarousel",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_Category_ParentId",
                table: "Sn_Cms_Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_Category_ThumbId",
                table: "Sn_Cms_Category",
                column: "ThumbId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_CategoryRltArticle_ArticleId",
                table: "Sn_Cms_CategoryRltArticle",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Cms_CategoryRltArticle_CategoryId",
                table: "Sn_Cms_CategoryRltArticle",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Common_Area_ParentId",
                table: "Sn_Common_Area",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_File_FolderId",
                table: "Sn_File_File",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileRltPermissions_FileId",
                table: "Sn_File_FileRltPermissions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileRltShare_FileId",
                table: "Sn_File_FileRltShare",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileRltTag_FileId",
                table: "Sn_File_FileRltTag",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileRltTag_TagId",
                table: "Sn_File_FileRltTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileVersion_FileId",
                table: "Sn_File_FileVersion",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FileVersion_OssId",
                table: "Sn_File_FileVersion",
                column: "OssId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_Folder_ParentId",
                table: "Sn_File_Folder",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FolderRltPermissions_FolderId",
                table: "Sn_File_FolderRltPermissions",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FolderRltShare_FolderId",
                table: "Sn_File_FolderRltShare",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FolderRltTag_FolderId",
                table: "Sn_File_FolderRltTag",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_File_FolderRltTag_TagId",
                table: "Sn_File_FolderRltTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Message_Bpm_BpmRltMessage_ProcessorId",
                table: "Sn_Message_Bpm_BpmRltMessage",
                column: "ProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Message_Bpm_BpmRltMessage_SponsorId",
                table: "Sn_Message_Bpm_BpmRltMessage",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Message_Bpm_BpmRltMessage_UserId",
                table: "Sn_Message_Bpm_BpmRltMessage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Message_Bpm_BpmRltMessage_WorkflowId",
                table: "Sn_Message_Bpm_BpmRltMessage",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Message_Notice_Notice_UserId",
                table: "Sn_Message_Notice_Notice",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogActions");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "Sn_App_ClaimTypes");

            migrationBuilder.DropTable(
                name: "Sn_App_FeatureValues");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiScopeClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiSecrets");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientGrantTypes");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientProperties");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientRedirectUris");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientScopes");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClientSecrets");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerDeviceFlowCodes");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerIdentityClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerPersistedGrants");

            migrationBuilder.DropTable(
                name: "Sn_App_OrganizationRltRoles");

            migrationBuilder.DropTable(
                name: "Sn_App_PermissionGrants");

            migrationBuilder.DropTable(
                name: "Sn_App_RoleClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_Settings");

            migrationBuilder.DropTable(
                name: "Sn_App_TenantConnectionStrings");

            migrationBuilder.DropTable(
                name: "Sn_App_UserClaims");

            migrationBuilder.DropTable(
                name: "Sn_App_UserLogins");

            migrationBuilder.DropTable(
                name: "Sn_App_UserRltOrganization");

            migrationBuilder.DropTable(
                name: "Sn_App_UserRoles");

            migrationBuilder.DropTable(
                name: "Sn_App_UserTokens");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplateNodeRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplateStep");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowData");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowStateRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowTemplateRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Cms_ArticleAccessory");

            migrationBuilder.DropTable(
                name: "Sn_Cms_ArticleCarousel");

            migrationBuilder.DropTable(
                name: "Sn_Cms_CategoryRltArticle");

            migrationBuilder.DropTable(
                name: "Sn_Common_Area");

            migrationBuilder.DropTable(
                name: "Sn_File_FileRltPermissions");

            migrationBuilder.DropTable(
                name: "Sn_File_FileRltShare");

            migrationBuilder.DropTable(
                name: "Sn_File_FileRltTag");

            migrationBuilder.DropTable(
                name: "Sn_File_FileVersion");

            migrationBuilder.DropTable(
                name: "Sn_File_FolderRltPermissions");

            migrationBuilder.DropTable(
                name: "Sn_File_FolderRltShare");

            migrationBuilder.DropTable(
                name: "Sn_File_FolderRltTag");

            migrationBuilder.DropTable(
                name: "Sn_Message_Bpm_BpmRltMessage");

            migrationBuilder.DropTable(
                name: "Sn_Message_Notice_Notice");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiScopes");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerClients");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerIdentityResources");

            migrationBuilder.DropTable(
                name: "Sn_App_Tenants");

            migrationBuilder.DropTable(
                name: "Sn_App_Organization");

            migrationBuilder.DropTable(
                name: "Sn_App_Roles");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplateNode");

            migrationBuilder.DropTable(
                name: "Sn_Cms_Article");

            migrationBuilder.DropTable(
                name: "Sn_Cms_Category");

            migrationBuilder.DropTable(
                name: "Sn_File_OssServer");

            migrationBuilder.DropTable(
                name: "Sn_File_Tag");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_Workflow");

            migrationBuilder.DropTable(
                name: "Sn_App_Users");

            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiResources");

            migrationBuilder.DropTable(
                name: "Sn_File_File");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplate");

            migrationBuilder.DropTable(
                name: "Sn_App_DataDictionary");

            migrationBuilder.DropTable(
                name: "Sn_File_Folder");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FormTemplate");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowTemplate");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowTemplateGroup");
        }
    }
}
