using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string SITConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        public string errorMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void btn_changePwd_Click(object sender, EventArgs e)
        {
            string email = (string)Session["Email"];
            string pwd = tb_existingPwd.Text.ToString().Trim();
            string new_pwd = tb_newPwd.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            if (passwordMinAgeValid(email))
            {
                if (Regex.IsMatch(tb_newPwd.Text.Trim(), "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]){12,}"))
                {
                    if (tb_newPwd.Text == tb_cfmNewPwd.Text)
                    {
                        try
                        {
                            if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                            {
                                string pwdWithSalt = pwd + dbSalt;
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                string userHash = Convert.ToBase64String(hashWithSalt);
                                if (userHash.Equals(dbHash))
                                {
                                    string newPwdWithSalt = new_pwd + dbSalt;
                                    byte[] newHashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newPwdWithSalt));
                                    string newUserHash = Convert.ToBase64String(newHashWithSalt);
                                    if (!newUserHash.Equals(userHash))
                                    {
                                        if (passwordReuseValid(email, newUserHash))
                                        {
                                            changePassword(email, newUserHash);
                                            Response.Redirect("Success.aspx", false);
                                        }
                                        else
                                        {
                                            lbl_validation.Text = "New password is the same as one of the previous 2 passwords!";
                                        }
                                    }
                                    else
                                    {
                                        lbl_validation.Text = "New password is the same as old password!";
                                    }
                                }
                                else
                                {
                                    lbl_validation.Text = "Existing password is incorrect!";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                    }
                    else
                    {
                        lbl_validation.Text = "Passwords do not match!";
                    }
                }
                else
                {
                    lbl_validation.Text = "New password is too weak!";
                }
            }
            else
            {
                lbl_validation.Text = "Changed passwords too fast!";
            }
        }
        protected void changePassword(string email, string password)
        {
            
            string sql = "UPDATE Account SET Password2 = Password1, Password1=Password, Password = @password, PasswordChangedAt=@pca WHERE Email = @email";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(SITConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@pca", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected bool passwordMinAgeValid(string email)
        {

            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "SELECT PasswordChangedAt FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordChangedAt"] != null)
                        {
                            if (reader["PasswordChangedAt"] != DBNull.Value)
                            {
                                if (DateTime.Now.AddMinutes(-1) < (DateTime)reader["PasswordChangedAt"])
                                {
                                    return false;
                                }
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
            return true;
        }
        protected bool passwordReuseValid(string email, string password)
        {
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "SELECT Password1, Password2 FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Password1"] != null)
                        {
                            if (reader["Password1"] != DBNull.Value)
                            {
                                if ((string)reader["Password1"] == password)
                                {
                                    return false;
                                }
                            }
                        }
                        if (reader["Password2"] != null)
                        {
                            if (reader["Password2"] != DBNull.Value)
                            {
                                if ((string)reader["Password2"] == password)
                                {
                                    return false;
                                }
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
            return true;

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
                                {
                                    h = reader["Password"].ToString();
                                }
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
                                {
                                    s = reader["Salt"].ToString();
                                }
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
    }
}