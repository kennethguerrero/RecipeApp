using RecipeApp.Models;
using RecipeApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    public class SelectRecipeViewModel : BaseViewModel
    {
        public ObservableCollection<Recipe> Recipes { get; }

        public Command SelectRecipeCommand { get; }

        IRecipeService recipeService;
        public SelectRecipeViewModel()
        {
            Title = "Select Recipe";
            Recipes = new ObservableCollection<Recipe>();
            SelectRecipeCommand = new Command(Select);
            recipeService = DependencyService.Get<IRecipeService>();

            Initialization = LoadRecipes();
        }

        async void Select()
        {
            IsBusy = true;
            try
            {
                Random random = new Random();

                await Task.Delay(2000);

                int recipesIndex = random.Next(Recipes.Count);
                if (recipesIndex == -1)
                    return;

                var recipe = Recipes[recipesIndex];
                Name = recipe.Name;
                Ingredients = recipe.Ingredients;
                Directions = recipe.Directions;
                Image = recipe.Image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task Initialization { get; set; }
        async Task LoadRecipes()
        {
            try
            {
                Recipes.Clear();
                var recipes = await recipeService.GetRecipes();
                foreach (var recipe in recipes)
                {
                    Recipes.Add(recipe);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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

        string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
    }
}
