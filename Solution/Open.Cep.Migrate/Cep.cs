namespace Open.Cep.Migrate
{
    public class Cep
    {
        public long Value { get; set; }
        public string PublicPlace { get; set; }
        public string Neighborhood { get; set; }
        public int CityID { get; set; }

        public Cep() { }
        public Cep(string[] fields)
        {
            Value = long.Parse(fields[0]);
            PublicPlace = fields[1];
            Neighborhood = fields[3];
            CityID = int.Parse(fields[4]);
        }

        public Models.Models.Cep ToModelCep()
        {
            return new Models.Models.Cep()
            {
                Neighborhood = Neighborhood,
                PublicPlace = PublicPlace,
                Value = Value
            };
        }
    }
}
