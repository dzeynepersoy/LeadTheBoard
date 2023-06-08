using LeadTheBoard.Domain.Entities;
using LeadTheBoard.Shared.Models.Badge;
using LeadTheBoard.WebUI.Controllers.Base;
using LeadTheBoard.WebUI.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadTheBoard.WebUI.Controllers
{
    public class BadgeOperationsController : BaseController<BadgeOperationsController>
    {
        [Authorize(Roles = "Admin,Manager,Operator")]
        public IActionResult Index()
        {
            //bütün badge leri getir ve onları geçerli tarihlerine göre sırala
            var badges = UnitOfWork.Badges.Find().OrderByDescending(i => i.ValidityDateStart).ToList();

            var result = badges.Select(i => new BadgeListModel()
            {
                Description = i.Description,
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Name = i.Name,

            });

            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(BadgeModel model, IFormFile? imageFile)
        {
            var badge = new Badge()
            {
                Description = model.Description,
                Name = model.Name,
                ValidityDateEnd = model.ValidityDateEnd,
                ValidityDateStart = model.ValidityDateStart,
                RequiredPoints = model.RequiredPoints,
            };

            if (imageFile != null)
            {
                var filepath = FileUploadHelper.UploadFileAsync(imageFile, WebHostEnvironment.WebRootPath).Result;
                badge.ImageUrl = filepath;
            }

            await UnitOfWork.Badges.AddAsync(badge);
            await UnitOfWork.CommitAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var badge = await UnitOfWork.Badges.GetByIdAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            var badgeModel = new BadgeModel()
            {
                Description = badge.Description,
                Id = badge.Id,
                ImageUrl = badge.ImageUrl,
                Name = badge.Name,
                ValidityDateEnd = badge.ValidityDateEnd,
                ValidityDateStart = badge.ValidityDateStart,
                RequiredPoints = badge.RequiredPoints
            };

            return View(badgeModel);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(BadgeModel model, IFormFile? imageFile)
        {
            var badge = await UnitOfWork.Badges.GetByIdAsync(model.Id);
            if (badge == null)
            {
                return NotFound();
            }

            if (imageFile != null)
            {
                var filepath = await FileUploadHelper.UploadFileAsync(imageFile, WebHostEnvironment.WebRootPath);
                badge.ImageUrl = filepath;
            }

            badge.Name = model.Name;
            badge.Description = model.Description;
            badge.ValidityDateStart = model.ValidityDateStart;
            badge.ValidityDateEnd = model.ValidityDateEnd;
            badge.RequiredPoints = model.RequiredPoints;

            await UnitOfWork.Badges.UpdateAsync(badge);
            await UnitOfWork.CommitAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var badge = await UnitOfWork.Badges.GetByIdAsync(id);
            if (badge == null)
            {
                return NotFound();
            }
            await UnitOfWork.Badges.DeleteAsync(badge);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}
