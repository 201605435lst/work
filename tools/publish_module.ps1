
Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：依赖更新 (1/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：本地发布 (2/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模块：Npm 发布 (3/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\publish.ps1