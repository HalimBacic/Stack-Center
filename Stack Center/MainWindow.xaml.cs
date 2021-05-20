using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
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
                StockAdmin stockAdmin;
                stockAdmin = new StockAdmin();
                stockAdmin.NameUser = login;
                stockAdmin.Show();
                this.Close();
            }
            else if (tip.Equals("Manager"))
            {
                StockManager stockManager;
                stockManager = new StockManager();
                stockManager.NameUser = login;
                stockManager.Show();
                this.Close();
            }
                
            Items.Connection.Disconnect();
        }

        public void Failure()
        {
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

        private void loginBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            loginBox.Text = "";
        }

        private void passBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            passBox.Text = "";
        }
    }
}