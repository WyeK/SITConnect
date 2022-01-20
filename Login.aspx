<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lc6U7odAAAAAA5sGGKI4ae2gTpL2TUgB6_3Obu1"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login</h1>
            <asp:Label ID="lbl_validation" runat="server"></asp:Label>

            <br />
            <table class="auto-style1">
                <tr>
                    <td>Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password</td>
                    <td>
                        <asp:TextBox ID="tb_password" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        </div>
        <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
        <br />
        <br />
        <asp:HyperLink ID="link_register" runat="server" NavigateUrl="~/Registration.aspx">Create Account Here</asp:HyperLink>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc6U7odAAAAAA5sGGKI4ae2gTpL2TUgB6_3Obu1', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        })
    </script>
</body>
</html>
