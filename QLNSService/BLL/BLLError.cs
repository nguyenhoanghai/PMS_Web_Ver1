using QLNSService.Data;
using QLNSService.Enum;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLError
    {
        private QLNSEntities db;
        private BLLShift bllShift;
        public BLLError()
        {
            db = new QLNSEntities();
            bllShift = new BLLShift();
        }
        public List<ModelSelectItem> GetErrors()
        {
            try
            {
                return db.Errors.Select(x => new ModelSelectItem()
                {
                    Value = x.Id,
                    Name = x.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelGroupError> GetErrorOfLine(int lineId)
        {
            try
            {
                var listGroupError = new List<ModelGroupError>();
                var groupErrors = db.GroupErrors.Where(c => !c.IsDeleted).ToList();
                var line = db.Chuyens.Where(c => c.MaChuyen == lineId && !c.IsDeleted).FirstOrDefault();
                if (groupErrors.Count > 0 && line != null)
                {
                    foreach (var group in groupErrors)
                    {
                        var listError = db.Errors.Where(c => c.GroupErrorId == group.Id && !c.IsDeleted).Select(c => new ModelError()
                        {
                            Id = c.Id,
                            ErrorName = c.Name
                        }).ToList();
                        if (listError.Count > 0)
                        {
                            var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                            DateTime dateNow = DateTime.Now;
                            TimeSpan timeNow = dateNow.TimeOfDay;
                            TimeSpan timeNSSEnd = timeNow.Add(new TimeSpan(0, 30, 0));
                            TimeSpan timeNSStart = timeNow.Add(new TimeSpan(0, -30, 0));
                            var timeNSSEndOld = timeNSSEnd.Add(new TimeSpan(-1, 0, 0));
                            var timeNSStartOld = timeNSStart.Add(new TimeSpan(-1, 0, 0));
                            var modelGroupError = new ModelGroupError();
                            modelGroupError.LineName = line.TenChuyen;
                            modelGroupError.Id = group.Id;
                            modelGroupError.GroupErrorName = group.Name;
                            foreach (var err in listError)
                            {
                                var listErrorLastHours = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.ErrorId == err.Id && c.IsEndOfLine && c.Time >= timeNSStartOld && c.Time <= timeNSSEndOld && c.Date == now && (c.CommandTypeId == 6 || c.CommandTypeId == 7)).ToList();
                                if (listErrorLastHours.Count > 0)
                                {
                                    err.CountErrorLastHours = listErrorLastHours.Where(c => c.CommandTypeId == 6).Sum(c => c.ThanhPham) - listErrorLastHours.Where(c => c.CommandTypeId == 7).Sum(c => c.ThanhPham);
                                    if (err.CountErrorLastHours < 0)
                                        err.CountErrorLastHours = 0;
                                    modelGroupError.CountErrorLastHours += err.CountErrorLastHours;
                                }

                                var listErrorCurrentHours = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.ErrorId == err.Id && c.IsEndOfLine && c.Time >= timeNSStart && c.Time <= timeNSSEnd && c.Date == now && (c.CommandTypeId == 6 || c.CommandTypeId == 7)).ToList();
                                if (listErrorCurrentHours.Count > 0)
                                {
                                    err.CountErrorCurrentHours = listErrorCurrentHours.Where(c => c.CommandTypeId == 6).Sum(c => c.ThanhPham) - listErrorCurrentHours.Where(c => c.CommandTypeId == 7).Sum(c => c.ThanhPham);
                                    if (err.CountErrorCurrentHours < 0)
                                        err.CountErrorCurrentHours = 0;
                                    modelGroupError.CountErrorCurrentHours += err.CountErrorCurrentHours;
                                }

                                var listErrorOnDay = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.ErrorId == err.Id && c.IsEndOfLine && c.Date == now && (c.CommandTypeId == 6 || c.CommandTypeId == 7)).ToList();
                                if (listErrorOnDay.Count > 0)
                                {
                                    err.TotalError = listErrorOnDay.Where(c => c.CommandTypeId == 6).Sum(c => c.ThanhPham) - listErrorOnDay.Where(c => c.CommandTypeId == 7).Sum(c => c.ThanhPham);
                                    if (err.TotalError < 0)
                                        err.TotalError = 0;
                                    modelGroupError.TotalError += err.TotalError;
                                }
                            }
                            modelGroupError.ListError = listError;
                            listGroupError.Add(modelGroupError);
                        }
                    }
                }
                return listGroupError;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // lấy thong tin loi cua chuyen trong ngay ve chart
        public List<ErrorReportModel> GetErrorsForChart(List<int> lineIds, DateTime date, bool isOneLine)
        {
            try
            {
                var now = date.Day + "/" + date.Month + "/" + date.Year;
                var errorsOflines = new List<ErrorReportModel>();
                if (lineIds != null && lineIds.Count > 0)
                {
                    var linesInfo = db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen));
                    var errorInfo = db.Errors.Where(x => !x.IsDeleted);
                    var theoDoiNgay = db.TheoDoiNgays.Where(x => lineIds.Contains(x.MaChuyen) && x.Date == now && x.IsEndOfLine);

                    // vào tb nang suat lay nhung row co ngay = param(date)
                    var NSTrongNgay = db.NangXuats.Where(x => !x.IsDeleted && x.Ngay == now);
                    var sttChuyen_SP = NSTrongNgay.Select(x => x.STTCHuyen_SanPham);
                    // tu sttChuyen_SP lấy ra thong tin phan cong chuyen va san pham  
                    var listPCC = db.Chuyen_SanPham.Where(c => !c.Chuyen.IsDeleted && !c.IsDelete && !c.SanPham.IsDelete && sttChuyen_SP.Contains(c.STT));
                    // lay thong tin nang suat trong ngay theo stt chuyen san pham
                    var listProductivityOfDay = db.NangXuats.Where(c => sttChuyen_SP.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).ToList();

                    if (linesInfo != null && linesInfo.Count() > 0)
                    {
                        #region Get Productivities of Lines in Date
                        foreach (var line in linesInfo)
                        {
                            var productivities = theoDoiNgay.Where(x => x.MaChuyen == line.MaChuyen);
                            IQueryable<TheoDoiNgay> error_Increases = null, error_Reduced = null;
                            if (productivities != null && productivities.Count() > 0)
                            {
                                error_Increases = productivities.Where(x => x.ErrorId.HasValue && x.ErrorId.Value != 0 && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease);
                                error_Reduced = productivities.Where(x => x.ErrorId.HasValue && x.ErrorId.Value != 0 && x.CommandTypeId == (int)eCommandRecive.ErrorReduce);

                            }
                            var sl = ((error_Increases != null && error_Increases.Count() > 0 ? error_Increases.Sum(x => x.ThanhPham) : 0) - (error_Reduced != null && error_Reduced.Count() > 0 ? error_Reduced.Sum(x => x.ThanhPham) : 0));
                            //
                            var obj = new ErrorReportModel()
                            {
                                LineName = line.TenChuyen,
                                Date = date.Date,
                                TotalErrors = (sl < 0 ? 0 : sl)
                            };

                            //
                            if (isOneLine)
                            {
                                #region  nếu chi lay loi cua chuyen trong ngay theo gio lam viec
                                var errorIds = errorInfo.Select(x => x.Id).ToList();
                                var NSChuyenOfDay = db.TheoDoiNgays.Where(x => x.MaChuyen == line.MaChuyen && errorIds.Contains(x.ErrorId ?? 0) && x.Date == now && x.IsEndOfLine);

                                if (errorInfo != null && errorInfo.Count() > 0)
                                {
                                    if (NSChuyenOfDay != null && NSChuyenOfDay.Count() > 0)
                                    {
                                        foreach (var item in errorInfo)
                                        {
                                            IQueryable<TheoDoiNgay> tang = null, giam = null;
                                            if (NSChuyenOfDay != null && NSChuyenOfDay.Count() > 0)
                                            {
                                                tang = NSChuyenOfDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ErrorIncrease && x.ErrorId == item.Id);
                                                giam = NSChuyenOfDay.Where(x => x.CommandTypeId == (int)eCommandRecive.ErrorReduce && x.ErrorId == item.Id);

                                            }
                                            var total = ((tang != null && tang.Count() > 0 ? tang.Sum(x => x.ThanhPham) : 0) - (giam != null && giam.Count() > 0 ? giam.Sum(x => x.ThanhPham) : 0));
                                            obj.SubErrors.Add(new SubErrorReportModel()
                                            {
                                                ErrorName = item.Name,
                                                Quantites = (total < 0 ? 0 : total)
                                            });
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in errorInfo)
                                        {
                                            obj.SubErrors.Add(new SubErrorReportModel()
                                            {
                                                ErrorName = item.Name,
                                                Quantites = 0
                                            });
                                        }
                                    }
                                }
                                #endregion
                            }

                            errorsOflines.Add(obj);
                        }
                        #endregion
                    }
                }
                return errorsOflines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}