using PMS.Business.Enum;
using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLBTP_HCStructure
    {
          static Object key = new object();
        private static volatile BLLBTP_HCStructure _Instance;
        public static BLLBTP_HCStructure Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLBTP_HCStructure();

                return _Instance;
            }
        }

        private BLLBTP_HCStructure() { }

        public List<BTP_HCStructureModel> Gets()
        {
            try
            {
                using (var db = new QLNSEntities())
                {
                    return db.P_Phase.Where(x => !x.IsDeleted && x.Type == (int)ePhaseType.BTP_HC &&x.IsShow).Select(x => new BTP_HCStructureModel()
                      {
                          Id = x.Id,
                          Index = x.Index,
                          Name = x.Name,
                          Note = x.Note
                      }).OrderBy(x=>x.Index).ToList();
                }
            }
            catch (Exception)
            { }
            return null;
        }
    }
}