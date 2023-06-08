using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Operation;
using LeadTheBoard.Shared.Models.TaskAssignment;
using LeadTheBoard.Shared.Models.User;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class TaskAssignmentController : BaseController<TaskAssignmentController>
    {
        public IActionResult Index()
        {
            var operators = UnitOfWork.Users
                .Find(i => i.UsersAndRoles.Any(x => x.Role.Name == "Operator"))
                .Include(i => i.UsersAndRoles)
                .ThenInclude(i => i.Role)
                .ToList();

            ViewBag.Operators = operators.Select(i => new UserListModel()
            {
                Email = i.Email,
                FullName = i.FullName,
                Id = i.Id,
            }).ToList();

            var operations = UnitOfWork.Operations.Find().ToList();

            ViewBag.Operations = operations.Select(i => new OperationListModel()
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();

            var taskAssignments = UnitOfWork.TaskAssignments
                .Find()
                .Include(i => i.Operator)
                .Include(i => i.Operation)
                .OrderByDescending(i => i.Id)
                .ToList();

            var result = taskAssignments.Select(i => new TaskAssignmentListModel
            {
                Id = i.Id,

                OperatorId = i.OperatorId,
                OperatorName = UnitOfWork.Users.Find(u => u.Id == i.OperatorId).FirstOrDefault()?.FullName ?? "",

                OperationId = i.OperationId,
                OperationName = UnitOfWork.Operations.Find(o => o.Id == i.OperationId).FirstOrDefault()?.Name ?? "",

                IsDone = i.IsCompleted,
                CreatedAt = i.CreatedAt
            }).ToList();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(TaskAssignmentModel model)
        {
            //bu bir edit işlemi
            if (model.Id > 0)
            {
                var taskAssignment = await UnitOfWork.TaskAssignments.GetByIdAsync(model.Id);
                if (taskAssignment == null)
                {
                    return NotFound();
                }

                taskAssignment.OperatorId = model.OperatorId;
                taskAssignment.OperationId = model.OperationId;

                await UnitOfWork.TaskAssignments.UpdateAsync(taskAssignment);
                await UnitOfWork.CommitAsync();

            }
            //bu bir create işlemi
            else
            {
                await UnitOfWork.TaskAssignments.AddAsync(new TaskAssignment()
                {
                    OperatorId = model.OperatorId,
                    OperationId = model.OperationId,
                    IsCompleted = false,
                });

                await UnitOfWork.CommitAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var taskAssignment = await UnitOfWork.TaskAssignments.GetByIdAsync(id);
            if (taskAssignment == null)
            {
                return NotFound();
            }
            await UnitOfWork.TaskAssignments.DeleteAsync(taskAssignment);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
