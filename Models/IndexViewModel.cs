using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tsvetelina_zlateva_employees.Models
{
    public class IndexViewModel
    {        public IndexViewModel()
        {
            this.Employees = new List<Employee>();
            this.EmployeesWorkedTogethers = new List<EmployeesWorkedTogether>();
        }

        public List<Employee> Employees { get; set; }
        public List<EmployeesWorkedTogether> EmployeesWorkedTogethers { get; set; }
    }
}
