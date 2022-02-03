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
            width: 237px;
        }
    </style>
    <script type="text/javascript">
        function validateFirstName() {
            var fName = document.getElementById("<%=tb_fName.ClientID %>").value;
            var fName_check = document.getElementById("lbl_fName_check");
            if (fName.length == 0) {
                fName_check.innerHTML = "Required!";
                fName_check.style.color = "Red";
            }
            else if (fName.length < 2) {
                fName_check.innerHTML = "Firstname must have a minimum of 2 characters!";
                fName_check.style.color = "Red";
            }
            else if (fName.length > 50) {
                fName_check.innerHTML = "Firstname must have a maximum of 50 characters!";
                fName_check.style.color = "Red";
            }
            else if (fName.search(/^[a-z ,.-]{2,50}$/i) == -1) {
                fName_check.innerHTML = "Firstname contains invalid characters!";
                fName_check.style.color = "Red";
            }
            else {
                fName_check.innerHTML = "Excellent!";
                fName_check.style.color = "Green";
            }
        }
        function validateLastName() {
            var lName = document.getElementById("<%=tb_lName.ClientID %>").value;
            var lName_check = document.getElementById("lbl_lName_check");
            if (lName.length == 0) {
                lName_check.innerHTML = "Required!";
                lName_check.style.color = "Red";
            }
            else if (lName.length < 2) {
                lName_check.innerHTML = "Lastname must have a minimum of 2 characters!";
                lName_check.style.color = "Red";
            }
            else if (lName.length > 50) {
                lName_check.innerHTML = "Lastname must have a maximum of 50 characters!";
                lName_check.style.color = "Red";
            }
            else if (lName.search(/^[a-z ,.-]{2,50}$/i) == -1) {
                lName_check.innerHTML = "Lastname contains invalid characters!";
                lName_check.style.color = "Red";
            }
            else {
                lName_check.innerHTML = "Excellent!";
                lName_check.style.color = "Green";
            }
        }
        function validateCreditCard() {
            var cCard = document.getElementById("<%=tb_creditcard.ClientID %>").value;
            var cCard_check = document.getElementById("lbl_creditcard_check");
            if (cCard.length == 0) {
                cCard_check.innerHTML = "Required!";
                cCard_check.style.color = "Red";
            }
            else if (isNaN(cCard)) {
                cCard_check.innerHTML = "Credit card must only contain numbers!";
                cCard_check.style.color = "Red";
            }
            else if (cCard.length != 16) {
                cCard_check.innerHTML = "Length of credit card must be 16!";
                cCard_check.style.color = "Red";
            }
            else {
                cCard_check.innerHTML = "Excellent!";
                cCard_check.style.color = "Green";
            }
        }
        function validateEmail() {
            var email = document.getElementById("<%=tb_email.ClientID %>").value;
            var email_check = document.getElementById("lbl_email_check");
            if (email.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i)) {
                email_check.innerHTML = "Email is invalid!";
                email_check.style.color = "Red";
            }
            else {
                email_check.innerHTML = "Excellent!";
                email_check.style.color = "Green";
            }
        }
        function validatePwd() {
            var pwd = document.getElementById("<%=tb_password.ClientID %>").value;
            var pwd_check = document.getElementById("lbl_pwd_check");
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
        function validateCfmPwd() {
            var pwd = document.getElementById("<%=tb_cfmPassword.ClientID %>").value;
            var pwd_check = document.getElementById("lbl_cfmPwd_check");
            if (pwd.length == 0) {
                pwd_check.innerHTML = "Required!";
                pwd_check.style.color = "Red";
            }
            else {
                pwd_check.innerHTML = "Excellent";
                pwd_check.style.color = "Green";
            }
        }
        function validateDOB() {
            var dob = document.getElementById("<%=tb_cfmPassword.ClientID %>").value;
            var dob_check = document.getElementById("lbl_date_check");
            if (dob.length == 0) {
                dob_check.innerHTML = "Required!";
                dob_check.style = "Red";
            }
            else {
                dob_check.innerHTML = "Excellent";
                dob_check.style.color = "Green";
            }
        }
        function validatePhoto() {
            console.log(photo.length);
            var photo = document.getElementById("<%=upload_photo.ClientID %>").files;
            var photo_check = document.getElementById("lbl_photo_check");

            
            if (photo.length == 0) {
                photo_check.innerHTML = "Required!";
                photo_check.style = "Red";
            }
            else {
                photo_check.innerHTML = "Excellent";
                photo_check.style = "Green";
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
                    <asp:TextBox ID="tb_fName" runat="server" onkeyup="javascript:validateFirstName()" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_fName_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Last Name</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_lName" runat="server" onkeyup="javascript:validateLastName()" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_lName_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Credit Card Number</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_creditcard" runat="server" onkeyup="javascript:validateCreditCard()" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_creditcard_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Email Address</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_email" runat="server" onkeyup="javascript:validateEmail()" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_email_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Password</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_password" runat="server" onkeyup="javascript:validatePwd()" TextMode="Password" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_pwd_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Confirm Password</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_cfmPassword" runat="server" onkeyup="javascript:validateCfmPwd()" TextMode="Password" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_cfmPwd_check" runat="server">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Date of Birth</td>
                <td class="auto-style3">
                    <asp:TextBox ID="tb_dob" runat="server" onchange="javascript:validateDOB()" TextMode="Date" required="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_date_check" runat="server" onchange="javascript:validatePhoto()">Required!</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Photo</td>
                <td class="auto-style3">
                    <asp:FileUpload ID="upload_photo" runat="server" accept=".png, .jpg" required="true" />
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
