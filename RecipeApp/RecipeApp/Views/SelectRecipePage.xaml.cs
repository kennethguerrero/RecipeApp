using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectRecipePage : ContentPage
    {
        public SelectRecipePage()
        {
            InitializeComponent();

            BindingContext = ServiceLocator.GetSelectRecipeViewModel(); 
        }
    }
}