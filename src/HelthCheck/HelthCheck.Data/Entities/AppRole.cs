using System;
using Microsoft.AspNetCore.Identity;

namespace HelthCheck.Web.Data
{
    public class AppRole : IdentityRole<int>, IAuditable
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
