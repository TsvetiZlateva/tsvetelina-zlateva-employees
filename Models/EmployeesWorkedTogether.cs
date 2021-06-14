using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tsvetelina_zlateva_employees.Models
{
    public class EmployeesWorkedTogether
    {
        public int FirstEmployeeID { get; set; }
        public int SecondEmployeeID { get; set; }
        public int ProjectID { get; set; }
        public int WorkedDays { get; set; }
    }
}
