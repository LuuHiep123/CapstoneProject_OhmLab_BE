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
        private readonly OhmLab_DBContext _DBContext;

        public EquipmentRepository(OhmLab_DBContext OhmLab_DBContext)
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
                return await _DBContext.Equipment.ToListAsync();
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
                return await _DBContext.Equipment.FirstOrDefaultAsync(e => e.EquipmentId == id);
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
