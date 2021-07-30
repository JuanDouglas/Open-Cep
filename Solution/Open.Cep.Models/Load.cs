using Newtonsoft.Json;
using Open.Cep.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.Cep.Models
{
    public class Load
    {
        public Models.Cep[] Ceps { get; set; }
        public City[] Cities { get; set; }
        public State[] States { get; set; }

        public Load() { 
        }
        public Load(string jsonContent) : this(JsonConvert.DeserializeObject<State[]>(jsonContent)) { }
        public Load(State[] states) {
            LoadCeps(states);
        }

        public void LoadCeps(State[] states)
        {
            States = states;
            List<Models.Cep> ceps = new();
            List<Models.City> cities  =new();
            foreach (var item in states)
            {
                cities.AddRange(item.Cities);
                foreach (var city in item.Cities)
                {
                    ceps.AddRange(city.Ceps);
                }
            }
            Cities = cities.ToArray();
            Ceps = ceps.ToArray();
        }
    }
}
