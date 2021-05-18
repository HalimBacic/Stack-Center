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
    class KupacDAO : IDAO<Kupac>
    {
        private List<Kupac> list = new List<Kupac>();

        public KupacDAO()
        { }

        public void addElement(Kupac obj)
        {
            list.Add(obj);

            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dodajKupca";
            cmd.Parameters.Add("ime", MySqlDbType.String).Value = obj.Ime;
            cmd.Parameters.Add("prezime", MySqlDbType.String).Value = obj.Prezime;
            cmd.Parameters.Add("telefon", MySqlDbType.String).Value = obj.Telefon;
            cmd.Parameters.Add("adresa", MySqlDbType.String).Value = obj.Adresa;
            cmd.Parameters.Add("jmbg", MySqlDbType.String).Value = obj.Jmbg;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public void Assign(int roba_id,string jmbg)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dodajKupovinu";
            cmd.Parameters.Add("id", MySqlDbType.Int64).Value = roba_id;
            cmd.Parameters.Add("jmbg", MySqlDbType.String).Value = jmbg;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public List<Kupac> getAll()
        {
            list.Clear();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `kupac`;");
            while (data.Read())
            {
                Kupac kupac = new Kupac(data.GetString("ime"), data.GetString("prezime"), data.GetString("jmbg"), data.GetString("adresa"), data.GetString("telefon"));
                list.Add(kupac);
            }
            Items.Connection.Disconnect();
            return list;
        }

        public Kupac getElement(int id)
        {
            throw new NotImplementedException();
        }

        public void removeElement(int id)
        {
            throw new NotImplementedException();
        }

        public void removeElement(string ime)
        {
            throw new NotImplementedException();
        }

        public void updateElement(Kupac obj, int id)
        {
            throw new NotImplementedException();
        }
    }
}
