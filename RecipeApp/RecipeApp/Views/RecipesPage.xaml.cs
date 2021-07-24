using RecipeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesPage : ContentPage
    {
        //RecipesViewModel vm;

        public RecipesPage()
        {
            InitializeComponent();
            //BindingContext = vm = new RecipesViewModel();
        }

        protected override void OnAppearing()
        {
            //base.OnAppearing();
            //vm.OnAppearing();
        }
    }
}