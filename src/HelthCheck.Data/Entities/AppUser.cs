using System;
using Microsoft.AspNetCore.Identity;

namespace HelthCheck.Data.Entities
{
    public class AppUser : IdentityUser<int>, IAuditable
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
