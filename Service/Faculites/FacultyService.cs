using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.FacultyManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;

namespace BKConnectBE.Service.Faculites
{
    public class FacultyService : IFacultyService
    {
        private readonly IGenericRepository<Faculty> _genericRepositoryForFaculty;
        private readonly IMapper _mapper;
        public FacultyService(IGenericRepository<Faculty> genericRepositoryForFaculty, IMapper mapper)
        {
            _genericRepositoryForFaculty = genericRepositoryForFaculty;
            _mapper = mapper;
        }

        public async Task<List<FacultyDto>> GetAllFaculiesAsync()
        {
            var faculties = await _genericRepositoryForFaculty.GetAllAsync();
            return _mapper.Map<List<FacultyDto>>(faculties);
        }

        public async Task<FacultyDto> GetFacultyByIdAsync(string facultyId)
        {
            Faculty faculty = await _genericRepositoryForFaculty.GetByIdAsync(facultyId);

            if (faculty == null)
            {
                throw new Exception(MsgNo.ERROR_FACULTY_NOT_FOUND);
            }

            return _mapper.Map<FacultyDto>(faculty);
        }
    }
}