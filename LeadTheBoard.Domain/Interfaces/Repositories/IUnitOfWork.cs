using LeadTheBoard.Domain.Entities;

namespace LeadTheBoard.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Badge> Badges { get; }
        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<DepartmentAndProduct> DepartmentsAndProducts { get; }
        public IGenericRepository<Machine> Machines { get; }
        public IGenericRepository<Operation> Operations { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Role> Roles { get; }
        public IGenericRepository<TaskAssignment> TaskAssignments { get; }
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<UserAndRole> UsersAndRoles { get; }
        public IGenericRepository<UserBadges> UserBadges { get; }

        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
