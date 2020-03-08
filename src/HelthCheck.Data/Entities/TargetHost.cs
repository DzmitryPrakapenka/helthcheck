using System.Collections.Generic;

namespace HelthCheck.Web.Data
{
    public class TargetHost : AuditableEntity
    {
        public int Id { get; set; }

        public string IP { get; set; }

        public ICollection<Check> Checks { get; set; } = new HashSet<Check>();
    }
}
