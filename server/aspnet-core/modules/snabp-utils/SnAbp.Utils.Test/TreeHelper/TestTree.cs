using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.Test.TreeHelper
{
    public class TestTree : ICodeTree<TestTree>, IGuidKeyTree<TestTree>
    {
        public Guid Id { get; }


        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        public TestTree Parent { get; set; }
        public List<TestTree> Children { get; set; }

        public TestTree(Guid id)
        {
            Id = id;
        }
    }
}
