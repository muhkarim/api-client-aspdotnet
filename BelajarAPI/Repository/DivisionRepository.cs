using BelajarAPI.Models;
using BelajarAPI.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BelajarAPI.Repository
{
    public class DivisionRepository : IDivisionRepository
    {

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameter = new DynamicParameters();

        public int Create(Division division)
        {
            var procName = "SP_InsertDivision";
            parameter.Add("@Name", division.Name);
            parameter.Add("@DepartmentId", division.DepartmentId);
            var create = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return create;

            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            var procName = "SP_DeleteDivision";
            parameter.Add("@Id", Id);
            var delete = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return delete;

            //throw new NotImplementedException();
        }

        public IEnumerable<DivisionViewModel> Get()
        {
            var procName = "SP_ViewDivision";
            var view = connection.Query<DivisionViewModel>(procName, commandType: CommandType.StoredProcedure);
            return view;


            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<DivisionViewModel>> Get(int Id)
        {
            var procName = "SP_GetDivisionById";
            parameter.Add("@Id", Id);
            var view = await connection.QueryAsync<DivisionViewModel>(procName, parameter, commandType: CommandType.StoredProcedure);
            return view;

            //throw new NotImplementedException();
        }

        public int Update(int Id, Division division)
        {
            var procName = "SP_UpdateDivision";
            parameter.Add("@newId", Id);
            parameter.Add("@newName", division.Name);
            parameter.Add("@newDepartmentId", division.DepartmentId);
            var update = connection.Execute(procName, parameter, commandType: CommandType.StoredProcedure);
            return update;

            //throw new NotImplementedException();
        }

    }
}