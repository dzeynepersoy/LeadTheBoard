using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Badge;
using LeadTheBoard.Shared.Models.Role;
using LeadTheBoard.Shared.Models.Task;
using LeadTheBoard.Shared.Models.User;
using LeadTheBoard.WebUI.Controllers.Base;
using LeadTheBoard.WebUI.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LeadTheBoard.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UsersController : BaseController<UsersController>
    {
        public async Task<IActionResult> Index()
        {
            var users = await UnitOfWork.Users
                .Find()
                .Include(i => i.UsersAndRoles)
                .ThenInclude(i => i.Role)
                .OrderByDescending(i => i.Id)
                .ToListAsync();

            var result = users.Select(i => new UserListModel()
            {
                Email = i.Email,
                FullName = i.FullName,
                Id = i.Id,
                LastActivity = i.LastActivity,
                Title = i.UsersAndRoles.FirstOrDefault()?.Role.Name ?? "",
                TitleId = i.UsersAndRoles.FirstOrDefault()?.Role.Id ?? 0

            }).ToList();

            return View(result);
        }

        public IActionResult Create()
        {
            var titles = UnitOfWork.Roles.Find().ToList();
            ViewBag.Titles = titles.Select(i => new RoleListModel()
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel model, IFormFile? imageFile)
        {
            var user = new User()
            {
                Email = model.Email,
                FullName = model.FullName,
                Password = model.Password,
            };

            if (imageFile != null)
            {
                var filepath = await FileUploadHelper.UploadFileAsync(imageFile, WebHostEnvironment.WebRootPath);
                user.ImageUrl = filepath;
            }

            await UnitOfWork.Users.AddAsyncReturnEntity(user);
            await UnitOfWork.CommitAsync();

            var role = await UnitOfWork.Roles.GetByIdAsync(model.TitleId);
            if (role == null)
            {
                return NotFound();
            }

            var userAndRole = new UserAndRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            await UnitOfWork.UsersAndRoles.AddAsync(userAndRole);
            await UnitOfWork.CommitAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await UnitOfWork.Users
              .Find(i => i.Id == id)
              .Include(i => i.UserBadges)
              .ThenInclude(i => i.Badge)
              .Include(i => i.TaskAssignments)
              .ThenInclude(i => i.Operation)
              .Include(i => i.UsersAndRoles)
              .ThenInclude(i => i.Role)
              .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            var result = new UserDetailsModel()
            {
                UserFullName = user.FullName,
                UserImageUrl = user.ImageUrl ?? "",
                UserId = user.Id,
                Title = user.UsersAndRoles.FirstOrDefault()?.Role?.Name ?? "",
                Badges = user.UserBadges.Select(i => new BadgeListModel()
                {
                    Id = i.Badge.Id,
                    Name = i.Badge.Name,
                    ImageUrl = i.Badge.ImageUrl,
                }).ToList(),
                Statistics = user.TaskAssignments.Where(i => i.IsCompleted).Select(i => new TaskStatisticsModel()
                {
                    Point = i.Operation.Point,
                    CompletedDate = i.CompletedDateTime,
                }).ToList(),
            };

            return View(result);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await UnitOfWork.Users.Find(i => i.Id == id).Include(i => i.UsersAndRoles).ThenInclude(i => i.Role).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            var titles = UnitOfWork.Roles.Find().ToList();
            ViewBag.Titles = titles.Select(i => new RoleListModel()
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();

            var model = new UserModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                LastActivity = user.LastActivity,
                Password = user.Password,
                TitleId = user.UsersAndRoles.FirstOrDefault()?.Role.Id ?? 0,
                Title = user.UsersAndRoles.FirstOrDefault()?.Role.Name ?? "",
                ImageUrl = user.ImageUrl,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModel model, IFormFile? imageFile)
        {
            var user = await UnitOfWork.Users.GetByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.FullName = model.FullName;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = model.Password;
            }

            if (imageFile != null)
            {
                var filepath = await FileUploadHelper.UploadFileAsync(imageFile, WebHostEnvironment.WebRootPath);
                user.ImageUrl = filepath;
            }

            await UnitOfWork.Users.UpdateAsync(user);
            await UnitOfWork.CommitAsync();

            var role = await UnitOfWork.Roles.GetByIdAsync(model.TitleId);
            if (role == null)
            {
                return NotFound();
            }

            var userAndRole = await UnitOfWork.UsersAndRoles.Find(i => i.UserId == user.Id).FirstOrDefaultAsync();
            if (userAndRole == null)
            {
                return NotFound();
            }

            userAndRole.RoleId = model.TitleId;
            await UnitOfWork.UsersAndRoles.UpdateAsync(userAndRole);
            await UnitOfWork.CommitAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await UnitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await UnitOfWork.Users.DeleteAsync(user);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
