using LeadTheBoard.Shared.Models.Account;
using LeadTheBoard.Shared.Models.Badge;
using LeadTheBoard.Shared.Models.Task;
using LeadTheBoard.Shared.Models.User;
using LeadTheBoard.WebUI.Controllers.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LeadTheBoard.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController<AccountController>
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await UnitOfWork.Users
                .Find(i => i.Email == model.Email && i.Password == model.Password)
                .Include(i => i.UsersAndRoles)
                .ThenInclude(i => i.Role)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View(model);
            }

            user.LastActivity = DateTime.Now;
            
            await _unitOfWork.Users.UpdateAsync(user);
            
            await UnitOfWork.CommitAsync();

            // Kullanıcı bilgilerini temel alarak kimlik doğrulama işlemi yapıyoruz
            var claims = new[]
            {
                new Claim("ImageUrl", user.ImageUrl),

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.FullName),
                
                // Kullanıcının rol bilgisi burada eklenir, örnek olarak "Admin" rolü
                new Claim(ClaimTypes.Role, user.UsersAndRoles.First().Role.Name)
            };

            // Kimlik bilgilerini oluştur ve oturum açıyoruz
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcı kendi hesabın detaylarını görecek.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var user = await UnitOfWork.Users
                .Find(i => i.Id == UserId)
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
    }
}
