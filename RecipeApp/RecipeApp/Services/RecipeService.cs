using Newtonsoft.Json;
using RecipeApp.Models;
using RecipeApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(RecipeService))]
namespace RecipeApp.Services
{
    public class RecipeService : IRecipeService
    {
        static HttpClient httpClient;

        static void Init()
        {
            if (httpClient == null)
                httpClient = new HttpClient();
        }

        public async Task<string> AddRecipe(Recipe recipe)
        {
            Init();

            var azureFunction = $"https://recipeappfunction.azurewebsites.net/api/AddRecipe?name={recipe.Name}&ingredients={recipe.Ingredients}&directions={recipe.Directions}&type={recipe.Type}";
            string message = await httpClient.GetStringAsync(azureFunction);
            return message;
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            Init();

            var azureFunction = "https://recipeappfunction.azurewebsites.net/api/GetRecipes";
            var response = await httpClient.GetAsync(azureFunction);
            var json = response.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<ObservableCollection<Recipe>>(json);

            return list;
        }

        public async Task<Recipe> GetRecipe(string id)
        {
            Init();

            var azureFunction = "https://recipeappfunction.azurewebsites.net/api/GetRecipes";
            var response = await httpClient.GetAsync(azureFunction);
            var json = response.Content.ReadAsStringAsync().Result;
            var recipe = JsonConvert.DeserializeObject<ObservableCollection<Recipe>>(json).Where(r => r.RowKey.Equals(id));
            var selectedRecipe = recipe.FirstOrDefault();
            return selectedRecipe;
        }

        public async Task<string> DeleteRecipe(string partitionKey, string rowKey)
        {
            Init();

            var azureFunction = $"https://recipeappfunction.azurewebsites.net/api/Delete/{partitionKey}/{rowKey}";
            string message = await httpClient.GetStringAsync(azureFunction);
            return message;
        }
    }
}