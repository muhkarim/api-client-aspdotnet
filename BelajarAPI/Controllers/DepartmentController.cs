using BelajarAPI.Models;
using BelajarAPI.MyContext;
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
        myContext conn = new myContext();

        DepartmentRepository dept = new DepartmentRepository();

        [HttpGet]
        public IEnumerable<Department>Get()
        {
            return dept.Get();
        }

        public IHttpActionResult Post(Department departments)
        {
            if (departments.Name == "" || departments.Name == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Name cannot empty");
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


        [HttpGet]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            
            //var department = conn.Departments.FirstOrDefault(x => x.Id == Id);

            //if(department == null)
            //{
            //    return await 
            //}
            
            return await dept.Get(Id);
            

        }

        public IHttpActionResult Put(int Id, Department departments)
        {
            var dept_id = conn.Departments.FirstOrDefault(x => x.Id == Id);

            if(dept_id == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Id not found");
            } 
            else if (departments.Name == "" || departments.Name == null)
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
            var dept_id = conn.Departments.FirstOrDefault(x => x.Id == Id);

            if(dept_id == null)
            {
                return BadRequest("Failed to delete department");
            }
            else
            {
                dept.Delete(Id);
                return Ok("Deleted successfully");
            }

            //old
            //var delete = dept.Delete(Id);
            //if (delete > 0)
            //{
            //    return Ok("Deleted successfully");
            //}
            //return BadRequest("Failed to delete department");
        }








    }
}
