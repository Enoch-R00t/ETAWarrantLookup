using Microsoft.AspNetCore.Identity;

namespace ETAWarrantLookup.Models
{
    /// <summary>
    /// This model extends the default IDentityUser properties
    /// </summary>
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
    }
}
