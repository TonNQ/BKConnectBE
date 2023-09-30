using BKConnect.BKConnectBE.Common;
using Microsoft.AspNetCore.Mvc;

namespace BKConnect.Controllers;

public static class ControllerExtension
{
    public static Responses Success(this ControllerBase controller, object data, string message) 
    {
        return new Responses
        {
            Message = message,
            Data = data
        };
    }

    public static Responses Error(this ControllerBase controller, string messageError)
    {
        return new Responses
        {
            Message = messageError,
            Data = null
        };
    }
}
