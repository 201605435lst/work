using System.ComponentModel;

namespace SnAbp.Safe
{
    /// <summary>
    /// $$
    /// </summary>
    public enum SafetyRiskLevel
    {
        [Description("特别重大事故")]
        Especially = 1,
        [Description("重大事故")]
        Great = 2,
        [Description("较大事故")]
        Larger = 3,
        [Description("一般事故")]
        General = 4
    }
}