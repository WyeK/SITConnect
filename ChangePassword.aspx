<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 174px;
        }
    </style>
</head>
<body>
    <h1>Change Password</h1>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbl_validation" runat="server"></asp:Label>
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">Existing Password</td>
                <td>
                    <asp:TextBox ID="tb_existingPwd" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">New Password</td>
                <td>
                    <asp:TextBox ID="tb_newPwd" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Confirm New Password</td>
                <td>
                    <asp:TextBox ID="tb_cfmNewPwd" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <p>
            <asp:Button ID="btn_changePwd" runat="server" Text="Confirm" OnClick="btn_changePwd_Click" style="height: 26px" />
        </p>
    </form>
</body>
</html>
