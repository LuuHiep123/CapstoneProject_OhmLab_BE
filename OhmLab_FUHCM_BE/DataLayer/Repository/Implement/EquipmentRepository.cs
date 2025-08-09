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
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public EquipmentRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<bool> CreateEquipment(Equipment equipment)
        {
            try
            {
                await _DBContext.Equipment.AddAsync(equipment);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteEquipment(Equipment equipment)
        {
            try
            {
                _DBContext.Equipment.Remove(equipment);
                await _DBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Equipment>> GetAllEquipment()
        {
            try
            {
                return await _DBContext.Equipment
                    .Include(e => e.EquipmentType)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Equipment>> GetEquipmentByEquipmentId(string equipmentId)
        {
            try
            {
                return await _DBContext.Equipment
                    .Include(e => e.EquipmentType)
                    .Where(e => e.EquipmentTypeId.Equals(equipmentId))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Equipment> GetEquipmentById(string id)
        {
            try
            {
                return await _DBContext.Equipment
                    .Include(e => e.EquipmentType)
                    .FirstOrDefaultAsync(e => e.EquipmentId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Equipment> GetEquipmentByName(string name)
        {
            try
            {
                return await _DBContext.Equipment.FirstOrDefaultAsync(e => e.EquipmentName == name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateEquipment(Equipment equipment)
        {
            try
            {
                _DBContext.Equipment.Update(equipment);
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
