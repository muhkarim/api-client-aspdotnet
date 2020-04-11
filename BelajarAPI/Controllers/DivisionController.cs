using BelajarAPI.Models;
using BelajarAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace BelajarAPI.Controllers
{
    public class DivisionController : ApiController
    {
        DivisionRepository div = new DivisionRepository();

        [HttpGet]
        public IHttpActionResult Get()
        {

            if (div.Get() == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Data Division is Empty");
            }
            return Ok(div.Get());
        }


        public IHttpActionResult Post(Division division)
        {
            if (division.Name == "" || division.Name == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Name cannot empty");
            }
            else
            {
                div.Create(division);
                return Ok("Division added successfully");
            }
        }

        [ResponseType(typeof(Department))]
        [HttpGet]
        public async Task<IEnumerable<DivisionViewModel>> Get(int Id)
        {

            if (await div.Get(Id) == null)
            {
                return null;
            }
            return await div.Get(Id);


            //return await div.Get(Id);


        }


        public IHttpActionResult Put(int Id, Division division)
        {
            if ((division.Name != null) && (division.Name != ""))
            {
                div.Update(Id, division);
                return Ok("Devisi Updated Successfully!");
            }

            return BadRequest("Failed to Update Devisi");


        }

        public IHttpActionResult Delete(int Id)
        {
            var delete = div.Delete(Id);
            if (delete > 0)
            {
                return Ok("Deleted successfully");
            }
            return BadRequest("Failed to delete division");
        }


    }
}
