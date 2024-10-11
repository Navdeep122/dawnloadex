<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="d.aspx.cs" Inherits="downlodEx.d" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login Page</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #000;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .login-container {
            background-color: #333;
            padding: 30px;
            height:266px;
            border-radius: 10px;
            box-shadow: 0px 0px 15px rgba(246, 255, 0, 0.5);
            text-align: center;
            width: 300px;
            color: white;
            z-index: 1;
        }

        .login-container h1 {
            margin-bottom: 20px;
            color: limegreen;
        }

        .login-container input[type="text"], 
        .login-container input[type="password"] {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border: none;
            border-radius: 5px;
            background-color: #444;
            color: white;
        }

        .login-container input[type="submit"] {
            width: 107%;
            padding: 10px;
            background-color: limegreen;
            border: none;
            color: black;
            cursor: pointer;
            border-radius: 5px;
            transition: background-color 0.3s ease;
        }

        .login-container input[type="submit"]:hover {
            background-color: #00cc00;
        }

        .login-container a {
            color: limegreen;
            text-decoration: none;
        }

        .login-container a:hover {
            text-decoration: underline;
        }

        /* Grid background animation */
        .background-grid {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: black;
            background-size: 50px 50px;
            z-index: -1;
            
        }

        .background-grid::before {
            content: "";
            position: absolute;
            width: 100%;
            height: 100%;
            background: linear-gradient(to right, limegreen 1px, transparent 1px), linear-gradient(to bottom, limegreen 1px, transparent 1px);
            background-size: 50px 50px;
            animation: animate 5s linear infinite;
        }

        @keyframes animate {
            0% {
                transform: translate(0, 0);
            }
            100% {
                transform: translate(-50px, -50px);
            }
        }

        /* Hover effect for grid */
        .background-grid:hover::before {
            background: linear-gradient(to right, red 1px, transparent 1px), linear-gradient(to bottom, red 1px, transparent 1px);
        }
    </style>
</head>
<body>
    <div class="background-grid" id="grid"></div>

    <form id="form1" runat="server">
        <div class="login-container">
            <h1>Findoc RMS User</h1>
            <asp:TextBox ID="txtUsernames" runat="server" placeholder="Username"></asp:TextBox><br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /><br />
           
        </div>
    </form>

    <script>
        // Change the color based on mouse movement over the grid
        document.addEventListener('mousemove', function (e) {
            const grid = document.getElementById('grid');
            const x = e.clientX / window.innerWidth;
            const y = e.clientY / window.innerHeight;
            const r = Math.floor(x * 255);
            const g = Math.floor(y * 255);
            const b = 150; // Fixed blue component for contrast

            grid.style.backgroundColor = `rgb(${r}, ${g}, ${b}, 0.1)`; // Set semi-transparent color
        });
    </script>
</body>
</html>--%>
