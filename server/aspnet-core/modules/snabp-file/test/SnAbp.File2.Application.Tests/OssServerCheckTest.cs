/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： OssServerCheckTest
*******类 说 明： 方法测试
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 16:11:56
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Xunit;

namespace SnAbp.File
{
    public class OssServerCheckTest
    {
        private class AAAA
        {
            public string Name { get; set; }
            public string Old { get; set; }
        }

        private class BBBB
        {
            public string Name { get; set; }
        }

        [Fact]
        public void FolderPathTest()
        {
            var list = new List<string>();
            list.Add("文件夹3");
            list.Add("文件夹2");
            list.Add("文件夹1");


            list.Reverse();
            var str = list.JoinAsString(@"\");
        }

        [Fact]
        public void MapperTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<AAAA>, List<BBBB>>();
                cfg.CreateMap<List<BBBB>, List<AAAA>>();
                cfg.CreateMap<List<BBBB>, List<AAAA>>();
                cfg.CreateMap<BBBB, AAAA>();
                cfg.CreateMap<AAAA, BBBB>();
            });
            // configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();

            var listA = new List<AAAA>();
            listA.Add(new AAAA {Name = "aaaa", Old = "aa"});

            var listB = mapper.Map<List<BBBB>>(listA);
        }

        [Fact]
        public void NodeTest()
        {

            var str = "12345678";
            var a = str.Substring(0, 4);







            var list = new List<string>();
            list.Add("0001");
            list.Add("00010001");
            list.Add("000100010001");
            list.Add("000100010002");
            list.Add("0001000100010001");
            list.Add("0001000100010002");
            list.Add("0001000100010003");
            list.Add("0001000100020001");
            list.Add("0001000100020002");
            list.Add("000100020001");
            list.Add("000100020002");

            list.Add("0002");
            list.Add("00020001");

            var nodes = new List<Node>();

            list.ForEach(a =>
            {
                if (a.Length == 4)
                {
                    var node = new Node();
                    node.Code = a;
                    NodeChildren(list, node);
                    nodes.Add(node);
                }
            });
            
        }

        private void NodeChildren(List<string> list,Node node)
        {
            node.Nodes=new List<Node>();
            list?.ForEach(a =>
            {
                if (a.Length > node.Code.Length)
                {
                    var code = $"{node.Code}{a.Substring(node.Code.Length, 4)}";
                    if (code == a)
                    {
                        var n = new Node();
                        n.Code = a;
                        NodeChildren(list, n);
                        node.Nodes.Add(n);
                    }
                }
            });
        }

        class Node
        {
            public  string Code { get; set; }
            public  List<Node> Nodes { get; set; }
        }
    }
}