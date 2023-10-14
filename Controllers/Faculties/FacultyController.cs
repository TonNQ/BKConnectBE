using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.FacultyManagement;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Faculites;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Faculties
{
    [CustomAuthorize]
    [ApiController]
    [Route("faculties")]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet("getFaculty")]
        public async Task<ActionResult<Responses>> GetFacultyInformation([FromQuery] SearchKeyCondition searchKeyCondition)
        {
            try
            {
                FacultyDto facultyDto = await _facultyService.GetFacultyByIdAsync(searchKeyCondition.SearchKey);

                return this.Success(facultyDto, MsgNo.SUCCESS_GET_FACULTY);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getAllFaculties")]
        public async Task<ActionResult<Responses>> GetAllFaculties()
        {
            try
            {
                var faculties = await _facultyService.GetAllFaculiesAsync();
                return this.Success(faculties, MsgNo.SUCCESS_GET_FACULTY);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}