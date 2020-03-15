//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AvailableCourses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AvailableCourses()
        {
            this.Enrollments = new HashSet<Enrollments>();
            this.AvailableMiscellaneous = new HashSet<AvailableMiscellaneous>();
            this.AvailableSubjects = new HashSet<AvailableSubjects>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> SchoolYearId { get; set; }
    
        public virtual Courses Courses { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrollments> Enrollments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvailableMiscellaneous> AvailableMiscellaneous { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvailableSubjects> AvailableSubjects { get; set; }
    }
}