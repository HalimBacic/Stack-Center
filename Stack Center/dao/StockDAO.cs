using MySql.Data.MySqlClient;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack_Center.dao
{
    class  StockDAO : IDAO<Stock>
    {
        private List<Stock> listStock = new List<Stock>();

        public StockDAO()
        {
        }

        public List<Stock> ListStock { get => ListStock; set => ListStock = value; }

        public void addElement(Stock obj)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "dodajSkladiste";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("adr", MySqlDbType.String).Value = obj.Adress;
            cmd.Parameters.Add("vis", MySqlDbType.Int32).Value = obj.Visina;
            cmd.Parameters.Add("sir", MySqlDbType.Int32).Value = obj.Sirina;
            cmd.Parameters.Add("duz", MySqlDbType.Int32).Value = obj.Duzina;
            cmd.Parameters.Add("info", MySqlDbType.String).Value = obj.Info;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public List<Stock> getAll()
        {
            List<Stock> lista = new List<Stock>();
            Items.Connection.Connect();
            MySqlDataReader data = Items.Connection.ReadData("select * from `skladiste`;");
            while (data.Read())
            {
                lista.Add(new Stock(data.GetString(0), data.GetInt32(1), data.GetInt32(2), data.GetInt32(3), data.GetString(4)));
            }
            Items.Connection.Disconnect();
            return lista;
        }

        public Stock getElement(int id)
        {
            throw new NotImplementedException();
        }

        public void removeElement(int id)
        {
            throw new NotImplementedException();
        }

        public void removeElement(string adr)
        {
            Items.Connection.Connect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "obrisiSkladiste";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("adr", MySqlDbType.String).Value = adr;
            Items.Connection.CallProcedure(cmd);
            Items.Connection.Disconnect();
        }

        public void updateElement(Stock obj, int id)
        {
            throw new NotImplementedException();
        }
    }
}
