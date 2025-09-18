using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ITeamKitRepository
    {
        public Task<bool> CreateTeamKit(TeamKit teamKit);
        public Task<bool> UpdateTeamKit(TeamKit teamKit);
        public Task<bool> DeleteTeamKit(TeamKit teamKit);
        public Task<List<TeamKit>> GetAllTeamKit();
        public Task<List<TeamKit>> GetAllTeamKitByTeamId(int teamId);
        public Task<List<TeamKit>> GetAllTeamKitByKitId(string kitId);
        public Task<TeamKit> GetTeamKitById(int id);
    }
}
