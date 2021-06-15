using System.Collections.Generic;
using System.Data;
using tsvetelina_zlateva_employees.Models;

namespace tsvetelina_zlateva_employees.Services
{
    public interface IEmployeesService
    {
        List<Employee> GetEmployeesFromDatatable(DataTable dt);

        List<EmployeesWorkedTogether> GetEmployeesWorkedTogether(List<Employee> employees);
    }
}
