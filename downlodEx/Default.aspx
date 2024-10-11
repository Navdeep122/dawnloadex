<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="downlodEx.Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Please Select in items</title>
    <style>
        .styled-dropdown {
            width: 100%;
            padding: 10px;
            font-size: 16px;
            border: 4px solid #ccc;
            border-radius: 13px;
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            background: #fff url('data:image/svg+xml;utf8,<svg fill="none" height="24" viewBox="0 0 24 24" width="24" xmlns="http://www.w3.org/2000/svg"><path d="M7 10l5 5 5-5H7z" fill="%23666"/></svg>') no-repeat right 10px center;
            background-size: 1em;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 20%;">
            <asp:DropDownList ID="ddlData" runat="server" CssClass="styled-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                <asp:ListItem Text="Select your item" Value="" />
                <asp:ListItem Text="MarginExport CASH1 & FO" Value="1" />
                <asp:ListItem Text="MarginExport MCX1 & NCDEX" Value="2" />
            </asp:DropDownList>
        </div>
    </form>
</body>
</html>
