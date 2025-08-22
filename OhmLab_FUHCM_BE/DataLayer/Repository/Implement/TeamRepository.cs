using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class TeamRepository : ITeamRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public TeamRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }

        public async Task<List<Team>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Teams
                    .Include(t => t.Class)
                    .Include(t => t.TeamUsers)
                        .ThenInclude(tu => tu.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Team> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.Teams
                    .Include(t => t.Class)
                    .Include(t => t.TeamUsers)
                        .ThenInclude(tu => tu.User)
                    .FirstOrDefaultAsync(t => t.TeamId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Team>> GetByClassIdAsync(int classId)
        {
            try
            {
                return await _DBContext.Teams
                    .Include(t => t.Class)
                    .Include(t => t.TeamUsers)
                        .ThenInclude(tu => tu.User)
                    .Where(t => t.ClassId == classId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Team> CreateAsync(Team team)
        {
            try
            {
                await _DBContext.Teams.AddAsync(team);
                await _DBContext.SaveChangesAsync();
                return team;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Team> UpdateAsync(Team team)
        {
            try
            {
                _DBContext.Teams.Update(team);
                await _DBContext.SaveChangesAsync();
                return team;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var team = await GetByIdAsync(id);
                if (team != null)
                {
                    _DBContext.Teams.Remove(team);
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
                return await _DBContext.Teams.AnyAsync(t => t.TeamId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 