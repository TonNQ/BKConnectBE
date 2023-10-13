using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authentication;

namespace BKConnectBE.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<Responses> Register(AccountDto AccountDto);

        Task ActiveAccount(string secretHash);

        Task<Responses> ForgotPassword(string email);

        Task<Responses> ResetPassword(ResetPasswordDto resetPasswordDto);

        Task<Responses> CheckToken(string secretHash);

        Task<Responses> Logout(string secretHash);
    }
}