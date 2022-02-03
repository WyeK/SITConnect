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
    <script>
        function validatePwd() {
            var pwd = document.getElementById("<%=tb_newPwd.ClientID %>").value;
            var pwd_check = document.getElementById("lbl_newPwd");
            if (pwd.length == 0) {
                pwd_check.innerHTML = "Required!";
                pwd_check.style.color = "Red";
            }
            else if (pwd.length < 12) {
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
                <td>
                    <asp:Label ID="lbl_existingPwd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">New Password</td>
                <td>
                    <asp:TextBox ID="tb_newPwd" runat="server" TextMode="Password" onkeyup="javascript:validatePwd()"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_newPwd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Confirm New Password</td>
                <td>
                    <asp:TextBox ID="tb_cfmNewPwd" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_cfmNewPwd" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <p>
            <asp:Button ID="btn_changePwd" runat="server" Text="Confirm" OnClick="btn_changePwd_Click" style="height: 26px" />
        </p>
    </form>
</body>
</html>
