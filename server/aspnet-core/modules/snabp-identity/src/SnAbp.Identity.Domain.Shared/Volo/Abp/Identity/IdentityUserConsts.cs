using SnAbp.Users;

namespace SnAbp.Identity
{
    public static class IdentityUserConsts
    {
        public static int MaxUserNameLength { get; set; } = SnAbpUserConsts.MaxUserNameLength;

        public static int MaxNameLength { get; set; } = SnAbpUserConsts.MaxNameLength;

        public static int MaxSurnameLength { get; set; } = SnAbpUserConsts.MaxSurnameLength;

        public static int MaxNormalizedUserNameLength { get; set; } = MaxUserNameLength;

        public static int MaxEmailLength { get; set; } = SnAbpUserConsts.MaxEmailLength;

        public static int MaxNormalizedEmailLength { get; set; } = MaxEmailLength;

        public static int MaxPhoneNumberLength { get; set; } = SnAbpUserConsts.MaxPhoneNumberLength;

        /// <summary>
        /// Default value: 128
        /// </summary>
        public static int MaxPasswordLength { get; set; } = 128;

        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxPasswordHashLength { get; set; } = 256;

        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxSecurityStampLength { get; set; } = 256;
    }
}