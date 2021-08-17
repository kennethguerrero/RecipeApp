using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    [QueryProperty(nameof(RecipeId), nameof(RecipeId))]
    public class RecipeDetailViewModel : BaseViewModel
    {
        private readonly IRecipeManager _recipeManager;
        private readonly IShellHelper _shellHelper;

        public RecipeDetailViewModel(IRecipeManager recipeManager, IShellHelper shellHelper)
        {
            Title = "Recipe Detail";
            _recipeManager = recipeManager;
            _shellHelper = shellHelper;
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
            get => recipeId;
            set => SetProperty(ref recipeId, value, onChanged: async () => await LoadRecipeId(recipeId));
        }

        string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public string RowKey { get; set; }
        public async Task LoadRecipeId(string recipeId)
        {
            try
            {
                var recipe = await _recipeManager.GetRecipe(recipeId);
                RowKey = recipe.RowKey;
                Name = recipe.Name;
                Ingredients = recipe.Ingredients;
                Directions = recipe.Directions;
                Image = recipe.Image;
            }
            catch(NoInternetException)
            {
                await _shellHelper.DisplayAlert("No Internet Connection");
                await _shellHelper.GotoAsync($"//{nameof(RecipesPage)}");
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
