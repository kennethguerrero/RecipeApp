using RecipeApp.Models;
using RecipeApp.Services;
using RecipeApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    [QueryProperty(nameof(IsAdmin), nameof(IsAdmin))]
    public class RecipesViewModel : BaseViewModel
    {
        public ObservableCollection<Recipe> Recipes { get; }
        public Command LoadRecipesCommand { get; }
        public Command AddCommand { get; }
        public Command<Recipe> ItemTapped { get; }
        public Command<Recipe> RateCommand { get; }

        public Command<Recipe> DeleteCommand { get; }

        IRecipeService recipeService;

        public RecipesViewModel()
        {
            Title = "Recipes";

            Recipes = new ObservableCollection<Recipe>();

            LoadRecipesCommand = new Command(async () => await LoadRecipes());
            AddCommand = new Command(Add);
            ItemTapped = new Command<Recipe>(Selected);
            RateCommand = new Command<Recipe>(Rate);
            DeleteCommand = new Command<Recipe>(Delete);

            recipeService = DependencyService.Get<IRecipeService>();

            Initialization = LoadRecipes();
        }

        private async void Add(object obj)
        {
            if (IsAdmin == false)
            {
                await Shell.Current.DisplayAlert("Error", "Guest account cannot add recipes", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(AddRecipePage));
        }

        private Task Initialization { get; set; }
        public async Task LoadRecipes()
        {
            IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }

            DependencyService.Get<IToast>()?.MakeToast("All Recipes Loaded");
        }

        Recipe previouslySelected;
        Recipe selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                if (value != null)
                {
                    Selected(value);
                    previouslySelected = value;
                    value = null;
                }
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        async void Selected(Recipe recipe)
        {
            if (recipe == null)
                return;

            //await Shell.Current.DisplayAlert("Selected", recipe.RowKey, "OK");
            await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}?{nameof(RecipeDetailViewModel.RecipeId)}={recipe.RowKey}");
        }

        async void Rate(Recipe recipe)
        {
           await Shell.Current.DisplayAlert("Rate", "Feature coming soon.", "OK");
        }

        bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set => SetProperty(ref isAdmin, value);
        }

        async void Delete(Recipe recipe)
        {
            if (recipe == null)
                return;

            var result = await Shell.Current.DisplayAlert("Delete Recipe", "Are you sure you want to delete?", "OK", "Cancel");

            if (result)
            {
                await recipeService.DeleteRecipe(recipe.PartitionKey, recipe.RowKey);
                await LoadRecipes();
            }
        }
    }
}