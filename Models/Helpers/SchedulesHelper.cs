using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Repository;

namespace Helpers
{
    public class SchedulesHelper
    {

        private UnitOfWork unitOfWork = new UnitOfWork();
        private ModelDb modelDb = new ModelDb();

        void DeleteSchedule(AvailableSubjects item)
        {
            modelDb.Database.ExecuteSqlCommand("delete from schedules where availableSubjectId={0}", item.Id);
        }
        public void GenerateSchedule(AvailableSubjects item)
        {
            var availableCourse = unitOfWork.AvailableCoursesRepo.Find(m => m.Id == item.AvailableCourseId);

            var endDate = availableCourse.SchoolYears.EndDate;


            foreach (var x in item.Schedule.Split(','))
            {
                var startDate = availableCourse.SchoolYears.StartDate;
                while (startDate <= endDate)
                {
                    //separate time
                    var time = x.Split(' ')[0];
                    var dayOfWeek = x.Split(' ')[1].Split('-').Select(m => m.ToDayOfWeek()).ToList();
                    //separate day of weeks
                    foreach (var d in dayOfWeek)
                    {
                        if (dayOfWeek.Contains(startDate?.DayOfWeek))
                        {
                            item.Schedules.Add(new Schedules()
                            {
                                TimeIn = Convert.ToDateTime(startDate?.ToShortDateString() + " " + time.Split('-')[0]),
                                TimeOut = Convert.ToDateTime(startDate?.ToShortDateString() + " " + time.Split('-')[1])
                            });
                        }
                        startDate = startDate?.AddDays(1);
                    }
                 
                }

            }

        }
        public void UpdateSchedule(AvailableSubjects item)
        {
            DeleteSchedule(item);
            var availableCourse = unitOfWork.AvailableCoursesRepo.Find(m => m.Id == item.AvailableCourseId);

            var endDate = availableCourse.SchoolYears.EndDate;

            //  var startDate = availableCourse.SchoolYears.StartDate;
            List<Schedules> schedules = new List<Schedules>();

            //separate time
            foreach (var x in item.Schedule.Split(','))
            {
                var startDate = availableCourse.SchoolYears.StartDate;
                var time = x.Split(' ')[0];
                var dayOfWeek = x.Split(' ')[1].Split('-').Select(m => m.ToDayOfWeek()).ToList();

                //separate day of weeks
                while (startDate <= endDate)
                {
                    foreach (var d in dayOfWeek)
                    {

                        if (dayOfWeek.Contains(startDate?.DayOfWeek))
                        {

                            schedules.Add(new Schedules()
                            {
                                AvailableSubjectId = item.Id,
                                TimeIn = Convert.ToDateTime(startDate?.ToShortDateString() + " " + time.Split('-')[0]),
                                TimeOut = Convert.ToDateTime(startDate?.ToShortDateString() + " " + time.Split('-')[1])
                            });


                        }
                        startDate = startDate?.AddDays(1);
                    }
                }

            }
            unitOfWork.SchedulesRepo.InsertRange(schedules);
            unitOfWork.Save();
        }
    }
}
