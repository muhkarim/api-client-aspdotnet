using BelajarAPI.Models;
using BelajarAPI.MyContext;
using BelajarAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BelajarAPI.Controllers
{
    public class DepartmentController : ApiController
    {
        myContext conn = new myContext();

        DepartmentRepository dept = new DepartmentRepository();

        [HttpGet]
        public IHttpActionResult Get()
        {
            if (dept.Get() == null)
            {
                return Content(HttpStatusCode.NotFound, "Data department is empty");
            }
            return Ok(dept.Get());
        }

        public IHttpActionResult Post(Department departments)
        {
            if (departments.Name == "" || departments.Name == null)
            {
                return Content(HttpStatusCode.NotFound, "Name cannot empty");
            }
            else
            {
                dept.Create(departments);
                return Ok("Department added successfully");
            }

            // old
            //var post = dept.Create(departments);
            //if (post > 0)
            //{
            //    return Ok("Department added successfully");
            //}
            //return BadRequest("Failed to add department");
        }


        [ResponseType(typeof(Department))]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            if (await dept.Get(Id) == null)
            {
                return null;
            }
            return await dept.Get(Id);
             
            //return await dept.Get(Id);
        }

        public IHttpActionResult Put(int Id, Department departments)
        {

            if (departments.Name == "" || departments.Name == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Name cannot empty");
            }
            else
            {
                dept.Update(Id, departments);
                return Ok("Update successfully");
            }
            
            
            // old
            //var update = dept.Update(Id, departments);
            //if (update > 0)
            //{
            //    return Ok("Update successfully");
            //}
            //return BadRequest("Failed to edit department");
        }


        public IHttpActionResult Delete(int Id)
        {
            var delete = dept.Delete(Id);
            if (delete > 0)
            {
                return Ok("Deleted successfully");
            }
            return BadRequest("Failed to delete department");
        }








    }
}
