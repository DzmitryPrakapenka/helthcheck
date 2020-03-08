using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelthCheck.Web.Data
{
    public class ApplicationContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Check> CheckList { get; set; }

        public virtual DbSet<TargetHost> TargetHosts { get; set; }

        #region override

        public override int SaveChanges()
        {
            DoAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DoAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

        private void DoAudit()
        {
            var auditableEntities = ChangeTracker.Entries<IAuditable>();

            foreach (var entity in auditableEntities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedDate = DateTime.UtcNow;
                    entity.Entity.LastModifiedDate = DateTime.UtcNow;
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Entity.LastModifiedDate = DateTime.UtcNow;
                }
            }
        }

        #endregion
    }
}
