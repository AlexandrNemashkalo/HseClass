using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Data.EF;
using HseClass.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HseClass.Core.Entities;
using HseClass.Core.Infrastructure;
using HseClass.Core.IRepositories;

namespace HseClass.Data.Repositories
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        private readonly HseClassContext _context;
        private readonly IMapper _mapper;

        public ClassRoomRepository(HseClassContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ClassRoom> Create(ClassRoom cl)
        {
            var classRoomEntity = _mapper.Map<ClassRoom, ClassRoomEntity>(cl);
            var result = await _context.ClassRooms.AddAsync(classRoomEntity);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ClassRoomEntity, ClassRoom>(result.Entity);
        }

        public async Task Delete(int classId)
        {
            var cl = await _context.ClassRooms.FirstOrDefaultAsync(c => c.Id == classId);
            _context.ClassRooms.Remove(cl);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ClassRoom>> GetByUserId(int userId)
        {
            var query =
                from c in _context.ClassRooms 
                join p in _context.UserClasses
                    on c.Id equals p.ClassRoomId
                join u in _context.Users
                    on p.UserId equals u.Id
                where p.UserId == userId
                select c;

            return await query.Select(cl => _mapper.Map<ClassRoomEntity, ClassRoom>(cl)).ToListAsync();
        }

        public async Task<ClassRoom> GetById(int classId)
        {
            var cl = await _context.ClassRooms
                .Include(c => c.Labs)
                .Include(c => c.UserClasses)
                .FirstOrDefaultAsync(c => c.Id == classId);

            return _mapper.Map<ClassRoomEntity, ClassRoom>(cl);
        }

        public async Task<ClassRoom> Update(ClassRoom cl)
        {
            var classRoomEntity = _mapper.Map<ClassRoom, ClassRoomEntity>(cl);
            var result = _context.ClassRooms.Update(classRoomEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClassRoomEntity, ClassRoom>(result.Entity);
        }
    }
}