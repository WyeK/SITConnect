using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        string SITConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnectDBConnection"].ConnectionString;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btn_register_Click(object sender, EventArgs e)
        {
            bool registration_valid = true;
            if (ValidateCaptcha())
            {
                if (!Regex.IsMatch(tb_fName.Text.Trim(), "^[a-z ,.-]{2,50}$", RegexOptions.IgnoreCase))
                {
                    registration_valid = false;
                    lbl_fName_check.Text = "Invalid First Name!";
                    
                }
                else
                {
                    lbl_fName_check.Text = "";
                }
                if (!Regex.IsMatch(tb_lName.Text.Trim(), "^[a-z ,.-]{2,50}$", RegexOptions.IgnoreCase))
                {
                    registration_valid = false;
                    lbl_lName_check.Text = "Invalid Last Name!";
                }
                else
                {
                    lbl_lName_check.Text = "";
                }
                if (!Regex.IsMatch(tb_creditcard.Text.Trim(), "^[0-9]{16}$"))
                {
                    registration_valid = false;
                    lbl_creditcard_check.Text = "Invalid Credit Card Number!";
                }
                else
                {
                    lbl_creditcard_check.Text = "";
                }
                if (!Regex.IsMatch(tb_email.Text.Trim(), "^\\w+[\\+\\.\\w-]*@([\\w-]+\\.)*\\w+[\\w-]*\\.([a-z]{2,4}|\\d+)$", RegexOptions.IgnoreCase))
                {
                    registration_valid = false;
                    lbl_email_check.Text = "Invalid Email!";
                }
                else
                {
                    if (!isUniqueEmail(tb_email.Text.Trim()))
                    {
                        registration_valid = false;
                        lbl_email_check.Text = "Email already exists!";
                    }
                    else
                    {
                        lbl_email_check.Text = "";
                    }
                }
                if (!Regex.IsMatch(tb_password.Text.Trim(), "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]){12,}"))
                {
                    registration_valid = false;
                    lbl_pwd_check.Text = "Invalid Password!";
                }
                else
                {
                    lbl_pwd_check.Text = "";
                }
                if (tb_password.Text != tb_cfmPassword.Text)
                {
                    registration_valid = false;
                    lbl_cfmPwd_check.Text = "Passwords do not match!";
                }
                if (registration_valid)
                {
                    RijndaelManaged cipher = new RijndaelManaged();
                    cipher.GenerateKey();
                    Key = cipher.Key;
                    IV = cipher.IV;
                    createAccount();
                    Response.Redirect("Login.aspx");
                }
            }
        }
        protected byte[] hashAndSaltPwd(string pwd)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] saltPassHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            return saltPassHash;
        }
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return cipherText;
        }
        protected bool isUniqueEmail(string email)
        {
            SqlConnection connection = new SqlConnection(SITConnectionString);
            string sql = "SELECT COUNT(*) FROM Account WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                if ((int)command.ExecuteScalar() > 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return true;
        }
        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(SITConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName, @CreditCardNum, @IV, @Key, @Email, @Password, @Salt, @Birthdate, @Photo, @LockoutEnabled, @LockoutEnd, @AccessFailedCount, @Password1, @Password2, @PasswordChangedAt)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_fName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lName.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardNum", Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", Convert.ToBase64String(hashAndSaltPwd(tb_password.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Salt", salt);
                            cmd.Parameters.AddWithValue("@Birthdate", Convert.ToDateTime(tb_dob.Text.Trim()));
                            SqlParameter imageParameter = new SqlParameter("@Photo", SqlDbType.Image);
                            imageParameter.Value = DBNull.Value;
                            cmd.Parameters.Add(imageParameter);
                            cmd.Parameters.AddWithValue("@LockoutEnabled", false);
                            cmd.Parameters.AddWithValue("@LockoutEnd", DBNull.Value);
                            cmd.Parameters.AddWithValue("@AccessFailedCount", 0);
                            cmd.Parameters.AddWithValue("@Password1", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Password2", DBNull.Value);
                            cmd.Parameters.AddWithValue("@PasswordChangedAt", DateTime.Now);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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