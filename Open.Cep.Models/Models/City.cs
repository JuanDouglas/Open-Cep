using System.Collections.Generic;

namespace Open.Cep.Models.Models
{
    public class City
    {
        public string Name { get; set; }
        public List<Cep> Ceps { get; set; }
    }
}
