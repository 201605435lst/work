/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： OssClientTest
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 10:43:21
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using SnAbp.File.OssSdk;
using SnAbp.File.OssSdk.Client;
using Xunit;

namespace SnAbp.File
{
    public class OssClientTest
    {
        [Fact]
        public void Create()
        {
            var option = new OssOption
            {
                EndPoint = "oss-cn-hongkong.aliyuncs.com",
                SecretKey = "AO331w0IYVnm30vxg1D6VQAkTxrbOc",
                AccessKey = "LTAI4G6PHe9KeuK5WibmJLr9",
                SecurityBucket = "easten-test",
                PublicBucket = "easten-test",
                Type = OssServerType.Aliyun
            };

            var client = OssAbstractClient.CreateClient(option);

            try
            {
                //var url =$"{option.PublicBucket}.{client.GetPresignedUrl("1111111_看图王.png").Result}";
                var url = $"{option.PublicBucket}.{client.GetObjectUrlAsync("1111111_看图王.png").Result}";
            }
            catch (Exception ex)
            {
            }
        }

        [Fact]
        public void OrganizationTest()
        {
            var list = new List<OrgModel>();
            list.Add(new OrgModel(){code = "00010001"});
            list.Add(new OrgModel(){code = "000100010001"});
            list.Add(new OrgModel(){code = "0001000100010001"});
            list.Add(new OrgModel(){code = "0001000100020001"});
            list.Add(new OrgModel(){code = "00010002"});
            list.Add(new OrgModel(){code = "000100020001"});
            list.Add(new OrgModel(){code = "0001000200010001"});
            list.Add(new OrgModel(){code = "000100020002"});
            list.Add(new OrgModel(){code = "0001000200020001"});
            list.Add(new OrgModel(){code = "00010003"});
            list.Add(new OrgModel(){code = "000100030001"});
            list.Add(new OrgModel(){code = "0001000300010001"});
            list.Add(new OrgModel(){code = "0001000300010002"});
            list.Add(new OrgModel(){code = "0001000300010003"});
            list.Add(new OrgModel(){code = "000100030002"});
            list.Add(new OrgModel(){code = "0001000300020001"});
            list.Add(new OrgModel(){code = "0001000300020002"});
            list.Add(new OrgModel(){code = "0001000300020003"});

            // 先获取当前已有数据的第一级
            var data = list.GroupBy(a => a.code.Length).OrderBy(b => b.Key).Select(a =>a.ToList()).FirstOrDefault();



        }

        class OrgModel
        {
            public  string code { get; set; }
        }
    }
}