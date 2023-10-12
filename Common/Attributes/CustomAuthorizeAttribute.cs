using HK1_2023_2024.PBL4.BKConnectBE.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }
}