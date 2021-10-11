using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace SnAbp.Users
{
    public class AbpUsersExternalTestData : ISingletonDependency
    {
        public IAbpUserData David { get; }
        public IAbpUserData Neo { get; }

        public AbpUsersExternalTestData(AbpUsersLocalTestData localTestData)
        {
            Neo = new AbpUserData(Guid.NewGuid(), "neo");
            David = localTestData.David.ToAbpUserData();
        }

        public List<IAbpUserData> GetAllUsers()
        {
            return new List<IAbpUserData>
            {
                David,
                Neo
            };
        }
    }
}