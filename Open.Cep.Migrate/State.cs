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

    }
}
