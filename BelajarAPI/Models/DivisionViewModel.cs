using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelajarAPI.Models
{
    public class DivisionViewModel
    {
        public int Id { get; set; }
        public string DivisionName { get; set; }
        public string DepartmentName {get; set;}
        public int DepartmentId { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }



    }
}