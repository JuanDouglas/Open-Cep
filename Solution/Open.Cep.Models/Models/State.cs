using System.Collections.Generic;

namespace Open.Cep.Models.Models
{
    public class State
    {
        public char[] Acronym { get; set; }
        public string Name { get; set; }
        public City[] Cities { get; set; }
    }
}
