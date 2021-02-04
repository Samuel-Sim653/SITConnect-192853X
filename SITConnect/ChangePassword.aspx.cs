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

namespace SITConnect
{
    public partial class ChangePassword : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string NewHash;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("HomePage.aspx?status=passwordchanged");
                }
                
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

            string strStatus = Request.QueryString?["status"]?.ToString();
            if (strStatus != null)
            {
                if (strStatus.ToUpper() == "PASSWORDEXPIRED")
                {
                    lbl_error.Text = "Your password has expired, please enter a new password.";
                    lbl_error.ForeColor = Color.Green;
                }
            }
        }

        protected void Btn_ChangePass_Click(object sender, EventArgs e)
        {
            lbl_currentpwd_errors.Text = String.Empty;

            bool validInput = ValidateInput();

            if (validInput)
            {
                string email = (string)Session["LoggedIn"];
                string currentpassword = HttpUtility.HtmlEncode(tb_currentpwd.Text).ToString().Trim();
                string newpassword = HttpUtility.HtmlEncode(tb_pwd.Text).ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                string dbHashold = getoldHash(email);

                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = currentpassword + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string currentHash = Convert.ToBase64String(hashWithSalt);

                    if (currentHash.Equals(dbHash))
                    {
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];
                        //Fills array of bytes with a cryptographically strong sequence of random values.
                        rng.GetBytes(saltByte);

                        string newpwdWithSalt = newpassword + dbSalt;
                        byte[] newhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwdWithSalt));
                        NewHash = Convert.ToBase64String(newhashWithSalt);

                        if (dbHashold != null)
                        {
                            if (NewHash.Equals(dbHashold))
                            {
                                lbl_pwd_errors.Text = "New password cannot be similar to previous 2 passwords";
                                lbl_pwd_errors.ForeColor = Color.Red;
                            }
                            else
                            {
                                updatePassword(email);
                                Response.Redirect("HomePage.aspx?status=passwordchanged");
                            }
                        }
                        else
                        {
                            if (newpassword == currentpassword)
                            {
                                lbl_pwd_errors.Text = "New password cannot be similar to current password";
                                lbl_pwd_errors.ForeColor = Color.Red;
                            }
                            else
                            {
                                updatePassword(email);
                                Response.Redirect("HomePage.aspx?status=passwordchanged");
                            }
                        }
                    }
                    else
                    {
                        lbl_currentpwd_errors.Text = "Password is incorrect";
                        lbl_currentpwd_errors.ForeColor = Color.Red;
                    }
                }
            }

        }

        private int checkPassword(string password)
        {
            int score = 0;

            // Very Weak
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            // Weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            //Medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            // Strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // Very Strong
            if (Regex.IsMatch(password, "[!@#$%^&*]"))
            {
                score++;
            }

            return score;

        }

        private bool ValidateInput()
        {
 
            lbl_pwd_errors.Text = String.Empty;
            lbl_pwd2_errors.Text = String.Empty;

            string oldpwd = HttpUtility.HtmlEncode(tb_currentpwd.Text).ToString().Trim();
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text).ToString().Trim();
            string pwd2 = HttpUtility.HtmlEncode(tb_pwd2.Text).ToString().Trim();

            if (pwd == "")
            {
                lbl_pwd_errors.Text += "Password is required!";
            }
            else
            {

                int scores = checkPassword(pwd);
                string status = "";
                switch (scores)
                {
                    case 1:
                        status = "Please enter a stronger password!";
                        break;
                    case 2:
                        status = "Please enter a stronger password!";
                        break;
                    case 3:
                        status = "Please enter a stronger password!";
                        break;
                    case 4:
                        status = "";
                        break;
                    case 5:
                        status = "";
                        break;
                    default:
                        break;
                }
                lbl_pwd_errors.Text = status;
                if (scores < 4)
                {
                    lbl_pwd_errors.ForeColor = Color.Red;
                    tb_pwd.BorderColor = Color.Red;

                }
            }

            if (pwd2 == "")
            {
                lbl_pwd2_errors.Text += "Confirm Password is required!";
            }
            else if (pwd != pwd2)
            {
                lbl_pwd2_errors.Text += "Password does not match!";
            }

            if (oldpwd == "")
            {
                lbl_currentpwd_errors.Text += "Confirm Password is required!";
            }
            else if (oldpwd == pwd2)
            {
                lbl_pwd_errors.Text = "New password cannot be similar to current password";
                lbl_pwd2_errors.Text = "New password cannot be similar to current password";
                lbl_pwd_errors.ForeColor = Color.Red;
            }

            if (String.IsNullOrEmpty(lbl_pwd_errors.Text + lbl_pwd2_errors.Text + lbl_currentpwd_errors.Text))
            {
                return true;
            }
            else
            {
                return false;
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

        protected string getoldHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PrevPasswordHash FROM Users WHERE Email= @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PrevPasswordHash"] != null)
                        {
                            if (reader["PrevPasswordHash"] != DBNull.Value)
                            {
                                h = reader["PrevPasswordHash"].ToString();
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

        protected void updatePassword(string email)
        {
            try
            {
                SqlConnection con = new SqlConnection(MYDBConnectionString);
                string sql = "UPDATE Users set PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, PrevPasswordHash = @PrevPasswordHash, MinPasswordAge =@MinPasswordAge, MaxPasswordAge =@MaxPasswordAge WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@PasswordHash", NewHash);
                    cmd.Parameters.AddWithValue("@PasswordSalt", getDBSalt(email));
                    cmd.Parameters.AddWithValue("@PrevPasswordHash", getDBHash(email));
                    cmd.Parameters.AddWithValue("@MinPasswordAge", DateTime.Now.AddMinutes(1));
                    cmd.Parameters.AddWithValue("@MaxPasswordAge", DateTime.Now.AddMinutes(3));


                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }  
}