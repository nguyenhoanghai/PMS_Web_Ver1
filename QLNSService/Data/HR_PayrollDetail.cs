//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class HR_PayrollDetail
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public int CycleWageDetailId { get; set; }
        public Nullable<int> PayrollDataId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal SISalary { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal TotalAllowanceHasTax { get; set; }
        public decimal IISalary { get; set; }
        public decimal IISalaryHasTax { get; set; }
        public decimal NETSalary { get; set; }
        public decimal Mobivi { get; set; }
        public decimal MobiviAllowances { get; set; }
        public decimal DeInsurance { get; set; }
        public decimal DeTax { get; set; }
        public Nullable<decimal> TotalDeductionFamily { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal OverTimeSalary { get; set; }
    
        public virtual HR_Employee HR_Employee { get; set; }
        public virtual HR_Payroll HR_Payroll { get; set; }
        public virtual HR_PayrollCycleWageDetail HR_PayrollCycleWageDetail { get; set; }
        public virtual HR_PayrollData HR_PayrollData { get; set; }
    }
}
