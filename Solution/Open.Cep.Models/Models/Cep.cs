using System;

namespace Open.Cep.Models.Models
{
    [Serializable]
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

    }
}
