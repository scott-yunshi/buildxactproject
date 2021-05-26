using buildxact_supplies.Model;
using buildxact_supplies.Utility;
using buildxact_supplies.ViewModel;
using System;
using System.Collections.Generic;

namespace SuppliesPriceLister
{
    class Program
    {
        static void Main(string[] args)
        {
            LogWriter.LogWrite("-- Process Begin -- ");

            List<Humphries> humphriesList = new List<Humphries>();
            List<Megacorp> megacorpList = new List<Megacorp>();
            List<BuildingSupply> OutputList = new List<BuildingSupply>();

            // Step1: Read file into list: humphriesList
            humphriesList = SupplyProcessor.ReadHumphriesInput();
            // Step2: Read file into list: megacorpList
            megacorpList = SupplyProcessor.ReadMegacorpInput();
            // Step3: Combine 2 list into output model and sort
            OutputList = SupplyProcessor.GenerateOutputList(humphriesList, megacorpList);
            // Step4: Print out result
            SupplyProcessor.PrintOutput(OutputList);

            LogWriter.LogWrite("-- Process End -- ");
            Console.ReadLine();
        }
    }
}
