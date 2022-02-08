<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OTP.aspx.cs" Inherits="SITConnect.OTP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>OTP</h1>
    <form id="form1" runat="server">
        *Click on Send Code button<div>
            One Time Pin:
            <asp:TextBox ID="tb_otp" runat="server" Width="209px"></asp:TextBox>
            <asp:Button ID="btn_send" runat="server" Height="36px" OnClick="btn_send_Click" Text="Send Code" />
            <br />
            <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
        </div>
    </form>
</body>
</html>
