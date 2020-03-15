using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class ModelDb
    {
        
        public static ModelDb Create()
        {
            return new ModelDb();
        }

        public static void SetConnection(string connection,string userName,string password)
        {
           
        }
    }
}
