using System;
using System.Collections.Generic;

namespace Open.Cep.Models.Models
{
    [Serializable]
    public class State
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public City[] Cities { get; set; }
    }
}
