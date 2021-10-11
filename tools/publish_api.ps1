
Write-Host ("*******************************************************************") -ForegroundColor Green 
Write-Host ("****************       后端：开始编译 (1/2)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************       后端：远程发布 (2/2)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\publish.ps1
