using MySql.Data.MySqlClient;
using Stack_Center.Items;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Stack_Center.dao
{
    internal class DobavljacDao : IDAO<Dobavljac>
    {
        private List<Dobavljac> list = new List<Dobavljac>();

        public DobavljacDao()
        { }

        public void addElement(Dobavljac obj)
        {
            list.Add(obj);

            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dodajDobavljaca";
            cmd.Parameters.Add("ime", MySqlDbType.String).Value = obj.Ime;
            cmd.Parameters.Add("telefon", MySqlDbType.String).Value = obj.Telefon;
            cmd.Parameters.Add("adresa", MySqlDbType.String).Value = obj.Adresa;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public List<Dobavljac> getAll()
        {
            List<Dobavljac> lista = new List<Dobavljac>();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `dobavljac`;");
            while (data.Read())
            {
                Dobavljac dob = new Dobavljac(data.GetString("ime"), data.GetString("telefon"), data.GetString("adresa"));
                lista.Add(dob);
            }
            Items.Connection.Disconnect();
            return lista;
        }

        public Dobavljac getElement(int id)
        {
            Dobavljac dobavljac = null;
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "pronadjiDobavljaca";
            cmd.Parameters.Add("id", MySqlDbType.Int32).Value = id;
            MySqlDataReader reader = Items.Connection.CallProcedureReader(cmd);
            while (reader.Read())
            {
                dobavljac = new Dobavljac(reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
            return dobavljac;
        }

        public void removeElement(int id)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "obrisiDobavljaca";
            cmd.Parameters.Add("id", MySqlDbType.Int32).Value = id;
            Items.Connection.CallProcedure(cmd);
        }

        public void removeElement(string ime)
        {
            throw new System.NotImplementedException();
        }

        public void updateElement(Dobavljac obj, int id)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "updateDobavljacTelefon";
            cmd.Parameters.Add("id", MySqlDbType.Int32).Value = id;
            cmd.Parameters.Add("telefon", MySqlDbType.String).Value = obj.Telefon;
            Items.Connection.CallProcedure(cmd);
        }
    }
}