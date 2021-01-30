<%@ Page Title="" Language="C#" MasterPageFile="~/SITConnect.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head runat="server">
        <script src="https://www.google.com/recaptcha/api.js?render=6LcjMD8aAAAAAKC84tWQyHPiXUryc7wZkFPQILQ7"></script>
    </head>

    <body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1" style="height: 310px; width: 651px">
            <tr>
                 <asp:Label ID="lb_error" runat="server" ForeColor="#FF3300"></asp:Label>
                <td class="auto-style3" style="width: 150px; height: 55px">
        <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                </td>
                <td class="auto-style1" style="height: 55px; width: 406px;">
                    <asp:TextBox ID="tb_email" runat="server" Height="65px" Width="360px" TextMode="Email" CssClass="auto-style2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style3" style="width: 150px; height: 55px">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="auto-style1" style="height: 55px; width: 406px;">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="65px" Width="360px" TextMode="Password" style="margin-right: 0px"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="auto-style4" style="width: 150px; height: 50px;">
       
    <asp:Button ID="btn_Register" runat="server" Height="78px" 
        onclick="btn_Register_Click" Text="Register" Width="231px" />
       
                </td>
                <td class="style2" style="width: 406px; height: 50px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button ID="btn_Login" runat="server" Height="77px" 
        onclick="btn_Login_Click" Text="Login" Width="204px" style="margin-left: 0px" />
                </td>
            </tr>
    </table>
           <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/> 
&nbsp;&nbsp;&nbsp;
           <br />
       
           <br />
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
        <script>
            grecaptcha.ready(function () {
                grecaptcha.execute('6LcjMD8aAAAAAKC84tWQyHPiXUryc7wZkFPQILQ7', { action: 'Login' }).then(function (token) {
                    document.getElementById("g-recaptcha-response").value = token;
                });
            });
        </script>
</body>
</asp:Content>
