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
    public class RecipeListTest
    {
        Mock<IRecipeManager> recipeManager;
        Mock<IShellHelper> shellHelperMock;
        RecipesViewModel recipeListVM;
        RecipeDetailViewModel recipeDetailVM;
        [TestInitialize]
        public void Initialize()
        {
            recipeManager = new Mock<IRecipeManager>();
            shellHelperMock = new Mock<IShellHelper>();
            recipeListVM = new RecipesViewModel(recipeManager.Object, shellHelperMock.Object);
            recipeDetailVM = new RecipeDetailViewModel(recipeManager.Object, shellHelperMock.Object);
        }

        [TestMethod]
        public async Task LoadRecipes_ReturnsRecipes()
        {
            recipeManager.Setup(m => m.GetRecipes())
                .ReturnsAsync(new Recipe[]
                {
                    new Recipe(),
                    new Recipe(),
                    new Recipe()
                });

            await recipeListVM.LoadRecipes();
            Assert.IsTrue(recipeListVM.Recipes.Count == 3);
        }

        [TestMethod]
        public async Task LoadRecipes_DisplayAlert_ThrowsException()
        {
            recipeManager.Setup(m => m.GetRecipes())
                .Throws<NoInternetException>();

            await recipeListVM.LoadRecipes();
            shellHelperMock.Verify(m => m.DisplayAlert("No Internet Connection"), Times.Once());
        }

        [TestMethod]
        [DataRow("r1")]
        [DataRow("10000")]
        public async Task Selected_RecipeHasValue_GotoAsyncCalled(string rowKey)
        {
            await recipeListVM.Selected(new Recipe { RowKey = rowKey });
            shellHelperMock.Verify(m => m.GotoAsync(It.Is<string>(x => x.Contains(rowKey))), Times.Once());
        }

        [TestMethod]
        public async Task Selected_RecipeIsNull_GotoAsyncNotCalled()
        {
            await recipeListVM.Selected(null);
            shellHelperMock.Verify(m => m.GotoAsync(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task Delete_Success_ReturnsSpecificDisplayAlert()
        {
            var recipe = new Recipe()
            {
                RowKey = "r1",
                PartitionKey = "p1"
            };

            recipeManager.Setup(m => m.DeleteRecipe(recipe.PartitionKey, recipe.RowKey))
                .ReturnsAsync(It.IsAny<string>());

            await recipeListVM.Delete(recipe);
            shellHelperMock.Verify(m => m.DisplayAlert("Recipe has been deleted"), Times.Once());
        }

        [TestMethod]
        public async Task Delete_RecipeIsNull_DeleteRecipeMethodNotCalled()
        {
            await recipeListVM.Delete(null);
            recipeManager.Verify(m => m.DeleteRecipe(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task Delete_DisplayAlert_ThrowsException()
        {
            var recipe = new Recipe()
            {
                RowKey = "r1",
                PartitionKey = "p1"
            };

            recipeManager.Setup(m => m.DeleteRecipe(recipe.PartitionKey, recipe.RowKey))
                .Throws<NoInternetException>();

            await recipeListVM.Delete(recipe);
            shellHelperMock.Verify(m => m.DisplayAlert("No Internet Connection"), Times.Once());
        }

        [TestMethod]
        public async Task LoadRecipeId_ReturnsRecipe()
        {
            recipeManager.Setup(m => m.GetRecipe("r1"))
                .ReturnsAsync(new Recipe()
                {
                    RowKey = "r1",
                    Name = "test recipe"
                });

            await recipeDetailVM.LoadRecipeId("r1");
            Assert.IsTrue(recipeDetailVM.Name.Equals("test recipe"));
        }

        [TestMethod]
        public async Task LoadRecipeId_DisplayAlert_ThrowsException()
        {
            recipeManager.Setup(m => m.GetRecipe(It.IsAny<string>()))
                 .Throws<NoInternetException>();

            await recipeDetailVM.LoadRecipeId(It.IsAny<string>());
            shellHelperMock.Verify(m => m.DisplayAlert(It.IsAny<string>()), Times.Once());
        }
    }
}
