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

        public async Task<List<UploadedFile>> GetListFilesOfClassRoomAsync(long roomId)
        {
            return await _context.UploadedFiles.Where(r => r.RoomId == roomId)
                .OrderByDescending(r => r.Id).ToListAsync();
        }

        public async Task<UploadedFile> GetFileByIdAsync(long fileId)
        {
            return await _context.UploadedFiles.Include(f => f.Room).FirstOrDefaultAsync(f => f.Id == fileId);
        }

        public async Task AddFileAsync(UploadedFile file)
        {
            await _context.UploadedFiles.AddAsync(file);
        }
    }
}