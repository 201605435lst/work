using JetBrains.Annotations;

namespace SnAbp.Users
{
    public interface IUpdateUserData
    {
        bool Update([NotNull] IUserData user);
    }
}