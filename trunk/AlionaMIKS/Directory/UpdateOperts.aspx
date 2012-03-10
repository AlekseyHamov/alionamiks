<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="UpdateOperts.aspx.cs" Inherits="AlionaMIKS.Directory.UpdateOperts" %>
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
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
        </SelectParameters>
      </asp:ObjectDataSource>
      <asp:ObjectDataSource 
        ID="SelectOneOperts" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataOperts.OpertsData" 
        SelectMethod="GetOneOperts"
        InsertMethod="InsertOperts"
        DeleteMethod="DeleteOperts"
        OnInserted="OpertsDataSource_OnInserted"   >
        <SelectParameters>
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="NameOperts" DefaultValue="" />
            <asp:Parameter Name="ChecOperts" DefaultValue="" />
        </InsertParameters> 
        <UpdateParameters>
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
            <asp:Parameter Name="NameOperts" DefaultValue="" />
            <asp:Parameter Name="ChecOperts" DefaultValue="" />
            <asp:Parameter Name="ID_Operts_Group" DefaultValue="" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
        </DeleteParameters> 

      </asp:ObjectDataSource>
      <asp:ObjectDataSource 
        ID="OpertsObjectDataSource" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataOperts.OpertsData" 
        SelectMethod="GetAllsOperts">
        <SelectParameters>
           <asp:Parameter Name="ID_Operts"  DefaultValue="" />
           <asp:Parameter Name="ID_Manufacture"  DefaultValue="" />
        </SelectParameters>
      </asp:ObjectDataSource>

      <asp:ObjectDataSource 
        ID="ObjectDataOpertsCheck" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataOperts.OpertsData" 
        InsertMethod="InsertOpertsCheck"
        DeleteMethod="DeleteOpertsCheck"  >
        <InsertParameters>
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
            <asp:Parameter Name="ID_Link" DefaultValue="" />
            <asp:Parameter Name="Link_NameTable" DefaultValue="Manufacture" />
        </InsertParameters> 
        <DeleteParameters>
            <asp:Parameter Name="ID_Operts" DefaultValue="" />
            <asp:Parameter Name="ID_Link" DefaultValue="" />
            <asp:Parameter Name="Link_NameTable" DefaultValue="Manufacture" />
        </DeleteParameters> 
      </asp:ObjectDataSource>

      <h3>Редактирование Операции</h3> 
      <asp:Label id="Msg" runat="server" ForeColor="Red" />
      <div style="display:inline" >
        <br />
        Наименование операции:
        <br />
        <asp:ListView ID="OpertsLV" DataSourceID="SelectOneOperts" 
            DataKeyNames="ID_Operts, NameOperts, ChecOperts, ID_Operts_Group" runat="server" GroupItemCount="5" 
            >
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
            <asp:TextBox ID="NameOperts" runat="server" Text='<%#Eval("NameOperts") %>' /> 
            <asp:CheckBox ID="ChecOperts" runat="server" Text="Группа" Checked='<%#Eval("ChecOperts") %>'  /> 
        </ItemTemplate> 
        </asp:ListView> 
        <asp:TextBox ID="NameOpertsIns" runat="server" Text='<%#Eval("NameOperts") %>' Visible="false" /> 
        <asp:CheckBox ID="ChecOpertsIns" runat="server" Text="Группа"  Visible="false" />
        <div runat="server">
        <div id="DivListViewImage" runat="server" style="height:70%; width:500px; overflow-x:scroll; float:left;">
        <asp:ListView  ID="ManufactureCheckBox" runat="server" 
            DataKeyNames="NameManufacture,fileType,fileName,ID_files, ID_Manufacture, Oplist " 
            DataSourceID="ManufactureChecked" GroupItemCount="5" >
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
                    <asp:CheckBox ID="IDCheck" Text='<%#Eval("NameManufacture") %>' Checked='<%#Eval("Oplist") %>' OnCheckedChanged="Select_Manufacture" ToolTip='<%#Eval("ID_Manufacture") %>' runat="server" />
                </td>
            </ItemTemplate>
        </asp:ListView >
        </div>
        <div id="DivGroups" runat="server" style="height:70%; width:500px; overflow-y:scroll;">
        <asp:RadioButtonList ID="RadioButtonGroups" runat="server" DataSourceID="OpertsObjectDataSource" 
            DataValueField="ID_Operts" 
            DataTextField="NameOperts">
        </asp:RadioButtonList> 
        <br />
            <asp:RadioButton ID="FlagGroupsList" runat="server" Text="Отвязать групу" />
        </div>
        </div>
        <br />
        <asp:Button Text="Обновить" ID="UpdateButtonOperts" runat="server" OnClick="OpertsDataSource_OnUpdated" Visible="false"/>
        <asp:Button Text="Добавить" ID="AddButtonOperts" runat="server" CommandName="Insert" 
                            OnCommand="CommandBtn_Click" Visible="false"/>
        <asp:Button Text="Удалить" ID="DeleteButtonOperts" runat="server" OnClick="OpertsDataSource_OnDeleteed" Visible="false"/>
</div>
    </asp:Content>
