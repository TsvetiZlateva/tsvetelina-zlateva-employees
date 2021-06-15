using System.Collections.Generic;

namespace tsvetelina_zlateva_employees.Models
{
    public class BestCoworkers
    {
        public BestCoworkers()
        {
            this.Projects = new List<int>();
        }
        public int FirstEmployeeID { get; set; }
        public int SecondEmployeeID { get; set; }
        public List<int> Projects { get; set; }
        public int WorkedDays { get; set; }
    }
}
