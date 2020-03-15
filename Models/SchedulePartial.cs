using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Schedules
    {
        public string Subject => this.AvailableSubjects?.SubjectCode + " - " + this.AvailableSubjects?.UserDetails?.FullName;
        public string Description => "";
        public int Label => 3;
        public int Status => 2;
    }
}
