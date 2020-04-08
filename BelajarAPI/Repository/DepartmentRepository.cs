using BelajarAPI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BelajarAPI.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;

namespace BelajarAPI.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameter = new DynamicParameters();



        public int Create(Department department)
        {
            var procName = "SP_InsertDepartment";
            parameter.Add("@Name", department.Name);
            var create = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return create;
            
            
            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            var procName = "SP_DeleteDepartment";
            parameter.Add("@Id", Id);
            var delete = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return delete;
        }

        public IEnumerable<Department> Get()
        {
            var procName = "SP_ViewDepartment";
            var view = connection.Query<Department>(procName, commandType: CommandType.StoredProcedure);
            return view;
            
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<Department>> Get(int Id)
        {
            var procName = "SP_GetDepartmentById";
            parameter.Add("@Id", Id);
            var view = await connection.QueryAsync<Department>(procName, parameter, commandType: CommandType.StoredProcedure);
            return view;

            //throw new NotImplementedException();
        }

        public int Update(int Id, Department department)
        {
            var procName = "SP_UpdateDepartment";
            parameter.Add("@newId", Id);
            parameter.Add("@newName", department.Name);
            var update = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return update;


            //throw new NotImplementedException();
        }
    }
}