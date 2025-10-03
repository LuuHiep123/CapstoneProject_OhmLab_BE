using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class GradeRepository : IGradeRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public GradeRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<IEnumerable<Grade>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Grade> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .AsSplitQuery() // Use split query for better performance
                    .FirstOrDefaultAsync(g => g.GradeId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Grade>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .Where(g => g.UserId == userId)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Grade>> GetByLabIdAsync(int labId)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .Where(g => g.LabId == labId)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Grade>> GetByTeamIdAsync(int teamId)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .Where(g => g.TeamId == teamId)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Grade>> GetByStatusAsync(string status)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .Where(g => g.GradeStatus == status)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Grade> CreateAsync(Grade grade)
        {
            try
            {
                await _DBContext.Grades.AddAsync(grade);
                await _DBContext.SaveChangesAsync();
                return grade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Grade> UpdateAsync(Grade grade)
        {
            _DBContext.Grades.Update(grade);
            await _DBContext.SaveChangesAsync();
            return grade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var grade = await GetByIdAsync(id);
                if (grade != null)
                {
                    _DBContext.Grades.Remove(grade);
                    await _DBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _DBContext.Grades.AnyAsync(g => g.GradeId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetAverageScoreByLabAsync(int labId)
        {
            try
            {
                var grades = await _DBContext.Grades
                    .Where(g => g.LabId == labId && g.GradeStatus == "Graded")
                    .ToListAsync();
                
                // Vì entity Grade không có trường Score, trả về số lượng grade đã chấm
                return grades.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> GetAverageScoreByUserAsync(Guid userId)
        {
            try
            {
                var grades = await _DBContext.Grades
                    .Where(g => g.UserId == userId && g.GradeStatus == "Graded")
                    .ToListAsync();
                
                // Vì entity Grade không có trường Score, trả về số lượng grade đã chấm
                return grades.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Grade>> GetGradesByStudentId(Guid studentId)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.Lab)
                    .Include(g => g.Team)
                    .Include(g => g.User)
                    .Where(g => g.UserId == studentId)
                    
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Grade>> GetGradesByLabAndTeam(int labId, int teamId)
        {
            try
            {
                return await _DBContext.Grades
                    .Include(g => g.User)
                    .Where(g => g.LabId == labId && g.TeamId == teamId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}