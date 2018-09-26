using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_VM.ModifyPassword
{
    public class ModifyPasswordVM
    {
        public string VerifyCode { get; set; }
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }
}
