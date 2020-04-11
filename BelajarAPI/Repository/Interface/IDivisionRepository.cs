using BelajarAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarAPI.Repository.Interface
{
    interface IDivisionRepository
    {
        IEnumerable<DivisionViewModel> Get(); // index
        Task<IEnumerable<DivisionViewModel>> Get(int Id); // getbyid
        int Create(Division division); 
        int Update(int Id, Division division);
        int Delete(int Id);
    }
}
