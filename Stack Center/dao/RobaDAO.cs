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
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public List<Roba> getAll()
        {
            List<Roba> lista = new List<Roba>();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `roba`;");
            while (data.Read())
            {
                lista.Add(new Roba(data.GetString(0), data.GetInt32(1), data.GetInt32(2), data.GetString(3), data.GetString(4)));
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
