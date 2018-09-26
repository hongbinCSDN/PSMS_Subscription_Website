using PSMS_Sub_DA;
using PSMS_Sub_DM;
using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
    public class CUSTOMER_BL
    {
        private CUSTOMER_DA _CUSTOMER_DA;

        protected CUSTOMER_DA DA
        {
            get { return _CUSTOMER_DA ?? (_CUSTOMER_DA = new CUSTOMER_DA()); }
        }

        private ResultVM _ResultVM;

        protected ResultVM resultVM
        {
            get { return _ResultVM ?? (_ResultVM = new ResultVM()); }
        }

        private ReadCSVHelper _ReadCSV;
        protected ReadCSVHelper ToReadCSV
        {
            get { return _ReadCSV ?? (_ReadCSV = new ReadCSVHelper()); }
        }



        /// <summary>
        /// Login Funtion
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ResultVM vmQueryByLogin(CUSTOMER customer)
        {
            return new ResultVM { Message = DA.GetQueryByLogin(customer) };
        }


        //Modify by Haskin 20180806   Add the params 'category'
        /// <summary>
        /// Register Account
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ResultVM vmQueryByRegister(CUSTOMER customer,string category)
        {
            return new ResultVM { Data = DA.AddQueryByRegister(customer,category) };
        }
        

        /// <summary>
        /// VaildAccount
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ResultVM vmQueryByVaildAccount(CUSTOMER customer, string mulcategory)
        {
            if (DA.GetQueryByVaildAccount(customer, mulcategory) == true)
            {
                resultVM.Affected = 1;
                resultVM.Message = ToReadCSV.OpenCSVWithIdentifying(ConfigurationManager.AppSettings["LoadSystemMessagePath"].ToString().Trim(), mulcategory, "1");
                return resultVM;
            }
            else
            {
                resultVM.Affected = 0;
                resultVM.Message = ToReadCSV.OpenCSVWithIdentifying(ConfigurationManager.AppSettings["LoadSystemMessagePath"].ToString().Trim(), mulcategory, "18");
                return resultVM;
            }

            //return new ResultVM { Message = DA.GetQueryByVaildAccount(customer) };
        }

        /// <summary>
        /// Get customer personal informaiton
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ResultVM vmQueryByGetPersonalInfo(CUSTOMER customer)
        {
            return new ResultVM { Data = DA.GetQueryByGetPersonalInfo(customer) };
        }


        public DataSet vmQueryByDataSetGetPersionInfo(CUSTOMER customer)
        {
            return DA.GetQueryByGetPersonalInfo(customer);
        }

        
        /// <summary>
        /// Update The Customer's Information
        /// </summary>
        /// <param name="customer">Update Customer</param>
        /// <returns></returns>
        public ResultVM vmQueryByUpdatePersonalInfo(CUSTOMER customer)
        {
            if (string.IsNullOrEmpty(customer.CUSTOMER_NAME) || string.IsNullOrEmpty(customer.EMAIL) || string.IsNullOrEmpty(customer.COMPANY) || string.IsNullOrEmpty(customer.PHONE))
            {
                return new ResultVM { Data = 0  };
            }
            customer.CNAME = customer.CNAME ?? "";
            customer.FIXED_TELEPHONE = customer.FIXED_TELEPHONE ?? "";
            customer.COMPANY_ADDRESS = customer.COMPANY_ADDRESS ?? "";
            return new ResultVM { Data = DA.UpdateQueryByUpdatePersonalInfo(customer) };
        }

        //Modify end


        // Add by bill 2018.8.21
        /// <summary>
        /// Write Login Log
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public void vmQueryByLoginLog(string username,string access_ip)
        {
            DA.WriteLoginLog(username,access_ip);
        }
        //End
    }
}
