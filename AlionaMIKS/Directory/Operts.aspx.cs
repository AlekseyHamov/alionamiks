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
    public partial class Operts : System.Web.UI.Page
    {
        /*protected void Page_Init()
        { 
            ImageButtons.Click +=  new ImageClickEventHandler(ShowImageMapingPanel); 
        }*/
         public TextBox Coordinats ;
         protected void CommandBtn_Click(Object sender, CommandEventArgs e)
         {
             switch (e.CommandName)
             {
                 case "Update":
                     OpertsObjectDataSource.Update();
                     InsertButton.Visible = true;
                     break;
                 case "Insert":
                     OpertsObjectDataSource.Insert();
                     break;
                 case "Delete":
                     OpertsObjectDataSource.Delete();
                     break;
                 default:
                     Msg.Text = "Command name not recogized.";
                     break;
             }
         }

         protected void Page_Load(object sender, EventArgs e)
         {
             Msg.Text = "";
             Coordinats = new TextBox();
             UpdateButton.Visible = false;
             InsertButton.Visible = true;
             DeleteButton.Visible = false;
         }
         protected void _DetailsView_ItemUpdated(Object sender, DetailsViewUpdatedEventArgs e)
         {
             OpertsGridView.DataBind();
         }
         protected void DetailsView_ItemDeleted(Object sender, DetailsViewDeletedEventArgs e)
         {
             OpertsGridView.DataBind();
         }
         protected void GridView_OnSelectedIndexChanged(object sender, EventArgs e)
         {
             ModalPopupExtender1.Show();
             GridViewRow row = OpertsGridView.SelectedRow;
             TextBox2.Text = row.Cells[2].Text;
             UpdateButton.Visible = true;
             InsertButton.Visible = false;
             DeleteButton.Visible = true;
         }
         protected void OpertsDataSource_OnInserted(object sender, ObjectDataSourceStatusEventArgs e)
         {   
             string ID_Operts = e.ReturnValue.ToString();
         }
         protected void OpertsDataSource_OnUpdated(object sender, ObjectDataSourceStatusEventArgs e)
         {
             OpertsGridView.DataBind();
             if ((int)e.ReturnValue == 0)
                 Msg.Text = "Employee was not updated. Please try again.";
         }
         protected void OpertsDataSource_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
         {
             if ((int)e.ReturnValue == 0)
                 Msg.Text = "Employee was not deleted. Please try again.";

         }
    }
}