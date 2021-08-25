using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Models;
using RecipeApp.ViewModels;
using System.Threading.Tasks;

namespace RecipeApp.Test
{
    [TestClass]
    public class SelectRecipeTest
    {
        Mock<IRecipeManager> recipeManager;
        Mock<IShellHelper> shellHelperMock;
        SelectRecipeViewModel selectRecipeVM;

        [TestInitialize]
        public void Initialize()
        {
            recipeManager = new Mock<IRecipeManager>();
            shellHelperMock = new Mock<IShellHelper>();
            selectRecipeVM = new SelectRecipeViewModel(recipeManager.Object, shellHelperMock.Object);
        }

        [TestMethod]
        public async Task LoadRecipes_ReturnsRecipes()
        {
            recipeManager.Setup(m => m.GetRecipes())
                .ReturnsAsync(new Recipe[]
                {
                    new Recipe(),
                    new Recipe()
                });

            await selectRecipeVM.LoadRecipes();
            Assert.IsTrue(selectRecipeVM.Recipes.Count == 2);
        }

        [TestMethod]
        public async Task LoadRecipes_DisplayAlert_ThrowsException()
        {
            recipeManager.Setup(m => m.GetRecipes())
                .Throws<NoInternetException>();

            await selectRecipeVM.LoadRecipes();
            shellHelperMock.Verify(m => m.DisplayAlert("No Internet Connection"), Times.Once());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        //[DataRow(2)]
        public async Task Select_ReturnsRandomRecipe(int recipeIndex)
        {
            recipeManager.Setup(m => m.GetRecipes())
                .ReturnsAsync(new Recipe[]
                {
                    new Recipe() { Name = "r1" },
                    new Recipe() { Name = "r2" }
                });

            await selectRecipeVM.LoadRecipes();
            await selectRecipeVM.Select(recipeIndex);
            Assert.IsNotNull(selectRecipeVM.Name);
        }
    }
}
