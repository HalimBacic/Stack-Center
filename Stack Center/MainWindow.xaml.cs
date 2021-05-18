using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

namespace Stack_Center
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string pass = passBox.Text;

            byte[] data = new byte[512];
            data = Encoding.UTF8.GetBytes(pass);
            byte[] result;
            SHA512 shaM = new SHA512Managed();
            result = shaM.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("x2"));
            if (Items.Connection.CallLogin(login, sb.ToString()))
            {
                Success(login);
            }
            else
                Failure();
        }


        public void Success(string login)
        {
            Items.Connection.Connect();
            StockAdmin stockAdmin;
            string tip = "Unknown";
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "pronadjiTip";
            cmd.Parameters.Add("loginCheck", MySqlDbType.String).Value = login;
            cmd.Parameters["loginCheck"].Direction = ParameterDirection.Input;
            MySqlDataReader data = Items.Connection.CallProcedureReader(cmd);
            while (data.Read())
                tip = data.GetString(0);
            if (tip.Equals("Administrator"))
            {
                stockAdmin = new StockAdmin();
                stockAdmin.Show();
            }
            else
                Trace.WriteLine("Open employee window");
            Items.Connection.Disconnect();
        }

        public void Failure()
        {
            //TODO Stilizovati
            MessageBoxResult result = MessageBox.Show("Warning", "Wrong username or password.");
        }

        private void engBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.LanguageChange("en-US");
        }

        private void srBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.LanguageChange("sr");
        }
    }
}