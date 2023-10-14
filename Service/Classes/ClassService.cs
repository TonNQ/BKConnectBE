using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.ClassManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository.Classes;

namespace BKConnectBE.Service.Classes
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ClassDto> GetClassByIdAsync(long classId)
        {
            Class studentClass = await _classRepository.GetClassByIdAsync(classId);

            if (studentClass == null)
            {
                throw new Exception(MsgNo.ERROR_CLASS_NOT_FOUND);
            }

            return _mapper.Map<ClassDto>(studentClass);
        }

        public async Task<List<ClassDto>> GetAllClassesAsync()
        {
            var studentClasses = await _classRepository.GetAllClassesAsync();

            return _mapper.Map<List<ClassDto>>(studentClasses);
        }
    }
}