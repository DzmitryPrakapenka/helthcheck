using System.Collections.Generic;

namespace HelthCheck.Data.Entities
{
    public class Check : AuditableEntity
    {
        public int Id { get; set; }

        public int TargetHostId { get; set; }

        public string HelthCheckUrl { get; set; }

        public string Cron { get; set; }

        public CheckStatus Status { get; set; }

        public virtual TargetHost TargetHost { get; set; }

        public virtual ICollection<CheckResult> CheckResults { get; set; } = new HashSet<CheckResult>();
    }
}
