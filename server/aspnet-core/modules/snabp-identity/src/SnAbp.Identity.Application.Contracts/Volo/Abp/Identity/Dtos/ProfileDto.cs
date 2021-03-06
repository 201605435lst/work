using Volo.Abp.ObjectExtending;

namespace SnAbp.Identity
{
    public class ProfileDto : ExtensibleObject
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }
    }
}