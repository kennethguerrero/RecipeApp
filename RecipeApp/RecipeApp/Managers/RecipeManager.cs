using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Models;
using RecipeApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApp.Managers
{
    public class RecipeManager : BaseManager, IRecipeManager
    {
        private readonly IRecipeService _recipeService;

        public RecipeManager(IRecipeService recipeService, IConnectivityHelper connectivityHelper) : base(connectivityHelper)
        {
            _recipeService = recipeService;
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            if (!CheckInternetConnection())
                throw new NoInternetException();

            var result = await _recipeService.GetRecipes();
            return result;
        }

        public async Task<Recipe> GetRecipe(string id)
        {
            if (!CheckInternetConnection())
                throw new NoInternetException();

            var result = await _recipeService.GetRecipe(id);
            return result;
        }

        public async Task<string> AddRecipe(Recipe recipe)
        {
            if (!CheckInternetConnection())
                throw new NoInternetException();

            var result = await _recipeService.AddRecipe(recipe);
            return result;
        }

        public async Task<string> DeleteRecipe(string partitionKey, string rowKey)
        {
            if (!CheckInternetConnection())
                throw new NoInternetException();

            var result = await _recipeService.DeleteRecipe(partitionKey, rowKey);
            return result;
        }
    }
}
