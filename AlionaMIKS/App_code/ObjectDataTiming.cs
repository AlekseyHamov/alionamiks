using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Samples.AspNet.ObjectDataTiming
{
    //
    //  Northwind Employee Data Factory
    //

    public class TimingData
    {

        private string _connectionString;


        public TimingData()
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

        public DataTable GetAll(string sortColumns, int startRecord, int maxRecords)
        {
            VerifySortColumns(sortColumns);

            string sqlCmd = "SELECT p.ID_Timing FROM Timing as p " +
                            "  ";

            if (sortColumns.Trim() == "")
                sqlCmd += "ORDER BY ID_Timing";
            else
                sqlCmd += "ORDER BY " + sortColumns;

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, conn);

            DataSet ds = new DataSet();

            try
            {
                conn.Open();

                da.Fill(ds, startRecord, maxRecords, "Timing");
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return ds.Tables["Timing"];
        }


        public int SelectCount()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Timing", conn);

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
                    case "id_timing":
                        break;
                    case "rem":
                        break;
                    case "":
                        break;
                    default:
                        throw new ArgumentException("SortColumns contains an invalid column name.");
                        break;
                }
            }
        }
   
        // Delete the Otdelen by ID_Person.
        public int DeleteRecord(int ID_Timing)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM Timing WHERE ID_Timing = @ID_Timing", conn);
            cmd.Parameters.Add("@ID_Timing", SqlDbType.Int).Value = ID_Timing;

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

        public int UpdateRecord(int ID_Timing, string Rem)
        {
            if (String.IsNullOrEmpty(Rem))
                throw new ArgumentException("Необходимо заполнять поле Наименование комнаты");
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE Timing " +
                                                "  SET Rem=@Rem " +
                                                 "  WHERE ID_Timing=@ID_Timing", conn);

            cmd.Parameters.Add("@Rem", SqlDbType.VarChar, 50).Value = Rem;
            cmd.Parameters.Add("@ID_Timing", SqlDbType.Int).Value = ID_Timing;
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

        public int InsertRecord(string Rem)

        {
            if (String.IsNullOrEmpty(Rem))
                throw new ArgumentException("NamePerson cannot be null or an empty string.");

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO Timing " +
                                                "  (Rem) " +
                                                "  Values(@Rem); " +
                                                "SELECT @ID_Timing = SCOPE_IDENTITY()", conn);

            cmd.Parameters.Add("@Rem", SqlDbType.VarChar, 50).Value = Rem;
            SqlParameter p = cmd.Parameters.Add("@ID_Timing", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;

            int newID_Person = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                newID_Person = (int)p.Value;
            }
            catch (SqlException e)
            {
                // Handle exception.
            }
            finally
            {
                conn.Close();
            }

            return newID_Person;
        }
           
        //
        // Methods that support Optimistic Concurrency checks.
        //

        // Delete the Otdelen by ID_Person.

        public int DeleteRecord(string Rem, int original_ID, string original_Rem)
        {
            if (String.IsNullOrEmpty(original_Rem))
                throw new ArgumentException("FirstName cannot be null or an empty string.");

            string sqlCmd = "DELETE FROM Timing WHERE ID_timing = @original_ID " +
                            " AND rem = @original_Rem ";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@Rem", SqlDbType.VarChar, 50).Value = Rem;
            cmd.Parameters.Add("@original_ID", SqlDbType.Int).Value = original_ID;
            cmd.Parameters.Add("@rem", SqlDbType.VarChar, 10).Value = original_Rem;

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
        // Update the Employee by original ID_Person.

        public int UpdateRecord(string Rem, string original_Rem, int original_ID_Timing)
        {
            if (String.IsNullOrEmpty(Rem))
                throw new ArgumentException("Необходимо заполнить поле Наименование комнаты.");

            string sqlCmd = "UPDATE Timing " +
                            "  SET Rem = @Rem  " +
                            "  WHERE ID_Timing = @original_ID_Timing " +
                            " AND Rem = @original_Rem";

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);

            cmd.Parameters.Add("@Rem", SqlDbType.VarChar, 50).Value = Rem;
            cmd.Parameters.Add("@original_ID_Timing", SqlDbType.Int).Value = original_ID_Timing;
            cmd.Parameters.Add("@original_Rem", SqlDbType.VarChar, 50).Value = original_Rem;
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