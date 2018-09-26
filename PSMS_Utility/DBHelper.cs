using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSMS_Utility
{
    public class DBHelper
    {
        public static SqlConnection GetConnection()
        {
            string setting = ConfigurationManager.AppSettings["PSMSWebConnection"];
            return new SqlConnection(setting);
        }

        /// <summary>
        /// Returns the number of affected rows
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="sParameters"></param>
        /// <returns></returns>
        public static int Exec(string sSql, SqlParameter[] sParameters)
        {
            using (SqlConnection connection = GetConnection())  
                {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sSql, connection);
                cmd.Parameters.AddRange(sParameters);

                return cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Returns the number of affected rows
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="sSql"></param>
        /// <param name="sParameters"></param>
        /// <returns></returns>
        public static int Exec(SqlConnection Connection, string sSql, SqlParameter[] sParameters)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sSql, connection);
                cmd.Parameters.AddRange(sParameters);

                return cmd.ExecuteNonQuery();
            }
        }

        // Modify / Add by Chester 2018.09.14

        /// <summary>
        /// Returns the number of affected rows,can exec the stored procedure
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="sParameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int Exec(string sSql, SqlParameter[] sParameters, CommandType type)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sSql, connection)
                {
                    CommandType = type
                };
                cmd.Parameters.AddRange(sParameters);

                return cmd.ExecuteNonQuery();
            }
        }

        // Modify End

        /// <summary>
        /// Return Dataset
        /// </summary>
        /// <param name="sSql"></param>
        /// <param name="sParameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sSql, SqlParameter[] sParameters = null, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.CommandType = type;
                    if (sParameters != null) { cmd.Parameters.AddRange(sParameters); }
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return Dataset
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="sSql"></param>
        /// <param name="sParameters"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(SqlConnection Connection, string sSql, SqlParameter[] sParameters = null, CommandType type = CommandType.Text)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.CommandType = type;
                    if (sParameters != null) { cmd.Parameters.AddRange(sParameters); }
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }
    }
}
