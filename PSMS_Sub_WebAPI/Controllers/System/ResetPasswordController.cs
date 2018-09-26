using PSMS_Sub_BL;
using PSMS_Sub_DM;
using PSMS_Sub_WebAPI.Common;
using PSMS_Sub_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PSMS_Sub_WebAPI.Controllers.System
{
    public class ResetPasswordController : ApiController
    {
        private RESETPASSWORD_BL _RESETPASSWORD_BL;
        protected RESETPASSWORD_BL RESETPASSWORD_BL
        {
            get { return _RESETPASSWORD_BL ?? (_RESETPASSWORD_BL = new RESETPASSWORD_BL()); }
        }
        private PASSWORD_BL _PASSWORD_BL;
        protected PASSWORD_BL BL_PASSWORD
        {
            get { return _PASSWORD_BL ?? (_PASSWORD_BL = new PASSWORD_BL()); }
        }
        private ForgetPasswordSessionVM _SESSIONVM;
        protected ForgetPasswordSessionVM SESSIONVM
        {
            get { return _SESSIONVM ?? (_SESSIONVM = new ForgetPasswordSessionVM()); }
        }

        private CUSTOMER _CUSTOMER;
        protected CUSTOMER _customer
        {
            get { return _CUSTOMER ?? (_CUSTOMER = new CUSTOMER()); }
        }


        [Route("System/Updatepassword")]
        [HttpGet]
        public async Task<IHttpActionResult> pUpdate(string password)
        {
            return await Task.FromResult(GetUpdae(password));
        }
        public IHttpActionResult GetUpdae(string password)
        {
            if (SessionWrapper.ResetUser != null)
            {
                try
                {
                    int count = RESETPASSWORD_BL.ResetPassword(new CUSTOMER()
                    {
                        //CUSTOMER_ID =HttpContext.Current.Session["UserID"].ToString(),
                        CUSTOMER_ID = SessionWrapper.ResetUser.CustomerID,
                        PASSWORD = BL_PASSWORD.MD5Encrypt64(password)
                    });
                    return Ok(count);
                }
                catch (Exception e)
                {
                    return Ok(e);
                }
            }
            else
            {
                return Ok("false");
            }

        }
        //Add by haskin 2018.7.27
        [Route("System/JudgeEmailcode")]
        [HttpGet]
        public async Task<IHttpActionResult> pJudgeEmailcode(string resetpasswordemailcode)
        {
            return await Task.FromResult(GetJudgeEmailValidation(resetpasswordemailcode));
        }
        public IHttpActionResult GetJudgeEmailValidation(string resetpasswordemailcode)
        {
            if (SessionWrapper.ResetUser != null)
            {
                try
                {

                    string emailcode = SessionWrapper.ResetUser.Resetpasswordemailcode;
                    if(DateTime.Now < SessionWrapper.ResetUser.EmailCodeExpireDate)    //Modify by bill 2018.9.19
                    {
                        if (emailcode == resetpasswordemailcode)
                        {
                            SESSIONVM.CustomerID = SessionWrapper.ResetUser.CustomerID;
                            SESSIONVM.Resetpasswordemailcode = SessionWrapper.ResetUser.Resetpasswordemailcode;
                            SESSIONVM.IsEmailcode = true;
                            SessionWrapper.ResetUser = SESSIONVM;
                            return Ok(1); //Emailcode is correct
                        }
                        else
                        {
                           
                            return Ok(0);// The emailcode is error
                        }
                    }
                    else
                    {
                        SessionWrapper.ResetUser.Resetpasswordemailcode = null;   
                        return Ok(-1);
                    }
                    
                }
                catch (Exception e)
                {
                    return Ok(e);
                }

            }
            else
                return Ok(3);//session is null



        }

        [Route("System/SendEmail")]
        [HttpGet]
        public async Task<IHttpActionResult> pSendEmail(string Language_Value)
        {
            return await Task.FromResult(GetSendEmail(Language_Value));
        }
        public IHttpActionResult GetSendEmail(string Language_Value)
        {
            if (SessionWrapper.ResetUser != null)
            {
                try
                {
                    SESSIONVM.IsEmailcode = false;
                    SESSIONVM.CustomerID = SessionWrapper.ResetUser.CustomerID;                   
                    string Emailcode = RESETPASSWORD_BL.SendEmailByPersonalInfo(new CUSTOMER() { CUSTOMER_ID = SessionWrapper.ResetUser.CustomerID }, Language_Value);
                    SESSIONVM.EmailCodeExpireDate = DateTime.Now.AddMinutes(int.Parse(ConfigurationManager.AppSettings["AccountVaildCodeEmailExpireDate"].ToString().Trim()));  //Add by bill 2018.9.19
                    if (Emailcode == "false")
                    {

                        return Ok(1);//发送邮件失败
                    }
                    else
                    {
                        SESSIONVM.Resetpasswordemailcode = Emailcode;
                        SessionWrapper.ResetUser = SESSIONVM;
                        return Ok(0);
                    }


                }
                catch (Exception e)
                {
                    return Ok(e);
                }
            }
            else
            {
                return Ok(3); //session is null
            }
        }


        //Add by Haskin 2018.07.25
        [Route("System/GetEmailByUserId")]
        [HttpGet]
        public async Task<IHttpActionResult> pGetEmailByUserId(string UserId)
        {
            return await Task.FromResult(GetEmailByUserId(UserId));
        }
        public IHttpActionResult GetEmailByUserId(string userId)
        {
            try
            {
                _customer.CUSTOMER_ID = userId;
                CUSTOMER pcustomer = RESETPASSWORD_BL.QueryEmailByCustomerId(_customer);
                if (pcustomer.EMAIL == "Failure")
                {
                    return Ok(0);
                }
                else
                {
                    SESSIONVM.IsEmailcode = false;
                    SESSIONVM.CustomerID = pcustomer.CUSTOMER_ID;
                    SessionWrapper.ResetUser = SESSIONVM;
                    return Ok(1);
                }
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        [Route("System/GetEmailbyEmail")]
        [HttpGet]
        public async Task<IHttpActionResult> pEmail(string email)
        {
            return await Task.FromResult(GetEmail(email));
        }

        public IHttpActionResult GetEmail(string email)
        {
            try
            {
                _customer.EMAIL = email;
                CUSTOMER pcustomer = RESETPASSWORD_BL.QueryEmailByCustomerEmail(_customer);
                if (pcustomer.EMAIL == "Failure")
                {
                    return Ok(0);
                }
                else
                {
                    SESSIONVM.IsEmailcode = false;
                    SESSIONVM.CustomerID = pcustomer.CUSTOMER_ID;
                    SessionWrapper.ResetUser = SESSIONVM;
                    return Ok(1);
                }
            }
            catch (Exception e)
            {
                return Ok(e);
            }


        }
        [Route("System/GetIsEmailcode")]
        [HttpGet]

        public async Task<IHttpActionResult> pIsEmailcode()
        {
            return await Task.FromResult(GetIsEmailcode());
        }
        public IHttpActionResult GetIsEmailcode()
        {
            if (SessionWrapper.ResetUser != null)
            {


                return Ok(SessionWrapper.ResetUser.IsEmailcode);

            }
            else
            {
                return Ok(3); //session is null
            }

        }

    }
}
