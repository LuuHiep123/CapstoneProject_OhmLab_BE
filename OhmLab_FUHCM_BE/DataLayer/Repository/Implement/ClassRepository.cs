using DataLayer.DBContext;
using DataLayer.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repository.Implement
{
    public class ClassRepository : IClassRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;
        public ClassRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }
        public async Task<Class> GetByIdAsync(int id)
        {
            return await _DBContext.Classes.FindAsync(id);
        }

        public async Task<List<Class>> GetByLecturerIdAsync(Guid lecturerId)
        {
            return await _DBContext.Classes
                .Where(c => c.LecturerId == lecturerId)
                .ToListAsync();
        }
    }
} 