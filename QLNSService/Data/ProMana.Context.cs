﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLNSService.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProManaEntities : DbContext
    {
        public ProManaEntities()
            : base("name=ProManaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<P_Config> P_Config { get; set; }
        public DbSet<P_DataCenter> P_DataCenter { get; set; }
        public DbSet<P_Job> P_Job { get; set; }
        public DbSet<P_JobGroup> P_JobGroup { get; set; }
        public DbSet<P_PM_EmployeeReference> P_PM_EmployeeReference { get; set; }
        public DbSet<P_PM_Job> P_PM_Job { get; set; }
        public DbSet<P_PM_JobDetail> P_PM_JobDetail { get; set; }
        public DbSet<P_PM_JobGroup> P_PM_JobGroup { get; set; }
        public DbSet<P_ProManagement> P_ProManagement { get; set; }
    }
}
