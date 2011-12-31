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
    public partial class Machine : System.Web.UI.Page
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
                     MachineObjectDataSource.Update();
                     InsertButton.Visible = true;
                     break;
                 case "Insert":
                     MachineObjectDataSource.Insert();
                     break;
                 case "Delete":
                     MachineObjectDataSource.Delete();
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
             MachineGridView.DataBind();
         }
         protected void DetailsView_ItemDeleted(Object sender, DetailsViewDeletedEventArgs e)
         {
             MachineGridView.DataBind();
         }
         protected void GridView_OnSelectedIndexChanged(object sender, EventArgs e)
         {
             ModalPopupExtender1.Show();
             GridViewRow row = MachineGridView.SelectedRow;
             TextBox2.Text = row.Cells[2].Text;
             UpdateButton.Visible = true;
             InsertButton.Visible = false;
             DeleteButton.Visible = true;
             Load_Image();
             AddImge.Visible = true;
         }
         protected void MachineDataSource_OnInserted(object sender, ObjectDataSourceStatusEventArgs e)
         {   
             string ID_Machine = e.ReturnValue.ToString();
         }
         protected void MachineDataSource_OnUpdated(object sender, ObjectDataSourceStatusEventArgs e)
         {
             MachineGridView.DataBind();
             if ((int)e.ReturnValue == 0)
                 Msg.Text = "Employee was not updated. Please try again.";
         }
         protected void MachineDataSource_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
         {
             if ((int)e.ReturnValue == 0)
                 Msg.Text = "Employee was not deleted. Please try again.";

         }
         // работа с картинками
         protected void LWImage_SelectedIndexChanged(Object sender, EventArgs e)
         {
             ImgButOne.ImageUrl = "~/Image_Data/" + LWImage.DataKeys[LWImage.SelectedIndex].Values[2].ToString() + "_" + LWImage.DataKeys[LWImage.SelectedIndex].Values[1].ToString() + "." + LWImage.DataKeys[LWImage.SelectedIndex].Values[3].ToString();
             ImgMapOne.ImageUrl = "~/Image_Data/" + LWImage.DataKeys[LWImage.SelectedIndex].Values[2].ToString() + "_" + LWImage.DataKeys[LWImage.SelectedIndex].Values[1].ToString() + "." + LWImage.DataKeys[LWImage.SelectedIndex].Values[3].ToString();
             ImageChildren.DataBind();
             ImgMapOne.HotSpots.Clear();
             for (int i = 0; i < ImageChildren.Rows.Count; i++)
             {
                 PolygonHotSpot Ph = new PolygonHotSpot();
                 Ph.AlternateText = ImageChildren.DataKeys[i].Values[3].ToString();
                 Ph.Coordinates = ImageChildren.DataKeys[i].Values[2].ToString();
                 ImgMapOne.HotSpots.Add(Ph);
             }
             ModalImageMaping.Show();
         }
         protected void Image_OnInserted(Object sender, EventArgs e)
         {
             string ID_Table = MachineGridView.SelectedValue.ToString();
             string strFileName = ImageFile.PostedFile.ContentType;
             strFileName = System.IO.Path.GetFileName(strFileName);
             ImageFile.PostedFile.SaveAs(Server.MapPath("../Image_Data/") + "Temp." + strFileName);
             string photoFilePath = Server.MapPath("../Image_Data/") + "Temp." + strFileName;
             ImageObjectDataSource.InsertParameters.Clear();
             ImageObjectDataSource.InsertParameters.Add("ID_Table", ID_Table);
             ImageObjectDataSource.InsertParameters.Add("fileType", strFileName);
             ImageObjectDataSource.InsertParameters.Add("photoFilePath", photoFilePath);
             ImageObjectDataSource.InsertParameters.Add("NameTable", "Machine");
             ImageObjectDataSource.Insert();
         }
         protected void ImageDataSource_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
         {
             if ((int)e.ReturnValue == 0)
                 Msg.Text = "Employee was not deleted. Please try again.";

         }
         protected void MapRelation_Click(object sender, EventArgs e)
         {
             string photoFilePath = Server.MapPath("../Image_Data/");
             ObjectDataTempGrig.SelectParameters.Clear();
             ObjectDataTempGrig.SelectParameters.Add("NameTable", "Machine");
             ObjectDataTempGrig.SelectParameters.Add("ID_Table", MachineGridView.DataKeys[MachineGridView.SelectedIndex].Values[1].ToString());

             MapText.Text = MachineGridView.Rows[MachineGridView.SelectedIndex].Cells[2].Text;
             //      GridView TempGrid = new GridView();
             TempGrid.DataSourceID = "ObjectDataTempGrig";
             TempGrid.DataKeyNames = new string[] { "ID_Table", "ID_Files", "fileName", "fileType" };
             //        ImageMapingPanel.Controls.Add(TempGrid);
             TempGrid.Visible = false;
             TempGrid.DataBind();
             for (int i = 0; i < TempGrid.Rows.Count; i++)
             {
                 if (!File.Exists(photoFilePath + TempGrid.DataKeys[i].Values[2].ToString() + "_" + TempGrid.DataKeys[i].Values[1].ToString() + "." + TempGrid.DataKeys[i].Values[3].ToString()))
                 {
                     ImageFilesObjectDataSource.SelectMethod = "TestGetSqlBytes";
                     ImageFilesObjectDataSource.SelectParameters.Clear();
                     ImageFilesObjectDataSource.SelectParameters.Add("documentID", TempGrid.DataKeys[i].Values[1].ToString());
                     ImageFilesObjectDataSource.SelectParameters.Add("filePath", photoFilePath);
                     ImageFilesObjectDataSource.Select();
                 }
                 ImgButOne.ImageUrl = "~/Image_Data/" + TempGrid.DataKeys[i].Values[2].ToString() + "_" + TempGrid.DataKeys[i].Values[1].ToString() + "." + TempGrid.DataKeys[i].Values[3].ToString();
                 ImgMapOne.ImageUrl = "~/Image_Data/" + TempGrid.DataKeys[i].Values[2].ToString() + "_" + TempGrid.DataKeys[i].Values[1].ToString() + "." + TempGrid.DataKeys[i].Values[3].ToString();
                 break;
             }
             ChangeMap.Visible = true;
             /*            Image img =
                             Image.FromFile(Server.MapPath(ImgMapOne.ImageUrl));
                         if (img.Height > img.Width)
                         {
                             ImageBM.Style.Add("height", "500px");
                         }
                         else { ImageBM.Style.Add("height", "auto"); }
             */
             ModalImageMaping.Show();
         }
         protected void ImageButton_Click(object sender, ImageClickEventArgs e)
         {
             Msg.Text = "You clicked the ImageButton control at the coordinates: (" +
                           e.X.ToString() + ", " + e.Y.ToString() + ")";
             OX.Text = e.X.ToString();
             OY.Text = e.Y.ToString();
             if (Coordin.Text == "")
             {
                 Coordin.Text = OX.Text + "," + OY.Text;
             }
             else
             {
                 Coordin.Text += "," + OX.Text + "," + OY.Text;
             }
             ModalImageMaping.Show();
         }
         protected void ChangeMap_CheckedChanged(object sender, EventArgs e)
         {
             Coordin.Text = "";
             if (ChangeMap.Checked == false)
             {
                 ImgButOne.Visible = false;
                 ImgMapOne.Visible = true;
                 UnitMap.Visible = false;
             }
             else
             {
                 ImgButOne.Visible = true;
                 ImgMapOne.Visible = false;
                 UnitMap.Visible = true;
             }
             ModalImageMaping.Show();
         }
         protected void DelCoordinate_Click(Object sender, EventArgs e)
         {
             ChangeMap.Visible = false;

             ImageFilesObjectDataSource.InsertMethod = "AddFileCoordinate";
             ImageFilesObjectDataSource.InsertParameters.Clear();
             ImageFilesObjectDataSource.InsertParameters.Add("Coordinate", Coordin.Text);

             ImageFilesObjectDataSource.InsertParameters.Add("ID_Files", TempGrid.DataKeys[0].Values[1].ToString());

             ImageFilesObjectDataSource.InsertParameters.Add("AlternateText", MapText.Text.ToString());
             ImageFilesObjectDataSource.InsertParameters.Add("ID_UrlTable", MachineGridView.SelectedValue.ToString());
             ImageFilesObjectDataSource.InsertParameters.Add("NameUrlTable", "Machine");
             ImageFilesObjectDataSource.Insert();
             ImageChildren.DataBind();
             ImgMapOne.HotSpots.Clear();
             for (int i = 0; i < ImageChildren.Rows.Count; i++)
             {
                 PolygonHotSpot Ph = new PolygonHotSpot();
                 Ph.AlternateText = ImageChildren.DataKeys[i].Values[3].ToString();
                 Ph.Coordinates = ImageChildren.DataKeys[i].Values[2].ToString();
                 ImgMapOne.HotSpots.Add(Ph);
             }
             Coordin.Text = "";
             ChangeMap.Checked = false;
             ImgButOne.Visible = false;
             ImgMapOne.Visible = true;
             UnitMap.Visible = false;
             ModalImageMaping.Show();
         }
         private void Load_Image()
         {
             string photoFilePath = Server.MapPath("../Image_Data/");
             LWImage.DataBind();
             MapPage.Visible = false;
             for (int i = 0; i < LWImage.Rows.Count; i++)
             {
                 if (!File.Exists(photoFilePath + LWImage.DataKeys[i].Values[2].ToString() + "_" + LWImage.DataKeys[i].Values[1].ToString() + "." + LWImage.DataKeys[i].Values[3].ToString()))
                 {
                     ImageFilesObjectDataSource.SelectMethod = "TestGetSqlBytes";
                     ImageFilesObjectDataSource.SelectParameters.Clear();
                     ImageFilesObjectDataSource.SelectParameters.Add("documentID", LWImage.DataKeys[i].Values[1].ToString());
                     ImageFilesObjectDataSource.SelectParameters.Add("filePath", photoFilePath);
                     ImageFilesObjectDataSource.Select();
                 }
                 MapPage.Visible = true;
                 FileCoordimateDataSource.SelectParameters.Clear();
                 FileCoordimateDataSource.SelectParameters.Add("ID_files", LWImage.DataKeys[0].Values[1].ToString());
                 MapPage.ImageUrl = "~/Image_Data/" + LWImage.DataKeys[0].Values[2].ToString() + "_" + LWImage.DataKeys[0].Values[1].ToString() + "." + LWImage.DataKeys[0].Values[3].ToString();
             }
             MapPage.HotSpots.Clear();

             ImageChildren.DataBind();
             ImgMapOne.HotSpots.Clear();
             for (int i = 0; i < ImageChildren.Rows.Count; i++)
             {
                 PolygonHotSpot Ph = new PolygonHotSpot();
                 Ph.AlternateText = ImageChildren.DataKeys[i].Values[3].ToString();
                 Ph.Coordinates = ImageChildren.DataKeys[i].Values[2].ToString();
                 MapPage.HotSpots.Add(Ph);
             }
         }
    }
}