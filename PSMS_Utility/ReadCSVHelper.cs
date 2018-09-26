using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Utility
{
    public class ReadCSVHelper
    {
        /// <summary>
        /// Read the data from CSV and return the table
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataTable OpenCSV(string filePath, string category)
        {
           System.Text.Encoding encoding = GetType(filePath);  //Encoding.ASCII;
            DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    string AreaCode = "";
                    if(category == "1")
                    {
                        AreaCode = "1028";
                    }
                    else if(category == "2")
                    {
                        AreaCode = "1033";
                    }
                    else if(category == "3")
                    {
                        AreaCode = "1030";
                    }
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        //Modify by Haskin 2018.8.3
                    
                        if (aryLine[j].Contains("\\"))         
                        {
                           aryLine[j] = aryLine[j].Replace("\\", ",");

                        }
                        //End
                        dr[j] = aryLine[j];
                    }
                    if (dr.ItemArray[0].ToString() == AreaCode)
                    {
                        dt.Rows.Add(dr);
                    }                  
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        public string OpenCSVWithIdentifying(string filePath, string category, string Identifying)
        {


            System.Text.Encoding encoding = GetType(filePath);  //Encoding.ASCII;
            DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //Records one row per read
            string strLine = "";
            //Records the contents of each field in each row record
            string[] aryLine = null;
            string[] tableHead = null;
            //Mark the number of columns
            int columnCount = 0;
            //Indicates whether the first line is read
            bool IsFirst = true;
            //Read the data in CSV line by line
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //Create the columns
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    string AreaCode = "";
                    if (category == "1")
                    {
                        AreaCode = "1028";
                    }
                    else if (category == "2")
                    {
                        AreaCode = "1033";
                    }
                    else if (category == "3")
                    {
                        AreaCode = "1030";
                    }
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {

                        dr[j] = aryLine[j];
                    }
                    if (dr.ItemArray[0].ToString() == AreaCode && dr.ItemArray[1].ToString() == Identifying)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }
            sr.Close();
            fs.Close();
            string message = dt.Rows[0].ItemArray[2].ToString();
            //return dt;
            return message;
        }

        /// <summary>
        /// Given the path of the file, read the binary data of the file, and determine the encoding type of the file
        /// </summary>
        /// <param name="FILE_NAME"></param>
        /// <returns>The encoding type of the file</returns>
        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xFF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// <summary>
        /// Determines if it is a UTF8 format without BOM
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  
            byte curByte; 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}
