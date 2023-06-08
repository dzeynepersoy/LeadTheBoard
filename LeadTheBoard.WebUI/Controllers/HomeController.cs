using LeadTheBoard.Application.Utilities.Helpers;
using LeadTheBoard.Shared.Models.MainPage;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize]
    public class HomeController : BaseController<HomeController>
    {
        public IActionResult Index()
        {
            var users = UnitOfWork.Users
                .Find()
                .Include(i => i.UsersAndRoles)
                .ThenInclude(i => i.Role)
                .ToList();

            var tasks = UnitOfWork.TaskAssignments.Find().ToList();

            var products = UnitOfWork.Products.Find().ToList();

            var machines = UnitOfWork.Machines.Find().ToList();

            var result = new MainPageModel()
            {
                Users = new MainPageUserModel()
                {
                    AdminsTotalCount = users.Count(i => i.UsersAndRoles.First().Role.Name == "Admin"),
                    ManagersTotalCount = users.Count(i => i.UsersAndRoles.First().Role.Name == "Manager"),
                    OperatorsTotalCount = users.Count(i => i.UsersAndRoles.First().Role.Name == "Operator"),
                },

                Machines = new MainPageMachinesModel()
                {
                    MachineTotalCount = machines.Count,
                },

                Products = new MainPageProductModel()
                {
                    ProductTotalCount = products.Count,
                },

                Tasks = new MainPageTaskModel()
                {
                    ActiveTaskTotalCount = tasks.Count(i => !i.IsCompleted),
                    CompletedTaskTotalCount = tasks.Count(i => i.IsCompleted),
                },

                //bu haftada bitirilen taskların istatistikleri
                TaskStatistics = new MainPageTaskStatistics()
                {
                    List = UnitOfWork.TaskAssignments
                        .Find()
                        .Where(i => i.IsCompleted && i.CreatedAt >= DateTimeHelper.GetStartOfWeek())
                        .GroupBy(i => i.CreatedAt.Date)
                        .Select(i => new MainPageTaskStatisticsListModel()
                        {
                            Date = i.Key,
                            CompletedTotalCount = i.Count()
                        })
                        .ToList()
                }

            };

            return View(result);
        }

    }
}