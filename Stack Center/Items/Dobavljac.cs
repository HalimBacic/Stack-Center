namespace Stack_Center.Items
{
    internal class Dobavljac
    {
        private string ime;
        private string telefon;
        private string adresa;

        public Dobavljac(string ime, string telefon, string adresa)
        {
            Ime = ime;
            Telefon = telefon;
            Adresa = adresa;
        }

        public string Ime { get => ime; set => ime = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Adresa { get => adresa; set => adresa = value; }
    }
}