using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereas.Domain.Entities
{
    public class Tickets
    {
        public int Id { get; set; }
        public string DepartFrom { get; set; }
        public string FlyingTo { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartFromDate { get; set; }
        public DateTime FlyingToDate { get; set; }

        public Tickets()
        {
           
        }

        public static bool VerifyDate(DateTime departFromDate, DateTime flyingToDate)
        {
            return (departFromDate > flyingToDate) ? false : true;
        }


    }
}
