using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using AjaxControlToolkit;

namespace AlionaMIKS.Directory
{
    public partial class UpdateOperts : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
         {
             
             if ((string)(Session["ID_Operts"]) != null)
            {
                 SelectOneOperts.SelectParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
                 ManufactureChecked.SelectParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
                 ManufactureCheckBox.DataBind();
                 UpdateButtonOperts.Visible = true;
                 AddButtonOperts.Visible = false;
                 DeleteButtonOperts.Visible = true;
                 NameOpertsIns.Visible = false;
                 Sel_Manufacture();
                 OpertsLV.DataBind();
                 RadioButtonGroups.SelectedValue = OpertsLV.DataKeys[0].Values[3].ToString();
                 try
                 {
                     if (RadioButtonGroups.SelectedItem.Selected == true)
                     {
                         FlagGroupsList.Checked = false;
                         FlagGroupsList.Visible = true;
                     }
                 }
                 catch { }
             }
             else
             {
                 NameOpertsIns.Visible = true;
                 ChecOpertsIns.Visible = true;
                 UpdateButtonOperts.Visible = false;
                 AddButtonOperts.Visible = true;
                 DeleteButtonOperts.Visible = false;
             }
         }
        protected void CommandBtn_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Insert":
                    SelectOneOperts.InsertParameters["NameOperts"].DefaultValue = NameOpertsIns.Text.ToString();
                    SelectOneOperts.Insert();
                    break;
                default:
                    Msg.Text = "Command name not recogized.";
                    break;
            }
        }
        protected void Select_Manufacture(Object sender, EventArgs e)
        { Sel_Manufacture();}
        protected void Sel_Manufacture()
        {
             string StrID= "" ;
             int i = 0;
 
             foreach (ListViewItem dli in ManufactureCheckBox.Items)
             {
                 CheckBox cb = (CheckBox)dli.FindControl("IDCheck");
                 if (cb.Checked == true && i == 0)
                 { StrID += cb.ToolTip.ToString(); i += 1; }
                 if (cb.Checked == true && i != 0)
                 { StrID += ", " + cb.ToolTip.ToString();}
             }
             OpertsObjectDataSource.SelectParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
             OpertsObjectDataSource.SelectParameters["ID_Manufacture"].DefaultValue = StrID;
             //Groups.DataBind();
        }
        protected void OpertsDataSource_OnUpdated(object sender, EventArgs e)
         {
            foreach (ListViewItem dli in OpertsLV.Items)
             {
                 TextBox tcd = (TextBox)dli.FindControl("NameOperts");
                 SelectOneOperts.UpdateParameters["NameOperts"].DefaultValue = tcd.Text.ToString();
                 CheckBox cb = (CheckBox)dli.FindControl("ChecOperts");
                 if (cb.Checked == true)
                 {
                     SelectOneOperts.UpdateParameters["ChecOperts"].DefaultValue = "1";
                 }
                 else { SelectOneOperts.UpdateParameters["ChecOperts"].DefaultValue = "0"; }
             }
             int i = 0;
             foreach (ListViewItem dli in ManufactureCheckBox.Items)
             {

                 CheckBox cd = (CheckBox)dli.FindControl("IDCheck");
                 if (cd.Checked == true && (bool)ManufactureCheckBox.DataKeys[i].Values[5] == false)
                 {
                     ObjectDataOpertsCheck.InsertParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
                     ObjectDataOpertsCheck.InsertParameters["ID_Link"].DefaultValue = ManufactureCheckBox.DataKeys[i].Values[4].ToString();
                     ObjectDataOpertsCheck.Insert();
                 }
                 if (cd.Checked == false && (bool)ManufactureCheckBox.DataKeys[i].Values[5] == true)
                 {
                     ObjectDataOpertsCheck.DeleteParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
                     ObjectDataOpertsCheck.DeleteParameters["ID_Link"].DefaultValue = ManufactureCheckBox.DataKeys[i].Values[4].ToString();
                     ObjectDataOpertsCheck.Delete();
                 }
                 i += 1;
             }
             Msg.Text = RadioButtonGroups.SelectedValue.ToString();// +"  " + RadioButtonGroups.SelectedItem.ToString();
             SelectOneOperts.UpdateParameters["ID_Operts_Group"].DefaultValue = RadioButtonGroups.SelectedValue.ToString();
             SelectOneOperts.UpdateParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
             SelectOneOperts.UpdateMethod = "UpdateOperts";
             SelectOneOperts.Update();

             Server.Transfer("Operts.aspx", false);
         }
        protected void OpertsDataSource_OnInserted(object sender, ObjectDataSourceStatusEventArgs e)
         {
            LoopCheckedAndInsert(e.ReturnValue.ToString());
            Server.Transfer("Operts.aspx", false);
         }
        protected void OpertsDataSource_OnDeleteed(object sender, EventArgs e)
         {
             SelectOneOperts.DeleteParameters["ID_Operts"].DefaultValue = (string)(Session["ID_Operts"]);
             SelectOneOperts.Delete();
             Server.Transfer("Operts.aspx", false);
         }
        protected void LoopCheckedAndInsert(string ID_Operts)
         {
             int i = 0;
             foreach (ListViewItem dli in ManufactureCheckBox.Items)
             {
                 CheckBox cd = (CheckBox)dli.FindControl("IDCheck");
                 if (cd.Checked == true)
                 {
                     ObjectDataOpertsCheck.InsertParameters["ID_Operts"].DefaultValue = ID_Operts;
                     ObjectDataOpertsCheck.InsertParameters["ID_Link"].DefaultValue = ManufactureCheckBox.DataKeys[i].Values[4].ToString();
                     ObjectDataOpertsCheck.Insert();
                 }
                 i+=1;
             }
        }
        protected void Clear_Groups(object sender, EventArgs e)
        {
            //RadioButtonGroups.SelectedItem.Selected = false;
            FlagGroupsList.Checked = false;
            FlagGroupsList.Visible = false;
        }
    }
}