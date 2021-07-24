using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeApp.Models
{
    public class RecipeTypeData
    {
        public static IList<RecipeType> RecipeTypes { get; set; }

        static RecipeTypeData()
        {
            RecipeTypes = new List<RecipeType>
            {
                new RecipeType
                {
                    Id = 1,
                    Value = "Breakfast"
                },
                new RecipeType
                {
                    Id = 2,
                    Value = "Lunch"
                },
                new RecipeType
                {
                    Id = 3,
                    Value = "Dinner"
                }
            };
        }
    }
}
