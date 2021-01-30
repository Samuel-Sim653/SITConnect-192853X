<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="SITConnect.Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>HomePage</legend>

                <asp:Label ID="lbl_Message" runat="server" EnableViewState="false" />

                <br />
                <br />
                Email:
                <asp:Label ID="lbl_email" runat="server" EnableViewState="false" />

                <br />
                <br />

                First Name:
                <asp:Label ID="lbl_fname" runat="server" EnableViewState="false"/> 

                <br />
                <br />

                Last Name:
                <asp:Label ID="lbl_lname" runat="server" EnableViewState="false"/> 

                <br />
                <br />

                 Credit Card:
                <asp:Label ID="lbl_credit" runat="server" EnableViewState="false" />

                <br />
                <br />

                <asp:Button ID="btnChangepass" runat="server" Text="Change Password" OnClick="ChangepassMe" Visible="false" style="height: 29px" />

                <br />
                <br />

                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" style="height: 29px" width="125px"/>

                <p />
            </fieldset>
        </div>
    </form>
</body>
</html>
    