using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Open.Cep.Migrate
{
    public class Program
    {
        static string path;
        static void Main(string[] args)
        {
            Console.Write("Write the files path: ");
            path = Console.ReadLine();


            Task ceps = ReadFiles();


        }

        public static async Task<Cep[]> ReadFiles()
        {
            string[] files = Directory.GetFiles(@$"{path}\Ceps");
            List<Cep> ceps = new(await ReadCepsAsync(files));
            List<City> cities = new(ReadCities());
            List<State> states = new(ReadStates());

            foreach (City city in cities)
            {
                foreach (Cep cep in ceps)
                {
                    if (cep.CityID == city.ID)
                    {
                        city.Ceps.Add(cep);
                    }
                }
            }

            throw new NotImplementedException();
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

        public static async Task<Cep[]> ReadCepsAsync(string[] files)
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
