namespace HelthCheck.Web.Data
{
    public class Check : AuditableEntity
    {
        public int Id { get; set; }

        public int TargetHostId { get; set; }

        public string HelthCheckUrl { get; set; }

        public virtual TargetHost TargetHost { get; set; }
    }
}
