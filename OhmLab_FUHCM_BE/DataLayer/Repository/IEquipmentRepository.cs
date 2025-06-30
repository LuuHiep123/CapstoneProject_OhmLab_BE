using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IEquipmentRepository
    {
        public Task<bool> CreateEquipment(Equipment equipment);
        public Task<bool> UpdateEquipment(Equipment equipment);
        public Task<bool> DeleteEquipment(Equipment equipment);
        public Task<List<Equipment>> GetAllEquipment();
        public Task<Equipment> GetEquipmentById(string id);
    }
}
