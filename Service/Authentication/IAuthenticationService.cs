using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authentication;

namespace BKConnectBE.Service.Authentication
{
    public interface IAuthenticationService
    {
        Task<Responses> Register(AccountDto AccountDto);

        Task ActiveAccount(string secretHash);
    }
}