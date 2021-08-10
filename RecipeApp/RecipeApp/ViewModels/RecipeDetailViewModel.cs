using RecipeApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    [QueryProperty(nameof(RecipeId), nameof(RecipeId))]
    public class RecipeDetailViewModel : BaseViewModel
    {
        IRecipeService recipeService;
        public RecipeDetailViewModel()
        {
            Title = "Recipe Detail";
            recipeService = DependencyService.Get<IRecipeService>();
        }

        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string ingredients;
        public string Ingredients
        {
            get => ingredients;
            set => SetProperty(ref ingredients, value);
        }

        string directions;
        public string Directions
        {
            get => directions;
            set => SetProperty(ref directions, value);
        }

        string recipeId;
        public string RecipeId
        {
            get { return recipeId; }
            set
            {
                recipeId = value;
                LoadRecipeId(value);
            }
        }

        string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public string RowKey { get; set; }
        public async void LoadRecipeId(string recipeId)
        {
            try
            {
                var recipe = await recipeService.GetRecipe(recipeId);
                RowKey = recipe.RowKey;
                Name = recipe.Name;
                Ingredients = recipe.Ingredients;
                Directions = recipe.Directions;
                Image = recipe.Image;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
