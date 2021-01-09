using project.models.RefreshTokens;
using project.models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IUserRepository
    {
        Task<List<GetUserModel>> GetUsers();
        Task<GetUserModel> GetUser(string id);
        Task<GetUserModel> PostUser(PostUserModel postUserModel);
        Task PutUser(string id, PutUserModel putUserModel);
        Task PatchUser(Guid id, PatchUserModel patchUserModel);
        Task DeleteUser(string id);
        Task<List<GetRefreshTokenModel>> GetUserRefreshTokens(Guid id);

        // JWT methods

        Task<PostAuthenticateResponseModel> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel, string ipAddress);
        Task<PostAuthenticateResponseModel> RefreshToken(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);

        Task SendResetToken(EmailModel emailModel);
        Task<string> ValidatePasswordReset(ResetPasswordModel resetPasswordModel);

    }
}
