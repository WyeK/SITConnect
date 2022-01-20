<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lc6U7odAAAAAA5sGGKI4ae2gTpL2TUgB6_3Obu1"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 265px;
        }
        .auto-style3 {
            width: 153px;
        }
    </style>
    <script type="text/javascript">
        function validatePwd() {
            var pwd = document.getElementById("<%=tb_password.ClientID %>").value;
            var pwd_check = document.getElementById("lbl_pwd_check");
            if (pwd.length < 12) {
                pwd_check.innerHTML = "Password require at least 12 characters";
                pwd_check.style.color = "Red";
            }
            else if (pwd.search(/[a-z]/) == -1) {
                pwd_check.innerHTML = "Password require at least 1 lowercase"
                pwd_check.style.color = "Red";
            }
            else if (pwd.search(/[A-Z]/) == -1) {
                pwd_check.innerHTML = "Password require at least 1 uppercase"
                pwd_check.style.color = "Red";
            }
            else if (pwd.search(/[0-9]/) == -1) {
                pwd_check.innerHTML = "Password require at least 1 number"
                pwd_check.style.color = "Red";
            }
            else if (pwd.search(/[^A-Za-z0-9]/) == -1) {
                pwd_check.innerHTML = "Password require at least 1 special character"
                pwd_check.style.color = "Red";
            }
            else {
                pwd_check.innerHTML = "Excellent";
                pwd_check.style.color = "Green";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Registration</h1>

        <table class="auto-style1">
            <tr>
                <td class="auto-style2">First Name</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_fName" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_fName_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Last Name</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_lName" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_lName_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Credit Card Number</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_creditcard" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_creditcard_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Email Address</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_email_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Password</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_password" runat="server" onkeyup="javascript:validatePwd()" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_pwd_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Confirm Password</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_cfmPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_cfmPwd_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Date of Birth</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_date_check" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Photo</td>
                <td class="auto-style3">
                    <asp:FileUpload ID="upload_photo" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lbl_photo_check" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
        <asp:Button ID="btn_register" runat="server" Text="Register" OnClick="btn_register_Click"/>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc6U7odAAAAAA5sGGKI4ae2gTpL2TUgB6_3Obu1', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        })
    </script>
        <p>
            <asp:HyperLink ID="link_login" runat="server" NavigateUrl="~/Login.aspx">Login Here</asp:HyperLink>
        </p>
    </form>
    </body>
</html>
