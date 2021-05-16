using MySql.Data.MySqlClient;
using Stack_Center.dao;
using Stack_Center.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        protected bool isDragging;
        private Point clickPosition;
        private TranslateTransform originTT;
        DobavljacDao dobavljacDao = new DobavljacDao();
        StockDAO stocks = new StockDAO();
        RobaDAO robaDAO = new RobaDAO();
        RadnikDAO radnikDAO = new RadnikDAO();
        KupacDAO kupacDAO = new KupacDAO();

        internal Stock SelectedStock { get => selectedStock; set => selectedStock = value; }

        public StockAdmin()
        {
            InitializeComponent();
            dataTable.Items.Clear();
            suppData.Items.Clear();
            customersGrid.Items.Clear();
            resultGrid.Items.Clear();
            InitializeSuppliers();
            InitializeStocks();
        }

        private void InitializeSuppliers()
        {
            List<Dobavljac> lista = dobavljacDao.getAll();
            suppData.ItemsSource = lista.ToArray();
            suppBox.ItemsSource = lista.ToArray();
        }

        private void InitializeStocks()
        {

            List<Stock> lista = stocks.getAll();

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
            slctdLabel.Visibility = Visibility.Collapsed;
            activePanel.Visibility = Visibility.Visible;

            SelectedStock = (Stock)stockBox.SelectedItem;
            CountDimension();
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

            Roba roba = new Roba(name, quan, price, selectedStock.Adress, suppName);
            robaDAO.addElement(roba);
        }

   /*     private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            string ime = deleteBox.GetLineText(0);
            robaDAO.removeElement(ime);
        } */

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
            Double plata = Double.Parse(payBox.GetLineText(0));
            Radnik radnik = new Radnik(loginBox.GetLineText(0), nameBox.GetLineText(0), surnameBox.GetLineText(0), 
                                    uidBox.GetLineText(0),plata, selectedStock.Adress, phoneBox.GetLineText(0));

            radnikDAO.addElement(radnik);
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
        }

        private void byeBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeAllInvisible();
            custGrid.Visibility = Visibility.Visible;
            InitializeCustomers();
        }

        private void addCustomer_Click(object sender, RoutedEventArgs e)
        {
            //  public Kupac(string ime, string prezime, string jmbg, string adresa, string telefon)
            Kupac kupac = new Kupac(nameCust.GetLineText(0),surnameCust.GetLineText(0),jmbgCust.GetLineText(0),addrCust.GetLineText(0),phoneCust.GetLineText(0));
            kupacDAO.addElement(kupac);
        }
    }
}