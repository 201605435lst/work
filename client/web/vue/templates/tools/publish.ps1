net use \\172.16.1.22\ipc$ serverCRMS123 /user:LIPAN-PC\administrator

$excPath = Split-Path -Parent $MyInvocation.MyCommand.Definition


# 清空旧版本
del "\\172.16.1.22\F$\wwwroot\SnAbp_ShiGong\api\wwwroot\*" -recurse

# 拷贝新版本
xcopy ($excPath + "\..\dist\*")  "\\172.16.1.22\F$\wwwroot\SnAbp_ShiGong\api\wwwroot\" /D /E /Y /H /K

# 覆盖配置文件 webconfig
del "\\172.16.1.22\F$\wwwroot\SnAbp_ShiGong\api\wwwroot\web_config.js"
xcopy ($excPath + "\web_config.js") "\\172.16.1.22\F$\wwwroot\SnAbp_ShiGong\api\wwwroot\" /D /E /Y /H /K


net use \\172.16.1.22\ipc$ /delete