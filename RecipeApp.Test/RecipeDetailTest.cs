using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RecipeApp.Exceptions;
using RecipeApp.Helpers;
using RecipeApp.Managers;
using RecipeApp.Models;
using RecipeApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Test
{
    [TestClass]
    public class RecipeDetailTest
    {
        Mock<IRecipeManager> recipeManager;
        Mock<IShellHelper> shellHelperMock;
        RecipeDetailViewModel recipeDetailVM;

        [TestInitialize]
        public void Initialize()
        {
            recipeManager = new Mock<IRecipeManager>();
            shellHelperMock = new Mock<IShellHelper>();
            recipeDetailVM = new RecipeDetailViewModel(recipeManager.Object, shellHelperMock.Object);
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
