using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using tsvetelina_zlateva_employees.Models;
using tsvetelina_zlateva_employees.Services;

namespace tsvetelina_zlateva_employees.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment Environment;
        private readonly IEmployeesService employeesService;

        public HomeController(ILogger<HomeController> logger,
            IHostingEnvironment _environment,
            IEmployeesService employeesService)
        {
            this._logger = logger;
            this.Environment = _environment;
            this.employeesService = employeesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile postedFile, string dateFormat)
        {
            if (postedFile != null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                string csvData = System.IO.File.ReadAllText(filePath);
                DataTable dt = new DataTable();
                bool firstRow = true;

                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (firstRow)
                            {
                                foreach (string cell in row.Split(','))
                                {
                                    dt.Columns.Add(cell.Trim());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int i = 0;
                                foreach (string cell in row.Split(','))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                    i++;
                                }
                            }
                        }
                    }
                }

                List<Employee> employees = employeesService.GetEmployeesFromDatatable(dt);
                List<EmployeesWorkedTogether> employeesWorkedTogether = employeesService.GetEmployeesWorkedTogether(employees);

                // TODO group employees and projects

                var bestCoworkers = employeesWorkedTogether
                    .GroupBy(gr => new { gr.FirstEmployeeID, gr.SecondEmployeeID})
                    .Select(g => new BestCoworkers()
                    {
                        FirstEmployeeID = g.Key.FirstEmployeeID,
                        SecondEmployeeID = g.Key.SecondEmployeeID,
                        WorkedDays = g.Sum(s => s.WorkedDays), 
                        Projects = g.Select(s => s.ProjectID).ToList() 
                     })
                    .OrderByDescending(bc => bc.WorkedDays)
                    .ToList();

                IndexViewModel viewModel = new IndexViewModel
                {
                    Employees = employees,
                    BestCoworkers = bestCoworkers
                };

                ViewData["dateFormat"] = dateFormat;

                return View(viewModel);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
