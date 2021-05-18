namespace Stack_Center.Items
{
    internal class Roba
    {
        private int id;
        private string naziv;
        private double kolicina;
        private double cijena;
        private string skladiste_id;
        private string dobavljac_id;
        private double visina;
        private double sirina;
        private double duzina;

        public Roba(string naziv, double kolicina, double cijena, string skladiste_id, string dobavljac_id,double v,double s, double d)
        {
            this.Naziv = naziv;
            this.Kolicina = kolicina;
            this.Cijena = cijena;
            this.Skladiste_id = skladiste_id;
            this.Dobavljac_id = dobavljac_id;
            this.Visina = v;
            this.Sirina = s;
            this.Duzina = d;
        }

        public Roba(int i,string naziv, double kolicina, double cijena, string skladiste_id, string dobavljac_id, double v, double s, double d)
        {
            this.Id = i;
            this.Naziv = naziv;
            this.Kolicina = kolicina;
            this.Cijena = cijena;
            this.Skladiste_id = skladiste_id;
            this.Dobavljac_id = dobavljac_id;
            this.Visina = v;
            this.Sirina = s;
            this.Duzina = d;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public double Kolicina { get => kolicina; set => kolicina = value; }
        public double Cijena { get => cijena; set => cijena = value; }
        public string Skladiste_id { get => skladiste_id; set => skladiste_id = value; }
        public string Dobavljac_id { get => dobavljac_id; set => dobavljac_id = value; }
        public int Id { get => id; set => id = value; }
        public double Visina { get => visina; set => visina = value; }
        public double Sirina { get => sirina; set => sirina = value; }
        public double Duzina { get => duzina; set => duzina = value; }
    }
}