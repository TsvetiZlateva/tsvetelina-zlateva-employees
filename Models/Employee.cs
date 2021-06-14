using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tsvetelina_zlateva_employees.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
