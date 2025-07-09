using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IEquipmentTypeRepository
    {
        public Task<bool> CreateEquipmentType(EquipmentType equipmentType);
        public Task<bool> UpdateEquipmentType(EquipmentType equipmentType);
        public Task<bool> DeleteEquipmentType(EquipmentType equipmentType);
        public Task<List<EquipmentType>> GetAllEquipmentType();
        public Task<EquipmentType> GetEquipmentTypeById(string id);
        public Task<EquipmentType> GetEquipmentTypeByCode(string code);

    }
}
