using AutoMapper;
using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.ClassManagement;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Classes
{
    public class ClassRepository : IClassRepository
    {
        private readonly BKConnectContext _context;
        public ClassRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            return await _context.Classes.Include(c => c.Faculty).ToListAsync();
        }

        public async Task<Class> GetClassByIdAsync(long classId)
        {
            return await _context.Classes.Include(c => c.Faculty).FirstOrDefaultAsync(c => c.Id == classId);
        }
    }
}