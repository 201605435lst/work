using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class Init_Database : Migration
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
                    JobName = table.Column<string>(maxLength: 1000, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1000, nullable: false),
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
                name: "Sn_Alarm_AlarmEquipmentIdBind",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: false),
                    EquipmentThirdIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Alarm_AlarmEquipmentIdBind", x => x.Id);
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
                name: "Sn_App_IdentityUserRltProject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_App_IdentityUserRltProject", x => x.Id);
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
                name: "Sn_Basic_Railway",
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
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_Railway", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Basic_Station",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    SectionStartStationId = table.Column<Guid>(nullable: true),
                    SectionEndStationId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_Station", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_Station_Sn_Basic_Station_SectionEndStationId",
                        column: x => x.SectionEndStationId,
                        principalTable: "Sn_Basic_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_Station_Sn_Basic_Station_SectionStartStationId",
                        column: x => x.SectionStartStationId,
                        principalTable: "Sn_Basic_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_WorkflowTemplateGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_Common_QRCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Border = table.Column<bool>(nullable: false),
                    Size = table.Column<decimal>(nullable: false),
                    Version = table.Column<decimal>(nullable: false),
                    ImageBase64Str = table.Column<string>(nullable: true),
                    ImageSize = table.Column<decimal>(nullable: false),
                    ShowLog = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Common_QRCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyTemplate",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchTemplate",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_Material",
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
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Unit = table.Column<int>(nullable: false),
                    IsSelf = table.Column<bool>(nullable: false),
                    IsPartyAProvide = table.Column<bool>(nullable: false),
                    PresentDays = table.Column<int>(nullable: false),
                    PrePurchaseDays = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_Section",
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
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_Section_Sn_ConstructionBase_Section_Par~",
                        column: x => x.ParentId,
                        principalTable: "Sn_ConstructionBase_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_Worker",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_Worker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_StatisticsEquipmentWorker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Finshed = table.Column<float>(nullable: false),
                    UnFinshed = table.Column<float>(nullable: false),
                    Changed = table.Column<float>(nullable: false),
                    OrgizationName = table.Column<string>(nullable: true),
                    OrgizationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_StatisticsEquipmentWorker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_StatisticsPieWorker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Finshed = table.Column<float>(nullable: false),
                    UnFinshed = table.Column<float>(nullable: false),
                    Changed = table.Column<float>(nullable: false),
                    OrgizationName = table.Column<string>(nullable: true),
                    OrgizationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_StatisticsPieWorker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_WorkOrderTestAdditional",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkOrderId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    TestConctent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_WorkOrderTestAdditional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_YearMonthAlterRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    PlanType = table.Column<int>(nullable: false),
                    ARKey = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    ChangeReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_YearMonthAlterRecord", x => x.Id);
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
                    StaticKey = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
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
                    AccessSecret = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
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
                    Name = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_File_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_Supplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Property = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Principal = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    LegalPerson = table.Column<string>(maxLength: 50, nullable: true),
                    TIN = table.Column<string>(maxLength: 100, nullable: true),
                    BusinessScope = table.Column<string>(maxLength: 500, nullable: true),
                    OpeningBank = table.Column<string>(maxLength: 200, nullable: true),
                    BankAccount = table.Column<string>(maxLength: 100, nullable: true),
                    AccountOpeningUnit = table.Column<string>(maxLength: 200, nullable: true),
                    RegisteredAssets = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Problem_Problem",
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
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Problem_Problem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Problem_ProblemCategory",
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
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Problem_ProblemCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Problem_ProblemCategory_Sn_Problem_ProblemCategory_Paren~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Problem_ProblemCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_ArchivesCategory",
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
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsEncrypt = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_ArchivesCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ArchivesCategory_Sn_Project_ArchivesCategory_Par~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Project_ArchivesCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_BooksClassification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_BooksClassification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_DossierCategory",
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
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_DossierCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_DossierCategory_Sn_Project_DossierCategory_Paren~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Project_DossierCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_FileCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_FileCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_Unit",
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
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Leader = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Telephone = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankAccount = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_Label",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Classify = table.Column<string>(nullable: true),
                    Citation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_Label", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_CableExtend",
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
                    Number = table.Column<int>(nullable: true),
                    SpareNumber = table.Column<int>(nullable: true),
                    RailwayNumber = table.Column<int>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Length = table.Column<float>(nullable: true),
                    LayType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_CableExtend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_TerminalBusinessPath",
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
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_TerminalBusinessPath", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_BlockCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_BlockCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_BlockCategory_Sn_StdBasic_BlockCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_BlockCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ComponentCategory",
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
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    LevelName = table.Column<string>(maxLength: 50, nullable: true),
                    Unit = table.Column<string>(maxLength: 30, nullable: true),
                    ExtendCode = table.Column<string>(maxLength: 50, nullable: true),
                    ExtendName = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategory_Sn_StdBasic_ComponentCategory~",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ComputerCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComputerCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_Manufacturer",
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
                    ShortName = table.Column<string>(maxLength: 50, nullable: true),
                    Introduction = table.Column<string>(maxLength: 500, nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    CSRGCode = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Principal = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_Manufacturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Manufacturer_Sn_StdBasic_Manufacturer_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_MVDCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_MVDCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProcessTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    PrepositionId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Duration = table.Column<decimal>(nullable: false),
                    DurationUnit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProcessTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProcessTemplate_Sn_StdBasic_ProcessTemplate_Par~",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_ProcessTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProductCategory",
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
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    LevelName = table.Column<string>(maxLength: 50, nullable: true),
                    Unit = table.Column<string>(maxLength: 30, nullable: true),
                    ExtendCode = table.Column<string>(maxLength: 50, nullable: true),
                    ExtendName = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    TerminalSymbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategory_Sn_StdBasic_ProductCategory_Par~",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProjectItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProjectItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RepairGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RepairGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairGroup_Sn_StdBasic_RepairGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_RepairGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_Disclose",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Size = table.Column<decimal>(nullable: false),
                    FileType = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_Disclose", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_Disclose_Sn_Technology_Disclose_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Technology_Disclose",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    IsRoot = table.Column<bool>(nullable: false),
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
                    IsChangePassword = table.Column<bool>(nullable: false),
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
                name: "Sn_ConstructionBase_EquipmentTeam",
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
                    Name = table.Column<string>(nullable: true),
                    Spec = table.Column<string>(nullable: true),
                    Cost = table.Column<double>(nullable: false),
                    TypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_EquipmentTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_EquipmentTeam_Sn_App_DataDictionary_Typ~",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_Procedure",
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
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_Procedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_Procedure_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_Standard",
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
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ProfessionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_Standard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_Standard_Sn_App_DataDictionary_Professi~",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CostManagement_Contract",
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
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CostManagement_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_Contract_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CostManagement_CostOther",
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
                    TypeId = table.Column<Guid>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CostManagement_CostOther", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_CostOther_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CostManagement_MoneyList",
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
                    TypeId = table.Column<Guid>(nullable: false),
                    Receivable = table.Column<decimal>(nullable: false),
                    Received = table.Column<decimal>(nullable: false),
                    Due = table.Column<decimal>(nullable: false),
                    Paid = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    PayeeId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CostManagement_MoneyList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_MoneyList_Sn_App_DataDictionary_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_MoneyList_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CostManagement_PeopleCost",
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
                    ProfessionalId = table.Column<Guid>(nullable: false),
                    Money = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    PayeeId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CostManagement_PeopleCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_PeopleCost_Sn_App_DataDictionary_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_PeopleCost_Sn_App_DataDictionary_Professi~",
                        column: x => x.ProfessionalId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_AlterRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Reason = table.Column<string>(maxLength: 500, nullable: true),
                    PlanTime = table.Column<DateTime>(nullable: false),
                    AlterTime = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    AlterType = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    AR_Key = table.Column<Guid>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_AlterRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_AlterRecord_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_DailyPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlanId = table.Column<Guid>(nullable: false),
                    PlanDate = table.Column<DateTime>(nullable: false),
                    Count = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    PlanType = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_DailyPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_DailyPlan_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_DailyPlanAlter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AlterRecordId = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    PlanCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    AlterCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true),
                    AlterTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_DailyPlanAlter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_DailyPlanAlter_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_PlanRelateEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlanDetailId = table.Column<Guid>(nullable: false),
                    EquipmentId = table.Column<Guid>(nullable: true),
                    PlanCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    WorkCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    IsComplete = table.Column<int>(nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_PlanRelateEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_PlanRelateEquipment_Sn_App_DataDictionary_RepairT~",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_RepairUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkerOrderId = table.Column<Guid>(nullable: true),
                    PlanRelateEquipmentId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    Duty = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_RepairUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_RepairUser_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_Worker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkOrderId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Duty = table.Column<int>(nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_Worker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_Worker_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_WorkOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    SkylightPlanId = table.Column<Guid>(nullable: false),
                    StartPlanTime = table.Column<DateTime>(nullable: false),
                    EndPlanTime = table.Column<DateTime>(nullable: false),
                    InfluenceScope = table.Column<string>(maxLength: 1000, nullable: true),
                    ToolSituation = table.Column<string>(nullable: true),
                    Announcements = table.Column<string>(nullable: true),
                    DispatchingTime = table.Column<DateTime>(nullable: false),
                    SendWorkersId = table.Column<Guid>(nullable: false),
                    StartRealityTime = table.Column<DateTime>(nullable: false),
                    EndRealityTime = table.Column<DateTime>(nullable: false),
                    OrderNo = table.Column<string>(maxLength: 50, nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    OrderType = table.Column<int>(nullable: false),
                    OrderState = table.Column<int>(nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_WorkOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_WorkOrder_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_WorkOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WorkOrderId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Duty = table.Column<int>(nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_WorkOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_WorkOrganization_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_YearMonthPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairDetailsId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    RepairGroup = table.Column<string>(maxLength: 50, nullable: true),
                    RepairType = table.Column<int>(nullable: false),
                    DeviceName = table.Column<string>(maxLength: 500, nullable: true),
                    IsImport = table.Column<bool>(nullable: true),
                    RepairContent = table.Column<string>(maxLength: 1000, nullable: true),
                    CompiledOrganization = table.Column<string>(maxLength: 500, nullable: true),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DeviceCount = table.Column<int>(nullable: false),
                    PlanType = table.Column<int>(nullable: false),
                    Times = table.Column<string>(maxLength: 50, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    ResponsibleUnit = table.Column<Guid>(nullable: false),
                    EquipmentLocation = table.Column<string>(maxLength: 500, nullable: true),
                    RepairMonth = table.Column<string>(maxLength: 50, nullable: true),
                    CreateUser = table.Column<string>(maxLength: 50, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    UnitTime = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    State = table.Column<int>(nullable: false),
                    PlanCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    AR_Key = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    ParentType = table.Column<int>(nullable: true),
                    SkyligetType = table.Column<string>(maxLength: 100, nullable: true),
                    Col_1 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_2 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_3 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_4 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_5 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_6 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_7 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_8 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_9 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_10 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_11 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_12 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_13 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_14 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_15 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_16 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_17 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_18 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_19 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_20 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_21 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_22 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_23 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_24 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_25 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_26 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_27 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_28 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_29 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_30 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_31 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_YearMonthPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_YearMonthPlan_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_YearMonthPlanAlter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlanId = table.Column<Guid>(nullable: false),
                    ExecYear = table.Column<int>(nullable: false),
                    ExecMonth = table.Column<int>(nullable: false),
                    IsImport = table.Column<bool>(nullable: true),
                    SkyligetType = table.Column<string>(maxLength: 100, nullable: true),
                    EquipmentLocation = table.Column<string>(maxLength: 500, nullable: true),
                    CompiledOrganization = table.Column<string>(maxLength: 100, nullable: true),
                    PlanType = table.Column<int>(nullable: false),
                    IsExec = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    AR_Key = table.Column<Guid>(nullable: true),
                    WorkShop = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 200, nullable: true),
                    FileId = table.Column<Guid>(nullable: false),
                    CreateUser = table.Column<string>(maxLength: 20, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    Total = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    PlanCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_1 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_2 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_3 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_4 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_5 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_6 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_7 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_8 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_9 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_10 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_11 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_12 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_13 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_14 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_15 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_16 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_17 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_18 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_19 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_20 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_21 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_22 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_23 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_24 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_25 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_26 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_27 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_28 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_29 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_30 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    Col_31 = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    ChangeReason = table.Column<string>(maxLength: 500, nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true),
                    YearMonthAlterRecordId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_YearMonthPlanAlter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_YearMonthPlanAlter_Sn_App_DataDictionary_RepairTa~",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlan",
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
                    Name = table.Column<string>(maxLength: 120, nullable: true),
                    LevelId = table.Column<Guid>(nullable: false),
                    Summary = table.Column<string>(maxLength: 1000, nullable: true),
                    Flow = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 120, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlan_Sn_App_DataDictionary_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRecord",
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
                    Name = table.Column<string>(maxLength: 120, nullable: true),
                    LevelId = table.Column<Guid>(nullable: false),
                    Summary = table.Column<string>(maxLength: 1000, nullable: true),
                    Flow = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 120, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecord_Sn_App_DataDictionary_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemLibrary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    ProfessionId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Measures = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemLibrary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemLibrary_Sn_App_DataDictionary_Prof~",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemLibrary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    EventTypeId = table.Column<Guid>(nullable: false),
                    ProfessionId = table.Column<Guid>(nullable: false),
                    RiskLevel = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Measures = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemLibrary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemLibrary_Sn_App_DataDictionary_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemLibrary_Sn_App_DataDictionary_Profession~",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_IndividualProject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_IndividualProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_IndividualProject_Sn_StdBasic_IndividualProject~",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_IndividualProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_IndividualProject_Sn_App_DataDictionary_Special~",
                        column: x => x.SpecialtyId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_InfluenceRange",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairLevel = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    TagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_InfluenceRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_InfluenceRange_Sn_App_DataDictionary_TagId",
                        column: x => x.TagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_WorkAttention",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: true),
                    IsType = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_WorkAttention", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_WorkAttention_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_WorkAttention_Sn_StdBasic_WorkAttention_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_StdBasic_WorkAttention",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_Material",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: true),
                    Spec = table.Column<string>(nullable: true),
                    ProductCategoryId = table.Column<Guid>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_Material", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_Material_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
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
                name: "Sn_CrPlan_SkylightPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Level = table.Column<string>(nullable: true),
                    RailwayId = table.Column<Guid>(nullable: true),
                    Station = table.Column<Guid>(nullable: false),
                    StationRelateRailwayType = table.Column<int>(nullable: false),
                    WorkArea = table.Column<string>(maxLength: 500, nullable: true),
                    TimeLength = table.Column<int>(nullable: false),
                    WorkTime = table.Column<DateTime>(nullable: false),
                    WorkContent = table.Column<string>(nullable: true),
                    WorkContentType = table.Column<int>(nullable: true),
                    Incidence = table.Column<string>(nullable: true),
                    WorkUnit = table.Column<Guid>(nullable: false),
                    WorkAreaId = table.Column<Guid>(nullable: true),
                    SubmitUser = table.Column<Guid>(nullable: true),
                    ResponsibleUser = table.Column<Guid>(nullable: true),
                    CreateUser = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    PlanType = table.Column<int>(nullable: false),
                    PlanState = table.Column<int>(nullable: false),
                    Opinion = table.Column<string>(nullable: true),
                    RegistrationPlace = table.Column<string>(nullable: true),
                    IsAdjacent = table.Column<bool>(nullable: false),
                    EndStationRelateRailwayType = table.Column<int>(nullable: false),
                    EndStationId = table.Column<Guid>(nullable: true),
                    IsChange = table.Column<bool>(nullable: false),
                    ChangTime = table.Column<string>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true),
                    IsOnRoad = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_SkylightPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlan_Sn_Basic_Railway_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Sn_Basic_Railway",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlan_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Basic_StationRltRailway",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    RailwayId = table.Column<Guid>(nullable: false),
                    StationId = table.Column<Guid>(nullable: false),
                    PassOrder = table.Column<int>(nullable: false),
                    KMMark = table.Column<int>(nullable: false),
                    RailwayType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_StationRltRailway", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_StationRltRailway_Sn_Basic_Railway_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Sn_Basic_Railway",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_StationRltRailway_Sn_Basic_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Sn_Basic_Station",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_StdBasic_QuotaCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    StandardCodeId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    AreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_QuotaCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaCategory_Sn_Common_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Sn_Common_Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaCategory_Sn_StdBasic_QuotaCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_QuotaCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaCategory_Sn_App_DataDictionary_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaCategory_Sn_App_DataDictionary_StandardCod~",
                        column: x => x.StandardCodeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialOfBill",
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
                    ConstructionTeam = table.Column<string>(nullable: true),
                    SectionId = table.Column<Guid>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialOfBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialOfBill_Sn_ConstructionBase_Section_Sect~",
                        column: x => x.SectionId,
                        principalTable: "Sn_ConstructionBase_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Url = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
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
                name: "Sn_Material_SupplierRltContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Telephone = table.Column<string>(maxLength: 50, nullable: true),
                    LandlinePhone = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    QQ = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_SupplierRltContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_SupplierRltContacts_Sn_Material_Supplier_Suppli~",
                        column: x => x.SupplierId,
                        principalTable: "Sn_Material_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Problem_ProblemRltProblemCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProblemId = table.Column<Guid>(nullable: false),
                    ProblemCategoryId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Problem_ProblemRltProblemCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Problem_ProblemRltProblemCategory_Sn_Problem_ProblemCate~",
                        column: x => x.ProblemCategoryId,
                        principalTable: "Sn_Problem_ProblemCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Problem_ProblemRltProblemCategory_Sn_Problem_Problem_Pro~",
                        column: x => x.ProblemId,
                        principalTable: "Sn_Problem_Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_Archives",
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
                    ArchivesCategoryId = table.Column<Guid>(nullable: false),
                    FileCode = table.Column<string>(nullable: true),
                    ProjectCode = table.Column<string>(nullable: true),
                    ArchivesFilesCode = table.Column<string>(nullable: true),
                    BooksClassificationId = table.Column<Guid>(nullable: false),
                    Year = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Security = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Copies = table.Column<int>(nullable: false),
                    Page = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_Archives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Archives_Sn_Project_ArchivesCategory_ArchivesCat~",
                        column: x => x.ArchivesCategoryId,
                        principalTable: "Sn_Project_ArchivesCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Archives_Sn_Project_BooksClassification_BooksCla~",
                        column: x => x.BooksClassificationId,
                        principalTable: "Sn_Project_BooksClassification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_Block",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    BlockCategoryId = table.Column<Guid>(nullable: true),
                    TwoDPreview = table.Column<string>(nullable: true),
                    TwoDSymbol = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_Block", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Block_Sn_StdBasic_BlockCategory_BlockCategoryId",
                        column: x => x.BlockCategoryId,
                        principalTable: "Sn_StdBasic_BlockCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Alarm_AlarmConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    MaxValue = table.Column<decimal>(nullable: false),
                    MinValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Alarm_AlarmConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Alarm_AlarmConfig_Sn_StdBasic_ComponentCategory_Componen~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_BasePrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComputerCodeId = table.Column<Guid>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    StandardId = table.Column<Guid>(nullable: true),
                    StandardCodeId = table.Column<Guid>(nullable: false),
                    AreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_BasePrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_BasePrice_Sn_Common_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Sn_Common_Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_BasePrice_Sn_StdBasic_ComputerCode_ComputerCode~",
                        column: x => x.ComputerCodeId,
                        principalTable: "Sn_StdBasic_ComputerCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_BasePrice_Sn_App_DataDictionary_StandardCodeId",
                        column: x => x.StandardCodeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_BasePrice_Sn_App_DataDictionary_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ComponentCategoryRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    ComputerCodeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComponentCategoryRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltMaterial_Sn_StdBasic_Compon~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltMaterial_Sn_StdBasic_Comput~",
                        column: x => x.ComputerCodeId,
                        principalTable: "Sn_StdBasic_ComputerCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_EquipmentControlType",
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
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: true),
                    TypeGroup = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_EquipmentControlType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_EquipmentControlType_Sn_StdBasic_Manufacturer_M~",
                        column: x => x.ManufacturerId,
                        principalTable: "Sn_StdBasic_Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_MVDProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(maxLength: 50, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsInstance = table.Column<bool>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    MVDCategoryId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_MVDProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_MVDProperty_Sn_StdBasic_MVDCategory_MVDCategory~",
                        column: x => x.MVDCategoryId,
                        principalTable: "Sn_StdBasic_MVDCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProjectItemRltProcessTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectItemId = table.Column<Guid>(nullable: false),
                    ProcessTemplateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProjectItemRltProcessTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltProcessTemplate_Sn_StdBasic_Proce~",
                        column: x => x.ProcessTemplateId,
                        principalTable: "Sn_StdBasic_ProcessTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ComponentCategoryRltProductCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: true),
                    ProductionCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComponentCategoryRltProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltProductCategory_Sn_StdBasic~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltProductCategory_Sn_StdBasi~1",
                        column: x => x.ProductionCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_Model",
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
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    CSRGCode = table.Column<string>(maxLength: 100, nullable: true),
                    ComponentCategoryId = table.Column<Guid>(nullable: true),
                    ProductCategoryId = table.Column<Guid>(nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: true),
                    ServiceLife = table.Column<float>(nullable: true),
                    ServiceLifeUnit = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_Model", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Model_Sn_StdBasic_ComponentCategory_ComponentCa~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Model_Sn_StdBasic_Manufacturer_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Sn_StdBasic_Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Model_Sn_StdBasic_ProductCategory_ProductCatego~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProductCategoryRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: false),
                    ComputerCodeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProductCategoryRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltMaterial_Sn_StdBasic_Computer~",
                        column: x => x.ComputerCodeId,
                        principalTable: "Sn_StdBasic_ComputerCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltMaterial_Sn_StdBasic_ProductC~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProjectItemRltComponentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectItemId = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProjectItemRltComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltComponentCategory_Sn_StdBasic_Com~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltComponentCategory_Sn_StdBasic_Pro~",
                        column: x => x.ProjectItemId,
                        principalTable: "Sn_StdBasic_ProjectItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProjectItemRltProductCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectItemId = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProjectItemRltProductCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltProductCategory_Sn_StdBasic_Produ~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltProductCategory_Sn_StdBasic_Proje~",
                        column: x => x.ProjectItemId,
                        principalTable: "Sn_StdBasic_ProjectItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RepairItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Unit = table.Column<string>(maxLength: 20, nullable: true),
                    Period = table.Column<string>(maxLength: 50, nullable: true),
                    PeriodUnit = table.Column<int>(maxLength: 50, nullable: false),
                    IsMonth = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    TagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RepairItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItem_Sn_StdBasic_RepairGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Sn_StdBasic_RepairGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItem_Sn_App_DataDictionary_TagId",
                        column: x => x.TagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Sn_Basic_InstallationSite",
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
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    CSRGCode = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CodeName = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    UseType = table.Column<int>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: true),
                    LocationType = table.Column<int>(nullable: true),
                    RailwayDirection = table.Column<int>(nullable: true),
                    Location = table.Column<string>(maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    UseDate = table.Column<DateTime>(nullable: true),
                    RailwayId = table.Column<Guid>(nullable: true),
                    StationId = table.Column<Guid>(nullable: true),
                    KMMark = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_InstallationSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_App_DataDictionary_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_Basic_InstallationSite_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Basic_InstallationSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_Basic_Railway_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Sn_Basic_Railway",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_Basic_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Sn_Basic_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_InstallationSite_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Basic_RailwayRltOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RailwayId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    DownLinkStartKM = table.Column<int>(nullable: false),
                    DownLinkEndKM = table.Column<int>(nullable: false),
                    UpLinkStartKM = table.Column<int>(nullable: false),
                    UpLinkEndKM = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_RailwayRltOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_RailwayRltOrganization_Sn_App_Organization_Organiz~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_RailwayRltOrganization_Sn_Basic_Railway_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Sn_Basic_Railway",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Basic_StationRltOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    StationId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    IsBackUp = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Basic_StationRltOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_StationRltOrganization_Sn_App_Organization_Organiz~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Basic_StationRltOrganization_Sn_Basic_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Sn_Basic_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_MaintenanceWork",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    MaintenanceProject = table.Column<string>(nullable: true),
                    RepairLevel = table.Column<string>(nullable: true),
                    MaintenanceType = table.Column<int>(nullable: false),
                    ARKey = table.Column<Guid>(nullable: true),
                    SecondARKey = table.Column<Guid>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_MaintenanceWork", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWork_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWork_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Oa_DutySchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_DutySchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_DutySchedule_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_Institution",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Classify = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    EffectiveTime = table.Column<DateTime>(nullable: false),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    Abstract = table.Column<string>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: false),
                    NewsClassify = table.Column<int>(nullable: false),
                    IsApprove = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_Institution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_Institution_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Report_Report",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Plan = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    WorkRecord = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Report_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Report_Report_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_EquipmentGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_EquipmentGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentGroup_Sn_App_Organization_Organization~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentGroup_Sn_Resource_EquipmentGroup_Paren~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Resource_EquipmentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreHouse",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreHouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreHouse_Sn_Common_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Sn_Common_Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreHouse_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreHouse_Sn_Resource_StoreHouse_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Resource_StoreHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Sn_ConstructionBase_SubItem",
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
                    Name = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_SubItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_SubItem_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_WorkTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderNumber = table.Column<string>(nullable: true),
                    FillInTime = table.Column<DateTime>(nullable: true),
                    WorkTitle = table.Column<string>(nullable: true),
                    WorkPlace = table.Column<string>(nullable: true),
                    RepairLevel = table.Column<string>(nullable: true),
                    WorkContent = table.Column<string>(nullable: true),
                    InfluenceRange = table.Column<string>(nullable: true),
                    PlanStartTime = table.Column<DateTime>(nullable: true),
                    PlanFinishTime = table.Column<DateTime>(nullable: true),
                    RealStartTime = table.Column<DateTime>(nullable: true),
                    RealFinsihTime = table.Column<DateTime>(nullable: true),
                    SecurityMeasuresAndAttentions = table.Column<string>(nullable: true),
                    PaperMaker = table.Column<string>(nullable: true),
                    PersonInCharge = table.Column<string>(nullable: true),
                    TechnicalCheckerId = table.Column<Guid>(nullable: true),
                    SafetyDispatchCheckerId = table.Column<Guid>(nullable: true),
                    FinishContent = table.Column<string>(nullable: true),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    SafeGuard = table.Column<bool>(nullable: true),
                    IsFine = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_WorkTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_WorkTicket_Sn_App_Users_SafetyDispatchCheckerId",
                        column: x => x.SafetyDispatchCheckerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_WorkTicket_Sn_App_Users_TechnicalCheckerId",
                        column: x => x.TechnicalCheckerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_Contract",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Contract_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialAcceptance",
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
                    TestingOrganizationId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    TestingType = table.Column<int>(nullable: false),
                    TestingStatus = table.Column<int>(nullable: false),
                    ReceptionTime = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialAcceptance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptance_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptance_Sn_App_DataDictionary_Testin~",
                        column: x => x.TestingOrganizationId,
                        principalTable: "Sn_App_DataDictionary",
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
                name: "Sn_Oa_Contract",
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
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    PartyA = table.Column<string>(nullable: true),
                    PartyB = table.Column<string>(nullable: true),
                    PartyC = table.Column<string>(nullable: true),
                    SignTime = table.Column<DateTime>(nullable: false),
                    HostDepartmentId = table.Column<Guid>(nullable: false),
                    UndertakerId = table.Column<Guid>(nullable: false),
                    UnderDepartmentId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    AmountWords = table.Column<string>(nullable: true),
                    Budge = table.Column<decimal>(nullable: false),
                    TypeId = table.Column<Guid>(nullable: true),
                    Abstract = table.Column<string>(nullable: true),
                    OtherPartInfo = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_Contract_Sn_App_Organization_HostDepartmentId",
                        column: x => x.HostDepartmentId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_Contract_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_Contract_Sn_App_Organization_UnderDepartmentId",
                        column: x => x.UnderDepartmentId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_Contract_Sn_App_Users_UndertakerId",
                        column: x => x.UndertakerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    ManagerId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PlannedStartTime = table.Column<DateTime>(nullable: false),
                    PlannedEndTime = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DetailAddress = table.Column<string>(nullable: true),
                    ConstructionPeriod = table.Column<decimal>(nullable: false),
                    TypeId = table.Column<Guid>(nullable: false),
                    QualityLevel = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Progress = table.Column<decimal>(nullable: false),
                    Scale = table.Column<string>(nullable: true),
                    Cost = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lng = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Project_Sn_App_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Project_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Project_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    ProfessionId = table.Column<Guid>(nullable: false),
                    CheckTime = table.Column<DateTime>(nullable: true),
                    LimitTime = table.Column<DateTime>(nullable: true),
                    CheckUnitId = table.Column<Guid>(nullable: true),
                    CheckUnitName = table.Column<string>(nullable: true),
                    CheckerId = table.Column<Guid>(nullable: false),
                    ResponsibleUserId = table.Column<Guid>(nullable: true),
                    VerifierId = table.Column<Guid>(nullable: true),
                    ResponsibleUnit = table.Column<string>(nullable: true),
                    ResponsibleOrganizationId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Suggestion = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_Organization_CheckUnitId",
                        column: x => x.CheckUnitId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_Users_CheckerId",
                        column: x => x.CheckerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_DataDictionary_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_Organization_ResponsibleOr~",
                        column: x => x.ResponsibleOrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblem_Sn_App_Users_VerifierId",
                        column: x => x.VerifierId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreEquipmentTest",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    TesterId = table.Column<Guid>(nullable: true),
                    TesterName = table.Column<string>(nullable: true),
                    Passed = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreEquipmentTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTest_Sn_App_Organization_Organiza~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTest_Sn_App_Users_TesterId",
                        column: x => x.TesterId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: false),
                    ProfessionId = table.Column<Guid>(nullable: false),
                    RiskLevel = table.Column<int>(nullable: false),
                    CheckTime = table.Column<DateTime>(nullable: true),
                    LimitTime = table.Column<DateTime>(nullable: true),
                    CheckUnitId = table.Column<Guid>(nullable: true),
                    CheckUnitName = table.Column<string>(nullable: true),
                    CheckerId = table.Column<Guid>(nullable: false),
                    ResponsibleUserId = table.Column<Guid>(nullable: true),
                    VerifierId = table.Column<Guid>(nullable: true),
                    ResponsibleUnit = table.Column<string>(nullable: true),
                    ResponsibleOrganizationId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Suggestion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_Organization_CheckUnitId",
                        column: x => x.CheckUnitId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_Users_CheckerId",
                        column: x => x.CheckerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_DataDictionary_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_Organization_ResponsibleOrganiza~",
                        column: x => x.ResponsibleOrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_DataDictionary_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblem_Sn_App_Users_VerifierId",
                        column: x => x.VerifierId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_ProcedureEquipmentTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    EquipmentTeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_ProcedureEquipmentTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureEquipmentTeam_Sn_ConstructionB~",
                        column: x => x.EquipmentTeamId,
                        principalTable: "Sn_ConstructionBase_EquipmentTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureEquipmentTeam_Sn_Construction~1",
                        column: x => x.ProcedureId,
                        principalTable: "Sn_ConstructionBase_Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_ProcedureMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    ConstructionBaseMaterialId = table.Column<Guid>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_ProcedureMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureMaterial_Sn_ConstructionBase_M~",
                        column: x => x.ConstructionBaseMaterialId,
                        principalTable: "Sn_ConstructionBase_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureMaterial_Sn_ConstructionBase_P~",
                        column: x => x.ProcedureId,
                        principalTable: "Sn_ConstructionBase_Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_ProcedureWorker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_ProcedureWorker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureWorker_Sn_ConstructionBase_Pro~",
                        column: x => x.ProcedureId,
                        principalTable: "Sn_ConstructionBase_Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureWorker_Sn_ConstructionBase_Wor~",
                        column: x => x.WorkerId,
                        principalTable: "Sn_ConstructionBase_Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRltComponentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmergPlanId = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRltComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRltComponentCategory_Sn_StdBasic_Componen~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRltComponentCategory_Sn_Emerg_EmergPlan_E~",
                        column: x => x.EmergPlanId,
                        principalTable: "Sn_Emerg_EmergPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanProcessRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmergPlanRecordId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    NodeId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanProcessRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanProcessRecord_Sn_Emerg_EmergPlanRecord_Em~",
                        column: x => x.EmergPlanRecordId,
                        principalTable: "Sn_Emerg_EmergPlanRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRecordRltComponentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmergPlanRecordId = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRecordRltComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecordRltComponentCategory_Sn_StdBasic_Co~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecordRltComponentCategory_Sn_Emerg_Emerg~",
                        column: x => x.EmergPlanRecordId,
                        principalTable: "Sn_Emerg_EmergPlanRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRecordRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmergPlanRecordId = table.Column<Guid>(nullable: false),
                    MemberType = table.Column<int>(nullable: false),
                    MemeberId = table.Column<Guid>(nullable: false),
                    Group = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRecordRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecordRltMember_Sn_Emerg_EmergPlanRecord_~",
                        column: x => x.EmergPlanRecordId,
                        principalTable: "Sn_Emerg_EmergPlanRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_Fault",
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
                    OrganizationId = table.Column<Guid>(nullable: false),
                    RailwayId = table.Column<Guid>(nullable: false),
                    StationId = table.Column<Guid>(nullable: false),
                    EquipmentNames = table.Column<string>(maxLength: 120, nullable: true),
                    Summary = table.Column<string>(maxLength: 120, nullable: true),
                    LevelId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Abnormal = table.Column<string>(maxLength: 1000, nullable: true),
                    ReasonTypeId = table.Column<Guid>(nullable: false),
                    Reason = table.Column<string>(maxLength: 1000, nullable: true),
                    WeatherDetail = table.Column<string>(maxLength: 120, nullable: true),
                    TemperatureMax = table.Column<float>(nullable: true),
                    TemperatureMin = table.Column<float>(nullable: true),
                    DisposeProcess = table.Column<string>(maxLength: 5000, nullable: true),
                    DisposePersons = table.Column<string>(maxLength: 500, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    Source = table.Column<int>(nullable: false),
                    EmergPlanRecordId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CheckInTime = table.Column<DateTime>(nullable: false),
                    CheckOutTime = table.Column<DateTime>(nullable: true),
                    CheckInUserId = table.Column<Guid>(nullable: true),
                    CheckOutUserId = table.Column<Guid>(nullable: true),
                    SubmitUserId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_Fault", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_Users_CheckInUserId",
                        column: x => x.CheckInUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_Users_CheckOutUserId",
                        column: x => x.CheckOutUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_Emerg_EmergPlanRecord_EmergPlanRecordId",
                        column: x => x.EmergPlanRecordId,
                        principalTable: "Sn_Emerg_EmergPlanRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_DataDictionary_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_Basic_Railway_RailwayId",
                        column: x => x.RailwayId,
                        principalTable: "Sn_Basic_Railway",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_DataDictionary_ReasonTypeId",
                        column: x => x.ReasonTypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_Basic_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Sn_Basic_Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_Fault_Sn_App_Users_SubmitUserId",
                        column: x => x.SubmitUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemLibraryRltScop",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QualityProblemLibraryId = table.Column<Guid>(nullable: false),
                    ScopId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemLibraryRltScop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemLibraryRltScop_Sn_Quality_QualityP~",
                        column: x => x.QualityProblemLibraryId,
                        principalTable: "Sn_Quality_QualityProblemLibrary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemLibraryRltScop_Sn_App_DataDictiona~",
                        column: x => x.ScopId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemLibraryRltScop",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SafeProblemLibraryId = table.Column<Guid>(nullable: false),
                    ScopId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemLibraryRltScop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemLibraryRltScop_Sn_Safe_SafeProblemLibrar~",
                        column: x => x.SafeProblemLibraryId,
                        principalTable: "Sn_Safe_SafeProblemLibrary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemLibraryRltScop_Sn_App_DataDictionary_Sco~",
                        column: x => x.ScopId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProjectItemRltIndividualProject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectItemId = table.Column<Guid>(nullable: false),
                    IndividualProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProjectItemRltIndividualProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltIndividualProject_Sn_StdBasic_Ind~",
                        column: x => x.IndividualProjectId,
                        principalTable: "Sn_StdBasic_IndividualProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProjectItemRltIndividualProject_Sn_StdBasic_Pro~",
                        column: x => x.ProjectItemId,
                        principalTable: "Sn_StdBasic_ProjectItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_PlanDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SkylightPlanId = table.Column<Guid>(nullable: false),
                    DailyPlanId = table.Column<Guid>(nullable: false),
                    InfluenceRangeId = table.Column<Guid>(nullable: true),
                    PlanCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    WorkCount = table.Column<decimal>(type: "decimal(13, 3)", nullable: false),
                    RepairTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_PlanDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_PlanDetail_Sn_StdBasic_InfluenceRange_InfluenceRa~",
                        column: x => x.InfluenceRangeId,
                        principalTable: "Sn_StdBasic_InfluenceRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_PlanDetail_Sn_App_DataDictionary_RepairTagId",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_StdBasic_Quota",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuotaCategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(nullable: false),
                    LaborCost = table.Column<decimal>(nullable: false),
                    MaterialCost = table.Column<decimal>(nullable: false),
                    MachineCost = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_Quota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_Quota_Sn_StdBasic_QuotaCategory_QuotaCategoryId",
                        column: x => x.QuotaCategoryId,
                        principalTable: "Sn_StdBasic_QuotaCategory",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_ConstructionBase_ProcedureRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_ProcedureRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_ProcedureRltFile_Sn_ConstructionBase_Pr~",
                        column: x => x.ProcedureId,
                        principalTable: "Sn_ConstructionBase_Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CostManagement_ContractRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContractId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CostManagement_ContractRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_ContractRltFile_Sn_CostManagement_Contrac~",
                        column: x => x.ContractId,
                        principalTable: "Sn_CostManagement_Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CostManagement_ContractRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_EquipmentTestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlanRelateEquipmentId = table.Column<Guid>(nullable: false),
                    TestId = table.Column<Guid>(nullable: false),
                    TestName = table.Column<string>(maxLength: 100, nullable: true),
                    TestResult = table.Column<string>(maxLength: 100, nullable: true),
                    CheckResult = table.Column<string>(maxLength: 100, nullable: true),
                    TestType = table.Column<int>(nullable: false),
                    PredictedValue = table.Column<string>(nullable: true),
                    MaxRated = table.Column<decimal>(nullable: false),
                    FileId = table.Column<Guid>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    MinRated = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_EquipmentTestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_EquipmentTestResult_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_EquipmentTestResult_Sn_App_DataDictionary_RepairT~",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_YearMonthPlanTestItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairDetailsID = table.Column<Guid>(nullable: false),
                    PlanYear = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    TestType = table.Column<int>(nullable: false),
                    TestUnit = table.Column<string>(maxLength: 50, nullable: true),
                    TestContent = table.Column<string>(maxLength: 500, nullable: true),
                    PredictedValue = table.Column<string>(maxLength: 5000, nullable: true),
                    MaxRated = table.Column<float>(nullable: true),
                    MinRated = table.Column<float>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    RepairTagId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_YearMonthPlanTestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_YearMonthPlanTestItem_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_YearMonthPlanTestItem_Sn_App_DataDictionary_Repai~",
                        column: x => x.RepairTagId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRecordRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    EmergPlanRecordId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRecordRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecordRltFile_Sn_Emerg_EmergPlanRecord_Em~",
                        column: x => x.EmergPlanRecordId,
                        principalTable: "Sn_Emerg_EmergPlanRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRecordRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_EmergPlanRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    EmergPlanId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_EmergPlanRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRltFile_Sn_Emerg_EmergPlan_EmergPlanId",
                        column: x => x.EmergPlanId,
                        principalTable: "Sn_Emerg_EmergPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_EmergPlanRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    OssUrl = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
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
                name: "Sn_Material_MaterialOfBillRltAccessory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    MaterialOfBillId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialOfBillRltAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialOfBillRltAccessory_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialOfBillRltAccessory_Sn_Material_Material~",
                        column: x => x.MaterialOfBillId,
                        principalTable: "Sn_Material_MaterialOfBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_Partition",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    X = table.Column<decimal>(nullable: false),
                    Y = table.Column<decimal>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    TopId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_Partition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Partition_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Partition_Sn_Material_Partition_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Material_Partition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_SupplierRltAccessory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_SupplierRltAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_SupplierRltAccessory_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_SupplierRltAccessory_Sn_Material_Supplier_Suppl~",
                        column: x => x.SupplierId,
                        principalTable: "Sn_Material_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Oa_Seal",
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
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    ImageId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_Seal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_Seal_Sn_File_File_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeSpeechVideo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    VideoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeSpeechVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeSpeechVideo_Sn_File_File_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_Dossier",
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
                    ArchivesId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    PersonName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Page = table.Column<int>(nullable: false),
                    FileCategoryId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_Dossier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Dossier_Sn_Project_Archives_ArchivesId",
                        column: x => x.ArchivesId,
                        principalTable: "Sn_Project_Archives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Project_Dossier_Sn_Project_FileCategory_FileCategoryId",
                        column: x => x.FileCategoryId,
                        principalTable: "Sn_Project_FileCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ComponentCategoryRltMVDProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(maxLength: 50, nullable: true),
                    MVDPropertyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComponentCategoryRltMVDProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltMVDProperty_Sn_StdBasic_Com~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltMVDProperty_Sn_StdBasic_MVD~",
                        column: x => x.MVDPropertyId,
                        principalTable: "Sn_StdBasic_MVDProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProductCategoryRltMVDProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(maxLength: 50, nullable: true),
                    MVDPropertyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProductCategoryRltMVDProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltMVDProperty_Sn_StdBasic_MVDPr~",
                        column: x => x.MVDPropertyId,
                        principalTable: "Sn_StdBasic_MVDProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltMVDProperty_Sn_StdBasic_Produ~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ModelFile",
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
                    ModelId = table.Column<Guid>(nullable: true),
                    ThumbId = table.Column<Guid>(nullable: true),
                    FamilyFileId = table.Column<Guid>(nullable: true),
                    DetailLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ModelFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelFile_Sn_File_File_FamilyFileId",
                        column: x => x.FamilyFileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelFile_Sn_StdBasic_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelFile_Sn_File_File_ThumbId",
                        column: x => x.ThumbId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ModelRltBlock",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: true),
                    BlockId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ModelRltBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltBlock_Sn_StdBasic_Block_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Sn_StdBasic_Block",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltBlock_Sn_StdBasic_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ModelRltModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ModelRltModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltModel_Sn_StdBasic_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltModel_Sn_StdBasic_Model_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ModelRltMVDProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(maxLength: 50, nullable: true),
                    MVDPropertyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ModelRltMVDProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltMVDProperty_Sn_StdBasic_MVDProperty_MVD~",
                        column: x => x.MVDPropertyId,
                        principalTable: "Sn_StdBasic_MVDProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelRltMVDProperty_Sn_StdBasic_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ModelTerminal",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ModelTerminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelTerminal_Sn_StdBasic_Model_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Sn_StdBasic_Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ModelTerminal_Sn_StdBasic_ProductCategory_Produ~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RepairItemRltComponentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairItemId = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RepairItemRltComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItemRltComponentCategory_Sn_StdBasic_Comp~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItemRltComponentCategory_Sn_StdBasic_Repa~",
                        column: x => x.RepairItemId,
                        principalTable: "Sn_StdBasic_RepairItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RepairItemRltOrganizationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairItemId = table.Column<Guid>(nullable: false),
                    OrganizationTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RepairItemRltOrganizationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItemRltOrganizationType_Sn_App_DataDictio~",
                        column: x => x.OrganizationTypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairItemRltOrganizationType_Sn_StdBasic_Repai~",
                        column: x => x.RepairItemId,
                        principalTable: "Sn_StdBasic_RepairItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RepairTestItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RepairItemId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    DefaultValue = table.Column<string>(maxLength: 500, nullable: true),
                    MaxRated = table.Column<float>(nullable: true),
                    MinRated = table.Column<float>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RepairTestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairTestItem_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RepairTestItem_Sn_StdBasic_RepairItem_RepairIte~",
                        column: x => x.RepairItemId,
                        principalTable: "Sn_StdBasic_RepairItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_SkylightPlanRltInstallationSite",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SkylightPlanId = table.Column<Guid>(nullable: false),
                    InstallationSiteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_SkylightPlanRltInstallationSite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlanRltInstallationSite_Sn_Basic_Installa~",
                        column: x => x.InstallationSiteId,
                        principalTable: "Sn_Basic_InstallationSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlanRltInstallationSite_Sn_CrPlan_Skyligh~",
                        column: x => x.SkylightPlanId,
                        principalTable: "Sn_CrPlan_SkylightPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_MaintenanceWorkRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaintenanceWorkId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    RelateFileId = table.Column<Guid>(nullable: true),
                    SchemeCoverName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_MaintenanceWorkRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWorkRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWorkRltFile_Sn_CrPlan_MaintenanceWork_~",
                        column: x => x.MaintenanceWorkId,
                        principalTable: "Sn_CrPlan_MaintenanceWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWorkRltFile_Sn_File_File_RelateFileId",
                        column: x => x.RelateFileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_MaintenanceWorkRltSkylightPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaintenanceWorkId = table.Column<Guid>(nullable: false),
                    SkylightPlanId = table.Column<Guid>(nullable: false),
                    WorkOrgAndDutyPerson = table.Column<string>(nullable: true),
                    SignOrganization = table.Column<string>(nullable: true),
                    FirstTrial = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_MaintenanceWorkRltSkylightPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWorkRltSkylightPlan_Sn_CrPlan_Maintena~",
                        column: x => x.MaintenanceWorkId,
                        principalTable: "Sn_CrPlan_MaintenanceWork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_MaintenanceWorkRltSkylightPlan_Sn_CrPlan_Skylight~",
                        column: x => x.SkylightPlanId,
                        principalTable: "Sn_CrPlan_SkylightPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Oa_DutyScheduleRltUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DutyScheduleId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_DutyScheduleRltUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_DutyScheduleRltUser_Sn_Oa_DutySchedule_DutyScheduleId",
                        column: x => x.DutyScheduleId,
                        principalTable: "Sn_Oa_DutySchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_DutyScheduleRltUser_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_InstitutionRltAuthority",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InstitutionId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsView = table.Column<bool>(nullable: false),
                    IsEdit = table.Column<bool>(nullable: false),
                    IsDownLoad = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_InstitutionRltAuthority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltAuthority_Sn_Regulation_Institu~",
                        column: x => x.InstitutionId,
                        principalTable: "Sn_Regulation_Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_InstitutionRltEdition",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    InstitutionId = table.Column<Guid>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Classify = table.Column<int>(nullable: false),
                    EffectiveTime = table.Column<DateTime>(nullable: false),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    Abstract = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    Version = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_InstitutionRltEdition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltEdition_Sn_Regulation_Instituti~",
                        column: x => x.InstitutionId,
                        principalTable: "Sn_Regulation_Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_InstitutionRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InstitutionId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_InstitutionRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFile_Sn_Regulation_Institution_~",
                        column: x => x.InstitutionId,
                        principalTable: "Sn_Regulation_Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_InstitutionRltLabel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InstitutionId = table.Column<Guid>(nullable: false),
                    LabelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_InstitutionRltLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltLabel_Sn_Regulation_Institution~",
                        column: x => x.InstitutionId,
                        principalTable: "Sn_Regulation_Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltLabel_Sn_Regulation_Label_Label~",
                        column: x => x.LabelId,
                        principalTable: "Sn_Regulation_Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Report_ReportRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Report_ReportRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Report_ReportRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Report_ReportRltFile_Sn_Report_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Sn_Report_Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Report_ReportRltUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Report_ReportRltUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Report_ReportRltUser_Sn_Report_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Sn_Report_Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Report_ReportRltUser_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: true),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: false),
                    ManufacturerId = table.Column<Guid>(nullable: true),
                    ManufactureDate = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StoreHouseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipment_Sn_StdBasic_ComponentCategory_Co~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipment_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipment_Sn_StdBasic_Manufacturer_Manufac~",
                        column: x => x.ManufacturerId,
                        principalTable: "Sn_StdBasic_Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipment_Sn_StdBasic_ProductCategory_Prod~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipment_Sn_Resource_StoreHouse_StoreHous~",
                        column: x => x.StoreHouseId,
                        principalTable: "Sn_Resource_StoreHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreEquipmentTransfer",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    StoreHouseId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreEquipmentTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTransfer_Sn_Resource_StoreHouse_S~",
                        column: x => x.StoreHouseId,
                        principalTable: "Sn_Resource_StoreHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTransfer_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_SubItemContent",
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
                    SubItemId = table.Column<Guid>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NodeType = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_SubItemContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_SubItemContent_Sn_ConstructionBase_SubI~",
                        column: x => x.ParentId,
                        principalTable: "Sn_ConstructionBase_SubItemContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "ForeignKey_SubItem_SubItemContent",
                        column: x => x.SubItemId,
                        principalTable: "Sn_ConstructionBase_SubItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_SkylightPlanRltWorkTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SkylightPlanId = table.Column<Guid>(nullable: false),
                    WorkTicketId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_SkylightPlanRltWorkTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlanRltWorkTicket_Sn_CrPlan_SkylightPlan_~",
                        column: x => x.SkylightPlanId,
                        principalTable: "Sn_CrPlan_SkylightPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_SkylightPlanRltWorkTicket_Sn_CrPlan_WorkTicket_Wo~",
                        column: x => x.WorkTicketId,
                        principalTable: "Sn_CrPlan_WorkTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_CrPlan_WorkTicketRltCooperationUnit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkTicketId = table.Column<Guid>(nullable: false),
                    CooperateWorkShopId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    MainWorkShopId = table.Column<Guid>(nullable: false),
                    Completion = table.Column<string>(nullable: true),
                    CooperateContent = table.Column<string>(nullable: true),
                    CooperateRealFinishTime = table.Column<DateTime>(nullable: false),
                    CooperateRealStartTime = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_CrPlan_WorkTicketRltCooperationUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_CrPlan_WorkTicketRltCooperationUnit_Sn_CrPlan_WorkTicket~",
                        column: x => x.WorkTicketId,
                        principalTable: "Sn_CrPlan_WorkTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_ContractRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContractId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_ContractRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_ContractRltFile_Sn_Material_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Sn_Material_Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_ContractRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialAcceptanceRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    MaterialAcceptanceId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialAcceptanceRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltFile_Sn_Material_MaterialA~",
                        column: x => x.MaterialAcceptanceId,
                        principalTable: "Sn_Material_MaterialAcceptance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialAcceptanceRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TestState = table.Column<int>(nullable: false),
                    MaterialAcceptanceId = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialAcceptanceRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltMaterial_Sn_Material_Mater~",
                        column: x => x.MaterialAcceptanceId,
                        principalTable: "Sn_Material_MaterialAcceptance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltMaterial_Sn_Technology_Mat~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialAcceptanceRltQRCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaterialAcceptanceId = table.Column<Guid>(nullable: false),
                    QRCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialAcceptanceRltQRCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltQRCode_Sn_Material_Materia~",
                        column: x => x.MaterialAcceptanceId,
                        principalTable: "Sn_Material_MaterialAcceptance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Oa_ContractRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContractId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_ContractRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_ContractRltFile_Sn_Oa_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Sn_Oa_Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_ContractRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_ProjectRltContract",
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
                    ContractId = table.Column<Guid>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_ProjectRltContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltContract_Sn_Oa_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Sn_Oa_Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltContract_Sn_Project_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Sn_Project_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_ProjectRltFile",
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
                    FileId = table.Column<Guid>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_ProjectRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltFile_Sn_Project_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Sn_Project_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_ProjectRltMember",
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
                    ManagerId = table.Column<Guid>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_ProjectRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltMember_Sn_App_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltMember_Sn_Project_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Sn_Project_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_ProjectRltUnit",
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
                    ProjectId = table.Column<Guid>(nullable: true),
                    UnitId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_ProjectRltUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltUnit_Sn_Project_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Sn_Project_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Project_ProjectRltUnit_Sn_Project_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Sn_Project_Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Task_Task",
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
                    ProjectId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Weight = table.Column<double>(nullable: false),
                    Proportion = table.Column<decimal>(nullable: false),
                    Progress = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Task_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Task_Task_Sn_Task_Task_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Task_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Task_Task_Sn_Project_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Sn_Project_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    QualityProblemId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRecord_Sn_Quality_QualityProblem_Q~",
                        column: x => x.QualityProblemId,
                        principalTable: "Sn_Quality_QualityProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRecord_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemRltCcUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QualityProblemId = table.Column<Guid>(nullable: false),
                    CcUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemRltCcUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltCcUser_Sn_App_Users_CcUserId",
                        column: x => x.CcUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltCcUser_Sn_Quality_QualityProble~",
                        column: x => x.QualityProblemId,
                        principalTable: "Sn_Quality_QualityProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QualityProblemId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltFile_Sn_Quality_QualityProblem_~",
                        column: x => x.QualityProblemId,
                        principalTable: "Sn_Quality_QualityProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    SafeProblemId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRecord_Sn_Safe_SafeProblem_SafeProblemId",
                        column: x => x.SafeProblemId,
                        principalTable: "Sn_Safe_SafeProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRecord_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemRltCcUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SafeProblemId = table.Column<Guid>(nullable: false),
                    CcUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemRltCcUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltCcUser_Sn_App_Users_CcUserId",
                        column: x => x.CcUserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltCcUser_Sn_Safe_SafeProblem_SafeProble~",
                        column: x => x.SafeProblemId,
                        principalTable: "Sn_Safe_SafeProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SafeProblemId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltFile_Sn_Safe_SafeProblem_SafeProblemId",
                        column: x => x.SafeProblemId,
                        principalTable: "Sn_Safe_SafeProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_FaultRltComponentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FaultId = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_FaultRltComponentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_FaultRltComponentCategory_Sn_StdBasic_ComponentCat~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_FaultRltComponentCategory_Sn_Emerg_Fault_FaultId",
                        column: x => x.FaultId,
                        principalTable: "Sn_Emerg_Fault",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_StdBasic_ComponentCategoryRltQuota",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentCategoryId = table.Column<Guid>(nullable: false),
                    QuotaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ComponentCategoryRltQuota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltQuota_Sn_StdBasic_Component~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ComponentCategoryRltQuota_Sn_StdBasic_Quota_Quo~",
                        column: x => x.QuotaId,
                        principalTable: "Sn_StdBasic_Quota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_ProductCategoryRltQuota",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductionCategoryId = table.Column<Guid>(nullable: false),
                    QuotaId = table.Column<Guid>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_ProductCategoryRltQuota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltQuota_Sn_StdBasic_ProductCate~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_ProductCategoryRltQuota_Sn_StdBasic_Quota_Quota~",
                        column: x => x.QuotaId,
                        principalTable: "Sn_StdBasic_Quota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_QuotaItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuotaId = table.Column<Guid>(nullable: false),
                    BasePriceId = table.Column<Guid>(nullable: false),
                    Number = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_QuotaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaItem_Sn_StdBasic_BasePrice_BasePriceId",
                        column: x => x.BasePriceId,
                        principalTable: "Sn_StdBasic_BasePrice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_QuotaItem_Sn_StdBasic_Quota_QuotaId",
                        column: x => x.QuotaId,
                        principalTable: "Sn_StdBasic_Quota",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_Material_EntryRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    PartitionId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_EntryRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecord_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecord_Sn_Material_Partition_PartitionId",
                        column: x => x.PartitionId,
                        principalTable: "Sn_Material_Partition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    MaterialId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    BatchNumber = table.Column<string>(nullable: true),
                    PartitionId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    SupplierId = table.Column<Guid>(nullable: true),
                    EntryTime = table.Column<DateTime>(nullable: false),
                    ProductionDate = table.Column<DateTime>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Inventory_Sn_Technology_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Inventory_Sn_Material_Partition_PartitionId",
                        column: x => x.PartitionId,
                        principalTable: "Sn_Material_Partition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_Inventory_Sn_Material_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Sn_Material_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_OutRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    PartitionId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_OutRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecord_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecord_Sn_Material_Partition_PartitionId",
                        column: x => x.PartitionId,
                        principalTable: "Sn_Material_Partition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Oa_SealRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SealId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false),
                    MemberType = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Oa_SealRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Oa_SealRltMember_Sn_Oa_Seal_SealId",
                        column: x => x.SealId,
                        principalTable: "Sn_Oa_Seal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Project_DossierRltFile",
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
                    FileId = table.Column<Guid>(nullable: true),
                    DossierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Project_DossierRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Project_DossierRltFile_Sn_Project_Dossier_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Sn_Project_Dossier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Project_DossierRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_StdBasic_RevitConnector",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModelFileId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_StdBasic_RevitConnector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_StdBasic_RevitConnector_Sn_StdBasic_ModelFile_ModelFileId",
                        column: x => x.ModelFileId,
                        principalTable: "Sn_StdBasic_ModelFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_Equipment",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    CSRGCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StandardName = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    ComponentCategoryId = table.Column<Guid>(nullable: true),
                    InstallationSiteId = table.Column<Guid>(nullable: true),
                    EndInstallationSiteId = table.Column<Guid>(nullable: true),
                    UseDate = table.Column<DateTime>(nullable: false),
                    ProductCategoryId = table.Column<Guid>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true),
                    ManufacturerId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    StoreEquipmentId = table.Column<Guid>(nullable: true),
                    CableExtendId = table.Column<Guid>(nullable: true),
                    GroupId = table.Column<Guid>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: true),
                    GisData = table.Column<string>(nullable: true),
                    CheckState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Resource_CableExtend_CableExtendId",
                        column: x => x.CableExtendId,
                        principalTable: "Sn_Resource_CableExtend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_StdBasic_ComponentCategory_Compone~",
                        column: x => x.ComponentCategoryId,
                        principalTable: "Sn_StdBasic_ComponentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Basic_InstallationSite_EndInstalla~",
                        column: x => x.EndInstallationSiteId,
                        principalTable: "Sn_Basic_InstallationSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Resource_EquipmentGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Sn_Resource_EquipmentGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Basic_InstallationSite_Installatio~",
                        column: x => x.InstallationSiteId,
                        principalTable: "Sn_Basic_InstallationSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_StdBasic_Manufacturer_Manufacturer~",
                        column: x => x.ManufacturerId,
                        principalTable: "Sn_StdBasic_Manufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_App_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Resource_Equipment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_StdBasic_ProductCategory_ProductCa~",
                        column: x => x.ProductCategoryId,
                        principalTable: "Sn_StdBasic_ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Equipment_Sn_Resource_StoreEquipment_StoreEquip~",
                        column: x => x.StoreEquipmentId,
                        principalTable: "Sn_Resource_StoreEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreEquipmentTestRltEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StoreEquipmentId = table.Column<Guid>(nullable: false),
                    StoreEquipmentTestId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreEquipmentTestRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTestRltEquipment_Sn_Resource_Stor~",
                        column: x => x.StoreEquipmentId,
                        principalTable: "Sn_Resource_StoreEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTestRltEquipment_Sn_Resource_Sto~1",
                        column: x => x.StoreEquipmentTestId,
                        principalTable: "Sn_Resource_StoreEquipmentTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_StoreEquipmentTransferRltEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StoreEquipmentId = table.Column<Guid>(nullable: false),
                    StoreEquipmentTransferId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_StoreEquipmentTransferRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTransferRltEquipment_Sn_Resource_~",
                        column: x => x.StoreEquipmentId,
                        principalTable: "Sn_Resource_StoreEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_StoreEquipmentTransferRltEquipment_Sn_Resource~1",
                        column: x => x.StoreEquipmentTransferId,
                        principalTable: "Sn_Resource_StoreEquipmentTransfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_SubItemContentRltProcedure",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProcedureId = table.Column<Guid>(nullable: false),
                    SubItemContentId = table.Column<Guid>(nullable: false),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_SubItemContentRltProcedure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_SubItemContentRltProcedure_Sn_Construct~",
                        column: x => x.ProcedureId,
                        principalTable: "Sn_ConstructionBase_Procedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_SubItemContentRltProcedure_Sn_Construc~1",
                        column: x => x.SubItemContentId,
                        principalTable: "Sn_ConstructionBase_SubItemContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Task_TaskRltFile",
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
                    FileId = table.Column<Guid>(nullable: true),
                    TaskId = table.Column<Guid>(nullable: true),
                    FileType = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Task_TaskRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Task_TaskRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Task_TaskRltFile_Sn_Task_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Sn_Task_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Task_TaskRltMember",
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
                    TaskId = table.Column<Guid>(nullable: true),
                    MemberId = table.Column<Guid>(nullable: true),
                    Responsible = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Task_TaskRltMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Task_TaskRltMember_Sn_App_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Task_TaskRltMember_Sn_Task_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Sn_Task_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemRecordRleFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QualityProblemRecordId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemRecordRleFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRecordRleFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRecordRleFile_Sn_Quality_QualityPr~",
                        column: x => x.QualityProblemRecordId,
                        principalTable: "Sn_Quality_QualityProblemRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemRecordRleFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SafeProblemRecordId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemRecordRleFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRecordRleFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRecordRleFile_Sn_Safe_SafeProblemRecord_~",
                        column: x => x.SafeProblemRecordId,
                        principalTable: "Sn_Safe_SafeProblemRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplateNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    FlowTemplateId = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Size = table.Column<List<float>>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    X = table.Column<float>(nullable: false),
                    Y = table.Column<float>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_Material_EntryRecordRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntryRecordId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_EntryRecordRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltFile_Sn_Material_EntryRecord_Entr~",
                        column: x => x.EntryRecordId,
                        principalTable: "Sn_Material_EntryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_EntryRecordRltQRCode",
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
                    EntryRecordId = table.Column<Guid>(nullable: false),
                    QRCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_EntryRecordRltQRCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltQRCode_Sn_Material_EntryRecord_En~",
                        column: x => x.EntryRecordId,
                        principalTable: "Sn_Material_EntryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_EntryRecordRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InventoryId = table.Column<Guid>(nullable: false),
                    EntryRecordId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    Count = table.Column<decimal>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true),
                    SupplierId = table.Column<Guid>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_EntryRecordRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltMaterial_Sn_Material_EntryRecord_~",
                        column: x => x.EntryRecordId,
                        principalTable: "Sn_Material_EntryRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltMaterial_Sn_Material_Inventory_In~",
                        column: x => x.InventoryId,
                        principalTable: "Sn_Material_Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltMaterial_Sn_Technology_Material_M~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_EntryRecordRltMaterial_Sn_Material_Supplier_Sup~",
                        column: x => x.SupplierId,
                        principalTable: "Sn_Material_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialOfBillRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    MaterialOfBillId = table.Column<Guid>(nullable: false),
                    InventoryId = table.Column<Guid>(nullable: true),
                    count = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialOfBillRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialOfBillRltMaterial_Sn_Material_Inventory~",
                        column: x => x.InventoryId,
                        principalTable: "Sn_Material_Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialOfBillRltMaterial_Sn_Material_MaterialO~",
                        column: x => x.MaterialOfBillId,
                        principalTable: "Sn_Material_MaterialOfBill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_OutRecordRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OutRecordId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_OutRecordRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltFile_Sn_Material_OutRecord_OutRecor~",
                        column: x => x.OutRecordId,
                        principalTable: "Sn_Material_OutRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_OutRecordRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OutRecordId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    InventoryId = table.Column<Guid>(nullable: false),
                    Count = table.Column<decimal>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    SupplierId = table.Column<Guid>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_OutRecordRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltMaterial_Sn_Material_Inventory_Inve~",
                        column: x => x.InventoryId,
                        principalTable: "Sn_Material_Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltMaterial_Sn_Technology_Material_Mat~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltMaterial_Sn_Material_OutRecord_OutR~",
                        column: x => x.OutRecordId,
                        principalTable: "Sn_Material_OutRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltMaterial_Sn_Material_Supplier_Suppl~",
                        column: x => x.SupplierId,
                        principalTable: "Sn_Material_Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_OutRecordRltQRCode",
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
                    OutRecordId = table.Column<Guid>(nullable: false),
                    QRCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_OutRecordRltQRCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_OutRecordRltQRCode_Sn_Material_OutRecord_OutRec~",
                        column: x => x.OutRecordId,
                        principalTable: "Sn_Material_OutRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Alarm_Alarm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ActivedTime = table.Column<DateTime>(nullable: false),
                    ConfirmedTime = table.Column<DateTime>(nullable: false),
                    ClearedTime = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Alarm_Alarm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Alarm_Alarm_Sn_Resource_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Emerg_FaultRltEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FaultId = table.Column<Guid>(nullable: false),
                    EquipmentId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Emerg_FaultRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_FaultRltEquipment_Sn_Resource_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Emerg_FaultRltEquipment_Sn_Emerg_Fault_FaultId",
                        column: x => x.FaultId,
                        principalTable: "Sn_Emerg_Fault",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Quality_QualityProblemRltEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QualityProblemId = table.Column<Guid>(nullable: false),
                    EquipmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Quality_QualityProblemRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltEquipment_Sn_Resource_Equipment~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Quality_QualityProblemRltEquipment_Sn_Quality_QualityPro~",
                        column: x => x.QualityProblemId,
                        principalTable: "Sn_Quality_QualityProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_CableCore",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    CableId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Rremark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_CableCore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_CableCore_Sn_Resource_Equipment_CableId",
                        column: x => x.CableId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_CableLocation",
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
                    CableId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Direction = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Positions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_CableLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_CableLocation_Sn_Resource_Equipment_CableId",
                        column: x => x.CableId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_ComponentRltQRCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GenerateEquipmentId = table.Column<Guid>(nullable: true),
                    InstallationEquipmentId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    QRCode = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_ComponentRltQRCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_ComponentRltQRCode_Sn_Resource_Equipment_Genera~",
                        column: x => x.GenerateEquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_ComponentRltQRCode_Sn_Resource_Equipment_Instal~",
                        column: x => x.InstallationEquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_EquipmentProperty",
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
                    EquipmentId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    MVDCategoryId = table.Column<Guid>(nullable: true),
                    MVDPropertyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_EquipmentProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentProperty_Sn_Resource_Equipment_Equipme~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentProperty_Sn_StdBasic_MVDCategory_MVDCa~",
                        column: x => x.MVDCategoryId,
                        principalTable: "Sn_StdBasic_MVDCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentProperty_Sn_StdBasic_MVDProperty_MVDPr~",
                        column: x => x.MVDPropertyId,
                        principalTable: "Sn_StdBasic_MVDProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_EquipmentRltFile",
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
                    EquipmentId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_EquipmentRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentRltFile_Sn_Resource_Equipment_Equipmen~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_EquipmentRltOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EquipmentId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_EquipmentRltOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentRltOrganization_Sn_Resource_Equipment_~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentRltOrganization_Sn_App_Organization_Or~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_EquipmentServiceRecord",
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
                    EquipmentId = table.Column<Guid>(nullable: false),
                    StoreEquipmentId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_EquipmentServiceRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentServiceRecord_Sn_Resource_Equipment_Eq~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentServiceRecord_Sn_Resource_StoreEquipme~",
                        column: x => x.StoreEquipmentId,
                        principalTable: "Sn_Resource_StoreEquipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_EquipmentServiceRecord_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_Terminal",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    EquipmentId = table.Column<Guid>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_Terminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_Terminal_Sn_Resource_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Safe_SafeProblemRltEquipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SafeProblemId = table.Column<Guid>(nullable: false),
                    EquipmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Safe_SafeProblemRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltEquipment_Sn_Resource_Equipment_Equip~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Safe_SafeProblemRltEquipment_Sn_Safe_SafeProblem_SafePro~",
                        column: x => x.SafeProblemId,
                        principalTable: "Sn_Safe_SafeProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_ConstructInterface",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ProfessionId = table.Column<Guid>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    MaterialSpec = table.Column<string>(nullable: true),
                    MarerialName = table.Column<string>(nullable: true),
                    MarerialCount = table.Column<string>(nullable: true),
                    GisData = table.Column<string>(nullable: true),
                    BuilderId = table.Column<Guid>(nullable: true),
                    MarkType = table.Column<int>(nullable: false),
                    InterfaceManagementTypeId = table.Column<Guid>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_ConstructInterface", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterface_Sn_App_DataDictionary_Buil~",
                        column: x => x.BuilderId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterface_Sn_Resource_Equipment_Equi~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterface_Sn_App_DataDictionary_Inte~",
                        column: x => x.InterfaceManagementTypeId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterface_Sn_App_DataDictionary_Prof~",
                        column: x => x.ProfessionId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_RltProcedureRltEquipmentTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RltProcedureId = table.Column<Guid>(nullable: false),
                    EquipmentTeamId = table.Column<Guid>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    SubItemContentRltProcedureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_RltProcedureRltEquipmentTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltEquipmentTeam_Sn_Constru~",
                        column: x => x.EquipmentTeamId,
                        principalTable: "Sn_ConstructionBase_EquipmentTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltEquipmentTeam_Sn_Constr~1",
                        column: x => x.SubItemContentRltProcedureId,
                        principalTable: "Sn_ConstructionBase_SubItemContentRltProcedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_RltProcedureRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RltProcedureId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    SubItemContentRltProcedureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_RltProcedureRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltFile_Sn_ConstructionBase~",
                        column: x => x.SubItemContentRltProcedureId,
                        principalTable: "Sn_ConstructionBase_SubItemContentRltProcedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_RltProcedureRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RltProcedureId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    ConstructionBaseMaterialId = table.Column<Guid>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    SubItemContentRltProcedureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_RltProcedureRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltMaterial_Sn_Construction~",
                        column: x => x.ConstructionBaseMaterialId,
                        principalTable: "Sn_ConstructionBase_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltMaterial_Sn_Constructio~1",
                        column: x => x.SubItemContentRltProcedureId,
                        principalTable: "Sn_ConstructionBase_SubItemContentRltProcedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_ConstructionBase_RltProcedureRltWorker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RltProcedureId = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    SubItemContentRltProcedureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_ConstructionBase_RltProcedureRltWorker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltWorker_Sn_ConstructionBa~",
                        column: x => x.SubItemContentRltProcedureId,
                        principalTable: "Sn_ConstructionBase_SubItemContentRltProcedure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_ConstructionBase_RltProcedureRltWorker_Sn_ConstructionB~1",
                        column: x => x.WorkerId,
                        principalTable: "Sn_ConstructionBase_Worker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Bpm_FlowTemplateNodeRltMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
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
                name: "Sn_Construction_Dispatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DispatchTemplateId = table.Column<Guid>(nullable: true),
                    Profession = table.Column<string>(nullable: true),
                    ContractorId = table.Column<Guid>(nullable: false),
                    Team = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    ExtraDescription = table.Column<string>(nullable: true),
                    IsNeedLargeEquipment = table.Column<bool>(nullable: false),
                    LargeEquipment = table.Column<string>(nullable: true),
                    IsDismantle = table.Column<bool>(nullable: false),
                    IsHighWork = table.Column<bool>(nullable: false),
                    Process = table.Column<string>(nullable: true),
                    RiskSources = table.Column<string>(nullable: true),
                    RecoveryTime = table.Column<DateTime>(nullable: true),
                    SafetyMeasure = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_Dispatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Dispatch_Sn_App_DataDictionary_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Dispatch_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Dispatch_Sn_Construction_DispatchTemplate_D~",
                        column: x => x.DispatchTemplateId,
                        principalTable: "Sn_Construction_DispatchTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Dispatch_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Dispatch_Sn_Bpm_WorkflowTemplate_WorkflowTe~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_MasterPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PlanStartTime = table.Column<DateTime>(nullable: false),
                    PlanEndTime = table.Column<DateTime>(nullable: false),
                    Period = table.Column<double>(nullable: false),
                    ChargerId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_MasterPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlan_Sn_App_Users_ChargerId",
                        column: x => x.ChargerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlan_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlan_Sn_Bpm_WorkflowTemplate_Workflow~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_FileApprove_FileApprove",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    FileId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_FileApprove_FileApprove", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_FileApprove_FileApprove_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_FileApprove_FileApprove_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_FileApprove_FileApprove_Sn_Bpm_WorkflowTemplate_Workflow~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchaseList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Submit = table.Column<bool>(nullable: false),
                    PlanTime = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchaseList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseList_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseList_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseList_Sn_Bpm_WorkflowTemplate_WorkflowTe~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchasePlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PlanTime = table.Column<DateTime>(nullable: true),
                    Submit = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchasePlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlan_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlan_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlan_Sn_Bpm_WorkflowTemplate_WorkflowTe~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "Sn_Regulation_InstitutionRltFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    InstitutionId = table.Column<Guid>(nullable: false),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    ApproveTime = table.Column<DateTime>(nullable: false),
                    ApproveState = table.Column<string>(nullable: true),
                    Suggestion = table.Column<string>(nullable: true),
                    SealId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Regulation_InstitutionRltFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFlow_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFlow_Sn_Regulation_Institution_~",
                        column: x => x.InstitutionId,
                        principalTable: "Sn_Regulation_Institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFlow_Sn_Oa_Seal_SealId",
                        column: x => x.SealId,
                        principalTable: "Sn_Oa_Seal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Regulation_InstitutionRltFlow_Sn_Bpm_Workflow_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_MaterialPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    PlanName = table.Column<string>(nullable: true),
                    PlanTime = table.Column<DateTime>(nullable: false),
                    Submit = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_MaterialPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlan_Sn_App_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlan_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlan_Sn_Bpm_WorkflowTemplate_Workflow~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_ComponentTrackRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComponentRltQRCodeId = table.Column<Guid>(nullable: false),
                    NodeType = table.Column<int>(nullable: false),
                    TrackingId = table.Column<Guid>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_ComponentTrackRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_ComponentTrackRecord_Sn_Resource_ComponentRltQR~",
                        column: x => x.ComponentRltQRCodeId,
                        principalTable: "Sn_Resource_ComponentRltQRCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_ComponentTrackRecord_Sn_App_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_TerminalBusinessPathNode",
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
                    TerminalBusinessPathId = table.Column<Guid>(nullable: false),
                    TerminalId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    CableCoreId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_TerminalBusinessPathNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalBusinessPathNode_Sn_Resource_CableCore_~",
                        column: x => x.CableCoreId,
                        principalTable: "Sn_Resource_CableCore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalBusinessPathNode_Sn_Resource_TerminalBu~",
                        column: x => x.TerminalBusinessPathId,
                        principalTable: "Sn_Resource_TerminalBusinessPath",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalBusinessPathNode_Sn_Resource_Terminal_T~",
                        column: x => x.TerminalId,
                        principalTable: "Sn_Resource_Terminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Resource_TerminalLink",
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
                    TerminalAId = table.Column<Guid>(nullable: false),
                    TerminalBId = table.Column<Guid>(nullable: false),
                    CableCoreId = table.Column<Guid>(nullable: true),
                    BusinessFunction = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_TerminalLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalLink_Sn_Resource_CableCore_CableCoreId",
                        column: x => x.CableCoreId,
                        principalTable: "Sn_Resource_CableCore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalLink_Sn_Resource_Terminal_TerminalAId",
                        column: x => x.TerminalAId,
                        principalTable: "Sn_Resource_Terminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_TerminalLink_Sn_Resource_Terminal_TerminalBId",
                        column: x => x.TerminalBId,
                        principalTable: "Sn_Resource_Terminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_ConstructInterfaceInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ConstructInterfaceId = table.Column<Guid>(nullable: true),
                    MarkerId = table.Column<Guid>(nullable: false),
                    BuilderId = table.Column<Guid>(nullable: false),
                    MarkType = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    MarkDate = table.Column<DateTime>(nullable: true),
                    ReformerId = table.Column<Guid>(nullable: true),
                    ReformDate = table.Column<DateTime>(nullable: true),
                    ReformExplain = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_ConstructInterfaceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfo_Sn_App_DataDictionary_~",
                        column: x => x.BuilderId,
                        principalTable: "Sn_App_DataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfo_Sn_Technology_Construc~",
                        column: x => x.ConstructInterfaceId,
                        principalTable: "Sn_Technology_ConstructInterface",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfo_Sn_App_Users_MarkerId",
                        column: x => x.MarkerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfo_Sn_App_Users_ReformerId",
                        column: x => x.ReformerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_Daily",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DispatchId = table.Column<Guid>(nullable: true),
                    DailyTemplateId = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    InformantId = table.Column<Guid>(nullable: true),
                    Weathers = table.Column<string>(nullable: true),
                    Temperature = table.Column<string>(nullable: true),
                    WindDirection = table.Column<string>(nullable: true),
                    AirQuality = table.Column<string>(nullable: true),
                    Team = table.Column<string>(nullable: true),
                    BuilderCount = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_Daily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Daily_Sn_Construction_DailyTemplate_DailyTe~",
                        column: x => x.DailyTemplateId,
                        principalTable: "Sn_Construction_DailyTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Daily_Sn_Construction_Dispatch_DispatchId",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Daily_Sn_App_Users_InformantId",
                        column: x => x.InformantId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Daily_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Daily_Sn_Bpm_WorkflowTemplate_WorkflowTempl~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltFile_Sn_Construction_Dispatch_Di~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    Count = table.Column<decimal>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltMaterial_Sn_Construction_Dispatc~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltMaterial_Sn_Technology_Material_~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    SectionId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltSection_Sn_Construction_Dispatch~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltSection_Sn_ConstructionBase_Sect~",
                        column: x => x.SectionId,
                        principalTable: "Sn_ConstructionBase_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltStandard",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    StandardId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltStandard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltStandard_Sn_Construction_Dispatc~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltStandard_Sn_ConstructionBase_Sta~",
                        column: x => x.StandardId,
                        principalTable: "Sn_ConstructionBase_Standard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltWorker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    WorkerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltWorker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltWorker_Sn_Construction_Dispatch_~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltWorker_Sn_App_Users_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltWorkFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DispatchId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltWorkFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltWorkFlow_Sn_Construction_Dispatc~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_MasterPlanContent",
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
                    SubItemContentId = table.Column<Guid>(nullable: true),
                    MasterPlanId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PlanStartTime = table.Column<DateTime>(nullable: false),
                    PlanEndTime = table.Column<DateTime>(nullable: false),
                    Period = table.Column<double>(nullable: false),
                    IsMilestone = table.Column<bool>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_MasterPlanContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterPlan_MasterPlanContent",
                        column: x => x.MasterPlanId,
                        principalTable: "Sn_Construction_MasterPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlanContent_Sn_Construction_MasterPla~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Construction_MasterPlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlanContent_Sn_ConstructionBase_SubIt~",
                        column: x => x.SubItemContentId,
                        principalTable: "Sn_ConstructionBase_SubItemContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_MasterPlanRltWorkflowInfo",
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
                    MasterPlanId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    WorkflowState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_MasterPlanRltWorkflowInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlanRltWorkflowInfo_Sn_Construction_M~",
                        column: x => x.MasterPlanId,
                        principalTable: "Sn_Construction_MasterPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlanRltWorkflowInfo_Sn_Bpm_Workflow_W~",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_Plan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkflowId = table.Column<Guid>(nullable: true),
                    WorkflowTemplateId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    MasterPlanId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PlanStartTime = table.Column<DateTime>(nullable: false),
                    PlanEndTime = table.Column<DateTime>(nullable: false),
                    Period = table.Column<double>(nullable: false),
                    ChargerId = table.Column<Guid>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_Plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Plan_Sn_App_Users_ChargerId",
                        column: x => x.ChargerId,
                        principalTable: "Sn_App_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Plan_Sn_Construction_MasterPlan_MasterPlanId",
                        column: x => x.MasterPlanId,
                        principalTable: "Sn_Construction_MasterPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Plan_Sn_Bpm_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Sn_Bpm_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_Plan_Sn_Bpm_WorkflowTemplate_WorkflowTempla~",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "Sn_Bpm_WorkflowTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_FileApprove_FileApproveRltFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    FileApproveId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_FileApprove_FileApproveRltFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_FileApprove_FileApproveRltFlow_Sn_FileApprove_FileApprov~",
                        column: x => x.FileApproveId,
                        principalTable: "Sn_FileApprove_FileApprove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_MaterialAcceptanceRltPurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaterialAcceptanceId = table.Column<Guid>(nullable: false),
                    PurchaseListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_MaterialAcceptanceRltPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltPurchase_Sn_Material_Mater~",
                        column: x => x.MaterialAcceptanceId,
                        principalTable: "Sn_Material_MaterialAcceptance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_MaterialAcceptanceRltPurchase_Sn_Material_Purch~",
                        column: x => x.PurchaseListId,
                        principalTable: "Sn_Material_PurchaseList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchaseListRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    PurchaseListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchaseListRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltFile_Sn_Material_PurchaseList_Pu~",
                        column: x => x.PurchaseListId,
                        principalTable: "Sn_Material_PurchaseList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchaseListRltFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    PurchaseListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchaseListRltFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltFlow_Sn_Material_PurchaseList_Pu~",
                        column: x => x.PurchaseListId,
                        principalTable: "Sn_Material_PurchaseList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchaseListRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    PurchaseListId = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchaseListRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltMaterial_Sn_Technology_Material_~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltMaterial_Sn_Material_PurchaseLis~",
                        column: x => x.PurchaseListId,
                        principalTable: "Sn_Material_PurchaseList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchaseListRltPurchasePlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PurchasePlanId = table.Column<Guid>(nullable: false),
                    PurchaseListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchaseListRltPurchasePlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltPurchasePlan_Sn_Material_Purchas~",
                        column: x => x.PurchaseListId,
                        principalTable: "Sn_Material_PurchaseList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchaseListRltPurchasePlan_Sn_Material_Purcha~1",
                        column: x => x.PurchasePlanId,
                        principalTable: "Sn_Material_PurchasePlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchasePlanRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false),
                    PurchasePlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchasePlanRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlanRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlanRltFile_Sn_Material_PurchasePlan_Pu~",
                        column: x => x.PurchasePlanId,
                        principalTable: "Sn_Material_PurchasePlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchasePlanRltFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    PurchasePlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchasePlanRltFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlanRltFlow_Sn_Material_PurchasePlan_Pu~",
                        column: x => x.PurchasePlanId,
                        principalTable: "Sn_Material_PurchasePlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Material_PurchasePlanRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    PurchasePlanId = table.Column<Guid>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Material_PurchasePlanRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlanRltMaterial_Sn_Technology_Material_~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Material_PurchasePlanRltMaterial_Sn_Material_PurchasePla~",
                        column: x => x.PurchasePlanId,
                        principalTable: "Sn_Material_PurchasePlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_MaterialPlanFlowInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    MaterialPlanId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_MaterialPlanFlowInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlanFlowInfo_Sn_Technology_MaterialPl~",
                        column: x => x.MaterialPlanId,
                        principalTable: "Sn_Technology_MaterialPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_MaterialPlanRltMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaterialPlanId = table.Column<Guid>(nullable: false),
                    MaterialId = table.Column<Guid>(nullable: false),
                    Count = table.Column<decimal>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_MaterialPlanRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlanRltMaterial_Sn_Technology_Materia~",
                        column: x => x.MaterialId,
                        principalTable: "Sn_Technology_Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_MaterialPlanRltMaterial_Sn_Technology_Materi~1",
                        column: x => x.MaterialPlanId,
                        principalTable: "Sn_Technology_MaterialPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Technology_ConstructInterfaceInfoRltMarkFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConstructInterfaceInfoId = table.Column<Guid>(nullable: false),
                    MarkFileId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Technology_ConstructInterfaceInfoRltMarkFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfoRltMarkFile_Sn_Technolo~",
                        column: x => x.ConstructInterfaceInfoId,
                        principalTable: "Sn_Technology_ConstructInterfaceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Technology_ConstructInterfaceInfoRltMarkFile_Sn_File_Fil~",
                        column: x => x.MarkFileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyFlowInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DailyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyFlowInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyFlowInfo_Sn_Construction_Daily_DailyId",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyRltFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    FileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltFile_Sn_Construction_Daily_DailyId",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyRltQuality",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    QualityProblemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyRltQuality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltQuality_Sn_Construction_Daily_Daily~",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltQuality_Sn_Quality_QualityProblem_Q~",
                        column: x => x.QualityProblemId,
                        principalTable: "Sn_Quality_QualityProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyRltSafe",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    SafeProblemId = table.Column<Guid>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyRltSafe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltSafe_Sn_Construction_Daily_DailyId",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltSafe_Sn_Safe_SafeProblem_SafeProble~",
                        column: x => x.SafeProblemId,
                        principalTable: "Sn_Safe_SafeProblem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_UnplannedTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    TaskType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_UnplannedTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_UnplannedTask_Sn_Construction_Daily_DailyId",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_MasterPlanRltContentRltAntecedent",
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
                    FrontMasterPlanContentId = table.Column<Guid>(nullable: true),
                    MasterPlanContentId = table.Column<Guid>(nullable: true),
                    MasterPlanContentId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_MasterPlanRltContentRltAntecedent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rlt",
                        column: x => x.FrontMasterPlanContentId,
                        principalTable: "Sn_Construction_MasterPlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Main",
                        column: x => x.MasterPlanContentId,
                        principalTable: "Sn_Construction_MasterPlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_MasterPlanRltContentRltAntecedent_Sn_Constr~",
                        column: x => x.MasterPlanContentId1,
                        principalTable: "Sn_Construction_MasterPlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanContent",
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
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    SubItemContentId = table.Column<Guid>(nullable: true),
                    PlanId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PlanStartTime = table.Column<DateTime>(nullable: false),
                    PlanEndTime = table.Column<DateTime>(nullable: false),
                    Period = table.Column<double>(nullable: false),
                    IsMilestone = table.Column<bool>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    AllProgress = table.Column<double>(nullable: false),
                    WorkDay = table.Column<decimal>(nullable: false),
                    WorkerNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContent_Sn_Construction_PlanContent_Par~",
                        column: x => x.ParentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plan_PlanContent",
                        column: x => x.PlanId,
                        principalTable: "Sn_Construction_Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContent_Sn_ConstructionBase_SubItemCont~",
                        column: x => x.SubItemContentId,
                        principalTable: "Sn_ConstructionBase_SubItemContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanRltWorkflowInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    PlanId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanRltWorkflowInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanRltWorkflowInfo_Sn_Construction_Plan_Pl~",
                        column: x => x.PlanId,
                        principalTable: "Sn_Construction_Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DispatchRltPlanContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DispatchId = table.Column<Guid>(nullable: false),
                    PlanContentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DispatchRltPlanContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltPlanContent_Sn_Construction_Disp~",
                        column: x => x.DispatchId,
                        principalTable: "Sn_Construction_Dispatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DispatchRltPlanContent_Sn_Construction_Plan~",
                        column: x => x.PlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanContentRltAntecedent",
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
                    FrontPlanContentId = table.Column<Guid>(nullable: true),
                    PlanContentId = table.Column<Guid>(nullable: true),
                    PlanContentId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanContentRltAntecedent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rlt",
                        column: x => x.FrontPlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Main",
                        column: x => x.PlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContentRltAntecedent_Sn_Construction_Pl~",
                        column: x => x.PlanContentId1,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanContentRltFile",
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
                    PlanContentId = table.Column<Guid>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanContentRltFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContentRltFile_Sn_File_File_FileId",
                        column: x => x.FileId,
                        principalTable: "Sn_File_File",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContentRltFile_Sn_Construction_PlanCont~",
                        column: x => x.PlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanContentRltMaterial",
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
                    PlanContentId = table.Column<Guid>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanContentRltMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanContentRltMaterial_Sn_Construction_Plan~",
                        column: x => x.PlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanMaterial",
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
                    PlanContentId = table.Column<Guid>(nullable: true),
                    ComponentCategoryName = table.Column<string>(nullable: true),
                    Spec = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    WorkDay = table.Column<decimal>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanMaterial_Sn_Construction_PlanContent_Pl~",
                        column: x => x.PlanContentId,
                        principalTable: "Sn_Construction_PlanContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_DailyRltPlanMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DailyId = table.Column<Guid>(nullable: false),
                    PlanMaterialId = table.Column<Guid>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    ProjectTagId = table.Column<Guid>(nullable: true),
                    OrganizationRootTagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_DailyRltPlanMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltPlanMaterial_Sn_Construction_Daily_~",
                        column: x => x.DailyId,
                        principalTable: "Sn_Construction_Daily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_DailyRltPlanMaterial_Sn_Construction_PlanMa~",
                        column: x => x.PlanMaterialId,
                        principalTable: "Sn_Construction_PlanMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sn_Construction_PlanMaterialRltEquipment",
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
                    EquipmentId = table.Column<Guid>(nullable: false),
                    PlanMaterialId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Construction_PlanMaterialRltEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanMaterialRltEquipment_Sn_Resource_Equipm~",
                        column: x => x.EquipmentId,
                        principalTable: "Sn_Resource_Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sn_Construction_PlanMaterialRltEquipment_Sn_Construction_Pl~",
                        column: x => x.PlanMaterialId,
                        principalTable: "Sn_Construction_PlanMaterial",
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
                name: "IX_Sn_Alarm_Alarm_EquipmentId",
                table: "Sn_Alarm_Alarm",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Alarm_AlarmConfig_ComponentCategoryId",
                table: "Sn_Alarm_AlarmConfig",
                column: "ComponentCategoryId");

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
                name: "IX_Sn_Basic_InstallationSite_CategoryId",
                table: "Sn_Basic_InstallationSite",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_CodeName",
                table: "Sn_Basic_InstallationSite",
                column: "CodeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_OrganizationId",
                table: "Sn_Basic_InstallationSite",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_ParentId",
                table: "Sn_Basic_InstallationSite",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_RailwayId",
                table: "Sn_Basic_InstallationSite",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_StationId",
                table: "Sn_Basic_InstallationSite",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_InstallationSite_TypeId",
                table: "Sn_Basic_InstallationSite",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_RailwayRltOrganization_OrganizationId",
                table: "Sn_Basic_RailwayRltOrganization",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_RailwayRltOrganization_RailwayId",
                table: "Sn_Basic_RailwayRltOrganization",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_Station_SectionEndStationId",
                table: "Sn_Basic_Station",
                column: "SectionEndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_Station_SectionStartStationId",
                table: "Sn_Basic_Station",
                column: "SectionStartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_StationRltOrganization_OrganizationId",
                table: "Sn_Basic_StationRltOrganization",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_StationRltOrganization_StationId",
                table: "Sn_Basic_StationRltOrganization",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_StationRltRailway_RailwayId",
                table: "Sn_Basic_StationRltRailway",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Basic_StationRltRailway_StationId",
                table: "Sn_Basic_StationRltRailway",
                column: "StationId");

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
                name: "IX_Sn_Construction_Daily_DailyTemplateId",
                table: "Sn_Construction_Daily",
                column: "DailyTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Daily_DispatchId",
                table: "Sn_Construction_Daily",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Daily_InformantId",
                table: "Sn_Construction_Daily",
                column: "InformantId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Daily_WorkflowId",
                table: "Sn_Construction_Daily",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Daily_WorkflowTemplateId",
                table: "Sn_Construction_Daily",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyFlowInfo_DailyId",
                table: "Sn_Construction_DailyFlowInfo",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltFile_DailyId",
                table: "Sn_Construction_DailyRltFile",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltFile_FileId",
                table: "Sn_Construction_DailyRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltPlanMaterial_DailyId",
                table: "Sn_Construction_DailyRltPlanMaterial",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltPlanMaterial_PlanMaterialId",
                table: "Sn_Construction_DailyRltPlanMaterial",
                column: "PlanMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltQuality_DailyId",
                table: "Sn_Construction_DailyRltQuality",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltQuality_QualityProblemId",
                table: "Sn_Construction_DailyRltQuality",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltSafe_DailyId",
                table: "Sn_Construction_DailyRltSafe",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DailyRltSafe_SafeProblemId",
                table: "Sn_Construction_DailyRltSafe",
                column: "SafeProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Dispatch_ContractorId",
                table: "Sn_Construction_Dispatch",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Dispatch_CreatorId",
                table: "Sn_Construction_Dispatch",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Dispatch_DispatchTemplateId",
                table: "Sn_Construction_Dispatch",
                column: "DispatchTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Dispatch_WorkflowId",
                table: "Sn_Construction_Dispatch",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Dispatch_WorkflowTemplateId",
                table: "Sn_Construction_Dispatch",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltFile_DispatchId",
                table: "Sn_Construction_DispatchRltFile",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltFile_FileId",
                table: "Sn_Construction_DispatchRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltMaterial_DispatchId",
                table: "Sn_Construction_DispatchRltMaterial",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltMaterial_MaterialId",
                table: "Sn_Construction_DispatchRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltPlanContent_DispatchId",
                table: "Sn_Construction_DispatchRltPlanContent",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltPlanContent_PlanContentId",
                table: "Sn_Construction_DispatchRltPlanContent",
                column: "PlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltSection_DispatchId",
                table: "Sn_Construction_DispatchRltSection",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltSection_SectionId",
                table: "Sn_Construction_DispatchRltSection",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltStandard_DispatchId",
                table: "Sn_Construction_DispatchRltStandard",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltStandard_StandardId",
                table: "Sn_Construction_DispatchRltStandard",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltWorker_DispatchId",
                table: "Sn_Construction_DispatchRltWorker",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltWorker_WorkerId",
                table: "Sn_Construction_DispatchRltWorker",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_DispatchRltWorkFlow_DispatchId",
                table: "Sn_Construction_DispatchRltWorkFlow",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlan_ChargerId",
                table: "Sn_Construction_MasterPlan",
                column: "ChargerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlan_WorkflowId",
                table: "Sn_Construction_MasterPlan",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlan_WorkflowTemplateId",
                table: "Sn_Construction_MasterPlan",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanContent_MasterPlanId",
                table: "Sn_Construction_MasterPlanContent",
                column: "MasterPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanContent_ParentId",
                table: "Sn_Construction_MasterPlanContent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanContent_SubItemContentId",
                table: "Sn_Construction_MasterPlanContent",
                column: "SubItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanRltContentRltAntecedent_FrontMast~",
                table: "Sn_Construction_MasterPlanRltContentRltAntecedent",
                column: "FrontMasterPlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanRltContentRltAntecedent_MasterPla~",
                table: "Sn_Construction_MasterPlanRltContentRltAntecedent",
                column: "MasterPlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanRltContentRltAntecedent_MasterPl~1",
                table: "Sn_Construction_MasterPlanRltContentRltAntecedent",
                column: "MasterPlanContentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanRltWorkflowInfo_MasterPlanId",
                table: "Sn_Construction_MasterPlanRltWorkflowInfo",
                column: "MasterPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_MasterPlanRltWorkflowInfo_WorkflowId",
                table: "Sn_Construction_MasterPlanRltWorkflowInfo",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Plan_ChargerId",
                table: "Sn_Construction_Plan",
                column: "ChargerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Plan_MasterPlanId",
                table: "Sn_Construction_Plan",
                column: "MasterPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Plan_WorkflowId",
                table: "Sn_Construction_Plan",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_Plan_WorkflowTemplateId",
                table: "Sn_Construction_Plan",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContent_ParentId",
                table: "Sn_Construction_PlanContent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContent_PlanId",
                table: "Sn_Construction_PlanContent",
                column: "PlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContent_SubItemContentId",
                table: "Sn_Construction_PlanContent",
                column: "SubItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltAntecedent_FrontPlanContentId",
                table: "Sn_Construction_PlanContentRltAntecedent",
                column: "FrontPlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltAntecedent_PlanContentId",
                table: "Sn_Construction_PlanContentRltAntecedent",
                column: "PlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltAntecedent_PlanContentId1",
                table: "Sn_Construction_PlanContentRltAntecedent",
                column: "PlanContentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltFile_FileId",
                table: "Sn_Construction_PlanContentRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltFile_PlanContentId",
                table: "Sn_Construction_PlanContentRltFile",
                column: "PlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanContentRltMaterial_PlanContentId",
                table: "Sn_Construction_PlanContentRltMaterial",
                column: "PlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanMaterial_PlanContentId",
                table: "Sn_Construction_PlanMaterial",
                column: "PlanContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanMaterialRltEquipment_EquipmentId",
                table: "Sn_Construction_PlanMaterialRltEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanMaterialRltEquipment_PlanMaterialId",
                table: "Sn_Construction_PlanMaterialRltEquipment",
                column: "PlanMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_PlanRltWorkflowInfo_PlanId",
                table: "Sn_Construction_PlanRltWorkflowInfo",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Construction_UnplannedTask_DailyId",
                table: "Sn_Construction_UnplannedTask",
                column: "DailyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_EquipmentTeam_TypeId",
                table: "Sn_ConstructionBase_EquipmentTeam",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_Procedure_TypeId",
                table: "Sn_ConstructionBase_Procedure",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureEquipmentTeam_EquipmentTeamId",
                table: "Sn_ConstructionBase_ProcedureEquipmentTeam",
                column: "EquipmentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureEquipmentTeam_ProcedureId",
                table: "Sn_ConstructionBase_ProcedureEquipmentTeam",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureMaterial_ConstructionBaseMater~",
                table: "Sn_ConstructionBase_ProcedureMaterial",
                column: "ConstructionBaseMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureMaterial_ProcedureId",
                table: "Sn_ConstructionBase_ProcedureMaterial",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureRltFile_FileId",
                table: "Sn_ConstructionBase_ProcedureRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureRltFile_ProcedureId",
                table: "Sn_ConstructionBase_ProcedureRltFile",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureWorker_ProcedureId",
                table: "Sn_ConstructionBase_ProcedureWorker",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_ProcedureWorker_WorkerId",
                table: "Sn_ConstructionBase_ProcedureWorker",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltEquipmentTeam_EquipmentT~",
                table: "Sn_ConstructionBase_RltProcedureRltEquipmentTeam",
                column: "EquipmentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltEquipmentTeam_SubItemCon~",
                table: "Sn_ConstructionBase_RltProcedureRltEquipmentTeam",
                column: "SubItemContentRltProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltFile_FileId",
                table: "Sn_ConstructionBase_RltProcedureRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltFile_SubItemContentRltPr~",
                table: "Sn_ConstructionBase_RltProcedureRltFile",
                column: "SubItemContentRltProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltMaterial_ConstructionBas~",
                table: "Sn_ConstructionBase_RltProcedureRltMaterial",
                column: "ConstructionBaseMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltMaterial_SubItemContentR~",
                table: "Sn_ConstructionBase_RltProcedureRltMaterial",
                column: "SubItemContentRltProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltWorker_SubItemContentRlt~",
                table: "Sn_ConstructionBase_RltProcedureRltWorker",
                column: "SubItemContentRltProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_RltProcedureRltWorker_WorkerId",
                table: "Sn_ConstructionBase_RltProcedureRltWorker",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_Section_ParentId",
                table: "Sn_ConstructionBase_Section",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_Standard_ProfessionId",
                table: "Sn_ConstructionBase_Standard",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_SubItem_CreatorId",
                table: "Sn_ConstructionBase_SubItem",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_SubItemContent_ParentId",
                table: "Sn_ConstructionBase_SubItemContent",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_SubItemContent_SubItemId",
                table: "Sn_ConstructionBase_SubItemContent",
                column: "SubItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_SubItemContentRltProcedure_ProcedureId",
                table: "Sn_ConstructionBase_SubItemContentRltProcedure",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_ConstructionBase_SubItemContentRltProcedure_SubItemConte~",
                table: "Sn_ConstructionBase_SubItemContentRltProcedure",
                column: "SubItemContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_Contract_TypeId",
                table: "Sn_CostManagement_Contract",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_ContractRltFile_ContractId",
                table: "Sn_CostManagement_ContractRltFile",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_ContractRltFile_FileId",
                table: "Sn_CostManagement_ContractRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_CostOther_TypeId",
                table: "Sn_CostManagement_CostOther",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_MoneyList_PayeeId",
                table: "Sn_CostManagement_MoneyList",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_MoneyList_TypeId",
                table: "Sn_CostManagement_MoneyList",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_PeopleCost_PayeeId",
                table: "Sn_CostManagement_PeopleCost",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CostManagement_PeopleCost_ProfessionalId",
                table: "Sn_CostManagement_PeopleCost",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_AlterRecord_RepairTagId",
                table: "Sn_CrPlan_AlterRecord",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_DailyPlan_RepairTagId",
                table: "Sn_CrPlan_DailyPlan",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_DailyPlanAlter_RepairTagId",
                table: "Sn_CrPlan_DailyPlanAlter",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_EquipmentTestResult_FileId",
                table: "Sn_CrPlan_EquipmentTestResult",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_EquipmentTestResult_RepairTagId",
                table: "Sn_CrPlan_EquipmentTestResult",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWork_OrganizationId",
                table: "Sn_CrPlan_MaintenanceWork",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWork_RepairTagId",
                table: "Sn_CrPlan_MaintenanceWork",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWorkRltFile_FileId",
                table: "Sn_CrPlan_MaintenanceWorkRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWorkRltFile_MaintenanceWorkId",
                table: "Sn_CrPlan_MaintenanceWorkRltFile",
                column: "MaintenanceWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWorkRltFile_RelateFileId",
                table: "Sn_CrPlan_MaintenanceWorkRltFile",
                column: "RelateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWorkRltSkylightPlan_MaintenanceWorkId",
                table: "Sn_CrPlan_MaintenanceWorkRltSkylightPlan",
                column: "MaintenanceWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_MaintenanceWorkRltSkylightPlan_SkylightPlanId",
                table: "Sn_CrPlan_MaintenanceWorkRltSkylightPlan",
                column: "SkylightPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_PlanDetail_InfluenceRangeId",
                table: "Sn_CrPlan_PlanDetail",
                column: "InfluenceRangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_PlanDetail_RepairTagId",
                table: "Sn_CrPlan_PlanDetail",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_PlanRelateEquipment_RepairTagId",
                table: "Sn_CrPlan_PlanRelateEquipment",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_RepairUser_RepairTagId",
                table: "Sn_CrPlan_RepairUser",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlan_RailwayId",
                table: "Sn_CrPlan_SkylightPlan",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlan_RepairTagId",
                table: "Sn_CrPlan_SkylightPlan",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlanRltInstallationSite_InstallationSiteId",
                table: "Sn_CrPlan_SkylightPlanRltInstallationSite",
                column: "InstallationSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlanRltInstallationSite_SkylightPlanId",
                table: "Sn_CrPlan_SkylightPlanRltInstallationSite",
                column: "SkylightPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlanRltWorkTicket_SkylightPlanId",
                table: "Sn_CrPlan_SkylightPlanRltWorkTicket",
                column: "SkylightPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_SkylightPlanRltWorkTicket_WorkTicketId",
                table: "Sn_CrPlan_SkylightPlanRltWorkTicket",
                column: "WorkTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_Worker_RepairTagId",
                table: "Sn_CrPlan_Worker",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkOrder_RepairTagId",
                table: "Sn_CrPlan_WorkOrder",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkOrganization_RepairTagId",
                table: "Sn_CrPlan_WorkOrganization",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkOrganization_OrganizationId_RepairTagId",
                table: "Sn_CrPlan_WorkOrganization",
                columns: new[] { "OrganizationId", "RepairTagId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkTicket_SafetyDispatchCheckerId",
                table: "Sn_CrPlan_WorkTicket",
                column: "SafetyDispatchCheckerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkTicket_TechnicalCheckerId",
                table: "Sn_CrPlan_WorkTicket",
                column: "TechnicalCheckerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_WorkTicketRltCooperationUnit_WorkTicketId",
                table: "Sn_CrPlan_WorkTicketRltCooperationUnit",
                column: "WorkTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_YearMonthPlan_RepairTagId",
                table: "Sn_CrPlan_YearMonthPlan",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_YearMonthPlanAlter_RepairTagId",
                table: "Sn_CrPlan_YearMonthPlanAlter",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_YearMonthPlanTestItem_FileId",
                table: "Sn_CrPlan_YearMonthPlanTestItem",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_CrPlan_YearMonthPlanTestItem_RepairTagId",
                table: "Sn_CrPlan_YearMonthPlanTestItem",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlan_LevelId",
                table: "Sn_Emerg_EmergPlan",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanProcessRecord_EmergPlanRecordId",
                table: "Sn_Emerg_EmergPlanProcessRecord",
                column: "EmergPlanRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecord_LevelId",
                table: "Sn_Emerg_EmergPlanRecord",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecordRltComponentCategory_ComponentCateg~",
                table: "Sn_Emerg_EmergPlanRecordRltComponentCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecordRltComponentCategory_EmergPlanRecor~",
                table: "Sn_Emerg_EmergPlanRecordRltComponentCategory",
                column: "EmergPlanRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecordRltFile_EmergPlanRecordId",
                table: "Sn_Emerg_EmergPlanRecordRltFile",
                column: "EmergPlanRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecordRltFile_FileId",
                table: "Sn_Emerg_EmergPlanRecordRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRecordRltMember_EmergPlanRecordId",
                table: "Sn_Emerg_EmergPlanRecordRltMember",
                column: "EmergPlanRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRltComponentCategory_ComponentCategoryId",
                table: "Sn_Emerg_EmergPlanRltComponentCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRltComponentCategory_EmergPlanId",
                table: "Sn_Emerg_EmergPlanRltComponentCategory",
                column: "EmergPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRltFile_EmergPlanId",
                table: "Sn_Emerg_EmergPlanRltFile",
                column: "EmergPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_EmergPlanRltFile_FileId",
                table: "Sn_Emerg_EmergPlanRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_CheckInUserId",
                table: "Sn_Emerg_Fault",
                column: "CheckInUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_CheckOutUserId",
                table: "Sn_Emerg_Fault",
                column: "CheckOutUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_EmergPlanRecordId",
                table: "Sn_Emerg_Fault",
                column: "EmergPlanRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_LevelId",
                table: "Sn_Emerg_Fault",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_OrganizationId",
                table: "Sn_Emerg_Fault",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_RailwayId",
                table: "Sn_Emerg_Fault",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_ReasonTypeId",
                table: "Sn_Emerg_Fault",
                column: "ReasonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_StationId",
                table: "Sn_Emerg_Fault",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_Fault_SubmitUserId",
                table: "Sn_Emerg_Fault",
                column: "SubmitUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_FaultRltComponentCategory_ComponentCategoryId",
                table: "Sn_Emerg_FaultRltComponentCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_FaultRltComponentCategory_FaultId",
                table: "Sn_Emerg_FaultRltComponentCategory",
                column: "FaultId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_FaultRltEquipment_EquipmentId",
                table: "Sn_Emerg_FaultRltEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Emerg_FaultRltEquipment_FaultId",
                table: "Sn_Emerg_FaultRltEquipment",
                column: "FaultId");

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
                name: "IX_Sn_FileApprove_FileApprove_FileId",
                table: "Sn_FileApprove_FileApprove",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_FileApprove_FileApprove_WorkflowId",
                table: "Sn_FileApprove_FileApprove",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_FileApprove_FileApprove_WorkflowTemplateId",
                table: "Sn_FileApprove_FileApprove",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_FileApprove_FileApproveRltFlow_FileApproveId",
                table: "Sn_FileApprove_FileApproveRltFlow",
                column: "FileApproveId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Contract_CreatorId",
                table: "Sn_Material_Contract",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_ContractRltFile_ContractId",
                table: "Sn_Material_ContractRltFile",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_ContractRltFile_FileId",
                table: "Sn_Material_ContractRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecord_CreatorId",
                table: "Sn_Material_EntryRecord",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecord_PartitionId",
                table: "Sn_Material_EntryRecord",
                column: "PartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltFile_EntryRecordId",
                table: "Sn_Material_EntryRecordRltFile",
                column: "EntryRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltFile_FileId",
                table: "Sn_Material_EntryRecordRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltMaterial_EntryRecordId",
                table: "Sn_Material_EntryRecordRltMaterial",
                column: "EntryRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltMaterial_InventoryId",
                table: "Sn_Material_EntryRecordRltMaterial",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltMaterial_MaterialId",
                table: "Sn_Material_EntryRecordRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltMaterial_SupplierId",
                table: "Sn_Material_EntryRecordRltMaterial",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_EntryRecordRltQRCode_EntryRecordId",
                table: "Sn_Material_EntryRecordRltQRCode",
                column: "EntryRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Inventory_MaterialId",
                table: "Sn_Material_Inventory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Inventory_PartitionId",
                table: "Sn_Material_Inventory",
                column: "PartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Inventory_SupplierId",
                table: "Sn_Material_Inventory",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptance_CreatorId",
                table: "Sn_Material_MaterialAcceptance",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptance_TestingOrganizationId",
                table: "Sn_Material_MaterialAcceptance",
                column: "TestingOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltFile_FileId",
                table: "Sn_Material_MaterialAcceptanceRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltFile_MaterialAcceptanceId",
                table: "Sn_Material_MaterialAcceptanceRltFile",
                column: "MaterialAcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltMaterial_MaterialAcceptanc~",
                table: "Sn_Material_MaterialAcceptanceRltMaterial",
                column: "MaterialAcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltMaterial_MaterialId",
                table: "Sn_Material_MaterialAcceptanceRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltPurchase_MaterialAcceptanc~",
                table: "Sn_Material_MaterialAcceptanceRltPurchase",
                column: "MaterialAcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltPurchase_PurchaseListId",
                table: "Sn_Material_MaterialAcceptanceRltPurchase",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialAcceptanceRltQRCode_MaterialAcceptanceId",
                table: "Sn_Material_MaterialAcceptanceRltQRCode",
                column: "MaterialAcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialOfBill_SectionId",
                table: "Sn_Material_MaterialOfBill",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialOfBillRltAccessory_FileId",
                table: "Sn_Material_MaterialOfBillRltAccessory",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialOfBillRltAccessory_MaterialOfBillId",
                table: "Sn_Material_MaterialOfBillRltAccessory",
                column: "MaterialOfBillId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialOfBillRltMaterial_InventoryId",
                table: "Sn_Material_MaterialOfBillRltMaterial",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_MaterialOfBillRltMaterial_MaterialOfBillId",
                table: "Sn_Material_MaterialOfBillRltMaterial",
                column: "MaterialOfBillId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecord_CreatorId",
                table: "Sn_Material_OutRecord",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecord_PartitionId",
                table: "Sn_Material_OutRecord",
                column: "PartitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltFile_FileId",
                table: "Sn_Material_OutRecordRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltFile_OutRecordId",
                table: "Sn_Material_OutRecordRltFile",
                column: "OutRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltMaterial_InventoryId",
                table: "Sn_Material_OutRecordRltMaterial",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltMaterial_MaterialId",
                table: "Sn_Material_OutRecordRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltMaterial_OutRecordId",
                table: "Sn_Material_OutRecordRltMaterial",
                column: "OutRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltMaterial_SupplierId",
                table: "Sn_Material_OutRecordRltMaterial",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_OutRecordRltQRCode_OutRecordId",
                table: "Sn_Material_OutRecordRltQRCode",
                column: "OutRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Partition_FileId",
                table: "Sn_Material_Partition",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_Partition_ParentId",
                table: "Sn_Material_Partition",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseList_CreatorId",
                table: "Sn_Material_PurchaseList",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseList_WorkflowId",
                table: "Sn_Material_PurchaseList",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseList_WorkflowTemplateId",
                table: "Sn_Material_PurchaseList",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltFile_FileId",
                table: "Sn_Material_PurchaseListRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltFile_PurchaseListId",
                table: "Sn_Material_PurchaseListRltFile",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltFlow_PurchaseListId",
                table: "Sn_Material_PurchaseListRltFlow",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltMaterial_MaterialId",
                table: "Sn_Material_PurchaseListRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltMaterial_PurchaseListId",
                table: "Sn_Material_PurchaseListRltMaterial",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltPurchasePlan_PurchaseListId",
                table: "Sn_Material_PurchaseListRltPurchasePlan",
                column: "PurchaseListId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchaseListRltPurchasePlan_PurchasePlanId",
                table: "Sn_Material_PurchaseListRltPurchasePlan",
                column: "PurchasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlan_CreatorId",
                table: "Sn_Material_PurchasePlan",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlan_WorkflowId",
                table: "Sn_Material_PurchasePlan",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlan_WorkflowTemplateId",
                table: "Sn_Material_PurchasePlan",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlanRltFile_FileId",
                table: "Sn_Material_PurchasePlanRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlanRltFile_PurchasePlanId",
                table: "Sn_Material_PurchasePlanRltFile",
                column: "PurchasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlanRltFlow_PurchasePlanId",
                table: "Sn_Material_PurchasePlanRltFlow",
                column: "PurchasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlanRltMaterial_MaterialId",
                table: "Sn_Material_PurchasePlanRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_PurchasePlanRltMaterial_PurchasePlanId",
                table: "Sn_Material_PurchasePlanRltMaterial",
                column: "PurchasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_SupplierRltAccessory_FileId",
                table: "Sn_Material_SupplierRltAccessory",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_SupplierRltAccessory_SupplierId",
                table: "Sn_Material_SupplierRltAccessory",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Material_SupplierRltContacts_SupplierId",
                table: "Sn_Material_SupplierRltContacts",
                column: "SupplierId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_Contract_HostDepartmentId",
                table: "Sn_Oa_Contract",
                column: "HostDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_Contract_TypeId",
                table: "Sn_Oa_Contract",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_Contract_UnderDepartmentId",
                table: "Sn_Oa_Contract",
                column: "UnderDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_Contract_UndertakerId",
                table: "Sn_Oa_Contract",
                column: "UndertakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_ContractRltFile_ContractId",
                table: "Sn_Oa_ContractRltFile",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_ContractRltFile_FileId",
                table: "Sn_Oa_ContractRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_DutySchedule_OrganizationId",
                table: "Sn_Oa_DutySchedule",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_DutyScheduleRltUser_DutyScheduleId",
                table: "Sn_Oa_DutyScheduleRltUser",
                column: "DutyScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_DutyScheduleRltUser_UserId",
                table: "Sn_Oa_DutyScheduleRltUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_Seal_ImageId",
                table: "Sn_Oa_Seal",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Oa_SealRltMember_SealId",
                table: "Sn_Oa_SealRltMember",
                column: "SealId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Problem_ProblemCategory_ParentId",
                table: "Sn_Problem_ProblemCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Problem_ProblemRltProblemCategory_ProblemCategoryId",
                table: "Sn_Problem_ProblemRltProblemCategory",
                column: "ProblemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Problem_ProblemRltProblemCategory_ProblemId",
                table: "Sn_Problem_ProblemRltProblemCategory",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Archives_ArchivesCategoryId",
                table: "Sn_Project_Archives",
                column: "ArchivesCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Archives_BooksClassificationId",
                table: "Sn_Project_Archives",
                column: "BooksClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ArchivesCategory_ParentId",
                table: "Sn_Project_ArchivesCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Dossier_ArchivesId",
                table: "Sn_Project_Dossier",
                column: "ArchivesId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Dossier_FileCategoryId",
                table: "Sn_Project_Dossier",
                column: "FileCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_DossierCategory_ParentId",
                table: "Sn_Project_DossierCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_DossierRltFile_DossierId",
                table: "Sn_Project_DossierRltFile",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_DossierRltFile_FileId",
                table: "Sn_Project_DossierRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Project_ManagerId",
                table: "Sn_Project_Project",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Project_OrganizationId",
                table: "Sn_Project_Project",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_Project_TypeId",
                table: "Sn_Project_Project",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltContract_ContractId",
                table: "Sn_Project_ProjectRltContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltContract_ProjectId",
                table: "Sn_Project_ProjectRltContract",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltFile_FileId",
                table: "Sn_Project_ProjectRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltFile_ProjectId",
                table: "Sn_Project_ProjectRltFile",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltMember_ManagerId",
                table: "Sn_Project_ProjectRltMember",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltMember_ProjectId",
                table: "Sn_Project_ProjectRltMember",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltUnit_ProjectId",
                table: "Sn_Project_ProjectRltUnit",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Project_ProjectRltUnit_UnitId",
                table: "Sn_Project_ProjectRltUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_CheckUnitId",
                table: "Sn_Quality_QualityProblem",
                column: "CheckUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_CheckerId",
                table: "Sn_Quality_QualityProblem",
                column: "CheckerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_ProfessionId",
                table: "Sn_Quality_QualityProblem",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_ResponsibleOrganizationId",
                table: "Sn_Quality_QualityProblem",
                column: "ResponsibleOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_ResponsibleUserId",
                table: "Sn_Quality_QualityProblem",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblem_VerifierId",
                table: "Sn_Quality_QualityProblem",
                column: "VerifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemLibrary_ProfessionId",
                table: "Sn_Quality_QualityProblemLibrary",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemLibraryRltScop_QualityProblemLibra~",
                table: "Sn_Quality_QualityProblemLibraryRltScop",
                column: "QualityProblemLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemLibraryRltScop_ScopId",
                table: "Sn_Quality_QualityProblemLibraryRltScop",
                column: "ScopId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRecord_QualityProblemId",
                table: "Sn_Quality_QualityProblemRecord",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRecord_UserId",
                table: "Sn_Quality_QualityProblemRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRecordRleFile_FileId",
                table: "Sn_Quality_QualityProblemRecordRleFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRecordRleFile_QualityProblemRecord~",
                table: "Sn_Quality_QualityProblemRecordRleFile",
                column: "QualityProblemRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltCcUser_CcUserId",
                table: "Sn_Quality_QualityProblemRltCcUser",
                column: "CcUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltCcUser_QualityProblemId",
                table: "Sn_Quality_QualityProblemRltCcUser",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltEquipment_EquipmentId",
                table: "Sn_Quality_QualityProblemRltEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltEquipment_QualityProblemId",
                table: "Sn_Quality_QualityProblemRltEquipment",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltFile_FileId",
                table: "Sn_Quality_QualityProblemRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Quality_QualityProblemRltFile_QualityProblemId",
                table: "Sn_Quality_QualityProblemRltFile",
                column: "QualityProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_Institution_OrganizationId",
                table: "Sn_Regulation_Institution",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltAuthority_InstitutionId",
                table: "Sn_Regulation_InstitutionRltAuthority",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltEdition_InstitutionId",
                table: "Sn_Regulation_InstitutionRltEdition",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFile_FileId",
                table: "Sn_Regulation_InstitutionRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFile_InstitutionId",
                table: "Sn_Regulation_InstitutionRltFile",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFlow_CreatorId",
                table: "Sn_Regulation_InstitutionRltFlow",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFlow_InstitutionId",
                table: "Sn_Regulation_InstitutionRltFlow",
                column: "InstitutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFlow_SealId",
                table: "Sn_Regulation_InstitutionRltFlow",
                column: "SealId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltFlow_WorkFlowId",
                table: "Sn_Regulation_InstitutionRltFlow",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltLabel_InstitutionId",
                table: "Sn_Regulation_InstitutionRltLabel",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Regulation_InstitutionRltLabel_LabelId",
                table: "Sn_Regulation_InstitutionRltLabel",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Report_Report_OrganizationId",
                table: "Sn_Report_Report",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Report_ReportRltFile_FileId",
                table: "Sn_Report_ReportRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Report_ReportRltFile_ReportId",
                table: "Sn_Report_ReportRltFile",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Report_ReportRltUser_ReportId",
                table: "Sn_Report_ReportRltUser",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Report_ReportRltUser_UserId",
                table: "Sn_Report_ReportRltUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_CableCore_CableId",
                table: "Sn_Resource_CableCore",
                column: "CableId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_CableLocation_CableId",
                table: "Sn_Resource_CableLocation",
                column: "CableId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_ComponentRltQRCode_GenerateEquipmentId",
                table: "Sn_Resource_ComponentRltQRCode",
                column: "GenerateEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_ComponentRltQRCode_InstallationEquipmentId",
                table: "Sn_Resource_ComponentRltQRCode",
                column: "InstallationEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_ComponentTrackRecord_ComponentRltQRCodeId",
                table: "Sn_Resource_ComponentTrackRecord",
                column: "ComponentRltQRCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_ComponentTrackRecord_UserId",
                table: "Sn_Resource_ComponentTrackRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_CableExtendId",
                table: "Sn_Resource_Equipment",
                column: "CableExtendId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_ComponentCategoryId",
                table: "Sn_Resource_Equipment",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_EndInstallationSiteId",
                table: "Sn_Resource_Equipment",
                column: "EndInstallationSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_GroupId",
                table: "Sn_Resource_Equipment",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_InstallationSiteId",
                table: "Sn_Resource_Equipment",
                column: "InstallationSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_ManufacturerId",
                table: "Sn_Resource_Equipment",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_OrganizationId",
                table: "Sn_Resource_Equipment",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_ParentId",
                table: "Sn_Resource_Equipment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_ProductCategoryId",
                table: "Sn_Resource_Equipment",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Equipment_StoreEquipmentId",
                table: "Sn_Resource_Equipment",
                column: "StoreEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentGroup_Name",
                table: "Sn_Resource_EquipmentGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentGroup_OrganizationId",
                table: "Sn_Resource_EquipmentGroup",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentGroup_ParentId",
                table: "Sn_Resource_EquipmentGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentProperty_EquipmentId",
                table: "Sn_Resource_EquipmentProperty",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentProperty_MVDCategoryId",
                table: "Sn_Resource_EquipmentProperty",
                column: "MVDCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentProperty_MVDPropertyId",
                table: "Sn_Resource_EquipmentProperty",
                column: "MVDPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentRltFile_EquipmentId",
                table: "Sn_Resource_EquipmentRltFile",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentRltFile_FileId",
                table: "Sn_Resource_EquipmentRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentRltOrganization_EquipmentId",
                table: "Sn_Resource_EquipmentRltOrganization",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentRltOrganization_OrganizationId",
                table: "Sn_Resource_EquipmentRltOrganization",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentServiceRecord_EquipmentId",
                table: "Sn_Resource_EquipmentServiceRecord",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentServiceRecord_StoreEquipmentId",
                table: "Sn_Resource_EquipmentServiceRecord",
                column: "StoreEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_EquipmentServiceRecord_UserId",
                table: "Sn_Resource_EquipmentServiceRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipment_ComponentCategoryId",
                table: "Sn_Resource_StoreEquipment",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipment_CreatorId",
                table: "Sn_Resource_StoreEquipment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipment_ManufacturerId",
                table: "Sn_Resource_StoreEquipment",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipment_ProductCategoryId",
                table: "Sn_Resource_StoreEquipment",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipment_StoreHouseId",
                table: "Sn_Resource_StoreEquipment",
                column: "StoreHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTest_OrganizationId",
                table: "Sn_Resource_StoreEquipmentTest",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTest_TesterId",
                table: "Sn_Resource_StoreEquipmentTest",
                column: "TesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTestRltEquipment_StoreEquipmentId",
                table: "Sn_Resource_StoreEquipmentTestRltEquipment",
                column: "StoreEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTestRltEquipment_StoreEquipmentTe~",
                table: "Sn_Resource_StoreEquipmentTestRltEquipment",
                column: "StoreEquipmentTestId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTransfer_StoreHouseId",
                table: "Sn_Resource_StoreEquipmentTransfer",
                column: "StoreHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTransfer_UserId",
                table: "Sn_Resource_StoreEquipmentTransfer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTransferRltEquipment_StoreEquipme~",
                table: "Sn_Resource_StoreEquipmentTransferRltEquipment",
                column: "StoreEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreEquipmentTransferRltEquipment_StoreEquipm~1",
                table: "Sn_Resource_StoreEquipmentTransferRltEquipment",
                column: "StoreEquipmentTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreHouse_AreaId",
                table: "Sn_Resource_StoreHouse",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreHouse_OrganizationId",
                table: "Sn_Resource_StoreHouse",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_StoreHouse_ParentId",
                table: "Sn_Resource_StoreHouse",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_Terminal_EquipmentId",
                table: "Sn_Resource_Terminal",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalBusinessPathNode_CableCoreId",
                table: "Sn_Resource_TerminalBusinessPathNode",
                column: "CableCoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalBusinessPathNode_TerminalBusinessPathId",
                table: "Sn_Resource_TerminalBusinessPathNode",
                column: "TerminalBusinessPathId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalBusinessPathNode_TerminalId",
                table: "Sn_Resource_TerminalBusinessPathNode",
                column: "TerminalId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalLink_CableCoreId",
                table: "Sn_Resource_TerminalLink",
                column: "CableCoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalLink_TerminalAId",
                table: "Sn_Resource_TerminalLink",
                column: "TerminalAId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_TerminalLink_TerminalBId",
                table: "Sn_Resource_TerminalLink",
                column: "TerminalBId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_CheckUnitId",
                table: "Sn_Safe_SafeProblem",
                column: "CheckUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_CheckerId",
                table: "Sn_Safe_SafeProblem",
                column: "CheckerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_ProfessionId",
                table: "Sn_Safe_SafeProblem",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_ResponsibleOrganizationId",
                table: "Sn_Safe_SafeProblem",
                column: "ResponsibleOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_ResponsibleUserId",
                table: "Sn_Safe_SafeProblem",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_TypeId",
                table: "Sn_Safe_SafeProblem",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblem_VerifierId",
                table: "Sn_Safe_SafeProblem",
                column: "VerifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemLibrary_EventTypeId",
                table: "Sn_Safe_SafeProblemLibrary",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemLibrary_ProfessionId",
                table: "Sn_Safe_SafeProblemLibrary",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemLibraryRltScop_SafeProblemLibraryId",
                table: "Sn_Safe_SafeProblemLibraryRltScop",
                column: "SafeProblemLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemLibraryRltScop_ScopId",
                table: "Sn_Safe_SafeProblemLibraryRltScop",
                column: "ScopId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRecord_SafeProblemId",
                table: "Sn_Safe_SafeProblemRecord",
                column: "SafeProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRecord_UserId",
                table: "Sn_Safe_SafeProblemRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRecordRleFile_FileId",
                table: "Sn_Safe_SafeProblemRecordRleFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRecordRleFile_SafeProblemRecordId",
                table: "Sn_Safe_SafeProblemRecordRleFile",
                column: "SafeProblemRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltCcUser_CcUserId",
                table: "Sn_Safe_SafeProblemRltCcUser",
                column: "CcUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltCcUser_SafeProblemId",
                table: "Sn_Safe_SafeProblemRltCcUser",
                column: "SafeProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltEquipment_EquipmentId",
                table: "Sn_Safe_SafeProblemRltEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltEquipment_SafeProblemId",
                table: "Sn_Safe_SafeProblemRltEquipment",
                column: "SafeProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltFile_FileId",
                table: "Sn_Safe_SafeProblemRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeProblemRltFile_SafeProblemId",
                table: "Sn_Safe_SafeProblemRltFile",
                column: "SafeProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Safe_SafeSpeechVideo_VideoId",
                table: "Sn_Safe_SafeSpeechVideo",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_BasePrice_AreaId",
                table: "Sn_StdBasic_BasePrice",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_BasePrice_ComputerCodeId",
                table: "Sn_StdBasic_BasePrice",
                column: "ComputerCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_BasePrice_StandardCodeId",
                table: "Sn_StdBasic_BasePrice",
                column: "StandardCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_BasePrice_StandardId",
                table: "Sn_StdBasic_BasePrice",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Block_BlockCategoryId",
                table: "Sn_StdBasic_Block",
                column: "BlockCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_BlockCategory_ParentId",
                table: "Sn_StdBasic_BlockCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategory_ParentId",
                table: "Sn_StdBasic_ComponentCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltMaterial_ComponentCategoryId",
                table: "Sn_StdBasic_ComponentCategoryRltMaterial",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltMaterial_ComputerCodeId",
                table: "Sn_StdBasic_ComponentCategoryRltMaterial",
                column: "ComputerCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltMVDProperty_ComponentCatego~",
                table: "Sn_StdBasic_ComponentCategoryRltMVDProperty",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltMVDProperty_MVDPropertyId",
                table: "Sn_StdBasic_ComponentCategoryRltMVDProperty",
                column: "MVDPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltProductCategory_ComponentCa~",
                table: "Sn_StdBasic_ComponentCategoryRltProductCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltProductCategory_ProductionC~",
                table: "Sn_StdBasic_ComponentCategoryRltProductCategory",
                column: "ProductionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltQuota_ComponentCategoryId",
                table: "Sn_StdBasic_ComponentCategoryRltQuota",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ComponentCategoryRltQuota_QuotaId",
                table: "Sn_StdBasic_ComponentCategoryRltQuota",
                column: "QuotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_EquipmentControlType_ManufacturerId",
                table: "Sn_StdBasic_EquipmentControlType",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_IndividualProject_ParentId",
                table: "Sn_StdBasic_IndividualProject",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_IndividualProject_SpecialtyId",
                table: "Sn_StdBasic_IndividualProject",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_InfluenceRange_TagId",
                table: "Sn_StdBasic_InfluenceRange",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Manufacturer_ParentId",
                table: "Sn_StdBasic_Manufacturer",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Model_ComponentCategoryId",
                table: "Sn_StdBasic_Model",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Model_ManufacturerId",
                table: "Sn_StdBasic_Model",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Model_ProductCategoryId",
                table: "Sn_StdBasic_Model",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelFile_FamilyFileId",
                table: "Sn_StdBasic_ModelFile",
                column: "FamilyFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelFile_ModelId",
                table: "Sn_StdBasic_ModelFile",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelFile_ThumbId",
                table: "Sn_StdBasic_ModelFile",
                column: "ThumbId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltBlock_BlockId",
                table: "Sn_StdBasic_ModelRltBlock",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltBlock_ModelId",
                table: "Sn_StdBasic_ModelRltBlock",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltModel_ModelId",
                table: "Sn_StdBasic_ModelRltModel",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltModel_ParentId",
                table: "Sn_StdBasic_ModelRltModel",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltMVDProperty_MVDPropertyId",
                table: "Sn_StdBasic_ModelRltMVDProperty",
                column: "MVDPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelRltMVDProperty_ModelId",
                table: "Sn_StdBasic_ModelRltMVDProperty",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelTerminal_ModelId",
                table: "Sn_StdBasic_ModelTerminal",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ModelTerminal_ProductCategoryId",
                table: "Sn_StdBasic_ModelTerminal",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_MVDProperty_MVDCategoryId",
                table: "Sn_StdBasic_MVDProperty",
                column: "MVDCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProcessTemplate_ParentId",
                table: "Sn_StdBasic_ProcessTemplate",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategory_ParentId",
                table: "Sn_StdBasic_ProductCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltMaterial_ComputerCodeId",
                table: "Sn_StdBasic_ProductCategoryRltMaterial",
                column: "ComputerCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltMaterial_ProductCategoryId",
                table: "Sn_StdBasic_ProductCategoryRltMaterial",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltMVDProperty_MVDPropertyId",
                table: "Sn_StdBasic_ProductCategoryRltMVDProperty",
                column: "MVDPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltMVDProperty_ProductCategoryId",
                table: "Sn_StdBasic_ProductCategoryRltMVDProperty",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltQuota_ProductCategoryId",
                table: "Sn_StdBasic_ProductCategoryRltQuota",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProductCategoryRltQuota_QuotaId",
                table: "Sn_StdBasic_ProductCategoryRltQuota",
                column: "QuotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltComponentCategory_ComponentCatego~",
                table: "Sn_StdBasic_ProjectItemRltComponentCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltComponentCategory_ProjectItemId",
                table: "Sn_StdBasic_ProjectItemRltComponentCategory",
                column: "ProjectItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltIndividualProject_IndividualProje~",
                table: "Sn_StdBasic_ProjectItemRltIndividualProject",
                column: "IndividualProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltIndividualProject_ProjectItemId",
                table: "Sn_StdBasic_ProjectItemRltIndividualProject",
                column: "ProjectItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltProcessTemplate_ProcessTemplateId",
                table: "Sn_StdBasic_ProjectItemRltProcessTemplate",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltProductCategory_ProductCategoryId",
                table: "Sn_StdBasic_ProjectItemRltProductCategory",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_ProjectItemRltProductCategory_ProjectItemId",
                table: "Sn_StdBasic_ProjectItemRltProductCategory",
                column: "ProjectItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_Quota_QuotaCategoryId",
                table: "Sn_StdBasic_Quota",
                column: "QuotaCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaCategory_AreaId",
                table: "Sn_StdBasic_QuotaCategory",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaCategory_ParentId",
                table: "Sn_StdBasic_QuotaCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaCategory_SpecialtyId",
                table: "Sn_StdBasic_QuotaCategory",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaCategory_StandardCodeId",
                table: "Sn_StdBasic_QuotaCategory",
                column: "StandardCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaItem_BasePriceId",
                table: "Sn_StdBasic_QuotaItem",
                column: "BasePriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_QuotaItem_QuotaId",
                table: "Sn_StdBasic_QuotaItem",
                column: "QuotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairGroup_ParentId",
                table: "Sn_StdBasic_RepairGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItem_GroupId",
                table: "Sn_StdBasic_RepairItem",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItem_TagId",
                table: "Sn_StdBasic_RepairItem",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItemRltComponentCategory_ComponentCategor~",
                table: "Sn_StdBasic_RepairItemRltComponentCategory",
                column: "ComponentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItemRltComponentCategory_RepairItemId",
                table: "Sn_StdBasic_RepairItemRltComponentCategory",
                column: "RepairItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItemRltOrganizationType_OrganizationTypeId",
                table: "Sn_StdBasic_RepairItemRltOrganizationType",
                column: "OrganizationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairItemRltOrganizationType_RepairItemId",
                table: "Sn_StdBasic_RepairItemRltOrganizationType",
                column: "RepairItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairTestItem_FileId",
                table: "Sn_StdBasic_RepairTestItem",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RepairTestItem_RepairItemId",
                table: "Sn_StdBasic_RepairTestItem",
                column: "RepairItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_RevitConnector_ModelFileId",
                table: "Sn_StdBasic_RevitConnector",
                column: "ModelFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_WorkAttention_RepairTagId",
                table: "Sn_StdBasic_WorkAttention",
                column: "RepairTagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_StdBasic_WorkAttention_TypeId",
                table: "Sn_StdBasic_WorkAttention",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_Task_ParentId",
                table: "Sn_Task_Task",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_Task_ProjectId",
                table: "Sn_Task_Task",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_TaskRltFile_FileId",
                table: "Sn_Task_TaskRltFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_TaskRltFile_TaskId",
                table: "Sn_Task_TaskRltFile",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_TaskRltMember_MemberId",
                table: "Sn_Task_TaskRltMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Task_TaskRltMember_TaskId",
                table: "Sn_Task_TaskRltMember",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterface_BuilderId",
                table: "Sn_Technology_ConstructInterface",
                column: "BuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterface_EquipmentId",
                table: "Sn_Technology_ConstructInterface",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterface_InterfaceManagementTypeId",
                table: "Sn_Technology_ConstructInterface",
                column: "InterfaceManagementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterface_ProfessionId",
                table: "Sn_Technology_ConstructInterface",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfo_BuilderId",
                table: "Sn_Technology_ConstructInterfaceInfo",
                column: "BuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfo_ConstructInterfaceId",
                table: "Sn_Technology_ConstructInterfaceInfo",
                column: "ConstructInterfaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfo_MarkerId",
                table: "Sn_Technology_ConstructInterfaceInfo",
                column: "MarkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfo_ReformerId",
                table: "Sn_Technology_ConstructInterfaceInfo",
                column: "ReformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfoRltMarkFile_ConstructIn~",
                table: "Sn_Technology_ConstructInterfaceInfoRltMarkFile",
                column: "ConstructInterfaceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_ConstructInterfaceInfoRltMarkFile_MarkFileId",
                table: "Sn_Technology_ConstructInterfaceInfoRltMarkFile",
                column: "MarkFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_Disclose_ParentId",
                table: "Sn_Technology_Disclose",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_Material_TypeId",
                table: "Sn_Technology_Material",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlan_CreatorId",
                table: "Sn_Technology_MaterialPlan",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlan_WorkflowId",
                table: "Sn_Technology_MaterialPlan",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlan_WorkflowTemplateId",
                table: "Sn_Technology_MaterialPlan",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlanFlowInfo_MaterialPlanId",
                table: "Sn_Technology_MaterialPlanFlowInfo",
                column: "MaterialPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlanRltMaterial_MaterialId",
                table: "Sn_Technology_MaterialPlanRltMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Technology_MaterialPlanRltMaterial_MaterialPlanId",
                table: "Sn_Technology_MaterialPlanRltMaterial",
                column: "MaterialPlanId");
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
                name: "Sn_Alarm_Alarm");

            migrationBuilder.DropTable(
                name: "Sn_Alarm_AlarmConfig");

            migrationBuilder.DropTable(
                name: "Sn_Alarm_AlarmEquipmentIdBind");

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
                name: "Sn_App_IdentityUserRltProject");

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
                name: "Sn_Basic_RailwayRltOrganization");

            migrationBuilder.DropTable(
                name: "Sn_Basic_StationRltOrganization");

            migrationBuilder.DropTable(
                name: "Sn_Basic_StationRltRailway");

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
                name: "Sn_Common_QRCode");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyFlowInfo");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyRltPlanMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyRltQuality");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyRltSafe");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltPlanContent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltSection");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltStandard");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltWorker");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchRltWorkFlow");

            migrationBuilder.DropTable(
                name: "Sn_Construction_MasterPlanRltContentRltAntecedent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_MasterPlanRltWorkflowInfo");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanContentRltAntecedent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanContentRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanContentRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanMaterialRltEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanRltWorkflowInfo");

            migrationBuilder.DropTable(
                name: "Sn_Construction_UnplannedTask");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_ProcedureEquipmentTeam");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_ProcedureMaterial");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_ProcedureRltFile");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_ProcedureWorker");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_RltProcedureRltEquipmentTeam");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_RltProcedureRltFile");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_RltProcedureRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_RltProcedureRltWorker");

            migrationBuilder.DropTable(
                name: "Sn_CostManagement_ContractRltFile");

            migrationBuilder.DropTable(
                name: "Sn_CostManagement_CostOther");

            migrationBuilder.DropTable(
                name: "Sn_CostManagement_MoneyList");

            migrationBuilder.DropTable(
                name: "Sn_CostManagement_PeopleCost");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_AlterRecord");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_DailyPlan");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_DailyPlanAlter");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_EquipmentTestResult");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_MaintenanceWorkRltFile");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_MaintenanceWorkRltSkylightPlan");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_PlanDetail");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_PlanRelateEquipment");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_RepairUser");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_SkylightPlanRltInstallationSite");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_SkylightPlanRltWorkTicket");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_StatisticsEquipmentWorker");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_StatisticsPieWorker");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_Worker");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_WorkOrder");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_WorkOrderTestAdditional");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_WorkOrganization");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_WorkTicketRltCooperationUnit");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_YearMonthAlterRecord");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_YearMonthPlan");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_YearMonthPlanAlter");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_YearMonthPlanTestItem");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanProcessRecord");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRecordRltComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRecordRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRecordRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRltComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_FaultRltComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_FaultRltEquipment");

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
                name: "Sn_FileApprove_FileApproveRltFlow");

            migrationBuilder.DropTable(
                name: "Sn_Material_ContractRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_EntryRecordRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_EntryRecordRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_EntryRecordRltQRCode");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialAcceptanceRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialAcceptanceRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialAcceptanceRltPurchase");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialAcceptanceRltQRCode");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialOfBillRltAccessory");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialOfBillRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_OutRecordRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_OutRecordRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_OutRecordRltQRCode");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchaseListRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchaseListRltFlow");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchaseListRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchaseListRltPurchasePlan");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchasePlanRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchasePlanRltFlow");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchasePlanRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Material_SupplierRltAccessory");

            migrationBuilder.DropTable(
                name: "Sn_Material_SupplierRltContacts");

            migrationBuilder.DropTable(
                name: "Sn_Message_Bpm_BpmRltMessage");

            migrationBuilder.DropTable(
                name: "Sn_Message_Notice_Notice");

            migrationBuilder.DropTable(
                name: "Sn_Oa_ContractRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Oa_DutyScheduleRltUser");

            migrationBuilder.DropTable(
                name: "Sn_Oa_SealRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Problem_ProblemRltProblemCategory");

            migrationBuilder.DropTable(
                name: "Sn_Project_DossierCategory");

            migrationBuilder.DropTable(
                name: "Sn_Project_DossierRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Project_ProjectRltContract");

            migrationBuilder.DropTable(
                name: "Sn_Project_ProjectRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Project_ProjectRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Project_ProjectRltUnit");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemLibraryRltScop");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemRecordRleFile");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemRltCcUser");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemRltEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_InstitutionRltAuthority");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_InstitutionRltEdition");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_InstitutionRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_InstitutionRltFlow");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_InstitutionRltLabel");

            migrationBuilder.DropTable(
                name: "Sn_Report_ReportRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Report_ReportRltUser");

            migrationBuilder.DropTable(
                name: "Sn_Resource_CableLocation");

            migrationBuilder.DropTable(
                name: "Sn_Resource_ComponentTrackRecord");

            migrationBuilder.DropTable(
                name: "Sn_Resource_EquipmentProperty");

            migrationBuilder.DropTable(
                name: "Sn_Resource_EquipmentRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Resource_EquipmentRltOrganization");

            migrationBuilder.DropTable(
                name: "Sn_Resource_EquipmentServiceRecord");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreEquipmentTestRltEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreEquipmentTransferRltEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Resource_TerminalBusinessPathNode");

            migrationBuilder.DropTable(
                name: "Sn_Resource_TerminalLink");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemLibraryRltScop");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemRecordRleFile");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemRltCcUser");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemRltEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeSpeechVideo");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComponentCategoryRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComponentCategoryRltMVDProperty");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComponentCategoryRltProductCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComponentCategoryRltQuota");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_EquipmentControlType");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ModelRltBlock");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ModelRltModel");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ModelRltMVDProperty");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ModelTerminal");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProductCategoryRltMaterial");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProductCategoryRltMVDProperty");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProductCategoryRltQuota");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProjectItemRltComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProjectItemRltIndividualProject");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProjectItemRltProcessTemplate");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProjectItemRltProductCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_QuotaItem");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RepairItemRltComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RepairItemRltOrganizationType");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RepairTestItem");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RevitConnector");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_WorkAttention");

            migrationBuilder.DropTable(
                name: "Sn_Task_TaskRltFile");

            migrationBuilder.DropTable(
                name: "Sn_Task_TaskRltMember");

            migrationBuilder.DropTable(
                name: "Sn_Technology_ConstructInterfaceInfoRltMarkFile");

            migrationBuilder.DropTable(
                name: "Sn_Technology_Disclose");

            migrationBuilder.DropTable(
                name: "Sn_Technology_MaterialPlanFlowInfo");

            migrationBuilder.DropTable(
                name: "Sn_Technology_MaterialPlanRltMaterial");

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
                name: "Sn_App_Roles");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplateNode");

            migrationBuilder.DropTable(
                name: "Sn_Cms_Article");

            migrationBuilder.DropTable(
                name: "Sn_Cms_Category");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_Standard");

            migrationBuilder.DropTable(
                name: "Sn_Construction_MasterPlanContent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanMaterial");

            migrationBuilder.DropTable(
                name: "Sn_Construction_Daily");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_EquipmentTeam");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_Material");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_SubItemContentRltProcedure");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_Worker");

            migrationBuilder.DropTable(
                name: "Sn_CostManagement_Contract");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_MaintenanceWork");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_InfluenceRange");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_SkylightPlan");

            migrationBuilder.DropTable(
                name: "Sn_CrPlan_WorkTicket");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlan");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_Fault");

            migrationBuilder.DropTable(
                name: "Sn_File_OssServer");

            migrationBuilder.DropTable(
                name: "Sn_File_Tag");

            migrationBuilder.DropTable(
                name: "Sn_FileApprove_FileApprove");

            migrationBuilder.DropTable(
                name: "Sn_Material_Contract");

            migrationBuilder.DropTable(
                name: "Sn_Material_EntryRecord");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialAcceptance");

            migrationBuilder.DropTable(
                name: "Sn_Material_MaterialOfBill");

            migrationBuilder.DropTable(
                name: "Sn_Material_Inventory");

            migrationBuilder.DropTable(
                name: "Sn_Material_OutRecord");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchaseList");

            migrationBuilder.DropTable(
                name: "Sn_Material_PurchasePlan");

            migrationBuilder.DropTable(
                name: "Sn_Oa_DutySchedule");

            migrationBuilder.DropTable(
                name: "Sn_Problem_ProblemCategory");

            migrationBuilder.DropTable(
                name: "Sn_Problem_Problem");

            migrationBuilder.DropTable(
                name: "Sn_Project_Dossier");

            migrationBuilder.DropTable(
                name: "Sn_Oa_Contract");

            migrationBuilder.DropTable(
                name: "Sn_Project_Unit");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemLibrary");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblemRecord");

            migrationBuilder.DropTable(
                name: "Sn_Oa_Seal");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_Institution");

            migrationBuilder.DropTable(
                name: "Sn_Regulation_Label");

            migrationBuilder.DropTable(
                name: "Sn_Report_Report");

            migrationBuilder.DropTable(
                name: "Sn_Resource_ComponentRltQRCode");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreEquipmentTest");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreEquipmentTransfer");

            migrationBuilder.DropTable(
                name: "Sn_Resource_TerminalBusinessPath");

            migrationBuilder.DropTable(
                name: "Sn_Resource_CableCore");

            migrationBuilder.DropTable(
                name: "Sn_Resource_Terminal");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemLibrary");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblemRecord");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_Block");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_MVDProperty");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_IndividualProject");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProcessTemplate");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProjectItem");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_BasePrice");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_Quota");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RepairItem");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ModelFile");

            migrationBuilder.DropTable(
                name: "Sn_Task_Task");

            migrationBuilder.DropTable(
                name: "Sn_Technology_ConstructInterfaceInfo");

            migrationBuilder.DropTable(
                name: "Sn_Technology_MaterialPlan");

            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "Sn_App_IdentityServerApiResources");

            migrationBuilder.DropTable(
                name: "Sn_Construction_PlanContent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DailyTemplate");

            migrationBuilder.DropTable(
                name: "Sn_Construction_Dispatch");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_Procedure");

            migrationBuilder.DropTable(
                name: "Sn_Emerg_EmergPlanRecord");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_Section");

            migrationBuilder.DropTable(
                name: "Sn_Technology_Material");

            migrationBuilder.DropTable(
                name: "Sn_Material_Supplier");

            migrationBuilder.DropTable(
                name: "Sn_Material_Partition");

            migrationBuilder.DropTable(
                name: "Sn_Project_Archives");

            migrationBuilder.DropTable(
                name: "Sn_Project_FileCategory");

            migrationBuilder.DropTable(
                name: "Sn_Quality_QualityProblem");

            migrationBuilder.DropTable(
                name: "Sn_Safe_SafeProblem");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_BlockCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_MVDCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComputerCode");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_QuotaCategory");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_RepairGroup");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_Model");

            migrationBuilder.DropTable(
                name: "Sn_Project_Project");

            migrationBuilder.DropTable(
                name: "Sn_Technology_ConstructInterface");

            migrationBuilder.DropTable(
                name: "Sn_Construction_Plan");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_SubItemContent");

            migrationBuilder.DropTable(
                name: "Sn_Construction_DispatchTemplate");

            migrationBuilder.DropTable(
                name: "Sn_File_File");

            migrationBuilder.DropTable(
                name: "Sn_Project_ArchivesCategory");

            migrationBuilder.DropTable(
                name: "Sn_Project_BooksClassification");

            migrationBuilder.DropTable(
                name: "Sn_Resource_Equipment");

            migrationBuilder.DropTable(
                name: "Sn_Construction_MasterPlan");

            migrationBuilder.DropTable(
                name: "Sn_ConstructionBase_SubItem");

            migrationBuilder.DropTable(
                name: "Sn_File_Folder");

            migrationBuilder.DropTable(
                name: "Sn_Resource_CableExtend");

            migrationBuilder.DropTable(
                name: "Sn_Basic_InstallationSite");

            migrationBuilder.DropTable(
                name: "Sn_Resource_EquipmentGroup");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreEquipment");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_Workflow");

            migrationBuilder.DropTable(
                name: "Sn_Basic_Railway");

            migrationBuilder.DropTable(
                name: "Sn_Basic_Station");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ComponentCategory");

            migrationBuilder.DropTable(
                name: "Sn_App_Users");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_Manufacturer");

            migrationBuilder.DropTable(
                name: "Sn_StdBasic_ProductCategory");

            migrationBuilder.DropTable(
                name: "Sn_Resource_StoreHouse");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FlowTemplate");

            migrationBuilder.DropTable(
                name: "Sn_Common_Area");

            migrationBuilder.DropTable(
                name: "Sn_App_Organization");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_FormTemplate");

            migrationBuilder.DropTable(
                name: "Sn_App_DataDictionary");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowTemplate");

            migrationBuilder.DropTable(
                name: "Sn_Bpm_WorkflowTemplateGroup");
        }
    }
}
