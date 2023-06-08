using LeadTheBoard.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LeadTheBoard.WebUI.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error-development")]
        public IActionResult ErrorDevelopment()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //Log kaydı
            _logger.LogError(exceptionDetails?.Error.Message, exceptionDetails);

            ErrorViewModel errorViewModel = new()
            {
                ExceptionMessage = exceptionDetails.Error.Message,
                ExceptionPath = exceptionDetails.Path,
                ExceptionStackTrace = exceptionDetails.Error.StackTrace

            };

            return View(errorViewModel);
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //Log kaydı
            _logger.LogError(exceptionDetails?.Error.Message, exceptionDetails);

            return View();
        }

        [Route("/unauthorized")]
        public IActionResult UnAuthorized()
        {
            return View();
        }

    }
}
