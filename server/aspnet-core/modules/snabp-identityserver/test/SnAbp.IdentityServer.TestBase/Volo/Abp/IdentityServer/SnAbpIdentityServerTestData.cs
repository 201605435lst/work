using System;
using Volo.Abp.DependencyInjection;

namespace SnAbp.IdentityServer
{
    public class SnAbpIdentityServerTestData : ISingletonDependency
    {
        public Guid Client1Id { get; } = Guid.NewGuid();

        public Guid ApiResource1Id { get; } = Guid.NewGuid();

        public Guid IdentityResource1Id { get; } = Guid.NewGuid();
    }
}
