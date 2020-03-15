using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class SchoolYears
    {
        public string SchoolYearAndSemester => this.SchoolYear + $"({this.Semesters?.Semester})";

        public string _IsActive => IsActive == true ? "Active" : "";
    }
}
