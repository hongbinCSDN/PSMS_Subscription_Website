using PSMS_Sub_DA;
using PSMS_Sub_DM;
using PSMS_Utility;
using PSMS_VM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Sub_BL
{
   public  class RESETPASSWORD_BL
    {
        private RESETPASSWORD_DA _RESETPASSWORD_DA;

        protected RESETPASSWORD_DA DA
        {
            get { return _RESETPASSWORD_DA ?? (_RESETPASSWORD_DA = new RESETPASSWORD_DA()); }
        }

        //private ResultVM _ResultVM;

        //protected ResultVM resultVM
        //{
        //    get { return _ResultVM ?? (_ResultVM = new ResultVM()); }
        //}

        //private ReadCSVHelper _ReadCSV;
        //protected ReadCSVHelper ToReadCSV
        //{
        //    get { return _ReadCSV ?? (_ReadCSV = new ReadCSVHelper()); }
        //}





        //Add by Haskin 2018.07.24
        public string SendEmailByPersonalInfo(CUSTOMER customer, string Language_Value)
        {

            return this.DA.SendEmailByPersonalInfo(customer, Language_Value);
        }
        public int ResetPassword(CUSTOMER customer)
        {
            return this.DA.ResetPassword(customer);
        }
        //
        //add by Haskin 2018. 07. 25
        public CUSTOMER QueryEmailByCustomerId(CUSTOMER customer)
        {
            return this.DA.QueryEmailByCustomerID(customer);
        }
        //Add by Haskin 2018.07.26
        public CUSTOMER QueryEmailByCustomerEmail(CUSTOMER customer)
        {
            return DA.QueryEmailByCustomerEmail(customer);
        }
    }
}
