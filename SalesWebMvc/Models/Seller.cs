using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SalesWebMcv.Models;

namespace SalesWebMvc.Models {
    public class Seller {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() {
        }

        public Seller(int id, string name, string email, DateTime birthdate, double baseSalary, Department department) {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthdate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr) {
            Sales.Add(sr);
        }
        public void UpdateSales(SalesRecord sr) {
            Sales.Remove(sr);
        }
        public double TotalSales(DateTime initial, DateTime final) {
            /*
            double sum = 0;
            foreach (SalesRecords sr in Sales) {
                if (sr.Date >= initial && sr.Date <= final) {
                    sum += sr.Amount;
                }
            }
            return sum;
            */

            //Usando expreção lambda
            
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
