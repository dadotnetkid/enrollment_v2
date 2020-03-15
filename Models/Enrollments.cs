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
    
    public partial class Enrollments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Enrollments()
        {
            this.Billings = new HashSet<Billings>();
            this.EnrolledSubject = new HashSet<EnrolledSubject>();
        }
    
        public int Id { get; set; }
        public string StudentId { get; set; }
        public Nullable<int> AvailableCourseId { get; set; }
        public Nullable<bool> IsDrop { get; set; }
    
        public virtual AvailableCourses AvailableCourses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Billings> Billings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrolledSubject> EnrolledSubject { get; set; }
        public virtual UserDetails UserDetails { get; set; }
    }
}
