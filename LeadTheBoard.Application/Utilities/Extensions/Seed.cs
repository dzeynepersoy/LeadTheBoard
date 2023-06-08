using LeadTheBoard.Application.Contexts;
using LeadTheBoard.Application.Utilities.Helpers;
using LeadTheBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTheBoard.Application.Utilities.Extensions
{
    public static class Seed
    {
        public static async ValueTask SeedDataAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //Eğer veritabanı yoksa oluşturur.
            await appDbContext.Database.MigrateAsync();

            if (!await appDbContext.Users.AnyAsync() && !await appDbContext.Roles.AnyAsync())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        Email = "admin@email.com",
                        ImageUrl="/Uploads/user.png",
                        FullName = "Admin",
                        LastActivity = DateTime.UtcNow,
                        Password = "123456",

                    },
                    new User()
                    {
                        Email = "manager@email.com",
                        ImageUrl="/Uploads/user.png",
                        FullName = "Manager",
                        LastActivity = DateTime.UtcNow,
                        Password="123456"
                    },
                    new User()
                    {
                        Email = "operator@email.com",
                        ImageUrl="/Uploads/user.png",
                        FullName = "Operator",
                        LastActivity = DateTime.UtcNow,
                        Password="123456"
                    }
                };

                await appDbContext.Users.AddRangeAsync(users);

                var roles = new List<Role>()
                {
                    new Role()
                    {
                        Name = "Admin",
                    },
                    new Role()
                    {
                        Name = "Manager",
                    },
                    new Role()
                    {
                        Name = "Operator",
                    }
                };

                await appDbContext.Roles.AddRangeAsync(roles);

                await appDbContext.SaveChangesAsync();

                await appDbContext.UsersAndRoles.AddRangeAsync(new List<UserAndRole>()
                {
                    new UserAndRole()
                    {
                        RoleId = roles.First(i => i.Name == "Admin").Id,
                        UserId = users.First(i => i.Email == "admin@email.com").Id
                    },
                    new UserAndRole()
                    {
                        RoleId = roles.First(i => i.Name == "Manager").Id,
                        UserId = users.First(i => i.Email == "manager@email.com").Id
                    },
                    new UserAndRole()
                    {
                        RoleId = roles.First(i => i.Name == "Operator").Id,
                        UserId = users.First(i => i.Email == "operator@email.com").Id
                    }
                });

                await appDbContext.SaveChangesAsync();
            }

            //eğer badge tablosunda kayıt yoksa aşağıdaki kayıtları ekler.
            if (!await appDbContext.Badges.AnyAsync())
            {
                await appDbContext.Badges.AddRangeAsync(new List<Badge>()
                {
                    new Badge { Name = "Ace Up",Description="Earn 90.000 Points",RequiredPoints=90000,ImageUrl="/Uploads/ace-up.png",ValidityDateStart=DateTime.UtcNow,ValidityDateEnd=DateTime.UtcNow.AddDays(15) },

                    new Badge { Name = "Legend",Description="Earn 500.000 Points",RequiredPoints=500000,ImageUrl="/Uploads/legend.png",ValidityDateStart=DateTime.UtcNow,ValidityDateEnd=DateTime.UtcNow.AddDays(15) },

                    new Badge { Name = "Point Collector",Description="Earn 150.000 Points",RequiredPoints=150000,ImageUrl="/Uploads/point-collector.png",ValidityDateStart=DateTime.UtcNow,ValidityDateEnd=DateTime.UtcNow.AddDays(15) },

                    new Badge { Name = "Winner Of Week",Description="Earn 10.000 Points",RequiredPoints=10000,ImageUrl="/Uploads/winner-of-week.png",ValidityDateStart=DateTime.UtcNow,ValidityDateEnd=DateTime.UtcNow.AddDays(15) },

                    new Badge { Name = "Work Hard",Description="Earn 50.000 Points",RequiredPoints=50000,ImageUrl="/Uploads/work-hard.png",ValidityDateStart=DateTime.UtcNow,ValidityDateEnd=DateTime.UtcNow.AddDays(15) },

                });
            }

            await appDbContext.SaveChangesAsync();

        }
    }
}
