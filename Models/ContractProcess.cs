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
    
    public partial class ContractProcess
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public string TotalProcess { get; set; }
        public string Status { get; set; }
    
        public virtual Contract Contract { get; set; }
    }
}