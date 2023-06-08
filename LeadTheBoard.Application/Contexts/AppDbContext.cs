using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace LeadTheBoard.Application.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries()
            .Where(t => t.State == EntityState.Added)
            .Select(t => t.Entity)
            .ToArray();

            foreach (var entity in added)
            {
                if (entity is BaseEntity track)
                {
                    track.CreatedAt = DateTime.Now;
                }
            }

            var modified = ChangeTracker.Entries()
            .Where(t => t.State == EntityState.Modified)
            .Select(t => t.Entity)
            .ToArray();

            foreach (var entity in modified)
            {
                if (entity is BaseEntity track)
                {
                    track.UpdatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Badge> Badges { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentAndProduct> DepartmentsAndProducts { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAndRole> UsersAndRoles { get; set; }
        public DbSet<UserBadges> UserBadges { get; set; }

    }
}
