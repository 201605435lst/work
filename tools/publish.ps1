
Write-Host ("*******************************************************************") -ForegroundColor Green 
Write-Host ("****************       ��ˣ���ʼ���� (1/8)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************       ��ˣ�Զ�̷��� (2/8)       *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\server\aspnet*core\templates\app\aspnet*core\tools\publish.ps1


Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺�������� (3/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺���ط��� (4/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�飺Npm ���� (5/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\modules\tools\publish.ps1


Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�壺�������� (6/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\update.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�壺���ط��� (7/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\dist.ps1

Write-Host ("*******************************************************************") -ForegroundColor Green
Write-Host ("****************      Vue ģ�壺Զ�̷��� (8/8)    *****************") -ForegroundColor Green
Write-Host ("*******************************************************************") -ForegroundColor Green
.\client\web\vue\templates\tools\publish.ps1