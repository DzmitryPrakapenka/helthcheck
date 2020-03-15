using System;

namespace HelthCheck.Data.Entities
{
    public class CheckResult : AuditableEntity
    {
        public Guid Id { get; set; }

        public int CheckId { get; set; }

        public CheckResultStatus Status { get; set; }

        public virtual Check Check { get; set; }
    }
}
