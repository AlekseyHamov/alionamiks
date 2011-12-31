using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Samples.AspNet.ObjectDataMachine
{
    //
    //  Northwind Employee Data Factory
    //

    public class MachineData
    {

        private string _connectionString;


        public MachineData()
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

        public DataTable GetAllMachine(string sortColumns, int startRecord, int maxRecords)
        {
            VerifySortColumns(sortColumns);

            string sqlCmd = "SELECT ID_Machine, NameMachine, MapMain FROM Machine  ";

            if (sortColumns.Trim() == "")
                sqlCmd += "ORDER BY ID_Machine";
            else
                sqlCmd += "ORDER BY " + sortColumns;

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, startRecord, maxRecords, "Machine");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Machine"];
        }

        public DataTable GetMachineTempGrid(int ID_Machine)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            string sqlCmd = "SELECT o.ID_Machine, o.NameMachine, o.MapMain, fr.id as ID_FilesRelation, fr.ID_Files, f.fileName, f.fileType    " +
                "  FROM Machine as o  " +
                " Left join FilesRelation as fr on fr.ID_Table = o.ID_Machine and fr.NameTable = 'Machine'" +
                " Left join files as f on f.ID = fr.ID_Files " +
                                 "WHERE b.ID_Machine = @ID_Machine";

            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);
            da.SelectCommand.Parameters.Add("@ID_Machine", SqlDbType.Int).Value = ID_Machine;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Machine");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Machine"];
        }

        public int SelectCount()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Machine", conn);

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
                    case "id_Machine":
                        break;
                    case "nameMachine":
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
        public DataTable GetMachine(int ID_Machine)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da =
              new SqlDataAdapter("SELECT ID_Machine, NameMachine,MapMain " +
                                 "  FROM Machine WHERE ID_Machine = @ID_Machine", conn);
            da.SelectCommand.Parameters.Add("@ID_Machine", SqlDbType.Int).Value = ID_Machine;

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, "Machine");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Machine"];
        }

        // Delete the Otdelen by ID_Machine.

        public int DeleteMachine(int ID_Machine)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Machine WHERE ID_Machine = @ID_Machine", conn);
            cmd.Parameters.Add("@ID_Machine", SqlDbType.Int).Value = ID_Machine;
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

        public int UpdateMachine(int ID_Machine, string NameMachine)
        {
            if (String.IsNullOrEmpty(NameMachine))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Machine " +
                                                "  SET NameMachine=@NameMachine" +
                                                 "  WHERE ID_Machine=@ID_Machine", conn);

            cmd.Parameters.Add("@NameMachine", SqlDbType.VarChar, 50).Value = NameMachine;
            cmd.Parameters.Add("@ID_Machine", SqlDbType.Int).Value = ID_Machine;

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

        public int InsertMachine(string NameMachine)

        {
            if (String.IsNullOrEmpty(NameMachine))
                throw new ArgumentException("NameMachine cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Machine " +
                                                "  (NameMachine) " +
                                                "  Values(@NameMachine); " +
                                                "SELECT @ID_Machine = SCOPE_IDENTITY()", conn);

            cmd.Parameters.Add("@NameMachine", SqlDbType.VarChar, 50).Value = NameMachine;
            SqlParameter p = cmd.Parameters.Add("@ID_Machine", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_Machine = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_Machine = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_Machine;
        }
           
        //
        // Methods that support Optimistic Concurrency checks.
        //

        // Delete the Otdelen by ID_Machine.

        public int DeleteMachine(string NameMachine, int original_ID, int original_MapMain, string original_NameMachine)
        {
            if (String.IsNullOrEmpty(original_NameMachine))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "DELETE FROM Machine WHERE ID_Machine = @original_ID " +
                            " AND NameMachine = @original_NameMachine ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameMachine", SqlDbType.VarChar, 50).Value = NameMachine;
            cmd.Parameters.Add("@original_ID", SqlDbType.Int).Value = original_ID;
            cmd.Parameters.Add("@original_MapMain", SqlDbType.Int).Value = original_MapMain;
            cmd.Parameters.Add("@original_NameMachine", SqlDbType.VarChar, 10).Value = original_NameMachine;

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
        // Update the Employee by original ID_Machine.

        public int UpdateMachine(string NameMachine, string original_NameMachine, int original_ID_Machine)
        {
            if (String.IsNullOrEmpty(NameMachine))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "UPDATE Machine " +
                            "  SET NameMachine = @NameMachine" +
                            "  WHERE ID_Machine = @original_ID_Machine " +
                            " AND NameMachine = @original_NameMachine";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@NameMachine", SqlDbType.VarChar, 50).Value = NameMachine;
            cmd.Parameters.Add("@original_ID_Machine", SqlDbType.Int).Value = original_ID_Machine;
            cmd.Parameters.Add("@original_NameMachine", SqlDbType.VarChar, 50).Value = original_NameMachine;

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