using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SnAbp.Users.EntityFrameworkCore
{
    public static class SnAbpUsersDbContextModelCreatingExtensions
    {
        public static void ConfigureAbpUser<TUser>(this EntityTypeBuilder<TUser> b)
            where TUser : class, IUser
        {
            b.Property(u => u.TenantId).HasColumnName(nameof(IUser.TenantId));
            b.Property(u => u.UserName).IsRequired().HasMaxLength(SnAbpUserConsts.MaxUserNameLength).HasColumnName(nameof(IUser.UserName));
            b.Property(u => u.Email).HasMaxLength(SnAbpUserConsts.MaxEmailLength).HasColumnName(nameof(IUser.Email));
            b.Property(u => u.Name).HasMaxLength(SnAbpUserConsts.MaxNameLength).HasColumnName(nameof(IUser.Name));
            b.Property(u => u.Surname).HasMaxLength(SnAbpUserConsts.MaxSurnameLength).HasColumnName(nameof(IUser.Surname));
            b.Property(u => u.EmailConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.EmailConfirmed));
            b.Property(u => u.PhoneNumber).HasMaxLength(SnAbpUserConsts.MaxPhoneNumberLength).HasColumnName(nameof(IUser.PhoneNumber));
            b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.PhoneNumberConfirmed));
        }
    }
}