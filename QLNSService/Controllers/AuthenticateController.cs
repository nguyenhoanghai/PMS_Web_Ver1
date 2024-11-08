using QLNSService.BLL;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace QLNSService.Controllers
{
    public class AuthenticateController : ApiController
    {
        [ActionName("Login")]
        public DataResult Login (ModelLogin modelLogin)
        {
            var result = new DataResult();
            try
            {
                bool IsUseAccount = false;
                bool.TryParse(ConfigurationManager.AppSettings["IsUseAccount"], out IsUseAccount);
                var bllAuthenticate = new BLLAuthenticate();
                result = bllAuthenticate.Login(modelLogin.UserName, modelLogin.PassWord, IsUseAccount);                
            }
            catch (Exception ex)
            {
                result.Result = "ERROR";
                result.ErrorMessages.Add("Lỗi: "+ex.Message);
                throw ex;
            }
            return result;
        }
       
        //[ActionName("Get")]
        //public DataResult Get()
        //{
        //    BLLAuthenticate bllAuthenticate = new BLLAuthenticate();
        //    var result = bllAuthenticate.Login("myhanh2", "12345");
        //    return result;
        //}

    }
}
