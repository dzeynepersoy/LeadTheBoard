using LeadTheBoard.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeadTheBoard.WebUI.Controllers.Base
{
    /// <summary>
    /// Bütün Controller larda ortak olan özellikleri barındıran Base Controller
    /// </summary>
    public class BaseController<T> : Controller
    {
        // Note : Eğer Contructor kullansaydım, bu base sınıfında türeyen diğer controllerda da Contructor yazmak zorundaydım.

        public IUnitOfWork _unitOfWork;

        private ILogger<T> _logger;

        private IWebHostEnvironment _webHostEnvironment;

        private int? _userId;

        // Giriş yapan kullanıcının Id bilgisini almak için HttpContext.User özelliğini kullanıyoruz
        protected int UserId => _userId ??= int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        protected IUnitOfWork UnitOfWork => _unitOfWork ??= HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();

        protected IWebHostEnvironment WebHostEnvironment => _webHostEnvironment ??= HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();


    }
}
