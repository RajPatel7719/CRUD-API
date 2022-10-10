using System;
using System.Collections.Generic;

namespace CRUD.Model.Models
{
    public partial class Employee
    {
        public int EmpId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
