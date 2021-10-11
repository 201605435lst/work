using System;
using Volo.Abp.DependencyInjection;

namespace SnAbp.FeatureManagement
{
    public class FeatureManagementTestData : ISingletonDependency
    {
        public Guid User1Id { get; } = Guid.NewGuid();
    }
}
