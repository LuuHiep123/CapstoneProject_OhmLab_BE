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
    public class EquipmentTypeRepository : IEquipmentTypeRepository
    {
        private readonly OhmLab_DBContext _DBContext;

        public EquipmentTypeRepository(OhmLab_DBContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<bool> CreateEquipmentType(EquipmentType equipmentType)
        {
            try
            {
                await _DBContext.EquipmentTypes.AddAsync(equipmentType);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteEquipmentType(EquipmentType equipmentType)
        {
            try
            {
                _DBContext.EquipmentTypes.Remove(equipmentType);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EquipmentType>> GetAllEquipmentType()
        {
            try
            {
                return await _DBContext.EquipmentTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EquipmentType> GetEquipmentTypeById(string id)
        {
            try
            {
                return await _DBContext.EquipmentTypes.FirstOrDefaultAsync(et => et.EquipmentTypeId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateEquipmentType(EquipmentType equipmentType)
        {
            try
            {
                _DBContext.EquipmentTypes.Update(equipmentType);
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
