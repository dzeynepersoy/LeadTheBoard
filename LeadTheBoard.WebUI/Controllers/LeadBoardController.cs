using LeadTheBoard.Application.Utilities.Helpers;
using LeadTheBoard.Shared.Models.LeadBoard;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize]
    public class LeadBoardController : BaseController<LeadBoardController>
    {
        public IActionResult Index(DateTime? filterStartDate, DateTime? filterEndDate)
        {
            if (filterStartDate == null)
            {
                filterStartDate = DateTimeHelper.GetStartOfWeek();
            }
            if (filterEndDate == null)
            {
                filterEndDate = DateTime.Now;
            }

            var users = UnitOfWork.Users
                .Find(i => i.TaskAssignments.Any(x => x.IsCompleted) && i.UsersAndRoles.First().Role.Name == "Operator" && i.CreatedAt >= filterStartDate && i.CreatedAt <= filterEndDate)
                .Include(i => i.UsersAndRoles)
                .ThenInclude(i => i.Role)
                .Include(i => i.TaskAssignments.Where(x => x.IsCompleted))
                .ThenInclude(i => i.Operation)
                .ToList();

            var leadBoardUserList = new List<LeadBoardUsersListModel>(capacity: users.Count);

            foreach (var user in users)
            {
                var points = user.TaskAssignments.Sum(i => i.Operation.Point);

                leadBoardUserList.Add(new LeadBoardUsersListModel
                {
                    UserId = user.Id,
                    Name = user.FullName,
                    Points = points,
                    UserImage = user.ImageUrl ?? "/img/avatar.png",
                    Rank = 0
                });

            }

            //sıralamasını güncelle
            leadBoardUserList = leadBoardUserList.OrderByDescending(i => i.Points).ToList();
            for (int i = 0; i < leadBoardUserList.Count; i++)
            {
                leadBoardUserList[i].Rank = i + 1;
            }

            var currentUser = leadBoardUserList.FirstOrDefault(i => i.UserId == UserId);

            var result = new LeadBoardModel
            {
                CurrentUser = currentUser ?? new(),

                Users = new LeadBoardUsersModel
                {
                    List = leadBoardUserList
                }
            };

            return View(result);
        }
    }
}
