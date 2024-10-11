<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportFile.aspx.cs" Inherits="downlodEx.ImportFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="display:flex; flex-direction:row; align-items:center;">
        <div style="margin-right:10px;">
            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
        </div>
        <div style="margin-right:36px;">
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        </div>
        <div>
            <asp:Button ID="btnDownload" runat="server" Text="Download Combined Excel" OnClick="btnDownload_Click"/>
        </div>
    </div>
</asp:Content>