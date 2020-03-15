using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Models
{
    public partial class UserRoles : IRole<string>
    {
        public string Priviledges => string.Join(", ", this.UserRolesInActions.Select(x => x.Action));
    }
}
