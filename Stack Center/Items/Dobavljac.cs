namespace Stack_Center.Items
{
    internal class Dobavljac
    {
        private string ime;
        private string prezime;
        private string telefon;
        private string adresa;
        private int id;

        public Dobavljac(string ime, string prezime, string telefon, string adresa)
        {
            Ime = ime;
            Prezime = prezime;
            Telefon = telefon;
            Adresa = adresa;
        }

        public Dobavljac(string ime, string prezime, string telefon, string adresa, int id)
        {
            Ime = ime;
            Prezime = prezime;
            Telefon = telefon;
            Adresa = adresa;
            Id = id;
        }

        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Adresa { get => adresa; set => adresa = value; }
        public int Id { get => id; set => id = value; }
    }
}