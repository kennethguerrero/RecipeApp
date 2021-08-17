using RecipeApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApp.Managers
{
    public interface IRecipeManager
    {
        Task<string> AddRecipe(Recipe recipe);
        Task<string> DeleteRecipe(string partitionKey, string rowKey);
        Task<Recipe> GetRecipe(string id);
        Task<IEnumerable<Recipe>> GetRecipes();
    }
}