<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="downlodEx.SearchResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
            <h1>Download  only Search Result</h1>

        </div>
    <br />
    <div style="display:flex; flex:1; flex-direction:row">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="Button1" runat="server" Text="DowanLoad EX" OnClick="UploadButton_Click" />
        <asp:Label ID="StatusLabel" runat="server" Text="" />
        </div>
</asp:Content>
