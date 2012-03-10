using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
namespace AlionaMIKS.Doc
{
    public partial class Timing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Msg.Text = "";
            if (IsPostBack == false)
            {
                Msg.Text = "";
            }
            if (IsPostBack == false && DateTime.Now.Hour < 8)
            {
                Msg.Text = "";
            }
            else if (IsPostBack == false && DateTime.Now.Hour >= 8)
            {
                Msg.Text = "";
            }
        }
        protected void CommandBtn_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Update":
                    ClaimObjectDataSource.Update();
                    break;
                case "Insert":
                    ClaimObjectDataSource.Insert();
                    break;
                case "Delete":
                    ClaimObjectDataSource.Delete();
                    break;
                case "VisibleButoonAdd":
                    Msg.Text = "Command name not recogized.";
                    break;
                default:
                    Msg.Text = "Command name not recogized.";
                    break;
            }

        }
        protected void CommandBtn_Click1(Object sender, EventArgs e)
        {
            ClaimObjectDataSource.Update();
        }
        protected void DetailsView_ItemInserted(Object sender, DetailsViewInsertedEventArgs e)
        {
            GridView.DataBind();
        }
        protected void DetailsView_ItemUpdated(Object sender, DetailsViewUpdatedEventArgs e)
        {
            GridView.DataBind();
        }
        protected void DetailsView_ItemDeleted(Object sender, DetailsViewDeletedEventArgs e)
        {
            GridView.DataBind();
        }
        protected void GridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Msg.Text = "Выбрана запись";
        }
        protected void DetailsObjectDataSource_OnInserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
        }
        protected void DetailsObjectDataSource_OnUpdated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if ((int)e.ReturnValue == 0)
                Msg.Text = "Employee was not deleted. Please try again.";
        }
        protected void DetailsObjectDataSource_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if ((int)e.ReturnValue == 0)
                Msg.Text = "Employee was not deleted. Please try again.";

        }
        // переход на др страницу
        protected void GetNextPage(object sender, System.EventArgs e)
        {
            Server.Transfer("UpdateTiming.aspx", true);
        }

    }
}