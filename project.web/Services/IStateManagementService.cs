using project.models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
