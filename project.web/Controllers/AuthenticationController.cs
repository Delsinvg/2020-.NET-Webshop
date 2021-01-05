using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Users;
using project.web.Services;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly IStateManagementService _stateManagementService;

        public AuthenticationController(ProjectApiService projectApiService, IStateManagementService stateManagementService)
        {
            _projectApiService = projectApiService;
            _stateManagementService = stateManagementService;
        }
        public IActionResult Index()
        {
            ViewData["RememberMe"] = _stateManagementService.CheckRememberMe();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Index(PostAuthenticateRequestModel postAuthenticateRequestModel, string rememberMe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Send an API request to authenticate the new user
                    PostAuthenticateResponseModel postAuthenticateResponseModel = await _projectApiService.Authenticate(postAuthenticateRequestModel);

                    _stateManagementService.SetState(postAuthenticateResponseModel, rememberMe);

                    // Redirect to the home page
                    return RedirectToRoute(new { action = "Index", controller = "Home" });
                }
                catch (ProjectException e)
                {
                    TempData["ApiError"] = e.Message;
                }
            }

            return View(postAuthenticateRequestModel);
        }

        public IActionResult Logout()
        {
            _stateManagementService.ClearState();

            return RedirectToRoute(new { action = "Index", controller = "Home" });
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> Confirmation(string email)
        {
            var response = await _projectApiService.SendResetToken(email);
            return View();
        }

        public IActionResult Email()
        {
            return View();
        }

        public ActionResult ResetPassword(string userId, string code)
        {
            string codeWithoutSpaces = code.Replace(" ", "+");
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel { UserId = new System.Guid(userId), Code =codeWithoutSpaces };
            return View(resetPasswordModel);
        }

        public async Task<ActionResult> CheckNewPassword(string userId, string code, string Password)
        {
            string codeWithoutSpaces = code.Replace(" ", "+");
            var response = await _projectApiService.ValidatePasswordReset(userId, codeWithoutSpaces, Password);
            return Redirect("/");
        }
    }
}
