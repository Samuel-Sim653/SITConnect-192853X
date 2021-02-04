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
    public partial class Register : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            bool validInput = ValidateInput();

            if (validInput)
            {
                string pwd = tb_pwd.Text.ToString().Trim(); ;
                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];
                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);

                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                createAccount();

                Response.Redirect("Login.aspx?status=registered");

            }

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
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }


        public void createAccount()
        {
            string fname = HttpUtility.HtmlEncode(tb_fname.Text);
            string lname = HttpUtility.HtmlEncode(tb_lname.Text);
            string credit = HttpUtility.HtmlEncode(tb_credit.Text);
            string email = HttpUtility.HtmlEncode(tb_email.Text);
            string dob = HttpUtility.HtmlEncode(tb_dob.Text);

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (Email, Fname, Lname, CreditCard, PasswordHash, PasswordSalt, Dob, Status, IV, Keys, MaxPasswordAge)" +
                        "VALUES (@Email, @Fname, @Lname, @CreditCard, @PasswordHash, @PasswordSalt, @Dob, @Status, @IV, @Keys, @MaxPasswordAge)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {


                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", email.Trim());
                            cmd.Parameters.AddWithValue("@Fname", fname.Trim());
                            cmd.Parameters.AddWithValue("@Lname", lname.Trim());
                            cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(credit.Trim())));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@Dob",dob.Trim());
                            cmd.Parameters.AddWithValue("@Status", 0);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Keys", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@MaxPasswordAge", DateTime.Now.AddMinutes(3));

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


        private bool CheckForEmail(String email)
        {
            bool IsValid = false;
            Regex r = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (r.IsMatch(email))
                IsValid = true;
            return IsValid;
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
            lbl_fname_errors.Text = String.Empty;
            lbl_lname_errors.Text = String.Empty;
            lbl_credit_errors.Text = String.Empty;
            lbl_email_errors.Text = String.Empty;
            lbl_pwd_errors.Text = String.Empty;
            lbl_pwd2_errors.Text = String.Empty;
            lbl_dob_errors.Text = String.Empty;

            string fname = HttpUtility.HtmlEncode(tb_fname.Text).ToString().Trim();
            string lname = HttpUtility.HtmlEncode(tb_lname.Text).ToString().Trim();
            string credit = HttpUtility.HtmlEncode(tb_credit.Text).ToString().Trim();
            string email = HttpUtility.HtmlEncode(tb_email.Text).ToString().Trim();
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text).ToString().Trim();
            string pwd2 = HttpUtility.HtmlEncode(tb_pwd2.Text).ToString().Trim();
            string dob = HttpUtility.HtmlEncode(tb_dob.Text).ToString().Trim();
            string repeatedemail = null;

            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT Email FROM Users WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        repeatedemail = reader["Email"].ToString();
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

            if (fname == "")
            {
                lbl_fname_errors.Text += "First Name is required!";
            }
            else if (!Regex.IsMatch(fname, "^[A-Za-z]+$"))
            {
                lbl_fname_errors.Text += "Only Alphabets allowed!";
            }

            if (lname == "")
            {
                lbl_lname_errors.Text += "Last name is required!";
            }
            else if (!Regex.IsMatch(lname, "^[A-Za-z]+$"))
            {
                lbl_lname_errors.Text += "Only Alphabets allowed!";
            }

            if (credit == "")
            {
                lbl_credit_errors.Text += "Credit Card is required!";
            }
            else if (!Regex.IsMatch(credit, "^[0-9]*$"))
            {
                lbl_credit_errors.Text += "Only Numbers allowed!";
            }

            if (email == "")
            {
                lbl_email_errors.Text += "Email is required!";
            }
            else if (CheckForEmail(email) == false)
            {
                lbl_email_errors.Text += "Enter Valid Email!";
            }
            else if (repeatedemail != null)
            {
                lbl_email_errors.Text += "Email already exists!" + "<br/>";
            }

            if (pwd == "")
            {
                lbl_pwd_errors.Text += "Password is required!";

            } else {
            
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
            else if (pwd != pwd2) {
                lbl_pwd2_errors.Text += "Password does not match!";
            }

            if (dob == "")
            {
                lbl_dob_errors.Text += "Date Of Birth is required!";
            }

            if (String.IsNullOrEmpty(lbl_fname_errors.Text + lbl_lname_errors.Text + lbl_credit_errors.Text + lbl_email_errors.Text + lbl_pwd_errors.Text + lbl_pwd2_errors.Text + lbl_dob_errors.Text) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}