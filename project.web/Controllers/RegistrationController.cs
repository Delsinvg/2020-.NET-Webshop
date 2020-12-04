using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Users;
using project.web.Services;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly IStateManagementService _stateManagementService;

        public RegistrationController(
            ProjectApiService projectApiService,
            IStateManagementService stateManagementService)
        {
            _projectApiService = projectApiService;
            _stateManagementService = stateManagementService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Index(PostUserModel postUserModel, string rememberMe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Send an API request to create the new user
                    GetUserModel getUserModel = await _projectApiService.PostModel<PostUserModel, GetUserModel>(postUserModel, "Users");

                    // When the user was successfully created send an API request to authenticate the new user
                    PostAuthenticateRequestModel postAuthenticateRequestModel = new PostAuthenticateRequestModel
                    {
                        UserName = postUserModel.Email,
                        Password = postUserModel.Password
                    };

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

            return View(postUserModel);
        }
    }
}
