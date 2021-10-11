
Write-Host ("*******************************************************************") -ForegroundColor Green 
Write-Host ("****************       后端：开始编译 (1/8)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************       后端：远程发布 (2/8)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\publish.ps1


Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：依赖更新 (3/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：本地发布 (4/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：Npm 发布 (5/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\publish.ps1


Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：依赖更新 (6/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：本地发布 (7/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：远程发布 (8/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\publish.ps1