$excPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$npmRoot = ($excPath + "\..\")

# 切换到 modules 项目根目录
cd $npmRoot


# 更新版本
$commonPath = "../../../../tools/common.json"
$common = (Get-Content $commonPath -Raw) | ConvertFrom-Json 
$version =  $common.version


$webConfigPath = "./tools/web_config.temp.js"
$webConfig = (Get-Content $webConfigPath -Raw)
$time = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$webConfig = $webConfig.Replace("{version}", "v" + $version + " " + $time)
$webConfig | Set-Content -Path "./tools/web_config.js"


# # 编译
yarn dist


# # 返回根目录
cd ../../../../