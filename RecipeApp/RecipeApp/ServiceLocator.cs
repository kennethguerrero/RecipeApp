using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Services;
using RecipeApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeApp
{
    public class ServiceLocator
    {
        public static readonly IShellHelper ShellHelper = new ShellHelper();
        private static readonly IConnectivityHelper ConnectivitHelper = new ConnectivityHelper();
        private static readonly IRecipeService RecipeService = new RecipeService();
        private static readonly IRecipeManager RecipeManager = new RecipeManager(RecipeService, ConnectivitHelper);

        public static RecipesViewModel GetRecipesViewModel()
        {
            return new RecipesViewModel(RecipeManager, ShellHelper);
        }

        public static RecipeDetailViewModel GetRecipeDetailViewModel()
        {
            return new RecipeDetailViewModel(RecipeManager, ShellHelper);
        }

        public static SelectRecipeViewModel GetSelectRecipeViewModel()
        {
            return new SelectRecipeViewModel(RecipeManager, ShellHelper);
        }

        public static AddRecipeViewModel GetAddRecipeViewModel()
        {
            return new AddRecipeViewModel(RecipeManager, ShellHelper);
        }
    }
}
