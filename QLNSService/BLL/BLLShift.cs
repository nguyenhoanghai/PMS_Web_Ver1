using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLShift
    {
        private QLNSEntities qlnsEntities;
        public BLLShift()
        {
            qlnsEntities = new QLNSEntities();
        }

        public List<LineWorkingShiftModel> GetShiftsOfLine(int lineId)
        {
            try
            {
                return qlnsEntities.P_LineWorkingShift.Where(x => !x.IsDeleted && !x.Chuyen.IsDeleted && !x.P_WorkingShift.IsDeleted && x.LineId == lineId).Select(x => new LineWorkingShiftModel()
                {
                    LineId = x.LineId,
                    Start = x.P_WorkingShift.TimeStart,
                    End = x.P_WorkingShift.TimeEnd,
                    Id = x.Id,
                    ShiftId = x.ShiftId,
                    ShiftOrder = x.ShiftOrder
                }).OrderBy(x => x.ShiftOrder).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountWorkHoursInDayOfLine(int lineId)
        {
            try
            {
                int countHours = 0;
                var shifts = GetShiftsOfLine(lineId);
                if (shifts != null && shifts.Count > 0)
                {
                    double hours = 0;
                    foreach (var item in shifts.OrderBy(x => x.ShiftOrder))
                    {
                        hours += item.End.TotalHours - item.Start.TotalHours;
                    }
                    countHours = (int)hours;
                    if (countHours < hours)
                        countHours++;
                }
                return countHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkingTimeModel> GetListWorkHoursOfLineByLineId(int lineId)
        {
            try
            {
                List<WorkingTimeModel> listWorkHours = null;
                var listShiftOfLine = GetShiftsOfLine(lineId);// qlnsEntities.Shifts.Where(c => c.MaChuyen == lineId).OrderBy(c => c.TimeStart).ToList();


                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    double dHoursShiftOld = 0;
                    for (int i = 0; i < listShiftOfLine.Count; i++)
                    {
                        var shift = listShiftOfLine[i];
                        while (true)
                        {
                            if (!isWaitingTimeEnd)
                            {
                                if (timeStart == new TimeSpan(0, 0, 0))
                                    timeStart = shift.Start;
                                else
                                    timeStart = timeEnd;
                            }
                            else
                            {
                                if (dHoursShiftOld == 0)
                                    timeStart = shift.Start;
                            }
                            if (timeStart > shift.End)
                            {
                                break;
                            }
                            else
                            {
                                if (!isWaitingTimeEnd)
                                    timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                else
                                {
                                    if (dHoursShiftOld > 0)
                                    {
                                        double hour = 1 - dHoursShiftOld;
                                        int minuter = (int)(hour * 60);
                                        timeEnd = shift.Start.Add(new TimeSpan(0, minuter, 0));
                                    }
                                    else
                                        timeEnd = timeStart.Add(new TimeSpan(1, 0, 0));
                                    isWaitingTimeEnd = false;
                                }
                                if (timeEnd <= shift.End)
                                {
                                    listWorkHours.Add(new WorkingTimeModel()
                                    { 
                                        IntHours = intHours,
                                        TimeStart = timeStart,
                                        TimeEnd = timeEnd,
                                        Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                        IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                    });
                                    intHours++;
                                }
                                else
                                {
                                    isWaitingTimeEnd = true;
                                    dHoursShiftOld = shift.End.TotalHours - timeStart.TotalHours;
                                    if (dHoursShiftOld != 0 && i == listShiftOfLine.Count - 1)
                                    {
                                        listWorkHours.Add(new WorkingTimeModel()
                                        {
                                            IntHours = intHours,
                                            TimeStart = timeStart,
                                            TimeEnd = timeEnd,
                                            Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                            IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                        });
                                        intHours++;
                                    }
                                    break;
                                }
                            }
                            if (intHours > 30)
                                break;
                        }
                    }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountWorkHoursMaxOfLines(List<int> listLineId)
        {
            try
            {
                int maxWorkHours = 0;
                if (listLineId != null && listLineId.Count > 0)
                {
                    foreach (var lineId in listLineId)
                    {
                        int workHours = CountWorkHoursInDayOfLine(lineId);
                        if (workHours > maxWorkHours)
                            maxWorkHours = workHours;
                    }
                }
                return maxWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TimeSpan GetTotalWorkingHourOfLine(int lineId)
        {
            try
            {
                TimeSpan timeWork = new TimeSpan();
                timeWork = TimeSpan.Parse("00:00:00");
                var listShift = GetShiftsOfLine(lineId);
                if (listShift != null && listShift.Count > 0)
                {
                    for (int j = 0; j < listShift.Count; j++)
                    {
                        timeWork += listShift[j].End - listShift[j].Start;
                    }
                }
                return timeWork;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorkingTimeModel> GetListWorkHoursOfLineByLineId(int lineId, int solan)
        {
            try
            {
                List<WorkingTimeModel> listWorkHours = null;
                var listShiftOfLine = GetShiftsOfLine(lineId);
                if (listShiftOfLine != null && listShiftOfLine.Count > 0)
                {
                    TimeSpan timeWork = new TimeSpan();
                    timeWork = TimeSpan.Parse("00:00:00");
                    int secondsEachTimes = 0;
                    foreach (var s in listShiftOfLine)
                    {
                        timeWork += s.End - s.Start;
                    }
                    secondsEachTimes = (int)timeWork.TotalSeconds / solan;

                    //
                    listWorkHours = new List<WorkingTimeModel>();
                    int intHours = 1;
                    TimeSpan timeEnd = new TimeSpan(0, 0, 0);
                    TimeSpan timeStart = new TimeSpan(0, 0, 0);
                    bool isWaitingTimeEnd = false;
                    int secondsOfShift = 0, sodu = 0;
                    foreach (var item in listShiftOfLine)
                    {
                        secondsOfShift = (int)(item.End - item.Start).TotalSeconds;
                        if (listWorkHours.Count > 0 || (isWaitingTimeEnd && sodu > 0))
                        {
                            var last = listWorkHours.LastOrDefault();
                            if (last != null || (isWaitingTimeEnd && sodu > 0))
                            {
                                var gio_nghi = (item.Start - listShiftOfLine[0].End).TotalSeconds;
                                if (isWaitingTimeEnd)
                                {
                                    DateTime dt = DateTime.ParseExact(last != null ? last.TimeEnd.ToString() : timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(gio_nghi + secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel()
                                    {
                                        IntHours = intHours,
                                        TimeStart = timeStart,
                                        TimeEnd = timeEnd,
                                        Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                        IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                    });
                                    intHours++;
                                    isWaitingTimeEnd = false;

                                    secondsOfShift -= (secondsEachTimes - sodu);
                                    timeStart = timeEnd;
                                    sodu = 0;

                                    if (secondsOfShift > secondsEachTimes)
                                    {
                                        for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                        {
                                            dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                            timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                            listWorkHours.Add(new WorkingTimeModel()
                                            {
                                                IntHours = intHours,
                                                TimeStart = timeStart,
                                                TimeEnd = timeEnd,
                                                Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                                IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                            });
                                            intHours++;
                                            isWaitingTimeEnd = false;
                                            timeStart = timeEnd;

                                            if ((i + secondsEachTimes) >= secondsOfShift)
                                            {
                                                sodu = secondsOfShift - i;
                                                isWaitingTimeEnd = true;
                                                break;
                                            }
                                        }
                                    }
                                    else if (secondsEachTimes == secondsOfShift)
                                    {
                                        dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel()
                                        {
                                            IntHours = intHours,
                                            TimeStart = timeStart,
                                            TimeEnd = timeEnd,
                                            Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                            IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                        });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = timeEnd;
                                    }
                                    else
                                    {
                                        if (secondsOfShift != 0)
                                        {
                                            sodu = secondsOfShift;
                                            isWaitingTimeEnd = true;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                    {
                                        if (timeStart == new TimeSpan(0, 0, 0))
                                            timeStart = item.Start;

                                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                        timeEnd = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;
                                        listWorkHours.Add(new WorkingTimeModel()
                                        {
                                            IntHours = intHours,
                                            TimeStart = timeStart,
                                            TimeEnd = timeEnd,
                                            Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                            IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                        });
                                        intHours++;
                                        isWaitingTimeEnd = false;
                                        timeStart = dt.AddSeconds(i - secondsEachTimes).TimeOfDay;

                                        if ((secondsOfShift % secondsEachTimes) == (secondsOfShift - i))
                                        {
                                            sodu = secondsOfShift - i;
                                            isWaitingTimeEnd = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (timeStart == new TimeSpan(0, 0, 0))
                                timeStart = item.Start;
                            if (secondsEachTimes > secondsOfShift)
                            {
                                sodu = secondsOfShift;
                                isWaitingTimeEnd = true;
                            }
                            else if (secondsOfShift == secondsEachTimes)
                            {
                                DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                listWorkHours.Add(new WorkingTimeModel()
                                {
                                    IntHours = intHours,
                                    TimeStart = timeStart,
                                    TimeEnd = timeEnd,
                                    Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                    IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                });
                                intHours++;
                                isWaitingTimeEnd = false;
                                timeStart = timeEnd;
                            }
                            else
                            {
                                for (int i = secondsEachTimes; i < secondsOfShift; i += secondsEachTimes)
                                {

                                    DateTime dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                                    timeEnd = dt.AddSeconds(secondsEachTimes).TimeOfDay;
                                    listWorkHours.Add(new WorkingTimeModel()
                                    {
                                        IntHours = intHours,
                                        TimeStart = timeStart,
                                        TimeEnd = timeEnd,
                                        Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                                        IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                                    });
                                    intHours++;
                                    isWaitingTimeEnd = false;
                                    timeStart = timeEnd;

                                    if ((i + secondsEachTimes) > secondsOfShift)
                                    {
                                        sodu = secondsOfShift - i;
                                        isWaitingTimeEnd = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (isWaitingTimeEnd)
                    {
                        var dt = DateTime.ParseExact(timeStart.ToString(), "HH:mm:ss", null);
                        timeEnd = dt.AddSeconds(sodu).TimeOfDay;
                        listWorkHours.Add(new WorkingTimeModel()
                        {
                            IntHours = intHours,
                            TimeStart = timeStart,
                            TimeEnd = timeEnd,
                            Name = "Giờ " + intHours + " (" + timeStart.ToString(@"hh\:mm") + "-" + timeEnd.ToString(@"hh\:mm") + ")",
                            IsShow = (DateTime.Now.TimeOfDay > timeStart && DateTime.Now.TimeOfDay < timeEnd) ? true : false
                        });
                    }
                }
                return listWorkHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}