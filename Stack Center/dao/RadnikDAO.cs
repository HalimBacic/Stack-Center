using MySql.Data.MySqlClient;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Center.dao
{
    class RadnikDAO : IDAO<Radnik>
    {
        private List<Radnik> list = new List<Radnik>();

        public RadnikDAO()
        { }

        public void addElement(Radnik obj)
        {

            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "dodajRadnika";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("login", MySqlDbType.String).Value = obj.Login;
            cmd.Parameters.Add("ime", MySqlDbType.String).Value = obj.Ime;
            cmd.Parameters.Add("prezime", MySqlDbType.String).Value = obj.Prezime;
            cmd.Parameters.Add("jmbg", MySqlDbType.String).Value = obj.Jmbg;
            cmd.Parameters.Add("telefon", MySqlDbType.String).Value = obj.Telefon;
            cmd.Parameters.Add("plata", MySqlDbType.Double).Value = obj.Plata;
            cmd.Parameters.Add("skladiste_adresa", MySqlDbType.String).Value = obj.Skladiste_adresa;
            Trace.WriteLine(cmd.ToString());
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public List<Radnik> getAll()
        {
            List<Radnik> lista = new List<Radnik>();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `radnik`;");
            while (data.Read())
            {
                Radnik radnik = new Radnik(data.GetString("login"),data.GetString("ime"), data.GetString("prezime"), 
                                            data.GetString("jmbg"), data.GetDouble("plata"), data.GetString("skladiste_adresa"), data.GetString("telefon"));
                lista.Add(radnik);
            }
            Items.Connection.Disconnect();
            return lista;
        }

        public Radnik getElement(int id)
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

        public void updateElement(Radnik obj, int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Radnik> GetObservableCollection()
        {
            var collection = new ObservableCollection<Radnik>();

            List<Radnik> lista = getAll();

            foreach(Radnik r in lista)
                collection.Add(r);

            return collection;
        }
    }
}
