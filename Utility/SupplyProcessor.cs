using buildxact_supplies.Model;
using buildxact_supplies.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace buildxact_supplies.Utility
{
    public static class SupplyProcessor
    {
        private static decimal AudUsdExchangeRate;
        private static string Supply_humphries;
        private static string Supply_megacorp;

        //SupplyProcessor Constructor
        static SupplyProcessor()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            AudUsdExchangeRate = Decimal.Parse(config["audUsdExchangeRate"]);
            Supply_humphries = config["humphries_input"];
            Supply_megacorp = config["megacorp_input"];
        }

        //Read supply information from Humphries file into List
        public static List<Humphries> ReadHumphriesInput()
        {
            List<Humphries> output = new List<Humphries>();
            using (FileStream fs = File.Open(Supply_humphries, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                sr.ReadLine(); // Skip header line
                string lineRead;

                while ((lineRead = sr.ReadLine()) != null)
                {
                    var inputLine = lineRead.Split(',');
                    if (inputLine.Length == 4)
                    {
                        Humphries record = new Humphries();
                        try
                        {
                            record.Identifier = Guid.Parse(inputLine[0]);
                            record.Description = inputLine[1];
                            record.Unit = inputLine[2];
                            record.CostAUD = Decimal.Parse(inputLine[3]);

                            output.Add(record);
                        }
                        catch (Exception exe)
                        {
                            LogWriter.LogWrite("Error Information: " + exe.Message + ", " + exe.StackTrace);
                        }
                    }
                    else
                    {
                        LogWriter.LogWrite("Error Information: Invalid line: " + lineRead);
                    }
                }
            }

            LogWriter.LogWrite(output.Count + " lines from Humphries added. ");
            return output;
        }

        //Read supply information from Megacorp file into List
        public static List<Megacorp> ReadMegacorpInput()
        {
            List<Megacorp> output = new List<Megacorp>();
            using (FileStream fs = File.Open(Supply_megacorp, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {

                var serializer = new JsonSerializer();
                try
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            output = serializer.Deserialize<JsonObject>(reader).Partners;
                        }
                    }
                }
                catch (Exception exe)
                {
                    LogWriter.LogWrite("Error Information: " + exe.Message + ", " + exe.StackTrace);
                }
            }

            LogWriter.LogWrite(output.Count + " lines from Megacorp added. ");
            return output;
        }

        //Generate output list and sorting
        public static List<BuildingSupply> GenerateOutputList(List<Humphries> humphriesList, List<Megacorp> megacorpList)
        {
            List<BuildingSupply> output = new List<BuildingSupply>();

            //Add humphriesList into final output list
            foreach (Humphries h in humphriesList)
            {
                var supply = new BuildingSupply
                {
                    ID = h.Identifier.ToString(),
                    Item = h.Description,
                    Price = h.CostAUD
                };

                output.Add(supply);
            }
            //Add megacorpList into final output list
            foreach (Megacorp m in megacorpList)
            {
                foreach (MegacorpSupply ms in m.Supplies)
                {
                    var supply = new BuildingSupply
                    {
                        ID = ms.ID.ToString(),
                        Item = ms.Description,
                        Price = ((decimal)ms.PriceInCents / 100M) * AudUsdExchangeRate
                    };

                    output.Add(supply);
                }
            }

            LogWriter.LogWrite(output.Count + " lines added to final list. ");
            // Sort and return output list
            return output.OrderByDescending(o => o.Price).ToList();
        }

        //Print final result from List
        public static void PrintOutput(List<BuildingSupply> buildingSupply)
        {
            foreach (BuildingSupply bs in buildingSupply)
            {
                Console.WriteLine(bs.ID + ", " + bs.Item + ", $" + bs.Price);
                Console.WriteLine();
            }
        }
    }
}
