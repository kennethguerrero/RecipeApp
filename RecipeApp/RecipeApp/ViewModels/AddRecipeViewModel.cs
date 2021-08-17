using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    public class AddRecipeViewModel : BaseViewModel
    {
        private string name;
        private string ingredients;
        private string directions;
        private bool isBlank;

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private readonly IRecipeManager _recipeManager;
        private readonly IShellHelper _shellHelper;

        public AddRecipeViewModel(IRecipeManager recipeManager, IShellHelper shellHelper)
        {
            Title = "Add Recipe";
            CancelCommand = new Command(async () => await Cancel());
            SaveCommand = new Command(async () => await Save());
            _recipeManager = recipeManager;
            _shellHelper = shellHelper;
        }

        public async Task Save()
        {
            try
            {
                Recipe newRecipe = new Recipe
                {
                    Name = Name,
                    Ingredients = Ingredients,
                    Directions = Directions,
                    Type = SelectedRecipeType.Value
                };
                string message = await _recipeManager.AddRecipe(newRecipe);
                await _shellHelper.DisplayAlert($"{message}");
                await _shellHelper.GotoAsync("..");
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

        public void ValidateSave()
        {
            if (!string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(ingredients)
                && !string.IsNullOrWhiteSpace(directions))
            {
                IsBlank = false;
            }
            else
            {
                IsBlank = true;
            }
        }

        public bool IsBlank
        {
            get => isBlank;
            set => SetProperty(ref isBlank, value);
        }

        public async Task Cancel()
        {
            await _shellHelper.GotoAsync("..");
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Ingredients
        {
            get => ingredients;
            set => SetProperty(ref ingredients, value);
        }

        public string Directions
        {
            get => directions;
            set => SetProperty(ref directions, value);
        }

        public IList<RecipeType> RecipeTypes { get { return RecipeTypeData.RecipeTypes; } }
        RecipeType selectedRecipeType;
        public RecipeType SelectedRecipeType
        {
            get => selectedRecipeType;
            set => SetProperty(ref selectedRecipeType, value);
        }
    }
}
