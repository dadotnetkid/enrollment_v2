using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Models.Repository
{
    public partial class UnitOfWork : IDisposable
    {
        private ModelDb context = new ModelDb();


        private GenericRepository<Credits> _creditsRepo;
        public GenericRepository<Credits> CreditsRepo
        {
            get
            {
                if (this._creditsRepo == null)
                    this._creditsRepo = new GenericRepository<Credits>(context);
                return _creditsRepo;
            }
            set { _creditsRepo = value; }
        }


        private GenericRepository<UserDetails> _UserDetailsRepo;
        public GenericRepository<UserDetails> UserDetailsRepo
        {
            get
            {
                if (this._UserDetailsRepo == null)
                    this._UserDetailsRepo = new GenericRepository<UserDetails>(context);
                return _UserDetailsRepo;
            }
            set { _UserDetailsRepo = value; }
        }


        private GenericRepository<Schedules> _SchedulesRepo;
        public GenericRepository<Schedules> SchedulesRepo
        {
            get
            {
                if (this._SchedulesRepo == null)
                    this._SchedulesRepo = new GenericRepository<Schedules>(context);
                return _SchedulesRepo;
            }
            set { _SchedulesRepo = value; }
        }

        private GenericRepository<Announcements> _AnnouncementsRepo;
        public GenericRepository<Announcements> AnnouncementsRepo
        {
            get
            {
                if (this._AnnouncementsRepo == null)
                    this._AnnouncementsRepo = new GenericRepository<Announcements>(context);
                return _AnnouncementsRepo;
            }
            set { _AnnouncementsRepo = value; }
        }


        private GenericRepository<Messages> _MessagesRepo;
        public GenericRepository<Messages> MessagesRepo
        {
            get
            {
                if (this._MessagesRepo == null)
                    this._MessagesRepo = new GenericRepository<Messages>(context);
                return _MessagesRepo;
            }
            set { _MessagesRepo = value; }
        }

        private GenericRepository<AvailableMiscellaneous> _AvailableMiscellaneousRepo;
        public GenericRepository<AvailableMiscellaneous> AvailableMiscellaneousRepo
        {
            get
            {
                if (this._AvailableMiscellaneousRepo == null)
                    this._AvailableMiscellaneousRepo = new GenericRepository<AvailableMiscellaneous>(context);
                return _AvailableMiscellaneousRepo;
            }
            set { _AvailableMiscellaneousRepo = value; }
        }

        private GenericRepository<Miscellaneous> _MiscellaneousRepo;
        public GenericRepository<Miscellaneous> MiscellaneousRepo
        {
            get
            {
                if (this._MiscellaneousRepo == null)
                    this._MiscellaneousRepo = new GenericRepository<Miscellaneous>(context);
                return _MiscellaneousRepo;
            }
            set { _MiscellaneousRepo = value; }
        }

        private GenericRepository<Billings> _BillingsRepo;
        public GenericRepository<Billings> BillingsRepo
        {
            get
            {
                if (this._BillingsRepo == null)
                    this._BillingsRepo = new GenericRepository<Billings>(context);
                return _BillingsRepo;
            }
            set { _BillingsRepo = value; }
        }

        private GenericRepository<Grades> _GradesRepo;
        public GenericRepository<Grades> GradesRepo
        {
            get
            {
                if (this._GradesRepo == null)
                    this._GradesRepo = new GenericRepository<Grades>(context);
                return _GradesRepo;
            }
            set { _GradesRepo = value; }
        }


        private GenericRepository<AvailableSubjects> _AvailableSubjectsRepo;
        public GenericRepository<AvailableSubjects> AvailableSubjectsRepo
        {
            get
            {
                if (this._AvailableSubjectsRepo == null)
                    this._AvailableSubjectsRepo = new GenericRepository<AvailableSubjects>(context);
                return _AvailableSubjectsRepo;
            }
            set { _AvailableSubjectsRepo = value; }
        }

        private GenericRepository<AvailableCourses> _AvailableCoursesRepo;
        public GenericRepository<AvailableCourses> AvailableCoursesRepo
        {
            get
            {
                if (this._AvailableCoursesRepo == null)
                    this._AvailableCoursesRepo = new GenericRepository<AvailableCourses>(context);
                return _AvailableCoursesRepo;
            }
            set { _AvailableCoursesRepo = value; }
        }

        private GenericRepository<EnrolledSubject> _EnrolledSubjectRepo;
        public GenericRepository<EnrolledSubject> EnrolledSubjectRepo
        {
            get
            {
                if (this._EnrolledSubjectRepo == null)
                    this._EnrolledSubjectRepo = new GenericRepository<EnrolledSubject>(context);
                return _EnrolledSubjectRepo;
            }
            set { _EnrolledSubjectRepo = value; }
        }

        private GenericRepository<Enrollments> _EnrollmentsRepo;
        public GenericRepository<Enrollments> EnrollmentsRepo
        {
            get
            {
                if (this._EnrollmentsRepo == null)
                    this._EnrollmentsRepo = new GenericRepository<Enrollments>(context);
                return _EnrollmentsRepo;
            }
            set { _EnrollmentsRepo = value; }
        }



        private GenericRepository<Courses> _CoursesRepo;
        public GenericRepository<Courses> CoursesRepo
        {
            get
            {
                if (this._CoursesRepo == null)
                    this._CoursesRepo = new GenericRepository<Courses>(context);
                return _CoursesRepo;
            }
            set { _CoursesRepo = value; }
        }

        private GenericRepository<Subjects> _SubjectsRepo;
        public GenericRepository<Subjects> SubjectsRepo
        {
            get
            {
                if (this._SubjectsRepo == null)
                    this._SubjectsRepo = new GenericRepository<Subjects>(context);
                return _SubjectsRepo;
            }
            set { _SubjectsRepo = value; }
        }
        private GenericRepository<SchoolYears> _SchoolYearsRepo;
        public GenericRepository<SchoolYears> SchoolYearsRepo
        {
            get
            {
                if (this._SchoolYearsRepo == null)
                    this._SchoolYearsRepo = new GenericRepository<SchoolYears>(context);
                return _SchoolYearsRepo;
            }
            set { _SchoolYearsRepo = value; }
        }

        private GenericRepository<Semesters> _SemestersRepo;
        public GenericRepository<Semesters> SemestersRepo
        {
            get
            {
                if (this._SemestersRepo == null)
                    this._SemestersRepo = new GenericRepository<Semesters>(context);
                return _SemestersRepo;
            }
            set { _SemestersRepo = value; }
        }

        private GenericRepository<Logs> _LogsRepo;
        public GenericRepository<Logs> LogsRepo
        {
            get
            {
                if (this._LogsRepo == null)
                    this._LogsRepo = new GenericRepository<Logs>(context);
                return _LogsRepo;
            }
            set { _LogsRepo = value; }
        }

        private GenericRepository<Provinces> _ProvincesRepo;
        public GenericRepository<Provinces> ProvincesRepo
        {
            get
            {
                if (this._ProvincesRepo == null)
                    this._ProvincesRepo = new GenericRepository<Provinces>(context);
                return _ProvincesRepo;
            }
            set { _ProvincesRepo = value; }
        }

        private GenericRepository<Towns> _TownsRepo;
        public GenericRepository<Towns> TownsRepo
        {
            get
            {
                if (this._TownsRepo == null)
                    this._TownsRepo = new GenericRepository<Towns>(context);
                return _TownsRepo;
            }
            set { _TownsRepo = value; }
        }

        private GenericRepository<Actions> _ActionsRepo;
        public GenericRepository<Actions> ActionsRepo
        {
            get
            {
                if (this._ActionsRepo == null)
                    this._ActionsRepo = new GenericRepository<Actions>(context);
                return _ActionsRepo;
            }
            set { _ActionsRepo = value; }
        }

        private GenericRepository<UserRolesInActions> _UserRolesInActionsRepo;
        public GenericRepository<UserRolesInActions> UserRolesInActionsRepo
        {
            get
            {
                if (this._UserRolesInActionsRepo == null)
                    this._UserRolesInActionsRepo = new GenericRepository<UserRolesInActions>(context);
                return _UserRolesInActionsRepo;
            }
            set { _UserRolesInActionsRepo = value; }
        }




        private GenericRepository<ControllersActions> _ControllersActionsRepo;
        public GenericRepository<ControllersActions> ControllersActionsRepo
        {
            get
            {
                if (this._ControllersActionsRepo == null)
                    this._ControllersActionsRepo = new GenericRepository<ControllersActions>(context);
                return _ControllersActionsRepo;
            }
            set { _ControllersActionsRepo = value; }
        }



        private GenericRepository<Items> _ItemsRepo;
        public GenericRepository<Items> ItemsRepo
        {
            get
            {
                if (this._ItemsRepo == null)
                    this._ItemsRepo = new GenericRepository<Items>(context);
                return _ItemsRepo;
            }
            set { _ItemsRepo = value; }
        }



        private GenericRepository<Users> usersRepo;
        public GenericRepository<Users> UsersRepo
        {
            get
            {
                if (this.usersRepo == null)
                    this.usersRepo = new GenericRepository<Users>(context);
                return usersRepo;
            }
            set { usersRepo = value; }
        }

        private GenericRepository<UserRoles> userRolesRepo;
        public GenericRepository<UserRoles> UserRolesRepo
        {
            get
            {
                if (this.userRolesRepo == null)
                    this.userRolesRepo = new GenericRepository<UserRoles>(context);
                return userRolesRepo;
            }
            set { userRolesRepo = value; }
        }




        public void Save()
        {
            context.SaveChanges();

        }


        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            context.Database.ExecuteSqlCommand(sql, parameters);
        }
    }

}