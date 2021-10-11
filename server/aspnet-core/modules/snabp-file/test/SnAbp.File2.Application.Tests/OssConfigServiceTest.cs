/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： OssConfigServiceTest
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 18:00:32
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using SnAbp.File.IServices;

namespace SnAbp.File
{
    public class OssConfigServiceTest : File2ApplicationTestBase
    {
        private readonly IFile2OssConfigAppService _ossService;

        public OssConfigServiceTest()
        {
            _ossService = GetRequiredService<IFile2OssConfigAppService>();
        }
    }
}