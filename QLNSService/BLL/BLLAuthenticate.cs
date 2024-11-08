using GPRO.Ultilities;
using QLNSService.Data;
using QLNSService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNSService.BLL
{
    public class BLLAuthenticate
    {
        private QLNSEntities qlnsEntities;
        public BLLAuthenticate()
        {
            qlnsEntities = new QLNSEntities();
        }
        public DataResult Login(string userName, string passWord, bool isUseAccount)
        {
            try
            {
                var dataResult = new DataResult();
                var user = qlnsEntities.TaiKhoans.Where(c => c.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && c.Password.Trim().ToUpper().Equals(passWord.Trim().ToUpper())).FirstOrDefault();
                if (user != null)
                {
                    dataResult.Result = "OK";
                    dataResult.Data = new LoginUser() { Name = user.Name, IsKanbanAccount = user.IsKanbanAcc, LineIds_Str = user.ListChuyenId, IsAccountUser = false };
                }
                else
                {
                    if (isUseAccount)
                    {
                        var acDb = new AccountEntities();
                        var passHash = GlobalFunction.EncryptMD5(passWord.Trim());
                        var Accuser = acDb.SUsers.Where(c => c.UserName.Trim().ToUpper().Equals(userName.Trim().ToUpper()) && c.PassWord.Equals(passHash)).FirstOrDefault();
                        if (Accuser != null)
                        {
                            dataResult.Result = "OK";
                            dataResult.Data = new LoginUser() { Id = Accuser.Id, Name = Accuser.FisrtName + Accuser.LastName, IsKanbanAccount = false, LineIds_Str = string.Empty, IsAccountUser = true };
                        }
                        else
                        {
                            dataResult.Result = "ERROR";
                            dataResult.ErrorMessages.Add("Lỗi: Tên hoặc mật khẩu đăng nhập sai.");
                        }
                    }

                }
                return dataResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}