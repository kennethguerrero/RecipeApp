using RecipeApp.Views;
using Xamarin.Forms;

namespace RecipeApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public Command LoginAsGuestCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            LoginAsGuestCommand = new Command(LoginAsGuest);
        }

        private async void OnLoginClicked(object obj)
        {
            if (userName == "bb1" && password == "stickerhappy123")
            {
                obj = true;
                await Shell.Current.GoToAsync($"//{nameof(RecipesPage)}?{nameof(RecipesViewModel.IsAdmin)}={obj}");
            }
            else
            {
                await Shell.Current.DisplayAlert(null, "Incorrect username or password.", "OK");
            }
        }

        private async void LoginAsGuest(object obj)
        {
            // await Shell.Current.GoToAsync($"//{nameof(RecipesPage)}");
            obj = false;
            await Shell.Current.GoToAsync($"//{nameof(RecipesPage)}?{nameof(RecipesViewModel.IsAdmin)}={obj}");
        }

        string userName;
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
