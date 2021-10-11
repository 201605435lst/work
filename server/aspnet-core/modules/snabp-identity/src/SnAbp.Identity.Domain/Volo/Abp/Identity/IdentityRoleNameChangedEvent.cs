using System;

namespace SnAbp.Identity
{
    public class IdentityRoleNameChangedEvent
    {
        public IdentityRole IdentityRole { get; set; }
        public string OldName { get; set; }

        public Guid OldGuid { get; set; }
    }
}