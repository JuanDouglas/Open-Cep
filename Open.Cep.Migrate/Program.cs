using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Open.Cep.Migrate
{
    public class Program
    {
        static string path;
        static bool Reading;
        static bool Organizer;
        static bool Converting;
        static bool Saving;
        static bool Loading => !(Reading && Organizer && Converting && Saving);
        static void Main(string[] args)
        {
            Console.Write("Write the files path: ");
            path = Console.ReadLine();

            Reading = true;
            Task.Run(() =>
            {
                while (Loading)
                {
                    Console.Clear();
                    if (Reading)
                        Console.Write("Reading files");
                    if (Organizer)
                        Console.Write("Organize content");
                    if (Converting)
                        Console.Write("Converting content");
                    if (Saving)
                        Console.Write("Saving content");

                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(250);
                        Console.Write(".");
                    }
                }

            });

            Task<Models.Models.State[]> states = ReadFiles();
            states.Wait();
            Converting = true;
            Organizer = false;

            string text = PrettyJson(Newtonsoft.Json.JsonConvert.SerializeObject(states.Result));
           
            Saving = true;
            Converting = false;

            if (File.Exists(@$"{path}\output.json"))
            {
                File.Create(@$"{path}\output.json").Close();
            }

            File.WriteAllText(@$"{path}\output.json", text);

        }
        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }

        public static async Task<Models.Models.State[]> ReadFiles()
        {
            Reading = true;
            string[] files = Directory.GetFiles(@$"{path}\Ceps");
            List<Cep> ceps = new(ReadCeps(files));
            List<City> cities = new(ReadCities());
            List<State> states = new(ReadStates());
            List<Models.Models.State> modelStates = new();

            Organizer = true;
            Reading = false;
            for (int i = 0; i < cities.Count; i++)
            {
                var findResult = ceps.FindAll(find => find.CityID == cities[i].ID);
                cities[i].Ceps = new Cep[findResult.Count];


                for (int j = 0; j < findResult.Count; j++)
                {
                    cities[i].Ceps[j] = findResult[j];
                }
            }

            for (int i = 0; i < states.Count; i++)
            {
                foreach (City city in cities.FindAll(find => find.StateID == states[i].ID))
                {
                    states[i].Cities.Add(city);
                }
                modelStates.Add(states[i].ToModelState());
                states.RemoveAll(re => re.ID == states[i].ID);
            }

            return modelStates.ToArray();
        }

        public static State[] ReadStates()
        {
            List<State> states = new();
            using TextFieldParser parser = new(@$"{path}\states.csv");
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                states.Add(new(fields));
            }
            return states.ToArray();
        }
        public static City[] ReadCities()
        {
            List<City> cities = new();
            using TextFieldParser parser = new(@$"{path}\cities.csv");
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                cities.Add(new(fields));
            }

            return cities.ToArray();
        }

        public static Cep[] ReadCeps(string[] files)
        {
            List<Cep> ceps = new();
            foreach (string file in files)
            {
                try
                {
                    using TextFieldParser parser = new(file);
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    List<Cep> fileCeps = new();
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        string[] fields = parser.ReadFields();
                        fileCeps.Add(new(fields));
                    }

                    ceps.AddRange(fileCeps);
                }
                catch (Exception e)
                {

                }
            }
            return ceps.ToArray();
        }
    }
}
