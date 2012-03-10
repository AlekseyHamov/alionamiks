<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="Operts.aspx.cs" Inherits="AlionaMIKS.Directory.Operts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
      <h3>Справочник Операций</h3> 
      <asp:Label id="Msg" runat="server" ForeColor="Red" />
<div style="display:inline" >
<div style="width:30%; float:left"  > 
      <asp:ObjectDataSource 
        ID="OpertsObjectDataSource" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataOperts.OpertsData" 
        SortParameterName="SortColumns"
        EnablePaging="true"
        SelectCountMethod="SelectCount"
        StartRowIndexParameterName="StartRecord"
        MaximumRowsParameterName="MaxRecords" 
        SelectMethod="GetAllOperts" 
        InsertMethod="InsertOperts" 
        UpdateMethod="UpdateOperts" 
        DeleteMethod="DeleteOperts"
        OnInserted="OpertsDataSource_OnInserted"
        OnUpdated="OpertsDataSource_OnUpdated"
        OnDeleted="OpertsDataSource_OnDeleted" >
        <InsertParameters>
              <asp:ControlParameter ControlID="TextBox2" Name="NameOperts" 
                  PropertyName="Text" />
          </InsertParameters>
        <deleteparameters>
            <asp:controlparameter name="ID_Operts" controlid="OpertsGridView" propertyname="SelectedValue" />
        </deleteparameters>
        <updateparameters>
            <asp:controlparameter name="ID_Operts" controlid="OpertsGridView" propertyname="SelectedValue" />
            <asp:ControlParameter ControlID="TextBox2" Name="NameOperts" 
                PropertyName="Text" />
        </updateparameters>
      </asp:ObjectDataSource>
      <asp:ObjectDataSource 
        ID="ObjectDataTempGrig" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataImage.ImageData" 
        SelectMethod="GetTempGrid" >
        <SelectParameters>
            <asp:Parameter Name= "NameTable" DefaultValue="Operts" />  
            <asp:ControlParameter Name= "ID_Table" ControlID="OpertsGridView" PropertyName="SelectedValue" DefaultValue="0" />  
        </SelectParameters> 
      </asp:ObjectDataSource>

      <table cellspacing="10">
        <tr>
          <td valign="top">
            <asp:GridView ID="OpertsGridView" 
              DataSourceID="OpertsObjectDataSource" 
              AutoGenerateColumns="False"
              AllowSorting="True"
              AllowPaging="True"
              PageSize="18"
              DataKeyNames="ID_Operts"
              OnSelectedIndexChanged="GridView_OnSelectedIndexChanged"
              RunAt="server" >  
              <HeaderStyle backcolor="lightblue" forecolor="black"/>
              <Columns>                
                <asp:ButtonField HeaderText = "Ред."
                                 CommandName="Select" ButtonType="Image" 
                      ImageUrl="~/Image/edit.png" FooterText="Проба">  
                  <ControlStyle Height="15px" Width="15px" />
                </asp:ButtonField>
                <asp:BoundField DataField="ID_Operts" HeaderText="Номер п/п" 
                      SortExpression="ID_Operts" Visible="False" />
                <asp:BoundField 
                        DataField="NameOperts"
                        HeaderText="Наименование" 
                      SortExpression="NameOperts" />
                <asp:ButtonField
                        CommandName="Delete" ButtonType="Image" 
                        ImageUrl="~/Image/deletion.png" HeaderText="Удалить"
                        >  
                    <ControlStyle Height="15px" Width="15px" />
                </asp:ButtonField>
              </Columns>                
            </asp:GridView>    
          </td>
        </tr>
        <tr>
          <td>
           <asp:Button ID="btnEditCustomer" OnClick="GetNextPage"  runat="server" Text="Добавить" />
          </td>
        </tr>
      </table>
</div>
</div>
    </asp:Content>
