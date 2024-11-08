using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Models;
using QLNSService.Data;
using QLNSService.Enum;

namespace QLNSService.BLL
{
    public class BLLProductivity
    {
        private QLNSEntities db;
        private BLLShift bllShift;
        private BLLLCDConfig bllLCDConfig;
        public BLLProductivity()
        {
            db = new QLNSEntities();
            bllShift = new BLLShift();
            bllLCDConfig = new BLLLCDConfig();
        }

        public ModelProductivity GetProductivityByLineId(int lineId, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            try
            {
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var datetime = DateTime.Now;
                var model = new ModelProductivity();
                var listPCC = db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && c.MaChuyen == lineId).OrderBy(c => c.STTThucHien).ToList();
                if (listPCC.Count > 0)
                {
                    #region get Data
                    var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                    var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        TimePerCommo = x.Chuyen_SanPham.SanPham.ProductionTime,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    var thanhphams = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                    var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();

                    var historyObj = BLLHistoryPressedKeypad.Instance.Get(lineId, now);
                    var pccSX = historyObj == null || historyObj.AssignmentId == null ? listPCC.First() : listPCC.FirstOrDefault(x => x.STT == historyObj.AssignmentId);
                    var productivity = listProductivity.FirstOrDefault(c => c.STTCHuyen_SanPham == pccSX.STT && c.Ngay == now);

                    var dayInfo = listDayInfo.FirstOrDefault(c => c.STTChuyen_SanPham == pccSX.STT);

                    var listBTPOfLine = db.BTPs.Where(c => !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                    var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && listSTTLineProduct.Contains(x.STT_C_SP)).ToList();
                    var nxInMonth = listProductivity.Where(x => x.STTCHuyen_SanPham == pccSX.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                    #endregion

                    if (productivity != null && dayInfo != null)
                    {
                        productivity.TimePerCommo = Math.Round((productivity.TimePerCommo * 100) / dayInfo.HieuSuat);
                        int dinhMucGio = 0;
                        #region Get Content Data
                        model.LineName = productivity.LineName;
                        model.ProductName = productivity.ProductName;

                        model.LK_KCS = (int)pccSX.LuyKeTH;
                        model.LK_TC = pccSX.LuyKeBTPThoatChuyen;
                        // model.laoDong = dayInfo.LaoDongChuyen + "/" + productivity.LaborsBase;
                        model.LDDB = productivity.LaborsBase;
                        model.LDTT = dayInfo.LaoDongChuyen;
                        model.SLCL = (pccSX.SanLuongKeHoach - model.LK_KCS);
                        //   model.ErrorNgay = productivity.SanLuongLoi - productivity.SanLuongLoiGiam;
                        #region
                        var donGia = productivity.ProductPrice != null ? (double)productivity.ProductPrice : 0;
                        var donGiaCM = (double)productivity.ProductPriceCM;
                        double doanhThuTHNgay = 0, doanhThuDMNgay = 0, doanhThuKHThang = 0, doanhThuTHThang = 0, ThuNhapBQThang = 0, DoanhThuBQThang = 0, DoanhThuBQNgay = 0;
                        int dinhMucNgay = 0, finishTH = 0, finishTC = 0, finishError = 0, BTPTrenChuyen = 0, tongTHThang = 0, tongSL_KH = 0;
                        dinhMucNgay = (int)Math.Round(productivity.DinhMucNgay);
                        doanhThuDMNgay = Math.Round((donGia * dinhMucNgay), 2);
                        doanhThuTHNgay = Math.Round((donGia * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)), 2);
                        DoanhThuBQNgay = Math.Round((donGiaCM * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)) / dayInfo.LaoDongChuyen, 2);

                        var currentMonthDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == productivity.STTCHuyen_SanPham);
                        if (currentMonthDetail != null)
                        {
                            doanhThuKHThang = Math.Round((donGia * currentMonthDetail.ProductionPlans), 2);
                            doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TH), 2);
                            tongTHThang = currentMonthDetail.LK_TH;
                            tongSL_KH = currentMonthDetail.ProductionPlans;
                        }


                        if (nxInMonth.Count > 0)
                        {
                            foreach (var item in nxInMonth)
                            {
                                var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                if (day_info != null)
                                {
                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                    ThuNhapBQThang += th > 0 ? ((th * donGia) / day_info.LaoDongChuyen) : 0;

                                    th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                    DoanhThuBQThang += th > 0 ? ((th * donGiaCM) / day_info.LaoDongChuyen) : 0;
                                }
                            }
                            ThuNhapBQThang = (ThuNhapBQThang > 0 ? Math.Round((ThuNhapBQThang / nxInMonth.Count), 2) : 0);
                            DoanhThuBQThang = (DoanhThuBQThang > 0 ? Math.Round((DoanhThuBQThang / nxInMonth.Count), 2) : 0);
                        }

                        int workingTimeOfLine = (int)bllShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;

