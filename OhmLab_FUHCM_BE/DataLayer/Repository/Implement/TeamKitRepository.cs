using DataLayer.DBContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class TeamKitRepository : ITeamKitRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public TeamKitRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<bool> CreateTeamKit(TeamKit teamKit)
        {
            try
            {
                var TeamKit = await _DBContext.TeamKits.AddAsync(teamKit);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTeamKit(TeamKit teamKit)
        {
            try
            {
                var TeamKit = _DBContext.TeamKits.Remove(teamKit);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamKit>> GetAllTeamKit()
        {
            try
            {
                var listTeamKit = await _DBContext.TeamKits
                    .Include(TK => TK.Team)
                        .ThenInclude(t => t.Class)
                    .Include(TK => TK.Kit)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamKit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamKit>> GetAllTeamKitByKitId(string kitId)
        {
            try
            {
                var listTeamKit = await _DBContext.TeamKits
                    .Where(TK => TK.KitId.Equals(kitId))
                    .Include(TK => TK.Team)
                        .ThenInclude(t => t.Class)
                    .Include(TK => TK.Kit)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamKit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamKit>> GetAllTeamKitByTeamId(int teamId)
        {
            try
            {
                var listTeamKit = await _DBContext.TeamKits
                    .Where(TK => TK.TeamId == teamId)
                    .Include(TK => TK.Team)
                        .ThenInclude(t => t.Class)
                    .Include(TK => TK.Kit)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamKit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamKit> GetTeamKitById(int id)
        {
            try
            {
                var TeamKit = await _DBContext.TeamKits
                    .Include(TK => TK.Team)
                        .ThenInclude(t => t.Class)
                    .Include(TK => TK.Kit)
                    .AsSplitQuery() // Use split query for better performance
                    .FirstOrDefaultAsync(TK => TK.TeamKitId == id);
                return TeamKit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateTeamKit(TeamKit teamKit)
        {
            try
            {
                _DBContext.TeamKits.Update(teamKit);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
