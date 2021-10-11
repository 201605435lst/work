using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Bpm.Dtos.Test
{
    public class TestB : Test
    {
        public List<string> Value { get; set; }
        public int PropB { get; set; }
        public TestA TestA { get; set; }
    }
}
