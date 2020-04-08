using BelajarAPI.Models;
using BelajarAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BelajarAPI.Controllers
{
    public class DepartmentController : ApiController
    {

        DepartmentRepository dept = new DepartmentRepository();

        [HttpGet]
        public IEnumerable<Department>Get()
        {
            return dept.Get();
        }

        public IHttpActionResult Post(Department departments)
        {
            var post = dept.Create(departments);
            if(post > 0)
            {
                return Ok("Department added successfully");
            }
            return BadRequest("Failed to add department");
        }


        [HttpGet]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            return await dept.Get(Id);
            
        }

        public IHttpActionResult Put(int Id, Department departments)
        {
            var update = dept.Update(Id, departments);
            if (update > 0)
            {
                return Ok("Update successfully");
            }
            return BadRequest("Failed to edit department");
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
