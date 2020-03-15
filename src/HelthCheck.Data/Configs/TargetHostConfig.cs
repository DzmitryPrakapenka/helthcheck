using HelthCheck.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelthCheck.Data.Configs
{
    internal class TargetHostConfig : IEntityTypeConfiguration<TargetHost>
    {
        public void Configure(EntityTypeBuilder<TargetHost> builder)
        {
            builder.ToTable("TargetHosts");

            builder.HasKey(u => u.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.IP).HasMaxLength(256).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.LastModifiedDate).IsRequired();
        }
    }
}
