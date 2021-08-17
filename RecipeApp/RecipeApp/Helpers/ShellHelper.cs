using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeApp.Helpers
{
    public class ShellHelper : IShellHelper
    {
        public async Task GotoAsync(string param)
        {
            await Shell.Current.GoToAsync(param);
        }

        public async Task DisplayAlert(string message)
        {
            await Shell.Current.DisplayAlert(null, message, "Close");
        }
    }
}
