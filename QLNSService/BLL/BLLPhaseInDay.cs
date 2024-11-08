using QLNSService.Data;
using QLNSService.Models;
using QLNSService.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLPhaseInDay
    {
        #region constructor
        static object key = new object();
        private static volatile BLLPhaseInDay _Instance;
        public static BLLPhaseInDay Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLPhaseInDay();

                return _Instance;
            }
        }
        private BLLPhaseInDay() { }
        #endregion

        public List<ModelSelectItem> GetPhases()
        {
            using (var db = new QLNSEntities())
            {
                return db.P_CompletionPhase.Select(x => new ModelSelectItem() { Value = x.Id, Name = x.Name }).ToList();
            }
        }

        public bool InsertPhaseQuantities(P_PhaseDailyLog model, int csp)
        {
            try
            {
                using (var db = new QLNSEntities())
                {
                    var phaseLog = db.P_Assign_Phase_QuantityLog.FirstOrDefault(x => x.AssignId == csp && x.PhaseId == model.PhaseId);
                    if (phaseLog != null)
                    {
                        switch (model.CommandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease: phaseLog.Quantity += model.Quantity; break;
                            case (int)eCommandRecive.ProductReduce: phaseLog.Quantity -= model.Quantity; break;
                        }
                    }
                    else
                    {
                        var newObj = new P_Assign_Phase_QuantityLog()
                        {
                            Quantity = 0,
                            PhaseId = model.PhaseId,
                            AssignId = csp,
                            CreatedDate = DateTime.Now
                        };
                        switch (model.CommandTypeId)
                        {
                            case (int)eCommandRecive.ProductIncrease: newObj.Quantity += model.Quantity; break;
                            case (int)eCommandRecive.ProductReduce: newObj.Quantity -= model.Quantity; break;
                        }
                        db.P_Assign_Phase_QuantityLog.Add(newObj);
                    }
                    model.CreatedDate = DateTime.Now;
                    db.P_PhaseDailyLog.Add(model);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public ModelSelectItem GetLKPhase(int phaseId, int cspId)
        {
            var rs = new ModelSelectItem() { Data = 0, Value = 0 };
            using (var db = new QLNSEntities())
            {
                var phaseLog = db.P_Assign_Phase_QuantityLog.FirstOrDefault(x => x.AssignId == cspId && x.PhaseId == phaseId);
                if (phaseLog != null)
                {
                    rs.Data = phaseLog.Quantity;
                    rs.Value = phaseLog.Chuyen_SanPham.LuyKeTH;
                }
                else
                {
                    var csp = db.Chuyen_SanPham.FirstOrDefault(x => x.STT == cspId);
                    if (csp != null)
                        rs.Value = csp.LuyKeTH;
                }
            }
            return rs;
        }

        public List<AddPhaseQuantitiesModel> GetPhaseDayInfo(int phaseId, string date)
        {
            var rs = new List<AddPhaseQuantitiesModel>();
            using (var db = new QLNSEntities())
            {
                rs.AddRange(db.P_PhaseDailyLog.Where(x => x.NangXuat.Ngay == date && x.PhaseId == phaseId).OrderByDescending(x => x.CreatedDate).Select(x => new AddPhaseQuantitiesModel()
                   {
                       Date = x.CreatedDate,
                       Quantity = x.Quantity,
                       CommandTypeId = x.CommandTypeId,
                       strCommandType = (x.CommandTypeId == (int)eCommandRecive.ProductIncrease ? "Tăng" : "Giảm")
                   }));
                for (int i = 0; i < rs.Count; i++)
                    rs[i].strDate = rs[i].Date.ToString("d/M/yyyy HH:mm");

            }
            return rs;
        }
    }
}