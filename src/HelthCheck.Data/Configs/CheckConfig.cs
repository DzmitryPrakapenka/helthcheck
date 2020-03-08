using HelthCheck.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelthCheck.Data.Configs
{
    internal class CheckConfig : IEntityTypeConfiguration<Check>
    {
        public void Configure(EntityTypeBuilder<Check> builder)
        {
            builder.ToTable("Checks");

            builder.HasKey(u => u.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.HelthCheckUrl).HasMaxLength(256).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.LastModifiedDate).IsRequired();

            builder.HasOne(p => p.TargetHost)
                .WithMany(c => c.Checks)
                .HasForeignKey(p => p.TargetHostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
