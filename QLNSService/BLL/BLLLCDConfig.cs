using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLLCDConfig
    {
        private QLNSEntities qlnsEntities;
        public BLLLCDConfig()
        {
            qlnsEntities = new QLNSEntities();
        }
        public List<ShowLCD_Config> GetConfig()
        { 
            try
            {
                var listConfig = qlnsEntities.ShowLCD_Config.ToList();
                return listConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public List<ShowLCD_LabelArea> GetLabelArea(int tableTypeId)
        {
            try
            {
                var listConfig = qlnsEntities.ShowLCD_LabelArea.Where(c=>c.TableType==tableTypeId ).ToList();
                return listConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ShowLCD_LabelForPanelContent> GetLabelForPanelContent(int tableTypeId)
        {
            try
            {
                var listConfig = qlnsEntities.ShowLCD_LabelForPanelContent.Where(c => c.TableType == tableTypeId && c.IsShow && c.SttNext==1).ToList();
                return listConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ShowLCD_Panel> GetPanel(int tableTypeId)
        {
            try
            {
                var listConfig = qlnsEntities.ShowLCD_Panel.Where(c => c.TableType == tableTypeId).ToList();
                return listConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ShowLCD_TableLayoutPanel> GetTableLayoutPanel(int tableTypeId)
        {
            try
            {
                var listConfig = qlnsEntities.ShowLCD_TableLayoutPanel.Where(c => c.TableType == tableTypeId && c.IsShow).ToList();
                return listConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ShowLCD_Config GetShowLCDConfigByName(string configName)
        {
            try
            {
                return qlnsEntities.ShowLCD_Config.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(configName));
            }
            catch (Exception)
            {
            }
            return null;
        }
    }

    public class BLLApplicationConfig
    {
        private static QLNSEntities db;
        static Object key = new object();
        private static volatile BLLApplicationConfig _Instance;  //volatile =>  tranh dung thread
        public static BLLApplicationConfig Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (key)
                    {
                        _Instance = new BLLApplicationConfig();
                       
                    }
                }
                return _Instance;
            }
        }

        private BLLApplicationConfig() { } 

        public string GetConfig(string name, int appId)
        { 
            try
            { 
                db = new QLNSEntities();
               var obj = db.Config_App.FirstOrDefault(x=>x.Config.Name.Trim().ToUpper().Equals(name) && x.AppId == appId);
               return obj != null ? obj.Value : string.Empty;
              }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}