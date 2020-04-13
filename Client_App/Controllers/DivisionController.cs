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
    public class DivisionController : Controller
    {
        readonly HttpClient client = new HttpClient();

        // GET: Division
        public ActionResult Index()
        {
            return View(LoadDivision());
        }

        public DivisionController()
        {
            client.BaseAddress = new Uri("http://localhost:51650/api/"); // reference api
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public JsonResult LoadDivision()
        {
            IEnumerable<DivisionViewModel> model = null;
            var responTask = client.GetAsync("division");
            responTask.Wait();
            var result = responTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisionViewModel>>();
                readTask.Wait();
                model = readTask.Result;
            }
            else
            {
                model = Enumerable.Empty<DivisionViewModel>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }

            return new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertorUpdate(Division division)
        {
            var myContent = JsonConvert.SerializeObject(division);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (division.Id == 0)
            {
                var result = client.PostAsync("division", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; 
            }
            else
            {
                var result = client.PutAsync("division/" + division.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }



        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("division");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DivisionViewModel>>();
                var division = data.FirstOrDefault(x => x.Id == Id);
                var json = JsonConvert.SerializeObject(division, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("internal server error");
        }



        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("division/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public async Task<ActionResult> convertToExcel()
        {
            var columnHeaders = new string[]
            {
                "Division Name",
                "Departname Name",
                "Create Date",

            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Division Excel");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("division");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<DivisionViewModel>>();
                    foreach (var division in readTask)
                    {
                        worksheet.Cells["A" + j].Value = division.DivisionName;
                        worksheet.Cells["B" + j].Value = division.DepartmentName;
                        worksheet.Cells["C" + j].Value = division.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Division_{DateTime.Now.ToString("MM/dd/yyyy")}.xlsx");
        }

        public async Task<ActionResult> convertToCSV()
        {
            var columnHeaders = new string[]
            {
                "Division Name",
                "Department Name",
                "Create Date"
            };

            HttpResponseMessage response = await client.GetAsync("division");
            var readTask = await response.Content.ReadAsAsync<IList<DivisionViewModel>>();
            var divisionRecords = from division in readTask
                                    select new object[]
                                    {
                                        $"{division.DivisionName}",
                                        $"{division.DepartmentName}",
                                        $"\"{division.CreateDate.ToString("MM/dd/yyyy")}\""
                                    }.ToList();

            var departmentcsv = new StringBuilder();
            divisionRecords.ForEach(line =>
            {
                departmentcsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{departmentcsv.ToString()}");

            return File(buffer, "application/ms-excel", $"CSV_Division_{DateTime.Now.ToString("MM/dd/yyyy")}.csv");
        }

        public ActionResult Report(DivisionViewModel division)
        {
            DivisionReport deptreport = new DivisionReport();
            byte[] abytes = deptreport.PrepareReport(exportToPdf());
            return File(abytes, "application/pdf", $"Division_{DateTime.Now.ToString("MM/dd/yyyy")}.pdf");
        }

        public List<DivisionViewModel> exportToPdf()
        {
            IEnumerable<DivisionViewModel> model = null;
            var responTask = client.GetAsync("division");
            responTask.Wait();
            var result = responTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisionViewModel>>();
                readTask.Wait();
                model = readTask.Result;
            }
            else
            {
                model = Enumerable.Empty<DivisionViewModel>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }

            return model.ToList();
        }

    }
}
