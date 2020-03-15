using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public partial class RegisterViewModel
    {
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string CompanyName { get; set; }
        public string Address { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public string Roles { get; set; }
    }

    public enum RegistrationType
    {
        Company = 0,
        Individual = 1
    }
}
