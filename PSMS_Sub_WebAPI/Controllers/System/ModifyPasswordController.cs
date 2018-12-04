using PSMS_Sub_BL;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using PSMS_Utility;
using PSMS_VM;
using PSMS_VM.ModifyPassword;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    [WebApiTracker, Authenticate]
    public class ModifyPasswordController : ApiController
    {
        private MODIFYPASSWORD_BL _MODIFYPASSWORD_BL;
        protected MODIFYPASSWORD_BL BL_MODIFYPASSWORD
        {
            get
            {
                return _MODIFYPASSWORD_BL ?? (_MODIFYPASSWORD_BL=new MODIFYPASSWORD_BL());
            }
        }

        private PASSWORD_BL _PASSWORD_BL;
        protected PASSWORD_BL BL_PASSWORD
        {
            get
            {
                return _PASSWORD_BL ?? (_PASSWORD_BL=new PASSWORD_BL());
            }
        }

        private EmailVerifyCodeSessionVM _MODIFYPASSWORDSESSION;
        protected EmailVerifyCodeSessionVM MODIFYPASSWORDSESSION
        {
            get
            {
                return _MODIFYPASSWORDSESSION ?? (_MODIFYPASSWORDSESSION = new EmailVerifyCodeSessionVM());
            }
        }

        [Route("System/GetVerifyCodeEmail")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> pSendVerifyCodeEmail()
        {
            return await Task.FromResult(SendVerifyCodeEmail());
        }

        public IHttpActionResult SendVerifyCodeEmail()
        {
            try
            {
                SendEmailHelper sendEmailHelper = new SendEmailHelper();
                string verifyCode = sendEmailHelper.getCode();

                MODIFYPASSWORDSESSION.EmailVerifyCode = verifyCode;
                MODIFYPASSWORDSESSION.ExpiredTime = DateTime.Now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["AccountVaildCodeEmailExpireDate"].ToString().Trim()));
                SessionWrapper.ModifyPasswordUser = MODIFYPASSWORDSESSION;

                sendEmailHelper.SendModifyPasswordVerifyCodeEmail(SessionWrapper.CurrentUser.Email, SessionWrapper.CurrentUser.CustomerID, verifyCode, CacheWrapper.CurrentMultilingual.MultilingualID);
                return Ok(1);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/ModifyPassword")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> pModifyPassword(ModifyPasswordVM passwordVM)
        {
            return await Task.FromResult(ModifyPassword(passwordVM));
        }

        /// <summary>
        /// Modify the Account Password.
        /// </summary>
        /// <param name="passwordVM"></param>
        /// <returns>
        /// 0: Incorrect Old Password;
        /// 1:Modify Password Success;
        /// -1:Modify Password Failer;
        /// -2:The old Password Equal New Password;
        /// -3:The Verification Code Incorrect;
        /// -4:Null Session,Verify Code Expire
        /// </returns>
        public IHttpActionResult ModifyPassword(ModifyPasswordVM passwordVM)
        {
            try
            {
                if (SessionWrapper.ModifyPasswordUser != null)
                {
                    if (DateTime.Now >= SessionWrapper.ModifyPasswordUser.ExpiredTime)
                    {
                        SessionWrapper.ModifyPasswordUser = null;
                        return Ok(-4);
                    }

                    if (SessionWrapper.ModifyPasswordUser.EmailVerifyCode != passwordVM.VerifyCode)
                    {
                        return Ok(-3);
                    }
                    int result = BL_MODIFYPASSWORD.ModifyPassword(SessionWrapper.CurrentUser.CustomerID, BL_PASSWORD.MD5Encrypt64(passwordVM.OldPwd), BL_PASSWORD.MD5Encrypt64(passwordVM.NewPwd));
                    if (result == 1)
                    {
                        SessionWrapper.ModifyPasswordUser = null;
                    }
                    return Ok(result);
                }
                return Ok(-4);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
}
