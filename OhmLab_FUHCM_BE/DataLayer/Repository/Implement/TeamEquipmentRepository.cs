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
                    .ToListAsync();
                return listTeamEquipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<TeamEquipment> GetTeamEquipmentByEquipmentId(string equipmentId)
        {
            throw new NotImplementedException();
        }

        public Task<TeamEquipment> GetTeamEquipmentById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TeamEquipment> GetTeamEquipmentByTeamId(int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTeamEquipment(TeamEquipment teamEquipment)
        {
            throw new NotImplementedException();
        }
    }
}
