using MySql.Data.MySqlClient;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Center.dao
{
    class RobaDAO : IDAO<Roba>
    {
        private List<Roba> listStock = new List<Roba>();

        public RobaDAO()
        {
        }

        public List<Roba> ListStock { get => ListStock; set => ListStock = value; }

        public void addElement(Roba roba)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dodajRobu";
            cmd.Parameters.Add("roba", MySqlDbType.String).Value = roba.Naziv;
            cmd.Parameters["roba"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("kolicina", MySqlDbType.Double).Value = roba.Kolicina;
            cmd.Parameters["kolicina"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("cijena", MySqlDbType.Double).Value = roba.Cijena;
            cmd.Parameters["cijena"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("skladiste_adresa", MySqlDbType.String).Value = roba.Skladiste_id;
            cmd.Parameters["skladiste_adresa"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("dobavljac_ime", MySqlDbType.String).Value = roba.Dobavljac_id;
            cmd.Parameters["dobavljac_ime"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("v", MySqlDbType.Double).Value = roba.Visina;
            cmd.Parameters["v"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("s", MySqlDbType.String).Value = roba.Sirina;
            cmd.Parameters["s"].Direction = ParameterDirection.Input;
            cmd.Parameters.Add("d", MySqlDbType.String).Value = roba.Duzina;
            cmd.Parameters["d"].Direction = ParameterDirection.Input;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public MySqlDataReader GetSearchStock(string stockAdr,string name)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "pretraziSkladiste";
            cmd.Parameters.Add("skl", MySqlDbType.String).Value = stockAdr;
            cmd.Parameters.Add("roba", MySqlDbType.String).Value = name;
            MySqlDataReader reader = Items.Connection.CallProcedureReader(cmd);
            return reader;
        }

        public List<Roba> getAll()
        {
            List<Roba> lista = new List<Roba>();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `roba`;");
            while (data.Read())
            {
                lista.Add(new Roba(data.GetInt32(0), data.GetString(1), Double.Parse(data.GetString(2)), Double.Parse(data.GetString(3)), data.GetString(4), 
                    data.GetString(5), data.GetDouble(6), data.GetDouble(7), data.GetDouble(8)));
            }
            Items.Connection.Disconnect();
            return lista;
        }

        public Roba getElement(int id)
        {
            throw new NotImplementedException();
        }

        public void removeElement(int id)
        {
            
        }

        public void removeElement(string ime)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "obrisiRobu";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("ime", MySqlDbType.String).Value = ime;
            cmd.Parameters["ime"].Direction = ParameterDirection.Input;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public void updateElement(Roba obj, int id)
        {
            throw new NotImplementedException();
        }
    }
}
