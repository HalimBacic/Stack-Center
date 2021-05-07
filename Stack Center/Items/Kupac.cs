namespace Stack_Center.Items
{
    internal class Kupac
    {
        private string ime;
        private string prezime;
        private string jmbg;
        private string adresa;
        private string telefon;
        private int roba_id;

        public Kupac(string ime, string prezime, string jmbg, string adresa, string telefon)
        {
            this.Ime = ime;
            this.Prezime = prezime;
            this.Jmbg = jmbg;
            this.Adresa = adresa;
            this.Telefon = telefon;
        }

        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Jmbg { get => jmbg; set => jmbg = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public int Roba_id { get => roba_id; set => roba_id = value; }
    }
}