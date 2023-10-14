using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.ClassManagement;
using BKConnectBE.Service.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Classes
{
    [CustomAuthorize]
    [ApiController]
    [Route("classes")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classSerive;
        public ClassController(IClassService classService)
        {
            _classSerive = classService;
        }

        [HttpGet("getClass")]
        public async Task<ActionResult<Responses>> GetClassInformation(long classId)
        {
            try
            {
                ClassDto studentClass = await _classSerive.GetClassByIdAsync(classId);

                return this.Success(studentClass, MsgNo.SUCCESS_GET_CLASS);
            }
            catch(Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getAllClasses")]
        public async Task<ActionResult<Responses>> GetAllClasses()
        {
            try
            {
                var classes = await _classSerive.GetAllClassesAsync();

                return this.Success(classes, MsgNo.SUCCESS_GET_CLASS);
            }
            catch(Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}