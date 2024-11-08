 using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLHistoryPressedKeypad
    {
        static Object key = new object();
        private static volatile BLLHistoryPressedKeypad _Instance;  //volatile =>  tranh dung thread
        public static BLLHistoryPressedKeypad Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (key)
                    {
                        _Instance = new BLLHistoryPressedKeypad();
                        
                    }
                }
                return _Instance;
            }
        }

        private BLLHistoryPressedKeypad() { }

        public P_HistoryPressedKeypad Get(int lineId, string date)
        {
            var db = new QLNSEntities();
            return db.P_HistoryPressedKeypad.FirstOrDefault(x => !x.IsDeleted && x.LineId == lineId && x.Date == date);
        }

        public P_HistoryPressedKeypad[] Get(List<int> lineIds, string date)
        {
            var db = new QLNSEntities();
            return db.P_HistoryPressedKeypad.Where(x => !x.IsDeleted && lineIds.Contains(x.LineId) && x.Date == date).ToArray();
        }


    }
}