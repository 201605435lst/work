Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：依赖更新 (1/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：本地发布 (2/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue 模板：远程发布 (3/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\publish.ps1