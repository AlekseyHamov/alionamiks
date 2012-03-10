using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Samples.AspNet.ObjectDataManufacture
{
    //
    //  Northwind Employee Data Factory
    //

    public class ManufactureData
    {

        private string _connectionString;
        public ManufactureData()
        {
            Initialize();
        }
        public void Initialize()
        {
            // Initialize data source. Use "Northwind" connection string from configuration.

            if (ConfigurationManager.ConnectionStrings["ApplicationServices"] == null ||
                ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString.Trim() == "")
            {
                throw new Exception("A connection string named 'DispOKBConnectionString1' with a valid connection string " +
                                    "must exist in the <connectionStrings> configuration section for the application.");
            }

            _connectionString =
              ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        }
        // Select all employees.
        public DataTable GetAll(string str_ID, string ID_Unit, string sortColumns, int startRecord, int maxRecords)
        {
            VerifySortColumns(sortColumns);

            string sqlCmd = " SELECT distinct Manufacture.ID_Manufacture, Manufacture_list.Manufacture as Parent_ID, "+
                " Manufacture.NameManufacture, Manufacture.Description, Manufacture.ID_Unit, Unit.NameUnit, Manufacture.CheckLog " +
                " FROM Manufacture " +
                " Left join Unit on Unit.ID_Unit=Manufacture.ID_Unit " +
                " Left join Manufacture_list on Manufacture_list.Manufacture_Spares=Manufacture.ID_Manufacture ";

            sqlCmd += " where 1=1 ";
            try
            {
                if (str_ID.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Manufacture in ( " + str_ID + " ) "; }
            }
            catch { }
            try
            {
                if (ID_Unit.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Unit=" + ID_Unit + " "; }
            }
            catch { }

            if (sortColumns.Trim() == "")
                sqlCmd += "ORDER BY Manufacture.ID_Manufacture DESC ";
            else
                sqlCmd += "ORDER BY " + sortColumns;

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();
            try
            {
                maxRecords = 1;
                conn.Open();
                da.Fill(ds, startRecord, maxRecords, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];

        }
        public DataTable GetAllImage(string str_ID, string ID_Unit)
        {
            string sqlCmd = " SELECT distinct Manufacture.ID_Manufacture,Manufacture_list.Manufacture as Parent_ID, " +
                " Manufacture.NameManufacture, Manufacture.Description, Manufacture.ID_Unit, Unit.NameUnit, Manufacture.CheckLog, " +
                " fr.ID, fr.ID_files, fr.ID_Table, fr.NameTable, f.fileName, f.fileType " +
                " FROM Manufacture " +
                " Left join FilesRelation as fr on fr.NameTable ='Manufacture' and fr.ID_Table=Manufacture.ID_Manufacture " +
                " Left join Files as f on f.ID=fr.ID_files " +
                " Left join Unit on Unit.ID_Unit=Manufacture.ID_Unit " +
                " Left join Manufacture_list on Manufacture_list.Manufacture_Spares=Manufacture.ID_Manufacture ";

            sqlCmd += " where 1=1 and Manufacture_list.Manufacture is null ";
            try
            {
                if (str_ID.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Manufacture in ( " + str_ID + " ) "; }
            }
            catch { }
            try
            {
                if (ID_Unit.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Unit=" + ID_Unit + " "; }
            }
            catch { }

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];

        }
        public DataTable GetManufactureImage(string str_ID, string ID_Unit, int ID_Operts)
        {
            string sqlCmd = " SELECT distinct Manufacture.ID_Manufacture,Manufacture_list.Manufacture as Parent_ID, " +
            " Manufacture.NameManufacture, Manufacture.Description, Manufacture.ID_Unit, Unit.NameUnit, Manufacture.CheckLog, " +
            " fr.ID, fr.ID_files, fr.ID_Table, fr.NameTable, f.fileName, f.fileType, " +
            " Cast(case when ol.ID_List_Operts >0 then 'True' else 'False' end as bit)  as Oplist " +
            " FROM Manufacture " +
            " Left join FilesRelation as fr on fr.NameTable ='Manufacture' and fr.ID_Table=Manufacture.ID_Manufacture " +
            " Left join Files as f on f.ID=fr.ID_files " +
            " Left join Unit on Unit.ID_Unit=Manufacture.ID_Unit " +
            " Left join Manufacture_list on Manufacture_list.Manufacture_Spares=Manufacture.ID_Manufacture "+
            " Left join Operts_list as ol on ol.ID_Link=Manufacture.ID_Manufacture and ol.Link_NameTable= 'Manufacture' and ol.ID_Operts in ("+ ID_Operts+") " ;

            sqlCmd += " where 1=1 and Manufacture_list.Manufacture is not null ";
            try
            {
                if (str_ID.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Manufacture in ( " + str_ID + " ) "; }
            }
            catch { }
            try
            {
                if (ID_Unit.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Unit=" + ID_Unit + " "; }
            }
            catch { }

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];

        }
        public DataTable GetView()
        {
            string sqlCmd = " Select ID, Parent_ID, Text from dbo.View_Dv_list";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];

        }
        public DataTable GetForCheck(int ID_Manufacture_Spares)
        {
            string sqlCmd = " Select dv_l.ID_DV_List, dv.NameManufacture, dv_l.Manufacture from Manufacture_list as dv_l " +
                            "Left Join Manufacture as dv on dv.ID_Manufacture=dv_l.Manufacture " +
                            "Left Join Manufacture as dv_s on dv_s.ID_Manufacture=dv_l.Manufacture_Spares " +
                            "Where dv_l.Manufacture is not null and dv_l.Manufacture_Spares = @ID_Manufacture_Spares ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);
            da.SelectCommand.Parameters.Add("@ID_Manufacture_Spares", SqlDbType.Int).Value = ID_Manufacture_Spares;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();
                da.Fill(ds, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];

        }
        public int SelectCount(string str_ID, string ID_Unit)
        {

            string sqlCmd = "";
            sqlCmd = "SELECT COUNT(*) FROM Manufacture ";
            sqlCmd += " where 1=1 ";
            try
            {
                if (str_ID.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Manufacture in ( " + str_ID + " ) "; }
            }
            catch { }
            try
            {
                if (ID_Unit.Trim() != "")
                { sqlCmd += " and Manufacture.ID_Unit=" + ID_Unit + " "; }
            }
            catch { }

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            int result = 0;

            try
            {
                conn.Open();

                result = (int)cmd.ExecuteScalar();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        //////////
        // Verify that only valid columns are specified in the sort expression to avoid a SQL Injection attack.
        private void VerifySortColumns(string sortColumns)
        {
            if (sortColumns.ToLowerInvariant().EndsWith(" desc"))
                sortColumns = sortColumns.Substring(0, sortColumns.Length - 5);

            string[] columnNames = sortColumns.Split(',');

            foreach (string columnName in columnNames)
            {
                switch (columnName.Trim().ToLowerInvariant())
                {
                    case "id_Manufacture":
                        break;
                    case "nameManufacture":
                        break;
                    case "":
                        break;
                    default:
                        throw new ArgumentException("SortColumns contains an invalid column name.");
                        break;
                }
            }
        }

        // Select an Otdelen.
        public DataTable GetOneRecord(int ID_Manufacture)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da =
              new SqlDataAdapter("SELECT ID_Manufacture, NameManufacture, Description, ID_Unit " +
                                 "  FROM Manufacture WHERE ID_Manufacture = @ID_Manufacture", conn);
            da.SelectCommand.Parameters.Add("@ID_Manufacture", SqlDbType.Int).Value = ID_Manufacture;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Manufacture");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Manufacture"];
        }

        // Delete the Otdelen by ID_Room.
        public int DeleteRecord(int ID_Manufacture, DBNull  Parent_ID)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Manufacture WHERE ID_Manufacture = @ID_Manufacture", conn);
            cmd.Parameters.Add("@ID_Manufacture", SqlDbType.Int).Value = ID_Manufacture;
            int result = 0;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        // Update the Otdelen by original ID_Otdelen.
        public int UpdateRecord(int ID_Manufacture, string NameManufacture, string Description, int ID_Unit)
        {
            if (String.IsNullOrEmpty(NameManufacture))
                throw new ArgumentException("Необходимо заполнять поле Наименование комнаты");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Manufacture " +
                                                "  SET NameManufacture=@NameManufacture, Description=@Description, ID_Unit=@ID_Unit " +
                                                 "  WHERE ID_Manufacture=@ID_Manufacture ", conn);

            cmd.Parameters.Add("@NameManufacture", SqlDbType.VarChar, 50).Value = NameManufacture;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = Description;
            cmd.Parameters.Add("@ID_Unit", SqlDbType.Int).Value = ID_Unit;
            cmd.Parameters.Add("@ID_Manufacture", SqlDbType.Int).Value = ID_Manufacture;
            int result = 0;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        public int UpdateRecord_Manufacture_list(int ID_Manufacture, int ID_NewManufacture)
        {
            // if (Int.IsNullOrEmpty(ID_NewManufacture))
            //     throw new ArgumentException("Новая запись не создана");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Manufacture_list " +
                                                "  SET Manufacture=@ID_NewManufacture " +
                                                 "  WHERE Manufacture_Spares=@ID_Manufacture ", conn);

            cmd.Parameters.Add("@ID_NewManufacture", SqlDbType.Int).Value = ID_NewManufacture;
            cmd.Parameters.Add("@ID_Manufacture", SqlDbType.Int).Value = ID_Manufacture;
            int result = 0;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        public int InsertRecord_Manufacture_list(int ID_Manufacture, int ID_NewManufacture)
        {
            //if (String.IsNullOrEmpty(NameManufacture))
            //    throw new ArgumentException("NameRoom cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Manufacture_list " +
                                                "  (Manufacture,Manufacture_Spares) " +
                                                "  Values(@ID_NewManufacture, @ID_Manufacture); " +
                                                "SELECT @ID_DV_List = SCOPE_IDENTITY()", conn);
            cmd.Parameters.Add("@ID_Manufacture", SqlDbType.Int).Value = ID_Manufacture;
            cmd.Parameters.Add("@ID_NewManufacture", SqlDbType.Int).Value = ID_NewManufacture;
            SqlParameter p = cmd.Parameters.Add("@ID_DV_List", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_DV_List = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_DV_List = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_DV_List;
        }
        public int DeleteRecord_Manufacture_list(int ID_Manufacture, int ID_NewManufacture)
        {

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Delete_Manufacture_list";
            cmd.Parameters.Add("@Manufacture_Spares", SqlDbType.Int).Value = ID_NewManufacture;
            cmd.Parameters.Add("@Manufacture", SqlDbType.Int).Value = ID_Manufacture;
            int result = 0;
            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        // Insert an Otdelen.
        public int InsertRecord(string NameManufacture, string Description, int ID_Unit)
        {
            if (String.IsNullOrEmpty(NameManufacture))
                throw new ArgumentException("NameRoom cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Manufacture " +
                                                "  (NameManufacture,Description, ID_unit) " +
                                                "  Values(@NameManufacture, @Description, @ID_Unit); " +
                                                "SELECT @ID_Manufacture = SCOPE_IDENTITY()", conn);

            cmd.Parameters.Add("@NameManufacture", SqlDbType.VarChar, 50).Value = NameManufacture;
            cmd.Parameters.Add("@ID_Unit", SqlDbType.Int).Value = ID_Unit;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = Description;
            SqlParameter p = cmd.Parameters.Add("@ID_Manufacture", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_Manufacture = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_Manufacture = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_Manufacture;
        }

        //
        // Methods that support Optimistic Concurrency checks.
        //

        // Delete the Otdelen by ID_Room.
        public int DeleteRecord(string NameManufacture, string Description, int original_ID, int Parent_id,
                                string original_NameManufacture, string original_Description, int original_Parent_id)
        {
            if (String.IsNullOrEmpty(original_NameManufacture))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "DELETE FROM Manufacture WHERE ID_Manufacture = @original_ID " +
                            " AND NameManufacture = @original_NameManufacture ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameManufacture", SqlDbType.VarChar, 50).Value = NameManufacture;
            cmd.Parameters.Add("@original_ID", SqlDbType.Int).Value = original_ID;
            cmd.Parameters.Add("@original_NameManufacture", SqlDbType.VarChar, 10).Value = original_NameManufacture;

            int result = 0;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        // Update the Employee by original ID_Room.

        public int UpdateRecord(string NameManufacture, string Description
                                , string original_NameManufacture, int original_ID_Manufacture, string original_Description)
        {
            if (String.IsNullOrEmpty(NameManufacture))
                throw new ArgumentException("Необходимо заполнить поле Наименование комнаты.");

            string sqlCmd = "UPDATE Manufacture " +
                            "  SET NameManufacture = @NameManufacture , Description=@Description  " +
                            "  WHERE ID_Manufacture = @original_ID_Manufacture " +
                            " AND NameManufacture = @original_NameManufacture";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameManufacture", SqlDbType.VarChar, 50).Value = NameManufacture;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 200).Value = Description;
            cmd.Parameters.Add("@original_ID_Manufacture", SqlDbType.Int).Value = original_ID_Manufacture;
            cmd.Parameters.Add("@original_NameManufacture", SqlDbType.VarChar, 50).Value = original_NameManufacture;
            cmd.Parameters.Add("@original_Description", SqlDbType.VarChar, 200).Value = original_Description;

            int result = 0;

            try
            {
                conn.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }
}