
Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺�������� (1/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺���ط��� (2/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺Npm ���� (3/3)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\publish.ps1