using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Models;
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

        public Command LoadRecipesCommand { get; }

        public Command SelectRecipeCommand { get; }

        private readonly IRecipeManager _recipeManager;
        private readonly IShellHelper _shellHelper;

        public SelectRecipeViewModel(IRecipeManager recipeManager, IShellHelper shellHelper)
        {
            Title = "Select Recipe";
            Recipes = new ObservableCollection<Recipe>();
            SelectRecipeCommand = new Command(async () => await Select());

            _recipeManager = recipeManager;
            _shellHelper = shellHelper;

            LoadRecipesCommand = new Command(async () => await LoadRecipes());
            Initialization = LoadRecipes();
        }

        public async Task Select()
        {
            try
            {
                IsBusy = true;
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
            catch (NoInternetException)
            {
                await _shellHelper.DisplayAlert("No Internet Connection");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task Initialization { get; set; }
        public async Task LoadRecipes()
        {
            try
            {
                Recipes.Clear();
                var recipes = await _recipeManager.GetRecipes();
                foreach (var recipe in recipes)
                {
                    Recipes.Add(recipe);
                }
            }
            catch (NoInternetException)
            {
                await _shellHelper.DisplayAlert("No Internet Connection");
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
