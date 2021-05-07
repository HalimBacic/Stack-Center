using Stack_Center.dao;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stack_Center
{
    /// <summary>
    /// Interaction logic for StockAdmin.xaml
    /// </summary>
    public partial class StockAdmin : Window
    {
        public StockAdmin()
        {
            InitializeComponent();
            InitializeStocks();
        }

        private void InitializeStocks()
        {
            StockDAO stocks = new StockDAO();
            List<Stock> lista = stocks.getAll();
            foreach (Stock st in lista)
                stockBox.Items.Add(st.Adress);
        }
    }
}
