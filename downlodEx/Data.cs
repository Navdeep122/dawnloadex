using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace downlodEx
{


    public class Data
    {
        private string _connectionString;

        public Data()
        {

            _connectionString = ConfigurationManager.AppSettings["SmartAPDatabase"];
        }


        public string fatchLogincrd(string userName, string pass)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("spLogin", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Pass", pass);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string userTypeFromDb = ds.Tables[0].Rows[0]["UserType"].ToString();
                        HttpContext.Current.Session["UserType"] = userTypeFromDb;
                        string UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                        HttpContext.Current.Session["UserName"] = UserName;
                        string Password = ds.Tables[0].Rows[0]["Password"].ToString();
                        HttpContext.Current.Session["Password"] = Password;
                      
                    }

                    return "okk";
                }

                catch (Exception ex)
                {

                    return null;
                }
            }
        }

    }
}