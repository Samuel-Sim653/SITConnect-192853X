<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>

<script>

    function validateoldpwd() {
        var oldpwd = document.getElementById('<%=tb_currentpwd.ClientID %>').value;

        if (oldpwd.length == 0) {
            document.getElementById('<%=lbl_currentpwd_errors.ClientID%>').innerHTML = "Current Password is required!";
            document.getElementById('<%=tb_currentpwd.ClientID%>').style.border = "1px solid red";
        } else {
            document.getElementById('<%=lbl_currentpwd_errors.ClientID%>').innerHTML = "";
            document.getElementById('<%=tb_currentpwd.ClientID%>').style.border = "";
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
</script>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>Change Password</legend>

               <table class="style1">
                    <tr>
                        <td class="style3" style="height: 55px">
                            Current Password</td>
                        <td class="style2" style="height: 55px">
                            <asp:TextBox ID="tb_currentpwd" runat="server" TextMode="Password" onkeyup="javascript:validateoldpwd()" maxlength="15"></asp:TextBox>
                    
                            <asp:Label ID="lbl_currentpwd_errors" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="style3" style="height: 55px">
                            New Password</td>
                        <td class="style2" style="height: 55px">
                            <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" onkeyup="javascript:validatepwd()" maxlength="15"></asp:TextBox>
                            <asp:Label ID="lbl_pwd_errors" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>

                   <tr>
                        <td class="style3" style="height: 55px">
                            Confirm Password</td>
                        <td class="style2" style="height: 55px">
                            <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" onkeyup="javascript:validatepwd2()" maxlength="15"></asp:TextBox>
                            <asp:Label ID="lbl_pwd2_errors" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <br />

                <asp:Button ID="btnChangepass" runat="server" Text="Change Password" OnClick="Btn_ChangePass_Click" style="height: 29px" />

                <br />
                <br />
                <p />
            </fieldset>
        </div>
    </form>
</body>
</html>
    