net use \\172.16.1.22\ipc$ serverCRMS123 /user:LIPAN-PC\administrator

$excPath = Split-Path -Parent $MyInvocation.MyCommand.Definition


# 拷贝维护页面 App_Offline.htm
xcopy ($excPath + "\App_Offline.htm") "\\172.16.1.22\F$\wwwroot\SnAbp_NanJing_Ditie\api\" /D /E /Y /H /K

# 拷贝程序
xcopy ($excPath + "\..\src\MyCompanyName.MyProjectName.HttpApi.Host\bin\Release\netcoreapp3.1\publish\*.*")  "\\172.16.1.22\F$\wwwroot\SnAbp_NanJing_Ditie\api\" /D /E /Y /H /K

# # 覆盖配置文件
del "\\172.16.1.22\F$\wwwroot\SnAbp_NanJing_Ditie\api\appsettings.json"
xcopy ($excPath + "\appsettings.json") "\\172.16.1.22\F$\wwwroot\SnAbp_NanJing_Ditie\api\" /D /E /Y /H /K

# # 删除维护页面 App_Offline.htm
del "\\172.16.1.22\F$\wwwroot\SnAbp_NanJing_Ditie\api\App_Offline.htm" 


net use \\172.16.1.22\ipc$ /delete

