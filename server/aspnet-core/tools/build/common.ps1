# COMMON PATHS

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions

$solutionPaths = (
	# 系统模块
	"../../modules/snabp-framework",
	"../../modules/snabp-utils",
	"../../modules/snabp-users",
	"../../modules/snabp-permission-management",
	"../../modules/snabp-setting-management",
	"../../modules/snabp-feature-management",
	"../../modules/snabp-identity",
	"../../modules/snabp-identityserver",
	"../../modules/snabp-tenant-management",
	"../../modules/snabp-account",
	"../../modules/audit-logging",
	# 公共数据模块
	"../../modules/snabp-common",
	# 应用模块
	"../../modules/snabp-message",
	"../../modules/snabp-message-notice",	
	"../../modules/snabp-alarm",
	"../../modules/snabp-file",
	"../../modules/snabp-bpm",
	"../../modules/snabp-message-bpm",
	"../../modules/snabp-exam",
	"../../modules/snabp-cms",
	"../../modules/snabp-std-basic",
	"../../modules/snabp-basic",
	"../../modules/snabp-resource",
	"../../modules/snabp-emerg",
	"../../modules/snabp-problem",
	"../../modules/snabp-cr-plan",
	"../../modules/snabp-oa",
	"../../modules/snabp-project",
	"../../modules/snabp-task",
	"../../modules/snabp-regulation",
	"../../modules/snabp-report",
	"../../modules/snabp-material",
	"../../modules/snabp-technology",
	"../../modules/snabp-costmanagement",
	"../../modules/snabp-quality",
	"../../modules/snabp-safe",
		"../../modules/snabp-construction-base",
		"../../modules/snabp-construction",
		"../../modules/snabp-file-approve",
	# 示例应用
	"../../templates/app/aspnet-core"
)
