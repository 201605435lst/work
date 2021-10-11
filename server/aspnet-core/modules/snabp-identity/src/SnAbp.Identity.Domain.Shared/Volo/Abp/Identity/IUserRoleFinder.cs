using System;
using System.Threading.Tasks;

namespace SnAbp.Identity
{
    public interface IUserRoleFinder
    {
        Task<string[]> GetRolesAsync(Guid userId);
    }
}