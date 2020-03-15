using System;

namespace HelthCheck.Data.Entities
{
    public abstract class AuditableEntity : IAuditable
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
