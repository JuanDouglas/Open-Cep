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
        static LoadingState LoadingState;
        const string delimiter = ",";
        static bool Loading => LoadingState != LoadingState.None;
        static void Main(string[] args)
        {
            Console.Write("Write the files path: ");
            path = Console.ReadLine();

            LoadingState = LoadingState.Reading;

            #region LoadingTask
            Task loading = Task.Run(
            #region Loading Method 
                () => {

                while (Loading)
                {
                    Console.Clear();
                    string text = string.Empty;


                    switch (LoadingState)
                    {
                        case LoadingState.Reading:
                            text = "Reading file";
                            break;
                        case LoadingState.Converting:
                            text = "Converting Obects";
                            break;
                        case LoadingState.Saving:
                            text = "Saving in file";
                            break;
                        case LoadingState.Organizing:
                            text = "Organizing objects in memory";
                            break;
                        default:
                            break;
                    }

                    Console.WriteLine(text);
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(250);
                        Console.Write(".");
                    }
                }
            }
            #endregion 
                );
            if (loading.Status == TaskStatus.Created)
                loading.Start();
            #endregion

            Task<Models.Models.State[]> states = Task.Run(() => { return ReadFiles(); });
            states.Wait();

            LoadingState = LoadingState.Converting;
            string text = PrettyJson(Newtonsoft.Json.JsonConvert.SerializeObject(states.Result));

            LoadingState = LoadingState.Saving;
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

        public static Models.Models.State[] ReadFiles()
        {
            LoadingState = LoadingState.Reading;
            string[] files = Directory.GetFiles(@$"{path}\Ceps");
            List<Models.Models.Cep> ceps = new(ReadCeps(files));
            List<City> cities = new(ReadCities());
            List<State> states = new(ReadStates());
            List<Models.Models.State> modelStates = new();

            LoadingState = LoadingState.Organizing;
            for (int i = 0; i < cities.Count; i++)
            {
                var findResult = ceps.FindAll(find => find.CityID == cities[i].ID);
                cities[i].Ceps = new Models.Models.Cep[findResult.Count];


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
            parser.SetDelimiters(delimiter);

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
            parser.SetDelimiters(delimiter);

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                cities.Add(new(fields));
            }

            return cities.ToArray();
        }

        public static Models.Models.Cep[] ReadCeps(string[] files)
        {
            List<Models.Models.Cep> ceps = new();
            foreach (string file in files)
            {
                try
                {
                    using TextFieldParser parser = new(file);
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(delimiter);

                    List<Models.Models.Cep> fileCeps = new();
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


    public enum LoadingState : short
    {
        Reading,
        Converting,
        Saving,
        Organizing,
        None = 0
    }
}
