using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Success : System.Web.UI.Page
    {
        string SITConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] creditCard = null;
        string email = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    email = (string)Session["Email"];
                    displayUserProfile(email);
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void displayUserProfile(string email)
        {
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "SELECT * FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FirstName"] != DBNull.Value)
                        {
                            lbl_fName.Text = reader["FirstName"].ToString();
                        }
                        if (reader["LastName"] != DBNull.Value)
                        {
                            lbl_lName.Text = reader["LastName"].ToString();
                        }
                        if (reader["CreditCardNum"] != DBNull.Value)
                        {
                            creditCard = Convert.FromBase64String(reader["CreditCardNum"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Birthdate"] != DBNull.Value)
                        {
                            lbl_dob.Text = reader["Birthdate"].ToString();
                        }
                        lbl_email.Text = email;
                        lbl_creditCard.Text = decryptData(creditCard);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.Key = Key;
                cipher.IV = IV;
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using(MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return plainText;
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}