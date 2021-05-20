using MySql.Data.MySqlClient;
using Stack_Center.dao;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Stack_Center
{
    /// <summary>
    /// Interaction logic for StockAdmin.xaml
    /// </summary>
    public partial class StockAdmin : Window
    {
        private Stock selectedStock;
        private string username;
        private string lozinka;
        protected bool isDragging;
        private Point clickPosition;
        private TranslateTransform originTT;
        DobavljacDao dobavljacDao = new DobavljacDao();
        StockDAO stockDAO = new StockDAO();
        RobaDAO robaDAO = new RobaDAO();
        RadnikDAO radnikDAO = new RadnikDAO();
        KupacDAO kupacDAO = new KupacDAO();

        internal Stock SelectedStock { get => selectedStock; set => selectedStock = value; }
        public string Username { get => username; set => username = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }

        public StockAdmin()
        {
            InitializeComponent();
            dataTable.Items.Clear();
            suppData.Items.Clear();
            customersGrid.Items.Clear();
            resultData.Items.Clear();
            InitializeSuppliers();
            InitializeStocks();
            pickType.Items.Add("Administrator");
            pickType.Items.Add("Manager");
        }

        private void InitializeSuppliers()
        {
            List<Dobavljac> lista = dobavljacDao.getAll();
            suppData.ItemsSource = lista.ToArray();
            suppBox.ItemsSource = lista.ToArray();
        }

        private void InitializeStocks()
        {

            List<Stock> lista = stockDAO.getAll();

            stockBox.ItemsSource = lista.ToArray();
        }

        private void InitializeWorkers()
        {
            List<Radnik> lista = radnikDAO.getAll();

            dataTable.ItemsSource = lista.ToArray();
        }

        private void InitializeCustomers()
        {
            List<Kupac> lista = kupacDAO.getAll();

            customersGrid.ItemsSource = lista.ToArray();
        }

        private void MakeAllInvisible()
        {
            stockGrid.Visibility = Visibility.Collapsed;
            workersGrid.Visibility = Visibility.Collapsed;
            suppGrid.Visibility = Visibility.Collapsed;
            custGrid.Visibility = Visibility.Collapsed;
        }

        public void ChangeSelected(object sender, SelectionChangedEventArgs e)
        {
            stateCanvas.Children.Clear();
            SelectedStock = (Stock)stockBox.SelectedItem;
            
            if (selectedStock != null)
            {
                addStockPanel.Visibility = Visibility.Collapsed;
                slctdLabel.Visibility = Visibility.Collapsed;
                activePanel.Visibility = Visibility.Visible;
                status.Visibility = Visibility.Visible;

                Double all = SelectedStock.Duzina * SelectedStock.Sirina * SelectedStock.Visina;
                Double free = 0.0;

                List<Roba> allRoba = robaDAO.getAll();
                List<Roba> stockRoba = new List<Roba>();
                foreach (Roba roba in allRoba)
                {
                    if (roba.Skladiste_id.Equals(SelectedStock.Adress))
                    {
                        free += roba.Duzina * roba.Sirina * roba.Visina;
                        stockRoba.Add(roba);
                    }
                }

                stockTable.ItemsSource = stockRoba.ToArray();
                allBox.Text = all.ToString();
                freeBox.Text = (all - free).ToString();
                vBox.Text = selectedStock.Visina.ToString();
                sBox.Text = selectedStock.Sirina.ToString();
                dBox.Text = selectedStock.Duzina.ToString();
                CountDimension();

                if (File.Exists("Stocks/" + selectedStock.Adress + ".scf"))
                {
                    string name = "Stocks/" + selectedStock.Adress + ".scf";
                    FileStream fs = File.Open(name, FileMode.Open, FileAccess.Read);
                    Canvas savedCanvas = XamlReader.Load(fs) as Canvas;
                    fs.Close();
                    this.stateCanvas.Children.Add(savedCanvas);
                }
            }
            else
            {
                allBox.Text = "";
                freeBox.Text = "";
            }
        }

        private void CountDimension()
        {
            //Dimenzije skladišta, unešene u metrima, konvertovane u cm
            double sirina = selectedStock.Sirina * 100;
            double duzina = selectedStock.Duzina * 100;

            //Razmjera veličine containera i skladišta
            if (duzina >= sirina)
            {
                double razmD = sirina / duzina;   //npr 2.66
                stateCanvas.Width = 700;
                stateCanvas.Height = 700 * razmD;
            }
            else if (sirina > duzina)
            {
                double razmD = duzina / sirina;   //npr 2.66
                stateCanvas.Height = 650;
                stateCanvas.Width = 650 * razmD;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var draggableControl = sender as Shape;
            originTT = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
            isDragging = true;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as Shape;
            draggable.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as Shape;
            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = originTT.X + (currentPosition.X - clickPosition.X);
                transform.Y = originTT.Y + (currentPosition.Y - clickPosition.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            }
        }

        private void CirclePack_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Stvarne dimenzije u m
            double visinaS = selectedStock.Visina;
            double sirinaS = selectedStock.Sirina;
            double duzinaS = selectedStock.Duzina;

            //Dimenzije predmeta u m
            double visinaP = Double.Parse(heightText.GetLineText(0));
            double duzinaP = Double.Parse(lengthText.GetLineText(0));
            double sirinaP = Double.Parse(widthText.GetLineText(0));

            //Odnos grafike i stvarne dimenzije skladišta
            double razmX = stateCanvas.Height / (visinaS * 100);
            double razmY = stateCanvas.Width / (duzinaS * 100);
            Trace.WriteLine(razmX + " " + razmY);

            Ellipse elipse = new Ellipse();
            //Dimenzije grafike u odnosu na skladiste
            elipse.Height = (sirinaP * razmX)*100;
            elipse.Width =  (duzinaP * razmY)*100;
            Trace.WriteLine(elipse.Height + " " + elipse.Width);
            elipse.Fill = Brushes.DarkRed;
            elipse.Stroke = Brushes.Black;
            elipse.StrokeThickness = 1.5;
            elipse.Opacity = visinaP / visinaS;
            elipse.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            elipse.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            elipse.MouseMove += Canvas_MouseMove;
            Stockgfx gfx = new Stockgfx(elipse,visinaP,duzinaP,sirinaP,Double.Parse(weightText.GetLineText(0)),int.Parse(quanText.GetLineText(0)));
                                        
            stateCanvas.Children.Add(gfx);
        }

        private void cubicPack_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //Stvarne dimenzije u m
            double visinaS = selectedStock.Visina;
            double sirinaS = selectedStock.Sirina;
            double duzinaS = selectedStock.Duzina;

            //Dimenzije predmeta u m
            double visinaP = Double.Parse(heightText.GetLineText(0));
            double duzinaP = Double.Parse(lengthText.GetLineText(0));
            double sirinaP = Double.Parse(widthText.GetLineText(0));

            //Odnos grafike i stvarne dimenzije skladišta
            double razmX = stateCanvas.Height / (visinaS * 100);
            double razmY = stateCanvas.Width / (duzinaS * 100);

            Rectangle rect = new Rectangle();
            //Dimenzije grafike u odnosu na skladiste
            rect.Height = (sirinaP * razmX) * 100;
            rect.Width = (duzinaP * razmY) * 100;
            rect.Fill = Brushes.DarkRed;
            rect.Opacity = visinaP / visinaS;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1.5;
            rect.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            rect.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            rect.MouseMove += Canvas_MouseMove;
            stateCanvas.Children.Add(rect);
        }

        private void lgtBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = nametext.GetLineText(0);
            double quan = Double.Parse(quanText.GetLineText(0));
            double price = Double.Parse(pricetext.GetLineText(0));
            string suppName = ((Dobavljac)suppBox.SelectedItem).Ime;

            double v = Double.Parse(heightText.GetLineText(0));
            double s = Double.Parse(widthText.GetLineText(0));
            double d = Double.Parse(lengthText.GetLineText(0));

            Roba roba = new Roba(name, quan, price, selectedStock.Adress, suppName,v,s,d);
            robaDAO.addElement(roba);
        }

        private void stcBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeAllInvisible();
            stockGrid.Visibility = Visibility.Visible;
        }

        private void wrksBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeAllInvisible();
            workersGrid.Visibility = Visibility.Visible;

            InitializeWorkers();
        }

        private void addWorker_Click(object sender, RoutedEventArgs e)
        {
            string username = nameBox.GetLineText(0) + surnameBox.GetLineText(0);
            Double plata = Double.Parse(payBox.GetLineText(0));
            Radnik radnik = new Radnik(username, nameBox.GetLineText(0), surnameBox.GetLineText(0), 
                                    uidBox.GetLineText(0),plata, selectedStock.Adress, phoneBox.GetLineText(0));

            radnikDAO.addElement(radnik);

            string pass = radnik.Jmbg;

            string tip = pickType.SelectedItem.ToString();

            /*        SHA512 shaM = new SHA512Managed();
                    byte[] result = shaM.ComputeHash(Encoding.UTF8.GetBytes(pass));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < result.Length; i++)
                        sb.Append(result[i].ToString("x2"));

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "dodajLogin";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("login", MySqlDbType.String).Value = username;
                    cmd.Parameters.Add("pass", MySqlDbType.String).Value = sb.ToString();
                    cmd.Parameters.Add("tip", MySqlDbType.String).Value = tip;
                    Items.Connection.Connect();
                    Items.Connection.CallProcedure(cmd);
                    Items.Connection.Disconnect(); */

            radnikDAO.AddLogin(username,pass,tip);
        }

        private void splrBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeAllInvisible();
            suppGrid.Visibility = Visibility.Visible;
            InitializeSuppliers();
        }

        private void addSuppBtn_Click(object sender, RoutedEventArgs e)
        {
            Dobavljac dobavljac = new Dobavljac(suppName.GetLineText(0),suppPhone.GetLineText(0),suppAdr.GetLineText(0));

            dobavljacDao.addElement(dobavljac);

        }

        private void dltSuppBtn_Click(object sender, RoutedEventArgs e)
        {
            dobavljacDao.removeElement(suppNameDel.GetLineText(0));
            InitializeSuppliers();
        }

        private void byeBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeAllInvisible();
            custGrid.Visibility = Visibility.Visible;
            InitializeCustomers();
        }

        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            Kupac kupac = new Kupac(nameCust.GetLineText(0),surnameCust.GetLineText(0),jmbgCust.GetLineText(0),addrCust.GetLineText(0),phoneCust.GetLineText(0));
            Double quan = 0.0;
            int id = 0;
            try
            {
                quan = Double.Parse(idGoodsQuan.GetLineText(0));
                id = Int32.Parse(idGoods.GetLineText(0));
            }
            catch (FormatException ex)
            {
                quan = 0.0;
                id = 0;
            }
            finally
            {
                if (quan != 0.0 || id != 0)
                {
                    kupacDAO.addElement(kupac);

                    Items.Connection.Connect();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "setKolicina";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("robaId", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("robaQuan", MySqlDbType.Double).Value = quan;
                    Items.Connection.CallProcedure(cmd);
                    Items.Connection.Disconnect();
                   
                    cmd = new MySqlCommand();
                    cmd.CommandText = "dodajKupovinu";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("id", MySqlDbType.Int32).Value = id;
                    cmd.Parameters.Add("jmbg", MySqlDbType.String).Value = kupac.Jmbg;
                    Items.Connection.Connect();
                    Items.Connection.CallProcedure(cmd);
                    Items.Connection.Disconnect();
                }
            }
        }

        private void searchStockBtn_Click(object sender, RoutedEventArgs e)
        {
            searchStack.Visibility = Visibility.Visible;
            List<Roba> dataRoba = new List<Roba>();
            string findString = searchBox.GetLineText(0);
            MySqlDataReader data = robaDAO.GetSearchStock(selectedStock.Adress, findString);
            while (data.Read())
            {
                dataRoba.Add(new Roba(data.GetInt32(0),data.GetString(1), Double.Parse(data.GetString(2)), Double.Parse(data.GetString(3)), data.GetString(4), 
                    data.GetString(5),data.GetDouble(6), data.GetDouble(7), data.GetDouble(8)));
            }

            findData.ItemsSource = dataRoba.ToArray();
            Items.Connection.Disconnect();
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Roba> dataRoba = new List<Roba>();
            string findString = searchBox2.GetLineText(0);
            MySqlDataReader data = robaDAO.GetSearchStock(selectedStock.Adress, findString);
            while (data.Read())
            {
                dataRoba.Add(new Roba(data.GetInt32(0), data.GetString(1), Double.Parse(data.GetString(2)), Double.Parse(data.GetString(3)), data.GetString(4), 
                    data.GetString(5), data.GetDouble(6), data.GetDouble(7), data.GetDouble(8)));
            }

            resultData.ItemsSource = dataRoba.ToArray();
            Items.Connection.Disconnect();
        }

        private void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (containerCanvas.Visibility == Visibility.Visible)
            {
                containerCanvas.Visibility = Visibility.Collapsed;
                containerTable.Visibility = Visibility.Visible;
            }
            else
            {
                containerCanvas.Visibility = Visibility.Visible;
                containerTable.Visibility = Visibility.Collapsed;
            }
        }

        private void dayBtn_Click(object sender, RoutedEventArgs e)
        {
            var app = App.Current as App;
            app.ChangeTheme(new Uri(@"/Themes/DayTheme.xaml", UriKind.Relative));
        }

        private void nightBtn_Click(object sender, RoutedEventArgs e)
        {
            var app = App.Current as App;
            app.ChangeTheme(new Uri(@"/Themes/NightTheme.xaml", UriKind.Relative));
        }

        private void addStock_Click(object sender, RoutedEventArgs e)
        {
            slctdLabel.Visibility = Visibility.Collapsed;
            activePanel.Visibility = Visibility.Collapsed;
            addStockPanel.Visibility = Visibility.Visible;
            stockBox.SelectedIndex = -1;
        }

        private void addStockBtn_Click(object sender, RoutedEventArgs e)
        {
            int v = Int32.Parse(vStock.GetLineText(0));
            int s = Int32.Parse(sStock.GetLineText(0));
            int d = Int32.Parse(dStock.GetLineText(0));

            Stock stock = new Stock(adrStock.GetLineText(0),v,s,d,iStock.Text);
            stockDAO.addElement(stock);
            InitializeStocks();
        }

        private void removeStock_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStock != null)
            {
                stockDAO.removeElement(selectedStock.Adress);
                stockBox.SelectedIndex = -1;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Select stock", "Problem", MessageBoxButton.OK);
            }
        }

        private void profilBtn_Click(object sender, RoutedEventArgs e)
        {
            if (changeProfil.Visibility == Visibility.Visible)
                changeProfil.Visibility = Visibility.Collapsed;
            else
                changeProfil.Visibility = Visibility.Visible;
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            radnikDAO.AddLogin(userBox.GetLineText(0),passBox.GetLineText(0),"Administrator");
            MessageBoxResult result = MessageBox.Show("Information updated", "Updated", MessageBoxButton.OK);
        }

        private void stateCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selectedStock != null)
            {
                string name = "Stocks/" + selectedStock.Adress + ".scf";
                FileStream fs = File.Open(name, FileMode.Create);
                XamlWriter.Save(stateCanvas, fs);
                fs.Close();
            }
        }

        private void searchWrk_Click(object sender, RoutedEventArgs e)
        {
            Radnik radnik = radnikDAO.getElement(wrkName.GetLineText(0));
            phhUpdBox.Text = radnik.Telefon;
            payUpdBox.Text = radnik.Plata.ToString();
        }
    }
}