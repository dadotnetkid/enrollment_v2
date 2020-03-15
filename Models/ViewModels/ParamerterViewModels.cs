using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ParamerterViewModels
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public dynamic RouteValue { get; set; }
        public int? Id { get; set; }
    }
}
