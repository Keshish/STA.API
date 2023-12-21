using Microsoft.AspNetCore.Identity;
using STA.API.Models.Users;

namespace STA.API.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public  Supervisor Supervisor { get; set; }

        public  Assistant Assistant { get; set; }

        public  Parent Parent { get; set; }
    }
}
