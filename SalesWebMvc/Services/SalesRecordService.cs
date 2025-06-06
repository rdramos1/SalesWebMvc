﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services {
    public class SalesRecordService {
        private readonly SalesWebMvcContext _context;
        public SalesRecordService(SalesWebMvcContext context) {
            _context = context;
        }
        public async Task<List<SalesRecord>> FindbyDateAsync(DateTime? minDate, DateTime? maxDate) {
            var result = _context.SalesRecords.AsQueryable(); // Change this line
            if (minDate.HasValue) {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue) {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) 
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ThenBy(x => x.Date)
                .ToListAsync();
        }
    }
}
