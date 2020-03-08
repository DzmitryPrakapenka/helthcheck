using System;

namespace HelthCheck.Web.Data
{
    public abstract class AuditableEntity : IAuditable
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
