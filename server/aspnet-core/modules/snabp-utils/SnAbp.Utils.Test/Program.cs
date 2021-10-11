using SnAbp.Utils.Test.TreeHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;

namespace SnAbp.Utils.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var t1 = new TestTree(Guid.NewGuid()) { Code = "01", ParentId = Guid.NewGuid() };
            var t2 = new TestTree(Guid.NewGuid()) { Code = "01.01", ParentId = t1.Id };
            var t3 = new TestTree(Guid.NewGuid()) { Code = "01.01.01", ParentId = t2.Id };
            var t32 = new TestTree(Guid.NewGuid()) { Code = "01.01.02", ParentId = t2.Id };
            var t4 = new TestTree(Guid.NewGuid()) { Code = "01.01.01.01", ParentId = t3.Id };

            var list = new List<TestTree>() { t1, t2, t3, t32, t4 };

            var parentsWithSibling = GuidKeyTreeHelper<TestTree>.GetParents(list, t3);
            var parents = GuidKeyTreeHelper<TestTree>.GetParents(list, t3, false);
            var parents4 = GuidKeyTreeHelper<TestTree>.GetParents(list, t4, false);
            var parents5 = GuidKeyTreeHelper<TestTree>.GetParents(list, t4);
            var parents6 = GuidKeyTreeHelper<TestTree>.GetParents(list, t1);

            var codeTreeList = CodeTreeHelper<TestTree>.GetTree(list, ".");
            var guidKeyTreeList = GuidKeyTreeHelper<TestTree>.GetTree(list);

            Test();

        }

        static void Test()
        {
            var t1 = new TestTree(Guid.NewGuid()) { Code = "01" };
            var t2 = new TestTree(Guid.NewGuid()) { Code = "02" };
            var t3 = new TestTree(Guid.NewGuid()) { Code = "03"};
       

            var list = new List<TestTree>() { t1, t2, t3 };
            var parents6 = GuidKeyTreeHelper<TestTree>.GetParents(list, t3);

        }
    }
}
