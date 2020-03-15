using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Repository;

namespace Models
{
    public partial class AvailableSubjects
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public IEnumerable<AvailableCourses> GetAvailableCourses => unitOfWork.AvailableCoursesRepo.Get();
        public IEnumerable<Subjects> GetSubjects => unitOfWork.SubjectsRepo.Get();
    }
}
