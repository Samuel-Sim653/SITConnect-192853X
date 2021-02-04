using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string strStatus = Request.QueryString?["status"]?.ToString();
            if (strStatus != null)
            {
                if (strStatus.ToUpper() == "REGISTERED")
                {
                    lb_error.Text = "Your account has been registered, please login to continue.";
                    lb_error.ForeColor = Color.Green;
                }
            }    
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {

            if (tb_pwd.Text == "" || tb_email.Text == "") 
            {
                lb_error.Text = "Please do not leave field empty";
                lb_error.ForeColor = Color.Red;

            } else { 

                string email = HttpUtility.HtmlEncode(tb_email.Text).ToString().Trim();
                string password = HttpUtility.HtmlEncode(tb_pwd.Text).ToString().Trim();

                int status = 0;
                DateTime MaxPasswordAge = default;
                DateTime TimeNow = DateTime.Now;

                SqlConnection con = new SqlConnection(MYDBConnectionString);
                string sql = "SELECT Status, MaxPasswordAge FROM Users WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Status"] != DBNull.Value)
                            {
                                status = Convert.ToInt32(reader["Status"].ToString());
                            }

                            if (reader["MaxPasswordAge"] != DBNull.Value)
                            {
                                MaxPasswordAge = Convert.ToDateTime(reader["MaxPasswordAge"].ToString());
                            }
                        }
                    }
                }//try
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    con.Close();
                }

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                if (dbHash == null) {
                    lb_error.Text = "Email or Password is not valid. Please try again.";
                    lb_error.ForeColor = Color.Red;
                }

                try
                {
                    if (status < 3)
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = password + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbHash))
                            {

                                LoginPass(email);

                                Session["LoggedIn"] = email;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                int diff = DateTime.Compare(TimeNow, MaxPasswordAge);

                                if (diff > 0)
                                {
                                    Response.Redirect("ChangePassword.aspx?status=passwordexpired", false);
                                }
                                else
                                {
                                    Response.Redirect("Homepage.aspx", false);
                                }
                            }
                            else
                            {
                                LoginFail(email);
                                lb_error.Text = "Email or Password is not valid. Please try again.";
                                lb_error.ForeColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        lb_error.Text = "Account is locked.";
                        lb_error.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }


          
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Users WHERE Email= @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
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
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM Users WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
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

        protected void LoginFail(string email)
        {
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Users set Status = Status + 1 WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", email);

            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void LoginPass(string email)
        {
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Users set Status = 0 WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", email);

            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }

        }

        //public bool ValidateCaptcha()
        //{
        //    bool result = true;

        //    string captchaResponse = Request.Form["g-recaptcha-response"];

        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.google.com/recaptcha/api/siteverify?secret=6LcjMD8aAAAAAAnWNoJdIlaZmXOqUNYCtmj9E-dz &response=" + captchaResponse);

        //    try
        //    {
        //        using (WebResponse wResponse = req.GetResponse())
        //        {
        //            using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
        //            {
        //                string jsonResponse = readStream.ReadToEnd();

        //                lb_error.Text = jsonResponse.ToString();

        //                JavaScriptSerializer js = new JavaScriptSerializer();

        //                MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

        //                result = Convert.ToBoolean(jsonObject.success);
        //            }
        //        }

        //        return result;
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}