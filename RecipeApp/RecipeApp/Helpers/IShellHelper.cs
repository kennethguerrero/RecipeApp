using System.Threading.Tasks;

namespace RecipeApp.Helpers
{
    public interface IShellHelper
    {
        Task DisplayAlert(string message);
        Task GotoAsync(string param);
    }
}