using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITeamUserRepository
    {
        Task<List<TeamUser>> GetAllAsync();
        Task<TeamUser> GetByIdAsync(int id);
        Task<List<TeamUser>> GetByTeamIdAsync(int teamId);
        Task<List<TeamUser>> GetByUserIdAsync(Guid userId);
        Task<TeamUser> CreateAsync(TeamUser teamUser);
        Task<TeamUser> UpdateAsync(TeamUser teamUser);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsUserInTeamAsync(Guid userId, int teamId);
    }
} 