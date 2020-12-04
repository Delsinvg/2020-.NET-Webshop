using project.models.Users;

namespace project.web.Services
{
    public interface IStateManagementService
    {
        void SetSession(string postAuthenticateResponseModelJson);
        void SetState(PostAuthenticateResponseModel postAuthenticateResponseModel, string rememberMe);
        void ClearState();
        bool CheckRememberMe();
    }
}
