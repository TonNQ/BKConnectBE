using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authorizations;

namespace BKConnectBE.Services.Authorizations
{
    public interface IAuthorizationService
    {
        Task<Responses> Register(RegisterDto registerDto);

        Task<Responses> ActiveAccount(string secretHash);
    }
}