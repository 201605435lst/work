using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public static class IdentityEfCoreQueryableExtensions
    {
        public static IQueryable<IdentityUser> IncludeDetails(this IQueryable<IdentityUser> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Roles)
                .Include(x => x.Logins)
                .Include(x => x.Claims)
                .Include(x => x.Tokens)
                .Include(x => x.Position)
                .Include(x => x.Organizations)?.ThenInclude(a => a.Organization);
        }

        public static IQueryable<IdentityRole> IncludeDetails(this IQueryable<IdentityRole> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Claims);
        }

        public static IQueryable<Organization> IncludeDetails(this IQueryable<Organization> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Roles)
                .Include(a => a.Children)
                .Include((x => x.Type));
        }
    }
}