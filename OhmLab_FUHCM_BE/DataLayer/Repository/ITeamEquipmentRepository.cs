using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITeamEquipmentRepository
    {
        public Task<bool> CreateTeamEquipment(TeamEquipment teamEquipment);
        public Task<bool> UpdateTeamEquipment(TeamEquipment teamEquipment);
        public Task<bool> DeleteTeamEquipment(TeamEquipment teamEquipment);
        public Task<List<TeamEquipment>> GetAllTeamEquipment();
        public Task<TeamEquipment> GetTeamEquipmentById(int id);
        public Task<TeamEquipment> GetTeamEquipmentByEquipmentId(string equipmentId);
        public Task<TeamEquipment> GetTeamEquipmentByTeamId(int teamId);
    }
}
