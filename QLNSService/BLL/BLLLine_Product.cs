using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLLine_Product
    {
        private QLNSEntities db;
        public BLLLine_Product()
        {
            db = new QLNSEntities();
        }

        public List<ModelSelectItem> GetProductInfos(int LineId)
        {
            try
            {
                if (LineId != 0)
                    return db.Chuyen_SanPham.Where(x => !x.IsDelete  && !x.SanPham.IsDelete && x.Thang == DateTime.Now.Month && x.Nam == DateTime.Now.Year && x.MaChuyen == LineId && !x.IsFinish && !x.IsFinishNow ).OrderBy(x => x.STTThucHien)
                        .Select(x => new ModelSelectItem()
                        {
                            Value = x.MaSanPham,
                            Name = x.SanPham.TenSanPham
                        }).ToList();
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ModelSelectItem> GetNSNgayCuaChuyen(int LineId)
        {
            try
            {
                var now = DateTime.Now.ToString("d/M/yyyy");
                if (LineId != 0)
                    return db.Chuyen_SanPham.Where(x => !x.IsDelete &&!x.IsFinish && !x.IsFinishNow && 
                        !x.SanPham.IsDelete && x.MaChuyen == LineId  )
                        .OrderBy(x => x.STTThucHien)
                        .Select(x => new ModelSelectItem()
                        {
                            Value = x.STT,
                            Data = x.STT,
                            Name = x.SanPham.TenSanPham
                        }).ToList();
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}