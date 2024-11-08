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
    
    public partial class HR_SALARY_Shift
    {
        public HR_SALARY_Shift()
        {
            this.HR_ContractDetail = new HashSet<HR_ContractDetail>();
            this.HR_Employee = new HashSet<HR_Employee>();
        }
    
        public int ShiftID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string ShiftName { get; set; }
        public byte DefaultEvaluateMark { get; set; }
        public Nullable<int> TimeDelay { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> MondayHourStart { get; set; }
        public Nullable<int> MondayHourEnd { get; set; }
        public Nullable<bool> MondayHoursNextDay1 { get; set; }
        public Nullable<int> MondayHourLunchBreakStart { get; set; }
        public Nullable<int> MondayHourLunchBreakEnd { get; set; }
        public Nullable<bool> MondayHoursNextDay2 { get; set; }
        public Nullable<int> TuesdayHourStart { get; set; }
        public Nullable<int> TuesdayHourEnd { get; set; }
        public Nullable<bool> TuesdayHoursNextDay1 { get; set; }
        public Nullable<int> TuesdayHourLunchBreakStart { get; set; }
        public Nullable<int> TuesdayHourLunchBreakEnd { get; set; }
        public Nullable<bool> TuesdayHoursNextDay2 { get; set; }
        public Nullable<int> WednesdayHourStart { get; set; }
        public Nullable<int> WednesdayHourEnd { get; set; }
        public Nullable<bool> WednesdayHoursNextDay1 { get; set; }
        public Nullable<int> WednesdayHourLunchBreakStart { get; set; }
        public Nullable<int> WednesdayHourLunchBreakEnd { get; set; }
        public Nullable<bool> WednesdayHoursNextDay2 { get; set; }
        public Nullable<int> ThursdayHourStart { get; set; }
        public Nullable<int> ThursdayHourEnd { get; set; }
        public Nullable<bool> ThursdayHoursNextDay1 { get; set; }
        public Nullable<int> ThursdayHourLunchBreakStart { get; set; }
        public Nullable<int> ThursdayHourLunchBreakEnd { get; set; }
        public Nullable<bool> ThursdayHoursNextDay2 { get; set; }
        public Nullable<int> FridayHourStart { get; set; }
        public Nullable<int> FridayHourEnd { get; set; }
        public Nullable<bool> FridayHoursNextDay1 { get; set; }
        public Nullable<int> FridayHourLunchBreakStart { get; set; }
        public Nullable<int> FridayHourLunchBreakEnd { get; set; }
        public Nullable<bool> FridayHoursNextDay2 { get; set; }
        public Nullable<int> SaturdayHourStart { get; set; }
        public Nullable<int> SaturdayHourEnd { get; set; }
        public Nullable<bool> SaturdayHoursNextDay1 { get; set; }
        public Nullable<int> SaturdayHourLunchBreakStart { get; set; }
        public Nullable<int> SaturdayHourLunchBreakEnd { get; set; }
        public Nullable<bool> SaturdayHoursNextDay2 { get; set; }
        public Nullable<int> MondayMinuteStart { get; set; }
        public Nullable<int> MondayMinuteEnd { get; set; }
        public Nullable<bool> MondayMinuteNextDay1 { get; set; }
        public Nullable<bool> MondayMinuteNextDay2 { get; set; }
        public Nullable<int> MondayMinuteLunchBreakStart { get; set; }
        public Nullable<int> MondayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> TuesdayMinuteStart { get; set; }
        public Nullable<int> TuesdayMinuteEnd { get; set; }
        public Nullable<bool> TuesdayMinuteNextDay1 { get; set; }
        public Nullable<bool> TuesdayMinuteNextDay2 { get; set; }
        public Nullable<int> TuesdayMinuteLunchBreakStart { get; set; }
        public Nullable<int> TuesdayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> WednesdayMinuteStart { get; set; }
        public Nullable<int> WednesdayMinuteEnd { get; set; }
        public Nullable<bool> WednesdayMinuteNextDay1 { get; set; }
        public Nullable<bool> WednesdayMinuteNextDay2 { get; set; }
        public Nullable<int> WednesdayMinuteLunchBreakStart { get; set; }
        public Nullable<int> WednesdayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> ThursdayMinuteStart { get; set; }
        public Nullable<int> ThursdayMinuteEnd { get; set; }
        public Nullable<bool> ThursdayMinuteNextDay1 { get; set; }
        public Nullable<bool> ThursdayMinuteNextDay2 { get; set; }
        public Nullable<int> ThursdayMinuteLunchBreakStart { get; set; }
        public Nullable<int> ThursdayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> FridayMinuteStart { get; set; }
        public Nullable<int> FridayMinuteEnd { get; set; }
        public Nullable<bool> FridayMinuteNextDay1 { get; set; }
        public Nullable<bool> FridayMinuteNextDay2 { get; set; }
        public Nullable<int> FridayMinuteLunchBreakStart { get; set; }
        public Nullable<int> FridayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> SaturdayMinuteStart { get; set; }
        public Nullable<int> SaturdayMinuteEnd { get; set; }
        public Nullable<int> SaturdayMinuteLunchBreakStart { get; set; }
        public Nullable<int> SaturdayMinuteLunchBreakEnd { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<double> UnitTime { get; set; }
        public Nullable<int> SundayHourStart { get; set; }
        public Nullable<int> SundayHourEnd { get; set; }
        public Nullable<int> SundayMinuteStart { get; set; }
        public Nullable<int> SundayMinuteEnd { get; set; }
        public Nullable<bool> SundayHoursNextDay1 { get; set; }
        public Nullable<int> SundayHourLunchBreakStart { get; set; }
        public Nullable<int> SundayHourLunchBreakEnd { get; set; }
        public Nullable<bool> SundayHoursNextDay2 { get; set; }
        public Nullable<int> SundayMinuteLunchBreakStart { get; set; }
        public Nullable<int> SundayMinuteLunchBreakEnd { get; set; }
        public Nullable<int> TimeTypeGroupId { get; set; }
    
        public virtual ICollection<HR_ContractDetail> HR_ContractDetail { get; set; }
        public virtual ICollection<HR_Employee> HR_Employee { get; set; }
        public virtual HR_TimeTypeGroup HR_TimeTypeGroup { get; set; }
    }
}
