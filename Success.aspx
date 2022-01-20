<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="SITConnect.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 23px;
        }
    </style>
</head>
<body>
    <h1>Profile</h1>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">First Name:</td>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_fName" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Last Name:</td>
                    <td>
                        <asp:Label ID="lbl_lName" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Credit Card Number:</td>
                    <td>
                        <asp:Label ID="lbl_creditCard" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Email:</td>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_email" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Date of Birth:</td>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_dob" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Photo:</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btn_logout" runat="server" OnClick="btn_logout_Click" Text="Logout" />
        <br />
        <br />
        <asp:HyperLink ID="link_changePwd" runat="server" NavigateUrl="~/ChangePassword.aspx">Change Passwords Here</asp:HyperLink>
    </form>
</body>
</html>
