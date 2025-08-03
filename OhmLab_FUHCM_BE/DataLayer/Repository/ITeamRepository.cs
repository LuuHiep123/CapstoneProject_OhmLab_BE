using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(int id);
        Task<List<Team>> GetByClassIdAsync(int classId);
        Task<Team> CreateAsync(Team team);
        Task<Team> UpdateAsync(Team team);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 