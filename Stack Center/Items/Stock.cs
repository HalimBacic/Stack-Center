namespace Stack_Center.Items
{
    internal class Stock
    {
        private string adress;
        private int visina;
        private int sirina;
        private int duzina;
        private string info;

        public Stock(string adress, int visina, int sirina, int duzina, string info)
        {
            this.Adress = adress;
            this.Visina = visina;
            this.Sirina = sirina;
            this.Duzina = duzina;
            this.Info = info;
        }

        public string Adress { get => adress; set => adress = value; }
        public string Update { get => Info; set => Info = value; }
        public int Visina { get => visina; set => visina = value; }
        public int Sirina { get => sirina; set => sirina = value; }
        public int Duzina { get => duzina; set => duzina = value; }
        public string Info { get => info; set => info = value; }
    }
}