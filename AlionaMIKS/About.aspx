<%@ Page Title="Диспетчер" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="AlionaMIKS.About" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<script type="text/javascript">

    function OpenD() {

        var r = window.showModalDialog("Web_Test.aspx", null, "dialogWidth:450px;menubar:0;dialogHeight:450px");
    }

</script>

    <h2>
        О программе
    </h2>
    <p>
        Просто программа
    </p>
</asp:Content>
