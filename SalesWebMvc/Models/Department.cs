using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;

namespace SalesWebMcv.Models {
    public class Department {

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; }


        public Department() {
        }

        public Department(int id, string name) {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller) {
            Sellers.Add(seller);
        }

        public double TotalSales(System.DateTime initial, System.DateTime final) {
            /*
            double sum = 0;
            foreach (Seller seller in Sellers) {
                sum += seller.TotalSales(initial, final);
            }
            return sum;
            */

            //Usando expreção lambda

            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    } 
}
