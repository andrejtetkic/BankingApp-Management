using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Database
    {
        static SqlCommand GetSqlCommand(string sqlNaredba)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-7LIVVKM\\SQLEXPRESS;Integrated Security=SSPI;Initial Catalog=Bank");
            conn.Open();

            return new SqlCommand(sqlNaredba, conn);
        }

        public static bool ExecuteNonQueryCommand(string sqlNaredba)
        {
            SqlCommand command = GetSqlCommand(sqlNaredba);

            if (command.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        public static bool ExecuteScalarCommand(string sqlNaredba)
        {
            SqlCommand command = GetSqlCommand(sqlNaredba);

            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                return true;
            else
                return false;
        }

        public static List<Dictionary<string, object>> ExecuteSelectCommand(string sqlNaredba)
        {
            SqlCommand command = GetSqlCommand(sqlNaredba);
            using (command.Connection)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                    return result;
                }
            }
        }
    }
}
