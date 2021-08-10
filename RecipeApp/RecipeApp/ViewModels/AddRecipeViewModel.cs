using RecipeApp.Models;
using RecipeApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    public class AddRecipeViewModel : BaseViewModel
    {
        private string name;
        private string ingredients;
        private string directions;

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        IRecipeService recipeService;

        public AddRecipeViewModel()
        {
            Title = "Add Recipe";
            CancelCommand = new Command(Cancel);
            SaveCommand = new Command(Save, ValidateSave);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            recipeService = DependencyService.Get<IRecipeService>();
        }

        private async void Save()
        {
            Recipe newRecipe = new Recipe
            {
                Name = Name,
                Ingredients = Ingredients,
                Directions = Directions,
                Type = SelectedRecipeType.Value
            };
            string message = await recipeService.AddRecipe(newRecipe);
            await Shell.Current.DisplayAlert("Success!", message, "OK");
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(ingredients)
                && !string.IsNullOrWhiteSpace(directions);
        }

        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
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