                        #region
                        if (productivity.IsEndDate)
                        {
                            //ngay cuoi thi dinh muc ngay se bi thieu
                            var pc_next = listPCC.FirstOrDefault(x => x.STTThucHien > pccSX.STTThucHien);
                            if (pc_next != null)
                            {
                                dinhMucNgay = TinhDinhMuc(pccSX, productivity, dayInfo.LaoDongChuyen, workingTimeOfLine, pc_next);
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == pc_next.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    doanhThuDMNgay += Math.Round((pro.DonGia * (dinhMucNgay - productivity.DinhMucNgay)), 2);
                                    var ns_next = listProductivity.FirstOrDefault(x => x.STTCHuyen_SanPham == pc_next.STT);
                                    doanhThuTHNgay += Math.Round((donGia * (ns_next.ThucHienNgay - ns_next.ThucHienNgayGiam)), 2);
                                    var monthD = monthDetails.FirstOrDefault(x => x.STT_C_SP == pc_next.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;

                                        newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;
                                    }

                                    var nxInMonth_next = listProductivity.Where(x => x.STTCHuyen_SanPham == pc_next.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                    if (nxInMonth_next.Count > 0)
                                    {
                                        double TN_BQ = 0, DT_BQ = 0;
                                        foreach (var item in nxInMonth_next)
                                        {
                                            var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                                            if (day_info != null)
                                            {
                                                double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                                TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;

                                                th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                                DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                                            }
                                        }
                                        ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_next.Count), 2) : 0);
                                        DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_next.Count), 2) : 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ///ktra xem co ma hang nao cua chuyen nay ket thuc trong ngay hay ko
                            ///neu co lay dinh muc cua ma hang truoc tinh xem thoi gian san xuat la bao nhieu gio 
                            ///con lai bao nhieu gio de san xuat ma 2 dc bao nhieu hang 2 cai cong lai ra dc dinh muc 
                            ///cua chuyen trong ngay 
                            var objFinishOnDay = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && x.IsFinish && x.FinishedDate.HasValue && x.FinishedDate.Value.Year == datetime.Year && x.FinishedDate.Value.Month == datetime.Month && x.FinishedDate.Value.Day == datetime.Day);
                            if (objFinishOnDay != null)
                            {
                                var nsOfFinishObj = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now);
                                dinhMucNgay = TinhDinhMuc(objFinishOnDay, nsOfFinishObj, dayInfo.LaoDongChuyen, workingTimeOfLine, pccSX);
                                finishTH = nsOfFinishObj.ThucHienNgay - nsOfFinishObj.ThucHienNgayGiam;
                                finishTC = nsOfFinishObj.BTPThoatChuyenNgay - nsOfFinishObj.BTPThoatChuyenNgayGiam;
                                finishError = db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).ToList().Sum(x => x.ThanhPham);
                                finishError -= db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).ToList().Sum(x => x.ThanhPham);
                                BTPTrenChuyen = nsOfFinishObj.BTPTrenChuyen;
                                var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objFinishOnDay.MaSanPham && x.DonGia > 0);
                                if (pro != null)
                                {
                                    doanhThuTHNgay += Math.Round((pro.DonGia * finishTH), 2);
                                    DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTH) / dayInfo.LaoDongChuyen, 2);

                                    var monthD = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == objFinishOnDay.STT);
                                    if (monthD != null)
                                    {
                                        var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                                        doanhThuKHThang += newDT;

                                        newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                                        doanhThuTHThang += newDT;

                                        tongTHThang += monthD.LK_TH;
                                        tongSL_KH += monthD.ProductionPlans;

                                        var nxInMonth_finish = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == objFinishOnDay.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                                        var thanhpham_finish = db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == objFinishOnDay.STT).ToList();
                                        if (nxInMonth_finish.Count > 0)
                                        {
                                            double TN_BQ = 0, DT_BQ = 0;
                                            foreach (var item in nxInMonth_finish)
                                            {
                                                var day_info = thanhpham_finish.FirstOrDefault(x => x.Ngay == item.Ngay);
                                                if (day_info != null)
                                                {
                                                    double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                                    TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;

                                                    th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                                                    DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                                                }
                                            }
                                            ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_finish.Count), 2) : 0);
                                            DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_finish.Count), 2) : 0);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #endregion
                        //model.DinhMucNgay = dinhMucNgay;
                        model.DMN = (int)productivity.DinhMucNgay;
                        model.DoanhThuKH_T = doanhThuKHThang;
                        model.DoanhThu_T = doanhThuTHThang;
                        model.DoanhThu = doanhThuTHNgay;
                        model.DoanhThu_T = doanhThuTHThang;
                        model.DoanhThuKH_T = doanhThuKHThang;

                        #region
                        int thucHienNgay = 0, TCNgay = 0, ErrorDay = 0;
                        var listProductivityOnDay = listProductivity.Where(c => c.Ngay == now).ToList();
                        if (listProductivityOnDay.Count > 0)
                        {
                            foreach (var p in listProductivityOnDay)
                            {
                                var THNgay = p.ThucHienNgay - p.ThucHienNgayGiam;
                                if (THNgay > 0)
                                {
                                    thucHienNgay += THNgay;
                                    TCNgay += (p.BTPThoatChuyenNgay - p.BTPThoatChuyenNgayGiam);
                                    if (p.ProductPrice > 0)
                                        doanhThuTHNgay += Math.Round((p.ProductPrice * THNgay), 2);
                                }
                                BTPTrenChuyen += p.BTPTrenChuyen;
                            }
                        }
                        thucHienNgay += finishTH > 0 ? finishTH : 0;
                        TCNgay += finishTC > 0 ? finishTC : 0;

                        model.KCS = thucHienNgay;
                        model.TC = TCNgay;

                        // model.thucHienVaDinhMuc = thucHienNgay + " / " + dinhMucNgay;
                        model.tiLeThucHien = (dinhMucNgay > 0 && thucHienNgay > 0) ? ((int)((thucHienNgay * 100) / dinhMucNgay)) + "" : "0";

                        //   model.doanhThuNgayTrenDinhMuc = doanhThuTHNgay + " / " + doanhThuDMNgay;
                        model.DoanhThu = doanhThuTHNgay;
                        model.DoanhThuDM = doanhThuDMNgay;


                        model.NhipSX = Math.Round((double)productivity.NhipDoSanXuat, 1);
                        model.NhipTT = Math.Round((double)productivity.NhipDoThucTe, 1);

                        int btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 ? 0 : (int)Math.Ceiling((double)BTPTrenChuyen / dayInfo.LaoDongChuyen);
                        model.BTPInLine = BTPTrenChuyen;
                        model.BTPInLine_BQ = btpTrenChuyenBinhQuan;

                        int lightId = productivity.IdDenNangSuat ?? 0;
                        double tyLeDen = 0;
                        if (productivity.NhipDoThucTe > 0)
                            tyLeDen = (model.NhipSX * 100) / productivity.NhipDoThucTe;

                        var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                        model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";

                        // model.LuyKeKCS = pccSX.LuyKeBTPThoatChuyen + " / " + pccSX.SanLuongKeHoach;
                        //  model.LKTH_SLKH = luyKeThucHien + " / " + pccSX.SanLuongKeHoach;

                        model.LK_TC = pccSX.LuyKeBTPThoatChuyen;

                        #endregion

                        model.ThuNhapBQ = Math.Round((doanhThuTHNgay / dayInfo.LaoDongChuyen));
                        model.ThuNhapBQ_T = Math.Round((ThuNhapBQThang));


                        model.DoanhThuBQ = Math.Round(DoanhThuBQNgay);
                        model.DoanhThuBQ_T = Math.Round(DoanhThuBQThang);


                        #endregion

                        #region Get Hours Productivity
                        var totalWorkingTimeInDay = bllShift.GetTotalWorkingHourOfLine(lineId);
                        int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                        int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                        double NangSuatPhutKH = 0;
                        int NangSuatGioKH = 0;
                        var dateNow = DateTime.Now.Date;
                        int tongTCNgay = 0, tongKCSNgay = 0;
                        if (intWorkTime > 0)
                        {
                            NangSuatPhutKH = (double)dinhMucNgay / intWorkMinuter;
                            NangSuatGioKH = (int)(dinhMucNgay / intWorkTime);
                            if (dinhMucNgay % intWorkTime != 0)
                                NangSuatGioKH++;

                            if (hienThiNSGio == (int)eShowNSType.PercentTH_OnDay)
                            {
                                #region  hiển thị một ô năng suất hiện tại duy nhất
                                double phutToNow = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, bllShift.GetShiftsOfLine(lineId));
                                double nsKHToNow = (phutToNow / intWorkMinuter) * model.DMN;
                                double tiLePhanTram = 0;
                                tiLePhanTram = (model.KCS > 0 && nsKHToNow > 0) ? (Math.Round((double)((model.KCS * 100) / nsKHToNow), 2)) : 0;
                                //  model.CurrentNS = model.KCS + "/" + (int)nsKHToNow + "  (" + tiLePhanTram + "%)";
                                model.SLKHToNow = (int)nsKHToNow;
                                #endregion
                            }

                            #region
                            List<WorkingTimeModel> listWorkHoursOfLine = new List<WorkingTimeModel>();
                            switch (hienThiNSGio)
                            {
                                case (int)eShowNSType.PercentTH_FollowHour:
                                case (int)eShowNSType.TH_Err_FollowHour:
                                case (int)eShowNSType.TH_DM_FollowHour:
                                case (int)eShowNSType.TH_TC_FollowHour:
                                    listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId);
                                    break;
                                case (int)eShowNSType.PercentTH_FollowConfig:
                                case (int)eShowNSType.TH_Err_FollowConfig:
                                case (int)eShowNSType.TH_DM_FollowConfig:
                                case (int)eShowNSType.TH_TC_FollowConfig:
                                    listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS);
                                    break;
                                case (int)eShowNSType.TH_DM_OnDay:
                                case (int)eShowNSType.TH_TC_OnDay:
                                case (int)eShowNSType.TH_Error_OnDay:
                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        TimeEnd = DateTime.Now.TimeOfDay,
                                        IntHours = 1,
                                    });

                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                                        TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        IntHours = 2,
                                    });
                                    break;
                            }

                            if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                            {
                                var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
                                {
                                    CommandTypeId = x.CommandTypeId,
                                    CumId = x.CumId,
                                    MaChuyen = x.MaChuyen,
                                    MaSanPham = x.MaSanPham,
                                    Time = x.Time,
                                    Date = x.Date,
                                    ErrorId = x.ErrorId,
                                    IsEndOfLine = x.IsEndOfLine,
                                    IsEnterByKeypad = x.IsEnterByKeypad,
                                    STT = x.STT,
                                    STTChuyenSanPham = x.STTChuyenSanPham,
                                    ThanhPham = x.ThanhPham,
                                    ProductOutputTypeId = x.ProductOutputTypeId
                                }).ToList();

                                var t = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                var g = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                tongTCNgay += (t - g);

                                for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                                {
                                    //DM Gio
                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                    if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                        listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                    #region
                                    int Tang = 0, Giam = 0;
                                    var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                    if (theoDoiNgays.Count > 0)
                                    {
                                        //Kcs
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Tang -= Giam;

                                        listWorkHoursOfLine[i].KCS = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                        tongKCSNgay += Tang;

                                        // TC
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Tang -= Giam;

                                        listWorkHoursOfLine[i].TC = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                        //   tongTCNgay += Tang;

                                        // lỗi
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].Error = Tang;
                                        model.Error += Tang;

                                    }
                                    else
                                    {
                                        listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            model.Error += finishError;
                            model.listWorkHours = listWorkHoursOfLine;
                            model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                            // tong thuc hien cua chuyen trong ngay cua cac ma hang
                            // model.tongThucHienNgay = tongKCSNgay + "/" + tongDinhMucNgay;
                            //  model.tongThucHienNgay = tongKCSNgay + "/" + dinhMucNgay;
                            // tong kcs cua chuyen cho cac ma hang trong ngay
                            //   model.ThoatChuyen = tongTCNgay + " / " + pccSX.LuyKeBTPThoatChuyen;

                            double minutes = 0, pro = 0, pro_lech = 0, time_lech = 0, LK_err = 0;
                            minutes = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, bllShift.GetShiftsOfLine(lineId));
                            pro = (minutes / intWorkMinuter) * model.DMN;
                            pro_lech = pro - productivity.BTPThoatChuyenNgay;
                            if (pro_lech > 0)
                                time_lech = Math.Round(((pro_lech / model.LDTT) * productivity.TimePerCommo) / 3600);
                            model.Hour_ChenhLech_Day = time_lech > 0 ? (int)time_lech : 0;

                            var proOfCommo = listProductivity.Where(x => x.STTCHuyen_SanPham == productivity.STTCHuyen_SanPham && x.Ngay != now);
                            foreach (var item in proOfCommo)
                            {
                                pro_lech = item.DinhMucNgay - (item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam);
                                if (pro_lech > 0)
                                    time_lech += ((pro_lech / item.LaborsBase) * item.TimePerCommo) / 3600;
                                LK_err += item.SanLuongLoi - item.SanLuongLoiGiam;
                            }

                            model.Hour_ChenhLech = time_lech > 0 ? (int)time_lech : 0;

                            model.KCS_QuaTay = model.KCS + model.Error;
                            model.LK_KCS_QuaTay = model.LK_KCS + (int)LK_err;
                            model.Lean = Math.Ceiling((double)(model.BTPInLine > 0 ? (model.BTPInLine / model.LDTT) : 0));
                        }
                        #endregion


                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ModelProductivity GetProductivityByLineId_KTC(int lineId, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            try
            {
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var datetime = DateTime.Now;
                var model = new ModelProductivity();

                var listPCC = db.Chuyen_SanPham.Where(c => !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted && c.MaChuyen == lineId).OrderBy(c => c.STTThucHien).ToList();
                if (listPCC.Count > 0)
                {
                    #region get Data
                    var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                    var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        TimePerCommo = x.Chuyen_SanPham.SanPham.ProductionTime,
                        CreatedDate = x.CreatedDate
                    }).ToList();

                    var thanhphams = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                    var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();

                    var hienthimanay = listDayInfo.FirstOrDefault(x => x.ShowLCD);
                    Chuyen_SanPham pccSX = null;
                    if (hienthimanay == null)
                    {
                        var historyObj = BLLHistoryPressedKeypad.Instance.Get(lineId, now);
                        pccSX = ((historyObj == null || historyObj.AssignmentId == null) ? listPCC.First() : listPCC.FirstOrDefault(x => x.STT == historyObj.AssignmentId));
                        if (pccSX == null && listPCC.Count > 0)
                            pccSX = listPCC.First();
                    }
                    else
                        pccSX = listPCC.FirstOrDefault(x => x.STT == hienthimanay.STTChuyen_SanPham);

                    var productivity = listProductivity.FirstOrDefault(c => c.STTCHuyen_SanPham == pccSX.STT && c.Ngay == now);
                    var dayInfo = listDayInfo.FirstOrDefault(c => c.STTChuyen_SanPham == pccSX.STT);

                    var listBTPOfLine = db.BTPs.Where(c => !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                    var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && listSTTLineProduct.Contains(x.STT_C_SP)).ToList();
                    var nxInMonth = listProductivity.Where(x => x.STTCHuyen_SanPham == pccSX.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                    #endregion

                    if (productivity != null && dayInfo != null)
                    {
                        productivity.TimePerCommo = Math.Round((productivity.TimePerCommo * 100) / dayInfo.HieuSuat);
                        #region Get Content Data
                        model.LineName = productivity.LineName;
                        model.ProductName = productivity.ProductName;
                        model.LK_KCS = (int)pccSX.LuyKeTH;
                        model.LK_TC = (int)pccSX.LuyKeBTPThoatChuyen;
                        model.LK_BTP = pccSX.LK_BTP;
                        model.LDDB = productivity.LaborsBase;
                        model.LDTT = dayInfo.LaoDongChuyen;
                        model.SLKH = pccSX.SanLuongKeHoach;
                        model.SLCL = (pccSX.SanLuongKeHoach - model.LK_KCS);
                        model.DMN = (int)Math.Round(productivity.DinhMucNgay);
                        model.KCS = productivity.ThucHienNgay - productivity.ThucHienNgayGiam;
                        model.TC = productivity.BTPThoatChuyenNgay - productivity.BTPThoatChuyenNgayGiam;
                        model.tiLeThucHien = (model.DMN > 0 && model.KCS > 0) ? ((int)((model.KCS * 100) / model.DMN)) + "" : "0";
                        model.NhipSX = Math.Round((double)productivity.NhipDoSanXuat, 1);
                        model.NhipTT = Math.Round((double)productivity.NhipDoThucTe, 1);
                        model.NhipTC = Math.Round((double)productivity.NhipDoThucTeBTPThoatChuyen, 1);
                        model.Error = productivity.SanLuongLoi - productivity.SanLuongLoiGiam;
                        model.KCSHour = 0;
                        model.DMHour = 0;
                        model.ErrHour = 0;
                        model.TCHour = 0;
                        model.BTPHour = 0;
                        model.TiLeLoi_H = 0;
                        model.TiLeLoi_D = 0;
                        #region
                        //var donGia = productivity.ProductPrice != null ? (double)productivity.ProductPrice : 0;
                        //var donGiaCM = (double)productivity.ProductPriceCM;
                        //double doanhThuTHNgay = 0, doanhThuDMNgay = 0, doanhThuKHThang = 0, doanhThuTHThang = 0, ThuNhapBQThang = 0, DoanhThuBQThang = 0, DoanhThuBQNgay = 0;
                        //int dinhMucNgay = 0, finishTH = 0, finishTC = 0, finishError = 0, BTPTrenChuyen = 0, tongTHThang = 0, tongSL_KH = 0;
                        //dinhMucNgay = (int)Math.Round(productivity.DinhMucNgay);
                        //doanhThuDMNgay = Math.Round((donGia * dinhMucNgay), 2);
                        //doanhThuTHNgay = Math.Round((donGia * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)), 2);
                        //DoanhThuBQNgay = Math.Round((donGiaCM * (productivity.ThucHienNgay - productivity.ThucHienNgayGiam)) / dayInfo.LaoDongChuyen, 2);

                        //var currentMonthDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == productivity.STTCHuyen_SanPham);
                        //if (currentMonthDetail != null)
                        //{
                        //    doanhThuKHThang = Math.Round((donGia * currentMonthDetail.ProductionPlans), 2);
                        //    doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TH), 2);
                        //    tongTHThang = currentMonthDetail.LK_TH;
                        //    tongSL_KH = currentMonthDetail.ProductionPlans;
                        //}


                        //if (nxInMonth.Count > 0)
                        //{
                        //    foreach (var item in nxInMonth)
                        //    {
                        //        var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                        //        if (day_info != null)
                        //        {
                        //            double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //            ThuNhapBQThang += th > 0 ? ((th * donGia) / day_info.LaoDongChuyen) : 0;

                        //            th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //            DoanhThuBQThang += th > 0 ? ((th * donGiaCM) / day_info.LaoDongChuyen) : 0;
                        //        }
                        //    }
                        //    ThuNhapBQThang = (ThuNhapBQThang > 0 ? Math.Round((ThuNhapBQThang / nxInMonth.Count), 2) : 0);
                        //    DoanhThuBQThang = (DoanhThuBQThang > 0 ? Math.Round((DoanhThuBQThang / nxInMonth.Count), 2) : 0);
                        //}

                        //int workingTimeOfLine = (int)bllShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;

                        #region
                        //if (productivity.IsEndDate)
                        //{
                        //    //ngay cuoi thi dinh muc ngay se bi thieu
                        //    var pc_next = listPCC.FirstOrDefault(x => x.STTThucHien > pccSX.STTThucHien);
                        //    if (pc_next != null)
                        //    {
                        //        dinhMucNgay = TinhDinhMuc(pccSX, productivity, dayInfo.LaoDongChuyen, workingTimeOfLine, pc_next);
                        //        var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == pc_next.MaSanPham && x.DonGia > 0);
                        //        if (pro != null)
                        //        {
                        //            doanhThuDMNgay += Math.Round((pro.DonGia * (dinhMucNgay - productivity.DinhMucNgay)), 2);
                        //            var ns_next = listProductivity.FirstOrDefault(x => x.STTCHuyen_SanPham == pc_next.STT);
                        //            doanhThuTHNgay += Math.Round((donGia * (ns_next.ThucHienNgay - ns_next.ThucHienNgayGiam)), 2);
                        //            var monthD = monthDetails.FirstOrDefault(x => x.STT_C_SP == pc_next.STT);
                        //            if (monthD != null)
                        //            {
                        //                var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                        //                doanhThuKHThang += newDT;

                        //                newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                        //                doanhThuTHThang += newDT;

                        //                tongTHThang += monthD.LK_TH;
                        //                tongSL_KH += monthD.ProductionPlans;
                        //            }

                        //            var nxInMonth_next = listProductivity.Where(x => x.STTCHuyen_SanPham == pc_next.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                        //            if (nxInMonth_next.Count > 0)
                        //            {
                        //                double TN_BQ = 0, DT_BQ = 0;
                        //                foreach (var item in nxInMonth_next)
                        //                {
                        //                    var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                        //                    if (day_info != null)
                        //                    {
                        //                        double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //                        TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;

                        //                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //                        DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                        //                    }
                        //                }
                        //                ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_next.Count), 2) : 0);
                        //                DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_next.Count), 2) : 0);
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    ///ktra xem co ma hang nao cua chuyen nay ket thuc trong ngay hay ko
                        //    ///neu co lay dinh muc cua ma hang truoc tinh xem thoi gian san xuat la bao nhieu gio 
                        //    ///con lai bao nhieu gio de san xuat ma 2 dc bao nhieu hang 2 cai cong lai ra dc dinh muc 
                        //    ///cua chuyen trong ngay 
                        //    var objFinishOnDay = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && x.IsFinish && x.FinishedDate.HasValue && x.FinishedDate.Value.Year == datetime.Year && x.FinishedDate.Value.Month == datetime.Month && x.FinishedDate.Value.Day == datetime.Day);
                        //    if (objFinishOnDay != null)
                        //    {
                        //        var nsOfFinishObj = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now);
                        //        dinhMucNgay = TinhDinhMuc(objFinishOnDay, nsOfFinishObj, dayInfo.LaoDongChuyen, workingTimeOfLine, pccSX);
                        //        finishTH = nsOfFinishObj.ThucHienNgay - nsOfFinishObj.ThucHienNgayGiam;
                        //        finishTC = nsOfFinishObj.BTPThoatChuyenNgay - nsOfFinishObj.BTPThoatChuyenNgayGiam;
                        //        finishError = db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).ToList().Sum(x => x.ThanhPham);
                        //        finishError -= db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).ToList().Sum(x => x.ThanhPham);
                        //        BTPTrenChuyen = nsOfFinishObj.BTPTrenChuyen;
                        //        var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objFinishOnDay.MaSanPham && x.DonGia > 0);
                        //        if (pro != null)
                        //        {
                        //            doanhThuTHNgay += Math.Round((pro.DonGia * finishTH), 2);
                        //            DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTH) / dayInfo.LaoDongChuyen, 2);

                        //            var monthD = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == objFinishOnDay.STT);
                        //            if (monthD != null)
                        //            {
                        //                var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                        //                doanhThuKHThang += newDT;

                        //                newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                        //                doanhThuTHThang += newDT;

                        //                tongTHThang += monthD.LK_TH;
                        //                tongSL_KH += monthD.ProductionPlans;

                        //                var nxInMonth_finish = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == objFinishOnDay.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                        //                var thanhpham_finish = db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == objFinishOnDay.STT).ToList();
                        //                if (nxInMonth_finish.Count > 0)
                        //                {
                        //                    double TN_BQ = 0, DT_BQ = 0;
                        //                    foreach (var item in nxInMonth_finish)
                        //                    {
                        //                        var day_info = thanhpham_finish.FirstOrDefault(x => x.Ngay == item.Ngay);
                        //                        if (day_info != null)
                        //                        {
                        //                            double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //                            TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;

                        //                            th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                        //                            DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                        //                        }
                        //                    }
                        //                    ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_finish.Count), 2) : 0);
                        //                    DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_finish.Count), 2) : 0);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        #endregion
                        //model.DinhMucNgay = dinhMucNgay;



                        //  model.doanhThuKHThang = doanhThuTHThang + " / " + doanhThuKHThang;

                        // model.thucHienKHThang = tongTHThang + " / " + tongSL_KH;

                        //  model.doanhThuThang = doanhThuTHNgay + " / " + doanhThuTHThang;
                        //  model.DoanhThu_Thang = doanhThuTHThang;
                        //   model.DoanhThuDMThang = doanhThuKHThang;

                        #region
                        int thucHienNgay = 0, TCNgay = 0, ErrorDay = 0;
                        //var listProductivityOnDay = listProductivity.Where(c => c.Ngay == now).ToList();
                        //if (listProductivityOnDay.Count > 0)
                        //{
                        //    foreach (var p in listProductivityOnDay)
                        //    {
                        //        var THNgay = p.ThucHienNgay - p.ThucHienNgayGiam;
                        //        if (THNgay > 0)
                        //        {
                        //            thucHienNgay += THNgay;
                        //            TCNgay += (p.BTPThoatChuyenNgay - p.BTPThoatChuyenNgayGiam);
                        //            if (p.ProductPrice > 0)
                        //                doanhThuTHNgay += Math.Round((p.ProductPrice * THNgay), 2);
                        //        }
                        //        BTPTrenChuyen += p.BTPTrenChuyen;
                        //    }
                        //}
                        //thucHienNgay += finishTH > 0 ? finishTH : 0;
                        //TCNgay += finishTC > 0 ? finishTC : 0;



                        //  model.doanhThuNgayTrenDinhMuc = doanhThuTHNgay + " / " + doanhThuDMNgay;
                        //  model.DoanhThuNgay = doanhThuTHNgay;
                        //  model.DoanhThuDMNgay = doanhThuDMNgay;



                        int btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 || productivity.BTPTrenChuyen == 0 ? 0 : (int)Math.Ceiling((double)productivity.BTPTrenChuyen / dayInfo.LaoDongChuyen);
                        model.BTPInLine = productivity.BTPTrenChuyen;
                        model.BTPInLine_BQ = btpTrenChuyenBinhQuan;

                        int lightId = productivity.IdDenNangSuat ?? 0;
                        model.SLKH = pccSX.SanLuongKeHoach;
                        model.LK_TC = pccSX.LuyKeBTPThoatChuyen;

                        #endregion

                        // model.ThuNhapBQNgay = Math.Round((doanhThuTHNgay / dayInfo.LaoDongChuyen));
                        //  model.ThuNhap_BQThang = Math.Round((ThuNhapBQThang));
                        //  model.thuNhapBQThang = model.ThuNhapBQNgay + " / " + model.ThuNhap_BQThang;

                        //  model.DoanhThuBQNgay = Math.Round(DoanhThuBQNgay);
                        //  model.DoanhThuBQThang = Math.Round(DoanhThuBQThang);
                        //  model.DoanhThuBQ = model.DoanhThuBQNgay + " / " + model.DoanhThuBQThang;
                        model.BTP = 0;
                        model.LK_BTP = 0;
                        if (listBTPOfLine != null && listBTPOfLine.Count > 0)
                        {
                            int btpGiaoChuyenNgayTang = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                            int btpGiaoChuyenNgayGiam = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                            model.BTP = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;

                            btpGiaoChuyenNgayTang = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                            btpGiaoChuyenNgayGiam = listBTPOfLine.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                            model.LK_BTP = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;
                        }

                        #endregion

                        #region Get Hours Productivity
                        var totalWorkingTimeInDay = bllShift.GetTotalWorkingHourOfLine(lineId);
                        int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                        int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                        double NangSuatPhutKH = 0;
                        int NangSuatGioKH = 0;
                        var dateNow = DateTime.Now.Date;
                        int tongTCNgay = 0, tongKCSNgay = 0;
                        model.DMHour = (int)Math.Ceiling((double)model.DMN / intWorkTime);
                        if (intWorkTime > 0)
                        {
                            NangSuatPhutKH = (double)model.DMN / intWorkMinuter;
                            NangSuatGioKH = (int)(model.DMN / intWorkTime);
                            if (model.DMN % intWorkTime != 0)
                                NangSuatGioKH++;

                            #region  hiển thị một ô năng suất hiện tại duy nhất
                            double phutToNow = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, bllShift.GetShiftsOfLine(lineId));
                            double nsKHToNow = (phutToNow / intWorkMinuter) * model.DMN;
                            double tiLePhanTram = 0;
                            tiLePhanTram = (model.KCS > 0 && nsKHToNow > 0) ? (Math.Round((double)((model.KCS * 100) / nsKHToNow), 2)) : 0;
                            // model.CurrentNS = model.KCS + "/" + (int)nsKHToNow + "  (" + tiLePhanTram + "%)";
                            model.SLKHToNow = (int)nsKHToNow;
                            #endregion


                            #region
                            List<WorkingTimeModel> listWorkHoursOfLine = new List<WorkingTimeModel>();
                            switch (hienThiNSGio)
                            {
                                case (int)eShowNSType.PercentTH_FollowHour:
                                case (int)eShowNSType.TH_Err_FollowHour:
                                case (int)eShowNSType.TH_DM_FollowHour:
                                case (int)eShowNSType.TH_TC_FollowHour:
                                    listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId);
                                    break;
                                case (int)eShowNSType.PercentTH_FollowConfig:
                                case (int)eShowNSType.TH_Err_FollowConfig:
                                case (int)eShowNSType.TH_DM_FollowConfig:
                                case (int)eShowNSType.TH_TC_FollowConfig:
                                    listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS);
                                    break;
                                case (int)eShowNSType.TH_DM_OnDay:
                                case (int)eShowNSType.TH_TC_OnDay:
                                case (int)eShowNSType.TH_Error_OnDay:
                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        TimeEnd = DateTime.Now.TimeOfDay,
                                        IntHours = 1,
                                    });

                                    listWorkHoursOfLine.Add(new WorkingTimeModel()
                                    {
                                        TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                                        TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                        IntHours = 2,
                                    });
                                    break;
                            }


                            if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                            {

                                var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.STTChuyenSanPham == pccSX.STT && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
                                {
                                    CommandTypeId = x.CommandTypeId,
                                    CumId = x.CumId,
                                    MaChuyen = x.MaChuyen,
                                    MaSanPham = x.MaSanPham,
                                    Time = x.Time,
                                    Date = x.Date,
                                    ErrorId = x.ErrorId,
                                    IsEndOfLine = x.IsEndOfLine,
                                    IsEnterByKeypad = x.IsEnterByKeypad,
                                    STT = x.STT,
                                    STTChuyenSanPham = x.STTChuyenSanPham,
                                    ThanhPham = x.ThanhPham,
                                    ProductOutputTypeId = x.ProductOutputTypeId
                                }).ToList();

                                var t = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                var g = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                tongTCNgay += (t - g);
                                bool isValid = false;

                                for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                                {
                                    if (DateTime.Now.TimeOfDay > listWorkHoursOfLine[i].TimeStart && DateTime.Now.TimeOfDay <= listWorkHoursOfLine[i].TimeEnd)
                                        isValid = true;

                                    //DM Gio
                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                    if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                        listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                    #region
                                    int Tang = 0, Giam = 0;
                                    var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                    if (theoDoiNgays.Count > 0)
                                    {
                                        //Kcs
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                        Tang -= Giam;

                                        listWorkHoursOfLine[i].KCS = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                        tongKCSNgay += Tang;
                                        if (isValid)
                                            model.KCSHour = Tang;

                                        // TC
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                        Tang -= Giam;

                                        listWorkHoursOfLine[i].TC = Tang;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                        if (isValid)
                                            model.TCHour = Tang;
                                        //   tongTCNgay += Tang;

                                        // lỗi
                                        Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                        Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                        Tang -= Giam;
                                        listWorkHoursOfLine[i].Error = Tang;
                                        if (isValid)
                                        {
                                            model.ErrHour = Tang;
                                            model.TiLeLoi_H = (model.ErrHour != 0 ? (int)Math.Ceiling((model.ErrHour / (double)(model.KCSHour + model.ErrHour)) * 100) : 0);
                                        }
                                    }
                                    else
                                    {
                                        listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                        listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                    }
                                    #endregion
                                    isValid = false;
                                }
                            }
                            #endregion

                            //  model.ErrorNgay += finishError;
                            model.listWorkHours = listWorkHoursOfLine;
                            model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                            //  model.KCS = tongKCSNgay;
                            model.DMN = (int)productivity.DinhMucNgay;
                            //  model.TC = tongTCNgay;

                            double minutes = 0, pro = 0, pro_lech = 0, time_lech = 0, LK_err = 0;
                            minutes = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, bllShift.GetShiftsOfLine(lineId));
                            pro = (minutes / intWorkMinuter) * model.DMN;
                            pro_lech = pro - productivity.BTPThoatChuyenNgay;
                            if (pro_lech > 0)
                                time_lech = Math.Round(((pro_lech / model.LDTT) * productivity.TimePerCommo) / 3600);
                            model.Hour_ChenhLech_Day = time_lech > 0 ? (int)time_lech : 0;

                            var proOfCommo = listProductivity.Where(x => x.STTCHuyen_SanPham == productivity.STTCHuyen_SanPham && x.Ngay != now);
                            foreach (var item in proOfCommo)
                            {
                                pro_lech = item.DinhMucNgay - (item.BTPThoatChuyenNgay - item.BTPThoatChuyenNgayGiam);
                                if (pro_lech > 0)
                                    time_lech += ((pro_lech / item.LaborsBase) * item.TimePerCommo) / 3600;
                                LK_err += item.SanLuongLoi - item.SanLuongLoiGiam;
                            }

                            model.Hour_ChenhLech = time_lech > 0 ? (int)time_lech : 0;

                            model.KCS_QuaTay = model.KCS + model.Error;
                            model.LK_KCS_QuaTay = model.LK_KCS + (int)LK_err;

                            model.TiLeLoi_D = (model.Error != 0 ? (int)Math.Ceiling((model.Error / (double)(model.Error + model.KCS)) * 100) : 0);

                        }
                        #endregion

                        double tyLeDen = 0;
                        //if (productivity.NhipDoThucTe > 0)
                        //    tyLeDen = (model.NhipSX * 100) / productivity.NhipDoThucTe;

                        // sua lai kcs / dmtoNow * 100
                        if (model.SLKHToNow > 0)
                            tyLeDen = Math.Round((model.KCS / model.SLKHToNow) * 100, 1);
                        var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                        model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";
                    }
                }
                if (model.BTPInLine < 0)
                    model.BTPInLine = 0;
                model.Lean = Math.Ceiling((double)(model.BTPInLine > 0 ? (model.BTPInLine / model.LDTT) : 0));
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// cho form nang suat moi
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="tableTypeId"></param>
        /// <param name="hienThiNSGio"></param>
        /// <param name="TimesGetNS"></param>
        /// <param name="KhoangCachGetNSOnDay"></param>
        /// <returns></returns>
        public ModelProductivity GetProductivityByLineId_new(int lineId, int tableTypeId, int hienThiNSGio, int TimesGetNS, int KhoangCachGetNSOnDay)
        {
            try
            {
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                var datetime = DateTime.Now;
                var model = new ModelProductivity();
                var historyObj = BLLHistoryPressedKeypad.Instance.Get(lineId, now);
                ProductivitiesModel nsObj;
                #region
                if (historyObj.AssignmentId == null)
                {
                    nsObj = db.NangXuats.Where(c => c.Chuyen_SanPham.MaChuyen == lineId && !c.Chuyen_SanPham.Chuyen.IsDeleted && !c.Chuyen_SanPham.SanPham.IsDelete && !c.IsDeleted && c.Ngay == now).Select(x => new ProductivitiesModel()
                     {
                         Id = x.Id,
                         Ngay = x.Ngay,
                         STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                         OrderIndex = x.Chuyen_SanPham.STTThucHien,
                         BTPGiam = x.BTPGiam,
                         BTPLoi = x.BTPLoi,
                         BTPTang = x.BTPTang,
                         BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                         BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                         BTPTrenChuyen = x.BTPTrenChuyen,
                         DinhMucNgay = x.DinhMucNgay,
                         IsBTP = x.IsBTP,
                         IsChange = x.IsChange,
                         IsChangeBTP = x.IsChangeBTP,
                         IsEndDate = x.IsEndDate,
                         IsStopOnDay = x.IsStopOnDay,
                         NhipDoSanXuat = x.NhipDoSanXuat,
                         NhipDoThucTe = x.NhipDoThucTe,
                         NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                         SanLuongLoi = x.SanLuongLoi,
                         SanLuongLoiGiam = x.SanLuongLoiGiam,
                         ThucHienNgay = x.ThucHienNgay,
                         ThucHienNgayGiam = x.ThucHienNgayGiam,
                         TimeLastChange = x.TimeLastChange,
                         TimeStopOnDay = x.TimeStopOnDay,
                         productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                         ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                         ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                         ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                         LineId = x.Chuyen_SanPham.MaChuyen,
                         LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                         IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                         LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                         CreatedDate = x.CreatedDate
                     }).ToList().OrderBy(x => x.OrderIndex).FirstOrDefault();
                }
                else
                {
                    nsObj = db.NangXuats.Where(c => c.Chuyen_SanPham.MaChuyen == lineId && !c.Chuyen_SanPham.Chuyen.IsDeleted && !c.Chuyen_SanPham.SanPham.IsDelete && !c.IsDeleted && c.STTCHuyen_SanPham == historyObj.AssignmentId && c.Ngay == now).Select(x => new ProductivitiesModel()
                    {
                        Id = x.Id,
                        Ngay = x.Ngay,
                        STTCHuyen_SanPham = x.STTCHuyen_SanPham,
                        OrderIndex = x.Chuyen_SanPham.STTThucHien,
                        BTPGiam = x.BTPGiam,
                        BTPLoi = x.BTPLoi,
                        BTPTang = x.BTPTang,
                        BTPThoatChuyenNgay = x.BTPThoatChuyenNgay,
                        BTPThoatChuyenNgayGiam = x.BTPThoatChuyenNgayGiam,
                        BTPTrenChuyen = x.BTPTrenChuyen,
                        DinhMucNgay = x.DinhMucNgay,
                        IsBTP = x.IsBTP,
                        IsChange = x.IsChange,
                        IsChangeBTP = x.IsChangeBTP,
                        IsEndDate = x.IsEndDate,
                        IsStopOnDay = x.IsStopOnDay,
                        NhipDoSanXuat = x.NhipDoSanXuat,
                        NhipDoThucTe = x.NhipDoThucTe,
                        NhipDoThucTeBTPThoatChuyen = x.NhipDoThucTeBTPThoatChuyen,
                        SanLuongLoi = x.SanLuongLoi,
                        SanLuongLoiGiam = x.SanLuongLoiGiam,
                        ThucHienNgay = x.ThucHienNgay,
                        ThucHienNgayGiam = x.ThucHienNgayGiam,
                        TimeLastChange = x.TimeLastChange,
                        TimeStopOnDay = x.TimeStopOnDay,
                        productId = x.Chuyen_SanPham.SanPham.MaSanPham,
                        ProductName = x.Chuyen_SanPham.SanPham.TenSanPham,
                        ProductPrice = x.Chuyen_SanPham.SanPham.DonGia,
                        ProductPriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                        LineId = x.Chuyen_SanPham.MaChuyen,
                        LineName = x.Chuyen_SanPham.Chuyen.TenChuyen,
                        IdDenNangSuat = x.Chuyen_SanPham.Chuyen.IdDenNangSuat,
                        LaborsBase = x.Chuyen_SanPham.Chuyen.LaoDongDinhBien,
                        CreatedDate = x.CreatedDate
                    }).ToList().FirstOrDefault();
                }
                #endregion


                #region get Data
                var pccSX = db.Chuyen_SanPham.FirstOrDefault(x => x.STT == nsObj.STTCHuyen_SanPham);
                var dayInfo = db.ThanhPhams.FirstOrDefault(c => c.STTChuyen_SanPham == nsObj.STTCHuyen_SanPham && !c.IsDeleted && c.Ngay == now);

                var listBTPOfLine = db.BTPs.Where(c => !c.IsBTP_PB_HC && c.STTChuyen_SanPham == nsObj.STTCHuyen_SanPham && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == (int)eCommandRecive.BTPIncrease || c.CommandTypeId == (int)eCommandRecive.BTPReduce)).ToList();
                var monthDetails = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && x.Month == datetime.Month && x.Year == datetime.Year && x.STT_C_SP == nsObj.STTCHuyen_SanPham).ToList();
                #endregion

                if (nsObj != null && dayInfo != null)
                {
                    #region Get Content Data
                    model.LineName = nsObj.LineName;
                    model.ProductName = nsObj.ProductName;

                    model.LK_KCS = (int)pccSX.LuyKeTH;
                    model.LK_BTP = pccSX.LK_BTP;
                    model.LDDB = nsObj.LaborsBase;
                    model.LDTT = dayInfo.LaoDongChuyen;
                    model.SLCL = (pccSX.SanLuongKeHoach - model.LK_KCS);
                    #region
                    var donGia = nsObj.ProductPrice != null ? (double)nsObj.ProductPrice : 0;
                    var donGiaCM = (double)nsObj.ProductPriceCM;
                    double doanhThuTHNgay = 0, doanhThuDMNgay = 0, doanhThuKHThang = 0, doanhThuTHThang = 0, ThuNhapBQThang = 0, DoanhThuBQThang = 0, DoanhThuBQNgay = 0;
                    int dinhMucNgay = 0, finishTH = 0, finishTC = 0, finishError = 0, BTPTrenChuyen = 0, tongTHThang = 0, tongSL_KH = 0;
                    dinhMucNgay = (int)Math.Round(nsObj.DinhMucNgay);
                    doanhThuDMNgay = Math.Round((donGia * dinhMucNgay), 2);
                    doanhThuTHNgay = Math.Round((donGia * (nsObj.ThucHienNgay - nsObj.ThucHienNgayGiam)), 2);
                    DoanhThuBQNgay = Math.Round((donGiaCM * (nsObj.ThucHienNgay - nsObj.ThucHienNgayGiam)) / dayInfo.LaoDongChuyen, 2);

                    var currentMonthDetail = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == nsObj.STTCHuyen_SanPham);
                    if (currentMonthDetail != null)
                    {
                        doanhThuKHThang = Math.Round((donGia * currentMonthDetail.ProductionPlans), 2);
                        doanhThuTHThang = Math.Round((donGia * currentMonthDetail.LK_TH), 2);
                        tongTHThang = currentMonthDetail.LK_TH;
                        tongSL_KH = currentMonthDetail.ProductionPlans;
                    }


                    //if (nxInMonth.Count > 0)
                    //{
                    //    foreach (var item in nxInMonth)
                    //    {
                    //        var day_info = thanhphams.FirstOrDefault(x => x.Ngay == item.Ngay);
                    //        if (day_info != null)
                    //        {
                    //            double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                    //            ThuNhapBQThang += th > 0 ? ((th * donGia) / day_info.LaoDongChuyen) : 0;

                    //            th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                    //            DoanhThuBQThang += th > 0 ? ((th * donGiaCM) / day_info.LaoDongChuyen) : 0;
                    //        }
                    //    }
                    //    ThuNhapBQThang = (ThuNhapBQThang > 0 ? Math.Round((ThuNhapBQThang / nxInMonth.Count), 2) : 0);
                    //    DoanhThuBQThang = (DoanhThuBQThang > 0 ? Math.Round((DoanhThuBQThang / nxInMonth.Count), 2) : 0);
                    //}

                    int workingTimeOfLine = (int)bllShift.GetTotalWorkingHourOfLine(lineId).TotalSeconds;

                    ///ktra xem co ma hang nao cua chuyen nay ket thuc trong ngay hay ko
                    ///neu co lay dinh muc cua ma hang truoc tinh xem thoi gian san xuat la bao nhieu gio 
                    ///con lai bao nhieu gio de san xuat ma 2 dc bao nhieu hang 2 cai cong lai ra dc dinh muc 
                    ///cua chuyen trong ngay 

                    //var objFinishOnDay = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && !x.SanPham.IsDelete && !x.Chuyen.IsDeleted && x.MaChuyen == lineId && x.IsFinish && x.FinishedDate.HasValue && x.FinishedDate.Value.Year == datetime.Year && x.FinishedDate.Value.Month == datetime.Month && x.FinishedDate.Value.Day == datetime.Day);
                    //if (objFinishOnDay != null)
                    //{
                    //    var nsOfFinishObj = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now);
                    //    dinhMucNgay = TinhDinhMuc(objFinishOnDay, nsOfFinishObj, dayInfo.LaoDongChuyen, workingTimeOfLine, pccSX);
                    //    finishTH = nsOfFinishObj.ThucHienNgay - nsOfFinishObj.ThucHienNgayGiam;
                    //    finishTC = nsOfFinishObj.BTPThoatChuyenNgay - nsOfFinishObj.BTPThoatChuyenNgayGiam;
                    //    finishError = db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorIncrease).ToList().Sum(x => x.ThanhPham);
                    //    finishError -= db.TheoDoiNgays.Where(x => x.Date == now && x.STTChuyenSanPham == nsOfFinishObj.STTCHuyen_SanPham && x.CommandTypeId == (int)eCommandRecive.ErrorReduce).ToList().Sum(x => x.ThanhPham);
                    //    BTPTrenChuyen = nsOfFinishObj.BTPTrenChuyen;
                    //    var pro = db.SanPhams.FirstOrDefault(x => !x.IsDelete && x.MaSanPham == objFinishOnDay.MaSanPham && x.DonGia > 0);
                    //    if (pro != null)
                    //    {
                    //        doanhThuTHNgay += Math.Round((pro.DonGia * finishTH), 2);
                    //        DoanhThuBQNgay += Math.Round((pro.DonGiaCM * finishTH) / dayInfo.LaoDongChuyen, 2);

                    //        var monthD = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == objFinishOnDay.STT);
                    //        if (monthD != null)
                    //        {
                    //            var newDT = Math.Round((monthD.ProductionPlans * pro.DonGia), 2);
                    //            doanhThuKHThang += newDT;

                    //            newDT = Math.Round((monthD.LK_TH * pro.DonGia), 2);
                    //            doanhThuTHThang += newDT;

                    //            tongTHThang += monthD.LK_TH;
                    //            tongSL_KH += monthD.ProductionPlans;

                    //            var nxInMonth_finish = db.NangXuats.Where(x => !x.IsDeleted && x.STTCHuyen_SanPham == objFinishOnDay.STT && x.CreatedDate.Month == datetime.Month && x.CreatedDate.Year == datetime.Year).ToList();
                    //            var thanhpham_finish = db.ThanhPhams.Where(x => !x.IsDeleted && x.STTChuyen_SanPham == objFinishOnDay.STT).ToList();
                    //            if (nxInMonth_finish.Count > 0)
                    //            {
                    //                double TN_BQ = 0, DT_BQ = 0;
                    //                foreach (var item in nxInMonth_finish)
                    //                {
                    //                    var day_info = thanhpham_finish.FirstOrDefault(x => x.Ngay == item.Ngay);
                    //                    if (day_info != null)
                    //                    {
                    //                        double th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                    //                        TN_BQ += th > 0 ? ((th * pro.DonGia) / day_info.LaoDongChuyen) : 0;

                    //                        th = ((item.ThucHienNgay - item.ThucHienNgayGiam));
                    //                        DT_BQ += th > 0 ? ((th * pro.DonGiaCM) / day_info.LaoDongChuyen) : 0;
                    //                    }
                    //                }
                    //                ThuNhapBQThang += (TN_BQ > 0 ? Math.Round((TN_BQ / nxInMonth_finish.Count), 2) : 0);
                    //                DoanhThuBQThang += (DT_BQ > 0 ? Math.Round((DT_BQ / nxInMonth_finish.Count), 2) : 0);
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion
                    model.DMN = dinhMucNgay;
                    model.DoanhThuKH_T = doanhThuKHThang;
                    model.KCS_KH_T = tongSL_KH;
                    model.DoanhThu_T = doanhThuTHThang;
                    model.DoanhThuDM_T = doanhThuKHThang;

                    #region
                    int thucHienNgay = 0, TCNgay = 0, ErrorDay = 0;
                    //var listProductivityOnDay = listProductivity.Where(c => c.Ngay == now).ToList();
                    //if (listProductivityOnDay.Count > 0)
                    //{
                    //    foreach (var p in listProductivityOnDay)
                    //    {
                    //        var THNgay = p.ThucHienNgay - p.ThucHienNgayGiam;
                    //        if (THNgay > 0)
                    //        {
                    //            thucHienNgay += THNgay;
                    //            TCNgay += (p.BTPThoatChuyenNgay - p.BTPThoatChuyenNgayGiam);
                    //            if (p.ProductPrice > 0)
                    //                doanhThuTHNgay += Math.Round((p.ProductPrice * THNgay), 2);
                    //        }
                    //        BTPTrenChuyen += p.BTPTrenChuyen;
                    //    }
                    //}
                    //thucHienNgay += finishTH > 0 ? finishTH : 0;
                    //TCNgay += finishTC > 0 ? finishTC : 0;
                    thucHienNgay = nsObj.ThucHienNgay - nsObj.ThucHienNgayGiam;
                    model.KCS = thucHienNgay > 0 ? thucHienNgay : 0;

                    TCNgay = nsObj.BTPThoatChuyenNgay - nsObj.BTPThoatChuyenNgayGiam;
                    model.TC = TCNgay > 0 ? TCNgay : 0;

                    //  model.thucHienVaDinhMuc = thucHienNgay + " / " + dinhMucNgay;
                    model.tiLeThucHien = (dinhMucNgay > 0 && thucHienNgay > 0) ? ((int)((thucHienNgay * 100) / dinhMucNgay)) + "" : "0";

                    // model.doanhThuNgayTrenDinhMuc = doanhThuTHNgay + " / " + doanhThuDMNgay;
                    model.DoanhThu = doanhThuTHNgay;
                    model.DoanhThuDM = doanhThuDMNgay;

                    double nhipDoSanXuat = Math.Round((double)nsObj.NhipDoSanXuat, 1);
                    model.NhipSX = nhipDoSanXuat;
                    model.NhipTT = nsObj.NhipDoThucTe;

                    BTPTrenChuyen = nsObj.BTPTrenChuyen;
                    int btpTrenChuyenBinhQuan = dayInfo.LaoDongChuyen == 0 ? 0 : (int)Math.Ceiling((double)BTPTrenChuyen / dayInfo.LaoDongChuyen);
                    model.BTPInLine = BTPTrenChuyen;
                    model.BTPInLine_BQ = btpTrenChuyenBinhQuan;

                    int lightId = nsObj.IdDenNangSuat ?? 0;
                    double tyLeDen = 0;
                    if (nsObj.NhipDoThucTe > 0)
                        tyLeDen = (nhipDoSanXuat * 100) / nsObj.NhipDoThucTe;

                    var lightConfig = db.Dens.Where(c => c.IdCatalogTable == tableTypeId && c.STTParent == lightId && c.ValueFrom <= tyLeDen && tyLeDen < c.ValueTo).FirstOrDefault();
                    model.mauDen = lightConfig != null ? lightConfig.Color.Trim().ToUpper() : "ĐỎ";

                    model.LK_KCS = pccSX.LuyKeTH;
                    model.LK_TC = pccSX.LuyKeBTPThoatChuyen;
                    model.SLKH = pccSX.SanLuongKeHoach;

                    #endregion

                    model.ThuNhapBQ = Math.Round((doanhThuTHNgay / dayInfo.LaoDongChuyen));
                    model.ThuNhapBQ_T = Math.Round((ThuNhapBQThang));

                    model.DoanhThuBQ = Math.Round(DoanhThuBQNgay);
                    model.DoanhThuBQ_T = Math.Round(DoanhThuBQThang);


                    #endregion

                    #region Get Hours Productivity
                    var totalWorkingTimeInDay = bllShift.GetTotalWorkingHourOfLine(lineId);
                    int intWorkTime = (int)(totalWorkingTimeInDay.TotalHours);
                    int intWorkMinuter = (int)totalWorkingTimeInDay.TotalMinutes;
                    double NangSuatPhutKH = 0;
                    int NangSuatGioKH = 0;
                    var dateNow = DateTime.Now.Date;
                    int tongTCNgay = 0, tongKCSNgay = 0;
                    if (intWorkTime > 0)
                    {
                        NangSuatPhutKH = (double)dinhMucNgay / intWorkMinuter;
                        NangSuatGioKH = (int)(dinhMucNgay / intWorkTime);
                        if (dinhMucNgay % intWorkTime != 0)
                            NangSuatGioKH++;

                        if (hienThiNSGio == (int)eShowNSType.PercentTH_OnDay)
                        {
                            #region  hiển thị một ô năng suất hiện tại duy nhất
                            double phutToNow = GetSoPhutLamViecTrongNgay_(DateTime.Now.TimeOfDay, bllShift.GetShiftsOfLine(lineId));
                            double nsKHToNow = (phutToNow / intWorkMinuter) * model.DMN;
                            double tiLePhanTram = 0;
                            tiLePhanTram = (model.KCS > 0 && nsKHToNow > 0) ? (Math.Round((double)((model.KCS * 100) / nsKHToNow), 2)) : 0;
                            //  model.CurrentNS = model.KCS + "/" + (int)nsKHToNow + "  (" + tiLePhanTram + "%)";
                            model.SLKHToNow = (int)nsKHToNow;
                            #endregion
                        }

                        List<WorkingTimeModel> listWorkHoursOfLine = new List<WorkingTimeModel>();
                        switch (hienThiNSGio)
                        {
                            case (int)eShowNSType.PercentTH_FollowHour:
                            case (int)eShowNSType.TH_Err_FollowHour:
                            case (int)eShowNSType.TH_DM_FollowHour:
                            case (int)eShowNSType.TH_TC_FollowHour:
                                listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId);
                                break;
                            case (int)eShowNSType.PercentTH_FollowConfig:
                            case (int)eShowNSType.TH_Err_FollowConfig:
                            case (int)eShowNSType.TH_DM_FollowConfig:
                            case (int)eShowNSType.TH_TC_FollowConfig:
                                listWorkHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS);
                                break;
                            case (int)eShowNSType.TH_DM_OnDay:
                            case (int)eShowNSType.TH_TC_OnDay:
                            case (int)eShowNSType.TH_Error_OnDay:
                                listWorkHoursOfLine.Add(new WorkingTimeModel()
                                {
                                    TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                    TimeEnd = DateTime.Now.TimeOfDay,
                                    IntHours = 1,
                                });

                                listWorkHoursOfLine.Add(new WorkingTimeModel()
                                {
                                    TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                                    TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                                    IntHours = 2,
                                });
                                break;
                        }

                        if (listWorkHoursOfLine != null && listWorkHoursOfLine.Count > 0)
                        {
                            var dayInformations = db.TheoDoiNgays.Where(c => c.MaChuyen == lineId && c.Date == now && c.IsEndOfLine).Select(x => new DayInfoModel()
                            {
                                CommandTypeId = x.CommandTypeId,
                                CumId = x.CumId,
                                MaChuyen = x.MaChuyen,
                                MaSanPham = x.MaSanPham,
                                Time = x.Time,
                                Date = x.Date,
                                ErrorId = x.ErrorId,
                                IsEndOfLine = x.IsEndOfLine,
                                IsEnterByKeypad = x.IsEnterByKeypad,
                                STT = x.STT,
                                STTChuyenSanPham = x.STTChuyenSanPham,
                                ThanhPham = x.ThanhPham,
                                ProductOutputTypeId = x.ProductOutputTypeId
                            }).ToList();

                            var t = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            var g = dayInformations.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                            tongTCNgay += (t - g);

                            for (int i = 0; i < listWorkHoursOfLine.Count; i++)
                            {
                                //DM Gio
                                listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[0].TimeEnd - listWorkHoursOfLine[0].TimeStart).TotalMinutes));
                                if ((hienThiNSGio == (int)eShowNSType.TH_DM_FollowHour || hienThiNSGio == (int)eShowNSType.PercentTH_FollowHour) && i == listWorkHoursOfLine.Count - 1)
                                    listWorkHoursOfLine[i].NormsHour = Math.Round(NangSuatPhutKH * (int)((listWorkHoursOfLine[i].TimeEnd - listWorkHoursOfLine[i].TimeStart).TotalMinutes));

                                #region
                                int Tang = 0, Giam = 0;
                                var theoDoiNgays = dayInformations.Where(c => c.MaChuyen == lineId && c.Time > listWorkHoursOfLine[i].TimeStart && c.Time <= listWorkHoursOfLine[i].TimeEnd && c.Date == now && c.IsEndOfLine).ToList();
                                if (theoDoiNgays.Count > 0)
                                {
                                    //Kcs
                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.KCS).Sum(c => c.ThanhPham);
                                    Tang -= Giam;

                                    listWorkHoursOfLine[i].KCS = Tang;
                                    listWorkHoursOfLine[i].HoursProductivity = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + listWorkHoursOfLine[i].NormsHour;
                                    tongKCSNgay += Tang;

                                    // TC
                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductIncrease && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ProductReduce && c.ProductOutputTypeId == (int)eProductOutputType.TC).Sum(c => c.ThanhPham);
                                    Tang -= Giam;

                                    listWorkHoursOfLine[i].TC = Tang;
                                    listWorkHoursOfLine[i].HoursProductivity_1 = (listWorkHoursOfLine[i].KCS < 0 ? 0 : listWorkHoursOfLine[i].KCS) + "/" + (listWorkHoursOfLine[i].TC < 0 ? 0 : listWorkHoursOfLine[i].TC);
                                    //   tongTCNgay += Tang;

                                    // lỗi
                                    Tang = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorIncrease).Sum(c => c.ThanhPham);
                                    Giam = theoDoiNgays.Where(c => c.CommandTypeId == (int)eCommandRecive.ErrorReduce).Sum(c => c.ThanhPham);
                                    Tang -= Giam;
                                    listWorkHoursOfLine[i].Error = Tang;
                                    model.Error += Tang;

                                }
                                else
                                {
                                    listWorkHoursOfLine[i].HoursProductivity = "0/" + listWorkHoursOfLine[i].NormsHour;
                                    listWorkHoursOfLine[i].HoursProductivity_1 = "0/0";
                                }
                                #endregion
                            }
                        }

                        model.Error += finishError;
                        model.listWorkHours = listWorkHoursOfLine;
                        model.KieuHienThiNangSuatGio = db.Configs.Where(x => x.Name.Trim().ToUpper().Equals("KieuHienThiNangSuatGio")).FirstOrDefault().ValueDefault.Trim();

                        // tong thuc hien cua chuyen trong ngay cua cac ma hang
                        // model.tongThucHienNgay = tongKCSNgay + "/" + tongDinhMucNgay;
                        //  model.tongThucHienNgay = tongKCSNgay + "/" + dinhMucNgay;
                        // tong kcs cua chuyen cho cac ma hang trong ngay
                        //  model.ThoatChuyen = tongTCNgay + " / " + pccSX.LuyKeBTPThoatChuyen;
                    }
                    #endregion
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private double GetSoPhutLamViecTrongNgay_(TimeSpan timeNow, List<LineWorkingShiftModel> workShift)
        {
            double soPhut = 0;
            try
            {
                if (workShift != null && workShift.Count > 0)
                {
                    foreach (var sh in workShift)
                    {
                        if (timeNow >= sh.Start)
                        {
                            if (timeNow < sh.End)
                            {
                                var h = timeNow.Hours - sh.Start.Hours;
                                soPhut += (h * 60 + timeNow.Minutes) - sh.Start.Minutes;
                            }
                            else if (timeNow >= sh.End)
                                soPhut += ((sh.End - sh.Start).TotalMinutes);
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return soPhut;
        }

        private static int TinhDinhMuc(Chuyen_SanPham currentPC, dynamic currentNS, int labours, int workingTimeOfLine, Chuyen_SanPham pc_next)
        {
            //var nangSuatLaoDongNow = currentNS.DinhMucNgay / labours;
            //int totalSecondFinishMH1 = (int)(nangSuatLaoDongNow * currentPC);
            //int secondsAfter = workingTimeOfLine - totalSecondFinishMH1;

            //double dinhMuc_Next = (secondsAfter / pc_next.NangXuatSanXuat) * labours;
            //return (int)Math.Round(currentNS.DinhMucNgay + dinhMuc_Next);
            return 0;
        }


        public ModelLCDGeneral GetGeneralInfo(List<int> listLineId, int tableType, bool IsKanbanAcc)
        {
            try
            {
                var model = new ModelLCDGeneral();
                var datetime = DateTime.Now;
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                if (listLineId != null && listLineId.Count() > 0)
                {
                    listLineId = listLineId.Distinct().ToList();
                    var listPCC = db.Chuyen_SanPham.Where(c => !c.IsFinish && listLineId.Contains(c.MaChuyen) && !c.IsDelete && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted).Select(x => new QLNSService.Models.ChuyenSanPhamModel()
                    {
                        STT = x.STT,
                        STTThucHien = x.STTThucHien,
                        Thang = x.Thang,
                        Nam = x.Nam,
                        MaChuyen = x.MaChuyen,
                        TenChuyen = x.Chuyen.TenChuyen,
                        MaSanPham = x.MaSanPham,
                        TenSanPham = x.SanPham.TenSanPham,
                        DonGiaSanXuat = x.SanPham.DonGia,
                        SanLuongKeHoach = x.SanLuongKeHoach,
                        LuyKeBTPThoatChuyen = x.LuyKeBTPThoatChuyen,
                        LuyKeTH = x.LuyKeTH,
                        IsFinish = x.IsFinish,
                        TimeAdd = x.TimeAdd,
                        IsFinishNow = x.IsFinishNow,
                        IsFinishBTPThoatChuyen = x.IsFinishBTPThoatChuyen,
                        IsMoveQuantityFromMorthOld = x.IsMoveQuantityFromMorthOld
                    }).ToList();

                    if (listPCC.Count > 0)
                    {
                        var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                        var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted).ToList();
                        var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted).ToList();
                        var listBTPOfLine = db.BTPs.Where(c => !c.IsBTP_PB_HC && listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == 8 || c.CommandTypeId == 13)).ToList();

                        var monthInfos = db.P_MonthlyProductionPlans.Where(x => !x.IsDeleted && listSTTLineProduct.Contains(x.STT_C_SP) && x.Month == datetime.Month && x.Year == datetime.Year).Select(x => new MonthlyPlansModel()
                        {
                            Month = x.Month,
                            Year = x.Year,
                            STT_C_SP = x.STT_C_SP,
                            ProductionPlans = x.ProductionPlans,
                            LK_BTP = x.LK_BTP,
                            LK_BTP_HC = x.LK_BTP_HC,
                            LK_TC = x.LK_TC,
                            LK_TH = x.LK_TH,
                            Id = x.Id,
                            Price = x.Chuyen_SanPham.SanPham.DonGia,
                            PriceCM = x.Chuyen_SanPham.SanPham.DonGiaCM,
                            LineId = x.Chuyen_SanPham.MaChuyen
                        }).ToList();


                        int tongTHThang = 0;
                        int tongKHThang = 0;
                        double tongDoanhThuTHThang = 0;
                        double tongDoanhThuKHThang = 0;
                        double tongNhipSanXuat = 0;
                        double tongNhipThucTe = 0;
                        int soNangXuat = 0;
                        int maxWorkHourslineId = 0;
                        int maxWorkHours = 0;
                        int tongDinhMucNgay = 0;
                        int keHoachThang = 0;
                        int thucHienThang = 0;
                        double doanhThuKH = 0;
                        double doanhThuTH = 0;

                        if (!IsKanbanAcc)
                        {
                            #region Sewing LCD
                            foreach (var lineId in listLineId)
                            {
                                int workHoursOfLine = bllShift.CountWorkHoursInDayOfLine(lineId);
                                if (workHoursOfLine > maxWorkHours)
                                {
                                    maxWorkHours = workHoursOfLine;
                                    maxWorkHourslineId = lineId;
                                }


                                var listPCCOfLine = listPCC.Where(c => c.MaChuyen == lineId).ToList();
                                if (listPCCOfLine.Count > 0)
                                {
                                    var modelMorthProductivity = new ModelMorthProductivity();
                                    var stt = listPCCOfLine.FirstOrDefault() != null ? listPCCOfLine.FirstOrDefault().STT : 0;

                                    modelMorthProductivity.LineName = listPCCOfLine.FirstOrDefault().TenChuyen;
                                    modelMorthProductivity.CommoName = listPCCOfLine.FirstOrDefault().TenSanPham;

                                    keHoachThang = 0;
                                    thucHienThang = 0;
                                    doanhThuKH = 0;
                                    doanhThuTH = 0;
                                    foreach (var pcc in monthInfos.Where(x => x.LineId == lineId))
                                    {
                                        double donGia = pcc.Price;
                                        doanhThuKH += pcc.ProductionPlans * donGia;
                                        doanhThuTH += pcc.LK_TH * donGia;
                                        keHoachThang += pcc.ProductionPlans;
                                        thucHienThang += pcc.LK_TH;
                                    }
                                    modelMorthProductivity.DoanhThuKH = doanhThuKH;
                                    modelMorthProductivity.THThang = thucHienThang;
                                    modelMorthProductivity.DoanhThuTH = doanhThuTH;
                                    modelMorthProductivity.KHThang = keHoachThang;
                                    modelMorthProductivity.TiLeTH = (thucHienThang > 0 && keHoachThang > 0 ? (double)Math.Round((((double)thucHienThang * 100) / keHoachThang)) : 0);
                                    tongTHThang += thucHienThang;
                                    tongKHThang += keHoachThang;
                                    tongDoanhThuTHThang += doanhThuTH;
                                    tongDoanhThuKHThang += doanhThuKH;
                                    var sttPCCOfLine = listPCCOfLine.Select(c => c.STT).ToList();
                                    var productivityOfLineOnDay = listProductivity.Where(c => sttPCCOfLine.Contains(c.STTCHuyen_SanPham) && c.Ngay == now).ToList();
                                    if (productivityOfLineOnDay.Count > 0)
                                    {
                                        modelMorthProductivity.KHNgay = (int)productivityOfLineOnDay.Sum(c => c.DinhMucNgay);
                                        tongDinhMucNgay += modelMorthProductivity.KHNgay;
                                        int thucHienNgay = productivityOfLineOnDay.Sum(c => c.ThucHienNgay);
                                        int thucHienNgayGiam = productivityOfLineOnDay.Sum(c => c.ThucHienNgayGiam);
                                        modelMorthProductivity.THNgay = thucHienNgay - thucHienNgayGiam;
                                        if (modelMorthProductivity.THNgay < 0)
                                            modelMorthProductivity.THNgay = 0;
                                    }
                                    var productivityOfLine = listProductivity.Where(c => sttPCCOfLine.Contains(c.STTCHuyen_SanPham) && c.Ngay == now).ToList();
                                    if (productivityOfLine.Count > 0)
                                    {
                                        tongNhipSanXuat += productivityOfLine.Sum(c => c.NhipDoSanXuat);
                                        tongNhipThucTe += productivityOfLine.Sum(c => c.NhipDoThucTe);
                                        soNangXuat += productivityOfLine.Count;
                                    }
                                    model.ListLineMorthProductivity.Add(modelMorthProductivity);
                                }

                            }
                            model.KHThang = tongKHThang;
                            model.THThang = tongTHThang;
                            model.DoanhThuKHThang = tongDoanhThuKHThang;
                            model.DoanhThuTHThang = tongDoanhThuTHThang;
                            if (soNangXuat > 0)
                            {
                                model.NhipSanXuatTH = Math.Round(tongNhipSanXuat / soNangXuat, 2);
                                model.NhipThucTeKH = Math.Round(tongNhipThucTe / soNangXuat, 2);
                            }
                            if (maxWorkHourslineId > 0)
                            {
                                var listModelWorkHours = bllShift.GetListWorkHoursOfLineByLineId(maxWorkHourslineId);
                                if (listModelWorkHours != null && listModelWorkHours.Count > 0)
                                {
                                    int nangSuatGioKH = (int)(tongDinhMucNgay / listModelWorkHours.Count);
                                    foreach (var item in listModelWorkHours)
                                    {
                                        int sanLuongGioTang = 0;
                                        int sanLuongGioGiam = 0;
                                        int sanLuongGio = 0;
                                        var theoDoiNgays = db.TheoDoiNgays.Where(c => listLineId.Contains(c.MaChuyen) && c.Time >= item.TimeStart && c.Time <= item.TimeEnd && c.Date == now && (c.CommandTypeId == 4 || c.CommandTypeId == 5) && c.ProductOutputTypeId == 1 && c.IsEndOfLine).ToList();
                                        if (theoDoiNgays.Count > 0)
                                        {
                                            sanLuongGioTang = theoDoiNgays.Where(c => c.CommandTypeId == 4).Sum(c => c.ThanhPham);
                                            sanLuongGioGiam = theoDoiNgays.Where(c => c.CommandTypeId == 5).Sum(c => c.ThanhPham);
                                            sanLuongGio = sanLuongGioTang - sanLuongGioGiam;
                                        }
                                        if (sanLuongGio < 0)
                                            sanLuongGio = 0;
                                        item.HoursProductivity = sanLuongGio.ToString() + "|" + nangSuatGioKH.ToString();
                                        item.KCS = sanLuongGio;
                                        item.NormsHour = nangSuatGioKH;
                                    }
                                }
                                model.ListHoursProductivity = listModelWorkHours;
                                model.Morth = "THÁNG " + datetime.Month + " - " + datetime.Year;
                            }
                            #endregion
                        }
                        else
                        {
                            #region KANBAN AND COMPLETE LCD
                            foreach (var lineId in listLineId)
                            {
                                int workHoursOfLine = bllShift.CountWorkHoursInDayOfLine(lineId);
                                if (workHoursOfLine > maxWorkHours)
                                {
                                    maxWorkHours = workHoursOfLine;
                                    maxWorkHourslineId = lineId;
                                }
                                keHoachThang = 0; thucHienThang = 0;
                                keHoachThang = listPCC.Where(x => x.MaChuyen == lineId).Sum(x => x.SanLuongKeHoach);
                                thucHienThang = listPCC.Where(x => x.MaChuyen == lineId).Sum(x => x.LuyKeTH);

                                ModelMorthProductivity productivityModel = null;
                                var listPCCOfLine = listPCC.Where(c => c.MaChuyen == lineId).ToList();
                                if (listPCCOfLine != null && listPCCOfLine.Count > 0)
                                {
                                    foreach (var pc in listPCCOfLine)
                                    {
                                        productivityModel = new ModelMorthProductivity();
                                        productivityModel.LineName = pc.TenChuyen;
                                        productivityModel.CommoName = pc.TenSanPham;
                                        productivityModel.THThang = thucHienThang;
                                        productivityModel.KHThang = keHoachThang;
                                        productivityModel.TiLeTH = (double)Math.Round((decimal)(((double)thucHienThang * 100) / keHoachThang), 2);

                                        var productivityOfLineOnDay = listProductivity.Where(c => pc.STT == c.STTCHuyen_SanPham && c.Ngay == now).ToList();
                                        if (productivityOfLineOnDay.Count > 0)
                                        {
                                            productivityModel.KHNgay = (int)productivityOfLineOnDay.Sum(c => c.DinhMucNgay);
                                            tongDinhMucNgay += productivityModel.KHNgay;
                                            int thucHienNgay = productivityOfLineOnDay.Sum(c => c.ThucHienNgay);
                                            int thucHienNgayGiam = productivityOfLineOnDay.Sum(c => c.ThucHienNgayGiam);
                                            productivityModel.THNgay = thucHienNgay - thucHienNgayGiam;
                                            if (productivityModel.THNgay < 0)
                                                productivityModel.THNgay = 0;
                                        }

                                        var productivityOfLine = listProductivity.Where(c => pc.STT == c.STTCHuyen_SanPham && c.Ngay == now).ToList();
                                        if (productivityOfLine.Count > 0)
                                        {
                                            tongNhipSanXuat += productivityOfLine.Sum(c => c.NhipDoSanXuat);
                                            tongNhipThucTe += productivityOfLine.Sum(c => c.NhipDoThucTe);
                                            soNangXuat += productivityOfLine.Count;
                                        }
                                        model.ListLineMorthProductivity.Add(productivityModel);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelKanBanInfo> GetKanBanInfo(List<int> listLineId, int tableType, bool includingBTPHC)
        {
            try
            {
                var listModel = new List<ModelKanBanInfo>();
                var dateTime = DateTime.Now;
                var now = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                if (listLineId != null && listLineId.Count > 0)
                {
                    listLineId = listLineId.Distinct().ToList();
                    var listPCC = db.Chuyen_SanPham.Where(c => listLineId.Contains(c.MaChuyen) && !c.IsDelete && !c.IsFinish && !c.SanPham.IsDelete && !c.Chuyen.IsDeleted).ToList();
                    if (listPCC.Count > 0)
                    {
                        var listSTTLineProduct = listPCC.Select(c => c.STT).ToList();
                        var listProductivity = db.NangXuats.Where(c => listSTTLineProduct.Contains(c.STTCHuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();
                        var listDayInfo = db.ThanhPhams.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.Ngay == now).ToList();
                        var listBTPOfLine = db.BTPs.Where(c => listSTTLineProduct.Contains(c.STTChuyen_SanPham) && !c.IsDeleted && c.IsEndOfLine && (c.CommandTypeId == 8 || c.CommandTypeId == 13)).ToList();
                        foreach (var item in listPCC.OrderBy(x => x.MaChuyen).ThenBy(x => x.STTThucHien))
                        {
                            var model = new ModelKanBanInfo();
                            var line = db.Chuyens.Where(c => c.MaChuyen == item.MaChuyen && !c.IsDeleted).FirstOrDefault();
                            var PCOfLine = listPCC.FirstOrDefault(c => c.STT == item.STT);
                            if (line != null && PCOfLine != null && listProductivity.Count > 0 && listDayInfo.Count > 0)
                            {
                                model.LineName = line.TenChuyen;
                                var product = db.SanPhams.Where(c => c.MaSanPham == PCOfLine.MaSanPham && !c.IsDelete).FirstOrDefault();
                                if (product != null)
                                    model.ProductName = product.TenSanPham;
                                int btpTrenChuyen = 0;
                                int dinhMucNgay = 0;
                                var productivity = listProductivity.Where(c => c.STTCHuyen_SanPham == PCOfLine.STT).FirstOrDefault();
                                if (productivity != null)
                                {
                                    btpTrenChuyen = productivity.BTPTrenChuyen;
                                    dinhMucNgay = (int)productivity.DinhMucNgay;
                                }
                                int laoDongChuyen = 0;
                                var dayInfo = listDayInfo.Where(c => c.STTChuyen_SanPham == PCOfLine.STT).FirstOrDefault();
                                if (dayInfo != null)
                                    laoDongChuyen = dayInfo.LaoDongChuyen;
                                var listBTP = listBTPOfLine.Where(c => c.STTChuyen_SanPham == PCOfLine.STT).ToList();
                                int btpGiaoChuyenNgay = 0;
                                int luyKeBTP_HC = 0, luyKeBTP = 0;
                                if (listBTP != null && listBTP.Count > 0)
                                {
                                    int btpGiaoChuyenNgayTang = listBTP.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    int btpGiaoChuyenNgayGiam = listBTP.Where(c => !c.IsBTP_PB_HC && c.Ngay == now && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    btpGiaoChuyenNgay = btpGiaoChuyenNgayTang - btpGiaoChuyenNgayGiam;
                                    int luyKeBTPTang = listBTP.Where(c => c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    int luyKeBTPGiam = listBTP.Where(c => c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    luyKeBTP_HC = luyKeBTPTang - luyKeBTPGiam;

                                    luyKeBTPTang = listBTP.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPIncrease).Sum(c => c.BTPNgay);
                                    luyKeBTPGiam = listBTP.Where(c => !c.IsBTP_PB_HC && c.CommandTypeId == (int)eCommandRecive.BTPReduce).Sum(c => c.BTPNgay);
                                    luyKeBTP = luyKeBTPTang - luyKeBTPGiam;
                                }
                                int btpBinhQuan = laoDongChuyen == 0 ? 0 : btpTrenChuyen / laoDongChuyen;

                                int von = btpTrenChuyen > 0 && laoDongChuyen > 0 ? (int)(Math.Ceiling((double)btpTrenChuyen / laoDongChuyen)) : 0;

                                int tyLeDenThucTe = von;// dinhMucNgay == 0 ? 0 : (btpTrenChuyen * 100) / dinhMucNgay;
                                model.BTPOnDay = btpGiaoChuyenNgay;
                                model.LK_BTP_HC = luyKeBTP_HC;
                                model.ProductionPlans = PCOfLine.SanLuongKeHoach;
                                model.BTPBQ = btpBinhQuan + "|" + btpTrenChuyen;
                                model.BTPBinhQuan = btpBinhQuan;
                                model.BTPInLine = btpTrenChuyen;
                                model.LK_BTP = luyKeBTP;
                                string colorDen = "Black";
                                var listTyLeDen = db.P_ReadPercentOfLine.FirstOrDefault(x => !x.IsDeleted && !x.P_LightPercent.IsDeleted && !x.Chuyen_SanPham.IsDelete && (x.Chuyen_SanPham.LuyKeBTPThoatChuyen > 0 || x.Chuyen_SanPham.LuyKeTH > 0) && x.Chuyen_SanPham.SanLuongKeHoach > x.Chuyen_SanPham.LK_BTP && x.AssignmentId == item.STT && x.P_LightPercent.Type == (int)PMS.Business.Enum.eLightType.KanBan && x.LineId == item.MaChuyen);
                                if (listTyLeDen != null)
                                {
                                    var den = db.P_LightPercent_De.FirstOrDefault(c => tyLeDenThucTe >= c.From && tyLeDenThucTe <= c.To && c.LightPercentId == listTyLeDen.KanbanLightPercentId);
                                    if (den != null)
                                    {
                                        if (den.ColorName.Trim().ToUpper().Equals("ĐỎ"))
                                            colorDen = "Red";
                                        else if (den.ColorName.Trim().ToUpper().Equals("VÀNG"))
                                            colorDen = "Yellow";
                                        if (den.ColorName.Trim().ToUpper().Equals("XANH"))
                                            colorDen = "Blue";
                                    }
                                }
                                model.StatusColor = colorDen;

                                //lay thong tin btphc
                                if (includingBTPHC)
                                {
                                    model.BTPHC_Structs.AddRange(db.P_Phase.Where(x => !x.IsDeleted && x.Type == (int)PMS.Business.Enum.ePhaseType.BTP_HC && x.IsShow).Select(x => new BTP_HCStructureModel()
                                          {
                                              Id = x.Id,
                                              Index = x.Index,
                                              Name = x.Name,
                                              Note = "0"
                                          }).OrderBy(x => x.Index));
                                    if (model.BTPHC_Structs.Count > 0)
                                    {
                                        var btpHC_collec = db.P_PhaseDaily.Where(x => x.NangXuat.STTCHuyen_SanPham == item.STT).ToList();
                                        if (btpHC_collec.Count > 0)
                                        {
                                            foreach (var iObj in model.BTPHC_Structs)
                                            {
                                                var sl = 0;
                                                sl = btpHC_collec.Where(x => x.PhaseId == iObj.Id).Sum(x => x.Quantity);
                                                iObj.Note = sl.ToString();
                                            }
                                        }
                                    }

                                }

                                listModel.Add(model);
                            }
                        }
                    }
                }
                return listModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // lay nag suat chuyen trong ngay ve chart
        public List<ProductivitiesOfLineByHoursModel> GetProductivities(List<int> lineIds, DateTime date, bool isOneLine, bool IsProductOutput)
        {
            try
            {
                var now = date.Day + "/" + date.Month + "/" + date.Year;
                var lines = new List<ProductivitiesOfLineByHoursModel>();
                if (lineIds != null && lineIds.Count > 0)
                {
                    var linesInfo = db.Chuyens.Where(x => !x.IsDeleted && lineIds.Contains(x.MaChuyen));
                    var productivitiesOfDay = db.TheoDoiNgays.Where(x => lineIds.Contains(x.MaChuyen) && x.Date == now && x.IsEndOfLine);

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
                            var productivities = productivitiesOfDay.Where(x => x.MaChuyen == line.MaChuyen);
                            IQueryable<TheoDoiNgay> pro_Increases = null, pro_Reduced = null;
                            if (productivities != null && productivities.Count() > 0)
                            {
                                if (IsProductOutput) //tp thoat chuyen 
                                {
                                    pro_Increases = productivities.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC);
                                    pro_Reduced = productivities.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC);
                                }
                                else
                                {
                                    pro_Increases = productivities.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS);
                                    pro_Reduced = productivities.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS);
                                }
                            }
                            var sl = ((pro_Increases != null && pro_Increases.Count() > 0 ? pro_Increases.Sum(x => x.ThanhPham) : 0) - (pro_Reduced != null && pro_Reduced.Count() > 0 ? pro_Reduced.Sum(x => x.ThanhPham) : 0));
                            //
                            double DinhMuc = 0;
                            var stt = listPCC.FirstOrDefault(x => x.MaChuyen == line.MaChuyen);
                            if (stt != null)
                            {
                                var ns = listProductivityOfDay.FirstOrDefault(x => x.STTCHuyen_SanPham == stt.STT);
                                DinhMuc = Math.Round(ns.DinhMucNgay);
                            }
                            var obj = new ProductivitiesOfLineByHoursModel()
                            {
                                LineName = line.TenChuyen,
                                Date = date.Date,
                                ProductivitiesOfLine = (sl < 0 ? 0 : sl),
                                NormsDay = DinhMuc
                            };

                            //
                            if (isOneLine)
                            {
                                #region  nếu chi lay nang suat cua chuyen trong ngay theo gio lam viec
                                var NSChuyenOfDay = db.TheoDoiNgays.Where(x => x.MaChuyen == line.MaChuyen && x.Date == now && x.IsEndOfLine);
                                var workHoursOfLine = bllShift.GetListWorkHoursOfLineByLineId(line.MaChuyen);
                                if (workHoursOfLine.Count > 0)
                                {
                                    if (NSChuyenOfDay != null && NSChuyenOfDay.Count() > 0)
                                    {
                                        foreach (var item in workHoursOfLine)
                                        {
                                            var NSC = NSChuyenOfDay.Where(x => x.Time >= item.TimeStart && x.Time <= item.TimeEnd);
                                            IQueryable<TheoDoiNgay> tang = null, giam = null;
                                            if (NSC != null && NSC.Count() > 0)
                                            {
                                                if (IsProductOutput)
                                                {
                                                    tang = NSC.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.TC);
                                                    giam = NSC.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.TC);
                                                }
                                                else
                                                {
                                                    tang = NSC.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductIncrease && x.ProductOutputTypeId == (int)eProductOutputType.KCS);
                                                    giam = NSC.Where(x => x.CommandTypeId == (int)eCommandRecive.ProductReduce && x.ProductOutputTypeId == (int)eProductOutputType.KCS);
                                                }
                                            }
                                            var total = ((tang != null && tang.Count() > 0 ? tang.Sum(x => x.ThanhPham) : 0) - (giam != null && giam.Count() > 0 ? giam.Sum(x => x.ThanhPham) : 0));
                                            obj.Productivities.Add(new ProductivitiesByHoursModel()
                                            {
                                                HourName = (item.TimeEnd.Hours + "H:" + item.TimeEnd.Minutes + "'"),
                                                Value = (total < 0 ? 0 : total),
                                                NormsHour = Math.Round(DinhMuc / workHoursOfLine.Count)
                                            });
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in workHoursOfLine)
                                        {
                                            obj.Productivities.Add(new ProductivitiesByHoursModel()
                                            {
                                                HourName = (item.TimeEnd.Hours + "H:" + item.TimeEnd.Minutes + "'"),
                                                Value = 0
                                            });
                                        }
                                    }
                                }
                                #endregion
                            }
                            lines.Add(obj);
                        }
                        #endregion
                    }
                }
                return lines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertProductivity(InsertProductivityModel productModel)
        {
            try
            {
                var date = DateTime.Now.Date;
                var timeNow = DateTime.Now.TimeOfDay;
                var now = date.Day + "/" + date.Month + "/" + date.Year;
                var chuyen_Sp = db.Chuyen_SanPham.FirstOrDefault(x => !x.IsDelete && x.MaChuyen == productModel.LineId && x.MaSanPham == productModel.ProductId && x.Thang == date.Month && x.Nam == date.Year);
                if (chuyen_Sp != null)
                {
                    var NS_Cum = db.NangSuat_Cum.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now && x.STTChuyen_SanPham == chuyen_Sp.STT && x.IdCum == productModel.ClusterId);
                    var NangSuat = db.NangXuats.FirstOrDefault(x => !x.IsDeleted && x.Ngay == now && x.STTCHuyen_SanPham == chuyen_Sp.STT);
                    var monthInfo = db.P_MonthlyProductionPlans.FirstOrDefault(x => x.STT_C_SP == chuyen_Sp.STT && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                    var hieuTime = TimeIsWork(productModel.LineId, db, date);
                    int second = (int)hieuTime.TotalSeconds;
                    var config = db.Configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.GetBTPInLineByType) && x.IsActive);
                    int getBTPInLineByType = 1;
                    int appId = 11;
                    if (config != null)
                    {
                        var appConfig = db.Config_App.FirstOrDefault(x => x.ConfigId == config.Id && x.AppId == appId);
                        if (appConfig != null)
                            int.TryParse(appConfig.Value, out getBTPInLineByType);
                    }

                    //update NS_Cum
                    var theodoingay = new TheoDoiNgay();
                    theodoingay.MaChuyen = productModel.LineId;
                    theodoingay.MaSanPham = productModel.ProductId;
                    theodoingay.CumId = productModel.ClusterId;
                    theodoingay.STTChuyenSanPham = chuyen_Sp.STT;
                    theodoingay.ThanhPham = productModel.Quantity;
                    theodoingay.Time = timeNow;
                    theodoingay.Date = now;
                    theodoingay.IsEndOfLine = true;
                    theodoingay.ErrorId = null;

                    if (NS_Cum == null)
                        NS_Cum = Create_NS_Cum(now, chuyen_Sp.STT, productModel.ClusterId);
                    if (NangSuat == null)
                    {
                        var thanhPham = db.ThanhPhams.FirstOrDefault(x => !x.IsDeleted && x.STTChuyen_SanPham == chuyen_Sp.STT);
                        NangSuat = Create_NangSuat(now, chuyen_Sp.STT, thanhPham.NangXuatLaoDong, thanhPham.LaoDongChuyen, Math.Round((chuyen_Sp.SanPham.ProductionTime * 100) / thanhPham.HieuSuat));
                    }

                    var btps = db.BTPs.Where(x => !x.IsBTP_PB_HC && x.STTChuyen_SanPham == chuyen_Sp.STT && x.IsEndOfLine);
                    int luykeTang = 0, luykeGiam = 0;
                    if (btps != null && btps.Count() > 0)
                    {
                        var a = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPIncrease);
                        luykeTang = (a != null && a.Count() > 0 ? a.Sum(x => x.BTPNgay) : 0);
                        a = null;
                        a = btps.Where(x => x.CommandTypeId == (int)eCommandRecive.BTPReduce);
                        luykeGiam = (a != null && a.Count() > 0 ? a.Sum(x => x.BTPNgay) : 0);
                    }

                    switch (productModel.TypeOfProductivity)
                    {
                        case (int)eCommandRecive.ProductKCSIncrease:
                            #region KCS - Kiểm đạt
                            if (productModel.IsIncrease)
                            {
                                NS_Cum.SanLuongKCSTang += productModel.Quantity;

                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductIncrease;

                                chuyen_Sp.LuyKeTH += productModel.Quantity;
                                NangSuat.ThucHienNgay = NS_Cum.SanLuongKCSTang;

                                int nhipDoThucTe = second / (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam);
                                NangSuat.NhipDoThucTe = nhipDoThucTe;

                                if (monthInfo != null)
                                    monthInfo.LK_TH += productModel.Quantity;
                            }
                            else
                            {
                                NS_Cum.SanLuongKCSGiam += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductReduce;

                                chuyen_Sp.LuyKeTH -= productModel.Quantity;
                                NangSuat.ThucHienNgayGiam = NS_Cum.SanLuongKCSGiam;

                                int thucHienNgay = NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam;
                                if (thucHienNgay > 0)
                                {
                                    int nhipDoThucTe = second / thucHienNgay;
                                    NangSuat.NhipDoThucTe = nhipDoThucTe;
                                }

                                if (monthInfo != null)
                                    monthInfo.LK_TH -= productModel.Quantity;
                            }
                            theodoingay.ProductOutputTypeId = (int)eProductOutputType.KCS;
                            NangSuat.IsBTP = 1;
                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    NangSuat.BTPTrenChuyen = ((luykeTang - luykeGiam) - (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam));
                                    break;
                                case 2:
                                    NangSuat.BTPTrenChuyen = ((luykeTang - luykeGiam) - (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam));
                                    break;
                            }
                            #endregion
                            db.TheoDoiNgays.Add(theodoingay);
                            break;
                        case (int)eCommandRecive.ProductTCIncrease:
                            #region Thoát Chuyền
                            if (productModel.IsIncrease)
                            {
                                NS_Cum.SanLuongTCTang += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductIncrease;

                                chuyen_Sp.LuyKeBTPThoatChuyen += productModel.Quantity;
                                NangSuat.BTPThoatChuyenNgay = NS_Cum.SanLuongTCTang;

                                if (monthInfo != null)
                                    monthInfo.LK_TC += productModel.Quantity;
                            }
                            else
                            {
                                NS_Cum.SanLuongTCGiam += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductReduce;

                                chuyen_Sp.LuyKeBTPThoatChuyen -= productModel.Quantity;
                                NangSuat.BTPThoatChuyenNgayGiam = NS_Cum.SanLuongTCGiam;

                                if (monthInfo != null)
                                    monthInfo.LK_TC += productModel.Quantity;
                            }

                            theodoingay.ProductOutputTypeId = (int)eProductOutputType.TC;
                            NangSuat.IsBTP = 1;
                            int btpThoatChuyenNgay = (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam);
                            if (btpThoatChuyenNgay > 0)
                            {
                                int nhipDoThucTeBTPThoatChuyen = second / btpThoatChuyenNgay;
                                NangSuat.NhipDoThucTeBTPThoatChuyen = nhipDoThucTeBTPThoatChuyen;
                            }

                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    NangSuat.BTPTrenChuyen = ((luykeTang - luykeGiam) - (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam));
                                    break;
                                case 2:
                                    NangSuat.BTPTrenChuyen = ((luykeTang - luykeGiam) - (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam));
                                    break;
                            }
                            #endregion
                            db.TheoDoiNgays.Add(theodoingay);
                            break;
                        case (int)eCommandRecive.BTPIncrease:
                            #region BTP
                            var btp = new BTP();
                            btp.Ngay = now;
                            btp.STTChuyen_SanPham = chuyen_Sp.STT;
                            btp.STT = 1;
                            btp.CumId = productModel.ClusterId;
                            btp.BTPNgay = productModel.Quantity;
                            btp.TimeUpdate = timeNow;
                            btp.IsEndOfLine = true;

                            if (productModel.IsIncrease)
                            {
                                NS_Cum.BTPTang += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductIncrease;
                                theodoingay.ProductOutputTypeId = (int)eProductOutputType.TC;
                                NangSuat.BTPTang = NS_Cum.BTPTang;
                                btp.CommandTypeId = (int)eCommandRecive.BTPIncrease;
                                if (monthInfo != null)
                                    monthInfo.LK_BTP += productModel.Quantity;
                            }
                            else
                            {
                                NS_Cum.BTPGiam += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ProductReduce;
                                theodoingay.ProductOutputTypeId = (int)eProductOutputType.TC;
                                NangSuat.BTPGiam = NS_Cum.BTPGiam;
                                btp.CommandTypeId = (int)eCommandRecive.BTPReduce;
                                if (monthInfo != null)
                                    monthInfo.LK_BTP += productModel.Quantity;
                            }
                            NangSuat.IsBTP = 1;
                            switch (getBTPInLineByType)
                            {
                                case 1:
                                    NangSuat.BTPTrenChuyen = (((luykeTang - luykeGiam) + productModel.Quantity) - (NangSuat.ThucHienNgay - NangSuat.ThucHienNgayGiam));
                                    break;
                                case 2:
                                    NangSuat.BTPTrenChuyen = (((luykeTang - luykeGiam) + productModel.Quantity) - (NangSuat.BTPThoatChuyenNgay - NangSuat.BTPThoatChuyenNgayGiam));
                                    break;
                            }
                            #endregion
                            db.BTPs.Add(btp);
                            break;
                        case (int)eCommandRecive.ErrorIncrease:
                            #region Error
                            var NSCumLoi = db.NangSuat_CumLoi.FirstOrDefault(x => !x.IsDeleted && x.STTChuyenSanPham == chuyen_Sp.STT && x.CumId == productModel.ClusterId && x.Ngay == now && x.ErrorId == productModel.ErrorId);
                            var exists = false;
                            if (NSCumLoi == null)
                            {
                                exists = true;
                                NSCumLoi = new NangSuat_CumLoi();
                                NSCumLoi.CumId = productModel.ClusterId;
                                NSCumLoi.STTChuyenSanPham = chuyen_Sp.STT;
                                NSCumLoi.ErrorId = productModel.ErrorId;
                                NSCumLoi.Ngay = now;
                                NSCumLoi.SoLuongGiam = 0;
                                NSCumLoi.SoLuongTang = 0;
                            }

                            if (productModel.IsIncrease)
                            {
                                NSCumLoi.SoLuongTang += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ErrorIncrease;
                                NangSuat.SanLuongLoi += productModel.Quantity;
                            }
                            else
                            {
                                NSCumLoi.SoLuongGiam += productModel.Quantity;
                                theodoingay.CommandTypeId = (int)eCommandRecive.ErrorReduce;
                                NangSuat.SanLuongLoiGiam += productModel.Quantity;
                            }
                            theodoingay.ErrorId = productModel.ErrorId;
                            db.TheoDoiNgays.Add(theodoingay);
                            if (exists)
                                db.NangSuat_CumLoi.Add(NSCumLoi);

                            #endregion
                            break;
                    }
                    NangSuat.TimeLastChange = timeNow;
                    NangSuat.IsChange = 1;

                    if (NS_Cum.Id == 0)
                        db.NangSuat_Cum.Add(NS_Cum);
                    if (NangSuat.Id == 0)
                        db.NangXuats.Add(NangSuat);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static NangXuat Create_NangSuat(string date, int Sttchuyen_Sp, double NS_LaoDong, int LDChuyen, double NS_SXuat)
        {
            var NangSuat = new NangXuat();
            NangSuat.Id = 0;
            NangSuat.Ngay = date;
            NangSuat.STTCHuyen_SanPham = Sttchuyen_Sp;
            NangSuat.DinhMucNgay = (float)Math.Round((NS_LaoDong * LDChuyen), 1);
            NangSuat.NhipDoSanXuat = (float)Math.Round((NS_SXuat / LDChuyen), 1);
            NangSuat.TimeLastChange = DateTime.Now.TimeOfDay;
            NangSuat.IsEndDate = false;
            NangSuat.CreatedDate = DateTime.Now;

            //NangSuat.ThucHienNgay = 0;
            //NangSuat.ThucHienNgayGiam = 0;

            //NangSuat.NhipDoThucTeBTPThoatChuyen = 0;
            //NangSuat.NhipDoThucTe = 0;

            //NangSuat.IsChange = 0;
            //NangSuat.IsChangeBTP = 0;
            //NangSuat.IsBTP = 0;

            //NangSuat.BTPLoi = 0;
            //NangSuat.BTPGiam = 0;
            //NangSuat.BTPTang = 0;
            //NangSuat.BTPThoatChuyenNgay = 0;
            //NangSuat.BTPThoatChuyenNgayGiam = "0";

            //NangSuat.SanLuongLoi = 0;
            //NangSuat.SanLuongLoiGiam = "0";           

            return NangSuat;
        }

        private static NangSuat_Cum Create_NS_Cum(string date, int STTchuyen_Sp, int IdCum)
        {
            var NS_Cum = new NangSuat_Cum();
            NS_Cum.Id = 0;
            NS_Cum.Ngay = date;
            NS_Cum.SanLuongKCSTang = 0;
            NS_Cum.STTChuyen_SanPham = STTchuyen_Sp;
            NS_Cum.IdCum = IdCum;
            return NS_Cum;
        }

        private TimeSpan TimeIsWork(int MaChuyen, QLNSEntities db, DateTime date)
        {
            TimeSpan timeWork = new TimeSpan();
            try
            {
                var now = date.Day + "/" + date.Month + "/" + date.Year;
                var TGTNDTT = db.ThoiGianTinhNhipDoTTs.FirstOrDefault(x => x.MaChuyen == MaChuyen && x.Ngay == now);
                var shiftsOfLine = db.Shifts.Where(x => x.MaChuyen == MaChuyen);

                TimeSpan timeStartTT = TGTNDTT != null ? TimeSpan.Parse(TGTNDTT.ThoiGianBatDau.ToString()) : TimeSpan.Parse("00:00:00");

                timeWork = TimeSpan.Parse("00:00:00");
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                if (shiftsOfLine != null && shiftsOfLine.Count() > 0)
                {
                    foreach (var shift in shiftsOfLine)
                    {
                        if (timeNow > shift.TimeStart)
                        {
                            if (timeNow < shift.TimeEnd)
                                timeWork += (timeNow - shift.TimeStart);
                            else
                                timeNow += (shift.TimeEnd - shift.TimeStart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return timeWork;
        }

        public DataForLCDModel GetLCDInfo_New(int tableTypeId, int lineId)
        {
            try
            {
                var configs = db.ShowLCD_Config;
                var data = new DataForLCDModel();
                data.LayoutPanelConfig.AddRange(db.ShowLCD_TableLayoutPanel.Where(c => c.TableType == tableTypeId && c.IsShow));
                data.PageHeight = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.WebPageHeight)).Value;
                if (tableTypeId == 3)
                {
                    data.paging = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.LCDTongHop_Paging)).Value;
                    data.TimeToChangeRow = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.Interval_VerticalAutoScroll_Tick)).Value;
                }
                else
                {
                    data.paging = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.SoDongHienThiLCDNS_New)).Value;
                    data.TimeToChangeRow = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.ThoiGianCuonLCDNS_New)).Value;
                }

                data.Panel_Background.AddRange(db.ShowLCD_Panel.Where(x => x.TableType == tableTypeId).ToList());
                data.FontStyle.AddRange(db.ShowLCD_LabelArea.Where(x => x.TableType == tableTypeId).ToList());
                data.BodyTitle.AddRange(db.ShowLCD_LabelForPanelContent.Where(x => x.TableType == tableTypeId && x.IsShow && x.SttNext == 1).OrderBy(x => x.IntRowTBLPanelContent).ToList());

                int hienThiNS = 0, TimesGetNS = 1, KhoangCachGetNSOnDay = 1, rowHeight = 0;
                string NSG_Formula = "";
                var LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.TypeShowProductivitiesPerHour);
                int.TryParse(LCDcf != null ? LCDcf.Value : "0", out hienThiNS);

                LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.TimesGetNSInDay);
                int.TryParse(LCDcf != null ? LCDcf.Value : "1", out TimesGetNS);

                LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.KhoangCachLayNangSuat);
                int.TryParse(LCDcf != null ? LCDcf.Value : "1", out KhoangCachGetNSOnDay);

                switch (tableTypeId)
                {
                    case 10: LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.ChieuCaoDong_LCDNS_New); break;
                    case 4: LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.ChieuCaoDong_LCDKanBan); break;
                }
                int.TryParse(LCDcf != null ? LCDcf.Value : "1", out rowHeight);

                LCDcf = bllLCDConfig.GetShowLCDConfigByName(eConfigName.NSG_Formula);
                NSG_Formula = LCDcf != null ? LCDcf.Value : "";

                data.ShowNSType = hienThiNS;
                data.TimesGetNS = TimesGetNS;
                data.KhoangCachGetNSOnDay = KhoangCachGetNSOnDay;
                data.rowHeight = rowHeight;
                data.NSG_Formula = NSG_Formula.Split(',').ToList();

                switch (hienThiNS)
                {
                    case (int)eShowNSType.TH_DM_FollowHour: // TH - ĐM -> theo tung gio
                    case (int)eShowNSType.TH_TC_FollowHour:
                    case (int)eShowNSType.TH_Err_FollowHour:
                    case (int)eShowNSType.PercentTH_FollowHour:
                        data.FooterTitle.AddRange(bllShift.GetListWorkHoursOfLineByLineId(lineId));
                        break;

                    case (int)eShowNSType.TH_DM_FollowConfig: // TH - ĐM -> theo so lan chia
                    case (int)eShowNSType.TH_TC_FollowConfig:
                    case (int)eShowNSType.PercentTH_FollowConfig:
                    case (int)eShowNSType.TH_Err_FollowConfig:
                        data.FooterTitle.AddRange(bllShift.GetListWorkHoursOfLineByLineId(lineId, TimesGetNS));
                        break;
                    case (int)eShowNSType.TH_DM_OnDay:
                    case (int)eShowNSType.TH_TC_OnDay:
                    case (int)eShowNSType.TH_Error_OnDay:
                        data.FooterTitle.Add(new WorkingTimeModel()
                        {
                            TimeStart = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                            TimeEnd = DateTime.Now.TimeOfDay,
                            Name = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay.ToString("HH:mm") + "-" + DateTime.Now.TimeOfDay.ToString("HH:mm"),
                            IntHours = 1,
                        });

                        data.FooterTitle.Add(new WorkingTimeModel()
                        {
                            TimeStart = DateTime.Now.AddHours(-(KhoangCachGetNSOnDay + KhoangCachGetNSOnDay)).TimeOfDay,
                            TimeEnd = DateTime.Now.AddHours(-KhoangCachGetNSOnDay).TimeOfDay,
                            IntHours = 2,
                        });
                        break;
                }
                if (tableTypeId != 3)
                    data.LineName = db.Chuyens.FirstOrDefault(x => x.MaChuyen == lineId).TenChuyen;
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CheckListConfigModel GetCheckListConfig(int tableTypeId)
        {
            try
            {
                var configs = db.ShowLCD_Config;
                var data = new CheckListConfigModel();
                data.PageHeight = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.WebPageHeight)).Value;
                data.LCDConfigs.AddRange(db.LCDConfigs.Where(x => !x.IsDeleted && x.TableType == tableTypeId).ToList());
                if (tableTypeId == 11)
                {
                    // hoan tat 
                    data.paging_Detail = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.PagingLCD_HT)).Value;
                    data.Timer_Detail = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.TimerTick_HT)).Value;
                    data.RowHeight = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.ChieuCaoDong_LCD_HT)).Value;
                }
                else
                {
                    // checklist
                    data.paging_Collection = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.PagingLCDCheckList_TH)).Value;
                    data.paging_Detail = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.PagingLCDCheckList_CT)).Value;
                    data.Timer_Collection = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.TimerTickCheckList_TH)).Value;
                    data.Timer_Detail = configs.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(eConfigName.TimerTickCheckList_CT)).Value;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Information for Completion LCD
        /// </summary>
        /// <returns></returns>
        public CheckListModel Get_LCD_Completion()
        {
            try
            {
                var model = new CheckListModel();
                model.Head.AddRange(db.P_CompletionPhase.Where(x => !x.IsDeleted && x.IsShow).ToList().Select(x => new P_CompletionPhase()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OrderIndex = x.OrderIndex,
                    IsShow = x.IsShow
                }).OrderBy(x => x.OrderIndex).ToList());
                if (model.Head.Count > 0)
                {
                    int tang = 0, giam = 0;
                    var body = db.P_CompletionPhase_Daily.Where(x => !x.IsDeleted && !x.P_AssignCompletion.IsDeleted && !x.P_AssignCompletion.IsFinish && !x.P_CompletionPhase.IsDeleted && x.P_CompletionPhase.IsShow).Select(x => new CompletionDailyModel()
                    {
                        Id = x.Id,
                        AssignId = x.AssignId,
                        CommandTypeId = x.CommandTypeId,
                        CommoId = x.P_AssignCompletion.CommoId,
                        CommoName = x.P_AssignCompletion.SanPham.TenSanPham,
                        CompletionPhaseId = x.CompletionPhaseId,
                        Date = x.Date,
                        PhaseName = x.P_CompletionPhase.Name,
                        Quantity = x.Quantity,
                        CreatedDate = x.CreatedDate,
                        OrderIndex = x.P_AssignCompletion.OrderIndex
                    }).ToList();
                    if (body.Count > 0)
                    {
                        var ab = body.OrderBy(x => x.OrderIndex).Select(x => x.CommoId).Distinct().ToList();
                        foreach (var commoId in ab)
                        {
                            var bodyItems = new BodyItem();
                            foreach (var phaseId in body.Select(x => x.CompletionPhaseId).Distinct())
                            {
                                var obj = body.FirstOrDefault(x => x.CommoId == commoId && phaseId == x.CompletionPhaseId);
                                if (obj != null)
                                {
                                    tang = body.Where(x => x.CommoId == commoId && x.CompletionPhaseId == phaseId && x.CommandTypeId == (int)eCommandRecive.ProductIncrease).Sum(x => x.Quantity);
                                    giam = body.Where(x => x.CommoId == commoId && x.CompletionPhaseId == phaseId && x.CommandTypeId == (int)eCommandRecive.ProductReduce).Sum(x => x.Quantity);
                                    obj.Quantity = tang - giam;
                                    if (obj.Quantity > 0)
                                        bodyItems.Items.Add(obj);
                                }
                            }

                            model.Body.Add(bodyItems);
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            { }
            return null;
        }















    }
}

