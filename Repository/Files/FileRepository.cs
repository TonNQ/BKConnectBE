using BKConnectBE.Model;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Files
{
    public class FileRepository : IFileRepository
    {
        private readonly BKConnectContext _context;
        public FileRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task<List<ClassFile>> GetListFilesOfClassRoomAsync(long roomId)
        {
            return await _context.ClassFiles.Where(r => r.RoomId == roomId)
                .OrderByDescending(r => r.Id).ToListAsync();
        }

        public async Task<ClassFile> GetFileByIdAsync(long fileId)
        {
            return await _context.ClassFiles.Include(f => f.Room).FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task AddFileAsync(ClassFile file)
        {
            await _context.ClassFiles.AddAsync(file);
        }
    }
}