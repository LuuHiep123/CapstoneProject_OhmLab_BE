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
    public class TeamUserRepository : ITeamUserRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public TeamUserRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }

        public async Task<List<TeamUser>> GetAllAsync()
        {
            try
            {
                return await _DBContext.TeamUsers
                    .Include(tu => tu.Team)
                    .Include(tu => tu.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamUser> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.TeamUsers
                    .Include(tu => tu.Team)
                    .Include(tu => tu.User)
                    .FirstOrDefaultAsync(tu => tu.TeamUserId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamUser>> GetByTeamIdAsync(int teamId)
        {
            try
            {
                return await _DBContext.TeamUsers
                    .Include(tu => tu.Team)
                    .Include(tu => tu.User)
                    .Where(tu => tu.TeamId == teamId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamUser>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _DBContext.TeamUsers
                    .Include(tu => tu.Team)
                    .Include(tu => tu.User)
                    .Where(tu => tu.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamUser> CreateAsync(TeamUser teamUser)
        {
            try
            {
                await _DBContext.TeamUsers.AddAsync(teamUser);
                await _DBContext.SaveChangesAsync();
                return teamUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamUser> UpdateAsync(TeamUser teamUser)
        {
            try
            {
                _DBContext.TeamUsers.Update(teamUser);
                await _DBContext.SaveChangesAsync();
                return teamUser;
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
                var teamUser = await GetByIdAsync(id);
                if (teamUser != null)
                {
                    _DBContext.TeamUsers.Remove(teamUser);
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
                return await _DBContext.TeamUsers.AnyAsync(tu => tu.TeamUserId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsUserInTeamAsync(Guid userId, int teamId)
        {
            try
            {
                return await _DBContext.TeamUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 