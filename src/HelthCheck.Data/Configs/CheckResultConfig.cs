using HelthCheck.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelthCheck.Data.Configs
{
    internal class CheckResultConfig : IEntityTypeConfiguration<CheckResult>
    {
        public void Configure(EntityTypeBuilder<CheckResult> builder)
        {
            builder.ToTable("CheckResults");

            builder.HasKey(u => u.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.LastModifiedDate).IsRequired();

            builder.HasOne(p => p.Check)
                .WithMany(c => c.CheckResults)
                .HasForeignKey(p => p.CheckId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
