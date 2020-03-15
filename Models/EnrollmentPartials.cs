using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Enrollments
    {
        public decimal? Balance =>
            (this.AvailableCourses?.TotalMiscellaneous ?? 0) - (this.Billings?.Sum(m => m.Amount) ?? 0);

        public ICollection<AvailableMiscellaneous> AvailableMiscellaneous => this.AvailableCourses?.AvailableMiscellaneous;
        public List<string> AvailableSubjectIds { get; set; }
    }
}
