using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Open.Cep.Models.Models
{
    [Serializable]
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        [JsonIgnore]
        public Cep[] Ceps { get; set; }
    }
}
