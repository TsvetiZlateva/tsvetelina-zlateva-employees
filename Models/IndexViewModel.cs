using System.Collections.Generic;

namespace tsvetelina_zlateva_employees.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Employees = new List<Employee>();
            this.BestCoworkers = new List<BestCoworkers>();
        }

        public List<Employee> Employees { get; set; }
        public List<BestCoworkers> BestCoworkers { get; set; }
    }
}
