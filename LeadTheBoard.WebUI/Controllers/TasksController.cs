using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Task;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Operator")]
    public class TasksController : BaseController<TasksController>
    {
        public IActionResult Index()
        {
            var task = UnitOfWork.TaskAssignments
                .Find(i => i.OperatorId == UserId)
                .Include(i => i.Operation)
                .Include(i => i.Operator)
                .ToList();

            var result = task.Select(x => new TaskListModel
            {
                TaskAssignmentId = x.Id,
                TaskName = x.Operation.Name,
                Point = x.Operation.Point,
                DifficultyLevel = x.Operation.DifficultyLevel,
                IsCompleted = x.IsCompleted
            }).ToList();

            return View(result);
        }

        public IActionResult Details(int id)
        {
            var task = UnitOfWork.TaskAssignments
                .Find(i => i.Id == id)
                .Include(i => i.Operation)
                .Include(i => i.Operator)
                .FirstOrDefault();

            var result = new TaskDetailModel()
            {
                CreatedAt = task.CreatedAt,
                DifficultyLevel = task.Operation?.DifficultyLevel ?? 0,
                MachineName = task.Operation?.Machine?.Name ?? "",
                OperatorName = task.Operator?.FullName ?? "",
                Point = task.Operation?.Point ?? 0,
                ProductName = task.Operation?.Product?.Name ?? "",
                TaskAssignmentId = task.Id,
                TaskName = task.Operation?.Name ?? "",
                UpdatedAt = task.UpdatedAt,
                OperationName = task.Operation?.Name ?? ""
            };
            return View(result);
        }

        public async Task<IActionResult> CompleteTask(int id)
        {
            var task = await UnitOfWork.TaskAssignments.GetByIdAsync(id);
            task.IsCompleted = true;
            task.CompletedDateTime = DateTime.Now;
            await UnitOfWork.TaskAssignments.UpdateAsync(task);
            await UnitOfWork.CommitAsync();

            //kullanıcı badge kazandı mı kontrol et

            //süresi geçmeyen bütün badge'leri getir
            var badges = await UnitOfWork.Badges
                .Find(i => i.ValidityDateStart <= DateTime.Now && i.ValidityDateEnd >= DateTime.Now)
                .ToListAsync();

            //daha öncesinden kazanılmış bütün badgeleri getir
            var userBadges = await UnitOfWork.UserBadges
                .Find(i => i.UserId == task.OperatorId)
                .ToListAsync();

            //tek tek bütün badgeleri kullanıcının bitirdiği tasklara bakarak kontrol et
            foreach (var badge in badges)
            {
                //kullanıcının badgenin validity date aralığında kazandığı bütün puanları topla
                var totalPoint = await UnitOfWork.TaskAssignments
                    .Find(i => i.OperatorId == task.OperatorId && i.CreatedAt >= badge.ValidityDateStart && i.CreatedAt <= badge.ValidityDateEnd)
                    .SumAsync(i => i.Operation.Point);

                //kullanıcının toplam puanı badgenin puanından büyükse kullanıcıya badge ver
                if (totalPoint >= badge.RequiredPoints)
                {
                    //daha önce kazanılmamışsa kullanıcıya badge ver
                    if (userBadges.Any(i => i.BadgeId == badge.Id))
                    {
                        continue;
                    }

                    var userBadge = new UserBadges()
                    {
                        BadgeId = badge.Id,
                        UserId = task.OperatorId
                    };
                    await UnitOfWork.UserBadges.AddAsync(userBadge);
                    await UnitOfWork.CommitAsync();
                }

            }

            return RedirectToAction("Index");
        }
    }
}
