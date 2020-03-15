using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class UserDetails
    {
        private List<string> _userRoles;
        public string FullName => this.FirstName + " " + this.LastName;

        public List<string> UserRoles
        {


            get
            {
                if (_userRoles == null)
                {
                    _userRoles = this.Users?.UserRoles?.Select(x => x.Name).ToList();
                }
                return _userRoles;
            }
            set => _userRoles = value;
        }

      

    }
}
