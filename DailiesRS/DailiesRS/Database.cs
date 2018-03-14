using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DailiesRS.Properties;

namespace DailiesRS
{
    public class Database
    {
        private string connString;

        public Database()
        {
            connString = Settings.Default.ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }

        public void CreateTask(string name)
        {
            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("InsertTask")
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Connection = sqlConnection;
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 150));
                cmd.Parameters["@Name"].Value = name;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not insert");
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public void RemoveTask(string name)
        {
            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
                SqlDataReader rdr = null;
                string readQuery = "Select * from DailyTask Where Name = '" + name + "'";
                SqlCommand readSqlCommand = new SqlCommand(readQuery);
                readSqlCommand.Connection = sqlConnection;
                rdr = readSqlCommand.ExecuteReader();
                List<string> strings = new List<string>();
                while (rdr.Read()) {
                    strings.Add((string)rdr["Name"]);
                }
                rdr.Close();
                if (strings.Count > 0)
                {
                    var itemToRemove = strings.FirstOrDefault();
                    string deleteQuery = "DELETE TOP(1) FROM dailytask Where Name = '" + itemToRemove + "'";
                    SqlCommand deleteSqlCommand = new SqlCommand(deleteQuery);
                    deleteSqlCommand.Connection = sqlConnection;
                    deleteSqlCommand.ExecuteNonQuery();
                }
            }
        }

        public List<string> ReadAllTasks()
        {
            using (var sqlConnection = GetConnection())
            {
                sqlConnection.Open();
                SqlDataReader rdr = null;
                string readQuery = "Select * from DailyTask";
                SqlCommand readSqlCommand = new SqlCommand(readQuery);
                readSqlCommand.Connection = sqlConnection;
                rdr = readSqlCommand.ExecuteReader();
                List<string> strings = new List<string>();
                while (rdr.Read())
                {
                    strings.Add((string)rdr["Name"]);
                }

                rdr.Close();
                return strings;
            }
        }

        public void DeleteAll()
        {
            using (var sqlConnection = GetConnection())
            {sqlConnection.Open();
                string deleteQuery = "DELETE FROM dailytask";
                SqlCommand deleteSqlCommand = new SqlCommand(deleteQuery);
                deleteSqlCommand.Connection = sqlConnection;
                deleteSqlCommand.ExecuteNonQuery();
            }
           
        }
    }
}
