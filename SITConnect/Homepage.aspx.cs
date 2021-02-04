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
using System.IO;

namespace SITConnect
{
    public partial class Homepage : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] creditcard = null;
        string email = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    email = (string)Session["LoggedIn"];
                    lbl_Message.Text = "You are logged in!";
                    lbl_Message.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;
                    btnChangepass.Visible = true;

                    displayUserProfile(email);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

            string strStatus = Request.QueryString?["status"]?.ToString();
            if (strStatus != null)
            {
                if (strStatus.ToUpper() == "PASSWORDCHANGED")
                {
                    lbl_Message.Text = "Your password has been changed.";
                    lbl_Message.ForeColor = Color.Green;
                }
            }
        }

        protected void LogoutMe(object sender, EventArgs e)
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

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
                
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void displayUserProfile(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM Users WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) { 

                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["Email"].ToString();
                        }

                        if (reader["Fname"] != DBNull.Value)
                        {
                            lbl_fname.Text = reader["Fname"].ToString();
                        }

                        if (reader["Lname"] != DBNull.Value)
                        {
                            lbl_lname.Text = reader["Lname"].ToString();
                        }

                        if (reader["CreditCard"] != DBNull.Value)
                        {
                            creditcard = Convert.FromBase64String(reader["CreditCard"].ToString());
                        }

                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }

                        if (reader["Keys"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Keys"].ToString());
                        }

                    }
                    
                    lbl_credit.Text = decryptData(creditcard);
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void ChangepassMe(object sender, EventArgs e)
        {
            email = (string)Session["LoggedIn"];
            DateTime TimeNow = DateTime.Now;
            DateTime MinPasswordAge = default;

            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT MinPasswordAge FROM Users WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        if (reader["MinPasswordAge"] != DBNull.Value)
                        {
                            MinPasswordAge = Convert.ToDateTime(reader["MinPasswordAge"].ToString());
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

            int diff = DateTime.Compare(TimeNow, MinPasswordAge);

            if (diff > 0)
            {
                Session["LoggedIn"] = lbl_email.Text;
                Response.Redirect("ChangePassword.aspx", false);
            }
            else
            {
                lbl_Message.Text = ("Password Change cooldown time is 1 minute");
                lbl_Message.ForeColor = System.Drawing.Color.Red;

            }
        }
    }
}