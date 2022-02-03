using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        string SITConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        public string errorMsg;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string email = tb_email.Text.ToString().Trim();
                string pwd = tb_password.Text.ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);
                if (!validateInput())
                {
                    lbl_validation.Text = "Invalid email or password!";
                }
                else
                {
                    try
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = pwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);
                            if (isLockoutDisabled(email))
                            {
                                if (userHash.Equals(dbHash))
                                {
                                    Session["OTPEmail"] = email;
                                    setLockoutCounter0(email);
                                    Response.Redirect("OTP.aspx", false);
                                }
                                else
                                {
                                    loginFailCounter(email);
                                    string logString = String.Format("User {0} - failed login attempt", getUserId(email));
                                    Logger.Warn(logString);
                                    lbl_validation.Text = "Email or password is not valid. Please Try Again.";
                                }
                            }
                            else
                            {
                                lbl_validation.Text = "Account has been locked out";
                            }
                        }
                        else
                        {
                            lbl_validation.Text = "Email or password is not valid. Please Try Again.";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }

                }
            }

        }
        protected bool validateInput()
        {
            if (!Regex.IsMatch(tb_email.Text.Trim(), "^\\w+[\\+\\.\\w-]*@([\\w-]+\\.)*\\w+[\\w-]*\\.([a-z]{2,4}|\\d+)$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (!Regex.IsMatch(tb_password.Text.Trim(), "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]){12,}"))
            {
                return false;
            }
            return true;
        }
        protected void setLockoutCounter0(string email)
        {
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string update_sql = "UPDATE Account SET AccessFailedCount=0 WHERE Email=@email";
            try
            {
                connection.Open();
                using (SqlCommand update_cmd = new SqlCommand(update_sql, connection))
                {
                    update_cmd.Parameters.AddWithValue("@email", email);
                    update_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
        }
        protected bool isLockoutDisabled(string email)
        {
            bool past_lockout = false;
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string select_end_sql = "SELECT LockoutEnd FROM Account WHERE Email=@email";
            string select_sql = "SELECT LockoutEnabled FROM Account WHERE Email=@email";
            string update_sql = "UPDATE Account SET LockoutEnabled=@le WHERE Email=@email";
            SqlCommand select_cmd = new SqlCommand(select_sql, connection);
            SqlCommand select_end_cmd = new SqlCommand(select_end_sql, connection);
            SqlCommand update_cmd = new SqlCommand(update_sql, connection);
            select_cmd.Parameters.AddWithValue("@email", email);
            select_end_cmd.Parameters.AddWithValue("@email", email);
            update_cmd.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = select_end_cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["LockoutEnd"] != null)
                        {
                            if (reader["LockoutEnd"] != DBNull.Value)
                            {
                                past_lockout =  DateTime.Now > (DateTime)reader["LockoutEnd"];
                            }
                        }
                    }
                }
                if (past_lockout)
                {
                    using (update_cmd)
                    {
                        update_cmd.Parameters.AddWithValue("@le", false);
                        update_cmd.ExecuteNonQuery();
                    }
                }
                using (SqlDataReader reader = select_cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((bool)reader["LockoutEnabled"])
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return true;
        }
        protected int loginFailCounter(string email)
        {
            int afc = 0;
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string select_sql = "SELECT AccessFailedCount FROM Account WHERE Email=@email";
            string update_sql = "UPDATE Account SET AccessFailedCount=@afc, LockoutEnabled=@te, LockoutEnd=@le WHERE Email=@email";
            SqlCommand select_cmd = new SqlCommand(select_sql, connection);
            select_cmd.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = select_cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["AccessFailedCount"] != null)
                        {
                            if (reader["AccessFailedCount"] != DBNull.Value)
                            {
                                afc = (int)reader["AccessFailedCount"];
                            }
                        }
                    }
                }
                using (SqlCommand update_cmd = new SqlCommand(update_sql, connection))
                {
                    update_cmd.Parameters.AddWithValue("email", email);
                    afc += 1;
                    if (afc == 3)
                    {
                        afc = 0;
                        update_cmd.Parameters.AddWithValue("@te", true);
                        update_cmd.Parameters.AddWithValue("@le", DateTime.Now.AddMinutes(10));
                        
                    }
                    else
                    {
                        update_cmd.Parameters.AddWithValue("@te", false);
                        update_cmd.Parameters.AddWithValue("@le", DBNull.Value);
                    }
                    update_cmd.Parameters.AddWithValue("@afc", afc);
                    update_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return afc;
        }
        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "SELECT Password FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Password"] != null)
                        {
                            if (reader["Password"] != DBNull.Value)
                            {
                                 h = reader["Password"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "Select Salt FROM ACCOUNT WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Salt"] != null)
                        {
                            if (reader["Salt"] != DBNull.Value)
                            {
                                s = reader["Salt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected string getUserId(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "Select Id FROM ACCOUNT WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Id"] != null)
                        {
                            if (reader["Id"] != DBNull.Value)
                            {
                                s = reader["Id"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lc6U7odAAAAANANZFbgSxfoHDs3eb5BzTwP29mX &response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}