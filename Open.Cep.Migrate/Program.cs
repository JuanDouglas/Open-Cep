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

            try
            {
                DirectoryInfo directoryInfo = new(path);
                FileInfo[] files = directoryInfo.GetFiles();

                Task<Cep[]> ceps = ReadFiles(files);
            }
            catch (Exception e)
            {
                Console.WriteLine("This path directory is invalid!");
            }



        }

        public static async Task<Cep[]> ReadFiles(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                FileStream fs = new(file.DirectoryName, FileMode.Open, FileAccess.Read);
                StreamReader streamReader = new(fs);
                string content = await streamReader.ReadToEndAsync();


                List<Cep> ceps = new();
                foreach (string item in content.Split('\n'))
                {
                    ceps.Add(new Cep(item));
                }
            }

            throw new NotImplementedException();
        }

    }


    public class Cep
    {
        public long Value { get; set; }
        public string PublicPlace { get; set; }
        public string Neighborhood { get; set; }
        public int CityID { get; set; }

        public Cep() { }
        public Cep(string content)
        {
            string[] values = content.Split(',');

            Value = long.Parse(values[0]);
            PublicPlace = values[1];
            Neighborhood = values[2];
            CityID = int.Parse(values[3]);
        }
    }
}
