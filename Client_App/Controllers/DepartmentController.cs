using BelajarAPI.Models;
using Client_App.Report;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Client_App.Controllers
{
    public class DepartmentController : Controller
    {
        readonly HttpClient client = new HttpClient();
       
        public DepartmentController()
        {
            client.BaseAddress = new Uri("http://localhost:51650/api/"); // reference api
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public JsonResult LoadDepartment()
        {
            IEnumerable<Department> model = null;
            var responTask = client.GetAsync("department");
            responTask.Wait();
            var result = responTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>();
                readTask.Wait();
                model = readTask.Result;
            }
            else
            {
                model = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }

            return new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertOrUpdate(Department department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (department.Id == 0) // insert
            {
                var result = client.PostAsync("department", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else // update
            {
                var result = client.PutAsync("department/" + department.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        public async Task<JsonResult> GetById(int Id)
        {

            HttpResponseMessage response = await client.GetAsync("department");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Department>>();
                var dept = data.FirstOrDefault(x => x.Id == Id);
                var json = JsonConvert.SerializeObject(dept, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal Server Eror");
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("department/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Department
        public ActionResult Index()
        {
            return View(LoadDepartment());
        }




        public  ActionResult Report(Department department)
        {
            DepartmentReport deptreport = new DepartmentReport();
            byte[] abytes = deptreport.PrepareReport(exportToPdf());
            return File(abytes, "application/pdf", $"Department_{DateTime.Now.ToString("MM/dd/yyyy")}.pdf"); 
        }

        public List<Department> exportToPdf()
        {
            IEnumerable<Department> model = null;
            var responTask = client.GetAsync("department");
            responTask.Wait();
            var result = responTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>();
                readTask.Wait();
                model = readTask.Result;
            }
            else
            {
                model = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }

            return model.ToList();
        }


        public async Task<ActionResult> convertToExcel()
        {
            var columnHeaders = new string[]
            {
                "Name",
                "Create Date"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Department Excel");
                using (var cells = worksheet.Cells[1, 1, 1, 2])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("department");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<Department>>();
                    foreach (var department in readTask)
                    {
                        worksheet.Cells["A" + j].Value = department.Name;
                        worksheet.Cells["B" + j].Value = department.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Department_{DateTime.Now.ToString("MM/dd/yyyy")}.xlsx");
        }


        public async Task<ActionResult> convertToCSV()
        {
            var columnHeaders = new string[]
            {
                "Nama Department",
                "Create Date"
            };

            HttpResponseMessage response = await client.GetAsync("department");
            var readTask = await response.Content.ReadAsAsync<IList<Department>>();
            var departmentRecords = from department in readTask
                                    select new object[]
                                    {
                                        $"{department.Name}",
                                        $"\"{department.CreateDate.ToString("MM/dd/yyyy")}\"" 
                                    }.ToList();

            var departmentcsv = new StringBuilder();
            departmentRecords.ForEach(line =>
            {
                departmentcsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{departmentcsv.ToString()}");

            return File(buffer, "application/ms-excel", $"CSV_Department_{DateTime.Now.ToString("MM/dd/yyyy")}.csv");
        }



    }
}
