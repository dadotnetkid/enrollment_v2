using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Courses
    {
        public int? EnrolledStudents => this.AvailableCourses.Sum(x => x.EnrolledStudents);
    }
    public partial class AvailableCourses
    {
        public string AvailableCourse => this.Courses?.Course + " - " + this.SchoolYears?.SchoolYearAndSemester;
        public decimal? TotalMiscellaneous => this.AvailableMiscellaneous.Sum(m => m.Miscellaneous.Cost);
        public int? EnrolledStudents => this.Enrollments.Count();

        public int AvailableCourseId { get; set; }
        public int DestinationAvailableCourseId { get; set; }
    }
}
