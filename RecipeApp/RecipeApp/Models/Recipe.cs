using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeApp.Models
{
    public class Recipe
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Directions { get; set; }
        public string Type { get; set; }

        public string Image
        {
            get
            {
                switch (Name)
                {
                    case "Adobo":
                        return "https://btngn.com/Products-Beans.JPG";
                    case "Spaghetti":
                        return "https://btngn.com/Products-Ground.JPG";
                    default:
                        return "https://btngn.com/Products-Tablea.JPG";
                }
            }
        }
    }
}
