//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlluringDecors.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DesignType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DesignType()
        {
            this.Projects = new HashSet<Project>();
        }
    
        public int ID { get; set; }
        public string DesignTypeName { get; set; }
        public int ProjectCategoryID { get; set; }
        public decimal Price { get; set; }
    
        public virtual ProjectCategory ProjectCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
    }
}