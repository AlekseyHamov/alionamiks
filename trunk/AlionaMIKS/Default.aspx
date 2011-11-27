<%@ Page Title="Домашняя страница" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="AlionaMIKS._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<script runat="server">
</script>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
        Добро пожаловать!
      <asp:Label id="Msg" runat="server" ForeColor="Red" />
    </h2>
</asp:Content> 
