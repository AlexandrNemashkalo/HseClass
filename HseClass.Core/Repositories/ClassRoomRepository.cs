using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class ClassRoomRepository: IClassRoomRepository
    {
        private readonly HseClassContext _context;

        public ClassRoomRepository(HseClassContext context)
        {
            _context = context;
        }
        
        public async Task<ClassRoom> Create(ClassRoom cl)
        {
            var result = await _context.ClassRooms.AddAsync(cl);
            await _context.SaveChangesAsync();
            
            return result.Entity;
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

            return await query.ToListAsync();
        }

        public async Task<ClassRoom> GetById(int classId)
        {
            return await _context.ClassRooms
                .Include(c => c.Labs)
                .Include(c => c.UserClasses)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }

        public async Task<ClassRoom> GetByCode(Guid code)
        {
            return await _context.ClassRooms
                .Include(c => c.Labs)
                .Include(c => c.UserClasses)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<ClassRoom> Update(ClassRoom cl)
        {
            var result = _context.ClassRooms.Update(cl);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}