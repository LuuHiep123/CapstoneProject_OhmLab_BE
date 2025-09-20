using DataLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;
        public AccessoryRepository(db_abadcb_ohmlabContext context)
        {
            _DBContext = context;
        }


    }
}
