using System;
using System.Collections.Generic;

namespace buildxact_supplies.Model
{
    public class JsonObject
    {
        public List<Megacorp> Partners { get; set; }
    }

    public class Megacorp
    {
        public string Name { get; set; }
        public string PartnerType { get; set; }
        public string PartnerAddress { get; set; }
        public List<MegacorpSupply> Supplies { get; set; }
    }

    public class MegacorpSupply
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public int PriceInCents { get; set; }
        public Guid ProviderId { get; set; }
        public string MaterialType { get; set; }
    }
}
