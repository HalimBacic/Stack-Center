using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace Stack_Center.Items
{
    public class Connection
    {
        private static string CONNECTION = ConfigurationManager.ConnectionStrings["connection"].ToString();
        private static MySqlConnection conn;

        public Connection()
        {

        }

        public static void Connect()
        {
            conn = new MySqlConnection(CONNECTION);
            conn.Open();
        }
    

        public static MySqlDataReader ReadData(string sql)
        {
            MySqlCommand sqlcmd = new MySqlCommand(sql, conn);

            return sqlcmd.ExecuteReader();
        }

        public static void InsertData(string sql)
        {
            MySqlDataAdapter adap = new MySqlDataAdapter();

            adap.InsertCommand = new MySqlCommand(sql, conn);
            adap.InsertCommand.ExecuteNonQuery();
        }

        public static void CallProcedure(MySqlCommand cmd)
        {
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        public static MySqlDataReader CallProcedureReader(MySqlCommand cmd)
        {
            cmd.Connection = conn;
            return cmd.ExecuteReader();
        }

        public static bool CallLogin(string login, string hash)
        {
            Connect();
            sbyte value = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "pronadjiLogin";
            cmd.Parameters.Add("loginCheck", MySqlDbType.String).Value = login;
            cmd.Parameters["loginCheck"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("passCheck", MySqlDbType.String).Value = hash;
            cmd.Parameters["passCheck"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("isOk", MySqlDbType.Byte).Value = value;   
            cmd.Parameters["isOk"].Direction = ParameterDirection.Output;
            CallProcedure(cmd);  
            value = (sbyte)cmd.Parameters["isOk"].Value;
            Disconnect();
            if (value == 1)
            {
                return true;
            }
            else
            return false;
        }

        public static void Disconnect()
        {
            conn.Close();
        }
    }
}