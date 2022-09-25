using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerraLinhasAereas.Domain.Entities
{
    public class Address
    {
        public string Cep { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }



    }
}
