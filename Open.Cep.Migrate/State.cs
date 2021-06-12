using System.Collections.Generic;

namespace Open.Cep.Migrate
{
    public class State
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<City> Cities { get; set; }
        public string Acronym { get; set; }

        public State()
        {
            Cities = new List<City>();
        }

        public State(string[] fields) : this()
        {
            ID = int.Parse(fields[0]);
            Name = fields[1];
            Acronym = fields[2];
        }

        public Models.Models.State ToModelState()
        {
            return new Models.Models.State()
            {
                Acronym = new char[] { Acronym[0], Acronym[1] },
                Cities = ToModelsCities(),
                Name = Name
            };
        }

        private Models.Models.City[] ToModelsCities()
        {
            List<Models.Models.City> cities = new();
            foreach (City city in Cities)
            {
                cities.Add(city.ToModelCity());
            }
            return cities.ToArray();
        }


    }
}
