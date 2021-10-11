namespace SnAbp.Identity
{
    public static class OrganizationConsts
    {
        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public static int MaxDisplayNameLength { get; set; } = 128;

        /// <summary>
        /// Maximum depth of an OU hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 4;

        /// <summary>
        /// Maximum length of the <see cref="Code"/> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
    }
}