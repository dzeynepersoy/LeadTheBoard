using LeadTheBoard.Application.Contexts;
using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Domain.Interfaces.Repositories;

namespace LeadTheBoard.Application.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _appDbContext;

        private readonly IGenericRepository<Badge> _badges;
        private readonly IGenericRepository<Department> _departments;
        private readonly IGenericRepository<DepartmentAndProduct> _departmentsAndProducts;
        private readonly IGenericRepository<Machine> _machines;
        private readonly IGenericRepository<Operation> _operations;
        private readonly IGenericRepository<Product> _products;
        private readonly IGenericRepository<Role> _roles;
        private readonly IGenericRepository<TaskAssignment> _taskAssignments;
        private readonly IGenericRepository<User> _users;
        private readonly IGenericRepository<UserAndRole> _usersAndRoles;
        private readonly IGenericRepository<UserBadges> _userBadges;

        public UnitOfWork(IGenericRepository<Badge> badges, IGenericRepository<Department> departments, IGenericRepository<DepartmentAndProduct> departmentsAndProducts, IGenericRepository<Machine> machines, IGenericRepository<Operation> operations, IGenericRepository<Product> products, IGenericRepository<Role> roles, IGenericRepository<TaskAssignment> taskAssignments, IGenericRepository<User> users, IGenericRepository<UserAndRole> usersAndRoles, AppDbContext appDbContext, IGenericRepository<UserBadges> userBadges)
        {
            _badges = badges;
            _departments = departments;
            _departmentsAndProducts = departmentsAndProducts;
            _machines = machines;
            _operations = operations;
            _products = products;
            _roles = roles;
            _taskAssignments = taskAssignments;
            _users = users;
            _usersAndRoles = usersAndRoles;
            _appDbContext = appDbContext;
            _userBadges = userBadges;
        }

        public IGenericRepository<Badge> Badges => _badges;

        public IGenericRepository<Department> Departments => _departments ;

        public IGenericRepository<DepartmentAndProduct> DepartmentsAndProducts => _departmentsAndProducts;

        public IGenericRepository<Machine> Machines => _machines;

        public IGenericRepository<Operation> Operations => _operations;

        public IGenericRepository<Product> Products => _products;

        public IGenericRepository<Role> Roles => _roles;

        public IGenericRepository<TaskAssignment> TaskAssignments => _taskAssignments;

        public IGenericRepository<User> Users => _users;

        public IGenericRepository<UserAndRole> UsersAndRoles => _usersAndRoles;

        public IGenericRepository<UserBadges> UserBadges => _userBadges;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            //eğer kayıt işlemi başarılı olursa veritabanına kayıt edilir.
            //başarısız olursa geri alınır.

            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                await _appDbContext.SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Dispose edilip edilmediğini tutar.
        /// </summary>
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Veritabanı için oluşturulmuş DbContext i Dispose eder. (Dispose = kullanılan kaynağı serbest bırakır.)
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
