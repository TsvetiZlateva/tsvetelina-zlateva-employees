using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using tsvetelina_zlateva_employees.Models;

namespace tsvetelina_zlateva_employees.Services
{
    public class EmployeesService : IEmployeesService
    {
        public List<Employee> GetEmployeesFromDatatable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<Employee> employees = new List<Employee>();

            foreach (DataRow dataRow in dt.Rows)
            {
                string[] dateFromParts = dataRow.ItemArray[2]
                    .ToString()
                    .Split('-', StringSplitOptions.RemoveEmptyEntries);
                int dateFromYear = int.Parse(dateFromParts[2]);
                int dateFromMonth = int.Parse(dateFromParts[1]);
                int dateFromDay = int.Parse(dateFromParts[0]);

                Employee employee = new Employee();
                employee.ID = Convert.ToInt32(dataRow.ItemArray[0]);
                employee.ProjectID = Convert.ToInt32(dataRow.ItemArray[1]);               
                employee.DateFrom = new DateTime(dateFromYear, dateFromMonth, dateFromDay);

                if (dataRow.ItemArray[3].ToString() != "NULL")
                {
                    string[] dateToParts = dataRow.ItemArray[3]
                        .ToString()
                        .Split('-', StringSplitOptions.RemoveEmptyEntries);
                    int dateToYear = int.Parse(dateToParts[2]);
                    int dateToMonth = int.Parse(dateToParts[1]);
                    int dateToDay = int.Parse(dateToParts[0]);
                                        
                    employee.DateTo = new DateTime(dateToYear, dateToMonth, dateToDay);
                }
                else
                {
                    employee.DateTo = DateTime.Now.Date;
                }

                employees.Add(employee);
            }

            return employees;
        }

        public List<EmployeesWorkedTogether> GetEmployeesWorkedTogether(List<Employee> employees)
        {
            var emplGroupedByProject = employees
                .GroupBy(e => e.ProjectID)
                .Select(grp => grp.OrderBy(e => e.DateFrom).ToList())
                .ToList();

            List<EmployeesWorkedTogether> employeesWorkedTogether = new List<EmployeesWorkedTogether>();

            foreach (List<Employee> employeesPerProject in emplGroupedByProject)
            {
                var employeesByID = employeesPerProject
                    .GroupBy(u => u.ID)
                    .Select(grp => grp.ToList())
                    .ToList();

                for (int i = 0; i < employeesByID.Count; i++)
                {
                    var emplList1 = employeesByID[i];

                    foreach (Employee employee1 in emplList1)
                    {
                        DateTime start1 = employee1.DateFrom;
                        DateTime end1 = employee1.DateTo;

                        for (int j = i + 1; j < employeesByID.Count; j++)
                        {
                            var emplList2 = employeesByID[j];

                            foreach (Employee employee2 in emplList2)
                            {
                                DateTime start2 = employee2.DateFrom;
                                DateTime end2 = employee2.DateTo;

                                if (start1 > end2 || end1 < start2)
                                {
                                    continue;
                                }

                                var workedDaysTogether = SumDaysForPair(start1, start2, end1, end2);

                                if (workedDaysTogether > 0)
                                {
                                    employeesWorkedTogether.Add(new EmployeesWorkedTogether
                                    {
                                        FirstEmployeeID = employee1.ID,
                                        SecondEmployeeID = employee2.ID,
                                        ProjectID = employee1.ProjectID,
                                        WorkedDays = workedDaysTogether
                                    });
                                }
                            }
                        }
                    }
                }
            }

            var workByPairs = employeesWorkedTogether
                .GroupBy(group => new
                {
                    group.FirstEmployeeID,
                    group.SecondEmployeeID,
                    group.ProjectID
                })
                .Select(g => new EmployeesWorkedTogether()
                {
                    FirstEmployeeID = g.Key.FirstEmployeeID,
                    SecondEmployeeID = g.Key.SecondEmployeeID,
                    ProjectID = g.First().ProjectID,
                    WorkedDays = g.Sum(s => s.WorkedDays)
                })
                .OrderByDescending(e => e.WorkedDays)
                .ToList();

            return workByPairs;
        }

        private int SumDaysForPair(DateTime start1, DateTime start2, DateTime end1, DateTime end2)
        {
            int daysTogether = 0;

            if (start1 <= start2 && end1 <= end2)
            {
                daysTogether = CalculateDaysDiff(start2, end1);
            }
            else if (start1 >= start2 && end1 >= end2)
            {
                daysTogether = CalculateDaysDiff(start1, end2);
            }
            else if (start1 >= start2 && end1 <= end2)
            {
                daysTogether = CalculateDaysDiff(start1, end1);
            }
            else if (start1 <= start2 && end1 >= end2)
            {
                daysTogether = CalculateDaysDiff(start2, end2);
            }
            return daysTogether;
        }

        private int CalculateDaysDiff(DateTime start, DateTime end)
        {
            return (int)(end - start).TotalDays;
        }
    }
}
