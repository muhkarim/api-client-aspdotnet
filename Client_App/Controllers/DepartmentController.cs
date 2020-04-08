using BelajarAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
    }
}
