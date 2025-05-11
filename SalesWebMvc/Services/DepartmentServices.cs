using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMcv.Models;
using SalesWebMvc.Data;

namespace SalesWebMvc.Services {
    public class DepartmentServices {

        private readonly SalesWebMvcContext _context;
        public DepartmentServices(SalesWebMvcContext context) {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync() {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
