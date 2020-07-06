using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Repository;

namespace Helpers
{
    public class IdHelpers
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public string GenerateSubjectCode(int availableCourseId)
        {
            var course = unitOfWork.AvailableCoursesRepo.Find(m => m.Id == availableCourseId);
            var subjectCode = course.AvailableSubjects.OrderByDescending(m => m.Id).FirstOrDefault()?.SubjectCode;

            if (subjectCode != null)
            {
                subjectCode = subjectCode?.Replace(course?.Courses?.CourseCode, "");
                subjectCode = course?.Courses?.CourseCode + (subjectCode.ToInt() + 1);
            }
            else
                return course?.Courses?.CourseCode + "101";
            return subjectCode;
        }
        public int GenerateSchoolId()
        {
            //FGSC20-0
            //get last id entry
            int year = DateTime.Now.Year;
            var userDetail = unitOfWork.UserDetailsRepo.Fetch(x => x.EnrolledYear == year && x.SchoolId != null).OrderByDescending(x => x.IdNo).FirstOrDefault();
            if (userDetail == null)
                return 1;
            return (userDetail.IdNo ?? 0) + 1;







            //if (!unitOfWork.UserDetailsRepo.Fetch().Any())
            //    return "FGSC" + DateTime.Now.ToString("yy") + "-0";
            //var schoolId = "FGSC" + DateTime.Now.ToString("yy") + "-" + userDetail;
            //while (unitOfWork.UserDetailsRepo.Fetch(m => m.SchoolId == schoolId).Any())
            //{
            //    userDetail++;
            //    schoolId = "FGSC" + DateTime.Now.ToString("yy") + "-" + userDetail;
            //}
            //return schoolId;
        }


        /* public string GenerateSchoolId()
         {
             //FGSC20-0
             var userDetail = unitOfWork.UserDetailsRepo.Fetch().Count() + 1;
             if (!unitOfWork.UserDetailsRepo.Fetch().Any())
                 return "FGSC" + DateTime.Now.ToString("yy") + "-0";
             var schoolId = "FGSC" + DateTime.Now.ToString("yy") + "-" + userDetail;
             while (unitOfWork.UserDetailsRepo.Fetch(m => m.SchoolId == schoolId).Any())
             {
                 userDetail++;
                 schoolId = "FGSC" + DateTime.Now.ToString("yy") + "-" + userDetail;
             }
             return schoolId;
         }*/
    }
}
