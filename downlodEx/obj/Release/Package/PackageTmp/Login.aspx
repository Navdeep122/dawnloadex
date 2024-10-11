<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="downlodEx.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <style>
            body {
                font-family: Arial, sans-serif;
                background: #3b3b98;
                display: flex;
                justify-content: center;
                align-items: center;
                height: 100vh;
                margin: 0;
            }

            .login-container {
                width: 300px;
                margin: 50px auto;
                padding: 136px;
                border: 1px solid #ccc;
                border-radius: 5px;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                color: #fff;
                padding-left: 53px;
                padding-right: 66px;
            }

                .login-container img {
                    max-width: 100%;
                    height: auto;
                    margin-bottom: 20px;
                }

                .login-container label {
                    display: block;
                    margin-bottom: 10px;
                }

                .login-container input[type="text"],
                .login-container input[type="password"] {
                    width: 100%;
                    padding: 8px;
                    margin-bottom: 10px;
                    border: 1px solid #ccc;
                    border-radius: 5px;
                }

                .login-container input[type="submit"],
                .login-container input[type="button"] {
                    width: 106%;
                    padding: 8px;
                    background-color: #007bff;
                    border: none;
                    color: white;
                    border-radius: 5px;
                    cursor: pointer;
                    margin-bottom: 10px;
                }
        </style>
        <div class="login-container">

            <h2>Login</h2>
            <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"  /> 
         



            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    
    </form>
</body>
</html>
