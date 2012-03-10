<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Culture="auto" UICulture="auto"  
    CodeBehind="UpdateTiming.aspx.cs" Inherits="AlionaMIKS.Doc.UpdateTiming" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
      <asp:ObjectDataSource 
        ID="ManufactureChecked" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataManufacture.ManufactureData" 
        SelectMethod="GetManufactureImage" >
        <SelectParameters>
            <asp:Parameter Name="str_ID" DefaultValue="" />
            <asp:Parameter Name="ID_Unit" DefaultValue="" />
        </SelectParameters>
      </asp:ObjectDataSource>

    <h3>Редактирование хронометрожа</h3> 
    <asp:Label id="Msg" runat="server" ForeColor="Red" />
    <div id="ManufactureScrol" runat="server" style="height:100px; width:1100px; overflow-x:scroll;" >
        <asp:ListView  ID="ManufactureCheckBox" runat="server" 
            RepeatDirection="Horizontal"
            DataKeyNames="NameManufacture,fileType,fileName,ID_files " 
            DataSourceID="ManufactureChecked" GroupItemCount="15" >
            <GroupTemplate>
                <tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </tr>
            </GroupTemplate>
            <LayoutTemplate>
                <table>
                    <asp:PlaceHolder ID="groupPlaceholder" runat="server" />
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <td style="border-style:ridge; border-width:thin;  ">
                    <asp:ImageMap ID="IMG" runat="server" Height="50" 
                    ImageUrl='<%#Eval("fileType", "~/Image_Data/"+Eval("fileName")+"_"+Eval("ID_files")+"."+Eval("fileType")) %>'  ToolTip='<%#Eval("NameManufacture") %>' />
                    <br>
                    </br>
                    <asp:RadioButton ID="IDCheck" Text='<%#Eval("NameManufacture")%>' runat="server" />
                </td>
            </ItemTemplate>
        </asp:ListView >
    </div>
      </asp:Content>
