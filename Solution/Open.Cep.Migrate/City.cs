using System;
using Open.Cep.Models;
using System.Collections.Generic;

namespace Open.Cep.Migrate
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public Models.Models.Cep[] Ceps { get; set; }
        public City() => Ceps = Array.Empty<Models.Models.Cep>();

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
                ID= ID,
                StateID = StateID,
                Ceps = Ceps
            };
        }
    }
}
