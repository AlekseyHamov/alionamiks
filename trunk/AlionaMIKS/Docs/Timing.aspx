<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Culture="auto" UICulture="auto"  
    CodeBehind="Timing.aspx.cs" Inherits="AlionaMIKS.Doc.Timing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
      <h3>Справочник Заявок</h3> 
      <asp:Label id="Msg" runat="server" ForeColor="Red" />
      <p >  
      <asp:ObjectDataSource 
        ID="ClaimObjectDataSource" 
        runat="server" 
        TypeName="Samples.AspNet.ObjectDataTiming.TimingData" 
        SortParameterName="SortColumns"
        EnablePaging="true"
        SelectCountMethod="SelectCount"
        StartRowIndexParameterName="StartRecord"
        MaximumRowsParameterName="MaxRecords" 
        DeleteMethod="DeleteRecord"
        SelectMethod="GetAll" 
        InsertMethod="InsertRecord" 
        UpdateMethod="UpdateRecord" 
        OnInserted="DetailsObjectDataSource_OnInserted"
        OnUpdated="DetailsObjectDataSource_OnUpdated"
        OnDeleted="DetailsObjectDataSource_OnDeleted">
        <InsertParameters>
        </InsertParameters>
        <updateparameters>
            <asp:controlparameter name="ID_Timing" controlid="GridView" propertyname="SelectedValue" />
        </updateparameters>
        <deleteparameters>
            <asp:controlparameter name="ID_Timing" controlid="GridView" propertyname="SelectedValue" />
        </deleteparameters>
        <SelectParameters>
        </SelectParameters> 
      </asp:ObjectDataSource>
      </p>

      <asp:GridView ID="GridView" 
              DataSourceID="ClaimObjectDataSource" 
              AutoGenerateColumns="False"
              AllowSorting="True"
              AllowPaging="True"
              PageSize="9"
              DataKeyNames="ID_Claim"
              OnSelectedIndexChanged="GridView_OnSelectedIndexChanged"
              RunAt="server" ToolTip="Список заявок">
              <HeaderStyle backcolor="lightblue" forecolor="black" />
              <Columns>                
                <asp:ButtonField 
                      CommandName="Select" ButtonType="Image" 
                      ImageUrl="~/Image/edit.png" HeaderText="Ред.">  
                  <ControlStyle Height="15px" Width="15px"/>
                </asp:ButtonField>
                <asp:BoundField DataField="ID_Claim" HeaderText="Номер п/п" 
                SortExpression="ID_Claim" Visible="False"/>
                <asp:BoundField 
                        DataField="Note"
                        HeaderText="Описание" 
                      SortExpression="Note"/>
                <asp:ButtonField
                        CommandName="Delete" ButtonType="Image" 
                        ImageUrl="~/Image/deletion.png" HeaderText="Удалить"
                        >  
                    <ControlStyle Height="15px" Width="15px" />
                </asp:ButtonField>
            </Columns>                
            </asp:GridView>
      <asp:Button ID="btnEditCustomer" OnClick="GetNextPage"  Text = "добавить" runat="server" />
</asp:Content>
