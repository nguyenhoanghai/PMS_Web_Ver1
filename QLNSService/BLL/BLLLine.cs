using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLLine
    {
        private QLNSEntities db;
        public BLLLine()
        {
            db = new QLNSEntities();
        }

        public List<ModelSelectItem> GetLines(List<int> LineIds)
        {
            try
            {
                if (LineIds != null)
                {
                    var lines = db.Chuyens.Where(x => !x.IsDeleted && LineIds.Contains(x.MaChuyen)).Select(x => new ModelSelectItem() { Value = x.MaChuyen, Name = x.TenChuyen, Data = 0 }).ToList();
                    if (lines != null && lines.Count  > 0)
                    { 
                        var clusters = db.Cums.Where(x => !x.IsDeleted && LineIds.Contains(x.IdChuyen) && x.IsEndOfLine);
                        if (clusters != null && clusters.Count() > 0)
                        {
                            foreach (var line in lines)
                            {
                                var lastCluster = clusters.FirstOrDefault(x => x.IdChuyen == line.Value);
                                line.Data = lastCluster!= null ? lastCluster.Id : 0;
                            }
                        }
                    }
                    return lines ;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}