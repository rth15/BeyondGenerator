using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Newtonsoft.Json.Serialization;
using System.IO;

namespace BeyondGenerator
{
    [Serializable]
    public class BeyondConfig
    {
        public General General { get; set; }
        public Dictionary<string,Kit> Kits { get; set; }
        public ShopItems ShopItems { get; set; }


        public BeyondConfig()
        {
            this.Kits = new Dictionary<string, Kit>();
            this.ShopItems = new ShopItems();
            this.General = new General();
        }

    }

    public class General
    {
        public TimedPointsReward TimedPointsReward { get; set; }
        public int ItemsPerPage { get; set; }
        public int ShopDisplayTime { get; set; }

        public General()
        {
            this.TimedPointsReward = new TimedPointsReward();
            this.ItemsPerPage = 0;
            this.ShopDisplayTime = 0;
        }
    }

    public class TimedPointsReward
    {
        public bool Enabled { get; set; }
        public int Interval { get; set; }
        public int Amount { get; set; }
    }

        
    public class Kit
    {
        public int DefaultAmount { get; set; }
        public int Price { get; set; }
        public List<KitItem> KitItems { get; set; }
        public List<KitDino> KitDinos { get; set; }
    }

    public class ShopItems
    {
        public Dictionary<string, Items> Items { get; set; }
        public Dictionary<string, Dino> Dinos { get; set; }

        public ShopItems()
        {
            this.Items = new Dictionary<string, Items>();
            this.Dinos = new Dictionary<string, Dino>();
        }
    }

    public class Items
    {
        public int Price { get; set; }
        public string Description { get; set; }
        public List<Item> ItemList { get; set; }
    }

    public class Item
    {
        public int Quality { get; set; }
        public string Type { get; set; }
        public bool ForceBlueprint { get; set; }
        public int Amount { get; set; }
        public string BluePrint { get; set; }
    }

    public class KitItem
    {
        public int amount { get; set; }
        public int quality { get; set; }
        public bool forceBlueprint { get; set; }
        public string blueprint { get; set; }
    }

    public class Dino
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }
        public string className { get; set; }
    }

    public class KitDino
    {
        public int Level { get; set; }
        public string className { get; set; }
    }
}

