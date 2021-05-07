namespace Stack_Center.Items
{
    internal class Radnik
    {
        private string login;
        private string ime;
        private string prezime;
        private string jmbg;
        private double plata;
        private int skladiste_id;

        public Radnik(string login, string ime, string prezime, string jmbg, double plata)
        {
            this.login = login;
            this.ime = ime;
            this.prezime = prezime;
            this.jmbg = jmbg;
            this.plata = plata;
        }

        public string Login { get => login; set => login = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Jmbg { get => jmbg; set => jmbg = value; }
        public double Plata { get => plata; set => plata = value; }
        public int Skladiste_id { get => skladiste_id; set => skladiste_id = value; }
    }
}