using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;

namespace SITConnect
{
    public partial class OTP : System.Web.UI.Page
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        string SITConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDBConnection"].ConnectionString;
        string senderEmail = System.Configuration.ConfigurationManager.ConnectionStrings["myEmailName"].ConnectionString;
        string senderPassword = System.Configuration.ConfigurationManager.ConnectionStrings["myEmailPassword"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OTPEmail"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btn_send_Click(object sender, EventArgs e)
        {
            string email = (string)Session["OTPEmail"];

            Random generator = new Random();
            String otp = generator.Next(100000, 1000000).ToString();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("qwertya004@gmail.com");

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtp.Host = "smtp.gmail.com";

            mail.To.Add(new MailAddress(email));

            mail.IsBodyHtml = true;
            string st = "Your verification code is: " + otp;

            updateOTP(email, otp);
            mail.Body = st;
            smtp.Send(mail);
            btn_send.Text = "Resend Code";
        }
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            string email = (string)Session["OTPEmail"];
            if (verifyOTP(email, tb_otp.Text.ToString()))
            {
                Session.Clear();
                //Create session for user
                Session["Email"] = email;
                string guid = Guid.NewGuid().ToString();
                //Creating second session for the same user and assigning random GUID
                Session["AuthToken"] = guid;
                //Creating cookie and storing the same value of second session in the cookie
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                string logString = String.Format("User {0} - has logged in successfully", getUserId(email));
                if (isPastMaxAge(email))
                {
                    Logger.Info(logString);
                    Response.Redirect("ChangePassword.aspx", false);
                }
                else
                {
                    Logger.Info(logString);
                    Response.Redirect("Success.aspx", false);
                }
            }
        }

        protected void updateOTP(string email, string OTP)
        {

            string sql = "UPDATE Account SET OTP=@otp, OTPExpiry = @otpe WHERE Email = @email";

            try
            {
                using (SqlConnection connection = new SqlConnection(SITConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@otp", OTP);
                        command.Parameters.AddWithValue("@otpe", DateTime.Now.AddMinutes(1));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected bool verifyOTP(string email, string OTP)
        {
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "Select OTP, OTPExpiry FROM ACCOUNT WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["OTP"] != null)
                        {
                            if (reader["OTP"] != DBNull.Value)
                            {
                                if (reader["OTP"].ToString() == OTP && DateTime.Now < (DateTime)reader["OTPExpiry"])
                                {
                                    return true;
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
            return false;
        }
        protected bool isPastMaxAge(string email)
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
                                if (DateTime.Now.AddMinutes(-5) > (DateTime)reader["PasswordChangedAt"])
                                {
                                    return true;
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
            return false;
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
    }
}