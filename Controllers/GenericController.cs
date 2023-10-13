using BKConnectBE.Common.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers
{
    [CustomAuthorize]
    [ApiController]
    public class GenericController : ControllerBase
    {
        
    }
}