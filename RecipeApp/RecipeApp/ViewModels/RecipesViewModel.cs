using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
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
        public Command RateCommand { get; }

        public Command<Recipe> DeleteCommand { get; }

        private const string pageTitle = "Recipes";
        private readonly IRecipeManager _recipeManager;
        private readonly IShellHelper _shellHelper;

        public RecipesViewModel(IRecipeManager recipeManager, IShellHelper shellHelper)
        {
            _recipeManager = recipeManager;
            _shellHelper = shellHelper;

            Title = pageTitle;
            Recipes = new ObservableCollection<Recipe>();
            LoadRecipesCommand = new Command(async () => await LoadRecipes());
            ItemTapped = new Command<Recipe>(async (recipe) => await Selected(recipe));
            DeleteCommand = new Command<Recipe>(async (recipe) => await Delete(recipe));
            AddCommand = new Command(async () => await Add());
            RateCommand = new Command(async () => await Rate());

            Initialization = LoadRecipes();
        }

        public async Task Add()
        {
            if (IsAdmin == false)
            {
                await _shellHelper.DisplayAlert("Guest account cannot add recipes");
                return;
            }

            await _shellHelper.GotoAsync(nameof(AddRecipePage));
        }

        private Task Initialization { get; set; }
        public async Task LoadRecipes()
        {
            try
            {
                IsBusy = true;
                Recipes.Clear();
                var recipes = await _recipeManager.GetRecipes();
                foreach (var recipe in recipes)
                {
                    Recipes.Add(recipe);
                }
                DependencyService.Get<IToast>()?.MakeToast("Load Complete");
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

        Recipe selectedRecipe;
        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set => SetProperty(ref selectedRecipe, value, onChanged: async () => await Selected(selectedRecipe));
        }

        public async Task Selected(Recipe recipe)
        {
            if (recipe == null)
                return;

            SelectedRecipe = null;

            await _shellHelper.GotoAsync($"{nameof(RecipeDetailPage)}?{nameof(RecipeDetailViewModel.RecipeId)}={recipe.RowKey}");
        }

        public async Task Rate()
        {
            await _shellHelper.DisplayAlert("Feature coming soon");
        }

        public async Task Delete(Recipe recipe)
        {
            try
            {
                if (recipe == null)
                {
                    return;
                }
                else if (IsAdmin == false)
                {
                    await _shellHelper.DisplayAlert("Guest account cannot delete recipes");
                    return;
                }

                await _recipeManager.DeleteRecipe(recipe.PartitionKey, recipe.RowKey);
                await _shellHelper.DisplayAlert("Recipe has been deleted");
                await LoadRecipes();
            }
            catch(NoInternetException)
            {
                await _shellHelper.DisplayAlert("No Internet Connection");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set => SetProperty(ref isAdmin, value);
        }
    }
}