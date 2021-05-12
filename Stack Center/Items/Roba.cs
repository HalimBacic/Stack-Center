namespace Stack_Center.Items
{
    internal class Roba
    {
        private string naziv;
        private double kolicina;
        private double cijena;
        private string skladiste_id;
        private string dobavljac_id;

        public Roba(string naziv, double kolicina, double cijena, string skladiste_id, string dobavljac_id)
        {
            this.Naziv = naziv;
            this.Kolicina = kolicina;
            this.Cijena = cijena;
            this.Skladiste_id = skladiste_id;
            this.Dobavljac_id = dobavljac_id;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public double Kolicina { get => kolicina; set => kolicina = value; }
        public double Cijena { get => cijena; set => cijena = value; }
        public string Skladiste_id { get => skladiste_id; set => skladiste_id = value; }
        public string Dobavljac_id { get => dobavljac_id; set => dobavljac_id = value; }
    }
}