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
            SelectRecipeCommand = new Command(async () => await GetRandomNumber());

            _recipeManager = recipeManager;
            _shellHelper = shellHelper;

            LoadRecipesCommand = new Command(async () => await LoadRecipes());
            Initialization = LoadRecipes();
        }

        public async Task Select(int recipeIndex)
        {
            try
            {
                var recipe = Recipes[recipeIndex];
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

        int recipeIndex;
        private async Task GetRandomNumber()
        {
            IsBusy = true;
            Random random = new Random();

            await Task.Delay(2000);

            recipeIndex = random.Next(Recipes.Count);
            if (recipeIndex == -1)
                return;

            await Select(recipeIndex);
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
