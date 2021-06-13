using System;
using System.Collections.Generic;

namespace Open.Cep.Migrate
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public Cep[] Ceps { get; set; }
        public City()
        {
            Ceps = Array.Empty<Cep>();
        }
        public City(string[] fields) : this()
        {
            ID = int.Parse(fields[0]);
            Name = fields[1];
            StateID = int.Parse(fields[2]);
        }

        public Models.Models.City ToModelCity()
        {
            return new Models.Models.City() { 
                Name = Name, 
                Ceps = ToModelsCeps() 
            };
        }

        private Models.Models.Cep[] ToModelsCeps()
        {
            List<Models.Models.Cep> ceps = new();
            foreach (Cep cep in Ceps)
            {
                ceps.Add(cep.ToModelCep());
            }
            return ceps.ToArray();
        }
    }
}
