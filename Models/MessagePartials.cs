using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Repository;

namespace Models
{
    public partial class Messages
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public string _MessageFrom => unitOfWork.UsersRepo.Fetch(m => m.Id == MessageFrom)
            .Select(x => new { x.UserDetails.FirstName }).FirstOrDefault()?.FirstName ?? MessageFrom;

        public bool? IsMail { get; set; }
        public string Body => $"{Subject}-{Message}";
    }
}
