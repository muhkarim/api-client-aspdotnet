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

    }
}
