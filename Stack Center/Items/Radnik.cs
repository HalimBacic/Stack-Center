namespace Stack_Center.Items
{
    internal class Radnik
    {
        private string login;
        private string ime;
        private string prezime;
        private string jmbg;
        private double plata;
        private string skladiste_adresa;
        private string telefon;

        public Radnik()
        {
            telefon = "Unknown";
            plata = 0;
        }

        public Radnik(string login, string ime, string prezime, string jmbg, double plata, string skladiste_adresa, string telefon)
        {
            this.Login = login;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Jmbg = jmbg;
            this.Plata = plata;
            this.Skladiste_adresa = skladiste_adresa;
            this.Telefon = telefon;
        }

        public string Login { get => login; set => login = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Jmbg { get => jmbg; set => jmbg = value; }
        public double Plata { get => plata; set => plata = value; }
        public string Skladiste_adresa { get => skladiste_adresa; set => skladiste_adresa = value; }
        public string Telefon { get => telefon; set => telefon = value; }
    }
}