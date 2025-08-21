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
    public class TeamEquipmentRepository : ITeamEquipmentRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public TeamEquipmentRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<bool> CreateTeamEquipment(TeamEquipment teamEquipment)
        {
            try
            {
                var TeamEquip = await _DBContext.TeamEquipments.AddAsync(teamEquipment);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTeamEquipment(TeamEquipment teamEquipment)
        {
            try
            {
                var TeamEquip = _DBContext.TeamEquipments.Remove(teamEquipment);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamEquipment>> GetAllTeamEquipment()
        {
            try
            {
                var listTeamEquipment = await _DBContext.TeamEquipments
                    .Include(TE => TE.Team)
                    .Include(TE => TE.Equipment)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamEquipment>> GetAllTeamEquipmentByEquipmentId(string equipmentId)
        {
            try
            {
                var listTeamEquipment = await _DBContext.TeamEquipments
                    .Where(TE => TE.EquipmentId.Equals(equipmentId))
                    .Include(TE => TE.Team)
                    .Include(TE => TE.Equipment)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeamEquipment>> GetAllTeamEquipmentByTeamId(int teamId)
        {
            try
            {
                var listTeamEquipment = await _DBContext.TeamEquipments
                    .Where(TE => TE.TeamId == teamId)
                    .Include(TE => TE.Team)
                    .Include(TE => TE.Equipment)
                    .AsSplitQuery() // Use split query for better performance
                    .ToListAsync();
                return listTeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamEquipment> GetTeamEquipmentById(int id)
        {
            try
            {
                var TeamEquipment = await _DBContext.TeamEquipments
                    .Include(TE => TE.Team)
                    .Include(TE => TE.Equipment)
                    .AsSplitQuery() // Use split query for better performance
                    .FirstOrDefaultAsync(TE => TE.TeamEquipmentId == id);
                return TeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TeamEquipment> GetTeamEquipmentByTeamId(int teamId)
        {
            try
            {
                var TeamEquipment = await _DBContext.TeamEquipments
                    .Include(TE => TE.Team)
                    .Include(TE => TE.Equipment)
                    .AsSplitQuery() // Use split query for better performance
                    .FirstOrDefaultAsync(TE => TE.TeamId == teamId);
                return TeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateTeamEquipment(TeamEquipment teamEquipment)
        {
            try
            {
                _DBContext.TeamEquipments.Update(teamEquipment);
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
