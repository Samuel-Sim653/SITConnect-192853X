<%@ Page Title="" Language="C#" MasterPageFile="~/SITConnect.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SITConnect.Register" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>

        function validatefname() {
            var fname = document.getElementById('<%=tb_fname.ClientID %>').value;

            if (fname.length == 0) {
                document.getElementById('<%=lbl_fname_errors.ClientID%>').innerHTML = "First name is required!";
                document.getElementById('<%=tb_fname.ClientID%>').style.border = "1px solid red";
            }
            else if (fname.search(/^[A-Za-z]+$/) == -1) {
                document.getElementById('<%=lbl_fname_errors.ClientID%>').innerHTML = "Only Alphabets allowed!";
                document.getElementById('<%=tb_fname.ClientID%>').style.border = "1px solid red";
            } else {
                document.getElementById('<%=lbl_fname_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_fname.ClientID%>').style.border = "";
                return true
            }
        }

        function validatelname() {
            var lname = document.getElementById('<%=tb_lname.ClientID %>').value;

            if (lname.length == 0) {
                document.getElementById('<%=lbl_lname_errors.ClientID%>').innerHTML = "Last name is required!";
                document.getElementById('<%=tb_lname.ClientID%>').style.border = "1px solid red";
            }
            else if (lname.search(/^[A-Za-z]+$/) == -1) {
                document.getElementById('<%=lbl_lname_errors.ClientID%>').innerHTML = "Only words allowed!";
                document.getElementById('<%=tb_lname.ClientID%>').style.border = "1px solid red";
            } else {
                document.getElementById('<%=lbl_lname_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_lname.ClientID%>').style.border = "";
                return true
            }
        }

        function validatecredit() {
            var cred = document.getElementById('<%=tb_credit.ClientID %>').value;

            if (cred.length == 0) {
                document.getElementById('<%=lbl_credit_errors.ClientID%>').innerHTML = "Credit Card Info is required!";
                document.getElementById('<%=tb_credit.ClientID%>').style.border = "1px solid red";
            }
            else if (cred.search(/^[0-9]*$/) == -1) {
                document.getElementById('<%=lbl_credit_errors.ClientID%>').innerHTML = "Only numbers allowed!";
                document.getElementById('<%=tb_credit.ClientID%>').style.border = "1px solid red";
            } else {
                document.getElementById('<%=lbl_credit_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_credit.ClientID%>').style.border = "";
                return true
            }
        }

        function validateemail() {
            var email = document.getElementById('<%=tb_email.ClientID %>').value;

            if (email.length == 0) {
                document.getElementById('<%=lbl_email_errors.ClientID%>').innerHTML = "Email Info is required!";
                document.getElementById('<%=tb_email.ClientID%>').style.border = "1px solid red";
            }
            else if (email.search(/^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/) == -1) {
                document.getElementById('<%=lbl_email_errors.ClientID%>').innerHTML = "Please use legitimate email!";
                document.getElementById('<%=tb_email.ClientID%>').style.border = "1px solid red";
            } else {
                document.getElementById('<%=lbl_email_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_email.ClientID%>').style.border = "";
                return true
            }
        }

        function validatepwd() {
            var pwd = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (pwd.length == 0) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password is required!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";

            }
            else if (pwd.length < 8) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password length must be at least 8 characters!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";
                
            }
            else if (pwd.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password must contain 1 number!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";
                
            }
            else if (pwd.search(/[A-Z]/) == -1) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password must contain 1 uppercase!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";
                
            }
            else if (pwd.search(/[a-z]/) == -1) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password must contain 1 lowercase!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";
                
            }
            else if (pwd.search(/[!@#$%^&*]/) == -1) {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "Password must contain 1 special character!";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "1px solid red";  
                
            } else {
                document.getElementById('<%=lbl_pwd_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_pwd.ClientID%>').style.border = "";  
                return true
            }
        }

        function validatepwd2() {
            var pwd = document.getElementById('<%=tb_pwd.ClientID %>').value;
            var pwd2 = document.getElementById('<%=tb_pwd2.ClientID %>').value;

            if (pwd2.length == 0) {
                document.getElementById('<%=lbl_pwd2_errors.ClientID%>').innerHTML = "Confirm Password is required!";
                document.getElementById('<%=tb_pwd2.ClientID%>').style.border = "1px solid red";

            }

            else if (pwd != pwd2) {
                document.getElementById('<%=lbl_pwd2_errors.ClientID%>').innerHTML = "Password does not match!";
                document.getElementById('<%=tb_pwd2.ClientID%>').style.border = "1px solid red";

            } else {
                document.getElementById('<%=lbl_pwd2_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_pwd2.ClientID%>').style.border = "";
                return true
            }
        }

        function validatedob() {
            var dob = document.getElementById('<%=tb_dob.ClientID%>').value;

            if (dob.length == 0) {
                document.getElementById('<%=lbl_dob_errors.ClientID%>').innerHTML = "Date Of Birth is required!";
                document.getElementById('<%=tb_dob.ClientID%>').style.border = "1px solid red";

            } else {
                document.getElementById('<%=lbl_dob_errors.ClientID%>').innerHTML = "";
                document.getElementById('<%=tb_dob.ClientID%>').style.border = "";
                return true
            }
        }

    </script>


<body>
        <form id="form1" runat="server">
    <div>
    
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3" style="height: 55px">
                    First Name</td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_fname" runat="server" onkeyup="javascript:validatefname()" maxlength="20"></asp:TextBox>
                    
                    <asp:Label ID="lbl_fname_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    Last Name</td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_lname" runat="server" onkeyup="javascript:validatelname()"  maxlength="20"></asp:TextBox>
                    <asp:Label ID="lbl_lname_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    Credit Card Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_credit" runat="server" onkeyup="javascript:validatecredit()" TextMode="Password" maxLength="16" min="0" > </asp:TextBox>
                    <asp:Label ID="lbl_credit_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    Email Address</td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_email" runat="server" TextMode="Email" onkeyup="javascript:validateemail()" maxlength="80" ></asp:TextBox>
                    <asp:Label ID="lbl_email_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    Password</td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" onkeyup="javascript:validatepwd()" maxlength="15" ></asp:TextBox>
                    <asp:Label ID="lbl_pwd_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

             <tr>
                <td class="style3" style="height: 55px">
                    Confirm Password </td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" onkeyup="javascript:validatepwd2()" maxlength="15" > </asp:TextBox>
                    <asp:Label ID="lbl_pwd2_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    Date Of Birth</td>
                <td class="style2" style="height: 55px">
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date" onkeyup="javascript:validatedob()" min="1900-01-01" max="2010-01-01" Width="167px"></asp:TextBox>
                    <asp:Label ID="lbl_dob_errors" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="style3" style="height: 55px">
                    </td>
                <td class="style2" style="height: 55px">
                    <asp:Button ID="btn_register" runat="server" Text="Register" onClick="btn_Register_Click" />
                    
                </td>

            </tr>
            <tr>
                <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
            </tr>
    </table>
&nbsp;
    </div>
    </form>
</body>

</asp:Content>
