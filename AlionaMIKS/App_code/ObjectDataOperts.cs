using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Samples.AspNet.ObjectDataOperts
{
    //
    //  Northwind Employee Data Factory
    //

    public class OpertsData
    {

        private string _connectionString;


        public OpertsData()
        {
            Initialize();
        }


        public void Initialize()
        {
            // Initialize data source. Use "Northwind" connection string from configuration.

            if (ConfigurationManager.ConnectionStrings["ApplicationServices"] == null ||
                ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString.Trim() == "")
            {
                throw new Exception("A connection string named 'ApplicationServices' with a valid connection string " +
                                    "must exist in the <connectionStrings> configuration section for the application.");
            }

            _connectionString =
              ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        }


        // Select all employees.

        public DataTable GetAllOperts(string sortColumns, int startRecord, int maxRecords)
        {
            VerifySortColumns(sortColumns);

            string sqlCmd = "SELECT ID_Operts, NameOperts, MapMain FROM Operts ";

            if (sortColumns.Trim() == "")
                sqlCmd += "ORDER BY ID_Operts";
            else
                sqlCmd += "ORDER BY " + sortColumns;

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, startRecord, maxRecords, "Operts");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Operts"];
        }
        public DataTable GetAllsOperts(int ID_Operts, string ID_Manufacture)
        {
            string sqlCmd = "SELECT distinct o.ID_Operts as ID_Operts , o.NameOperts, o.MapMain,  " +
                            " Cast(case when o.ChecOperts >0 then 'True' else 'False' end as bit)  as ChecOperts " +
                            " FROM Operts o "+
                            " Left join Operts_List as ol on o.ID_Operts=ol.ID_Operts and ol.Link_NameTable='Manufacture' "+
                            " Left join Manufacture as m on ol.ID_Link= m.ID_Manufacture " ;
            sqlCmd += " where 1=1 and o.ChecOperts=1 ";
            if (ID_Operts != 0)
            { sqlCmd += " and o.ID_Operts<>@ID_Operts "; }
            if (ID_Manufacture != null)
            { sqlCmd += " and m.ID_Manufacture in ( "+ID_Manufacture+" ) "; }

            sqlCmd += " ORDER BY o.ID_Operts ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            da.SelectCommand.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;
//            if (ID_Manufacture != null)
//            { da.SelectCommand.Parameters.Add("@ID_Manufacture", SqlDbType.VarChar, 50).Value = ID_Manufacture; }
            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Operts");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Operts"];
        }

        public DataTable GetOneOperts(int ID_Operts)
        {
            string sqlCmd = "SELECT ID_Operts, NameOperts, MapMain,  "+
                            " Cast(case when ChecOperts >0 then 'True' else 'False' end as bit)  as ChecOperts , ID_Operts_Group " +
                            " FROM Operts  ";
            sqlCmd += " where ID_Operts=@ID_Operts ";
            sqlCmd += " ORDER BY ID_Operts";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);
            
            da.SelectCommand.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;
            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds,  "Operts");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Operts"];
        }

        public DataTable GetOpertsTempGrid(int ID_Operts)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            string sqlCmd = "SELECT o.ID_Operts, o.NameOperts, o.MapMain, fr.id as ID_FilesRelation, fr.ID_Files, f.fileName, f.fileType    " +
                "  FROM Operts as o  " +
                " Left join FilesRelation as fr on fr.ID_Table = o.ID_Operts and fr.NameTable = 'Operts'" +
                " Left join files as f on f.ID = fr.ID_Files " +
                                 "WHERE b.ID_Operts = @ID_Operts";

            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);
            da.SelectCommand.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Operts");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Operts"];
        }

        public int SelectCount()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Operts", conn);

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
                    case "id_operts":
                        break;
                    case "nameoperts":
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
        public DataTable GetOperts(int ID_Operts)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da =
              new SqlDataAdapter("SELECT ID_Operts, NameOperts,MapMain " +
                                 "  FROM Operts WHERE ID_Operts = @ID_Operts", conn);
            da.SelectCommand.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Operts");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Operts"];
        }

        // Delete the Otdelen by ID_Operts.

        public int DeleteOperts(int ID_Operts)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Operts WHERE ID_Operts = @ID_Operts", conn);
            cmd.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;
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
        public int DeleteOpertsCheck(int ID_Operts, int ID_Link, string Link_NameTable)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Operts_List WHERE ID_Operts = @ID_Operts and ID_Link=@ID_Link and Link_NameTable=@Link_NameTable ", conn);
            cmd.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;
            cmd.Parameters.Add("@ID_Link", SqlDbType.Int).Value = ID_Link;
            cmd.Parameters.Add("@Link_NameTable", SqlDbType.VarChar, 50).Value = Link_NameTable;
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

        public int UpdateOperts(int ID_Operts, string NameOperts, int ChecOperts, int ID_Operts_Group)
        {
            if (String.IsNullOrEmpty(NameOperts))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Operts " +
                                                "  SET NameOperts=@NameOperts , ChecOperts=@ChecOperts, ID_Operts_Group=@ID_Operts_Group" +
                                                 "  WHERE ID_Operts=@ID_Operts", conn);

            cmd.Parameters.Add("@NameOperts", SqlDbType.VarChar, 50).Value = NameOperts;
            cmd.Parameters.Add("@ChecOperts", SqlDbType.Int).Value = ChecOperts;
            cmd.Parameters.Add("@ID_Operts_Group", SqlDbType.Int).Value = ID_Operts_Group;
            cmd.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;

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

        public int InsertOperts(string NameOperts, int ChecOperts)

        {
            if (String.IsNullOrEmpty(NameOperts))
                throw new ArgumentException("NameOperts cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Operts " +
                                                "  (NameOperts , ChecOperts) " +
                                                "  Values(@NameOperts, @ChecOperts); " +
                                                "SELECT @ID_Operts = SCOPE_IDENTITY()", conn);
            cmd.Parameters.Add("@NameOperts", SqlDbType.VarChar, 150).Value = NameOperts;
            cmd.Parameters.Add("@ChecOperts", SqlDbType.Int).Value = ChecOperts;
            SqlParameter p = cmd.Parameters.Add("@ID_Operts", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_Operts = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_Operts = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_Operts;
        }
        public int InsertOpertsCheck(int ID_Operts, int ID_Link, string Link_NameTable)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Operts_List " +
                                                "  (ID_Operts,ID_Link,Link_NameTable) " +
                                                "  Values(@ID_Operts, @ID_Link, @Link_NameTable); " +
                                                "SELECT @ID_List_Operts = SCOPE_IDENTITY()", conn);

            cmd.Parameters.Add("@ID_Operts", SqlDbType.Int).Value = ID_Operts;
            cmd.Parameters.Add("@ID_Link", SqlDbType.Int).Value = ID_Link;
            cmd.Parameters.Add("@Link_NameTable", SqlDbType.VarChar, 50).Value = Link_NameTable;
            SqlParameter p = cmd.Parameters.Add("@ID_List_Operts", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_Operts = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_Operts = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_Operts;
        }
           
        //
        // Methods that support Optimistic Concurrency checks.
        //

        // Delete the Otdelen by ID_Operts.

        public int DeleteOperts(string NameOperts, int original_ID, int original_MapMain, string original_NameOperts)
        {
            if (String.IsNullOrEmpty(original_NameOperts))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "DELETE FROM Operts WHERE ID_Operts = @original_ID " +
                            " AND NameOperts = @original_NameOperts ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameOperts", SqlDbType.VarChar, 50).Value = NameOperts;
            cmd.Parameters.Add("@original_ID", SqlDbType.Int).Value = original_ID;
            cmd.Parameters.Add("@original_MapMain", SqlDbType.Int).Value = original_MapMain;
            cmd.Parameters.Add("@original_NameOperts", SqlDbType.VarChar, 10).Value = original_NameOperts;

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
        // Update the Employee by original ID_Operts.

        public int UpdateOperts(string NameOperts, string original_NameOperts, int original_ID_Operts, int original_ChecOperts, int ChecOperts)
        {
            if (String.IsNullOrEmpty(NameOperts))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "UPDATE Operts " +
                            "  SET NameOperts = @NameOperts" +
                            "  WHERE ID_Operts = @original_ID_Operts " +
                            " AND NameOperts = @original_NameOperts";
                                                              
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameOperts", SqlDbType.VarChar, 150).Value = NameOperts;
            cmd.Parameters.Add("@ChecOperts", SqlDbType.Int).Value = ChecOperts;
            cmd.Parameters.Add("@original_ID_Operts", SqlDbType.Int).Value = original_ID_Operts;
            cmd.Parameters.Add("@original_NameOperts", SqlDbType.VarChar, 50).Value = original_NameOperts;

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