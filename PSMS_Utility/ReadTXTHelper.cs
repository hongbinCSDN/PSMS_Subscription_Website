using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Utility
{
    public class ReadTXTHelper
    {
        public string OpenTXT(string filePath)
        {
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            string line;
            StringBuilder result = new StringBuilder();
            while ((line = sr.ReadLine()) != null)
            {
                result.Append(line);
            }
            return result.ToString();
        }
    }
}
